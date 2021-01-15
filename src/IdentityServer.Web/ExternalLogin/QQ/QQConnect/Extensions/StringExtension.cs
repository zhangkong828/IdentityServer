using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.QQ.QQConnect.Extensions
{
    public static class StringExtension
    {
        public static string EmptyIfNull(this string @this)
        {
            return @this ?? string.Empty;
        }
    }
}
