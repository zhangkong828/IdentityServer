using IdentityServer.EntityFramework;
using IdentityServer.EntityFramework.DbContexts;
using IdentityServer.EntityFramework.MySql;
using IdentityServer.EntityFramework.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb
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

            RegisterDbContexts(services);

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        /// <summary>
        /// 注册数据库上下文
        /// </summary>
        public virtual void RegisterDbContexts(IServiceCollection services)
        {
            //var databaseProvider = Config.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();
            var databaseProvider = Config.Get<DatabaseProviderConfiguration>("DatabaseProviderConfiguration");

            var identityConnectionString = Config.GetString("ConnectionStrings:AdminIdentityConnection");
            var configurationConnectionString = Config.GetString("ConnectionStrings:IdentityServerConnection");
            var persistedGrantsConnectionString = Config.GetString("ConnectionStrings:IdentityServerConnection");

            switch (databaseProvider.ProviderType)
            {
                case DatabaseProviderType.SqlServer:
                    services.RegisterSqlServerDbContexts<IdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext>(identityConnectionString, configurationConnectionString, persistedGrantsConnectionString);
                    break;
                case DatabaseProviderType.MySql:
                    services.RegisterMySqlDbContexts<IdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext>(identityConnectionString, configurationConnectionString, persistedGrantsConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider.ProviderType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseProviderType)))}.");
            }
        }
    }
}
