using System;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class ProductMetadata : ITimestampable
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}