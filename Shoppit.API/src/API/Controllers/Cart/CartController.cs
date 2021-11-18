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
        public Task<CartResponse> GetCart([FromQuery] GetCart.Query query)
            => _mediator.Send(query);

        [HttpPost]
        public Task<CartResponse> AddOrUpdateCartItem([FromBody] AddOrUpdateCartItem.Command command)
            => _mediator.Send(command);
    }
}