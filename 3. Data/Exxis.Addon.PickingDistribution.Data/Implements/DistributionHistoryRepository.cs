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
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class DistributionHistoryRepository : BaseDistributionHistoryRepository
    {
        private static readonly string HISTORY_QUERY = $"select * from \"@{OHDM.ID}\" {{0}}";
        private static readonly string HISTORY_DETAIL_QUERY = $"select * from \"@{HDM1.ID}\" where \"DocEntry\" = {{0}}";

        public DistributionHistoryRepository(Company company) : base(company)
        {
        }

        public override Tuple<bool, string> Register(OHDM document)
        {
            try
            {
                Company.StartTransaction();
                var companyService = Company.GetCompanyService();
                var generalService = companyService.GetGeneralService(OHDM.ID);
                var generalData = (GeneralData)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                foreach (PropertyInfo property in document.GetType().GetProperties())
                {
                    if (property.Name == nameof(document.HistoryDetails))
                    {
                        GeneralDataCollection childCollection = generalData.Child(HDM1.ID);
                        foreach (HDM1 detail in document.HistoryDetails)
                        {
                            GeneralData childData = childCollection.Add();
                            foreach (var detailProperty in detail.GetType().GetProperties())
                            {
                                var detailFieldAttribute = detailProperty.GetCustomAttribute<FieldNoRelated>();
                                if (detailProperty.GetCustomAttribute<ColumnProperty>() != null && detailFieldAttribute == null || detailFieldAttribute == null)
                                    continue;

                                var detailSapValue = detailProperty.GetValue(detail);
                                
                                if (detailSapValue is decimal || detailSapValue is float)
                                    detailSapValue = Convert.ChangeType(detailSapValue, typeof(double));

                                childData.SetValueIfNotNull(detailFieldAttribute.sAliasID, detailSapValue);
                            }
                        }
                        continue;
                    }

                    var fieldAttribute = property.GetCustomAttribute<FieldNoRelated>();
                    if (property.GetCustomAttribute<ColumnProperty>() != null && fieldAttribute == null || fieldAttribute == null)
                        continue;

                    var sapValue = property.GetValue(document);
                    var sapFullNameProperty = fieldAttribute.sAliasID;

                    if (sapValue is decimal || sapValue is float)
                        sapValue = Convert.ChangeType(sapValue, typeof(double));

                    generalData.SetValueIfNotNull(sapFullNameProperty, sapValue);
                }
                
                var generalDataParams = generalService.Add(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                generalData = generalService.GetByParams(generalDataParams);
                var documentEntry = generalData.GetProperty("DocEntry");
                return Tuple.Create(true, documentEntry.ToString());
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
        }

        public override Tuple<bool, string> Update(OHDM document)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OHDM.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", document.DocumentEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                foreach (PropertyInfo property in document.GetType().GetProperties())
                {
                    if (property.Name == nameof(document.HistoryDetails))
                    {
                        GeneralDataCollection childCollection = generalData.Child(HDM1.ID);
                        foreach (var detail in document.HistoryDetails)
                        {
                            try
                            {
                                GeneralData childData = null;
                                try
                                {
                                    childData = childCollection.Item(detail.LineId - 1);
                                }
                                catch
                                {
                                    childData = childCollection.Add();
                                }

                                foreach (var detailProperty in detail.GetType().GetProperties())
                                {
                                    var detailFieldAttribute = detailProperty.GetCustomAttribute<FieldNoRelated>();
                                    if (detailProperty.GetCustomAttribute<ColumnProperty>() != null && detailFieldAttribute == null || detailFieldAttribute == null)
                                        continue;

                                    var value = detailProperty.GetValue(detail);
                                    if (value is decimal || value is float)
                                        value = Convert.ChangeType(value, typeof(double));

                                    childData.SetValueIfNotNull(detailFieldAttribute.sAliasID, value);
                                }
                            }
                            catch (Exception exception)
                            {
                                var x = exception.Message;
                            }
                        }
                        continue;
                    }

                    var fieldAttribute = property.GetCustomAttribute<FieldNoRelated>();
                    if (property.GetCustomAttribute<ColumnProperty>() != null && fieldAttribute == null || fieldAttribute == null)
                        continue;

                    var sapValue = property.GetValue(document);
                    var sapFullNameProperty = fieldAttribute.sAliasID;

                    if (sapValue is decimal || sapValue is float)
                        sapValue = Convert.ChangeType(sapValue, typeof(double));

                    generalData.SetValueIfNotNull(sapFullNameProperty, sapValue);
                }

                generalService.Update(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, document.DocumentEntry.ToString());
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
        }

        public override Tuple<bool, string> Remove(int documentEntry)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OHDM.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", documentEntry);
                generalService.Delete(generalDataParams);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
        }

        public override Tuple<bool, string> RemoveLine(int documentEntry, int lineId)
        {
            throw new NotImplementedException();
        }
        
        public override IEnumerable<OHDM> Retrieve(Expression<Func<OHDM, bool>> expression)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return get_histories(string.Format(HISTORY_QUERY, whereStatement));
        }

        private IEnumerable<OHDM> get_histories(string query)
        {
            IList<OHDM> routes = new List<OHDM>();

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            IEnumerable<PropertyInfo> entityProperties = CurrentReferenceType.RetrieveSAPProperties()
                .OrderBy(item => item.GetCustomAttribute<ChildProperty>() != null ? 1 : 0)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new OHDM();
                foreach (PropertyInfo property in entityProperties)
                {
                    if (property.Name == nameof(document.HistoryDetails))
                    {
                        document.HistoryDetails = get_history_detail(document.DocumentEntry);
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

        private List<HDM1> get_history_detail(int documentEntry)
        {
            var detailRoute = new List<HDM1>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(HISTORY_DETAIL_QUERY, documentEntry));

            IEnumerable<PropertyInfo> entityProperties = typeof(HDM1).GetProperties()
                .Where(is_sap_property)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new HDM1();
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

        private bool is_sap_property(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<FieldNoRelated>() != null ||
                   propertyInfo.GetCustomAttribute<ColumnProperty>() != null ||
                   propertyInfo.GetCustomAttribute<ChildProperty>() != null;
        }

    }
}