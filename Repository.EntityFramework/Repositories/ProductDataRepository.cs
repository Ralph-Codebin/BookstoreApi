
using Application.ErrorHandlers;
using Application.Repositories;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain.Model.Entities;
using Domain.Rules;
using FluentValidation;
using Repository.EntityFramework.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.EntityFramework.Repositories
{
    public class ProductDataRepository : IProductDataRepository

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DNewProductRequest> _validator;
        private readonly IValidator<DUpdateProductDataRequest> _updatevalidator;
        private readonly IRepositoryExceptionHandler _exceptionHandler;

        public ProductDataRepository(
            IUnitOfWork<DepContext> unitOfWork,
            IValidator<DNewProductRequest> validator,
            IValidator<DUpdateProductDataRequest> updatevalidator,
            IRepositoryExceptionHandler exceptionHandler)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _updatevalidator = updatevalidator;
            _exceptionHandler = exceptionHandler;
        }

        public async Task<DReturnObject> ListAll()
        {            
            DReturnObject bdr = new DReturnObject();
            try
            {
                var busRepo = _unitOfWork.GetRepository<DProductData>();
                bdr.data = busRepo.FromSql("select * from ProductData");
                bdr.status = status.success.ToString();
            }
            catch (Exception e)
            {
                bdr = _exceptionHandler.HandleException(bdr, e);
            }
            return bdr;
        }

        public async Task<DReturnObject> NewProduct(DNewProductRequest newproduct, CancellationToken cancellationToken)
        {
            DReturnObject ro = new DReturnObject();

            var validationResult = await _validator.ValidateAsync(newproduct, cancellationToken, ruleSet: RuleSets.ExecuteUpdateRules);
            if (!validationResult.IsValid)
            {
                ro.status = status.invalidinput.ToString();
                ro.message = validationResult.ToString();
                return ro;
            }

            var proRepo = _unitOfWork.GetRepository<DProductData>();
            DProductData test = (await proRepo.GetFirstOrDefaultAsync(
                predicate: a => a.title.ToLower().Equals(newproduct.title.ToLower()),
               disableTracking: false));

            if (test != null)
            {
                ro.status = status.error.ToString();
                ro.message = $"The item: {newproduct.title} already exists with id {test.id}";
                return ro;
            }

            var itemToAdd = new DProductData
            {
                title = newproduct.title,
                description = newproduct.description,
                imagePath = newproduct.imagePath,
                price = newproduct.price
            };

            _unitOfWork.GetRepository<DProductData>().Insert(itemToAdd);
            await _unitOfWork.SaveChangesAsync();

            ro.data = itemToAdd.id;
            ro.status = status.success.ToString();

            return ro;
        }

        public async Task<DReturnObject> UpdateProduct(DUpdateProductDataRequest updateproduct, CancellationToken cancellationToken)
        {
            DReturnObject ro = new DReturnObject();

            var prodRepo = await _unitOfWork.GetRepository<DProductData>().GetFirstOrDefaultAsync(
                predicate: a => a.id.Equals(updateproduct.id));

            if (prodRepo is null)
            {
                ro.status = status.error.ToString();
                ro.message = $"Product with id: {updateproduct.id.ToString()} was not found.";
                return ro;
            }

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<DUpdateProductDataRequest, DProductData>();
            });
            var mapper = new Mapper(config);

            mapper.Map(updateproduct, prodRepo);

            var validationResult = await _updatevalidator.ValidateAsync(updateproduct, cancellationToken, ruleSet: RuleSets.ExecuteUpdateRules);
            if (!validationResult.IsValid)
            {
                ro.status = status.error.ToString();
                ro.message = validationResult.ToString();
                // return ro;
            }
            else
            {
                var repo = _unitOfWork.GetRepository<DProductData>();
                repo.Update(prodRepo);
                await _unitOfWork.SaveChangesAsync();
                ro.data = updateproduct;
                ro.status = status.success.ToString();
            }

            return ro;
        }
    }
}

