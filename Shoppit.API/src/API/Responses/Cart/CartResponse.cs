using System.Collections.Generic;
using System.Linq;
using API.Controllers.Products;

namespace API.Responses.Cart
{
    public class CartResponse
    {
        public List<CartItemResponse> CartItems { get; set; }
        public decimal Vat { get; set; }
        public decimal TotalWithoutVat { get; set; }
        public decimal Total { get; set; }

        public CartResponse(List<CartItemResponse> cartItems)
        {
            CartItems = cartItems;
            Total = CartItems.Sum(cartItem => cartItem.Quantity * cartItem.Product.Price);
            TotalWithoutVat = Total / (115) * 100;
            Vat = Total - TotalWithoutVat;
        }
    }
}