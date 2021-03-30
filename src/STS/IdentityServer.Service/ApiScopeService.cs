using IdentityServer.EntityFramework.Repositories.Interfaces;
using IdentityServer.Service.Dtos.Configuration;
using IdentityServer.Service.Interfaces;
using IdentityServer.Service.Mappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service
{
    public class ApiScopeService : IApiScopeService
    {
        protected readonly IApiScopeRepository ApiScopeRepository;
        public ApiScopeService(IApiScopeRepository apiScopeRepository)
        {
            ApiScopeRepository = apiScopeRepository;
        }

        public async Task<int> AddApiScopeAsync(ApiScopeDto apiScope)
        {
            var canInsert = await CanInsertApiScopeAsync(apiScope);
            if (!canInsert)
            {
                return -1;
            }

            var scope = apiScope.ToEntity();

            var saved = await ApiScopeRepository.AddApiScopeAsync(scope);
            return saved;
        }

        public async Task<int> AddApiScopePropertyAsync(int apiScopeId, ApiScopePropertyDto apiScopePropertyDto)
        {
            var apiScopeProperty = apiScopePropertyDto.ToEntity();

            var added = await ApiScopeRepository.AddApiScopePropertyAsync(apiScopeId, apiScopeProperty);

            return added;
        }

        public async Task<bool> CanInsertApiScopeAsync(ApiScopeDto apiScope)
        {
            var scope = apiScope.ToEntity();

            return await ApiScopeRepository.CanInsertApiScopeAsync(scope);
        }

        public async Task<int> DeleteApiScopeAsync(int apiScopeId)
        {
            var deleted = await ApiScopeRepository.DeleteApiScopeAsync(apiScopeId);

            return deleted;
        }

        public async Task<int> DeleteApiScopePropertyAsync(int apiScopePropertyId)
        {
            var deleted = await ApiScopeRepository.DeleteApiScopePropertyAsync(apiScopePropertyId);

            return deleted;
        }

        public async Task<ApiScopeDto> GetApiScopeAsync(int apiScopeId)
        {
            var apiScope = await ApiScopeRepository.GetApiScopeAsync(apiScopeId);
            if (apiScope == null) return null;

            var apiScopeDto = apiScope.ToModel();

            return apiScopeDto;
        }

        public async Task<ApiScopesDto> GetApiScopesAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await ApiScopeRepository.GetApiScopesAsync(search, page, pageSize);
            var ApiScopesDto = pagedList.ToModel();

            return ApiScopesDto;
        }

        public async Task<int> UpdateApiScopeAsync(ApiScopeDto apiScope)
        {
            var canInsert = await CanInsertApiScopeAsync(apiScope);
            if (!canInsert)
            {
                return -1;
            }

            var scope = apiScope.ToEntity();

            var updated = await ApiScopeRepository.UpdateApiScopeAsync(scope);

            return updated;
        }
    }
}
