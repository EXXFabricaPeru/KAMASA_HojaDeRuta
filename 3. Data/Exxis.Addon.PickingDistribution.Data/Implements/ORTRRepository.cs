using System;
using System.Collections;
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

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class ORTRRepository : BaseORTRRepository
    {
        private static readonly string DOCUMENT_QUERY = $"select * from \"@{ORTR.ID}\" {{0}}";
        private const string RELATED_DOCUMENT_QUERY = "select * from \"@VS_PD_RTR1\" where \"DocEntry\" = {0}";
        private const string RELATED_ENTRY_QUERY = "select * from \"@VS_PD_RTR2\" where \"DocEntry\" = {0}";
        private const string RELATED_BATCH_QUERY = "select * from \"@VS_PD_RTR3\" where \"DocEntry\" = {0}";

        public ORTRRepository(Company company) : base(company)
        {
        }

        public override Tuple<bool, string, string> Register(ORTR entity)
        {
            try
            {
                Company.StartTransaction();
                var companyService = Company.GetCompanyService();
                var generalService = companyService.GetGeneralService(ORTR.ID);
                var generalData = (GeneralData)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                generalData.SetValueIfNotNull("Series", entity.Serie);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.COCL)), entity.COCL);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.DSCL)), entity.DSCL);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.COEN)), entity.COEN);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.DREN)), entity.DREN);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.COSA)), entity.COSA);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.DRSA)), entity.DRSA);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.FJOR)), entity.FJOR);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.STAD)), entity.STAD);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.STMT)), entity.STMT);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.STDS)), entity.STDS);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.FEDS)), entity.FEDS);
                //generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.HRDS)), entity.HRDS);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.CNDC)), entity.CNDC);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.CNAR)), entity.CNAR.ToDouble());
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.PTOT)), entity.PTOT.ToDouble());
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.PPTO)), entity.PPTO.ToDouble());
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.VTOT)), entity.VRTO.ToDouble());
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.MTOT)), entity.MTOT.ToDouble());
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.TPCR)), entity.TPCR);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.TPDS)), entity.TPDS);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.CLVT)), entity.CLVT);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.OBJT)), entity.OBJT);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.PRDS)), entity.PRDS);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.CVRF)), entity.CVRF);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.ODCV)), entity.ODCV);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.VTHR)), entity.VTHR);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.RSEN)), entity.RSEN);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.TMDS)), entity.TMDS);
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.PRTO)), entity.PRTO.ToDouble());
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.VRTO)), entity.VRTO.ToDouble());
                generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.isPickup)), entity.isPickup);
                if (entity.DocEntryTransferOrderReference != null)
                    generalData.SetValueIfNotNull(entity.GetUDONameFromFieldNoRelated(nameof(entity.DocEntryTransferOrderReference)), entity.DocEntryTransferOrderReference);

                GeneralDataCollection rtr1collection = generalData.Child(RTR1.ID);
                foreach (RTR1 rtr1 in entity.RelatedSAPDocuments)
                {
                    GeneralData childData = rtr1collection.Add();
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.DCEN)), rtr1.DCEN);
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.DCNM)), rtr1.DCNM);
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.DCCC)), rtr1.DCCC);
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.DCFE)), rtr1.DCFE);
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.DCST)), rtr1.DCST);
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.STAD)), rtr1.STAD);
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.PRDS)), rtr1.PRDS);
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.CNAR)), rtr1.CNAR.ToDouble());
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.CPAR)), rtr1.CPAR.ToDouble());
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.PTOT)), rtr1.PTOT.ToDouble());
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.PPTO)), rtr1.PPTO.ToDouble());
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.VTOT)), rtr1.VTOT.ToDouble());
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.MTOT)), rtr1.MTOT.ToDouble());
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.MPTO)), rtr1.MPTO.ToDouble());
                    childData.SetValueIfNotNull(rtr1.GetUDONameFromFieldNoRelated(nameof(rtr1.OBJT)), rtr1.OBJT);
                }

                GeneralDataCollection rtr2collection = generalData.Child(RTR2.ID);
                foreach (RTR2 rtr2 in entity.RelatedSAPDocumentLines)
                {
                    GeneralData childData = rtr2collection.Add();
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.DCEN)), rtr2.DCEN);
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.DCNM)), rtr2.DCNM);
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.DCLN)), rtr2.DCLN);
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.CDAR)), rtr2.CDAR);
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.DSAR)), rtr2.DSAR);
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.TVAR)), rtr2.TVAR);
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.TPCR)), rtr2.TPCR);
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.CDUM)), rtr2.CDUM);
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.DSUM)), rtr2.DSUM);
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.DCCA)), rtr2.DCCA.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.DCCP)), rtr2.DCCP.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.CAPR)), rtr2.CAPR.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.PEAR)), rtr2.PEAR.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.PSAR)), rtr2.PSAR.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.PSAA)), rtr2.PSAA.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.VLAR)), rtr2.VLAR.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.VTAR)), rtr2.VTAR.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.PCAR)), rtr2.PCAR.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.PTAR)), rtr2.PTAR.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.PTAA)), rtr2.PTAA.ToDouble());
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.OBJT)), rtr2.OBJT);
                    childData.SetValueIfNotNull(rtr2.GetUDONameFromFieldNoRelated(nameof(rtr2.CDAL)), rtr2.CDAL);
                }

                GeneralDataCollection rtr3collection = generalData.Child(RTR3.ID);
                foreach (RTR3 rtr3 in entity.RelatedBatchLines)
                {
                    GeneralData childData = rtr3collection.Add();
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.DCEN)), rtr3.DCEN);
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.DCNM)), rtr3.DCNM);
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.DCLN)), rtr3.DCLN);
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.CDAR)), rtr3.CDAR);
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.CDAL)), rtr3.CDAL);
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.CDLT)), rtr3.CDLT);
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.FCEX)), rtr3.FCEX);
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.FCFA)), rtr3.FCFA);
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.DCCA)), rtr3.DCCA.ToDouble());
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.CADI)), rtr3.CADI.ToDouble());
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.CADE)), rtr3.CADE.ToDouble());
                    childData.SetValueIfNotNull(rtr3.GetUDONameFromFieldNoRelated(nameof(rtr3.OBJT)), rtr3.OBJT);
                }

                var generalDataParams = generalService.Add(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                generalData = generalService.GetByParams(generalDataParams);
                var documentNumber = generalData.GetProperty("DocNum");
                var documentEntry = generalData.GetProperty("DocEntry");

                return Tuple.Create(true, documentNumber.ToString(), documentEntry.ToString());
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message, string.Empty);
            }
        }

        public override IEnumerable<ORTR> Retrieve(Expression<Func<ORTR, bool>> expression)
        {
            string whereStatement = QueryHelper.ParseToHANAQuery(expression);
            return documents_from_query(string.Format(DOCUMENT_QUERY, $"where {whereStatement}"));
        }

        public override ORTR RetrieveDocEntryRelated(int documentEntry)
        {
            //string whereStatement = QueryHelper.ParseToHANAQuery(expression);
            var list= documents_from_query(string.Format(DOCUMENT_QUERY, $"where  \"U_EXK_DTOR\" = {documentEntry}"));
            return list.FirstOrDefault();
        }

        private bool is_sap_property(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<FieldNoRelated>() != null ||
                   propertyInfo.GetCustomAttribute<ColumnProperty>() != null ||
                   propertyInfo.GetCustomAttribute<ChildProperty>() != null;
        }

        private IEnumerable<ORTR> documents_from_query(string query)
        {
            IList<ORTR> documents = new List<ORTR>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            IEnumerable<PropertyInfo> entityProperties = CurrentReferenceType.RetrieveSAPProperties()
                .OrderBy(item => item.GetCustomAttribute<ChildProperty>() != null ? 1 : 0)
                .ToList();
            while (!recordSet.EoF)
            {
                var document = new ORTR();
                foreach (PropertyInfo property in entityProperties)
                {
                    if (document.DocumentEntry == 562)
                    {
                        var x = "";
                    }
                    if (property.Name == nameof(document.RelatedSAPDocuments))
                    {
                        document.RelatedSAPDocuments = retrieve_related_documents(document.DocumentEntry);
                        continue;
                    }

                    if (property.Name == nameof(document.RelatedSAPDocumentLines))
                    {
                        document.RelatedSAPDocumentLines = retrieve_related_entry(document.DocumentEntry);
                        continue;
                    }

                    if (property.Name == nameof(document.RelatedBatchLines))
                    {
                        document.RelatedBatchLines = retrieve_related_batch(document.DocumentEntry);
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
                            parsedValue = Convert.ChangeType(parsedValue, propertyType);
                    }
                    else if (columnAttribute != null)
                    {
                        object originalValue = recordSet.GetColumnValue(columnAttribute.ColumnName) ?? property.PropertyType.GetDefaultValue();
                        parsedValue = Convert.ChangeType(originalValue, property.PropertyType);
                    }

                    property.SetValue(document, parsedValue);
                }

                documents.Add(document);
                recordSet.MoveNext();
            }

            return documents;
        }

        private List<RTR1> retrieve_related_documents(int documentEntry)
        {
            var documents = new List<RTR1>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(RELATED_DOCUMENT_QUERY, documentEntry));

            IEnumerable<PropertyInfo> entityProperties = typeof(RTR1).GetProperties()
                .Where(is_sap_property)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new RTR1();
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
                            parsedValue = Convert.ChangeType(parsedValue, propertyType);
                    }
                    else if (columnAttribute != null)
                    {
                        object originalValue = recordSet.GetColumnValue(columnAttribute.ColumnName) ?? property.PropertyType.GetDefaultValue();
                        parsedValue = Convert.ChangeType(originalValue, property.PropertyType);
                    }

                    property.SetValue(document, parsedValue);
                }

                documents.Add(document);
                recordSet.MoveNext();
            }

            return documents;
        }

        private List<RTR2> retrieve_related_entry(int documentEntry)
        {
            var documents = new List<RTR2>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(RELATED_ENTRY_QUERY, documentEntry));

            IEnumerable<PropertyInfo> entityProperties = typeof(RTR2).GetProperties()
                .Where(is_sap_property)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new RTR2();
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
                            parsedValue = Convert.ChangeType(parsedValue, propertyType);
                    }
                    else if (columnAttribute != null)
                    {
                        object originalValue = recordSet.GetColumnValue(columnAttribute.ColumnName) ?? property.PropertyType.GetDefaultValue();
                        parsedValue = Convert.ChangeType(originalValue, property.PropertyType);
                    }

                    property.SetValue(document, parsedValue);
                }

                documents.Add(document);
                recordSet.MoveNext();
            }

            return documents;
        }

        private List<RTR3> retrieve_related_batch(int documentEntry)
        {
            var documents = new List<RTR3>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(RELATED_BATCH_QUERY, documentEntry));

            IEnumerable<PropertyInfo> entityProperties = typeof(RTR3).GetProperties()
                .Where(is_sap_property)
                .ToList();

            while (!recordSet.EoF)
            {
                var document = new RTR3();
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
                            parsedValue = Convert.ChangeType(parsedValue, propertyType);
                    }
                    else if (columnAttribute != null)
                    {
                        object originalValue = recordSet.GetColumnValue(columnAttribute.ColumnName) ?? property.PropertyType.GetDefaultValue();
                        parsedValue = Convert.ChangeType(originalValue, property.PropertyType);
                    }

                    property.SetValue(document, parsedValue);
                }

                documents.Add(document);
                recordSet.MoveNext();
            }

            return documents;
        }

        public override Tuple<bool, string> UpdateBaseEntity(ORTR document)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(ORTR.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", document.DocumentEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                foreach (PropertyInfo property in document.GetType().GetProperties())
                {
                    if (property.Name == "RelatedSAPDocumentLines")
                    {
                        var c = "";
                    }
                    var childAttribute = property.GetCustomAttribute<ChildProperty>();
                    if (childAttribute != null)
                    {
                        GeneralDataCollection dataCollection = generalData.Child(childAttribute.ChildName);
                        var propertyTypeReflectedType = property.PropertyType.GenericTypeArguments[0];
                        var list = property.GetValue(document).To<IList>();
                        int cont = 0;
                        foreach (object @object in list)
                        {
                            PropertyInfo[] nestedProperties = propertyTypeReflectedType.GetProperties();
                            PropertyInfo lineIdProperty = nestedProperties.First(item => item.GetCustomAttribute<ColumnProperty>()?.ColumnName == "LineId");
                            var lineIdValue = lineIdProperty.GetValue(@object).ToInt32();
                            GeneralData data = null;
                            if (dataCollection.Count == 0 || lineIdValue == 0)
                            {
                                data = dataCollection.Add();
                            }
                            else
                            {
                                //data = dataCollection.Item(lineIdValue - 1);
                                data = dataCollection.Item(cont);
                                cont++;
                            }

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
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
        }

        public override Tuple<bool, string> DeleteBatchRelated(ORTR document)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(ORTR.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", document.DocumentEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                foreach (PropertyInfo property in document.GetType().GetProperties())
                {
                    var childAttribute = property.GetCustomAttribute<ChildProperty>();
                    if (childAttribute != null)
                    {

                        if (childAttribute.ChildName == "VS_PD_RTR3")
                        {
                            GeneralDataCollection dataCollection = generalData.Child(childAttribute.ChildName);
                            var propertyTypeReflectedType = property.PropertyType.GenericTypeArguments[0];

                            var list = property.GetValue(document).To<IList>();
                            for (var i = 0; i < list.Count; i++)
                            {
                                var @object = list[i];
                                //@object.
                                if (dataCollection.Count > 0)
                                    dataCollection.Remove(0);
                                //dataCollection.Remove(i);
                            }
                            list.Clear();

                        }
                    }

                }


                generalService.Update(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                return Tuple.Create(false, ex.Message);
            }
            return Tuple.Create(true, "");
        }

        public override Tuple<bool, string> DeleteBatchRelatedByItem(ORTR document, RTR3 documentline)
        {
            try
            {

                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(ORTR.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", documentline.Code);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                GeneralDataCollection dataCollection = generalData.Child("VS_PD_RTR3");
                IList list = null;

                
                foreach (PropertyInfo property in document.GetType().GetProperties())
                {
                    var childAttribute = property.GetCustomAttribute<ChildProperty>();

                    if (childAttribute != null)
                    {
                        if (childAttribute.ChildName == "VS_PD_RTR3")
                        {
                            list = property.GetValue(document).To<IList>();
                            break;
                        }
                    }
                }
                restart:
                //dataCollection = generalData.Child("VS_PD_RTR3");
                var count = list.Count.DeepClone();
                for (int i = 0; i < count; i++)
                {
                    var childLine = dataCollection.Item(i);
                    var codArt = childLine.GetProperty("U_EXK_CDAR").ToString();
                    var linArt = childLine.GetProperty("U_EXK_DCLN").ToInt32();
                    var numDocArt = childLine.GetProperty("U_EXK_DCEN").ToInt32();

                    if (codArt == documentline.CDAR && linArt == documentline.DCLN && numDocArt == documentline.DCEN)
                    {
                        try
                        {
                            dataCollection.Remove(i);
                            list.RemoveAt(i);
                            goto restart;
                        }
                        catch (Exception)
                        {
                            //dataCollection = generalData.Child("VS_PD_RTR3");
                            dataCollection.Remove(i);
                            list.RemoveAt(i);
                            goto restart;
                        }
          
                    }
                }

                generalService.Update(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                return Tuple.Create(false, ex.Message);
            }
            //return Tuple.Create(true, "");
        }
        public override Tuple<bool, string> AddBatchRelated(ORTR document)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(ORTR.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", document.DocumentEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                foreach (PropertyInfo property in document.GetType().GetProperties())
                {
                    var childAttribute = property.GetCustomAttribute<ChildProperty>();
                    if (childAttribute != null)
                    {

                        if (childAttribute.ChildName == "VS_PD_RTR3")
                        {
                            GeneralDataCollection dataCollection = generalData.Child(childAttribute.ChildName);
                            var propertyTypeReflectedType = property.PropertyType.GenericTypeArguments[0];

                            var list = property.GetValue(document).To<IList>();
                            foreach (var o in list)
                            {
                                //var @object = list[i];
                                var nestedProperties = propertyTypeReflectedType.GetProperties();
                                GeneralData data;
                                data = dataCollection.Add();
                                foreach (var propertyInfo in nestedProperties.Where(item => item.GetCustomAttribute<FieldNoRelated>() != null))
                                {
                                    var fieldRelatedAttribute = propertyInfo.GetCustomAttribute<FieldNoRelated>();
                                    var value = propertyInfo.GetValue(o);
                                    if (value is decimal || value is float)
                                        value = Convert.ChangeType(value, typeof(double));

                                    data.SetValueIfNotNull(fieldRelatedAttribute.sAliasID, value);
                                }
                            }
                        }
                    }

                }
                //GeneralDataCollection dataCollection = generalData.Child("VS_PD_RTR3");
                //var propertyTypeReflectedType = property.PropertyType.GenericTypeArguments[0];
                //var list = property.GetValue(document).To<IList>();
                //foreach (var o in list)
                //{
                //    var @object = list[i];
                //    var nestedProperties = propertyTypeReflectedType.GetProperties();
                //    GeneralData data;
                //    data = dataCollection.Add();
                //    foreach (var propertyInfo in nestedProperties.Where(item => item.GetCustomAttribute<FieldNoRelated>() != null))
                //    {
                //        var fieldRelatedAttribute = propertyInfo.GetCustomAttribute<FieldNoRelated>();
                //        var value = propertyInfo.GetValue(@object);
                //        if (value is decimal || value is float)
                //            value = Convert.ChangeType(value, typeof(double));

                //        data.SetValueIfNotNull(fieldRelatedAttribute.sAliasID, value);
                //    }
                //}


                generalService.Update(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                return Tuple.Create(false, ex.Message);
            }
            return Tuple.Create(true, "");
        }
        public override Tuple<bool, string> Close(int documentEntry)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(ORTR.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", documentEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);
                generalData.SetValueIfNotNull("U_EXK_STAD", ORTR.Status.CLOSED);
                generalService.Update(generalData);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, exception.Message);
            }
        }

        public override Tuple<bool, string> Postponed(int documentEntry)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(ORTR.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("DocEntry", documentEntry);
                GeneralData generalData = generalService.GetByParams(generalDataParams);
                generalData.SetValueIfNotNull("U_EXK_STAD", ORTR.Status.POSTPONED);
                generalService.Update(generalData);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, exception.Message);
            }
        }

        public override Tuple<bool, string> Remove(int documentEntry)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ORTR> RetrieveByRoute(int codRoute)
        {
            //string whereStatement = QueryHelper.ParseToHANAQuery(expression);
            string query = $"Select A.\"DocEntry\" as \"U_PD_VS_CVRF\", R.*   from \"@VS_PD_ORTR\"  R join \"@VS_PD_ARD1\" A on A.\"U_EXK_CDTR\" = R.\"DocEntry\" where A.\"DocEntry\" = {codRoute}";
            return documents_from_query(query);

        }


    }
}