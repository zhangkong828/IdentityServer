using IdentityServer.EntityFramework;
using IdentityServer.EntityFramework.DbContexts;
using IdentityServer.Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb
{
    public class Program
    {
        private const string SeedArgs = "/seed";

        public static async Task Main(string[] args)
        {
            var log = Log.GetLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                await ApplyDbMigrationsWithDataSeedAsync(args, host);

                host.Run();
            }
            catch (Exception ex)
            {
                log.Fatal("host terminated unexpectedly", ex);
            }
            finally
            {
                log.Release();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddConfigFile("IdentityServer.json", optional: true, reloadOnChange: true);
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        /// <summary>
        /// 确保数据库迁移、生成Seed数据
        /// </summary>
        private static async Task ApplyDbMigrationsWithDataSeedAsync(string[] args, IHost host)
        {
            //启动参数是否有  /seed
            var seed = args.Any(x => x == SeedArgs);
            if (seed) args = args.Except(new[] { SeedArgs }).ToArray();

            await DbMigrationHelpers.ApplyDbMigrationsWithDataSeedAsync<IdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext>(host, seed);
        }
    }
}
