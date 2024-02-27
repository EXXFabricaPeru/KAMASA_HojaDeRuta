using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class OARDRepository : BaseOARDRepository
    {
        private static readonly string ROUTE_QUERY = $"select * from \"@{OARD.ID}\" {{0}}";
        private static readonly string ROUTE_DETAIL_QUERY = $"select * from \"@{ARD1.ID}\" where \"DocEntry\" = {{0}}";
        private static readonly string ROUTE_DOCUMENT_QUERY = $"select * from \"@{ARD2.ID}\" where \"DocEntry\" = {{0}}";

        public OARDRepository(Company company) : base(company)
        {
        }

        public override IEnumerable<OARD> Retrieve(Expression<Func<OARD, bool>> expression = null)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return get_routes(string.Format(ROUTE_QUERY, whereStatement));
        }

        public override IEnumerable<OARD> RetrieveByDate(DateTime date)
        {
            string whereStatement = $"where \"U_EXK_FEDS\" = TO_DATE('{date.ToShortDateString()}' , 'DD/MM/YYYY') ";
            return get_routes(string.Format(ROUTE_QUERY, whereStatement));
        }

        public override IEnumerable<OARD> RetrieveBetweenDate(DateTime date, DateTime date2)
        {
            string whereStatement = $"where \"U_EXK_FEDS\" BETWEEN TO_DATE('{date.ToShortDateString()}' , 'DD/MM/YYYY') and  TO_DATE('{date2.ToShortDateString()}' , 'DD/MM/YYYY') ";
            return get_routes(string.Format(ROUTE_QUERY, whereStatement));
        }

        private IEnumerable<OARD> get_routes(string query)
        {
            IList<OARD> routes = new List<OARD>();

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            IEnumerable<PropertyInfo> entityProperties = CurrentReferenceType.RetrieveSAPProperties()
                .OrderBy(item => item.GetCustomAttribute<ChildProperty>() != null ? 1 : 0)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new OARD();
                foreach (PropertyInfo property in entityProperties)
                {
                    if (property.Name == nameof(document.RelatedTransferOrders))
                    {
                        document.RelatedTransferOrders = get_route_detail(document.DocumentEntry);
                        continue;
                    }

                    if (property.Name == nameof(document.RelatedMarketingDocuments))
                    {
                        document.RelatedMarketingDocuments = get_route_documents(document.DocumentEntry);
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

        private List<ARD1> get_route_detail(int documentEntry)
        {
            var detailRoute = new List<ARD1>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(ROUTE_DETAIL_QUERY, documentEntry));

            IEnumerable<PropertyInfo> entityProperties = typeof(ARD1).GetProperties()
                .Where(is_sap_property)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new ARD1();
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

        private List<ARD2> get_route_documents(int documentEntry)
        {
            var result = new List<ARD2>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(ROUTE_DOCUMENT_QUERY, documentEntry));

            IEnumerable<PropertyInfo> entityProperties = typeof(ARD2).GetProperties()
                .Where(is_sap_property)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new ARD2();
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

                result.Add(document);
                recordSet.MoveNext();
            }

            return result;
        }

        public override Tuple<bool, string, string> Register(OARD entity)
        {
            try
            {
                Company.StartTransaction();
                var companyService = Company.GetCompanyService();
                var generalService = companyService.GetGeneralService(OARD.ID);
                var generalData = (GeneralData) generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                var properties = entity.GetType().RetrieveSAPProperties().Select(item =>
                    {
                        if (item.IsColumnProperty())
                            return Tuple.Create(item, item.RetrieveColumnProperty().ColumnName);

                        if (item.IsFieldNoRelated())
                            return Tuple.Create(item, item.RetrieveFieldNoRelated().sAliasID);

                        return null;
                    })
                    .Where(item => item != null);

                foreach (var property in properties)
                    generalData.SetValueIfNotDefault(property.Item2, property.Item1, property.Item1.GetValue(entity));

                var childDataCollection = generalData.Child(ARD1.ID);
                foreach (var transferOrder in entity.RelatedTransferOrders)
                {
                    var childGeneralData = childDataCollection.Add();
                    var transferOrderProperties = transferOrder.GetType().RetrieveSAPProperties().Select(item =>
                        {
                            if (item.IsColumnProperty())
                                return Tuple.Create(item, item.RetrieveColumnProperty().ColumnName);

                            if (item.IsFieldNoRelated())
                                return Tuple.Create(item, item.RetrieveFieldNoRelated().sAliasID);

                            return null;
                        })
                        .Where(item => item != null);
                    foreach (var property in transferOrderProperties)
                        childGeneralData.SetValueIfNotDefault(property.Item2, property.Item1, property.Item1.GetValue(transferOrder));
                }

                var childDataCollection2 = generalData.Child(ARD2.ID);

                if (entity.RelatedMarketingDocuments != null)
                {
                    foreach (var marketin in entity.RelatedMarketingDocuments)
                    {
                        var childGeneralData = childDataCollection2.Add();
                        var transferOrderProperties = marketin.GetType().RetrieveSAPProperties().Select(item =>
                            {
                                if (item.IsColumnProperty())
                                    return Tuple.Create(item, item.RetrieveColumnProperty().ColumnName);

                                if (item.IsFieldNoRelated())
                                    return Tuple.Create(item, item.RetrieveFieldNoRelated().sAliasID);

                                return null;
                            })
                            .Where(item => item != null);
                        foreach (var property in transferOrderProperties)
                            childGeneralData.SetValueIfNotDefault(property.Item2, property.Item1, property.Item1.GetValue(marketin));
                    }
                }


                var generalDataParams = generalService.Add(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                generalData = generalService.GetByParams(generalDataParams);
                var documentEntry = generalData.GetProperty("DocEntry");
                var documentNumber = generalData.GetProperty("DocNum");
                return Tuple.Create(true, documentEntry.ToString(), documentNumber.ToString());
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message, string.Empty);
            }
        }

        public override Tuple<bool, string, string> RegisterPrueba(OARD entity, ref List<string> msj)
        {
            try
            {
                Company.StartTransaction();
                //msj.Add("OARD");
                //msj.Add(DateTime.Now.ToString());
                var companyService = Company.GetCompanyService();
                var generalService = companyService.GetGeneralService(OARD.ID);
                var generalData = (GeneralData)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                var properties = entity.GetType().RetrieveSAPProperties().Select(item =>
                {
                    if (item.IsColumnProperty())
                        return Tuple.Create(item, item.RetrieveColumnProperty().ColumnName);

                    if (item.IsFieldNoRelated())
                        return Tuple.Create(item, item.RetrieveFieldNoRelated().sAliasID);

                    return null;
                })
                    .Where(item => item != null);

                foreach (var property in properties)
                {
                    //try
                    //{
                    //    msj.Add("Column Name " + property.Item2);
                    //    msj.Add("Property " + property.Item1.ToString());
                    //    msj.Add("Value" + property.Item1.GetValue(entity).ToString());
                    //}
                    //catch (Exception)
                    //{
                    //}
                  
                    generalData.SetValueIfNotDefault(property.Item2, property.Item1, property.Item1.GetValue(entity));
                }
                   
                //msj.Add("ARD1-1");
                var childDataCollection = generalData.Child(ARD1.ID);
                foreach (var transferOrder in entity.RelatedTransferOrders)
                {
                    //msj.Add("ARD1");
                    //msj.Add(transferOrder.TransferOrderNumber.ToString());
                    //msj.Add(transferOrder.DeliveryHour.ToString());
                    var childGeneralData = childDataCollection.Add();
                    var transferOrderProperties = transferOrder.GetType().RetrieveSAPProperties().Select(item =>
                    {
                        if (item.IsColumnProperty())
                            return Tuple.Create(item, item.RetrieveColumnProperty().ColumnName);

                        if (item.IsFieldNoRelated())
                            return Tuple.Create(item, item.RetrieveFieldNoRelated().sAliasID);

                        return null;
                    })
                        .Where(item => item != null);
                    foreach (var property in transferOrderProperties)
                        childGeneralData.SetValueIfNotDefault(property.Item2, property.Item1, property.Item1.GetValue(transferOrder));
                }

                var childDataCollection2 = generalData.Child(ARD2.ID);
               
                if (entity.RelatedMarketingDocuments != null)
                {
                    //msj.Add("ARD2");
                    foreach (var marketin in entity.RelatedMarketingDocuments)
                    {
                        var childGeneralData = childDataCollection2.Add();
                        var transferOrderProperties = marketin.GetType().RetrieveSAPProperties().Select(item =>
                        {
                            if (item.IsColumnProperty())
                                return Tuple.Create(item, item.RetrieveColumnProperty().ColumnName);

                            if (item.IsFieldNoRelated())
                                return Tuple.Create(item, item.RetrieveFieldNoRelated().sAliasID);

                            return null;
                        })
                            .Where(item => item != null);
                        foreach (var property in transferOrderProperties)
                            childGeneralData.SetValueIfNotDefault(property.Item2, property.Item1, property.Item1.GetValue(marketin));
                    }
                }


                var generalDataParams = generalService.Add(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                generalData = generalService.GetByParams(generalDataParams);
                var documentEntry = generalData.GetProperty("DocEntry");
                var documentNumber = generalData.GetProperty("DocNum");
                return Tuple.Create(true, documentEntry.ToString(), documentNumber.ToString());
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message, string.Empty);
            }
        }

        private bool is_sap_property(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<FieldNoRelated>() != null ||
                   propertyInfo.GetCustomAttribute<ColumnProperty>() != null ||
                   propertyInfo.GetCustomAttribute<ChildProperty>() != null;
        }

        public override Tuple<bool, string> Update(OARD document)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OARD.ID);
                var generalDataParams = (GeneralDataParams) generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", document.DocumentEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                IEnumerable<PropertyInfo> entityProperties = CurrentReferenceType.RetrieveSAPProperties()
                    .OrderBy(item => item.GetCustomAttribute<ChildProperty>() != null ? 1 : 0)
                    .ToList();

                foreach (PropertyInfo property in entityProperties)
                {
                    var childAttribute = property.GetCustomAttribute<ChildProperty>();
                    if (childAttribute != null)
                    {
                        GeneralDataCollection dataCollection = generalData.Child(childAttribute.ChildName);
                        var propertyTypeReflectedType = property.PropertyType.GenericTypeArguments[0];
                        var list = property.GetValue(document).To<IList>();
                        if (list == null)
                            continue;

                        for (var i = 0; i < list.Count; i++)
                        {
                            var @object = list[i];
                            var nestedProperties = propertyTypeReflectedType.GetProperties();
                            GeneralData data;
                            try
                            {
                                data = dataCollection.Item(i);
                                foreach (var propertyInfo in nestedProperties.Where(item => item.GetCustomAttribute<FieldNoRelated>() != null))
                                {
                                    var fieldRelatedAttribute = propertyInfo.GetCustomAttribute<FieldNoRelated>();
                                    var value = propertyInfo.GetValue(@object);
                                    if (value is decimal || value is float)
                                        value = Convert.ChangeType(value, typeof(double));

                                    if (propertyInfo.GetCustomAttribute<SAPForceUpdateAttribute>() != null)
                                        data.SetProperty(fieldRelatedAttribute.sAliasID, value);
                                    else
                                        data.SetValueIfNotDefault(fieldRelatedAttribute.sAliasID, propertyInfo, value);
                                }
                            }
                            catch (Exception ex)
                            {
                                data = dataCollection.Add();
                                foreach (var propertyInfo in nestedProperties.Where(item => item.GetCustomAttribute<FieldNoRelated>() != null))
                                {
                                    var fieldRelatedAttribute = propertyInfo.GetCustomAttribute<FieldNoRelated>();
                                    var value = propertyInfo.GetValue(@object);
                                    if (value is decimal || value is float)
                                        value = Convert.ChangeType(value, typeof(double));

                                    data.SetValueIfNotNull(fieldRelatedAttribute.sAliasID, value);
                                }
                            }
                        }
                    }


                    var fieldAttribute = property.GetCustomAttribute<FieldNoRelated>();
                    if (property.GetCustomAttribute<ColumnProperty>() != null && fieldAttribute == null || fieldAttribute == null)
                        continue;

                    var sapValue = property.GetValue(document);
                    var sapFullNameProperty = fieldAttribute.sAliasID;

                    if (sapValue is decimal || sapValue is float)
                        sapValue = Convert.ChangeType(sapValue, typeof(double));

                    generalData.SetValueIfNotDefault(sapFullNameProperty, property, sapValue);
                }

                generalService.Update(generalData);
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

        public override Tuple<bool, string> Remove(int documentEntry)
        {
            throw new NotImplementedException();
        }

        public override Tuple<bool, string> RemoveDetail<T>(int routeEntry)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OARD.ID);
                var generalDataParams = (GeneralDataParams) generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", routeEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);
                GeneralDataCollection dataCollection = get_child_collection<T>(generalData);
                while (dataCollection.Count > 0)
                    dataCollection.Remove(0);

                // for (var i = 0; i < dataCollection.Count; i++)
                //     dataCollection.Remove(i);

                generalService.Update(generalData);
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

        private GeneralDataCollection get_child_collection<T>(GeneralData generalData) where T : BaseUDO
        {
            if (typeof(T) == typeof(ARD1))
                return generalData.Child(ARD1.ID);
            if (typeof(T) == typeof(ARD2))
                return generalData.Child(ARD2.ID);
            if (typeof(T) == typeof(ARD3))
                return generalData.Child(ARD3.ID);

            return null;
        }

        public override IEnumerable<OARD> RetrieveDetail(Expression<Func<ARD1, bool>> expression)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return get_routes(string.Format(ROUTE_QUERY, whereStatement));
        }

        public override List<ARD3> RetrieveRelatedDeliveries(string routeEntry, int documentEntry)
        {
            var result = new List<ARD3>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery($"select * from \"@VS_PD_ARD3\" where \"DocEntry\" = '{routeEntry}' and \"U_EXK_DCEN\" = {documentEntry}");

            IEnumerable<PropertyInfo> entityProperties = typeof(ARD3)
                .RetrieveSAPPropertiesWithoutChild()
                .ToList();

            while (!recordSet.EoF)
            {
                var document = SapHelper.MakeEntityByRecordSet<ARD3>(recordSet, entityProperties);
                result.Add(document);
                recordSet.MoveNext();
            }

            return result;
        }

        public override Tuple<bool, string> InsertDetail<T>(int routeEntry, IEnumerable<T> details)
        {
            try
            {
                Company.StartTransaction();
                IEnumerable<PropertyInfo> detailProperties = typeof(T).GetProperties()
                    .Where(item => item.GetCustomAttribute<FieldNoRelated>() != null)
                    .ToList();

                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OARD.ID);

                var generalDataParams = generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
                    .To<GeneralDataParams>();
                generalDataParams.SetProperty("DocEntry", routeEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);
                GeneralDataCollection dataCollection = get_child_collection<T>(generalData);
                GeneralData childData = null;
                foreach (T item in details)
                {
                    childData = dataCollection.Add();
                    foreach (PropertyInfo property in detailProperties)
                    {
                        var fieldRelatedAttribute = property.GetCustomAttribute<FieldNoRelated>();
                        object value = property.GetValue(item);
                        if (value is decimal || value is float)
                            value = Convert.ChangeType(value, typeof(double));
                        childData.SetValueIfNotDefault(fieldRelatedAttribute.sAliasID, property, value);
                    }
                }

                generalService.Update(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                GenericHelper.ReleaseCOMObjects(childData, dataCollection, generalData, generalDataParams, generalService, companyService);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
        }

        public override Tuple<bool, string> UpdateDeliveryStatusNOriginTranshipingRelatedTransferOrder(int entry, int transferOrderIndex,
            string deliveryStatusCode, int transshippingEntry)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OARD.ID);
                var generalDataParams = generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams).To<GeneralDataParams>();
                generalDataParams.SetProperty("DocEntry", entry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);
                GeneralDataCollection dataCollection = generalData.Child(ARD1.ID);
                GeneralData generalChildData = dataCollection.Item(transferOrderIndex);
                generalChildData.SetProperty(ARD1.RetrieveSAPName(t => t.DeliveryStatus), deliveryStatusCode);
                generalChildData.SetProperty(ARD1.RetrieveSAPName(t => t.Origin), transshippingEntry);
                generalService.Update(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                GenericHelper.ReleaseCOMObjects(generalChildData, dataCollection, generalData, generalDataParams, generalService, companyService);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
        }
    }
}