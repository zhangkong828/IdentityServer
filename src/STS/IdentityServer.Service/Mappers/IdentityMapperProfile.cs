using AutoMapper;
using IdentityServer.EntityFramework.Entities.Identity;
using IdentityServer.Service.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Mappers
{
    public class IdentityMapperProfile : Profile
    {
        public IdentityMapperProfile()
        {
            CreateMap<UserIdentity, UserIdentityDto>().ReverseMap();
        }
    }
}
