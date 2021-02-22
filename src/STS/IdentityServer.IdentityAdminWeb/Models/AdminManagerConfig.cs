using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Models
{
    public class AdminManagerConfig
    {
        public List<User> Users { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
