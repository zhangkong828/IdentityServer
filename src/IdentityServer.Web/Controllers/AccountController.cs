using IdentityServer.Web.Constants;
using IdentityServer.Web.Models;
using IdentityServer.Web.Models.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AdminUserConfig _userConfig;
        public AccountController(IOptionsSnapshot<AdminUserConfig> userConfigAccessor)
        {
            _userConfig = userConfigAccessor.Value;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(returnUrl)) returnUrl = "/dashboard";
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginPost(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            var user = _userConfig.Users.Where(x => x.UserName.Equals(request.UserName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (user == null) return Json(new { code = -1, msg = "用户不存在" });
            if (user.Password != request.Password) return Json(new { code = -1, msg = "密码错误" });


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, AuthorizationConsts.AdministrationRole)
            };

            var expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(2));
            if (request.RememberLogin)
                expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(7));

            AuthenticationProperties props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = expires
            };

            var identity = new ClaimsIdentity(claims, AuthorizationConsts.AdministrationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(AuthorizationConsts.AdministrationScheme, principal, props);
            return Json(new { code = 0, msg = "登录成功" });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthorizationConsts.AdministrationScheme);
            return Redirect("/Account/Login");
        }
    }
}
