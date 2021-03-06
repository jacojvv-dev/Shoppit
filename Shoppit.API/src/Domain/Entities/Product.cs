using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Product : ITimestampable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IList<ProductMetadata> ProductMetadata { get; set; }
        public IList<ProductImage> ProductImages { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}