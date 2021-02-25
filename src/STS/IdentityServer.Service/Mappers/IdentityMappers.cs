using AutoMapper;
using IdentityServer.EntityFramework.Entities.Identity;
using IdentityServer.Service.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Mappers
{
    public static class IdentityMappers
    {
        static IdentityMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static UserIdentityDto ToModel(this UserIdentity user)
        {
            return Mapper.Map<UserIdentityDto>(user);
        }
    }
}
