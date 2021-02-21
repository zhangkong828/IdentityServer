using IdentityServer.EntityFramework.Common.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.EntityFramework.Common.DbContexts
{
    public class AdminIdentityDbContext : DbContext
    {
        public AdminIdentityDbContext(DbContextOptions<AdminIdentityDbContext> options) : base(options)
        {

        }

        public DbSet<UserIdentity> UserIdentity { get; set; }
        public DbSet<UserIdentityExternal> UserIdentityExternal { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserIdentity>().ToTable(TableConsts.UserIdentity);
            builder.Entity<UserIdentityExternal>().ToTable(TableConsts.UserIdentityExternal);
        }
    }
}
