using IdentityServer.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Infrastructure.Utility
{
    public class AvatarHelper
    {
        public static string GenerateAvatarUrl(string email, int size = 150)
        {
            var hash = Md5Helper.Md5By32(email);
            var sizeArg = size > 0 ? $"?s={size}" : "";
            return $"https://www.gravatar.com/avatar/{hash}{sizeArg}";
        }
    }
}
