using IdentityServer.EntityFramework;
using IdentityServer.EntityFramework.DbContexts;
using IdentityServer.EntityFramework.MySql;
using IdentityServer.EntityFramework.SqlServer;
using IdentityServer.IdentityAdminWeb.Constants;
using IdentityServer.IdentityAdminWeb.Extensions;
using IdentityServer.IdentityAdminWeb.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            RegisterAuthentication(services);

            services.AddControllersWithViews()
            .AddRazorRuntimeCompilation();
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

            app.UseAuthentication();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

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
        public void RegisterDbContexts(IServiceCollection services)
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

        /// <summary>
        /// 注册身份验证
        /// </summary>
        public void RegisterAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(AuthorizationConsts.AdministrationScheme)
            .AddCookie(AuthorizationConsts.AdministrationScheme, options =>
            {
                options.Cookie.Name = AuthorizationConsts.AdministrationScheme;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;

                options.AccessDeniedPath = "/Account/Login";
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });
        }

        /// <summary>
        /// 注册MVC、多语言
        /// </summary>
        public void AddMvcWithLocalization(IServiceCollection services)
        {
            var resourcesPath = "Resources";

            services.AddLocalization(opts => { opts.ResourcesPath = resourcesPath; });

            services.TryAddTransient(typeof(IGenericControllerLocalizer<>), typeof(GenericControllerLocalizer<>));

            services.AddControllersWithViews()
            .AddRazorRuntimeCompilation()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = resourcesPath; })
            .AddDataAnnotationsLocalization();

            var cultureConfiguration = new CultureConfiguration();
            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultureCodes = CultureConfiguration.AvailableCultures;
                var supportedCultures = supportedCultureCodes.Select(c => new CultureInfo(c)).ToList();

                opts.DefaultRequestCulture = new RequestCulture(CultureConfiguration.DefaultRequestCulture);
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });
        }
    }
}
