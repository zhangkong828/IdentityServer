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
    public class IdentityResourceRepository<TDbContext> : IIdentityResourceRepository
        where TDbContext : DbContext, IAdminConfigurationDbContext
    {
        protected readonly TDbContext DbContext;

        public bool AutoSaveChanges { get; set; } = true;

        public IdentityResourceRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<int> AddIdentityResourceAsync(IdentityResource identityResource)
        {
            DbContext.IdentityResources.Add(identityResource);

            await AutoSaveChangesAsync();

            return identityResource.Id;
        }

        public async Task<int> AddIdentityResourcePropertyAsync(int identityResourceId, IdentityResourceProperty identityResourceProperty)
        {
            var identityResource = await DbContext.IdentityResources.Where(x => x.Id == identityResourceId).SingleOrDefaultAsync();

            identityResourceProperty.IdentityResource = identityResource;
            await DbContext.IdentityResourceProperties.AddAsync(identityResourceProperty);

            return await AutoSaveChangesAsync();
        }

        public async Task<bool> CanInsertIdentityResourceAsync(IdentityResource identityResource)
        {
            if (identityResource.Id == 0)
            {
                var existsWithSameName = await DbContext.IdentityResources.Where(x => x.Name == identityResource.Name).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
            else
            {
                var existsWithSameName = await DbContext.IdentityResources.Where(x => x.Name == identityResource.Name && x.Id != identityResource.Id).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
        }

        public async Task<bool> CanInsertIdentityResourcePropertyAsync(IdentityResourceProperty identityResourceProperty)
        {
            var existsWithSameName = await DbContext.IdentityResourceProperties.Where(x => x.Key == identityResourceProperty.Key
                                                                                       && x.IdentityResource.Id == identityResourceProperty.IdentityResourceId).SingleOrDefaultAsync();
            return existsWithSameName == null;
        }

        public async Task<int> DeleteIdentityResourceAsync(int identityResourceId)
        {
            var identityResourceToDelete = await DbContext.IdentityResources.Where(x => x.Id == identityResourceId).SingleOrDefaultAsync();

            DbContext.IdentityResources.Remove(identityResourceToDelete);
            return await AutoSaveChangesAsync();
        }

        public async Task<int> DeleteIdentityResourcePropertyAsync(int identityResourcePropertyId)
        {
            var propertyToDelete = await DbContext.IdentityResourceProperties.Where(x => x.Id == identityResourcePropertyId).SingleOrDefaultAsync();

            DbContext.IdentityResourceProperties.Remove(propertyToDelete);
            return await AutoSaveChangesAsync();
        }

        public Task<IdentityResource> GetIdentityResourceAsync(int identityResourceId)
        {
            return DbContext.IdentityResources
                .Include(x => x.UserClaims)
                .Include(x=>x.Properties)
                .Where(x => x.Id == identityResourceId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<PageData<IdentityResourceProperty>> GetIdentityResourcePropertiesAsync(int identityResourceId, int page = 1, int pageSize = 10)
        {
            var pagedList = new PageData<IdentityResourceProperty>();

            var properties = await DbContext.IdentityResourceProperties.Where(x => x.IdentityResource.Id == identityResourceId).PageBy(x => x.Id, page, pageSize)
                .ToListAsync();

            pagedList.List.AddRange(properties);
            pagedList.TotalCount = await DbContext.IdentityResourceProperties.Where(x => x.IdentityResource.Id == identityResourceId).CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.PageIndex = page;
            return pagedList;
        }

        public Task<IdentityResourceProperty> GetIdentityResourcePropertyAsync(int identityResourcePropertyId)
        {
            return DbContext.IdentityResourceProperties
               .Include(x => x.IdentityResource)
               .Where(x => x.Id == identityResourcePropertyId)
               .SingleOrDefaultAsync();
        }

        public async Task<PageData<IdentityResource>> GetIdentityResourcesAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PageData<IdentityResource>();

            Expression<Func<IdentityResource, bool>> searchCondition = x => x.Name.Contains(search);

            var identityResources = await DbContext.IdentityResources.WhereIf(!string.IsNullOrEmpty(search), searchCondition).PageBy(x => x.Name, page, pageSize).ToListAsync();

            pagedList.List.AddRange(identityResources);
            pagedList.TotalCount = await DbContext.IdentityResources.WhereIf(!string.IsNullOrEmpty(search), searchCondition).CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.PageIndex = page;
            return pagedList;
        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateIdentityResourceAsync(IdentityResource identityResource)
        {
            //Remove old relations
            await RemoveIdentityResourceClaimsAsync(identityResource);

            //Update with new data
            DbContext.IdentityResources.Update(identityResource);

            return await AutoSaveChangesAsync();
        }

        private async Task RemoveIdentityResourceClaimsAsync(IdentityResource identityResource)
        {
            var identityClaims = await DbContext.IdentityResourceClaims.Where(x => x.IdentityResource.Id == identityResource.Id).ToListAsync();
            DbContext.IdentityResourceClaims.RemoveRange(identityClaims);
        }

        private async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await DbContext.SaveChangesAsync() : (int)SavedStatus.WillBeSavedExplicitly;
        }
    }
}
