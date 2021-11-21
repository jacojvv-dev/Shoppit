using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using ApplicationCore.Models;
using ApplicationCore.Options;
using Microsoft.Extensions.Options;
using Nest;

namespace ApplicationCore.Services
{
    public interface IElasticSearchService
    {
        Task EnsureIndexes(CancellationToken cancellationToken = default);
    }

    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IElasticClient _client;
        private readonly ElasticSearchOptions _options;

        public ElasticSearchService(IOptions<ElasticSearchOptions> options, IElasticClient client)
        {
            _client = client;
            _options = options.Value;
        }

        public async Task EnsureIndexes(CancellationToken cancellationToken = default)
        {
            var indexName = ElasticSearchIndexHelper.GetProductIndexName(_options.IndexPrefix);
            if (!(await _client.Indices.ExistsAsync(indexName, ct: cancellationToken)).Exists)
            {
                await _client.Indices.CreateAsync(indexName, c => c
                    .Map<ElasticProduct>(m => m
                        .AutoMap()
                        .Properties(ps => ps
                            .SearchAsYouType(g => g.Name(n => n.AutocompleteName))
                        )
                    ), cancellationToken);
            }
        }
    }
}