using IdentityServer.Service.Dtos.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Models
{
    public class AddApiScopePropertyRequest
    {
        public int ApiScopeId { get; set; }
        public ApiScopePropertyDto ApiScopeProperty { get; set; }
    }
}
