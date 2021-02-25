using IdentityModel;
using IdentityServer.IdentityWeb.Models;
using IdentityServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.IdentityWeb.Controllers
{
    [Authorize]
    [TypeFilter(typeof(GlobalExceptionFilter))]
    public class ManagerController : Controller
    {
        private readonly IIdentityService _identityService;
        public ManagerController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        [Route("/user")]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Subject);
            var user = _identityService.QueryUserByUserId(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet]
        [Route("/user/changepassword")]
        public IActionResult ChangePassword()
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Subject);
            var user = _identityService.QueryUserByUserId(userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [Route("/user/changepassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            var userId = User.FindFirstValue(JwtClaimTypes.Subject);

            var changePasswordResult = _identityService.ChangePassword(userId, model.OldPassword, model.NewPassword, out string msg);

            return Json(new { code = changePasswordResult ? 0 : -1, msg = msg });
        }
    }
}
