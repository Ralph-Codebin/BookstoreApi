
using Application.ErrorHandlers;
using Application.Repositories;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using bookstore_api.Requests;
using Domain.Model.Entities;
using Domain.Rules;
using FluentValidation;
using Repository.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.EntityFramework.Repositories
{
    public class SubscriptionDataRepository : ISubscriptionDataRepository

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DNewSubscriptiontRequest> _validator;
        private readonly IValidator<DUpdateSubscriptiontRequest> _updatevalidator;
        private readonly IRepositoryExceptionHandler _exceptionHandler;

        public SubscriptionDataRepository(
            IUnitOfWork<DepContext> unitOfWork,
            IValidator<DNewSubscriptiontRequest> validator,
            IValidator<DUpdateSubscriptiontRequest> updatevalidator,
            IRepositoryExceptionHandler exceptionHandler)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _updatevalidator = updatevalidator;
            _exceptionHandler = exceptionHandler;
        }

        public async Task<DReturnObject> ListAll(string useremail, CancellationToken cancellationToken)
        {            
            DReturnObject bdr = new DReturnObject();
            try
            {
                var busRepo = _unitOfWork.GetRepository<UserData>();
                UserData ud = (await busRepo.GetFirstOrDefaultAsync(
                    predicate: a => a.email.ToLower().Equals(useremail),
                   disableTracking: false));

                if (ud == null) {
                    bdr.status = status.success.ToString();
                    bdr.message = "No subscription data for this user: "+ useremail;
                    return bdr;
                }

                var subRepo = _unitOfWork.GetRepository<DSubscriptionData>();
                bdr.data = subRepo.FromSql("select s.id, s.price, s.state, pd.imagePath, pd.title, pd.description from SubscriptionData s inner join ProductData pd on pd.id = s.prodid where s.usrid={0} and s.state=1", ud.id);

                bdr.status = status.success.ToString();
            }
            catch (Exception e)
            {
                bdr = _exceptionHandler.HandleException(bdr, e);
            }
            return bdr;
        }


        public async Task<DReturnObject> NewSubscription(DNewSubscriptiontRequest data, CancellationToken cancellationToken)
        {
            DReturnObject ro = new DReturnObject();

            var validationResult = await _validator.ValidateAsync(data, cancellationToken, ruleSet: RuleSets.ExecuteUpdateRules);
            if (!validationResult.IsValid)
            {
                ro.status = status.invalidinput.ToString();
                ro.message = validationResult.ToString();
                return ro;
            }

            List<object> parameterList = new List<object>();            
            parameterList.Add(data.prodid);
            parameterList.Add(data.email);
            parameterList.Add(data.name);
            parameterList.Add(data.lasename);
            parameterList.Add(0);

            int dd =_unitOfWork.ExecuteSqlCommand("exec SP_NewSubscription @prodid=@P0, @email=@P1, @name=@P2, @lastname=@P3, @newid=@P4", parameterList.ToArray());
            
            ro.status = status.success.ToString();

            return ro;
        }

        public async Task<DReturnObject> UpdateSubscription(DUpdateSubscriptiontRequest data, CancellationToken cancellationToken)
        {
            DReturnObject ro = new DReturnObject();

            var validationResult = await _updatevalidator.ValidateAsync(data, cancellationToken, ruleSet: RuleSets.ExecuteUpdateRules);
            if (!validationResult.IsValid)
            {
                ro.status = status.error.ToString();
                ro.message = validationResult.ToString();
                return ro;
            }

            List<object> parameterList = new List<object>();
            parameterList.Add(data.id);
            parameterList.Add(data.state);

            _unitOfWork.ExecuteSqlCommand($"update [dbo].[SubscriptionData] set " +
                $"[state] = @P1 " +
                $"WHERE id = @P0", parameterList.ToArray());

            ro.status = status.success.ToString();            

            return ro;
        }
    }
}

