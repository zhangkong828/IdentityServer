﻿using IdentityServer.EntityFramework.Entities.Identity;
using IdentityServer.EntityFramework.Repositories.Interfaces;
using IdentityServer.Infrastructure.Security;
using IdentityServer.Infrastructure.Utility;
using IdentityServer.Service.Interfaces;
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

        public UserIdentity AutoRegisterByExternal(string scheme, string externalId, string loginIP, string nickname, string avatar)
        {
            throw new NotImplementedException();
        }

        public UserIdentity QueryUserByExternal(string scheme, string externalId)
        {
            return _identityRepository.QueryUserByExternal(scheme, externalId);
        }

        public UserIdentity QueryUserByUserId(string userId)
        {
            return _identityRepository.QueryUserByUserId(userId);
        }

        public UserIdentity QueryUserByUsername(string username)
        {
            return _identityRepository.QueryUserByUsername(username);
        }

        public bool EmailRegister(string nickName, string username, string password, string loginIP)
        {
            var user = new UserIdentity()
            {
                UserId = ObjectId.Default().NextString(),
                Username = username,
                Password = Md5Helper.Md5By32(password),
                NickName = nickName,
                Avatar = "",
                Email = username,
                LastLoginIp = loginIP,
                LastLoginTime = DateTime.Now,
                CreateTime = DateTime.Now
            };
            return _identityRepository.AddUser(user);
        }

        public bool ValidateUsername(string username, string password, string loginIP, out UserIdentity user)
        {
            user = _identityRepository.QueryUserByUsername(username);

            if (user == null) return false;
            if (Md5Helper.Md5By32(password) != user.Password) return false;

            _identityRepository.UpdateLastLoginState(user.Id, loginIP, DateTime.Now);
            return true;
        }
    }
}
