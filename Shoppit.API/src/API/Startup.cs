using API.Exceptions;
using API.Mapping;
using API.Seeders;
using Azure.Storage.Blobs;
using Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                )
            );

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            services.AddOpenIddict()
                .AddValidation(options =>
                {
                    options.SetIssuer("https://localhost:5001/");
                    options.UseSystemNetHttp();
                    options.UseAspNetCore();
                });

            services.AddAutoMapper(cfg =>
            {
                AutoMapperBase.AddMappings(cfg, Configuration["CdnHost"]);
            });
            
            services.AddSingleton(new BlobServiceClient(Configuration.GetConnectionString("Storage")));

            services.AddHostedService<ProductSeeder>();

            services.AddHttpContextAccessor();

            services.AddMediatR(typeof(Startup));
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "API", Version = "v1"}); });
            services.AddControllers(opts =>
            {
                opts.Filters.Add(new ApplicationExceptionFilter());
            }).AddDataAnnotationsLocalization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}