using IdentityServer.Service.Dtos.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Models
{
    public class AddApiResourceSecretRequest
    {
        public int ApiResourceId { get; set; }
        public ApiResourceSecretDto ApiResourceSecret { get; set; }
    }
}
