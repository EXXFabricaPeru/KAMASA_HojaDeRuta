using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.DisposableBO;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public static class SapHelper
    {
        public static SAPError RetrieveSAPError(this SAPbobsCOM.Company company)
        {
            int errorCode;
            string errorMessage;
            company.GetLastError(out errorCode, out errorMessage);
            return new SAPError {Code = errorCode, Message = errorMessage};
        }

        public static SAPbobsCOM.Messages MakeMessage(this SAPbobsCOM.Company company)
        {
            return company
                .GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                .To<SAPbobsCOM.Messages>();
        }

        public static SafeRecordSet MakeSafeRecordSet(this SAPbobsCOM.Company company)
        {
            var recordSet = company
                .GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx)
                .To<SAPbobsCOM.RecordsetEx>();

            return new SafeRecordSet(recordSet);
        }
        
        /// <summary>
        /// Retrieve the SAP field name.<br/> Works only with <c>SAPColumn</c> and <c>FieldNoRelated</c>
        /// </summary>
        public static string GetSAPFieldName<T, TK>(this T baseSAPTable, Expression<Func<T, TK>> expression)
            where T : BaseSAPTable
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(expression);
            var sapColumnAttribute = property.GetCustomAttribute<SAPColumnAttribute>();
            if (sapColumnAttribute != null)
                return sapColumnAttribute.Name;

            var fieldNoRelatedAttribute = property.GetCustomAttribute<FieldNoRelated>();
            if (fieldNoRelatedAttribute != null)
                return fieldNoRelatedAttribute.ColumnName;

            throw new Exception("[Error] The property doesn't have a SAP attributes");
        }

        public static T MakeEntityByRecordSet<T>(RecordsetEx recordSet, IEnumerable<PropertyInfo> entityProperties)
            where T : new()
        {
            T entity = Activator.CreateInstance<T>();
            foreach (PropertyInfo property in entityProperties)
            {
                object parsedValue = null;

                var fieldAttribute = property.GetCustomAttribute<FieldNoRelated>();
                var columnAttribute = property.GetCustomAttribute<ColumnProperty>();
                if (fieldAttribute != null)
                {
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    parsedValue = recordSet.GetColumnValue(fieldAttribute.sAliasID) ?? property.PropertyType.GetDefaultValue();

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
                    object originalValue = recordSet.GetColumnValue(columnAttribute.ColumnName) ?? property.PropertyType.GetDefaultValue();
                    parsedValue = Convert.ChangeType(originalValue, property.PropertyType);
                }

                property.SetValue(entity, parsedValue);
            }

            return entity;
        }

        public static IEnumerable<PropertyInfo> RetrieveSAPPropertiesWithoutChild(this Type type)
            => type.GetProperties().Where(is_sap_property)
                .Where(property => property.GetCustomAttribute<ChildProperty>() == null);

        
        public static IEnumerable<PropertyInfo> RetrieveSAPFieldNoRelatedProperties(this Type type)
            => type.GetProperties().Where(propertyInfo => propertyInfo.IsFieldNoRelated());

        public static IEnumerable<PropertyInfo> RetrieveSAPProperties(this Type type)
            => type.GetProperties().Where(is_sap_property);

        public static IEnumerable<PropertyInfo> RetrieveSAPProperties<T>() where T : BaseSAPTable
            => typeof(T).RetrieveSAPProperties();

        private static bool is_sap_property(PropertyInfo propertyInfo)
        {
            return propertyInfo.IsFieldNoRelated() || propertyInfo.IsColumnProperty() ||
                   propertyInfo.IsChildProperty();
        }

        public static bool IsChildProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<ChildProperty>() != null;
        }

        public static bool IsColumnProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<ColumnProperty>() != null;
        }

        public static ColumnProperty RetrieveColumnProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<ColumnProperty>();
        }

        public static bool IsFieldNoRelated(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<FieldNoRelated>() != null;
        }

        public static FieldNoRelated RetrieveFieldNoRelated(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<FieldNoRelated>();
        }

        public static ChildProperty RetrieveChildProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<ChildProperty>();
        }

        public static T GetElement<T>(this IMatrix matrix, object columnIndex, int rowIndex)
        {
            var editText = matrix.GetCellSpecific(columnIndex, rowIndex).As<EditText>();
            return (T) Convert.ChangeType(editText.Value, typeof(T), CultureInfo.InvariantCulture);
        }

        public static bool IsCreateMode(this SBOItemEventArg eventArg)
        {
            return eventArg.FormMode == 3;
        }

        public static bool IsFindMode(this SBOItemEventArg eventArg)
        {
            return eventArg.FormMode == 0;
        }

        public static bool IsUpdateMode(this SBOItemEventArg eventArg)
        {
            return eventArg.FormMode == 2;
        }

        public static void ShowStatusBarMessage(this SystemFormBase formBase, string message)
        {
            BoMessageTime Seconds = BoMessageTime.bmt_Medium;
            SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(message, Seconds, false);
        }

        public static void ShowMessage(this UserFormBase formBase, string message)
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(message);
        }

        public static void ShowMessage(this SystemFormBase formBase, string message)
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(message);
        }

        public static void ForEach(this Columns columns, Action<Column> action)
        {
            for (var i = 0; i < columns.Count; i++)
            {
                Column column = columns.Item(i);
                action(column);
            }
        }

        public static void ForEachWhen(this Columns columns, Func<Column, bool> predicate, Action<Column> action)
        {
            columns.ForEach(column =>
            {
                if (predicate(column))
                    action(column);
            });
        }
    }
}