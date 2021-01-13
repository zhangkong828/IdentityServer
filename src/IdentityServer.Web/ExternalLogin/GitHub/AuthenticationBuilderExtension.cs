using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.GitHub
{
    public static class AuthenticationBuilderExtension
    {
        public static AuthenticationBuilder AddGitHub(
          this AuthenticationBuilder @this,
          string scheme,
          string displayName,
          Action<GitHubOAuthOptions> configureOptions)
        {
            return @this.AddOAuth<GitHubOAuthOptions, GitHubOAuthHandler>(scheme, displayName, configureOptions);
        }
    }
}
