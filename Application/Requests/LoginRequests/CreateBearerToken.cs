
using Application.Responses.LoginResponses;
using Application.Services.Abstractions;
using MediatR;
using Optional;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.LoginRequests
{
    public class CreateBearerToken : IRequest<Option<BearerToken>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class CreateBearerTokenHandler : IRequestHandler<CreateBearerToken, Option<BearerToken>>
    {
        private readonly ICreateBearerTokenService _tokenService;

        public CreateBearerTokenHandler(ICreateBearerTokenService tokenService) => _tokenService = tokenService;

        public Task<Option<BearerToken>> Handle(CreateBearerToken request, CancellationToken cancellationToken)
        {
            return _tokenService.CreateTokenAsync(request.Username, request.Password);
        }
    }
}
