using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Localization
{
    public class CultureConfiguration
    {
        public static readonly string[] AvailableCultures = { "en", "zh" };
        public static readonly string DefaultRequestCulture = "zh";

        //public List<string> Cultures { get; set; } = AvailableCultures.ToList();
        //public string DefaultCulture { get; set; } = DefaultRequestCulture;
    }
}
