using IdentityServer.Web.ExternalLogin.QQ.QQConnect.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.QQ.QQConnect.Response
{
    public sealed class UserResponse
    {
        private UserResponse()
        {
        }

        public string NickName { get; private set; }

        public string Avatar { get; private set; }

        public static UserResponse From(string userJson)
        {
            var user = JObject.Parse(userJson);
            return new UserResponse
            {
                NickName = user.TryGetValue("nickname"),
                Avatar = user.TryGetValue("figureurl_qq_1")
            };
        }
    }
}
