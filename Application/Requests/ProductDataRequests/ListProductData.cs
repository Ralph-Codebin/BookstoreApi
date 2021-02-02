using Application.Repositories;
using Domain.Model.Entities;
using MediatR;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.ProductDataRequests
{
    public class ListProductData : IRequest<DReturnObject> { }

    public class ListProductDataHandler : IRequestHandler<ListProductData, DReturnObject>
    {
        private readonly IProductDataRepository _busRepo;
        private readonly IPrincipal _user;
        public ListProductDataHandler(
            IProductDataRepository busRepo,
            IPrincipal user)
        {
            _user = user;
            _busRepo = busRepo;
        }

        public async Task<DReturnObject> Handle(ListProductData request, CancellationToken cancellationToken)
        {
            return await _busRepo.ListAll();
        }
    }
}
