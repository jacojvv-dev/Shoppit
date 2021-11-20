using System.Collections.Generic;

namespace API.Responses.Product
{
    public class ProductDetailResponse : ProductResponse
    {
        public string Description { get; set; }
        public List<ProductMetadataResponse> Metadata { get; set; }
    }
}