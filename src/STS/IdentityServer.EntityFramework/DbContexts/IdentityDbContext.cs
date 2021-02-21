using IdentityServer.EntityFramework.Entities.Identity;
using IdentityServer.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.EntityFramework.DbContexts
{
    public class IdentityDbContext : DbContext, IIdentityDbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
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
