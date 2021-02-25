using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Dtos.Identity
{
    public class UserIdentityDto
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string NickName { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public string PhoneCode { get; set; }

        public string PhoneNumber { get; set; }

        public string LastLoginIp { get; set; }

        public DateTime LastLoginTime { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
