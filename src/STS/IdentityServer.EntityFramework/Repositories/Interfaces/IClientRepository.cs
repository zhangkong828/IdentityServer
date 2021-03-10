using IdentityServer.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.EntityFramework.Repositories.Interfaces
{
    public interface IClientRepository
    {
		Task<int> AddClientAsync(Client client);

		Task<int> UpdateClientAsync(Client client);

		Task<int> RemoveClientAsync(Client client);

		Task<int> CloneClientAsync(Client client, string clientId, string clientName);

		Task<bool> CanInsertClientAsync(Client client, bool isCloned = false);

		Task<Client> GetClientAsync(int clientId);

		Task<(string ClientId, string ClientName)> GetClientIdAsync(int clientId);

		Task<PageData<Client>> GetClientsAsync(string search = "", int page = 1, int pageSize = 10);

		Task<List<string>> GetScopesAsync(string scope, int limit = 0);

		List<string> GetGrantTypes(string grant, int limit = 0);

		List<SelectItem> GetProtocolTypes();

		List<SelectItem> GetAccessTokenTypes();

		List<SelectItem> GetTokenExpirations();

		List<SelectItem> GetTokenUsage();

		List<SelectItem> GetHashTypes();

		List<SelectItem> GetSecretTypes();

		List<string> GetStandardClaims(string claim, int limit = 0);

		Task<int> AddClientSecretAsync(int clientId, ClientSecret clientSecret);

		Task<int> DeleteClientSecretAsync(int clientSecretId);

		Task<PageData<ClientSecret>> GetClientSecretsAsync(int clientId, int page = 1, int pageSize = 10);

		Task<ClientSecret> GetClientSecretAsync(int clientSecretId);

		Task<PageData<ClientClaim>> GetClientClaimsAsync(int clientId, int page = 1, int pageSize = 10);

		Task<PageData<ClientProperty>> GetClientPropertiesAsync(int clientId, int page = 1, int pageSize = 10);

		Task<ClientClaim> GetClientClaimAsync(int clientClaimId);

		Task<ClientProperty> GetClientPropertyAsync(int clientPropertyId);

		Task<int> AddClientClaimAsync(int clientId, ClientClaim clientClaim);

		Task<int> AddClientPropertyAsync(int clientId, ClientProperty clientProperties);

		Task<int> DeleteClientClaimAsync(int clientClaimId);

		Task<int> DeleteClientPropertyAsync(int clientPropertyId);

		Task<int> SaveAllChangesAsync();
	}
}
