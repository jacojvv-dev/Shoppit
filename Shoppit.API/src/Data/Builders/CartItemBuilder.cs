using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Builders
{
    public static class CartItemBuilder
    {
        public static void BuildCartItem(this ModelBuilder builder)
        {
            builder.Entity<CartItem>()
                .HasKey(item => new {item.UserId, item.ProductId});
            builder.Entity<CartItem>()
                .Property(item => item.Quantity)
                .IsRequired();
            
            builder.Entity<CartItem>()
                .HasOne(item => item.Product);
        }
    }
}