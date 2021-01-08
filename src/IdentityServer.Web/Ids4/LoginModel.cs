using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.Ids4
{
    public class LoginFormModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ReturnUrl { get; set; }
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
}
