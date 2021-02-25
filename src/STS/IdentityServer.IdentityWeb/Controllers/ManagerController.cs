using IdentityModel;
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

            if (user==null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
