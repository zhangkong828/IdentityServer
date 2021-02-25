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
            return DbContext.UserIdentity.AsNoTracking().Where(x => x.Email == username || x.PhoneNumber == username).SingleOrDefault();
        }

        public bool AddUser(UserIdentity user)
        {
            DbContext.UserIdentity.Add(user);
            return DbContext.SaveChanges() > 0;
        }

        public bool UpdateLastLoginState(long id, string ip, DateTime datetime)
        {
            var user = DbContext.UserIdentity.Find(id);
            if (user == null) return false;

            user.LastLoginIp = ip;
            user.LastLoginTime = datetime;
            return DbContext.SaveChanges() > 0;
        }

        public UserIdentityExternal QueryExternal(string scheme, string externalId)
        {
            return DbContext.UserIdentityExternal.AsNoTracking().Where(x => x.Scheme == scheme && x.ExternalId == externalId).SingleOrDefault();
        }

        public bool AddExternalUser(UserIdentityExternal external, UserIdentity user)
        {
            DbContext.UserIdentity.Add(user);
            DbContext.UserIdentityExternal.Add(external);
            return DbContext.SaveChanges() > 0;
        }
    }
}
