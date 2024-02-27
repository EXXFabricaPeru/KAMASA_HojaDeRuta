using System;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public static class TypeExtensions
    {
        public static object GetDefaultValue(this Type type)
        {
            if (type == null || type == typeof(void) || type.IsInterface)
                return null;

            if (type == typeof(string))
                return string.Empty;

            return Activator.CreateInstance(type);
        }
    }
}