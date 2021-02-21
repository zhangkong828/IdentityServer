using IdentityModel;
using IdentityServer.EntityFramework.Configuration;
using IdentityServer.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.EntityFramework
{
    public static class DbMigrationHelpers
    {
        /// <summary>
        /// 迁移数据库、生成Seed数据
        /// </summary>
        public static async Task ApplyDbMigrationsWithDataSeedAsync<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext>(IHost host, bool seed)
            where TIdentityDbContext : DbContext, IIdentityDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                await EnsureDatabasesMigratedAsync<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext>(services);

                //if (seed)
                {
                    await EnsureSeedDataAsync<TConfigurationDbContext>(services);
                }
            }
        }

        /// <summary>
        /// 确保数据迁移
        /// </summary>
        public static async Task EnsureDatabasesMigratedAsync<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext>(IServiceProvider services)
          where TIdentityDbContext : DbContext
          where TPersistedGrantDbContext : DbContext
          where TConfigurationDbContext : DbContext
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<TIdentityDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TConfigurationDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TPersistedGrantDbContext>())
                {
                    await context.Database.MigrateAsync();
                }
            }
        }

        /// <summary>
        /// 生成Seed数据
        /// </summary>
        public static async Task EnsureSeedDataAsync<TConfigurationDbContext>(IServiceProvider serviceProvider)
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
        {
            var identityServerDataConfiguration = Config.Get<IdentityServerDataConfiguration>("IdentityServerData");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TConfigurationDbContext>();

                if (!await context.IdentityResources.AnyAsync())
                {
                    foreach (var resource in identityServerDataConfiguration.IdentityResources)
                    {
                        await context.IdentityResources.AddAsync(resource.ToEntity());
                    }

                    await context.SaveChangesAsync();
                }

                if (!await context.ApiResources.AnyAsync())
                {
                    foreach (var resource in identityServerDataConfiguration.ApiResources)
                    {
                        foreach (var s in resource.ApiSecrets)
                        {
                            s.Value = s.Value.ToSha256();
                        }

                        await context.ApiResources.AddAsync(resource.ToEntity());
                    }

                    await context.SaveChangesAsync();
                }

                if (!await context.Clients.AnyAsync())
                {
                    foreach (var client in identityServerDataConfiguration.Clients)
                    {
                        foreach (var secret in client.ClientSecrets)
                        {
                            secret.Value = secret.Value.ToSha256();
                        }

                        await context.Clients.AddAsync(client.ToEntity());
                    }

                    await context.SaveChangesAsync();
                }
            }
        }



    }
}
