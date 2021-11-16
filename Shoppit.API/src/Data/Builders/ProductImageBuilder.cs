using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Builders
{
    public static class ProductImageBuilder
    {
        public static void BuildProductImage(this ModelBuilder builder)
        {
            builder.Entity<ProductImage>()
                .HasKey(productImage => productImage.Id);
            builder.Entity<ProductImage>()
                .Property(productImage => productImage.Location)
                .IsRequired()
                .HasMaxLength(250);

            builder.Entity<ProductImage>()
                .HasOne(productImage => productImage.Product)
                .WithMany(product => product.ProductImages);
        }
    }
}