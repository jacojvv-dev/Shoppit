using System;

namespace API.Responses.Product
{
    public class ProductMetadataResponse
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}