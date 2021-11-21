using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Responses;
using API.Responses.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<PaginatedResponse<ProductResponse>> List([FromQuery] List.Query query)
            => _mediator.Send(query);

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailResponse>> Get([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new Get.Query() {Id = id});
            if (response == null) return NotFound();
            return response;
        }
    }
}