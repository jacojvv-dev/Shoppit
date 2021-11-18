using System.Collections.Generic;
using API.Responses.Product;

namespace API.Responses.Cart
{
    public class CartItemResponse
    {
        public int Quantity { get; set; }
        public ProductResponse Product { get; set; }
    }
}