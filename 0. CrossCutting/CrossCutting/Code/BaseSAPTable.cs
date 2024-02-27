using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using VSVersionControl.FlagElements.Attributes;
using Newtonsoft.Json;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code
{
    [Serializable]
    public abstract class BaseSAPTable
    {
        private Type _type;

        [JsonIgnore]
        protected Type ClassTypeInfo
        {
            get
            {
                if (_type == null) _type = GetType();
                return _type;
            }
        }
        [JsonIgnore]
        public BoObjectTypes CurrentSAPObjectType
        {
            get
            {
                var objectAttribute = GetType().GetCustomAttribute<SAPObjectAttribute>();
                return objectAttribute.SapTypes;
            }
        }

        public string GetFieldWithPrefix(string propertyName)
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            var columnAttribute = property.GetCustomAttribute<SAPColumnAttribute>();
            if (columnAttribute != null)
                return columnAttribute.Name;

            var columnProperty = property.GetCustomAttribute<ColumnProperty>();
            if (columnProperty != null)
                return columnProperty.ColumnName;

            var fieldNoRelated = property.GetCustomAttribute<FieldNoRelated>();
            if (fieldNoRelated != null)
                return fieldNoRelated.sAliasID;

            throw new Exception();
        }

        public virtual string GetSAPFieldNameWithoutPrefix<T, TK>(Expression<Func<T, TK>> expression)
            where T : BaseSAPTable
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(expression);
            var columnAttribute = property.GetCustomAttribute<SAPColumnAttribute>();
            if (columnAttribute.SystemField)
                throw new Exception();

            return columnAttribute.Name.SubstringStartAt(2);
        }

        public string GetFieldWithoutPrefix(string propertyName)
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            var columnAttribute = property.GetCustomAttribute<SAPColumnAttribute>();
            if (columnAttribute.SystemField)
                throw new Exception();

            return columnAttribute.Name.SubstringStartAt(2);
        }

        [JsonIgnore]
        public IEnumerable<PropertyInfo> SAPProperties
            => ClassTypeInfo.GetProperties()
                .Where(is_sap_property)
                .OrderBy(t => t.IsChildProperty() ? 1 : 0);

        private bool is_sap_property(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<FieldNoRelated>() != null ||
                   propertyInfo.GetCustomAttribute<ColumnProperty>() != null ||
                   propertyInfo.GetCustomAttribute<ChildProperty>() != null;
        }
    }
}