using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using VersionDLL.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class OWHSRepository : BaseOWHSRepository
    {
        private static readonly string WAREHOUSE_BASE_QUERY = $"select * from \"OWHS\" {{0}}";
        private const string WAREHOUSE_QUERY = "select * from \"OWHS\" where \"WhsCode\" = '{0}'";

        public OWHSRepository(Company company) : base(company)
        {
        }

        public override OWHS RetrieveWarehouse(string code)
        {
            var recordSet = (RecordsetEx) Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(WAREHOUSE_QUERY, code));
            var warehouse = new OWHS();

            var warehouseCode = recordSet.GetColumnValue("WhsCode")?.ToString();
            warehouse.Code = string.IsNullOrEmpty(warehouseCode) ? string.Empty : warehouseCode;

            var warehouseName = recordSet.GetColumnValue("WhsName")?.ToString();
            warehouse.Name = string.IsNullOrEmpty(warehouseName) ? string.Empty : warehouseName;

            var street = recordSet.GetColumnValue("Street")?.ToString();
            warehouse.Street = string.IsNullOrEmpty(street) ? string.Empty : street;

            var streetNumber = recordSet.GetColumnValue("StreetNo")?.ToString();
            warehouse.StreetNumber = string.IsNullOrEmpty(streetNumber) ? string.Empty : streetNumber;

            warehouse.Latitude = recordSet.GetColumnValue(@"U_EXK_LOCX")?.ToDecimal() ?? decimal.Zero;
            warehouse.Longitude = recordSet.GetColumnValue(@"U_EXK_LOCY")?.ToDecimal() ?? decimal.Zero;
            warehouse.DeliveryControl = recordSet.GetColumnValue(@"U_EXK_RSEN")?.ToString() ?? string.Empty;
            warehouse.CodigoAlmacenTemporal = recordSet.GetColumnValue(@"U_EXK_TEMP")?.ToString() ?? string.Empty;
            return warehouse;
        }

        public override IEnumerable<OWHS> Retrieve(Expression<Func<OWHS, bool>> expression)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return get_warehouses(string.Format(WAREHOUSE_BASE_QUERY, whereStatement));
        }

        private IEnumerable<OWHS> get_warehouses(string query)
        {
            IList<OWHS> routes = new List<OWHS>();

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            IEnumerable<PropertyInfo> entityProperties = CurrentReferenceType
                .GetProperties()
                .Where(item => item.GetCustomAttribute<SAPColumnAttribute>() != null || item.GetCustomAttribute<FieldNoRelated>() != null)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new OWHS();
                foreach (PropertyInfo property in entityProperties)
                {
                    string sapColumnName = get_sap_column_name(property);
                    object parsedValue = try_get_sap_value(recordSet, sapColumnName, property);
                    property.SetValue(document, parsedValue);
                }

                routes.Add(document);
                recordSet.MoveNext();
            }

            return routes;
        }

        private string get_sap_column_name(MemberInfo propertyInfo)
        {
            var sapColumnAttribute = propertyInfo.GetCustomAttribute<SAPColumnAttribute>();
            var fieldNoRelatedAttribute = propertyInfo.GetCustomAttribute<FieldNoRelated>();
            return fieldNoRelatedAttribute == null ? sapColumnAttribute.Name : fieldNoRelatedAttribute.ColumnName;
        }

        private object try_get_sap_value(RecordsetEx recordSet, string sapColumnName, PropertyInfo propertyInfo)
        {
            try
            {
                object originalValue = recordSet.GetColumnValue(sapColumnName) ??
                                       propertyInfo.PropertyType.GetDefaultValue();
                return Convert.ChangeType(originalValue, propertyInfo.PropertyType);
            }
            catch
            {
                return propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
            }
        }
    }
}