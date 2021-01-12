using IdentityServer.IdentityServer4Extension.Validator;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class IdentityServer4Extensions
    {
        /// <summary>
        /// 基于MySql
        /// </summary>
        /// <param name="services"></param>
        /// <param name="identityServer4OptionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityServer4UseMySql(this IServiceCollection services, Action<IdentityServer4Options> identityServer4OptionsAction = null)
        {
            var options = new IdentityServer4Options();
            identityServer4OptionsAction?.Invoke(options);


            services.AddIdentityServer(o =>
            {
                if (!string.IsNullOrWhiteSpace(options.LoginUrl))
                {
                    o.UserInteraction = new IdentityServer4.Configuration.UserInteractionOptions()
                    {
                        LoginUrl = options.LoginUrl,
                        LogoutUrl = options.LogoutUrl
                    };
                }
            })
            .AddConfigurationStore(storeOptions =>
            {
                storeOptions.ConfigureDbContext = b => b.UseMySql(options.DbConnectionStrings, sql => sql.MigrationsAssembly(options.MigrationsAssembly));
            })
            .AddOperationalStore(operationalStoreOptions =>
            {
                operationalStoreOptions.ConfigureDbContext = b => b.UseMySql(options.DbConnectionStrings, sql => sql.MigrationsAssembly(options.MigrationsAssembly));

                operationalStoreOptions.EnableTokenCleanup = options.EnableTokenCleanup;
                operationalStoreOptions.TokenCleanupInterval = options.TokenCleanupInterval;
            })
            .AddDeveloperSigningCredential()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            .AddProfileService<ProfileService>();

            return services;
        }

        /// <summary>
        /// 基于配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityServer4UseConfig(this IServiceCollection services, Action<IdentityServer4Options> identityServer4OptionsAction = null)
        {
            var options = new IdentityServer4Options();
            identityServer4OptionsAction?.Invoke(options);

            services.AddIdentityServer(o =>
            {
                o.UserInteraction = new IdentityServer4.Configuration.UserInteractionOptions()
                {
                    LoginUrl = options.LoginUrl,
                    LogoutUrl = options.LogoutUrl

                };
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
                .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
                .AddInMemoryClients(IdentityServerConfig.Clients)
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>()
            ;

            return services;
        }


        public static IApplicationBuilder UseIdentityServer4(this IApplicationBuilder app)
        {
            return app.UseIdentityServer();
        }
    }
}
