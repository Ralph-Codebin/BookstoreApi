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
    public class ProductDataController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductDataController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("ListAll")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ReturnObject>> ListAll()
        {
            return _mapper.Map<ReturnObject>(await _mediator.Send(new ListProductData()));                  
        }

        [HttpPost("NewProduct")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ReturnObject>> NewProduct([FromBody] NewProductRequest newproduct)
        {
            DNewProductRequest ndf = new DNewProductRequest();
            _mapper.Map(newproduct, ndf);
            return _mapper.Map<ReturnObject>(await _mediator.Send(new NewProduct(ndf)));
        }

        [HttpPost("UpdateProduct")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ReturnObject>> UpdateProduct([FromBody] UpdateProductDataRequest updateproduct)
        {
            DUpdateProductDataRequest prod = new DUpdateProductDataRequest();
            _mapper.Map(updateproduct, prod);
            return _mapper.Map<ReturnObject>(await _mediator.Send(new UpdateProduct(prod)));
        }
    }

}
