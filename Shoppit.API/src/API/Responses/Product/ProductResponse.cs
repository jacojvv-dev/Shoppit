using System;
using System.Collections.Generic;

namespace API.Responses.Product
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<ProductMetadataResponse> Metadata { get; set; }
        public List<ProductImageResponse> Images { get; set; }
    }
}