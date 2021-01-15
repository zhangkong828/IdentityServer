using IdentityServer.Web.ExternalLogin.QQ.QQConnect.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.QQ.QQConnect.Response
{
    public sealed class TokenResponse
    {
        private TokenResponse()
        {
        }

        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }

        public string ExpiresIn { get; private set; }

        public static TokenResponse From(string urlEncodedToken)
        {
            //！！！Content-Type是text/html,格式却是form-urlencoded
            //access_token=YOUR_ACCESS_TOKEN&expires_in=3600
            var keyValues = new Dictionary<string, string>();
            foreach (var param in urlEncodedToken.Split('&'))
            {
                var keyValue = param.Split('=');
                keyValues.Add(keyValue[0], keyValue[1]);
            }
            return new TokenResponse
            {
                AccessToken = keyValues.TryGetValue("access_token"),
                RefreshToken = keyValues.TryGetValue("refresh_token"),
                ExpiresIn = keyValues.TryGetValue("expires_in")
            };
        }
    }
}
