using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
                {
                    new ApiResource("api", "Some API")
                    {
                        Scopes=ApiScopes.Select(x=>x.Name).ToList()
                    },
                    new ApiResource("GoofyAlgoTraderAPI", "GoofyAlgoTrader API")
                };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
                    new ApiScope("api1", "Test API"),
                    new ApiScope("GoofyAlgoTraderAPI", "GoofyAlgoTrader API")
        };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "Test.ClientCredentials",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },

                    AllowedScopes = { "api1" }
                },

                new Client
                {
                    ClientId = "Test.ResourceOwnerPassword",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                     AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },
                    AllowOfflineAccess=true
                },

                new Client
                {
                    ClientId = "Test.Code",
                    ClientSecrets = { new Secret("123456".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                }
            };


        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "test",
                        Password = "123456",

                        Claims = new List<Claim>
                        {
                            new Claim("name", "test"),
                            new Claim("website", "https://izk.cloud")
                        }
                    }
                };
        }
    }
}
