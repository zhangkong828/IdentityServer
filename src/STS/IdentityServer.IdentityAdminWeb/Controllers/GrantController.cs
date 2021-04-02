using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Controllers
{
    public class GrantController : BaseController
    {
        public GrantController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
