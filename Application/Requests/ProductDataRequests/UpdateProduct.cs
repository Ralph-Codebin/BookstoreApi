
using Application.Repositories;
using Domain.Model.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.ProductDataRequests
{
    public class UpdateProduct : IRequest<DReturnObject> {
        public UpdateProduct(DUpdateProductDataRequest prd)
        {
            Prd = prd;
        }
        public DUpdateProductDataRequest Prd { get; set; }

    }

    public class UpdateProductHandler : IRequestHandler<UpdateProduct, DReturnObject>
    {
        private readonly IProductDataRepository _posRepo;
        private readonly IValidator<DUpdateProductDataRequest> _validator;

        public UpdateProductHandler(
            IProductDataRepository busRepo,
            IValidator<DUpdateProductDataRequest> validator)
        {
            _posRepo = busRepo;
            _validator = validator;
        }

        public async Task<DReturnObject> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            return await _posRepo.UpdateProduct(request.Prd, cancellationToken);
        }
    }
}
