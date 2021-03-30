using IdentityServer.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.EntityFramework.Repositories.Interfaces
{
    public interface IApiScopeRepository
    {
        Task<PageData<ApiScope>> GetApiScopesAsync(string search, int page = 1, int pageSize = 10);

        Task<ApiScope> GetApiScopeAsync(int apiScopeId);

        Task<bool> CanInsertApiScopeAsync(ApiScope apiScope);

        Task<int> AddApiScopeAsync(ApiScope apiScope);

        Task<int> UpdateApiScopeAsync(ApiScope apiScope);

        Task<int> DeleteApiScopeAsync(int apiScopeId);

        Task<int> AddApiScopePropertyAsync(int apiScopeId,
            ApiScopeProperty apiScopeProperty);

        Task<int> DeleteApiScopePropertyAsync(int apiScopePropertyId);

        Task<int> SaveAllChangesAsync();
    }
}
