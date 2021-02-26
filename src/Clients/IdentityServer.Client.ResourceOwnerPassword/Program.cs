using IdentityModel.Client;
using IdentityServer.HttpDiagnostic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Client.ResourceOwnerPassword
{
    class Program
    {
        private static HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            DiagnosticListener.AllListeners.Subscribe(new HttpDiagnosticListenerObserver());
            await MainAsync();
            Console.WriteLine("ok");
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            var tokenResponse = await GetTokenAsync();

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var apis = new List<string>
            {
                "http://localhost:26951/api/books/GetAll"
            };

            foreach (var api in apis)
            {
                try
                {
                    await httpClient.GetAsync(api);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static async Task<TokenResponse> GetTokenAsync()
        {
            var discoveryResponse = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "http://localhost:40763",
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            });

            return await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = "Test.ResourceOwnerPassword",
                ClientSecret = "123456",
                UserName = "test",
                Password = "123456",
                Scope = "scope1 offline_access openid profile"
            });
        }
    }
}
