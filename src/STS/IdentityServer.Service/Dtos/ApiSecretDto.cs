﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityServer.Service.Dtos
{
	public class ApiSecretDto
	{
		[Required]
		public string Type { get; set; } = "SharedSecret";

		public int Id { get; set; }

		public string Description { get; set; }

		[Required]
		public string Value { get; set; }

		public DateTime? Expiration { get; set; }

		public DateTime Created { get; set; }
	}
}
