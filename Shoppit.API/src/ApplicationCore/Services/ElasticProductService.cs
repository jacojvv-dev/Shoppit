using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using ApplicationCore.Models;
using ApplicationCore.Options;
using Microsoft.Extensions.Options;
using Nest;

namespace ApplicationCore.Services
{
    public interface IElasticProductService
    {
        Task<IndexResponse> IndexProductAsync(ElasticProduct product, CancellationToken cancellationToken = default);

        Task<ISearchResponse<ElasticProduct>> SearchProductsAsync(
            int page,
            int perPage,
            string query,
            CancellationToken cancellationToken = default);
    }

    public class ElasticProductService : IElasticProductService
    {
        private readonly ElasticSearchOptions _options;
        private readonly IElasticClient _client;

        public ElasticProductService(IOptions<ElasticSearchOptions> options, IElasticClient client)
        {
            _options = options.Value;
            _client = client;
        }

        public async Task<IndexResponse> IndexProductAsync(ElasticProduct product,
            CancellationToken cancellationToken = default)
        {
            var indexName = ElasticSearchIndexHelper.GetProductIndexName(_options.IndexPrefix);
            return await _client.IndexAsync(product, idx => idx.Index(indexName), cancellationToken);
        }

        public async Task<ISearchResponse<ElasticProduct>> SearchProductsAsync(
            int page,
            int perPage,
            string query,
            CancellationToken cancellationToken = default)
        {
            var indexName = ElasticSearchIndexHelper.GetProductIndexName(_options.IndexPrefix);

            var nameField = nameof(ElasticProduct.Name).ToLower();
            var descriptionField = nameof(ElasticProduct.Description).ToLower();

            return await _client.SearchAsync<ElasticProduct>(s => s
                .Source(sf => sf
                    .Excludes(e => e
                        .Fields(
                            f => f.AutocompleteName
                        )
                    )
                )
                .Index(indexName)
                .From((page - 1) * perPage)
                .Size(perPage)
                .Query(q =>
                    q.DisMax(
                        m => m.TieBreaker(0.3D)
                            .Queries(
                                q2 => q2.MultiMatch(s2 =>
                                    s2.Fields(f => f.Fields(nameField))
                                        .Boost(5)
                                        .Query(query)
                                        .Type(TextQueryType.MostFields)
                                        .Fuzziness(Fuzziness.Auto)
                                ),
                                q3 => q3.MultiMatch(s3 =>
                                    s3.Fields(f => f.Fields(descriptionField))
                                        .Boost(2)
                                        .Query(query)
                                        .Type(TextQueryType.Phrase)
                                )
                            )
                    )
                ), cancellationToken);
        }
    }
}