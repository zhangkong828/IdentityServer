using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Extensions
{
    public class GenericControllerRouteConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                var genericName = controller.ControllerType.Name;
                var genericNameWithoutArity = genericName.Substring(0, genericName.IndexOf('`'));
                controller.ControllerName = genericNameWithoutArity.Substring(0, genericNameWithoutArity.LastIndexOf("Controller"));
            }
        }
    }
}
