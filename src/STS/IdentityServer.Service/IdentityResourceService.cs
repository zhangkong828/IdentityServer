using IdentityServer.Service.Dtos.Configuration;
using IdentityServer.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service
{
    public class IdentityResourceService : IIdentityResourceService
    {
        public IdentityResourceService()
        {

        }


        public Task<int> AddIdentityResourceAsync(IdentityResourceDto identityResource)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CanInsertIdentityResourceAsync(IdentityResourceDto identityResource)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteIdentityResourceAsync(IdentityResourceDto identityResource)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResourceDto> GetIdentityResourceAsync(int identityResourceId)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResourcesDto> GetIdentityResourcesAsync(string search, int page = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateIdentityResourceAsync(IdentityResourceDto identityResource)
        {
            throw new NotImplementedException();
        }
    }
}
