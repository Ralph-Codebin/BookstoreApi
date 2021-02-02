using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Application.Requests.LoginRequests;
using Application.Responses.LoginResponses;

namespace bookstore_api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LoginController(
            IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] PostLoginRequest loginRequest)
        {
            return
                (await _mediator.Send(_mapper.Map<CreateBearerToken>(loginRequest)))
                .Match<IActionResult>(
                    some: token => Created(nameof(Login), _mapper.Map<PostLoginResponse>(token)),
                    none: () => Unauthorized());
        }     
    }
}
