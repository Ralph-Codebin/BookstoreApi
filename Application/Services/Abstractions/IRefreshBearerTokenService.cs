
using Application.Responses.LoginResponses;
using Optional;
using System.Security.Principal;

namespace Application.Services.Abstractions
{
    public interface IRefreshBearerTokenService
    {
        Option<BearerToken> RefreshToken(IPrincipal authenticatedUser);
    }
}
