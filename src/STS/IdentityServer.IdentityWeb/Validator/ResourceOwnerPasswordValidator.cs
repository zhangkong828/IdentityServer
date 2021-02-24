using IdentityModel;
using IdentityServer.EntityFramework.Entities.Identity;
using IdentityServer.Service.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.IdentityWeb.Validator
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IIdentityService _identityService;
        public ResourceOwnerPasswordValidator(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (_identityService.ValidateUsername(context.UserName, context.Password, null, out UserIdentity user))
            {
                context.Result = new GrantValidationResult(
                 subject: user.UserId,
                 authenticationMethod: "custom",
                 claims: GetUserClaims(user));
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
            return Task.CompletedTask;
        }

        private Claim[] GetUserClaims(UserIdentity user)
        {
            return new Claim[]
            {
            new Claim("UserId", user.UserId),
            new Claim(JwtClaimTypes.NickName,user.NickName),
            new Claim(JwtClaimTypes.GivenName, "222"),
            new Claim(JwtClaimTypes.FamilyName, "333"),
            new Claim(JwtClaimTypes.Email, "444"),
            new Claim(JwtClaimTypes.Role,"admin")
            };
        }
    }
}
