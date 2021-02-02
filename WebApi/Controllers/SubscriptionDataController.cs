using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Application.Requests.ProductDataRequests;
using bookstore_api.Requests;
using bookstore_api.Models;
using Domain.Model.Entities;

namespace bookstore_api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(401)]
    public class SubscriptionDataController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SubscriptionDataController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("ListAll")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ReturnObject>> ListAll(string useremail)
        {
            return _mapper.Map<ReturnObject>(await _mediator.Send(new ListSubscriptionData(useremail)));                  
        }

        [HttpPost("NewSubscription")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ReturnObject>> NewSubscription([FromBody] NewSubscriptiontRequest newproduct)
        {
            DNewSubscriptiontRequest ndf = new DNewSubscriptiontRequest();
            _mapper.Map(newproduct, ndf);
            return _mapper.Map<ReturnObject>(await _mediator.Send(new NewSubscriptionData(ndf)));
        }

        [HttpPost("UpdateSubscription")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ReturnObject>> UpdateSubscription([FromBody] UpdateSubscriptiontRequest updateproduct)
        {
            DUpdateSubscriptiontRequest prod = new DUpdateSubscriptiontRequest();
            _mapper.Map(updateproduct, prod);
            return _mapper.Map<ReturnObject>(await _mediator.Send(new UpdateSubscriptionData(prod)));
        }
    }

}
