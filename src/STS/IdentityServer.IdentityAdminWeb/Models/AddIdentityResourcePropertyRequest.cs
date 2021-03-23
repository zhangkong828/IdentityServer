using IdentityServer.Service.Dtos.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Models
{
    public class AddIdentityResourcePropertyRequest
    {
        public int IdentityResourceId { get; set; }
        public IdentityResourcePropertyDto IdentityResourceProperty { get; set; }
    }
}
