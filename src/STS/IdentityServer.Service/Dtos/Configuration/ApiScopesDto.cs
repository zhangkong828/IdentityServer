using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityServer.Service.Dtos.Configuration
{
	public class ApiScopesDto
	{
		public ApiScopesDto()
		{
			Scopes = new List<ApiScopeDto>();
		}

		
		public int PageSize { get; set; }

		public int TotalCount { get; set; }
		public int PageIndex { get; set; }

		public List<ApiScopeDto> Scopes { get; set; }
	}
}
