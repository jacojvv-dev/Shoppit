using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Responses.Product;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Products
{
    public class Get
    {
        public class Query : IRequest<ProductDetailResponse>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProductDetailResponse>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<ProductDetailResponse> Handle(Query query, CancellationToken cancellationToken)
                => _context
                    .Products
                    .Where(product => product.Id == query.Id)
                    .ProjectTo<ProductDetailResponse>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}