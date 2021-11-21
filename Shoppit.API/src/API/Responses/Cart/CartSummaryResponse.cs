using System.Collections.Generic;
using System.Linq;
using API.Controllers.Products;

namespace API.Responses.Cart
{
    public class CartSummaryResponse
    {
        public decimal Vat { get; set; }
        public decimal TotalWithoutVat { get; set; }
        public decimal Total { get; set; }

        public CartSummaryResponse(decimal total)
        {
            var totalWithoutVat = total / (115) * 100;

            Total = total;
            TotalWithoutVat = totalWithoutVat;
            Vat = total - totalWithoutVat;
        }
    }
}