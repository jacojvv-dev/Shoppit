using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Nest;

namespace ApplicationCore.Models
{
    public class ElasticProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// The name, but for search-as-you-type
        /// </summary>
        [Text]
        public string AutocompleteName { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        [Nested]
        public List<ElasticProductMetadata> Metadata { get; set; }
        [Nested]
        public List<ElasticProductImage> Images { get; set; }

        public ElasticProduct()
        {
        }

        public ElasticProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (product.ProductImages == null) throw new ArgumentNullException(nameof(product.ProductImages));
            if (product.ProductMetadata == null) throw new ArgumentNullException(nameof(product.ProductMetadata));

            var metadata = product.ProductMetadata
                .Select(plm => new ElasticProductMetadata(plm.Key, plm.Value))
                .ToList();

            var images = product.ProductImages.Select(pi => new ElasticProductImage(pi.Id, pi.Location)).ToList();
            
            Id = product.Id;
            Price = product.Price;
            Name = product.Name;
            AutocompleteName = product.Name;
            Description = product.Description;
            Metadata = metadata;
            Images = images;
        }
    }
}