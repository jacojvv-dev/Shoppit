using System;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class OrderItem : ITimestampable
    {
        public Guid Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        // because price can change in a real system you'd want to capture the price at the time of sale for 
        // auditing purposes
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        // this assumes that products won't get deleted - and that a system of "active" or "inactive" will be implemented
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public OrderItem()
        {
            
        }

        public OrderItem(CartItem cartItem)
        {
            Price = cartItem.Product.Price;
            Quantity = cartItem.Quantity;
            ProductId = cartItem.ProductId;
        }
    }
}