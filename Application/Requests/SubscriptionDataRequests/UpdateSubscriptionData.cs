using Application.Repositories;
using bookstore_api.Requests;
using Domain.Model.Entities;
using MediatR;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.ProductDataRequests
{
    public class UpdateSubscriptionData : IRequest<DReturnObject>
    {
        public UpdateSubscriptionData(DUpdateSubscriptiontRequest data)
        {
            Data = data;
        }
        public DUpdateSubscriptiontRequest Data { get; set; }

    }


    public class UpdateSubscriptionDataHandler : IRequestHandler<UpdateSubscriptionData, DReturnObject>
    {
        private readonly ISubscriptionDataRepository _busRepo;
        private readonly IPrincipal _user;
        public UpdateSubscriptionDataHandler(
            ISubscriptionDataRepository busRepo,
            IPrincipal user)
        {
            _user = user;
            _busRepo = busRepo;
        }

        public async Task<DReturnObject> Handle(UpdateSubscriptionData request, CancellationToken cancellationToken)
        {
            return await _busRepo.UpdateSubscription(request.Data, cancellationToken);
        }
    }
}
