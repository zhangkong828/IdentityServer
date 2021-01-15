using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.QQ.QQConnect.Extensions
{
    public static class DictionaryExtension
    {
        public static TV TryGetValue<TK, TV>(this IDictionary<TK, TV> dictionary, TK key)
        {
            if (dictionary == null)
            {
                return default(TV);
            }

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return default(TV);
        }
    }
}
