using IdentityServer.EntityFramework.Entities;
using IdentityServer.EntityFramework.Entities.Identity;
using IdentityServer.Service.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service.Interfaces
{
    public interface IIdentityService
    {
        Task<PageData<UserIdentityDto>> QueryUsersAsync(string search, int page = 1, int pageSize = 10);

        public bool ValidateUsername(string username, string password, string loginIP, out UserIdentityDto user);

        public UserIdentityDto QueryUserByUserId(string userId);

        public UserIdentityDto QueryUserByUsername(string username);

        public UserIdentityDto QueryUserByExternal(string scheme, string externalId);

        public UserIdentityDto AutoRegisterByExternal(string scheme, string externalId, string loginIP, string nickname, string email);

        public bool EmailRegister(string nickName, string username, string password, string loginIP, out UserIdentityDto user);

        public bool ChangePassword(string userId, string oldPassword, string newPassword,out string msg);
    }
}
