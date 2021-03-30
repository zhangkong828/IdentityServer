using AutoMapper;
using IdentityServer.EntityFramework.Entities;
using IdentityServer.Service.Dtos.Configuration;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityServer.Service.Mappers
{
   public class ApiScopeMapperProfile : Profile
    {
        public ApiScopeMapperProfile()
        {
            // entity to model
            CreateMap<ApiScope, ApiScopeDto>(MemberList.Destination)
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)));

            CreateMap<ApiScopeProperty, ApiScopePropertyDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<PageData<ApiScope>, ApiScopesDto>(MemberList.Destination)
                .ForMember(x => x.Scopes,
                    opt => opt.MapFrom(src => src.List));

            // model to entity
            CreateMap<ApiScopeDto, ApiScope>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new ApiScopeClaim { Type = x })));
        }
    }
}
