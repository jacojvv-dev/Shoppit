using System;
using System.Threading;
using System.Threading.Tasks;
using API.Responses;
using API.Responses.Product;
using ApplicationCore.Models;
using ApplicationCore.Services;
using AutoMapper;
using Data;
using MediatR;

namespace API.Controllers.Products
{
    public class List
    {
        public class Query : IRequest<PaginatedResponse<ProductResponse>>
        {
            public string SearchQuery { get; set; }
            public int Page { get; set; }
            public int PerPage { get; set; }
        }

        public class Handler : IRequestHandler<Query, PaginatedResponse<ProductResponse>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IElasticProductService _elasticProductService;

            public Handler(ApplicationDbContext context, IMapper mapper, IElasticProductService elasticProductService)
            {
                _context = context;
                _mapper = mapper;
                _elasticProductService = elasticProductService;
            }

            public async Task<PaginatedResponse<ProductResponse>> Handle(Query query, CancellationToken token)
            {
                var page = Math.Clamp(query.Page, 1, int.MaxValue);
                var perPage = Math.Clamp(query.PerPage, 1, 50);

                var searchResponse =
                    await _elasticProductService.SearchProductsAsync(page, perPage, query.SearchQuery, token);
                var paginatedData =
                    new PaginatedData<ElasticProduct>(searchResponse.Documents, searchResponse.Total, page, perPage);

                return _mapper.Map<PaginatedResponse<ProductResponse>>(paginatedData);
            }
        }
    }
}