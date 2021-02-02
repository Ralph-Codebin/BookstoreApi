
using Application.Responses.LoginResponses;
using Application.Services.Abstractions;
using MediatR;
using Optional;
using System.Security.Principal;

namespace Application.Requests.LoginRequests
{
    public class RefreshBearerToken : IRequest<Option<BearerToken>>
    {
        public RefreshBearerToken(IPrincipal authenticatedUser)
        {
            AuthenticatedUser = authenticatedUser;
        }

        public IPrincipal AuthenticatedUser { get; set; }
    }

    public class RefreshBearerTokenHandler : RequestHandler<RefreshBearerToken, Option<BearerToken>>
    {
        private readonly IRefreshBearerTokenService _refreshTokenService;
        public RefreshBearerTokenHandler(IRefreshBearerTokenService refreshTokenService) => _refreshTokenService = refreshTokenService;
        protected override Option<BearerToken> Handle(RefreshBearerToken request)
        {
            return _refreshTokenService.RefreshToken(request.AuthenticatedUser);
        }
    }
}
