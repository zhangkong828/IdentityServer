using AutoMapper;
using IdentityServer.EntityFramework.Entities;
using IdentityServer.Service.Dtos.Configuration;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Mappers
{
    public static class IdentityResourceMappers
    {
        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }


        public static IdentityResourceDto ToModel(this IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResourceDto>(resource);
        }

        public static IdentityResourcesDto ToModel(this PageData<IdentityResource> resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResourcesDto>(resource);
        }

        public static IdentityResource ToEntity(this IdentityResourceDto resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResource>(resource);
        }

        public static IdentityResourcePropertiesDto ToModel(this PageData<IdentityResourceProperty> identityResourceProperties)
        {
            return Mapper.Map<IdentityResourcePropertiesDto>(identityResourceProperties);
        }

        public static IdentityResourcePropertiesDto ToModel(this IdentityResourceProperty identityResourceProperty)
        {
            return Mapper.Map<IdentityResourcePropertiesDto>(identityResourceProperty);
        }


        public static IdentityResourceProperty ToEntity(this IdentityResourcePropertiesDto identityResourceProperties)
        {
            return Mapper.Map<IdentityResourceProperty>(identityResourceProperties);
        }

        public static IdentityResourceProperty ToEntity(this IdentityResourcePropertyDto identityResourceProperty)
        {
            return Mapper.Map<IdentityResourceProperty>(identityResourceProperty);
        }
    }
}
