using bookstore_api.Requests;
using Domain.Model.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ISubscriptionDataRepository
    {
        Task<DReturnObject> ListAll(string useremail, CancellationToken cancellationToken);
        Task<DReturnObject> NewSubscription(DNewSubscriptiontRequest data, CancellationToken cancellationToken);
        Task<DReturnObject> UpdateSubscription(DUpdateSubscriptiontRequest data, CancellationToken cancellationToken);
    }
}
