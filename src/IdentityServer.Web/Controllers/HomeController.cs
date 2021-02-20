using IdentityServer.Web.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        public ActionResult Index()
        {
            string authority = new Uri(this.Request.GetDisplayUrl())
                .GetLeftPart(UriPartial.Authority);

            return Redirect(authority + "/.well-known/openid-configuration");

            //return Json(new Dictionary<string, object>
            //{
            //    ["oidc_discovery_url"] = authority + "/.well-known/openid-configuration",
            //    //["debug"] = new Dictionary<string, string>
            //    //{
            //    //    ["client"] = this.Url.Link("debug.client.get", null),
            //    //    ["user"] = this.Url.Link("debug.user.get", null),
            //    //    ["user_claim"] = this.Url.Link("debug.user_claim.get", null),
            //    //    ["api_scope"] = this.Url.Link("debug.api_scope.get", null),
            //    //    ["api_resource"] = this.Url.Link("debug.api_resource.get", null),
            //    //    ["identity_resource"] = this.Url.Link("debug.identity_resource.get", null)
            //    //}
            //});
        }

        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel2();

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
