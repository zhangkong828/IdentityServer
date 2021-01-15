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
        public bool ValidateUsername(string username, string password, string loginIP, out UserAccount user);

        public UserAccount QueryUserByUserId(string userId);

        public UserAccount QueryUserByUsername(string username);

        public UserAccount QueryUserByExternal(string scheme, string externalId);

        public UserAccount AutoRegisterByExternal(string scheme, string externalId, string loginIP,string nickname,string avatar);
    }
}
