using IdentityServer.EntityFramework.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.EntityFramework.Interfaces
{
    public interface IIdentityDbContext
    {
         DbSet<UserIdentity> UserIdentity { get; set; }
         DbSet<UserIdentityExternal> UserIdentityExternal { get; set; }
    }
}
