using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Responses.Cart;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class CartItemDbSetExtensions
    {
        public static Task<List<CartItemResponse>> GetCartItemsForUser(
            this DbSet<CartItem> cartItems,
            IMapper mapper,
            Guid userId,
            CancellationToken cancellationToken = default)
            => cartItems.Where(cartItem => cartItem.UserId == userId)
                .ProjectTo<CartItemResponse>(mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
    }
}