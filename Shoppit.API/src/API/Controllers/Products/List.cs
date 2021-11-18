using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Responses.Product;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Products
{
    public class List
    {
        public class Query : IRequest<List<ProductResponse>>
        {
            /// <summary>
            /// Query to search by
            /// </summary>
            public string SearchQuery { get; set; }
            /// <summary>
            /// Page to retrieve
            /// </summary>
            public int Page { get; set; }
            /// <summary>
            /// Amount of items to retrieve per page
            /// </summary>
            public int PerPage { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<ProductResponse>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<List<ProductResponse>> Handle(Query query, CancellationToken token)
                => _context
                    .Products
                    .Where(Product.GetCommonPredicate(query.SearchQuery))
                    .ProjectTo<ProductResponse>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .ToListAsync(token);
        }
    }
}