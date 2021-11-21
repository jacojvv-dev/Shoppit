using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Extensions
{
    public static class CartItemDbSetExtensions
    {
        public static IQueryable<CartItem> GetCartItemsForUser(this DbSet<CartItem> dbSet, Guid userId)
            => dbSet.Where(cartItem => cartItem.UserId == userId);

        public static Task<decimal> GetCartTotalForUser(
            this DbSet<CartItem> dbSet,
            Guid userId,
            CancellationToken cancellationToken = default)
            => dbSet
                .GetCartItemsForUser(userId)
                .SumAsync(cartItem => cartItem.Quantity * cartItem.Product.Price,
                    cancellationToken: cancellationToken);
    }
}