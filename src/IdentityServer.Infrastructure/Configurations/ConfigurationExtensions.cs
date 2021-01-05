﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Infrastructure.Configurations
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddConfigFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            builder.AddJsonFile(path, optional: optional, reloadOnChange: reloadOnChange);

            var configurationBuilder = new ConfigurationBuilder()
             .SetBasePath(AppContext.BaseDirectory)
             .AddJsonFile(path, optional: optional, reloadOnChange: reloadOnChange);

            Config.Configuration = configurationBuilder.Build();
            return builder;
        }

    }
}
