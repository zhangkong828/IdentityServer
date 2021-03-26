using AutoMapper;
using IdentityServer.EntityFramework.Entities;
using IdentityServer.Service.Dtos.Configuration;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Mappers
{
    public static class ApiResourceMappers
    {
        static ApiResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ApiResourceDto ToModel(this ApiResource resource)
        {
            return resource == null ? null : Mapper.Map<ApiResourceDto>(resource);
        }

        public static ApiResourcesDto ToModel(this PageData<ApiResource> resources)
        {
            return resources == null ? null : Mapper.Map<ApiResourcesDto>(resources);
        }

        public static ApiResourcePropertiesDto ToModel(this PageData<ApiResourceProperty> apiResourceProperties)
        {
            return Mapper.Map<ApiResourcePropertiesDto>(apiResourceProperties);
        }

        public static ApiResourceSecretsDto ToModel(this PageData<ApiResourceSecret> apiResourceSecrets)
        {
            return Mapper.Map<ApiResourceSecretsDto>(apiResourceSecrets);
        }

        public static ApiResourcePropertiesDto ToModel(this ApiResourceProperty apiResourceProperty)
        {
            return Mapper.Map<ApiResourcePropertiesDto>(apiResourceProperty);
        }


        public static ApiResourceSecretDto ToModel(this ApiResourceSecret apiSecret)
        {
            return apiSecret == null ? null : Mapper.Map<ApiResourceSecretDto>(apiSecret);
        }

        public static ApiResource ToEntity(this ApiResourceDto resource)
        {
            return resource == null ? null : Mapper.Map<ApiResource>(resource);
        }


        public static ApiResourceSecret ToEntity(this ApiResourceSecretDto secret)
        {
            return secret == null ? null : Mapper.Map<ApiResourceSecret>(secret);
        }

        public static ApiResourceProperty ToEntity(this ApiResourcePropertiesDto apiResourceProperties)
        {
            return Mapper.Map<ApiResourceProperty>(apiResourceProperties);
        }
    }
}
