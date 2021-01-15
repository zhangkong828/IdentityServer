using IdentityServer.Entity;
using IdentityServer.Infrastructure.Security;
using IdentityServer.Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service.Impl
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IdentityContext _identityContext;
        public UserAccountService(IdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public UserAccount AutoRegisterByExternal(string scheme, string externalId, string loginIP, string nickname, string avatar)
        {
            var external = _identityContext.UserAccountExternals.AsNoTracking().Where(x => x.Scheme == scheme && x.ExternalId == externalId).SingleOrDefault();
            if (external != null)
            {
                var oldUser = _identityContext.UserAccounts.Where(x => x.UserId == external.UserId).SingleOrDefault();
                oldUser.NickName = nickname;
                oldUser.Avatar = avatar;

                oldUser.LastLoginIp = loginIP;
                oldUser.LastLoginTime = DateTime.Now;
                _identityContext.SaveChanges();
                return oldUser;
            }

            var userId = ObjectId.Default().NextString();
            _identityContext.UserAccountExternals.Add(new UserAccountExternal()
            {
                UserId = userId,
                Scheme = scheme,
                ExternalId = externalId,
                CreateTime = DateTime.Now
            });

            var user = new UserAccount()
            {
                UserId = userId,
                NickName = nickname,
                Avatar = avatar,
                LastLoginIp = loginIP,
                LastLoginTime = DateTime.Now,
                Status = 0,
                CreateTime = DateTime.Now
            };
            _identityContext.UserAccounts.Add(user);
            _identityContext.SaveChanges();
            return user;
        }

        public UserAccount QueryUserByExternal(string scheme, string externalId)
        {
            var external = _identityContext.UserAccountExternals.AsNoTracking().Where(x => x.Scheme == scheme && x.ExternalId == externalId).SingleOrDefault();
            if (external == null) return null;

            return _identityContext.UserAccounts.Where(x => x.UserId == external.UserId).SingleOrDefault();
        }

        public UserAccount QueryUserByUserId(string userId)
        {
            return _identityContext.UserAccounts.AsNoTracking().FirstOrDefault(x => x.UserId == userId);
        }

        public UserAccount QueryUserByUsername(string username)
        {
            return _identityContext.UserAccounts.AsNoTracking().FirstOrDefault(x => x.Username == username);
        }

        public bool ValidateUsername(string username, string password, string loginIP, out UserAccount user)
        {
            user = _identityContext.UserAccounts.Where(x => x.Username == username || x.Email == username).SingleOrDefault();
            if (user == null) return false;
            if (Md5Helper.Md5By32(password) != user.Password) return false;

            if (!string.IsNullOrWhiteSpace(loginIP))
            {
                user.LastLoginIp = loginIP;
                user.LastLoginTime = DateTime.Now;
                _identityContext.SaveChanges();
            }

            return true;
        }
    }
}
