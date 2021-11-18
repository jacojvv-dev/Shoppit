using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API.Seeders
{
    /// <summary>
    /// This seeder exists solely for the purposes of this assessment and would
    /// not be present in a production system
    /// </summary>
    public class ProductSeeder : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ProductSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var (productJsonPath, imageFolderPath) = ExtractProductZipFile();
            var seedData = await DeserializeProductSeedData(productJsonPath, cancellationToken);

            await SeedProductData(seedData, cancellationToken);
            await EnsureImages(seedData, imageFolderPath, cancellationToken);
        }
        
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private (string productJsonPath, string imageFolderPath) ExtractProductZipFile()
        {
            using var scope = _serviceProvider.CreateScope();

            var hostEnvironment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

            var contentRoot = hostEnvironment.ContentRootPath;
            var productDataPath = Path.Combine(contentRoot, "App_Data", "product_data.zip");
            var extractionPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            ZipFile.ExtractToDirectory(productDataPath, extractionPath);

            var productJsonPath = Path.Combine(extractionPath, "product_data", "products.json");
            var imagePath = Path.Combine(extractionPath, "product_data", "images");

            return (productJsonPath, imagePath);
        }

        private async Task<List<ProductSeed>> DeserializeProductSeedData(
            string productJsonPath,
            CancellationToken cancellationToken = default)
        {
            var jsonText = await File.ReadAllTextAsync(productJsonPath, cancellationToken);
            return JsonSerializer.Deserialize<List<ProductSeed>>(jsonText);
        }

        private async Task SeedProductData(List<ProductSeed> seedData, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync(cancellationToken);

            foreach (var seed in seedData)
            {
                var exists = await context.Products.AnyAsync(
                    product => product.Name == seed.Name,
                    cancellationToken);
                if (exists) continue;

                var product = new Product()
                {
                    Name = seed.Name,
                    Price = seed.Price,
                    ProductMetadata = seed.Metadata.Select(metadata => new ProductMetadata()
                    {
                        Key = metadata.Key,
                        Value = metadata.Value
                    }).ToList(),
                    ProductImages = seed.Images.Select(image => new ProductImage()
                    {
                        Location = image
                    }).ToList()
                };
                context.Products.Add(product);
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureImages(
            List<ProductSeed> seedData,
            string imageFolderPath,
            CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var blobServiceClient = scope.ServiceProvider.GetRequiredService<BlobServiceClient>();
            var containerClient = blobServiceClient.GetBlobContainerClient("shoppit-images");
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = "image/jpeg",
                CacheControl = "max-age=15778800"
            };

            foreach (var imageName in seedData.SelectMany(seed => seed.Images))
            {
                var fileLocation = Path.Combine(imageFolderPath, imageName);
                var fs = File.OpenRead(fileLocation);
                var blobClient = containerClient.GetBlobClient(imageName);
                if (!await blobClient.ExistsAsync())
                    await blobClient.UploadAsync(fs, httpHeaders, cancellationToken: cancellationToken);
            }
        }

        private class ProductSeed
        {
            [JsonPropertyName("name")] public string Name { get; set; }
            [JsonPropertyName("price")] public decimal Price { get; set; }
            [JsonPropertyName("metadata")] public Dictionary<string, string> Metadata { get; set; }
            [JsonPropertyName("images")] public List<string> Images { get; set; }
        }
    }
}