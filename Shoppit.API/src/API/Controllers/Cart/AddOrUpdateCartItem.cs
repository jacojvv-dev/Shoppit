using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using API.Responses.Cart;
using ApplicationCore.Exceptions;
using ApplicationCore.Extensions;
using AutoMapper;
using Data;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Cart
{
    public class AddOrUpdateCartItem
    {
        public class Command : IRequest<CartItemResponse>
        {
            [Required] public Guid ProductId { get; set; }
            [Required] public int Quantity { get; set; }
        }

        public class Handler : IRequestHandler<Command, CartItemResponse>
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

            public async Task<CartItemResponse> Handle(Command command, CancellationToken cancellationToken)
            {
                await EnsureProductExists(command, cancellationToken);

                var userId = _contextAccessor.HttpContext.User.GetUserId();
                var cartItem = await GetCartItem(command, userId, cancellationToken) ?? new CartItem();
                var cartItemIsNew = cartItem.ProductId == default;

                cartItem.Quantity = command.Quantity;
                if (cartItemIsNew)
                {
                    cartItem.ProductId = command.ProductId;
                    cartItem.UserId = userId;
                    _context.CartItems.Add(cartItem);
                }

                await _context.SaveChangesAsync(cancellationToken);

                if (cartItemIsNew)
                    cartItem = await GetCartItem(command, userId, cancellationToken);

                return _mapper.Map<CartItemResponse>(cartItem);
            }

            private Task<CartItem> GetCartItem(Command command, Guid userId, CancellationToken cancellationToken)
                => _context
                    .CartItems
                    .Include(cartItem => cartItem.Product)
                    .ThenInclude(cartItem => cartItem.ProductImages)
                    .FirstOrDefaultAsync(
                        item => item.UserId == userId && item.ProductId == command.ProductId,
                        cancellationToken: cancellationToken);

            private async Task EnsureProductExists(Command command, CancellationToken cancellationToken)
            {
                var productExists = await _context
                    .Products
                    .AnyAsync(product => product.Id == command.ProductId, cancellationToken);
                if (!productExists) throw new ApplicationDataNotFoundException("Product not found");
            }
        }
    }
}