using AutoMapper;
using IdentityServer.EntityFramework.Entities;
using IdentityServer.Service.Dtos.Configuration;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Mappers
{
    public static class ClientMappers
    {
        static ClientMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ClientDto ToModel(this Client client)
        {
            return Mapper.Map<ClientDto>(client);
        }

        public static ClientClaimsDto ToModel(this PageData<ClientClaim> clientClaims)
        {
            return Mapper.Map<ClientClaimsDto>(clientClaims);
        }

        public static ClientsDto ToModel(this PageData<Client> clients)
        {
            return Mapper.Map<ClientsDto>(clients);
        }

        public static ClientPropertiesDto ToModel(this PageData<ClientProperty> clientProperties)
        {
            return Mapper.Map<ClientPropertiesDto>(clientProperties);
        }

        public static Client ToEntity(this ClientDto client)
        {
            return Mapper.Map<Client>(client);
        }

        public static ClientSecret ToEntity(this ClientSecretDto clientSecret)
        {
            return Mapper.Map<ClientSecret>(clientSecret);
        }

        public static ClientClaimsDto ToModel(this ClientClaim clientClaim)
        {
            return Mapper.Map<ClientClaimsDto>(clientClaim);
        }

        public static ClientPropertiesDto ToModel(this ClientProperty clientProperty)
        {
            return Mapper.Map<ClientPropertiesDto>(clientProperty);
        }

        public static ClientClaim ToEntity(this ClientClaimsDto clientClaim)
        {
            return Mapper.Map<ClientClaim>(clientClaim);
        }

        public static ClientProperty ToEntity(this ClientPropertiesDto clientProperties)
        {
            return Mapper.Map<ClientProperty>(clientProperties);
        }

    }
}
