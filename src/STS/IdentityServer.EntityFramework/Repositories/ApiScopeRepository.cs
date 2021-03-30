using IdentityServer.EntityFramework.Entities;
using IdentityServer.EntityFramework.Enums;
using IdentityServer.EntityFramework.Extensions;
using IdentityServer.EntityFramework.Interfaces;
using IdentityServer.EntityFramework.Repositories.Interfaces;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.EntityFramework.Repositories
{
    public class ApiScopeRepository<TDbContext> : IApiScopeRepository
        where TDbContext : DbContext, IAdminConfigurationDbContext
    {
        protected readonly TDbContext DbContext;

        public bool AutoSaveChanges { get; set; } = true;

        public ApiScopeRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await DbContext.SaveChangesAsync() : (int)SavedStatus.WillBeSavedExplicitly;
        }

        public async Task<int> AddApiScopeAsync(ApiScope apiScope)
        {
            DbContext.ApiScopes.Add(apiScope);

            await AutoSaveChangesAsync();

            return apiScope.Id;
        }

        public async Task<int> AddApiScopePropertyAsync(int apiScopeId, ApiScopeProperty apiScopeProperty)
        {
            var apiScope = await DbContext.ApiScopes.Where(x => x.Id == apiScopeId).SingleOrDefaultAsync();

            apiScopeProperty.Scope = apiScope;
            await DbContext.ApiScopeProperties.AddAsync(apiScopeProperty);

            return await AutoSaveChangesAsync();
        }

        public async Task<bool> CanInsertApiScopeAsync(ApiScope apiScope)
        {
            if (apiScope.Id == 0)
            {
                var existsWithSameName = await DbContext.ApiScopes.Where(x => x.Name == apiScope.Name).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
            else
            {
                var existsWithSameName = await DbContext.ApiScopes.Where(x => x.Name == apiScope.Name && x.Id != apiScope.Id).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
        }

        public async Task<int> DeleteApiScopeAsync(int apiScopeId)
        {
            var apiScopeToDelete = await DbContext.ApiScopes.Where(x => x.Id == apiScopeId).SingleOrDefaultAsync();

            DbContext.ApiScopes.Remove(apiScopeToDelete);
            return await AutoSaveChangesAsync();
        }

        public async Task<int> DeleteApiScopePropertyAsync(int apiScopePropertyId)
        {
            var propertyToDelete = await DbContext.ApiScopeProperties.Where(x => x.Id == apiScopePropertyId).SingleOrDefaultAsync();

            DbContext.ApiScopeProperties.Remove(propertyToDelete);
            return await AutoSaveChangesAsync();
        }

        public async Task<ApiScope> GetApiScopeAsync(int apiScopeId)
        {
            return await DbContext.ApiScopes
                .Include(x => x.UserClaims)
                .Include(x => x.Properties)
                .Where(x => x.Id == apiScopeId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<PageData<ApiScope>> GetApiScopesAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PageData<ApiScope>();

            Expression<Func<ApiScope, bool>> searchCondition = x => x.Name.Contains(search);

            var ApiScopes = await DbContext.ApiScopes.WhereIf(!string.IsNullOrEmpty(search), searchCondition).PageBy(x => x.Name, page, pageSize).ToListAsync();

            pagedList.List.AddRange(ApiScopes);
            pagedList.TotalCount = await DbContext.ApiScopes.WhereIf(!string.IsNullOrEmpty(search), searchCondition).CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.PageIndex = page;
            return pagedList;
        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateApiScopeAsync(ApiScope apiScope)
        {
            //Remove old relations
            await RemoveApiScopeClaimsAsync(apiScope);

            //Update with new data
            DbContext.ApiScopes.Update(apiScope);

            return await AutoSaveChangesAsync();
        }

        private async Task RemoveApiScopeClaimsAsync(ApiScope apiScope)
        {
            var apiScopeClaims = await DbContext.ApiScopeClaims.Where(x => x.Scope.Id == apiScope.Id).ToListAsync();
            DbContext.ApiScopeClaims.RemoveRange(apiScopeClaims);
        }
    }
}
