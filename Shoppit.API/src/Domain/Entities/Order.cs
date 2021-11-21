using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Order : ITimestampable
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Order()
        {
        }

        public Order(Guid userId, IEnumerable<CartItem> cartItems)
        {
            UserId = userId;
            OrderItems = cartItems.Select(item => new OrderItem(item)).ToList();
        }
    }
}