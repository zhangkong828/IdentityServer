using IdentityModel;
using IdentityServer.Entity;
using IdentityServer.Service;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.IdentityServer4Extension.Validator
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserAccountService _userAccountService;
        public ResourceOwnerPasswordValidator(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (_userAccountService.ValidateUsername(context.UserName, context.Password, null, out UserAccount user))
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

        private Claim[] GetUserClaims(UserAccount user)
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
