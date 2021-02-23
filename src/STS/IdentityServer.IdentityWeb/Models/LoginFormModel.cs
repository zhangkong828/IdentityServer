using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityWeb.Models
{
    public class LoginFormModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ReturnUrl { get; set; }

        public bool Remember { get; set; }
    }

    public class LoginViewModel : LoginFormModel
    {
        public LoginViewModel(LoginFormModel form)
        {
            if (form != null)
            {
                UserName = form.UserName;
                ReturnUrl = form.ReturnUrl;
            }
        }

        public List<ExternalLoginViewModel> ExternalLoginList { get; set; }
    }

    public class ExternalLoginViewModel
    {
        public string DisplayName { get; set; }

        public string LoginUrl { get; set; }
    }
}
