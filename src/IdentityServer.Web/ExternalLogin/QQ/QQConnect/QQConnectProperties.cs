using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Web.ExternalLogin.QQ.QQConnect
{
    public sealed class QQConnectProperties
    {
        public static readonly string Key = "qq-connect.props";

        public ISet<string> Scopes { get; set; }

        public bool IsMobile { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        public static QQConnectProperties From(string json)
        {
            return JsonConvert.DeserializeObject<QQConnectProperties>(json);
        }
    }
}
