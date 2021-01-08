using IdentityServer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service
{
    public interface IUserAccountService
    {
        public bool ValidateCredentials(string username, string password);

        public UserAccount QueryUserByUserId(string userId);

        public UserAccount QueryUserByUsername(string username);
    }
}
