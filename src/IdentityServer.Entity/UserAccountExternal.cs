﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Entity
{
    public class UserAccountExternal
    {
        public long Id { get; set; }

        [Required]
        [StringLength(60)]
        public string UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string Scheme { get; set; }

        [Required]
        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}