using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.EntityFramework.Common
{

    public enum DatabaseProviderType
    {
        SqlServer,
        PostgreSQL,
        MySql
    }

    public class DatabaseProviderConfiguration
    {
        public DatabaseProviderType ProviderType { get; set; }
    }
}
