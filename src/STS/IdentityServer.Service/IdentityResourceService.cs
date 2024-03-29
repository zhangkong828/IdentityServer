﻿using IdentityServer.EntityFramework.Repositories.Interfaces;
using IdentityServer.Service.Dtos.Configuration;
using IdentityServer.Service.Interfaces;
using IdentityServer.Service.Mappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Service
{
    public class IdentityResourceService : IIdentityResourceService
    {
        protected readonly IIdentityResourceRepository IdentityResourceRepository;
        public IdentityResourceService(IIdentityResourceRepository identityResourceRepository)
        {
            IdentityResourceRepository = identityResourceRepository;
        }


        public async Task<int> AddIdentityResourceAsync(IdentityResourceDto identityResource)
        {
            var canInsert = await CanInsertIdentityResourceAsync(identityResource);
            if (!canInsert)
            {
                return -1;
            }

            var resource = identityResource.ToEntity();

            var saved = await IdentityResourceRepository.AddIdentityResourceAsync(resource);
            return saved;
        }

        public async Task<int> AddIdentityResourcePropertyAsync(int identityResourceId, IdentityResourcePropertyDto identityResourcePropertyDto)
        {
            var identityResourceProperty = identityResourcePropertyDto.ToEntity();

            var added = await IdentityResourceRepository.AddIdentityResourcePropertyAsync(identityResourceId, identityResourceProperty);

            return added;
        }

        public async Task<bool> CanInsertIdentityResourceAsync(IdentityResourceDto identityResource)
        {
            var resource = identityResource.ToEntity();

            return await IdentityResourceRepository.CanInsertIdentityResourceAsync(resource);
        }

        public async Task<int> DeleteIdentityResourceAsync(int identityResourceId)
        {
            var deleted = await IdentityResourceRepository.DeleteIdentityResourceAsync(identityResourceId);

            return deleted;
        }

        public async Task<int> DeleteIdentityResourcePropertyAsync(int identityResourcePropertyId)
        {
            var deleted = await IdentityResourceRepository.DeleteIdentityResourcePropertyAsync(identityResourcePropertyId);

            return deleted;
        }

        public async Task<IdentityResourceDto> GetIdentityResourceAsync(int identityResourceId)
        {
            var identityResource = await IdentityResourceRepository.GetIdentityResourceAsync(identityResourceId);
            if (identityResource == null) return null;

            var identityResourceDto = identityResource.ToModel();

            return identityResourceDto;
        }

        public async Task<IdentityResourcesDto> GetIdentityResourcesAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await IdentityResourceRepository.GetIdentityResourcesAsync(search, page, pageSize);
            var identityResourcesDto = pagedList.ToModel();

            return identityResourcesDto;
        }

        public async Task<int> UpdateIdentityResourceAsync(IdentityResourceDto identityResource)
        {
            var canInsert = await CanInsertIdentityResourceAsync(identityResource);
            if (!canInsert)
            {
                return -1;
            }

            var resource = identityResource.ToEntity();

            var updated = await IdentityResourceRepository.UpdateIdentityResourceAsync(resource);

            return updated;
        }
    }
}
