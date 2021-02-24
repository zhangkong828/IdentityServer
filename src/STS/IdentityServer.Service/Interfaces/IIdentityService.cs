using IdentityServer.EntityFramework.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Interfaces
{
    public interface IIdentityService
    {
        public bool ValidateUsername(string username, string password, string loginIP, out UserIdentity user);

        public UserIdentity QueryUserByUserId(string userId);

        public UserIdentity QueryUserByUsername(string username);

        public UserIdentity QueryUserByExternal(string scheme, string externalId);

        public UserIdentity AutoRegisterByExternal(string scheme, string externalId, string loginIP, string nickname, string avatar);

        public bool EmailRegister(string nickName, string username, string password, string loginIP);
    }
}
