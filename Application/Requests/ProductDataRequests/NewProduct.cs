
using Application.Repositories;
using Domain.Model.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.ProductDataRequests
{
    public class NewProduct : IRequest<DReturnObject> {
        public NewProduct(DNewProductRequest newproduct)
        {
            Newproduct = newproduct;
        }
        public DNewProductRequest Newproduct { get; set; }

    }

    public class NewDynamicFieldHandler : IRequestHandler<NewProduct, DReturnObject>
    {
        private readonly IProductDataRepository _posRepo;
        private readonly IValidator<DNewProductRequest> _validator;

        public NewDynamicFieldHandler(
            IProductDataRepository busRepo,
            IValidator<DNewProductRequest> validator)
        {
            _posRepo = busRepo;
            _validator = validator;
        }

        public async Task<DReturnObject> Handle(NewProduct request, CancellationToken cancellationToken)
        {
            return await _posRepo.NewProduct(request.Newproduct, cancellationToken);
        }
    }
}
