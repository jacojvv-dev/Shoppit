using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Builders
{
    public static class ProductBuilder
    {
        public static void BuildProduct(this ModelBuilder builder)
        {
            builder.Entity<Product>()
                .HasKey(product => product.Id);
            builder.Entity<Product>()
                .Property(product => product.Name)
                .IsRequired()
                .HasMaxLength(1000);
            builder.Entity<Product>()
                .Property(product => product.Description)
                .IsRequired(false)
                .HasMaxLength(2500);
            builder.Entity<Product>()
                .Property(product => product.Price)
                .IsRequired()
                .HasPrecision(19, 4);

            builder.Entity<Product>()
                .HasMany(product => product.ProductMetadata)
                .WithOne(productMetadata => productMetadata.Product);
            builder.Entity<Product>()
                .HasMany(product => product.ProductImages)
                .WithOne(productImage => productImage.Product);
        }
    }
}