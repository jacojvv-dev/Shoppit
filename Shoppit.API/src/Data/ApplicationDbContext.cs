using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Data.Builders;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductMetadata> ProductMetadata { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.BuildCartItem();
            modelBuilder.BuildProduct();
            modelBuilder.BuildProductImage();
            modelBuilder.BuildProductMetadata();
            modelBuilder.BuildOrder();
            modelBuilder.BuildOrderItem();
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var changes = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var entry in changes)
            {
                var type = entry.Entity.GetType();

                if (type.GetInterfaces().All(x => x != typeof(ITimestampable))) continue;

                PropertyInfo property;
                if (entry.State == EntityState.Added)
                {
                    property = type.GetProperty(nameof(ITimestampable.CreatedAt));
                    property?.SetValue(entry.Entity, DateTimeOffset.UtcNow, null);
                }

                property = type.GetProperty(nameof(ITimestampable.UpdatedAt));
                property?.SetValue(entry.Entity, DateTimeOffset.UtcNow, null);
            }
        }
    }
}