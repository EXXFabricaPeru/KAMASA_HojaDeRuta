using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public static class RecordSetHelper
    {
        public static T MakeBasicSAPEntity<T>(this RecordsetEx recordset, IEnumerable<PropertyInfo> entityProperties)
        {
            var result = Activator.CreateInstance<T>();

            foreach (PropertyInfo property in entityProperties)
            {
                object parsedValue = null;

                var fieldAttribute = property.GetCustomAttribute<FieldNoRelated>();
                var columnAttribute = property.GetCustomAttribute<ColumnProperty>();
                if (fieldAttribute != null)
                {
                    Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    parsedValue = recordset.GetColumnValue(fieldAttribute.sAliasID) ?? property.PropertyType.GetDefaultValue();

                    if (parsedValue != null)
                    {
                        if (fieldAttribute.BoType == BoDbTypes.Hour)
                        {
                            var rawTime = parsedValue.ToInt32();
                            var hours = rawTime / 100;
                            var minutes = rawTime % 100;
                            parsedValue = new DateTime().AddHours(hours).AddMinutes(minutes);
                        }

                        parsedValue = Convert.ChangeType(parsedValue, propertyType);
                    }
                }
                else if (columnAttribute != null)
                {
                    object originalValue = recordset.GetColumnValue(columnAttribute.ColumnName) ?? property.PropertyType.GetDefaultValue();
                    parsedValue = Convert.ChangeType(originalValue, property.PropertyType);
                }

                property.SetValue(result, parsedValue);
            }

            return result;
        }

        public static T RetrieveBasicSAPEntity<T>(this RecordsetEx recordset)
        {
            var result = Activator.CreateInstance<T>();

            IEnumerable<Tuple<PropertyInfo, string>> tupledProperties = typeof(T).GetProperties()
                .Select(item =>
                {
                    var sapColumn = item.GetCustomAttribute<SAPColumnAttribute>();
                    if (sapColumn != null)
                        return Tuple.Create(item, sapColumn.Name);

                    var fieldNoRelated = item.GetCustomAttribute<FieldNoRelated>();
                    if (fieldNoRelated != null)
                        return Tuple.Create(item, fieldNoRelated.sAliasID);

                    return null;
                })
                .Where(item => item?.Item2 != null);


            foreach (Tuple<PropertyInfo, string> tupledProperty in tupledProperties)
            {
                var value = recordset.GetColumnValue(tupledProperty.Item2);

                if (value == null)
                    continue;

                if (value.GetType() != tupledProperty.Item1.PropertyType)
                    value = Convert.ChangeType(value, tupledProperty.Item1.PropertyType);

                tupledProperty.Item1.SetValue(result, value);
            }

            return result;
        }

        public static string GetStringValue(this RecordsetEx recordSet, string columnId)
        {
            object columnValue = recordSet.GetColumnValue(columnId);
            return columnValue == null ? string.Empty : columnValue.ToString();
        }
    }
}