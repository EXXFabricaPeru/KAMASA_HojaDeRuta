using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class CounterRepository : BaseCounterRepository
    {
        private const string BASE_QUERY = "select * from \"@VS_PD_ODCR\"";
        private const string BASE_CHILD_QUERY = "select * from \"@VS_PD_DCR1\"";

        private IEnumerable<PropertyInfo> _sapProperties;

        private IEnumerable<PropertyInfo> SAPProperties
            => _sapProperties ?? (_sapProperties = new ODCR().SAPProperties.ToList());

        public CounterRepository(SAPbobsCOM.Company company) : base(company)
        {
        }

        public override ResponseDocumentTransaction Register(ODCR entity)
        {
            try
            {
                Company.StartTransaction();
                SAPbobsCOM.CompanyService companyService = Company.GetCompanyService();
                SAPbobsCOM.GeneralService generalService = companyService.GetGeneralService(ODCR.ID);
                var generalData = generalService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData).To<SAPbobsCOM.GeneralData>();

                foreach (PropertyInfo property in SAPProperties.Where(t => t.IsChildProperty() || t.IsFieldNoRelated()))
                {
                    if (property.IsFieldNoRelated())
                    {
                        FieldNoRelated noRelatedProperty = property.RetrieveFieldNoRelated();
                        generalData.SetValueIfNotDefault(noRelatedProperty.sAliasID, property, property.GetValue(entity));
                    }
                    else if (property.IsChildProperty())
                        register_child(generalData, property, property.GetValue(entity));
                }

                SAPbobsCOM.GeneralDataParams generalDataParams = generalService.Add(generalData);
                Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                generalData = generalService.GetByParams(generalDataParams);
                var documentEntry = generalData.GetProperty("DocEntry").ToInt32();
                var documentNumber = generalData.GetProperty("DocNum").ToInt32();
                return ResponseDocumentTransaction.MakeSuccessTransaction(documentEntry, documentNumber);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                return ResponseDocumentTransaction.MakeFailureTransaction(exception.Message);
            }
        }

        private void register_child(SAPbobsCOM.GeneralData generalData, PropertyInfo property, object value)
        {
            ChildProperty childPropertyAttribute = property.RetrieveChildProperty();
            Type childType = property.PropertyType.GetGenericArguments()[0];
            IEnumerable<PropertyInfo> childProperties = childType.RetrieveSAPFieldNoRelatedProperties().ToList();
            var list = value.To<IList>();
            SAPbobsCOM.GeneralDataCollection dataCollection = generalData.Child(childPropertyAttribute.ChildName);
            foreach (object item in list)
            {
                SAPbobsCOM.GeneralData childData = dataCollection.Add();
                foreach (PropertyInfo childProperty in childProperties)
                {
                    FieldNoRelated fieldNoRelated = childProperty.RetrieveFieldNoRelated();
                    childData.SetValueIfNotDefault(fieldNoRelated.sAliasID, childProperty, childProperty.GetValue(item));
                }
            }
        }

        public override Tuple<bool, string> Update(ODCR entity, params Expression<Func<ODCR, object>>[] properties)
        {
            throw new NotImplementedException();
        }

        public override ResponseTransaction AppendChild(int entry, DCR1 child)
        {
            try
            {
                Company.StartTransaction();
                
                SAPbobsCOM.CompanyService companyService = Company.GetCompanyService();
                SAPbobsCOM.GeneralService generalService = companyService.GetGeneralService(ODCR.ID);
                var generalDataParams = generalService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams).To<SAPbobsCOM.GeneralDataParams>();
                generalDataParams.SetProperty("DocEntry", entry);
                SAPbobsCOM.GeneralData generalData = generalService.GetByParams(generalDataParams);
                SAPbobsCOM.GeneralDataCollection dataCollection = generalData.Child(DCR1.ID);
                IEnumerable<PropertyInfo> childProperties = typeof(DCR1).RetrieveSAPFieldNoRelatedProperties().ToList();
                SAPbobsCOM.GeneralData childData = dataCollection.Add();
                foreach (PropertyInfo childProperty in childProperties)
                {
                    FieldNoRelated fieldNoRelated = childProperty.RetrieveFieldNoRelated();
                    childData.SetValueIfNotDefault(fieldNoRelated.sAliasID, childProperty, childProperty.GetValue(child));
                }

                generalService.Update(generalData);
                Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                return ResponseTransaction.MakeSuccessTransaction();
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                return ResponseTransaction.MakeFailureTransaction(exception.Message);
            }
        }

        public override Tuple<bool, string> UpdateChild(int entry, DCR1 child, params Expression<Func<DCR1, object>>[] properties)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ODCR> Retrieve(Expression<Func<ODCR, bool>> expression = null)
        {
            var whereStatement = string.Empty;
            if (expression != null)
                whereStatement = $"where {QueryHelper.ParseToHANAQuery(expression)}";

            return retrieve_entities(whereStatement);
        }

        private IEnumerable<ODCR> retrieve_entities(string whereStatement)
        {
            var recordSet = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx).To<SAPbobsCOM.RecordsetEx>();
            recordSet.DoQuery($"{BASE_QUERY} {whereStatement}");
            IList<ODCR> result = new List<ODCR>();
            while (!recordSet.EoF)
            {
                ODCR entity = SAPProperties
                    .Aggregate(new ODCR(),
                        (current, property) => (ODCR) set_property(recordSet, current, property, property.GetCustomAttribute<FieldNoRelated>(),
                            property.GetCustomAttribute<ColumnProperty>(), property.GetCustomAttribute<ChildProperty>()));

                result.Add(entity);
                recordSet.MoveNext();
            }

            return result;
        }

        private object set_property(SAPbobsCOM.RecordsetEx recordSet, object entity, PropertyInfo property, FieldNoRelated fieldNoRelated,
            ColumnProperty columnProperty, ChildProperty childProperty = null)
        {
            object parsedValue = null;

            if (childProperty != null)
            {
                parsedValue = get_list_from_child(property, entity.To<ODCR>().DocumentEntry);
            }
            else if (fieldNoRelated != null)
            {
                parsedValue = get_value_from_field_no_related(fieldNoRelated, property, recordSet);
            }
            else if (columnProperty != null)
            {
                object originalValue = recordSet.GetColumnValue(columnProperty.ColumnName) ?? property.PropertyType.GetDefaultValue();
                parsedValue = Convert.ChangeType(originalValue, property.PropertyType);
            }

            property.SetValue(entity, parsedValue);
            return entity;
        }

        private object get_list_from_child(PropertyInfo baseProperty, int entry)
        {
            Type listType = baseProperty.PropertyType.GetGenericArguments()[0];
            Type constructedListType = typeof(List<>).MakeGenericType(listType);
            var instance = (IList) Activator.CreateInstance(constructedListType);

            var recordSet = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx).To<SAPbobsCOM.RecordsetEx>();
            recordSet.DoQuery($"{BASE_CHILD_QUERY} where \"DocEntry\" = {entry}");

            IEnumerable<PropertyInfo> properties = listType.GetProperties()
                .Where(t => t.GetCustomAttribute<FieldNoRelated>() != null ||
                            t.GetCustomAttribute<ColumnProperty>() != null ||
                            t.GetCustomAttribute<ChildProperty>() != null)
                .ToList();

            while (!recordSet.EoF)
            {
                object entity = properties
                    .Aggregate(Activator.CreateInstance(listType),
                        (current, property) => set_property(recordSet, current, property, property.GetCustomAttribute<FieldNoRelated>(),
                            property.GetCustomAttribute<ColumnProperty>()));

                instance.Add(entity);
                recordSet.MoveNext();
            }

            return instance;
        }

        private object get_value_from_field_no_related(FieldNoRelated fieldAttribute, PropertyInfo property, SAPbobsCOM.RecordsetEx recordSet)
        {
            Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            object value = recordSet.GetColumnValue(fieldAttribute.sAliasID) ?? property.PropertyType.GetDefaultValue();
            if (value == null)
                return null;

            if (fieldAttribute.BoType == BoDbTypes.Hour)
                value = parse_hour(value.ToInt32());

            value = Convert.ChangeType(value, propertyType);
            return value;
        }

        private DateTime parse_hour(int rawHour)
        {
            int hours = rawHour / 100;
            int minutes = rawHour % 100;

            return new DateTime().AddHours(hours).AddMinutes(minutes);
        }
    }
}