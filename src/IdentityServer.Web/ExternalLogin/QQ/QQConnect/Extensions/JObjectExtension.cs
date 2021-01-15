using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.QQ.QQConnect.Extensions
{
    public static class JObjectExtension
    {
        public static string TryGetValue(this JObject @this, string name)
        {
            if (@this == null)
            {
                return null;
            }

            if (@this.TryGetValue(name, out var value))
            {
                return value.ToString();
            }

            return null;
        }
    }
}
