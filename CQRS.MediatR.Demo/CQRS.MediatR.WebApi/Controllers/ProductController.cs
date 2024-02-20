using CQRS.MediatR.Domain.Command;
using CQRS.MediatR.Domain.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.MediatR.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await mediator.Send(new GetAllProductsQuery());
            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById([FromQuery]GetProductByIdQuery request)
        {
            var response = await mediator.Send(request);
            if (response is not null)
                return new JsonResult(response);
            else
                return BadRequest("No Product exists with the given id");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request)
        {
            var response = await mediator.Send(request);
            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery] DeleteProductCommand request)
        {
            var result = await mediator.Send(request);
           
            if (result!=null && result.IsDeleteSuccessful)
                return new JsonResult("Successfully deleted");
            else
                return BadRequest("No Product exists with the given id");
        }
    }
}
