using IdentityServer.Service.Dtos.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service.Interfaces
{
    public interface IIdentityResourceService
    {
        Task<IdentityResourcesDto> GetIdentityResourcesAsync(string search, int page = 1, int pageSize = 10);

        Task<IdentityResourceDto> GetIdentityResourceAsync(int identityResourceId);

        Task<bool> CanInsertIdentityResourceAsync(IdentityResourceDto identityResource);

        Task<int> AddIdentityResourceAsync(IdentityResourceDto identityResource);

        Task<int> UpdateIdentityResourceAsync(IdentityResourceDto identityResource);

        Task<int> DeleteIdentityResourceAsync(int identityResourceId);


        Task<int> AddIdentityResourcePropertyAsync(int identityResourceId, IdentityResourcePropertyDto identityResourcePropertyDto);

        Task<int> DeleteIdentityResourcePropertyAsync(int identityResourcePropertyId);

    }
}
