using IdentityServer.EntityFramework.Entities;
using IdentityServer.Service.Dtos.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service.Interfaces
{
    public interface IClientService
    {
        ClientDto BuildClientViewModel(ClientDto client = null);

        ClientCloneDto BuildClientCloneViewModel(int id, ClientDto clientDto);

        Task<int> AddClientAsync(ClientDto client);

        Task<int> UpdateClientAsync(ClientDto client);

        Task<int> RemoveClientAsync(ClientDto client);

        Task<int> CloneClientAsync(string originalClientId, string clientId, string clientName);

        Task<bool> CanInsertClientAsync(ClientDto client, bool isCloned = false);

        Task<ClientDto> GetClientAsync(int clientId);

        Task<ClientsDto> GetClientsAsync(string search, int page = 1, int pageSize = 10);

        Task<List<string>> GetScopesAsync(string scope, int limit = 0);

        List<string> GetGrantTypes(string grant, int limit = 0);

        List<SelectItem> GetAccessTokenTypes();

        List<SelectItem> GetTokenExpirations();

        List<SelectItem> GetTokenUsage();

        List<SelectItem> GetHashTypes();

        List<SelectItem> GetSecretTypes();

        List<string> GetStandardClaims(string claim, int limit = 0);

        Task<int> AddClientSecretAsync(int clientId, ClientSecretDto clientSecret);

        Task<int> DeleteClientSecretAsync(int clientSecretId);

        Task<ClientClaimsDto> GetClientClaimsAsync(int clientId, int page = 1, int pageSize = 10);

        Task<ClientPropertiesDto> GetClientPropertiesAsync(int clientId, int page = 1, int pageSize = 10);

        Task<ClientClaimsDto> GetClientClaimAsync(int clientClaimId);

        Task<ClientPropertiesDto> GetClientPropertyAsync(int clientPropertyId);

        Task<int> AddClientClaimAsync(int clientId, ClientClaimDto clientClaim);

        Task<int> AddClientPropertyAsync(int clientId, ClientPropertyDto clientProperty);

        Task<int> DeleteClientClaimAsync(int clientClaimId);

        Task<int> DeleteClientPropertyAsync(int clientPropertyId);

        List<SelectItem> GetProtocolTypes();
    }
}
