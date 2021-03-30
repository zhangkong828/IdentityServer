using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Dtos.Configuration
{
	public class IdentityResourcesDto
	{
		public IdentityResourcesDto()
		{
			IdentityResources = new List<IdentityResourceDto>();
		}

		public int PageSize { get; set; }

		public int TotalCount { get; set; }

		public int PageIndex { get; set; }

		public List<IdentityResourceDto> IdentityResources { get; set; }
	}
}
