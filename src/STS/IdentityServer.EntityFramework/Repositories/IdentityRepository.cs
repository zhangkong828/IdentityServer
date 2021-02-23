using IdentityServer.EntityFramework.Entities.Identity;
using IdentityServer.EntityFramework.Interfaces;
using IdentityServer.EntityFramework.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityServer.EntityFramework.Repositories
{
    public class IdentityRepository<TDbContext> : IIdentityRepository
    where TDbContext : DbContext, IIdentityDbContext
    {
        protected readonly TDbContext DbContext;
        public bool AutoSaveChanges { get; set; } = true;

        public IdentityRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public UserIdentity AutoRegisterByExternal(UserIdentity user)
        {
            throw new NotImplementedException();
        }

        public UserIdentity QueryUserByExternal(string scheme, string externalId)
        {
            var external = DbContext.UserIdentityExternal.AsNoTracking().Where(x => x.Scheme == scheme && x.ExternalId == externalId).SingleOrDefault();
            if (external == null) return null;

            return DbContext.UserIdentity.Where(x => x.UserId == external.UserId).SingleOrDefault();
        }

        public UserIdentity QueryUserByUserId(string userId)
        {
            return DbContext.UserIdentity.AsNoTracking().FirstOrDefault(x => x.UserId == userId);
        }

        public UserIdentity QueryUserByUsername(string username)
        {
            return DbContext.UserIdentity.AsNoTracking().FirstOrDefault(x => x.Username == username);
        }
    }
}
