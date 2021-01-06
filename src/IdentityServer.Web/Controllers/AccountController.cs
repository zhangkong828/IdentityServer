using IdentityServer.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController()
        {

        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(returnUrl)) returnUrl = "/";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            //var user = _userConfig.Users.Where(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            //if (user == null) return Json(new { code = -1, msg = "用户不存在" });
            //if (user.Password != password) return Json(new { code = -1, msg = "密码错误" });

            var identity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim("UserId", "1"),
                new Claim("UserName", "2"),
                new Claim("Roles","3")
            });

            var expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(2));
            if (request.RememberLogin)
                expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(7));

            AuthenticationProperties props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = expires
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), props);

            return Json(new { code = 0, msg = "登录成功" });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Account/Login");
        }
    }
}
