using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.QQ.QQConnect.Extensions
{
    public static class ClaimExtension
    {
        public static string FindFirstValue(this ICollection<Claim> @this, string type)
        {
            return @this?.FirstOrDefault(claim => claim.Type == type)?.Value;
        }
    }
}
