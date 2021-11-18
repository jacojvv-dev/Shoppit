using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public Task<List<ProductResponse>> List([FromQuery] List.Query query)
            => _mediator.Send(query);
    }
}