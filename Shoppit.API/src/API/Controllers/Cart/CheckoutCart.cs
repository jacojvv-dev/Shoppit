using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Extensions;
using ApplicationCore.Logic;
using ApplicationCore.Services;
using Data;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace API.Controllers.Cart
{
    public class CheckoutCart
    {
        public class CheckoutCommand : IRequest
        {
        }

        public class Handler : IRequestHandler<CheckoutCommand>
        {
            private readonly ApplicationDbContext _context;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IHostEnvironment _hostEnvironment;
            private readonly IEmailSender _emailSender;

            public Handler(ApplicationDbContext context,
                IHttpContextAccessor contextAccessor,
                IHostEnvironment hostEnvironment,
                IEmailSender emailSender)
            {
                _context = context;
                _contextAccessor = contextAccessor;
                _hostEnvironment = hostEnvironment;
                _emailSender = emailSender;
            }

            public async Task<Unit> Handle(CheckoutCommand checkoutCommand, CancellationToken token)
            {
                var userId = _contextAccessor.HttpContext.User.GetUserId();
                var email = _contextAccessor.HttpContext.User.GetUserEmail();

                var cartItems = await GetCartItems(userId, token);
                var order = await CreateOrderAndClearCart(userId, cartItems, token);
                await SendOrderConfirmationEmail(cartItems, order.Id, email, token);

                return Unit.Value;
            }

            private async Task<Order> CreateOrderAndClearCart(Guid userId, List<CartItem> cartItems,
                CancellationToken token)
            {
                var order = new Order(userId, cartItems);
                _context.Orders.Add(order);
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync(token);
                return order;
            }

            private Task<List<CartItem>> GetCartItems(Guid userId, CancellationToken cancellationToken)
                => _context
                    .CartItems
                    .GetCartItemsForUser(userId)
                    .Include(item => item.Product)
                    .ToListAsync(cancellationToken);

            private async Task SendOrderConfirmationEmail(
                IEnumerable<CartItem> cartItems,
                int orderNumber,
                string email,
                CancellationToken cancellationToken)
            {
                var emailHtml = await new OrderEmailBuilder()
                    .WithContentRoot(_hostEnvironment.ContentRootPath)
                    .WithCartItems(cartItems)
                    .WithOrderNumber(orderNumber)
                    .BuildAsync(cancellationToken);

                await _emailSender.SendEmailAsync(email, "Your order confirmation", emailHtml, cancellationToken);
            }
        }
    }
}