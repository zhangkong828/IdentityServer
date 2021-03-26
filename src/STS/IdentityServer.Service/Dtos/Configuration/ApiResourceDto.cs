using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityServer.Service.Dtos.Configuration
{
    public class ApiResourceDto
    {
		public ApiResourceDto()
		{
			UserClaims = new List<string>();
		}

		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string DisplayName { get; set; }

		public string Description { get; set; }

		public bool Enabled { get; set; } = true;

		public string AllowedAccessTokenSigningAlgorithms { get; set; }
		public bool ShowInDiscoveryDocument { get; set; }

		public List<string> UserClaims { get; set; }

		public string UserClaimsItems { get; set; }

		public DateTime Created { get; set; }
		public DateTime? Updated { get; set; }
		public DateTime? LastAccessed { get; set; }
		public bool NonEditable { get; set; }


		public List<ApiResourceSecretDto> Secrets { get; set; }
		public List<string> Scopes { get; set; }
		public List<ApiResourcePropertyDto> Properties { get; set; }
	}
}
