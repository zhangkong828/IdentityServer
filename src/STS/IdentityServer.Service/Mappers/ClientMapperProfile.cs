﻿using AutoMapper;
using IdentityServer.EntityFramework.Entities;
using IdentityServer.Service.Dtos.Configuration;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Mappers
{
    public class ClientMapperProfile : Profile
    {
        public ClientMapperProfile()
        {
            // entity to model
            CreateMap<Client, ClientDto>(MemberList.Destination)
                .ForMember(dest => dest.ProtocolType, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<ClientGrantType, string>()
                .ConstructUsing(src => src.GrantType)
                .ReverseMap()
                .ForMember(dest => dest.GrantType, opt => opt.MapFrom(src => src));

            CreateMap<ClientRedirectUri, string>()
                .ConstructUsing(src => src.RedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.RedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<ClientPostLogoutRedirectUri, string>()
                .ConstructUsing(src => src.PostLogoutRedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.PostLogoutRedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<ClientScope, string>()
                .ConstructUsing(src => src.Scope)
                .ReverseMap()
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

            CreateMap<ClientSecret, ClientSecretDto>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<ClientClaim, ClientClaimDto>(MemberList.None)
                .ConstructUsing(src => new ClientClaimDto() { Type = src.Type, Value = src.Value })
                .ReverseMap();

            CreateMap<ClientIdPRestriction, string>()
                .ConstructUsing(src => src.Provider)
                .ReverseMap()
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src));

            CreateMap<ClientCorsOrigin, string>()
                .ConstructUsing(src => src.Origin)
                .ReverseMap()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

            CreateMap<ClientProperty, ClientPropertyDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<ClientClaim, ClientClaimsDto>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.ClientClaimId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ClientId, opt => opt.MapFrom(x => x.Client.Id));

            CreateMap<ClientProperty, ClientPropertiesDto>(MemberList.Destination)
                .ForMember(dest => dest.Key, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.ClientPropertyId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ClientId, opt => opt.MapFrom(x => x.Client.Id));

            //PagedLists
            CreateMap<PageData<ClientClaim>, ClientClaimsDto>(MemberList.Destination)
                .ForMember(x => x.ClientClaims, opt => opt.MapFrom(src => src.List));

            CreateMap<PageData<ClientProperty>, ClientPropertiesDto>(MemberList.Destination)
                .ForMember(x => x.ClientProperties, opt => opt.MapFrom(src => src.List));

            CreateMap<PageData<Client>, ClientsDto>(MemberList.Destination)
                .ForMember(x => x.Clients, opt => opt.MapFrom(src => src.List));

            // model to entity
            CreateMap<ClientClaimsDto, ClientClaim>(MemberList.Source)
                .ForMember(x => x.Client, dto => dto.MapFrom(src => new Client() { Id = src.ClientId }))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ClientClaimId));

            CreateMap<ClientPropertiesDto, ClientProperty>(MemberList.Source)
                .ForMember(x => x.Client, dto => dto.MapFrom(src => new Client() { Id = src.ClientId }))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ClientPropertyId));
        }
    }
}
