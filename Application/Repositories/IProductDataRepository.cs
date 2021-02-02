using Domain.Model.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IProductDataRepository
    {
        Task<DReturnObject> ListAll();
        Task<DReturnObject> NewProduct(DNewProductRequest newproduct, CancellationToken cancellationToken);
        Task<DReturnObject> UpdateProduct(DUpdateProductDataRequest updateproduct, CancellationToken cancellationToken);
    }
}
