using System.Collections.Generic;
using System.Linq;
using API.Responses.Product;
using AutoMapper;
using Domain.Entities;

namespace API.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping(string cdnHost)
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(x => x.Metadata, opt => opt.MapFrom(src => src.ProductMetadata))
                .ForMember(x => x.Images, opt => opt.MapFrom(src => src.ProductImages));
            CreateMap<ProductMetadata, ProductMetadataResponse>();
            CreateMap<ProductImage, ProductImageResponse>()
                .ForMember(x => x.Url, opt => opt.MapFrom(src => $"{cdnHost}/shoppit-images/{src.Location}"));
        }
    }
}