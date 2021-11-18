using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Extensions;
using API.Responses.Cart;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Cart
{
    public class GetCart
    {
        public class Query : IRequest<CartResponse>
        {
        }

        public class Handler : IRequestHandler<Query, CartResponse>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor contextAccessor)
            {
                _context = context;
                _mapper = mapper;
                _contextAccessor = contextAccessor;
            }

            public async Task<CartResponse> Handle(Query query, CancellationToken token)
            {
                var userId = _contextAccessor.HttpContext.User.GetUserId();
                var cartItems = await _context
                    .CartItems
                    .GetCartItemsForUser(_mapper, userId, token);

                return new CartResponse(cartItems);
            }
        }
    }
}