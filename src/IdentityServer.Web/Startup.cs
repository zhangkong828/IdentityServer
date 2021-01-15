using IdentityServer.Entity;
using IdentityServer.Service;
using IdentityServer.Service.Impl;
using IdentityServer.Web.Data;
using IdentityServer.Web.ExternalLogin.GitHub;
using IdentityServer.Web.ExternalLogin.QQ;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureNonBreakingSameSiteCookies();

            services.AddScoped<IUserAccountService, UserAccountService>();

            services.AddDbContextPool<IdentityContext>(options =>
            {
                options.UseMySql(Config.GetString("ConnectionStrings:IdentityUserConnection"), sql => sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
            });


            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie(options =>
            //{
            //    options.Cookie.IsEssential = true;
            //    options.LoginPath = "/Account/Login";
            //    options.LogoutPath = "/Account/Logout";
            //});

            services.AddAuthentication()
              .AddQQ("qq", "QQ", options =>
              {
                  options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                  options.ClientId = "You App Id";
                  options.ClientSecret = "You App Secret";
              })
              .AddGitHub("github", "Github", options =>
              {
                  options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                  options.ClientId = "03c3f213ec1e4b224c72";
                  options.ClientSecret = "9e56eae44f59189984aef0fedfea6490af9dc1ee";
              });

            //services.AddIdentityServer4UseMySql(options =>
            //{
            //    options.LoginUrl = "/login";
            //    options.LogoutUrl = "/logout";
            //    options.DbConnectionStrings = Config.GetString("ConnectionStrings:IdentityServerConnection");
            //    options.EnableTokenCleanup = true;
            //    options.TokenCleanupInterval = 600;
            //    options.MigrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //});

            services.AddIdentityServer4UseConfig(options =>
            {
                options.LoginUrl = "/login";
                options.LogoutUrl = "/logout";
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase.Init(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseCors("AllowAllOrigins");

            app.UseIdentityServer4();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
