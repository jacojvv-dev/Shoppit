using System.Collections.Generic;
using System.Threading.Tasks;
using API.Responses.Cart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Cart
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<List<CartItemResponse>> GetCartItems([FromQuery] GetCartItems.Query query)
            => _mediator.Send(query);

        [HttpPost]
        public Task<CartItemResponse> AddOrUpdateCartItem([FromBody] AddOrUpdateCartItem.Command command)
            => _mediator.Send(command);

        [HttpDelete("{id}")]
        public Task AddOrUpdateCartItem([FromRoute] RemoveCartItem.Command command)
            => _mediator.Send(command);

        [HttpGet("summary")]
        public Task<CartSummaryResponse> GetCartSummary()
            => _mediator.Send(new GetCartSummary.Query());
    }
}