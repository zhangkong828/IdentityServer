﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Dtos.Configuration
{
    public class IdentityResourcePropertyDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}