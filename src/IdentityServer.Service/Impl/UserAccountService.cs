using IdentityServer.Entity;
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

        public bool ValidateCredentials(string username, string password)
        {
            return false;
        }
    }
}
