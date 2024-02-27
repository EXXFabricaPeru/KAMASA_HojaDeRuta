using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class TransporterLiquidateRepository : BaseTransporterLiquidateRepository
    {
        private static readonly string SELECT_BASE_QUERY = $"select * from \"@{OLDP.ID}\" {{0}}";
        private static readonly string SELECT_DETAIL_QUERY = $"select * from \"@{LDP1.ID}\" where \"DocEntry\" = {{0}}";
        private static readonly string SELECT_MOTIVES_QUERY = $"select * from \"@{LDP2.ID}\" where \"DocEntry\" = {{0}}";

        public TransporterLiquidateRepository(Company company) : base(company)
        {
        }

        public override Tuple<bool, string> Register(OLDP entity)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OLDP.ID);

                var generalData = (GeneralData) generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                set_values(generalData, entity);

                GeneralDataCollection childDataCollection = generalData.Child(LDP1.ID);
                foreach (LDP1 relatedRoutes in entity.RelatedRoutes)
                {
                    GeneralData childGeneralData = childDataCollection.Add();
                    set_values(childGeneralData, relatedRoutes);
                }

                GeneralDataParams generalDataParams = generalService.Add(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                generalData = generalService.GetByParams(generalDataParams);
                object documentEntry = generalData.GetProperty("DocEntry");
                return Tuple.Create(true, documentEntry.ToString());
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
        }

        private Tuple<PropertyInfo, string> parse_tuple_property(PropertyInfo info)
        {
            if (info.IsColumnProperty()) return Tuple.Create(info, info.RetrieveColumnProperty().ColumnName);

            if (info.IsFieldNoRelated()) return Tuple.Create(info, info.RetrieveFieldNoRelated().sAliasID);

            throw new Exception();
        }

        private void set_values(GeneralData generalData, object entity)
        {
            IEnumerable<Tuple<PropertyInfo, string>> entityProperties = entity.GetType()
                .RetrieveSAPFieldNoRelatedProperties()
                .Select(parse_tuple_property);
            foreach (Tuple<PropertyInfo, string> property in entityProperties)
            {
                object sapValue = property.Item1.GetValue(entity);
                if (sapValue is decimal || sapValue is float)
                    sapValue = Convert.ChangeType(sapValue, typeof(double));
                if (sapValue is bool)
                    sapValue = (bool)sapValue? "Y":"N";
                generalData.SetValueIfNotDefault(property.Item2, property.Item1, sapValue);
            }
        }

        private GeneralData build_general_data(GeneralDataCollection dataCollection, int lineId)
        {
            for (var i = 0; i < dataCollection.Count; i++)
            {
                GeneralData generalData = dataCollection.Item(i);
                object currentLine = generalData.GetProperty("LineId");
                if (currentLine.Equals(lineId))
                    return generalData;
            }

            return dataCollection.Add();
        }

        public override Tuple<bool, string> Update(OLDP entity)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OLDP.ID);
                var generalDataParams = (GeneralDataParams) generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", entity.DocumentEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                IEnumerable<Tuple<PropertyInfo, string>> entityProperties = CurrentReferenceType
                    .RetrieveSAPFieldNoRelatedProperties()
                    .Select(info => Tuple.Create(info, info.RetrieveFieldNoRelated().sAliasID));
                foreach (Tuple<PropertyInfo, string> property in entityProperties)
                {
                    object sapValue = property.Item1.GetValue(entity);
                    if (sapValue is decimal || sapValue is float)
                        sapValue = Convert.ChangeType(sapValue, typeof(double));

                    generalData.SetValueIfNotDefault(property.Item2, property.Item1, sapValue);
                }

                GeneralDataCollection childDataCollection = generalData.Child(LDP1.ID);
                foreach (LDP1 relatedRoutes in entity.RelatedRoutes)
                {
                    GeneralData childGeneralData = build_general_data(childDataCollection, relatedRoutes.LineId);
                    set_values(childGeneralData, relatedRoutes);
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

        public override Tuple<bool, string> UpdateHeaderFields(OLDP entity)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OLDP.ID);
                var generalDataParams = (GeneralDataParams) generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", entity.DocumentEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);
                IEnumerable<Tuple<PropertyInfo, string>> entityProperties = CurrentReferenceType
                    .RetrieveSAPFieldNoRelatedProperties()
                    .Select(info => Tuple.Create(info, info.RetrieveFieldNoRelated().sAliasID));
                foreach (Tuple<PropertyInfo, string> property in entityProperties)
                {
                    object sapValue = property.Item1.GetValue(entity);
                    if (sapValue is decimal || sapValue is float)
                        sapValue = Convert.ChangeType(sapValue, typeof(double));

                    generalData.SetValueIfNotDefault(property.Item2, property.Item1, sapValue);
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

        public override IEnumerable<OLDP> Retrieve(Expression<Func<OLDP, bool>> expression = null)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return get_liquidations(string.Format(SELECT_BASE_QUERY, whereStatement));
        }

        private IEnumerable<OLDP> get_liquidations(string query)
        {
            IList<OLDP> routes = new List<OLDP>();

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            IEnumerable<PropertyInfo> entityProperties = CurrentReferenceType.RetrieveSAPProperties()
                .OrderBy(item => item.GetCustomAttribute<ChildProperty>() != null ? 1 : 0)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new OLDP();
                foreach (PropertyInfo property in entityProperties)
                {
                    if (property.Name == nameof(document.RelatedRoutes))
                    {
                        document.RelatedRoutes = get_detail(document.DocumentEntry);
                        continue;
                    }

                    if (property.Name == nameof(document.RelatedMotives))
                    {
                        document.RelatedMotives = get_motives(document.DocumentEntry);
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

        private List<LDP1> get_detail(int documentEntry)
        {
            var detailRoute = new List<LDP1>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(SELECT_DETAIL_QUERY, documentEntry));

            IEnumerable<PropertyInfo> entityProperties = typeof(LDP1).RetrieveSAPProperties()
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new LDP1();
                foreach (PropertyInfo property in entityProperties)
                {
                    object parsedValue = null;

                    var columnAttribute = property.GetCustomAttribute<ColumnProperty>();
                    var fieldAttribute = property.GetCustomAttribute<FieldNoRelated>();
                    if (fieldAttribute != null)
                    {
                        Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
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
                            else if (propertyType == typeof(bool))
                            {
                                parsedValue = parsedValue.ToString().ToUpper() == "Y";
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

        private List<LDP2> get_motives(int entry)
        {
            var result = new List<LDP2>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(SELECT_MOTIVES_QUERY, entry));

            IEnumerable<PropertyInfo> entityProperties = typeof(LDP2)
                .RetrieveSAPProperties()
                .ToList();

            while (!recordSet.EoF)
            {
                var entity = recordSet.MakeBasicSAPEntity<LDP2>(entityProperties);
                result.Add(entity);
                recordSet.MoveNext();
            }

            return result;
        }

        public override Tuple<bool, string> RemoveChild<T>(int entry)
        {
            CompanyService companyService = null;
            GeneralService generalService = null;
            GeneralDataParams generalDataParams = null;
            GeneralData generalData = null;
            GeneralDataCollection dataCollection = null;

            try
            {
                Company.StartTransaction();
                companyService = Company.GetCompanyService();
                generalService = companyService.GetGeneralService(OLDP.ID);
                generalDataParams = generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
                    .To<GeneralDataParams>();
                generalDataParams.SetProperty("DocEntry", entry);
                generalData = generalService.GetByParams(generalDataParams);
                dataCollection = get_child_general_collection<T>(generalData);
                while (dataCollection.Count > 0)
                    dataCollection.Remove(0);

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
            finally
            {
                GenericHelper.ReleaseCOMObjects(dataCollection, generalData, generalDataParams, generalService, companyService);
            }
        }

        public override Tuple<bool, string> RegisterChild<T>(int entry, IEnumerable<T> children)
        {
            CompanyService companyService = null;
            GeneralService generalService = null;
            GeneralDataParams generalDataParams = null;
            GeneralData generalData = null;
            GeneralDataCollection dataCollection = null;
            GeneralData childData = null;
            try
            {
                IEnumerable<PropertyInfo> childProperties = typeof(T)
                    .RetrieveSAPFieldNoRelatedProperties()
                    .ToList();

                Company.StartTransaction();
                companyService = Company.GetCompanyService();
                generalService = companyService.GetGeneralService(OLDP.ID);
                generalDataParams = generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
                    .To<GeneralDataParams>();
                generalDataParams.SetProperty("DocEntry", entry);
                generalData = generalService.GetByParams(generalDataParams);
                dataCollection = get_child_general_collection<T>(generalData);

                foreach (T item in children)
                {
                    childData = dataCollection.Add();
                    foreach (PropertyInfo property in childProperties)
                    {
                        var fieldRelatedAttribute = property.GetCustomAttribute<FieldNoRelated>();
                        object value = property.GetValue(item);
                        if (value is decimal || value is float)
                            value = Convert.ChangeType(value, typeof(double));
                        if (value is bool)
                            value = value.ToBoolean() ? "Y" : "N";
                        childData.SetValueIfNotDefault(fieldRelatedAttribute.sAliasID, property, value);
                    }
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
            finally
            {
                GenericHelper.ReleaseCOMObjects(childData, dataCollection, generalData, generalDataParams, generalService, companyService);
            }
        }

        public override Tuple<bool, string> ClosePayment(int entry, DateTime closeDate)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OLDP.ID);
                var generalDataParams = (GeneralDataParams) generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", entry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);
                generalData.SetProperty(@"U_EXK_STAD", "CL");
                generalData.SetProperty(@"U_EXK_FELQ", closeDate);
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

        private GeneralDataCollection get_child_general_collection<T>(GeneralData generalData) where T : BaseUDO
        {
            if (typeof(T) == typeof(LDP1))
                return generalData.Child(LDP1.ID);
            if (typeof(T) == typeof(LDP2))
                return generalData.Child(LDP2.ID);

            return null;
        }

        public override IEnumerable<OLDP> RetrieveByDate(DateTime date, string Transporter)
        {
            string dateCond = "";
            if (date.Year!=1)
            {
                 dateCond = $" \"U_EXK_FELQ\" = TO_DATE('{date.ToShortDateString()}' , 'DD/MM/YYYY') and";
            }
            string whereStatement = $"where {dateCond} \"U_EXK_CDPR\" = '{Transporter}'  ";
            return get_liquidations(string.Format(SELECT_BASE_QUERY, whereStatement));
        }
    }
}