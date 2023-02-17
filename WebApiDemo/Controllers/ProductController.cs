using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Application.Commands;
using WebApiDemo.Application.Entity;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProductService _service;
        public ProductController(IMediator mediator, IProductService service)
        {
            _mediator = mediator;
            _service = service;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _service.GetAllProducts());
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var state = await _mediator.Send(command);
            if (state)
                return Ok("Created successfully!");
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            await _service.DeleteProduct(Id);
            return Ok();
        }
    }
}
