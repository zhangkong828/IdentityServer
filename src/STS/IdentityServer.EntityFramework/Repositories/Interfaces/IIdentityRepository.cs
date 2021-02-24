using IdentityServer.EntityFramework.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.EntityFramework.Repositories.Interfaces
{
    public interface IIdentityRepository
    {
        public UserIdentity QueryUserByUserId(string userId);

        public UserIdentity QueryUserByUsername(string username);

        public UserIdentity QueryUserByExternal(string scheme, string externalId);

        public bool AddUser(UserIdentity user);

        public bool UpdateLastLoginState(long id,string ip,DateTime datetime);
    }
}
