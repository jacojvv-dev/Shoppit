using System;
using System.Threading;
using System.Threading.Tasks;
using API.Exceptions.Types;
using API.Extensions;
using API.Responses.Cart;
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

            public async Task<Unit> Handle(Command command, CancellationToken token)
            {
                var userId = _contextAccessor.HttpContext.User.GetUserId();
                var cartItem = await GetCartItem(command, token, userId);
                if (cartItem != default)
                {
                    _context.Remove(cartItem);
                    await _context.SaveChangesAsync(token);
                }

                return Unit.Value;
            }

            private Task<CartItem> GetCartItem(Command command, CancellationToken token, Guid userId)
                => _context
                    .CartItems
                    .FirstOrDefaultAsync(
                        item => item.UserId == userId && item.ProductId == command.Id,
                        cancellationToken: token);
        }
    }
}