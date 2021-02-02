using Application.Repositories;
using bookstore_api.Requests;
using Domain.Model.Entities;
using MediatR;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.ProductDataRequests
{
    public class NewSubscriptionData : IRequest<DReturnObject>
    {
        public NewSubscriptionData(DNewSubscriptiontRequest data)
        {
            Data = data;
        }
        public DNewSubscriptiontRequest Data { get; set; }

    }


    public class NewSubscriptionDataDataHandler : IRequestHandler<NewSubscriptionData, DReturnObject>
    {
        private readonly ISubscriptionDataRepository _busRepo;
        private readonly IPrincipal _user;
        public NewSubscriptionDataDataHandler(
            ISubscriptionDataRepository busRepo,
            IPrincipal user)
        {
            _user = user;
            _busRepo = busRepo;
        }

        public async Task<DReturnObject> Handle(NewSubscriptionData request, CancellationToken cancellationToken)
        {
            return await _busRepo.NewSubscription(request.Data, cancellationToken);
        }
    }
}
