using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Models
{
    public class CloneClientRequest
    {
        [Required]
        public int OriginalId { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientName { get; set; }
    }
}
