using Application.Repositories;
using Domain.Model.Entities;
using MediatR;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.ProductDataRequests
{
    public class ListSubscriptionData : IRequest<DReturnObject>
    {
        public ListSubscriptionData(string useremail)
        {
            Email = useremail;
        }
        public string Email { get; set; }

    }


    public class ListSubscriptionDataHandler : IRequestHandler<ListSubscriptionData, DReturnObject>
    {
        private readonly ISubscriptionDataRepository _busRepo;
        private readonly IPrincipal _user;
        public ListSubscriptionDataHandler(
            ISubscriptionDataRepository busRepo,
            IPrincipal user)
        {
            _user = user;
            _busRepo = busRepo;
        }

        public async Task<DReturnObject> Handle(ListSubscriptionData request, CancellationToken cancellationToken)
        {
            return await _busRepo.ListAll(request.Email, cancellationToken);
        }
    }
}
