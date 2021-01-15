using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Entity
{
    public class UserAccount
    {
        public long Id { get; set; }

        [StringLength(60)]
        public string UserId { get; set; }

        [StringLength(60)]
        public string Username { get; set; }

        [StringLength(60)]
        public string Password { get; set; }

        [StringLength(60)]
        public string NickName { get; set; }

        [StringLength(200)]
        public string Avatar { get; set; }

        [StringLength(60)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneCode { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(30)]
        public string LastLoginIp { get; set; }

        public DateTime LastLoginTime { get; set; }

        [Required]
        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
