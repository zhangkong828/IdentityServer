using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger = Log.GetLogger("Exception");
        public GlobalExceptionFilter()
        {

        }

        public void OnException(ExceptionContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var path = context.HttpContext.Request.Path;
            var queryString = context.HttpContext.Request.QueryString.Value;
            _logger.Error($"\r\n[url]:{path + queryString}\r\n[controller]:{controller}\r\n[action]:{action}", context.Exception);

            context.HttpContext.Response.StatusCode = 500;

            context.ExceptionHandled = true;

            bool IsAjax = false;
            if (context.HttpContext.Request.Headers.ContainsKey("x-requested-with"))
            {
                IsAjax = context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest" ? true : false;
            }

            if (!IsAjax)
            {
                context.Result = new RedirectResult("/");
            }
            else
            {
                context.Result = new JsonResult(new { code = 500, msg = context.Exception.Message });
            }
        }
    }
}
