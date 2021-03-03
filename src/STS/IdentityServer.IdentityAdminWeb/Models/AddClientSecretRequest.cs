using IdentityServer.Service.Dtos.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Models
{
    public class AddClientSecretRequest
    {
        public int ClientId { get; set; }
        public ClientSecretDto ClientSecret { get; set; }
    }
}
