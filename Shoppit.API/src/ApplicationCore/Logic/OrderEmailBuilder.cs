using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace ApplicationCore.Logic
{
    public class OrderEmailBuilder
    {
        private string ContentRoot { get; set; }
        private int OrderNumber { get; set; }
        private IEnumerable<CartItem> CartItems { get; set; }

        public OrderEmailBuilder WithContentRoot(string contentRoot)
        {
            ContentRoot = contentRoot;
            return this;
        }

        public OrderEmailBuilder WithOrderNumber(int orderNumber)
        {
            OrderNumber = orderNumber;
            return this;
        }

        public OrderEmailBuilder WithCartItems(IEnumerable<CartItem> cartItems)
        {
            CartItems = cartItems;
            return this;
        }

        public async Task<string> BuildAsync(CancellationToken cancellationToken = default)
        {
            var (orderReceiptTemplatePath, orderReceiptItemTemplatePath) = GetTemplatePaths();

            var orderReceiptTemplateHtml = await File.ReadAllTextAsync(
                orderReceiptTemplatePath,
                cancellationToken);
            var orderReceiptItemTemplateHtml = await File.ReadAllTextAsync(
                orderReceiptItemTemplatePath,
                cancellationToken);

            return ApplyTemplating(orderReceiptItemTemplateHtml, orderReceiptTemplateHtml);
        }

        public string Build()
        {
            var (orderReceiptTemplatePath, orderReceiptItemTemplatePath) = GetTemplatePaths();

            var orderReceiptTemplateHtml = File.ReadAllText(orderReceiptTemplatePath);
            var orderReceiptItemTemplateHtml = File.ReadAllText(orderReceiptItemTemplatePath);

            return ApplyTemplating(orderReceiptItemTemplateHtml, orderReceiptTemplateHtml);
        }


        private (string orderReceiptTemplatePath, string orderReceiptItemTemplatePath) GetTemplatePaths()
        {
            var emailTemplatePath = Path.Combine(ContentRoot, "App_Data", "EmailTemplates");
            var orderReceiptTemplatePath = Path.Combine(emailTemplatePath, "OrderReceipt.html");
            var orderReceiptItemTemplatePath = Path.Combine(emailTemplatePath, "OrderReceiptItem.html");

            return (orderReceiptTemplatePath, orderReceiptItemTemplatePath);
        }

        private string ApplyTemplating(string orderReceiptItemTemplateHtml, string orderReceiptTemplateHtml)
        {
            var orderReceiptItemsStringBuilder = new StringBuilder();
            foreach (var cartItem in CartItems)
            {
                var itemHtml = orderReceiptItemTemplateHtml
                    .Replace("[[ItemName]]", cartItem.Product.Name)
                    .Replace("[[ItemQuantity]]", cartItem.Quantity.ToString())
                    .Replace("[[ItemPrice]]", (cartItem.Quantity * cartItem.Product.Price).ToString("F"));
                orderReceiptItemsStringBuilder.Append(itemHtml);
            }

            return orderReceiptTemplateHtml
                .Replace("[[OrderNumber]]", "1")
                .Replace("[[OrderReceiptItems]]", orderReceiptItemsStringBuilder.ToString());
        }
    }
}