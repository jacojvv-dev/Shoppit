using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Builders
{
    public static class ProductMetadataBuilder
    {
        public static void BuildProductMetadata(this ModelBuilder builder)
        {
            builder.Entity<ProductMetadata>()
                .HasKey(productMetadata => productMetadata.Id);
            builder.Entity<ProductMetadata>()
                .Property(productMetadata => productMetadata.Key)
                .IsRequired()
                .HasMaxLength(250);
            builder.Entity<ProductMetadata>()
                .Property(productMetadata => productMetadata.Value)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Entity<ProductMetadata>()
                .HasOne(productMetadata => productMetadata.Product)
                .WithMany(product => product.ProductMetadata);
        }
    }
}