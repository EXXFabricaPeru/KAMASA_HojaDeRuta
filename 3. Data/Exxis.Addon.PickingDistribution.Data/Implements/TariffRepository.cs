using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class TariffRepository : BaseTariffRepository
    {
        private static readonly string TARIFF_QUERY = $"select * from \"@{OATP.ID}\" {{0}}";
        private static readonly string TARIFF_DETAIL_QUERY = $"select * from \"@{ATP1.ID}\" where \"DocEntry\" = {{0}}";
        private static readonly string TARIFF_PENALTY_QUERY = $"select * from \"@{ATP2.ID}\" where \"DocEntry\" = {{0}}";

        public TariffRepository(Company company) : base(company)
        {
        }

        #region Overrides of BaseTariffRepository

        public override IEnumerable<OATP> Retrieve(Expression<Func<OATP, bool>> expression = null)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return get_tariffs(string.Format(TARIFF_QUERY, whereStatement));
        }

        private IEnumerable<OATP> get_tariffs(string query)
        {
            IList<OATP> routes = new List<OATP>();

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            IEnumerable<PropertyInfo> entityProperties = CurrentReferenceType.RetrieveSAPProperties()
                .OrderBy(item => item.GetCustomAttribute<ChildProperty>() != null ? 1 : 0)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new OATP();
                foreach (PropertyInfo property in entityProperties)
                {
                    if (property.Name == nameof(document.RelatedTariffs))
                    {
                        document.RelatedTariffs = get_tariff_detail(document.DocumentEntry);
                        continue;
                    }

                    if (property.Name == nameof(document.RelatedPenalties))
                    {
                        document.RelatedPenalties = get_penalty_detail(document.DocumentEntry);
                        continue;
                    }

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

                    property.SetValue(document, parsedValue);
                }

                routes.Add(document);
                recordSet.MoveNext();
            }

            return routes;
        }

        private List<ATP1> get_tariff_detail(int documentEntry)
        {
            var detailRoute = new List<ATP1>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(TARIFF_DETAIL_QUERY, documentEntry));

            IEnumerable<PropertyInfo> entityProperties = typeof(ATP1)
                .RetrieveSAPPropertiesWithoutChild()
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new ATP1();
                foreach (PropertyInfo property in entityProperties)
                {
                    object parsedValue = null;

                    var columnAttribute = property.GetCustomAttribute<ColumnProperty>();
                    var fieldAttribute = property.GetCustomAttribute<FieldNoRelated>();
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

                    property.SetValue(document, parsedValue);
                }

                detailRoute.Add(document);
                recordSet.MoveNext();
            }

            return detailRoute;
        }

        private List<ATP2> get_penalty_detail(int documentEntry)
        {
            var detailRoute = new List<ATP2>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(TARIFF_PENALTY_QUERY, documentEntry));

            IEnumerable<PropertyInfo> entityProperties = typeof(ATP2)
                .RetrieveSAPPropertiesWithoutChild()
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new ATP2();
                foreach (PropertyInfo property in entityProperties)
                {
                    object parsedValue = null;

                    var columnAttribute = property.GetCustomAttribute<ColumnProperty>();
                    var fieldAttribute = property.GetCustomAttribute<FieldNoRelated>();
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

                    property.SetValue(document, parsedValue);
                }

                detailRoute.Add(document);
                recordSet.MoveNext();
            }

            return detailRoute;
        }

        #endregion


    }
}