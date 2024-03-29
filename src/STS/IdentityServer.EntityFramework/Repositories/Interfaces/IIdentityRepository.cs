﻿using IdentityServer.EntityFramework.Entities;
using IdentityServer.EntityFramework.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.EntityFramework.Repositories.Interfaces
{
    public interface IIdentityRepository
    {
        Task<PageData<UserIdentity>> QueryUsersAsync(string search, int page = 1, int pageSize = 10);

        public UserIdentityExternal QueryExternal(string scheme, string externalId);

        public bool AddExternalUser(UserIdentityExternal external, UserIdentity user);

        public UserIdentity QueryUserByUserId(string userId);

        public UserIdentity QueryUserByUsername(string username);

        public UserIdentity QueryUserByExternal(string scheme, string externalId);

        public bool AddUser(UserIdentity user);

        public bool UpdateLastLoginState(long id, string ip, DateTime datetime);

        public bool UpdateUserPassword(string userId, string newPassword);
    }
}
