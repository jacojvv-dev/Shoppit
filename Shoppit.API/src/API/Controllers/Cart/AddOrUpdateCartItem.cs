using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Controllers.Products;
using API.Exceptions.Types;
using API.Extensions;
using API.Responses.Cart;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Cart
{
    public class AddOrUpdateCartItem
    {
        public class Command :  IRequest<CartResponse>
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public class Handler : IRequestHandler<Command, CartResponse>
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

            public async Task<CartResponse> Handle(Command command, CancellationToken token)
            {
                await EnsureProductExists(command, token);

                var userId = Guid.NewGuid(); // _contextAccessor.HttpContext.User.GetUserId();
                var cartItem = await GetCartItem(command, token, userId) ?? new CartItem();
                
                cartItem.Quantity = command.Quantity;
                if (cartItem.ProductId == default)
                {
                    cartItem.ProductId = command.ProductId;
                    cartItem.UserId = userId;
                    _context.CartItems.Add(cartItem);
                }
                await _context.SaveChangesAsync(token);

                var cartItems = await _context
                    .CartItems
                    .GetCartItemsForUser(_mapper, userId, token);

                return new CartResponse(cartItems);
            }

            private Task<CartItem> GetCartItem(Command command, CancellationToken token, Guid userId)
                => _context
                    .CartItems
                    .FirstOrDefaultAsync(
                        item => item.UserId == userId && item.ProductId == command.ProductId,
                        cancellationToken: token);

            private async Task EnsureProductExists(Command command, CancellationToken token)
            {
                var productExists = await _context
                    .Products
                    .AnyAsync(product => product.Id == command.ProductId, cancellationToken: token);
                if (!productExists) throw new ApplicationDataNotFoundException("Product not found");
            }
        }
    }
}