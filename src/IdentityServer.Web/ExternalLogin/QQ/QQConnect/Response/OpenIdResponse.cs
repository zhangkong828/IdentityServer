using IdentityServer.Web.ExternalLogin.QQ.QQConnect.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.QQ.QQConnect.Response
{
    public sealed class OpenIdResponse
    {
        private OpenIdResponse()
        {
        }

        public string ClientId { get; private set; }

        public string OpenId { get; private set; }

        public static OpenIdResponse From(string openIdJsonp)
        {
            //！！！jsonp
            //callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} );
            var json = openIdJsonp.Substring(8).Trim().Trim('(', ')', ';');
            var openId = JObject.Parse(json);
            return new OpenIdResponse
            {
                ClientId = openId.TryGetValue("client_id"),
                OpenId = openId.TryGetValue("openid")
            };
        }
    }
}
