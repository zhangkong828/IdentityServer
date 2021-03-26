using IdentityServer.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.EntityFramework.Repositories.Interfaces
{
    public interface IApiResourceRepository
    {
        Task<PageData<ApiResource>> GetApiResourcesAsync(string search, int page = 1, int pageSize = 10);

        Task<ApiResource> GetApiResourceAsync(int apiResourceId);

        Task<PageData<ApiResourceProperty>> GetApiResourcePropertiesAsync(int apiResourceId, int page = 1, int pageSize = 10);

        Task<ApiResourceProperty> GetApiResourcePropertyAsync(int apiResourcePropertyId);

        Task<int> AddApiResourcePropertyAsync(int apiResourceId, ApiResourceProperty apiResourceProperty);

        Task<int> DeleteApiResourcePropertyAsync(ApiResourceProperty apiResourceProperty);

        Task<bool> CanInsertApiResourcePropertyAsync(ApiResourceProperty apiResourceProperty);

        Task<int> AddApiResourceAsync(ApiResource apiResource);

        Task<int> UpdateApiResourceAsync(ApiResource apiResource);

        Task<int> DeleteApiResourceAsync(ApiResource apiResource);

        Task<bool> CanInsertApiResourceAsync(ApiResource apiResource);

        Task<PageData<ApiResourceSecret>> GetApiResourceSecretsAsync(int apiResourceId, int page = 1, int pageSize = 10);

        Task<int> AddApiResourceSecretAsync(int apiResourceId, ApiResourceSecret apiSecret);

        Task<ApiResourceSecret> GetApiResourceSecretAsync(int apiSecretId);

        Task<int> DeleteApiResourceSecretAsync(int apiSecretId);

        Task<bool> CanInsertApiResourceScopeAsync(ApiResourceScope apiScope);

        Task<int> SaveAllChangesAsync();

        Task<string> GetApiResourceNameAsync(int apiResourceId);
    }
}
