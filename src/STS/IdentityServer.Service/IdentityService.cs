using IdentityServer.EntityFramework.Entities.Identity;
using IdentityServer.EntityFramework.Repositories.Interfaces;
using IdentityServer.Infrastructure.Security;
using IdentityServer.Infrastructure.Utility;
using IdentityServer.Service.Dtos.Identity;
using IdentityServer.Service.Interfaces;
using IdentityServer.Service.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _identityRepository;
        public IdentityService(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        private string GetAvatarUrl(string email, int size = 150)
        {
            var hash = Md5Helper.Md5By32(email);
            var sizeArg = size > 0 ? $"?s={size}" : "";
            return $"https://www.gravatar.com/avatar/{hash}{sizeArg}";
        }

        public UserIdentityDto AutoRegisterByExternal(string scheme, string externalId, string loginIP, string nickname, string email)
        {
            var dateTimeNow = DateTime.Now;
            var external = _identityRepository.QueryExternal(scheme, externalId);
            if (external != null)
            {
                var oldUser = _identityRepository.QueryUserByUserId(external.UserId);
                _identityRepository.UpdateLastLoginState(oldUser.Id, loginIP, dateTimeNow);
                return oldUser.ToModel();
            }

            var userId = ObjectId.Default().NextString();
            var newExternal = new UserIdentityExternal()
            {
                UserId = userId,
                Scheme = scheme,
                ExternalId = externalId,
                CreateTime = dateTimeNow
            };

            var user = new UserIdentity()
            {
                UserId = userId,
                Username = email,
                Password = "",
                NickName = nickname,
                Avatar = GetAvatarUrl(email),
                Email = email,
                LastLoginIp = loginIP,
                LastLoginTime = dateTimeNow,
                CreateTime = dateTimeNow
            };
            _identityRepository.AddExternalUser(newExternal, user);
            return user.ToModel();
        }

        public UserIdentityDto QueryUserByExternal(string scheme, string externalId)
        {
            var entity = _identityRepository.QueryUserByExternal(scheme, externalId);
            return entity.ToModel();
        }

        public UserIdentityDto QueryUserByUserId(string userId)
        {
            var entity = _identityRepository.QueryUserByUserId(userId);
            return entity.ToModel();
        }

        public UserIdentityDto QueryUserByUsername(string username)
        {
            var entity = _identityRepository.QueryUserByUsername(username);
            return entity.ToModel();
        }

        public bool EmailRegister(string nickName, string username, string password, string loginIP, out UserIdentityDto user)
        {
            var userIdentity = new UserIdentity()
            {
                UserId = ObjectId.Default().NextString(),
                Username = username,
                Password = Md5Helper.Md5By32(password),
                NickName = nickName,
                Avatar = GetAvatarUrl(username),
                Email = username,
                LastLoginIp = loginIP,
                LastLoginTime = DateTime.Now,
                CreateTime = DateTime.Now
            };
            user = userIdentity.ToModel();

            return _identityRepository.AddUser(userIdentity);
        }

        public bool ValidateUsername(string username, string password, string loginIP, out UserIdentityDto user)
        {
            var userIdentity = _identityRepository.QueryUserByUsername(username);
            user = userIdentity.ToModel();

            if (userIdentity == null) return false;

            if (Md5Helper.Md5By32(password) != user.Password) return false;

            _identityRepository.UpdateLastLoginState(user.Id, loginIP, DateTime.Now);
            return true;
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword, out string msg)
        {
            msg = "修改密码成功";
            var user = _identityRepository.QueryUserByUserId(userId);
            if (user == null)
            {
                msg = "用户不存在";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(oldPassword) && !user.Password.Equals(Md5Helper.Md5By32(oldPassword)))
            {
                msg = "旧密码错误";
                return false;
            }
            return _identityRepository.UpdateUserPassword(userId, Md5Helper.Md5By32(newPassword));
        }
    }
}
