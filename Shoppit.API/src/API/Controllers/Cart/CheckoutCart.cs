using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Extensions;
using ApplicationCore.Logic;
using ApplicationCore.Services;
using AutoMapper;
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

                var cartItems = await _context
                    .CartItems
                    .GetCartItemsForUser(userId)
                    .Include(item => item.Product)
                    .ToListAsync(cancellationToken: token);

                var emailHtml = await new OrderEmailBuilder()
                    .WithContentRoot(_hostEnvironment.ContentRootPath)
                    .WithCartItems(cartItems)
                    .WithOrderNumber(1)
                    .BuildAsync(token);


                await _emailSender.SendEmailAsync(email, "Your order confirmation", emailHtml, token);

                return Unit.Value;
            }
        }
    }
}