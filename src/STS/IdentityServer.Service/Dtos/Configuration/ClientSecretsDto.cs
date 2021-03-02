using IdentityServer.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityServer.Service.Dtos.Configuration
{
	public class ClientSecretsDto1
	{
		public ClientSecretsDto1()
		{
			ClientSecrets = new List<ClientSecretDto>();
		}

		public int ClientId { get; set; }

		public string ClientName { get; set; }

		public int ClientSecretId { get; set; }

		[Required]
		public string Type { get; set; } = "SharedSecret";

		public List<SelectItem> TypeList { get; set; }

		public string Description { get; set; }

		[Required]
		public string Value { get; set; }

		public string HashType { get; set; }

		public List<SelectItem> HashTypes { get; set; }

		public DateTime? Expiration { get; set; }

		public int TotalCount { get; set; }

		public int PageSize { get; set; }

		public List<ClientSecretDto> ClientSecrets { get; set; }
	}
}
