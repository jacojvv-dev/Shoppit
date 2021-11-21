using System;
using API.Mapping;
using API.Seeders;
using ApplicationCore.Options;
using ApplicationCore.Services;
using Azure.Storage.Blobs;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using OpenIddict.Validation.AspNetCore;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabase(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    connectionString
                )
            );
        }

        public static void AddAuthenticationAndOpenIddict(this IServiceCollection serviceCollection, string issuer)
        {
            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            serviceCollection.AddOpenIddict()
                .AddValidation(options =>
                {
                    options.SetIssuer(issuer);
                    options.UseSystemNetHttp();
                    options.UseAspNetCore();
                });
        }

        public static void AddApplicationOptions(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.Configure<ElasticSearchOptions>(configuration.GetSection("ElasticSearch"));
        }


        public static void AddApplicationServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddScoped<IElasticSearchService, ElasticSearchService>();
            serviceCollection.AddScoped<IElasticProductService, ElasticProductService>();

            serviceCollection.AddHostedService<ProductSeeder>();
        }

        public static void AddThirdPartyServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddAutoMapper(cfg => { AutoMapperBase.AddMappings(cfg, configuration["CdnHost"]); });
            serviceCollection.AddMediatR(typeof(Startup));

            serviceCollection.AddSingleton(new BlobServiceClient(configuration.GetConnectionString("Storage")));
        }

        public static void AddElasticSearch(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var settings = new ConnectionSettings(new Uri(configuration["Elasticsearch:Host"]));
            settings.BasicAuthentication(
                configuration["Elasticsearch:Username"],
                configuration["Elasticsearch:Password"]);
            settings.DisableDirectStreaming();
            services.AddSingleton<IElasticClient>(new ElasticClient(settings));
        }
    }
}