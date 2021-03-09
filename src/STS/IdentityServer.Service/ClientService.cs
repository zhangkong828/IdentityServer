using IdentityServer.EntityFramework.Entities;
using IdentityServer.EntityFramework.Enums;
using IdentityServer.EntityFramework.Repositories.Interfaces;
using IdentityServer.Service.Dtos.Configuration;
using IdentityServer.Service.Dtos.Enums;
using IdentityServer.Service.Helpers;
using IdentityServer.Service.Interfaces;
using IdentityServer.Service.Mappers;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service
{
    public class ClientService : IClientService
    {
        protected readonly IClientRepository ClientRepository;
        private const string SharedSecret = "SharedSecret";

        public ClientService(IClientRepository clientRepository)
        {
            ClientRepository = clientRepository;
        }

        private string GetClientName(string clientId, string clientName)
        {
            return $"{clientId} ({clientName})";
        }

        public ClientDto BuildClientViewModel(ClientDto client = null)
        {
            if (client == null)
            {
                var clientDto = new ClientDto
                {
                    AccessTokenTypes = GetAccessTokenTypes(),
                    RefreshTokenExpirations = GetTokenExpirations(),
                    RefreshTokenUsages = GetTokenUsage(),
                    ProtocolTypes = GetProtocolTypes(),
                    Id = 0
                };

                return clientDto;
            }

            client.AccessTokenTypes = GetAccessTokenTypes();
            client.RefreshTokenExpirations = GetTokenExpirations();
            client.RefreshTokenUsages = GetTokenUsage();
            client.ProtocolTypes = GetProtocolTypes();

            ComboBoxHelpers.PopulateValuesToList(client.AllowedScopesItems, client.AllowedScopes);
            ComboBoxHelpers.PopulateValuesToList(client.PostLogoutRedirectUrisItems, client.PostLogoutRedirectUris);
            ComboBoxHelpers.PopulateValuesToList(client.IdentityProviderRestrictionsItems, client.IdentityProviderRestrictions);
            ComboBoxHelpers.PopulateValuesToList(client.RedirectUrisItems, client.RedirectUris);
            ComboBoxHelpers.PopulateValuesToList(client.AllowedCorsOriginsItems, client.AllowedCorsOrigins);
            ComboBoxHelpers.PopulateValuesToList(client.AllowedGrantTypesItems, client.AllowedGrantTypes);

            return client;
        }

        public ClientCloneDto BuildClientCloneViewModel(int id, ClientDto clientDto)
        {
            var client = new ClientCloneDto
            {
                CloneClientCorsOrigins = true,
                CloneClientGrantTypes = true,
                CloneClientIdPRestrictions = true,
                CloneClientPostLogoutRedirectUris = true,
                CloneClientRedirectUris = true,
                CloneClientScopes = true,
                CloneClientClaims = true,
                CloneClientProperties = true,
                ClientIdOriginal = clientDto.ClientId,
                ClientNameOriginal = clientDto.ClientName,
                Id = id
            };

            return client;
        }

        public async Task<int> AddClientAsync(ClientDto client)
        {
            var canInsert = await CanInsertClientAsync(client);
            if (!canInsert)
            {
                return -1;
            }

            switch (client.ClientType)
            {
                case ClientType.Empty:
                    break;
                case ClientType.WebHybrid:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Hybrid);
                    break;
                case ClientType.Spa:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    break;
                case ClientType.Native:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Hybrid);
                    break;
                case ClientType.Machine:
                    client.AllowedGrantTypes.AddRange(GrantTypes.ResourceOwnerPasswordAndClientCredentials);
                    break;
                case ClientType.Device:
                    client.AllowedGrantTypes.AddRange(GrantTypes.DeviceFlow);
                    client.RequireClientSecret = false;
                    client.AllowOfflineAccess = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var clientEntity = client.ToEntity();

            var added = await ClientRepository.AddClientAsync(clientEntity);

            return added;
        }

        public async Task<int> UpdateClientAsync(ClientDto client)
        {
            var canInsert = await CanInsertClientAsync(client);
            if (!canInsert)
            {
                return -1;
            }

            var clientEntity = client.ToEntity();

            var updated = await ClientRepository.UpdateClientAsync(clientEntity);

            return updated;
        }

        public async Task<int> RemoveClientAsync(ClientDto client)
        {
            var clientEntity = client.ToEntity();

            var deleted = await ClientRepository.RemoveClientAsync(clientEntity);

            return deleted;
        }

        public async Task<int> CloneClientAsync(string originalClientId, string clientId, string clientName)
        {
            var canInsert = await CanInsertClientAsync(client, true);
            if (!canInsert)
            {
                var clientInfo = await ClientRepository.GetClientIdAsync(client.Id);
                client.ClientIdOriginal = clientInfo.ClientId;
                client.ClientNameOriginal = clientInfo.ClientName;

                return -1;
            }

            var clientEntity = client.ToEntity();

            var clonedClientId = await ClientRepository.CloneClientAsync(clientEntity, client.CloneClientCorsOrigins,
                client.CloneClientGrantTypes, client.CloneClientIdPRestrictions,
                client.CloneClientPostLogoutRedirectUris,
                client.CloneClientScopes, client.CloneClientRedirectUris, client.CloneClientClaims, client.CloneClientProperties);

            return clonedClientId;
        }

        public Task<bool> CanInsertClientAsync(ClientDto client, bool isCloned = false)
        {
            var clientEntity = client.ToEntity();

            return ClientRepository.CanInsertClientAsync(clientEntity, isCloned);
        }

        public async Task<ClientDto> GetClientAsync(int clientId)
        {
            var client = await ClientRepository.GetClientAsync(clientId);

            if (client == null) return null;

            var clientDto = client.ToModel();

            return clientDto;
        }

        public async Task<ClientsDto> GetClientsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await ClientRepository.GetClientsAsync(search, page, pageSize);
            var clientsDto = pagedList.ToModel();

            return clientsDto;
        }

        public async Task<List<string>> GetScopesAsync(string scope, int limit = 0)
        {
            var scopes = await ClientRepository.GetScopesAsync(scope, limit);

            return scopes;
        }

        public List<string> GetGrantTypes(string grant, int limit = 0)
        {
            var grantTypes = ClientRepository.GetGrantTypes(grant, limit);

            return grantTypes;
        }

        public List<SelectItem> GetAccessTokenTypes()
        {
            var accessTokenTypes = ClientRepository.GetAccessTokenTypes();

            return accessTokenTypes;
        }

        public List<SelectItem> GetTokenExpirations()
        {
            var tokenExpirations = ClientRepository.GetTokenExpirations();

            return tokenExpirations;
        }

        public List<SelectItem> GetTokenUsage()
        {
            var tokenUsage = ClientRepository.GetTokenUsage();

            return tokenUsage;
        }

        public List<SelectItem> GetHashTypes()
        {
            var hashTypes = ClientRepository.GetHashTypes();

            return hashTypes;
        }

        public List<SelectItem> GetSecretTypes()
        {
            var secretTypes = ClientRepository.GetSecretTypes();

            return secretTypes;
        }

        public List<string> GetStandardClaims(string claim, int limit = 0)
        {
            var standardClaims = ClientRepository.GetStandardClaims(claim, limit);

            return standardClaims;
        }

        public async Task<int> AddClientSecretAsync(int clientId, ClientSecretDto clientSecret)
        {
            clientSecret.Value = clientSecret.Value.Sha256();

            var clientSecretEntity = clientSecret.ToEntity();
            var added = await ClientRepository.AddClientSecretAsync(clientId, clientSecretEntity);

            return added;
        }

        public async Task<int> DeleteClientSecretAsync(int clientSecretId)
        {
            var deleted = await ClientRepository.DeleteClientSecretAsync(clientSecretId);

            return deleted;
        }


        public async Task<ClientClaimsDto> GetClientClaimsAsync(int clientId, int page = 1, int pageSize = 10)
        {
            var clientInfo = await ClientRepository.GetClientIdAsync(clientId);
            if (clientInfo.ClientId == null) return null;

            var pagedList = await ClientRepository.GetClientClaimsAsync(clientId, page, pageSize);
            var clientClaimsDto = pagedList.ToModel();
            clientClaimsDto.ClientId = clientId;
            clientClaimsDto.ClientName = GetClientName(clientInfo.ClientId, clientInfo.ClientName);


            return clientClaimsDto;
        }

        public async Task<ClientPropertiesDto> GetClientPropertiesAsync(int clientId, int page = 1, int pageSize = 10)
        {
            var clientInfo = await ClientRepository.GetClientIdAsync(clientId);
            if (clientInfo.ClientId == null) return null;

            var pagedList = await ClientRepository.GetClientPropertiesAsync(clientId, page, pageSize);
            var clientPropertiesDto = pagedList.ToModel();
            clientPropertiesDto.ClientId = clientId;
            clientPropertiesDto.ClientName = GetClientName(clientInfo.ClientId, clientInfo.ClientName);

            return clientPropertiesDto;
        }

        public async Task<ClientClaimsDto> GetClientClaimAsync(int clientClaimId)
        {
            var clientClaim = await ClientRepository.GetClientClaimAsync(clientClaimId);
            if (clientClaim == null) return null;

            var clientInfo = await ClientRepository.GetClientIdAsync(clientClaim.Client.Id);
            if (clientInfo.ClientId == null) return null;

            var clientClaimsDto = clientClaim.ToModel();
            clientClaimsDto.ClientId = clientClaim.Client.Id;
            clientClaimsDto.ClientName = GetClientName(clientInfo.ClientId, clientInfo.ClientName);

            return clientClaimsDto;
        }

        public async Task<ClientPropertiesDto> GetClientPropertyAsync(int clientPropertyId)
        {
            var clientProperty = await ClientRepository.GetClientPropertyAsync(clientPropertyId);
            if (clientProperty == null) return null;

            var clientInfo = await ClientRepository.GetClientIdAsync(clientProperty.Client.Id);
            if (clientInfo.ClientId == null) return null;

            var clientPropertiesDto = clientProperty.ToModel();
            clientPropertiesDto.ClientId = clientProperty.Client.Id;
            clientPropertiesDto.ClientName = GetClientName(clientInfo.ClientId, clientInfo.ClientName);

            return clientPropertiesDto;
        }

        public async Task<int> AddClientClaimAsync(int clientId, ClientClaimDto clientClaim)
        {
            var clientClaimEntity = clientClaim.ToEntity();

            var saved = await ClientRepository.AddClientClaimAsync(clientId, clientClaimEntity);

            return saved;
        }

        public async Task<int> AddClientPropertyAsync(int clientId, ClientPropertyDto clientPropertyDto)
        {
            var clientProperty = clientPropertyDto.ToEntity();

            var saved = await ClientRepository.AddClientPropertyAsync(clientId, clientProperty);

            return saved;
        }

        public async Task<int> DeleteClientClaimAsync(int clientClaimId)
        {
            var deleted = await ClientRepository.DeleteClientClaimAsync(clientClaimId);

            return deleted;
        }

        public async Task<int> DeleteClientPropertyAsync(int clientPropertyId)
        {
            var deleted = await ClientRepository.DeleteClientPropertyAsync(clientPropertyId);

            return deleted;
        }

        public List<SelectItem> GetProtocolTypes()
        {
            var protocolTypes = ClientRepository.GetProtocolTypes();

            return protocolTypes;
        }
    }
}
