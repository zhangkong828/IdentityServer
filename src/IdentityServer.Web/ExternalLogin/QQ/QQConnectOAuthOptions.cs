using IdentityServer.Web.ExternalLogin.QQ.QQConnect;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.QQ
{
    public class QQConnectOAuthOptions : OAuthOptions
    {
        public QQConnectOAuthOptions()
        {
            base.AuthorizationEndpoint = "empty";
            base.TokenEndpoint = "empty";
            base.CallbackPath = new PathString("/oauth2/qq-connect/callback");
            base.Scope.Add("get_user_info");
        }

        public bool IsMobile { get; set; } = false;

        internal QQConnectOptions BuildQQConnectOptions(Func<string> redirectUrl)
        {
            return new QQConnectOptions(ClientId, ClientSecret, IsMobile, new HashSet<string>(base.Scope))
            {
                RedirectUrl = redirectUrl
            };
        }
    }
}
