using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Localization
{
    public class GenericControllerLocalizer<TResourceSource> : IGenericControllerLocalizer<TResourceSource>
    {
        private IStringLocalizer _localizer;

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Localization.StringLocalizer`1" />.
        /// </summary>
        /// <param name="factory">The <see cref="T:Microsoft.Extensions.Localization.IStringLocalizerFactory" /> to use.</param>
        public GenericControllerLocalizer(IStringLocalizerFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var type = typeof(TResourceSource);
            var assemblyName = type.GetTypeInfo().Assembly.GetName().Name;
            var typeName = type.Name.Remove(type.Name.IndexOf('`'));
            var baseName = (type.Namespace + "." + typeName).Substring(assemblyName.Length).Trim('.');

            _localizer = factory.Create(baseName, assemblyName);
        }

        public virtual LocalizedString this[string name]
        {
            get
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                return _localizer[name];
            }
        }

        public virtual LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                return _localizer[name, arguments];
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }
    }
}
