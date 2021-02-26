using IdentityServer.EntityFramework;
using IdentityServer.EntityFramework.DbContexts;
using IdentityServer.EntityFramework.MySql;
using IdentityServer.EntityFramework.Repositories;
using IdentityServer.EntityFramework.Repositories.Interfaces;
using IdentityServer.EntityFramework.SqlServer;
using IdentityServer.IdentityWeb.Extensions;
using IdentityServer.IdentityWeb.Validator;
using IdentityServer.Service;
using IdentityServer.Service.Interfaces;
using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityWeb
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

            RegisterAuthentication(services);

            RegisterServices(services);

            AddIdentityServer4(services);

            services.AddControllersWithViews()
            .AddRazorRuntimeCompilation();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader();
                    });
            });
        }

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

            app.UseCors("AllowAllOrigins");
            app.UseIdentityServer();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void RegisterDbContexts(IServiceCollection services)
        {
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

        public void RegisterAuthentication(IServiceCollection services)
        {
            var authenticationBuilder = services.AddAuthentication();

            var qqEnabled = Config.GetBool("ExternalProvidersConfiguration:QQ:Enabled");
            if (qqEnabled)
            {
                var clientId = Config.GetString("ExternalProvidersConfiguration:QQ:ClientId");
                var clientSecret = Config.GetString("ExternalProvidersConfiguration:QQ:ClientSecret");
                authenticationBuilder.AddQQ("qq", "QQ", options =>
                  {
                      options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                      options.ClientId = clientId;
                      options.ClientSecret = clientSecret;
                  });
            }

            var githubEnabled = Config.GetBool("ExternalProvidersConfiguration:Github:Enabled");
            if (githubEnabled)
            {
                var clientId = Config.GetString("ExternalProvidersConfiguration:Github:ClientId");
                var clientSecret = Config.GetString("ExternalProvidersConfiguration:Github:ClientSecret");
                authenticationBuilder.AddGitHub("github", "Github", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;
                });
            }
        }

        public void RegisterServices(IServiceCollection services)
        {
            //Repositories
            services.AddTransient<IIdentityRepository, IdentityRepository<IdentityDbContext>>();

            //Services
            services.AddTransient<IIdentityService, IdentityService>();
        }

        public void AddIdentityServer4(IServiceCollection services)
        {
            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.UserInteraction = new UserInteractionOptions
                {
                    LoginUrl = "/login",
                    LogoutUrl = "/logout"
                };
            })
            .AddConfigurationStore<IdentityServerConfigurationDbContext>()
            .AddOperationalStore<IdentityServerPersistedGrantDbContext>()
            .AddDeveloperSigningCredential()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            .AddProfileService<ProfileService>();
        }
    }
}
