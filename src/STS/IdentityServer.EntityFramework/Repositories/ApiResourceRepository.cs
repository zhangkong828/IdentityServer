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
    public class ApiResourceRepository<TDbContext> : IApiResourceRepository
        where TDbContext : DbContext, IAdminConfigurationDbContext
    {

        protected readonly TDbContext DbContext;

        public bool AutoSaveChanges { get; set; } = true;

        public ApiResourceRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await DbContext.SaveChangesAsync() : (int)SavedStatus.WillBeSavedExplicitly;
        }

        public async Task<int> AddApiResourceAsync(ApiResource apiResource)
        {
            DbContext.ApiResources.Add(apiResource);

            await AutoSaveChangesAsync();

            return apiResource.Id;
        }

        public async Task<int> AddApiResourcePropertyAsync(int apiResourceId, ApiResourceProperty apiResourceProperty)
        {
            var apiResource = await DbContext.ApiResources.Where(x => x.Id == apiResourceId).SingleOrDefaultAsync();

            apiResourceProperty.ApiResource = apiResource;
            await DbContext.ApiResourceProperties.AddAsync(apiResourceProperty);

            return await AutoSaveChangesAsync();
        }

        public async Task<int> AddApiResourceSecretAsync(int apiResourceId, ApiResourceSecret apiSecret)
        {
            apiSecret.ApiResource = await DbContext.ApiResources.Where(x => x.Id == apiResourceId).SingleOrDefaultAsync();
            await DbContext.ApiResourceSecrets.AddAsync(apiSecret);

            return await AutoSaveChangesAsync();
        }

        public async Task<bool> CanInsertApiResourceAsync(ApiResource apiResource)
        {
            if (apiResource.Id == 0)
            {
                var existsWithSameName = await DbContext.ApiResources.Where(x => x.Name == apiResource.Name).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
            else
            {
                var existsWithSameName = await DbContext.ApiResources.Where(x => x.Name == apiResource.Name && x.Id != apiResource.Id).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
        }

        public async Task<bool> CanInsertApiResourcePropertyAsync(ApiResourceProperty apiResourceProperty)
        {
            var existsWithSameName = await DbContext.ApiResourceProperties.Where(x => x.Key == apiResourceProperty.Key
                                                                                      && x.ApiResource.Id == apiResourceProperty.ApiResourceId).SingleOrDefaultAsync();
            return existsWithSameName == null;
        }

        public async Task<bool> CanInsertApiResourceScopeAsync(ApiResourceScope apiScope)
        {
            if (apiScope.Id == 0)
            {
                var existsWithSameName = await DbContext.ApiResourceScopes.Where(x => x.Scope == apiScope.Scope).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
            else
            {
                var existsWithSameName = await DbContext.ApiResourceScopes.Where(x => x.Scope == apiScope.Scope && x.Id != apiScope.Id).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
        }

        public async Task<int> DeleteApiResourceAsync(ApiResource apiResource)
        {
            var resource = await DbContext.ApiResources.Where(x => x.Id == apiResource.Id).SingleOrDefaultAsync();

            DbContext.Remove(resource);

            return await AutoSaveChangesAsync();
        }

        public async Task<int> DeleteApiResourcePropertyAsync(ApiResourceProperty apiResourceProperty)
        {
            var propertyToDelete = await DbContext.ApiResourceProperties.Where(x => x.Id == apiResourceProperty.Id).SingleOrDefaultAsync();

            DbContext.ApiResourceProperties.Remove(propertyToDelete);
            return await AutoSaveChangesAsync();
        }


        public async Task<int> DeleteApiResourceSecretAsync(int apiSecretId)
        {
            var apiSecretToDelete = await DbContext.ApiResourceSecrets.Where(x => x.Id == apiSecretId).SingleOrDefaultAsync();
            DbContext.ApiResourceSecrets.Remove(apiSecretToDelete);

            return await AutoSaveChangesAsync();
        }

        public async Task<ApiResource> GetApiResourceAsync(int apiResourceId)
        {
            return await DbContext.ApiResources
              .Include(x => x.UserClaims)
              .Include(x => x.Scopes)
              .Where(x => x.Id == apiResourceId)
              .AsNoTracking()
              .SingleOrDefaultAsync();
        }

        public async Task<string> GetApiResourceNameAsync(int apiResourceId)
        {
            var apiResourceName = await DbContext.ApiResources.Where(x => x.Id == apiResourceId).Select(x => x.Name).SingleOrDefaultAsync();

            return apiResourceName;
        }

        public async Task<PageData<ApiResourceProperty>> GetApiResourcePropertiesAsync(int apiResourceId, int page = 1, int pageSize = 10)
        {
            var pagedList = new PageData<ApiResourceProperty>();

            var properties = await DbContext.ApiResourceProperties.Where(x => x.ApiResource.Id == apiResourceId).PageBy(x => x.Id, page, pageSize)
                .ToListAsync();

            pagedList.List.AddRange(properties);
            pagedList.TotalCount = await DbContext.ApiResourceProperties.Where(x => x.ApiResource.Id == apiResourceId).CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.PageIndex = page;
            return pagedList;
        }

        public async Task<ApiResourceProperty> GetApiResourcePropertyAsync(int apiResourcePropertyId)
        {
            return await DbContext.ApiResourceProperties
                .Include(x => x.ApiResource)
                .Where(x => x.Id == apiResourcePropertyId)
                .SingleOrDefaultAsync();
        }

        public async Task<PageData<ApiResource>> GetApiResourcesAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PageData<ApiResource>();
            Expression<Func<ApiResource, bool>> searchCondition = x => x.Name.Contains(search);

            var apiResources = await DbContext.ApiResources.WhereIf(!string.IsNullOrEmpty(search), searchCondition).PageBy(x => x.Name, page, pageSize).ToListAsync();

            pagedList.List.AddRange(apiResources);
            pagedList.TotalCount = await DbContext.ApiResources.WhereIf(!string.IsNullOrEmpty(search), searchCondition).CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.PageIndex = page;
            return pagedList;
        }

        public async Task<ApiResourceSecret> GetApiResourceSecretAsync(int apiSecretId)
        {
            return await DbContext.ApiResourceSecrets
              .Include(x => x.ApiResource)
              .Where(x => x.Id == apiSecretId)
              .AsNoTracking()
              .SingleOrDefaultAsync();
        }

        public async Task<PageData<ApiResourceSecret>> GetApiResourceSecretsAsync(int apiResourceId, int page = 1, int pageSize = 10)
        {
            var pagedList = new PageData<ApiResourceSecret>();
            var apiSecrets = await DbContext.ApiResourceSecrets.Where(x => x.ApiResource.Id == apiResourceId).PageBy(x => x.Id, page, pageSize).ToListAsync();

            pagedList.List.AddRange(apiSecrets);
            pagedList.TotalCount = await DbContext.ApiResourceSecrets.Where(x => x.ApiResource.Id == apiResourceId).CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.PageIndex = page;
            return pagedList;
        }


        public async Task<int> SaveAllChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateApiResourceAsync(ApiResource apiResource)
        {
            //Remove old relations
            var apiResourceClaims = await DbContext.ApiResourceClaims.Where(x => x.ApiResource.Id == apiResource.Id).ToListAsync();
            DbContext.ApiResourceClaims.RemoveRange(apiResourceClaims);

            var apiResourceScopes = await DbContext.ApiResourceScopes.Where(x => x.ApiResource.Id == apiResource.Id).ToListAsync();
            DbContext.ApiResourceScopes.RemoveRange(apiResourceScopes);

            //Update with new data
            DbContext.ApiResources.Update(apiResource);

            return await AutoSaveChangesAsync();
        }
    }
}
