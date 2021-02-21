using IdentityServer.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IdentityServer.EntityFramework.MySql
{
    public static class DatabaseExtensions
    {
        public static void RegisterMySqlDbContexts<TIdentityDbContext, TConfigurationDbContext,TPersistedGrantDbContext>(this IServiceCollection services,string identityConnectionString, string configurationConnectionString,string persistedGrantConnectionString)
            where TIdentityDbContext : DbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<TIdentityDbContext>(options => options.UseMySql(identityConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddConfigurationDbContext<TConfigurationDbContext>(options => options.ConfigureDbContext = b => b.UseMySql(configurationConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddOperationalDbContext<TPersistedGrantDbContext>(options => options.ConfigureDbContext = b => b.UseMySql(persistedGrantConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

        }
    }
}
