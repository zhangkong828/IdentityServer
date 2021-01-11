using IdentityModel.Client;
using IdentityServer.HttpDiagnostic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Client.ClientCredentials
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

            return await httpClient.RequestTokenAsync(new TokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = "Test.ClientCredentials",
                ClientSecret = "123456",
                GrantType = "client_credentials",
                Parameters =
                {
                    ["scope"]="scope1"
                }
            });
        }
    }
}
