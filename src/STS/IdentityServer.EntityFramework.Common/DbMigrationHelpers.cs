using IdentityServer.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.EntityFramework.Common
{
    public static class DbMigrationHelpers
    {
        /// <summary>
        /// 迁移数据库、生成Seed数据
        /// </summary>
        public static async Task ApplyDbMigrationsWithDataSeedAsync<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext>(IHost host, bool seed)
            where TIdentityDbContext : DbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                await EnsureDatabasesMigratedAsync<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext>(services);

                if (seed)
                {
                    await EnsureSeedDataAsync<TIdentityDbContext>(services);
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


        public static async Task EnsureSeedDataAsync<TIdentityDbContext>(IServiceProvider serviceProvider)
            where TIdentityDbContext : DbContext
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                var identityDbContext = scope.ServiceProvider.GetRequiredService<TIdentityDbContext>();
                identityDbContext
                var context = scope.ServiceProvider.GetRequiredService<TIdentityServerDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<TRole>>();
                var rootConfiguration = scope.ServiceProvider.GetRequiredService<IRootConfiguration>();

                await EnsureSeedIdentityServerData(context, rootConfiguration.IdentityServerDataConfiguration);
                await EnsureSeedIdentityData(userManager, roleManager, rootConfiguration.IdentityDataConfiguration);
            }
        }



    }
}
