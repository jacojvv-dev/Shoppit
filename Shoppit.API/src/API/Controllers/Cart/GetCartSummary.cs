using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Responses.Cart;
using ApplicationCore.Extensions;
using AutoMapper;
using Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Cart
{
    public class GetCartSummary
    {
        public class Query : IRequest<CartSummaryResponse>
        {
        }

        public class Handler : IRequestHandler<Query, CartSummaryResponse>
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

            public async Task<CartSummaryResponse> Handle(Query query, CancellationToken cancellationToken)
            {
                var userId = _contextAccessor.HttpContext.User.GetUserId();
                var cartTotal = await _context
                    .CartItems
                    .GetCartTotalForUser(userId, cancellationToken: cancellationToken);

                return new CartSummaryResponse(cartTotal);
            }
        }
    }
}