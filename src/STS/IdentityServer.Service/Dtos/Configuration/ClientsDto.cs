﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Dtos.Configuration
{
	public class ClientsDto
	{
		public ClientsDto()
		{
			Clients = new List<ClientDto>();
		}

		public List<ClientDto> Clients { get; set; }

		public int TotalCount { get; set; }

		public int PageSize { get; set; }

		public int PageIndex { get; set; }
	}
}
