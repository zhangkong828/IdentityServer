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
    public class ApiResourceMapperProfile : Profile
    {
        public ApiResourceMapperProfile()
        {
            // entity to model
            CreateMap<ApiResource, ApiResourceDto>(MemberList.Destination)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => x.Type)))
                .ForMember(x => x.Scopes, opts => opts.MapFrom(src => src.Scopes.Select(x => x.Scope)));

            CreateMap<ApiResourceSecret, ApiResourceSecretDto>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null));

            CreateMap<ApiResourceProperty, ApiResourcePropertyDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<ApiResourceProperty, ApiResourcePropertiesDto>(MemberList.Destination)
                .ForMember(dest => dest.Key, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.ApiResourcePropertyId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ApiResourceId, opt => opt.MapFrom(x => x.ApiResource.Id));

            //PagedLists
            CreateMap<PageData<ApiResource>, ApiResourcesDto>(MemberList.Destination)
                .ForMember(x => x.ApiResources, opt => opt.MapFrom(src => src.List));

            CreateMap<PageData<ApiResourceSecret>, ApiResourceSecretsDto>(MemberList.Destination)
                .ForMember(x => x.ApiResourceSecrets, opt => opt.MapFrom(src => src.List));

            CreateMap<PageData<ApiResourceProperty>, ApiResourcePropertiesDto>(MemberList.Destination)
                .ForMember(x => x.ApiResourceProperties, opt => opt.MapFrom(src => src.List));

            // model to entity
            CreateMap<ApiResourceDto, ApiResource>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new ApiResourceClaim { Type = x })));

            CreateMap<ApiScopesDto, ApiScope>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new ApiScopeClaim { Type = x })))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ApiScopeId));

            CreateMap<ApiScopeDto, ApiScope>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new ApiScopeClaim { Type = x })));

            CreateMap<ApiResourcePropertiesDto, ApiResourceProperty>(MemberList.Source)
                .ForMember(x => x.ApiResource, dto => dto.MapFrom(src => new ApiResource() { Id = src.ApiResourceId }))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ApiResourcePropertyId));
        }
    }
}
