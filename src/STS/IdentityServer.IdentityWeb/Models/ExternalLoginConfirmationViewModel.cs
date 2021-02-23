using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityWeb.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        public string NickName { get; set; }

        public string Avatar { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
