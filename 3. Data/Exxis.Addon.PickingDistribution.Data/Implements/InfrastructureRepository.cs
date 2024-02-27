using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.DisposableBO;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Local;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using VSVersionControl.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class InfrastructureRepository : BaseInfrastructureRepository
    {
        private const string DESCRIPTION_VALID_VALUE_QUERY =
            "select distinct DFD.\"Descr\" from \"CUFD\" UDF, \"UFD1\" DFD where UDF.\"FieldID\" = DFD.\"FieldID\" and UDF.\"TableID\" = DFD.\"TableID\" and UDF.\"AliasID\" = '{0}' and DFD.\"FldValue\" = '{1}'";

        private const string DISAPPROVAL_REASONS_QUERY = "select * from	\"@VS_PD_OMTT\"";

        private const string GET_VALID_VALUE_QUERY =
            "select \"UFD1\".\"FldValue\",\"UFD1\".\"Descr\" from \"CUFD\", \"UFD1\" where \"CUFD\".\"TableID\" = \"UFD1\".\"TableID\" and \"CUFD\".\"FieldID\" = \"UFD1\".\"FieldID\" and \"CUFD\".\"TableID\" = '{0}' and \"CUFD\".\"AliasID\" = '{1}'";

        public InfrastructureRepository(Company company) : base(company)
        {
        }

        public override IEnumerable<OMTT> RetrieveReasons()
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(DISAPPROVAL_REASONS_QUERY);
            IList<OMTT> transfer = new List<OMTT>();
            while (!recordSet.EoF)
            {
                var mapping = new OMTT
                {
                    Code = recordSet.GetColumnValue(@"Code").ToString(),
                    Name = recordSet.GetColumnValue(@"Name").ToString(),
                    Type = recordSet.GetColumnValue(@"U_EXK_COTP")?.ToString() ?? string.Empty
                };
                transfer.Add(mapping);
                recordSet.MoveNext();
            }
            transfer = transfer.OrderBy(t => t.Code).ToList();
            return transfer;
        }

        public override OMTT RetrieveDisapprovalReasonByCode(string code)
        {
            var recordSet = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            recordSet.DoQuery($"{DISAPPROVAL_REASONS_QUERY} where \"Code\" = '{code}'");
            if (recordSet.RecordCount == 0)
                throw new Exception($"El código '{code}' no esta registrado en la Configuración de Motivos.");

            Fields fields = recordSet.Fields;
            Field field = fields.Item("Code");
            var reasonCode = field.Value.ToString();
            field = fields.Item("Name");
            var reasonName = field.Value.ToString();

            var reason = new OMTT
            {
                Code = reasonCode,
                Name = reasonName
            };

            GenericHelper.ReleaseCOMObjects(field, fields, recordSet);
            return reason;
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
            recordSet.DoQuery(
            // "select \"UFD1\".\"FldValue\" as \"Code\", \"UFD1\".\"Descr\" as \"Description\" from \"CUFD\", \"UFD1\" where \"CUFD\".\"TableID\" = \"UFD1\".\"TableID\" and \"CUFD\".\"FieldID\" = \"UFD1\".\"FieldID\" and \"CUFD\".\"TableID\" = 'ORDR' and \"CUFD\".\"AliasID\" = 'CL_CANAL'");

            "select \"Code\", \"Name\" as \"Description\" from \"@VS_LP_CANALVENTA\" ");
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

        public override IEnumerable<Tuple<string, string>> RetrieveValidValues<TEntity, TKProperty>(
            Expression<Func<TEntity, TKProperty>> propertyExpression)
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(build_valid_values_query(propertyExpression));
            IList<Tuple<string, string>> result = new List<Tuple<string, string>>();
            while (!recordSet.EoF)
            {
                var code = recordSet.GetColumnValue("FldValue").ToString();
                var description = recordSet.GetColumnValue("Descr").ToString();
                result.Add(Tuple.Create(code, description));
                recordSet.MoveNext();
            }

            return result;
        }

        public override IEnumerable<SAPSelectedBatch> RetrieveBatchesFromDocument(int documentEntry, int lineNumber, int sapType,
            string warehouseCode)
        {
            IList<SAPSelectedBatch> result = new List<SAPSelectedBatch>();
            const string batchQuery =
                "select CAB.\"ItemCode\", CAB.\"BatchNum\", CAB.\"WhsCode\", CAB.\"ExpDate\", CAB.\"PrdDate\", DET.\"Quantity\" from \"OIBT\" CAB, \"IBT1\" DET where DET.\"BaseEntry\" = {0} and DET.\"BaseType\" = {1} and DET.\"BaseLinNum\" = {2} and CAB.\"WhsCode\" = '{3}' and CAB.\"BatchNum\" = DET.\"BatchNum\" and CAB.\"ItemCode\" = DET.\"ItemCode\" order by DET.\"LineNum\"";
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(batchQuery, documentEntry, sapType, lineNumber, warehouseCode));
            while (!recordSet.EoF)
            {
                var batch = new SAPSelectedBatch();
                batch.ItemCode = recordSet.GetColumnValue("ItemCode").ToString();
                batch.BatchNumber = recordSet.GetColumnValue("BatchNum").ToString();
                batch.WarehouseCode = recordSet.GetColumnValue("WhsCode").ToString();
                batch.Expiry = recordSet.GetColumnValue("ExpDate").ToDateTime();
                batch.Quantity = recordSet.GetColumnValue("Quantity").ToDecimal();
                result.Add(batch);
                recordSet.MoveNext();
            }

            return result;
        }

        public override IEnumerable<SAPSelectedBatch> RetrieveBatchesFromRequestDocument(int documentEntry, int lineNumber, int sapType,
            string warehouseCode)
        {
            IList<SAPSelectedBatch> result = new List<SAPSelectedBatch>();
            const string batchQuery = "select A.\"ItemCode\", A.\"BatchNum\", A.\"WhsCode\", A.\"ExpDate\", A.\"PrdDate\", B.\"Qty\" " +
                                      "from (select CAB.\"ItemCode\", CAB.\"BatchNum\", CAB.\"WhsCode\", CAB.\"ExpDate\", CAB.\"PrdDate\", DET.\"BaseLinNum\" " +
                                      "from \"OIBT\" CAB, \"IBT1\" DET " +
                                      "where CAB.\"BatchNum\" = DET.\"BatchNum\" " +
                                      "and CAB.\"ItemCode\" = DET.\"ItemCode\" " +
                                      "and DET.\"BaseEntry\" = {0} " +
                                      "and DET.\"BaseLinNum\" = {1} " +
                                      "and DET.\"BaseType\" = {2} " +
                                      "and CAB.\"WhsCode\" = '{3}') A, " +
                                      "(select \"DocLine\", sum(\"DocQty\") as \"Qty\" " +
                                      "from \"OITL\" " +
                                      "where \"DocType\" = {2} " +
                                      "and \"DocEntry\" = {0} " +
                                      "group by \"DocLine\") B " +
                                      "where A.\"BaseLinNum\" = B.\"DocLine\"";
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(batchQuery, documentEntry, lineNumber, sapType, warehouseCode));
            while (!recordSet.EoF)
            {
                var batch = new SAPSelectedBatch();
                batch.ItemCode = recordSet.GetColumnValue("ItemCode").ToString();
                batch.BatchNumber = recordSet.GetColumnValue("BatchNum").ToString();
                batch.WarehouseCode = recordSet.GetColumnValue("WhsCode").ToString();
                batch.Expiry = recordSet.GetColumnValue("ExpDate").ToDateTime();
                batch.Quantity = recordSet.GetColumnValue("Qty").ToDecimal();
                result.Add(batch);
                recordSet.MoveNext();
            }

            return result;//1212101
        }

        public override IEnumerable<Tuple<string, string>> RetrieveDocumentFields()
        {
            Documents documents = null;
            UserFields userFields = null;
            Fields fields = null;
            Field field = null;

            try
            {
                IList<Tuple<string, string>> result = typeof(IDocuments)
                    .GetProperties()
                    .Where(t => t.PropertyType.IsValueType || t.PropertyType == typeof(string) || t.PropertyType == typeof(BoYesNoEnum))
                    .Select(t => Tuple.Create(t.Name, $"{t.PropertyType.FullName} {t.Name}"))
                    .ToList();

                documents = Company.GetBusinessObject(BoObjectTypes.oOrders)
                    .To<Documents>();

                userFields = documents.UserFields;
                fields = userFields.Fields;

                for (var i = 0; i < fields.Count; i++)
                {
                    field = fields.Item(i);
                    result.Add(Tuple.Create(field.Name, field.Description));
                }

                return result;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(field, fields, userFields, documents);
            }
        }

        public override IEnumerable<Tuple<string, string>> RetrieveLineDocumentFields()
        {
            Documents documents = null;
            Document_Lines documentLines = null;
            UserFields userFields = null;
            Fields fields = null;
            Field field = null;

            try
            {
                IList<Tuple<string, string>> result = typeof(IDocument_Lines)
                    .GetProperties()
                    .Where(t => t.PropertyType.IsValueType || t.PropertyType == typeof(string) || t.PropertyType == typeof(BoYesNoEnum))
                    .Select(t => Tuple.Create(t.Name, $"{t.PropertyType.FullName} {t.Name}"))
                    .ToList();

                documents = Company.GetBusinessObject(BoObjectTypes.oOrders)
                    .To<Documents>();
                documentLines = documents.Lines;

                userFields = documentLines.UserFields;
                fields = userFields.Fields;

                for (var i = 0; i < fields.Count; i++)
                {
                    field = fields.Item(i);
                    result.Add(Tuple.Create(field.Name, field.Description));
                }

                return result;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(field, fields, userFields, documents, documentLines);
            }
        }

        public override IEnumerable<Tuple<string, string>> RetrieveLineLiquidationCardsFields()
        {


            try
            {
                List<Tuple<string, string>> result = new List<Tuple<string, string>>();

                foreach (var item in ListaSAPPlantilla.staticTupleList)
                {
                    result.Add(Tuple.Create(item.Item1, item.Item2));
                }
                //result.Add(Tuple.Create("U_VS_LT_NREF", "Nro. Referencia de Cobro"));
                //result.Add(Tuple.Create("U_VS_LT_FECO", "Fecha Cobro"));
                //result.Add(Tuple.Create("U_VS_LT_NTAR", "Nro.  de Tarjeta"));
                //result.Add(Tuple.Create("U_VS_LT_IMTA", "Importe Cobrado Tarjeta"));
                //result.Add(Tuple.Create("U_VS_LT_COMI", "Comisión"));
                //result.Add(Tuple.Create("U_VS_LT_COEM", "Comisión Emisor"));
                //result.Add(Tuple.Create("U_VS_LT_COMP", "Comisión MCPeru"));
                //result.Add(Tuple.Create("U_VS_LT_IGVT", "IGV"));
                //result.Add(Tuple.Create("U_VS_LT_NTPA", "Neto Parcial"));

                return result;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override IEnumerable<OEIT> RetrieveMappingValuesByTemplate(string templateCode)
        {
            var mappingQuery = $"select * from \"@VS_PD_OEIT\" where \"U_EXK_TPCD\" = '{templateCode}'";
            using (SafeRecordSet recordSet = Company.MakeSafeRecordSet())
                return recordSet.RetrieveListFromQuery<OEIT>(mappingQuery);
        }

       
        private string build_valid_values_query<TEntity, TKProperty>(
            Expression<Func<TEntity, TKProperty>> propertyExpression)
        {
            var tableName = get_table_name(typeof(TEntity));
            var memberExpression = propertyExpression.Body.To<MemberExpression>();
            var columnName = memberExpression.Member.GetCustomAttribute<FieldNoRelated>().sAliasID.SubstringStartAt(2);
            return string.Format(GET_VALID_VALUE_QUERY, tableName, columnName);
        }

        private string get_table_name(Type tableType)
        {
            const string tableNamePrefix = "@";

            var tableUdo = tableType.GetCustomAttribute<Udo>();
            if (tableUdo != null)
                return tableNamePrefix + tableUdo.UdoName;

            var userDefinedTable = tableType.GetCustomAttribute<UserDefinedTable>();
            if (userDefinedTable != null)
                return tableNamePrefix + userDefinedTable.Name;

            return string.Empty;
        }

        public override string RetrieveLastNumberBySerie(string Serie, string tipo)
        {
            try
            {
                var bf = "";
                if (tipo == "F")
                    bf = "25";
                else if (tipo == "B")
                    bf = "14";
                else if (tipo == "G")
                    bf = "16";

                var recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                string query = $"CALL \"bpvs_LC_BF_OP_VENTAS\"('13','{bf}','N','{Serie}','P','','','')";
                recordSet.DoQuery(query);

                string correlativo = "";
                while (!recordSet.EoF)
                {
                    correlativo = recordSet.Fields.Item("U_BP_NROACT").Value.ToString();
                    //documentValid.UserFields.Fields.Item("U_BPV_NCON2").Value = correlativo;
                    //documents.Add(document);
                    recordSet.MoveNext();

                }


                return correlativo;
            }
            catch (Exception)
            {
                return "";

            }
        }

        public override List<Tuple<string, string>> RetriveMotiveTransferSoraya()
        {
            var qry = "select * from \"@VS_PD_MOTR\" ";
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(qry);
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            while (!recordSet.EoF)
            {
                var code = recordSet.GetColumnValue("Code").ToString();
                var description = recordSet.GetColumnValue("Name").ToString();
                result.Add(Tuple.Create(code, description));
                recordSet.MoveNext();
            }


            return result;
            
        }

        public override List<Tuple<string, string>> RetriveMotiveTransferSunat()
        {
            var qry = "select * from \"@VS_CE20\" ";
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(qry);

            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            while (!recordSet.EoF)
            {
                var code = recordSet.GetColumnValue("Code").ToString();
                var description = recordSet.GetColumnValue("U_VS_DESC").ToString();
                result.Add(Tuple.Create(code, description));
                recordSet.MoveNext();
            }


            return result;
        }


        public override string RetrievePaymentGroupNumDescription(int code)
        {
            var qry = $@"select * from ""OCTG"" where ""GroupNum""='{code.ToString()}'";
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(qry);
            while (!recordSet.EoF)
            {
                var description = recordSet.GetColumnValue("PymntGroup").ToString();
                return description;
                recordSet.MoveNext();
            }
            return null;
        }

        public override Tuple<int, int> RetrievePaymentGroupNumDetail(int code)
        {
            var qry = $@"select * from ""OCTG"" where ""GroupNum""='{code.ToString()}'";
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(qry);
            while (!recordSet.EoF)
            {
                var months = recordSet.GetColumnValue("ExtraMonth").ToString().ToInt32();
                var days = recordSet.GetColumnValue("ExtraDays").ToString().ToInt32();

                return Tuple.Create(months, days);
                recordSet.MoveNext();
            }
            return null;
        }

        public override IEnumerable<Tuple<string, string>> RetrieveTipoFlujos()
        {
            try
            {
                List<Tuple<string, string>> result = new List<Tuple<string, string>>();

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select * from \"OCFW\" where \"CFWId\"<>1 ";
                recordSet.DoQuery(string.Format(query));

                while (!recordSet.EoF)
                {
                    var Code = recordSet.GetColumnValue("CFWId").ToString();
                    var Name = recordSet.GetColumnValue("CFWName").ToString();
                    result.Add(Tuple.Create(Code, Name));
                    recordSet.MoveNext();
                }

                //    foreach (var item in list.Result)
                //{
                //    result.Add(Tuple.Create(item.Code, item.Name));
                //}

                return result;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }
    }
}