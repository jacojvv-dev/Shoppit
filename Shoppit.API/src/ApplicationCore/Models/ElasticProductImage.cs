using System;
using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class ElasticProductImage
    {
        public Guid Id { get; set; }
        public string Url { get; set; }


        public ElasticProductImage()
        {
        }

        public ElasticProductImage(Guid id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}