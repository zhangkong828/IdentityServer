using IdentityServer.Service.Dtos.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service.Interfaces
{
    public interface IApiResourceService
    {
        Task<ApiResourcesDto> GetApiResourcesAsync(string search, int page = 1, int pageSize = 10);

        Task<ApiResourceDto> GetApiResourceAsync(int apiResourceId); 
        Task<int> AddApiResourceAsync(ApiResourceDto apiResource);

        Task<int> UpdateApiResourceAsync(ApiResourceDto apiResource);

        Task<int> DeleteApiResourceAsync(int apiResourceId);

        Task<bool> CanInsertApiResourceAsync(ApiResourceDto apiResource);



        Task<ApiResourcePropertiesDto> GetApiResourcePropertiesAsync(int apiResourceId, int page = 1, int pageSize = 10);

        Task<ApiResourcePropertiesDto> GetApiResourcePropertyAsync(int apiResourcePropertyId);

        Task<int> AddApiResourcePropertyAsync(int apiResourceId, ApiResourcePropertyDto apiResourceProperty);

        Task<int> DeleteApiResourcePropertyAsync(int apiResourcePropertyId);

        Task<bool> CanInsertApiResourcePropertyAsync(ApiResourcePropertiesDto apiResourceProperty);




        Task<ApiResourceSecretsDto> GetApiResourceSecretsAsync(int apiResourceId, int page = 1, int pageSize = 10);

        Task<int> AddApiResourceSecretAsync(int apiResourceId, ApiResourceSecretDto apiSecret);

        Task<ApiResourceSecretDto> GetApiResourceSecretAsync(int apiSecretId);

        Task<int> DeleteApiResourceSecretAsync(int apiSecretId);


        Task<string> GetApiResourceNameAsync(int apiResourceId);
    }
}
