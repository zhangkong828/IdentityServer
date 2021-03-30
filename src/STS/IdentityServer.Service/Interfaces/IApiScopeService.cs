using IdentityServer.Service.Dtos.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service.Interfaces
{
    public interface IApiScopeService
    {
        Task<ApiScopesDto> GetApiScopesAsync(string search, int page = 1, int pageSize = 10);

        Task<ApiScopeDto> GetApiScopeAsync(int apiScopeId);

        Task<bool> CanInsertApiScopeAsync(ApiScopeDto apiScope);

        Task<int> AddApiScopeAsync(ApiScopeDto apiScope);

        Task<int> UpdateApiScopeAsync(ApiScopeDto apiScope);

        Task<int> DeleteApiScopeAsync(int apiScopeId);

        Task<int> AddApiScopePropertyAsync(int apiScopeId, ApiScopePropertyDto apiScopePropertyDto);

        Task<int> DeleteApiScopePropertyAsync(int apiScopePropertyId);

    }
}
