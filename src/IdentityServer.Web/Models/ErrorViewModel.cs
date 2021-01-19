using IdentityServer4.Models;
using System;

namespace IdentityServer.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class ErrorViewModel2
    {
        public ErrorMessage Error { get; set; }
    }
}
