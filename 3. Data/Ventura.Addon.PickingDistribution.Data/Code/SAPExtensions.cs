using System;
using System.Reflection;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Code
{
    public static class SAPExtensions
    {
        public static void SetValueIfNotNull(this GeneralData generalData, string propertyName, object value)
        {
            if (value != null)
                generalData.SetProperty(propertyName, value);
        }

        public static void SetValueIfNotDefault(this GeneralData generalData, string columnName, PropertyInfo property, object value)
        {
            if (value == null || get_default_value(property).Equals(value))
                return;

            generalData.SetProperty(columnName, value);
        }

        private static object get_default_value(PropertyInfo property)
        {
            Type nullableTypeReference = Nullable.GetUnderlyingType(property.PropertyType);
            return nullableTypeReference == null ? property.PropertyType.GetDefaultValue() : nullableTypeReference.GetDefaultValue();
        }
    }
}