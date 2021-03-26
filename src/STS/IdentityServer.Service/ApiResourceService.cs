using IdentityServer.EntityFramework.Enums;
using IdentityServer.EntityFramework.Repositories.Interfaces;
using IdentityServer.Service.Dtos.Configuration;
using IdentityServer.Service.Interfaces;
using IdentityServer.Service.Mappers;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service
{
    public class ApiResourceService : IApiResourceService
    {
        protected readonly IApiResourceRepository ApiResourceRepository;
        private const string SharedSecret = "SharedSecret";
        public ApiResourceService(IApiResourceRepository apiResourceRepository)
        {
            ApiResourceRepository = apiResourceRepository;
        }

        public async Task<int> AddApiResourceAsync(ApiResourceDto apiResource)
        {
            var canInsert = await CanInsertApiResourceAsync(apiResource);
            if (!canInsert)
            {
                return -1;
            }

            var resource = apiResource.ToEntity();

            var added = await ApiResourceRepository.AddApiResourceAsync(resource);

            return added;
        }

        public async Task<int> AddApiResourcePropertyAsync(int apiResourceId, ApiResourcePropertyDto apiResourcePropertyDto)
        {
            var canInsert =await CanInsertApiResourcePropertyAsync(apiResourcePropertyDto);
            if (!canInsert)
            {
                return -1;
            }
            var apiResourceProperty = apiResourcePropertyDto.ToEntity();

            var saved = await ApiResourceRepository.AddApiResourcePropertyAsync(apiResourceId, apiResourceProperty);

            return saved;
        }

        public async Task<int> AddApiResourceSecretAsync(int apiResourceId, ApiResourceSecretDto apiSecret)
        {
            if (apiSecret.Type == SharedSecret)
            {
                apiSecret.Value = apiSecret.Value.Sha256();
            }

            var secret = apiSecret.ToEntity();

            var added = await ApiResourceRepository.AddApiResourceSecretAsync(apiResourceId, secret);

            return added;
        }

        public async Task<bool> CanInsertApiResourceAsync(ApiResourceDto apiResource)
        {
            var resource = apiResource.ToEntity();

            return await ApiResourceRepository.CanInsertApiResourceAsync(resource);
        }

        public async Task<bool> CanInsertApiResourcePropertyAsync(ApiResourcePropertyDto apiResourceProperty)
        {
            var resource = apiResourceProperty.ToEntity();

            return await ApiResourceRepository.CanInsertApiResourcePropertyAsync(resource);
        }

        public async Task<int> DeleteApiResourceAsync(int apiResourceId)
        {
            var deleted = await ApiResourceRepository.DeleteApiResourceAsync(apiResourceId);

            return deleted;
        }

        public async Task<int> DeleteApiResourcePropertyAsync(int apiResourcePropertyId)
        {
            var deleted = await ApiResourceRepository.DeleteApiResourcePropertyAsync(apiResourcePropertyId);

            return deleted;
        }


        public async Task<int> DeleteApiResourceSecretAsync(int apiSecretId)
        {
            var deleted = await ApiResourceRepository.DeleteApiResourceSecretAsync(apiSecretId);

            return deleted;
        }

        public async Task<ApiResourceDto> GetApiResourceAsync(int apiResourceId)
        {
            var apiResource = await ApiResourceRepository.GetApiResourceAsync(apiResourceId);
            if (apiResource == null) return null;

            var apiResourceDto = apiResource.ToModel();

            return apiResourceDto;
        }

        public async Task<string> GetApiResourceNameAsync(int apiResourceId)
        {
            return await ApiResourceRepository.GetApiResourceNameAsync(apiResourceId);
        }

        public async Task<ApiResourcePropertiesDto> GetApiResourcePropertiesAsync(int apiResourceId, int page = 1, int pageSize = 10)
        {
            var apiResource = await ApiResourceRepository.GetApiResourceAsync(apiResourceId);
            if (apiResource == null) return null;

            var pagedList = await ApiResourceRepository.GetApiResourcePropertiesAsync(apiResourceId, page, pageSize);
            var apiResourcePropertiesDto = pagedList.ToModel();
            apiResourcePropertiesDto.ApiResourceId = apiResourceId;
            apiResourcePropertiesDto.ApiResourceName = await ApiResourceRepository.GetApiResourceNameAsync(apiResourceId);

            return apiResourcePropertiesDto;
        }

        public async Task<ApiResourcePropertiesDto> GetApiResourcePropertyAsync(int apiResourcePropertyId)
        {
            var apiResourceProperty = await ApiResourceRepository.GetApiResourcePropertyAsync(apiResourcePropertyId);
            if (apiResourceProperty == null) return null;

            var apiResourcePropertiesDto = apiResourceProperty.ToModel();
            apiResourcePropertiesDto.ApiResourceId = apiResourceProperty.ApiResourceId;
            apiResourcePropertiesDto.ApiResourceName = await ApiResourceRepository.GetApiResourceNameAsync(apiResourceProperty.ApiResourceId);

            return apiResourcePropertiesDto;
        }

        public async Task<ApiResourcesDto> GetApiResourcesAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await ApiResourceRepository.GetApiResourcesAsync(search, page, pageSize);
            var apiResourcesDto = pagedList.ToModel();

            return apiResourcesDto;
        }


        public async Task<ApiResourceSecretDto> GetApiResourceSecretAsync(int apiSecretId)
        {
            var apiSecret = await ApiResourceRepository.GetApiResourceSecretAsync(apiSecretId);
            if (apiSecret == null) return null;
            var apiSecretsDto = apiSecret.ToModel();

            return apiSecretsDto;
        }

        public async Task<ApiResourceSecretsDto> GetApiResourceSecretsAsync(int apiResourceId, int page = 1, int pageSize = 10)
        {
            var apiResource = await ApiResourceRepository.GetApiResourceAsync(apiResourceId);
            if (apiResource == null) return null;

            var pagedList = await ApiResourceRepository.GetApiResourceSecretsAsync(apiResourceId, page, pageSize);

            var apiSecretsDto = pagedList.ToModel();

            return apiSecretsDto;
        }

        public async Task<int> UpdateApiResourceAsync(ApiResourceDto apiResource)
        {
            var canInsert = await CanInsertApiResourceAsync(apiResource);
            if (!canInsert)
            {
                return -1;
            }

            var resource = apiResource.ToEntity();

            var originalApiResource = await GetApiResourceAsync(apiResource.Id);

            var updated = await ApiResourceRepository.UpdateApiResourceAsync(resource);

            return updated;
        }
    }
}
