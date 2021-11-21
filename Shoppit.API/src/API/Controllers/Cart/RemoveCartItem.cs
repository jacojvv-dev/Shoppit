using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Extensions;
using AutoMapper;
using Data;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Cart
{
    public class RemoveCartItem
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IMapper _mapper;
            private readonly ApplicationDbContext _context;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor contextAccessor)
            {
                _mapper = mapper;
                _context = context;
                _contextAccessor = contextAccessor;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var userId = _contextAccessor.HttpContext.User.GetUserId();
                var cartItem = await GetCartItem(command, userId, cancellationToken);
                if (cartItem != default)
                {
                    _context.Remove(cartItem);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }

            private Task<CartItem> GetCartItem(Command command, Guid userId, CancellationToken token)
                => _context
                    .CartItems
                    .FirstOrDefaultAsync(
                        item => item.UserId == userId && item.ProductId == command.Id,
                        cancellationToken: token);
        }
    }
}