using IdentityServer.Entity;
using IdentityServer.Infrastructure.Security;
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


        public UserAccount QueryUserByUserId(string userId)
        {
            return _identityContext.UserAccounts.AsNoTracking().FirstOrDefault(x => x.UserId == userId);
        }

        public UserAccount QueryUserByUsername(string username)
        {
            return _identityContext.UserAccounts.AsNoTracking().FirstOrDefault(x => x.Username == username);
        }

        public bool ValidateCredentials(string username, string password, string loginIP, out UserAccount user)
        {
            user = _identityContext.UserAccounts.FirstOrDefault(x => x.Username == username);
            if (user == null) return false;
            if (Md5Helper.Md5By32(password) != user.Password) return false;

            user.LastLoginIp = loginIP;
            user.LastLoginTime = DateTime.Now;
            _identityContext.SaveChanges();
            return true;
        }
    }
}
