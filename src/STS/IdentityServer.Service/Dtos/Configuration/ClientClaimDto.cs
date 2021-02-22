using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityServer.Service.Dtos.Configuration
{
	public class ClientClaimDto
	{
		public int Id { get; set; }

		[Required]
		public string Type { get; set; }

		[Required]
		public string Value { get; set; }
	}
}
