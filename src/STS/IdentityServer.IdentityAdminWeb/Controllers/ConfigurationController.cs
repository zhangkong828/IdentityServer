using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Controllers
{
    public class ConfigurationController : BaseController
    {
        public ConfigurationController()
        {

        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
