using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Builders
{
    public static class OrderItemBuilder
    {
        public static void BuildOrderItem(this ModelBuilder builder)
        {
            builder.Entity<OrderItem>()
                .HasKey(item => item.Id);
            builder.Entity<OrderItem>()
                .Property(item => item.Price)
                .IsRequired()
                .HasPrecision(19, 4);
            builder.Entity<OrderItem>()
                .Property(item => item.Quantity)
                .IsRequired();

            builder.Entity<OrderItem>()
                .HasOne(item => item.Order);
            builder.Entity<OrderItem>()
                .HasOne(item => item.Product);
        }
    }
}