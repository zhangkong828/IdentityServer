﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityServer.Service.Dtos.Configuration
{
    public class ApiResourceSecretDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [Required]
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        [Required]
        public string Type { get; set; } = "SharedSecret";
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
