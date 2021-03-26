using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Service.Dtos.Configuration
{
    public class ApiResourceSecretsDto
    {
        public ApiResourceSecretsDto()
        {
            ApiResourceSecrets = new List<ApiResourceSecretDto>();
        }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<ApiResourceSecretDto> ApiResourceSecrets { get; set; }
    }
}
