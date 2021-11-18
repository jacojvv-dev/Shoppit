using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using Shoppit.Auth.Data;

namespace Shoppit.Auth.Seeders
{
    public class OpeniddictSeeder : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public OpeniddictSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync(cancellationToken);

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("storefront", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "storefront",
                    DisplayName = "Storefront",
                    RedirectUris = {new Uri("https://oauth.pstmn.io/v1/callback"), new Uri("http://localhost:4200")},
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,

                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",

                        OpenIddictConstants.Permissions.ResponseTypes.Code
                    }
                }, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}