using System;
using System.Linq.Expressions;
using System.Reflection;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VersionDLL.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Code
{
    public static class BaseUDOExtensions
    {
        public static string RetrieveFullSAPFieldName<T, TK>(this T entity, SAPColumnType columnType, Expression<Func<T, TK>> mapping)
            where T : BaseUDO
        {
            PropertyInfo property = get_property_name_for_expression(mapping);
            switch (columnType)
            {
                case SAPColumnType.FieldNoRelated:
                    return property.GetCustomAttribute<FieldNoRelated>().sAliasID;
                default:
                    throw new ArgumentOutOfRangeException(nameof(columnType), columnType, null);
            }
        }

        private static PropertyInfo get_property_name_for_expression<T, TK>(Expression<Func<T, TK>> mapping)
        {
            var memberExpression = mapping.Body as MemberExpression;
            var propertyInfo = memberExpression?.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new NotImplementedException();
            return propertyInfo;
        }
    }
}