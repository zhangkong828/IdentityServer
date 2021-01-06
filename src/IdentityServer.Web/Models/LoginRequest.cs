using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.Models
{
    public class LoginRequest
    {
        public string ReturnUrl { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
    }
}
