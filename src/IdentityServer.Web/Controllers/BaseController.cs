using IdentityServer.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.Controllers
{
    [Authorize(AuthenticationSchemes= AuthorizationConsts.AdministrationScheme, Roles = AuthorizationConsts.AdministrationRole)]
    [TypeFilter(typeof(GlobalExceptionFilter))]
    public class BaseController : Controller
    {

    }
}
