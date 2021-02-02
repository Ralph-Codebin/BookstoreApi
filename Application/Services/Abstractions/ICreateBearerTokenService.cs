using Application.Responses.LoginResponses;
using Optional;
using System.Threading.Tasks;

namespace Application.Services.Abstractions
{
    public interface ICreateBearerTokenService
    {
        Task<Option<BearerToken>> CreateTokenAsync(string username, string password);
    }
}
