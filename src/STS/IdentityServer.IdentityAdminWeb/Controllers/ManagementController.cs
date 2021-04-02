using IdentityServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Controllers
{
    public class ManagementController : BaseController
    {
        private readonly IIdentityService _identityService;
        public ManagementController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult Users()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryUsers(int pageIndex, int pageSize, string key)
        {
            var pageData = await _identityService.QueryUsersAsync(key, pageIndex, pageSize);
            return Json(new { code = 0, data = pageData });
        }
    }
}
