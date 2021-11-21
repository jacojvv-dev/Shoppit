using API.Responses;
using ApplicationCore.Models;
using AutoMapper;

namespace API.Mapping
{
    public class AutoMapperBase : Profile
    {
        public AutoMapperBase()
        {
            CreateMap(typeof(PaginatedData<>), typeof(PaginatedResponse<>));
        }

        public static void AddMappings(IMapperConfigurationExpression configuration, string cdnHost)
        {
            configuration.AddProfile<AutoMapperBase>();
            configuration.AddProfile(new ProductMapping(cdnHost));
            configuration.AddProfile(new CartItemMapping());
        }
    }
}