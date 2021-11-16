using System;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class ProductImage : ITimestampable
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}