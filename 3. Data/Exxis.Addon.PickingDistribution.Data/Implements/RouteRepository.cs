using System;
using System.Linq;
using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class RouteRepository : BaseRouteRepository
    {
        private const string DESCRIPTION_VALID_VALUE_QUERY =
            "select distinct DFD.\"Descr\" from \"CUFD\" UDF, \"UFD1\" DFD where UDF.\"FieldID\" = DFD.\"FieldID\" and UDF.\"TableID\" = DFD.\"TableID\" and UDF.\"AliasID\" = '{0}' and DFD.\"FldValue\" = '{1}'";

        private const string DISAPPROVAL_REASONS_QUERY = "select * from	\"@VS_PD_OARD\"";
        public RouteRepository(Company company) : base(company)
        {
        }

        public override Tuple<bool, string> Register(OARD entity)
        {
            try
            {
                Company.StartTransaction();
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
                    generalData.SetValueIfNotNull(property.Item2, property.Item1.GetValue(entity));

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
                        childGeneralData.SetValueIfNotNull(property.Item2, property.Item1.GetValue(transferOrder));
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

        public override IEnumerable<OARD> RetrieveDisapprovalReasons()
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(DISAPPROVAL_REASONS_QUERY);
            IList<OARD> transfer = new List<OARD>();
            while (!recordSet.EoF)
            {
                var mapping = new OARD
                {
                    //Code = recordSet.GetColumnValue("Code").ToString(),
                    //Name = recordSet.GetColumnValue("Name").ToString()
                };
                transfer.Add(mapping);
                recordSet.MoveNext();
            }

            return transfer;
        }

        public override OARD RetrieveDisapprovalReasonByCode(string code)
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery($"{DISAPPROVAL_REASONS_QUERY} where \"Code\" = '{code}'");
            return new OARD
            {
                //Code = recordSet.GetColumnValue("Code").ToString(),
                //Name = recordSet.GetColumnValue("Name").ToString()
            };
        }

        public override string RetrieveDescriptionOfValidValueCode(string field, string code)
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(DESCRIPTION_VALID_VALUE_QUERY, field, code));
            return recordSet.GetColumnValue("Descr").ToString();
        }

        public override IEnumerable<Tuple<string, string>> RetrieveSaleChannels()
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery("select \"UFD1\".\"FldValue\" as \"Code\", \"UFD1\".\"Descr\" as \"Description\" from \"CUFD\", \"UFD1\" where \"CUFD\".\"TableID\" = \"UFD1\".\"TableID\" and \"CUFD\".\"FieldID\" = \"UFD1\".\"FieldID\" and \"CUFD\".\"TableID\" = 'ORDR' and \"CUFD\".\"AliasID\" = 'CL_CANAL'");
            IList<Tuple<string, string>> result = new List<Tuple<string, string>>();
            while (!recordSet.EoF)
            {
                var code = recordSet.GetColumnValue("Code").ToString();
                var description = recordSet.GetColumnValue("Description").ToString();
                result.Add(Tuple.Create(code, description));
                recordSet.MoveNext();
            }

            return result;
        }
    }
}
