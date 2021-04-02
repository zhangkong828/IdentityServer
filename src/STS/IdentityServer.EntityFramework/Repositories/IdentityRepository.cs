using IdentityServer.EntityFramework.Entities;
using IdentityServer.EntityFramework.Entities.Identity;
using IdentityServer.EntityFramework.Extensions;
using IdentityServer.EntityFramework.Interfaces;
using IdentityServer.EntityFramework.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public bool UpdateUserPassword(string userId, string newPassword)
        {
            var user = DbContext.UserIdentity.SingleOrDefault(x => x.UserId == userId);
            if (user == null) return false;

            user.Password = newPassword;
            return DbContext.SaveChanges() > 0;
        }

        public async Task<PageData<UserIdentity>> QueryUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PageData<UserIdentity>();

            Expression<Func<UserIdentity, bool>> searchCondition = x => x.UserId.Contains(search) || x.Username.Contains(search) || x.NickName.Contains(search) || x.Email.Contains(search);

            var identities = await DbContext.UserIdentity.WhereIf(!string.IsNullOrEmpty(search), searchCondition).PageBy(x => x.LastLoginTime, page, pageSize).ToListAsync();

            pagedList.List.AddRange(identities);
            pagedList.TotalCount = await DbContext.UserIdentity.WhereIf(!string.IsNullOrEmpty(search), searchCondition).CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.PageIndex = page;
            return pagedList;
        }
    }
}
