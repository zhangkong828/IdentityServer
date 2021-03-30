using IdentityServer.IdentityWeb.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IIdentityServerInteractionService interaction)
        {
            _logger = logger;
            _interaction = interaction;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);

        }
    }
}
