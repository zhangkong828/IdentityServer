using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.GitHub
{
    public class GitHubOAuthOptions: OAuthOptions
    {
        public GitHubOAuthOptions()
        {
            base.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
            base.TokenEndpoint = "https://github.com/login/oauth/access_token";
            base.UserInformationEndpoint = "https://api.github.com/user";
            base.CallbackPath = new PathString("/external-login/callback");
            base.Scope.Add("user");
            base.SaveTokens = false;
            base.ClaimActions.Add(new GitHubClaimsAction());
        }
    }
}
