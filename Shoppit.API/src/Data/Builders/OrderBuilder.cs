using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Builders
{
    public static class OrderBuilder
    {
        public static void BuildOrder(this ModelBuilder builder)
        {
            builder.Entity<Order>()
                .HasKey(order => order.Id);
            builder.Entity<Order>()
                .Property(order => order.UserId)
                .IsRequired();

            builder.Entity<Order>()
                .HasMany(order => order.OrderItems)
                .WithOne(item => item.Order);
        }
    }
}