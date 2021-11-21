using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Responses.Cart;
using ApplicationCore.Extensions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Cart
{
    public class GetCartItems
    {
        public class Query : IRequest<List<CartItemResponse>>
        {
        }

        public class Handler : IRequestHandler<Query, List<CartItemResponse>>
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

            public Task<List<CartItemResponse>> Handle(Query query, CancellationToken cancellationToken)
            {
                var userId = _contextAccessor.HttpContext.User.GetUserId();
                return _context
                    .CartItems
                    .GetCartItemsForUser(userId)
                    .ProjectTo<CartItemResponse>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
        }
    }
}