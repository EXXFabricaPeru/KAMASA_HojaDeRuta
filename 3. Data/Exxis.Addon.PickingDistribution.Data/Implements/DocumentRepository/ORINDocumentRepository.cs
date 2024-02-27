using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository
{
    // ReSharper disable once InconsistentNaming
    public class ORINDocumentRepository : BaseSAPDocumentRepository<ORIN, RIN1>
    {
        private const string TOTAL_RETURN_MOTIVE = "07";
        private const string INTERNAL_SALE_MOTIVE = "0101";
        private const string SUNAT_TYPE_CLIENT_INVOICE = "01";

        private struct InvoiceReference
        {
            public string SUNATType { get; set; }
            public string Series { get; set; }
            public string Correlative { get; set; }
            public DateTime Emission { get; set; }
            public double TotalAmount { get; set; }
        }

        public ORINDocumentRepository(Company company) : base(company)
        {
        }

        public override Tuple<bool, string> RegisterDocument(ORIN entity)
        {
            try
            {
                Company.StartTransaction();
                var document = Company.GetBusinessObject(BoObjectTypes.oCreditNotes).To<Documents>();
                document.CardCode = entity.CardCode;
                document.DocDate = entity.DocumentDate;
                document.DocDueDate = entity.DocumentDeliveryDate;
                // document.DocTotal = entity.TotalPrice.ToDouble();
                document.JournalMemo = entity.CardName;
                string series = get_document_series();
                string correlative = get_document_correlative(series);
                document.NumAtCard = build_number_at_card(series, correlative);

                InvoiceReference invoiceReference = get_invoice_reference(entity.ReferenceInvoiceDocumentEntry);
                document.UserFields.Fields.Item("U_BPV_SERI").Value = series;
                document.UserFields.Fields.Item("U_BPV_NCON2").Value = correlative;
                document.UserFields.Fields.Item("U_BPP_MDTO").Value = invoiceReference.SUNATType;
                document.UserFields.Fields.Item("U_BPP_MDSO").Value = invoiceReference.Series;
                document.UserFields.Fields.Item("U_BPP_MDCO").Value = invoiceReference.Correlative;
                document.UserFields.Fields.Item("U_BPP_SDOCDATE").Value = invoiceReference.Emission;
                document.UserFields.Fields.Item("U_VS_SDOCTOTAL").Value = invoiceReference.TotalAmount;
                document.UserFields.Fields.Item("U_VS_SUSTNT").Value = "Devolución por ítem";// get_description_return_motive();
                document.UserFields.Fields.Item("U_VS_MOTEMI").Value = TOTAL_RETURN_MOTIVE;
                //document.UserFields.Fields.Item("U_VS_TIPO_FACT").Value = INTERNAL_SALE_MOTIVE;
                document.UserFields.Fields.Item("U_VS_TDOCORG").Value = SAPCodeType.INVOICE.ToString();
                document.UserFields.Fields.Item("U_VS_DOCORG").Value = entity.ReferenceInvoiceDocumentEntry;

                Document_Lines documentLines = document.Lines;
                entity.DocumentLines.ForEach((line, index, lastIteration) =>
                {
                    documentLines.BaseType = line.DocumentTypeReferenced;
                    documentLines.BaseEntry = line.DocumentEntryReferenced;
                    documentLines.BaseLine = line.LineNumberReferenced;
                    documentLines.Quantity = line.Quantity.ToDouble();
                    documentLines.ItemCode = line.ItemCode;

                    BatchNumbers batchNumbers = documentLines.BatchNumbers;
                    line.SelectedBatches.ForEach((batch, batchIndex, batchLastIteration) =>
                    {
                        batchNumbers.BatchNumber = batch.BatchNumber;
                        batchNumbers.Quantity = batch.Quantity.ToDouble();
                        batchLastIteration.IfFalse(() => batchNumbers.Add());
                    });

                    lastIteration.IfFalse(() => documentLines.Add());
                });

                int operationResult = document.Add();
                if (operationResult.IsDefault())
                {
                    string key;
                    Company.GetNewObjectCode(out key);
                    if (Company.InTransaction)
                        Company.EndTransaction(BoWfTransOpt.wf_Commit);
                    return Tuple.Create(true, key);
                }

                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                return Tuple.Create(false, Company.GetLastErrorDescription());
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
        }

        private string get_document_series()
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(
                "SELECT nlc.\"Code\" FROM \"@BPVSN_NROLEGC\" AS nlc LEFT JOIN \"@BPVSN_NROLEGO\" AS nlo ON nlo.\"Code\" = nlc.\"Code\" WHERE nlo.\"U_BP_FRMSAP\" = '179'");
            if (recordSet.EoF)
                throw new Exception(
                    @"No se ha registrado la numeración legal para las Notas de Crédito, hacerlas en el formulario 'bpvs: Númerción Legal'");
            return recordSet.GetColumnValue(0).ToString();
        }

        private string get_document_correlative(string series)
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(
                $"SELECT T0.\"U_BP_NROACT\" FROM \"@BPVSN_NROLEGC\" AS T0 LEFT JOIN \"@BPVSN_NROLEGO\" AS T1 ON T1.\"Code\" = T0.\"Code\" WHERE T1.\"U_BP_FRMSAP\" = '179' AND T0.\"Code\" = '{series}'");
            return recordSet.GetColumnValue(0).ToString();
        }

        private string build_number_at_card(string series, string correlative)
        {
            string[] splitSeries = series.Split('-');
            return $"{splitSeries[1]}{splitSeries[0]}-{correlative}";
        }

        private InvoiceReference get_invoice_reference(int documentEntry)
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery($"select \"U_BPP_MDSD\", \"U_BPP_MDCD\", \"DocDate\", \"DocTotal\" from \"OINV\" where \"DocEntry\" = {documentEntry}");

            return new InvoiceReference
            {
                SUNATType = SUNAT_TYPE_CLIENT_INVOICE,
                Series = recordSet.GetStringValue("U_BPP_MDSD"),
                Correlative = recordSet.GetStringValue("U_BPP_MDCD"),
                Emission = recordSet.GetColumnValue(2).ToDateTime(),
                TotalAmount = recordSet.GetColumnValue(3).ToDouble()
            };
        }

        private string get_description_return_motive()
        {
            try
            {
                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                recordSet.DoQuery($"select \"U_VS_DESC\" from \"@VS_CE09\" where \"Code\" = '{TOTAL_RETURN_MOTIVE}'");
                return recordSet.GetColumnValue(0).ToString();
            }
            catch (Exception)
            {
                throw new Exception(@"Error al traer información de 'C09: Tipo NC Electronica' con código 06");
            }
        }
    }
}