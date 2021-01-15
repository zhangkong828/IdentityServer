using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.QQ
{
    public static class AuthenticationBuilderExtension
    {
        public static AuthenticationBuilder AddQQ(
        this AuthenticationBuilder builder,
        string scheme,
        string displayName,
        Action<QQConnectOAuthOptions> configureOptions)
        {
            return builder.AddOAuth<QQConnectOAuthOptions, QQConnectOAuthHandler>(scheme,displayName,configureOptions);
        }
    }
}
