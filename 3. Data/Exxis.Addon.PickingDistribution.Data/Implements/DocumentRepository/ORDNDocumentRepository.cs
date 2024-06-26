﻿// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository
{
    public class ORDNDocumentRepository : BaseSAPDocumentRepository<ORDN, RDN1>
    {
        private const string OTHERS_DEVOLUTION_TYPE = "24";

        public ORDNDocumentRepository(Company company) : base(company)
        {
        }

        public override Tuple<bool, string> RegisterDocument(ORDN entity)
        {
            try
            {
                Company.StartTransaction();
                var document = Company.GetBusinessObject(BoObjectTypes.oReturns).To<Documents>();
                document.CardCode = entity.CardCode;
                document.DocDate = entity.DocumentDate;
                document.DocDueDate = entity.DocumentDeliveryDate;
                document.JournalMemo = entity.CardName;
                
                string series = get_document_series();
                string correlative = get_correlative(series);
                document.NumAtCard = build_number_at_card(series, correlative);

                Fields userFields = document.UserFields.Fields;
                userFields.Item("U_BPV_SERI").Value = series;
                userFields.Item("U_BPV_NCON2").Value = correlative;

                Document_Lines sapDocumentLines = document.Lines;
                entity.DocumentLines.ForEach((line, index, lastIteration) => set_line(line, lastIteration, ref sapDocumentLines));
                
                int documentEntry = document.Add();

                if (documentEntry == 0)
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

        public override Tuple<bool, string> RegisterDocumentDraft(ORDN entity)
        {
            try
            {
                Company.StartTransaction();
                var document = Company.GetBusinessObject(BoObjectTypes.oDrafts).To<Documents>();
                document.CardCode = entity.CardCode;
                document.DocDate = entity.DocumentDate;
                document.DocDueDate = entity.DocumentDeliveryDate;
                document.JournalMemo = entity.CardName;
                document.DocObjectCode = BoObjectTypes.oReturns;

                document.DocumentReferences.ReferencedObjectType = entity.ReferencedObjectType== SAPCodeType.DELIVERY? ReferencedObjectTypeEnum.rot_DeliveryNotes:ReferencedObjectTypeEnum.rot_SalesInvoice ; //ReferencedObjectTypeEnum.rot_SalesOrder
                document.DocumentReferences.ReferencedDocEntry = entity.ReferencedDocEntry; // document.DocumentLines.FirstOrDefault().BaseEntry;
                document.DocumentReferences.Add();

                if(entity.ReferencedObjectType == SAPCodeType.INVOICE)
                {
                    document.DocumentReferences.ReferencedObjectType =  ReferencedObjectTypeEnum.rot_DeliveryNotes; //ReferencedObjectTypeEnum.rot_SalesOrder
                    document.DocumentReferences.ReferencedDocEntry = entity.ReferencedDocEntryGuia; // document.DocumentLines.FirstOrDefault().BaseEntry;
                    document.DocumentReferences.Add();
                }

                string series = get_document_series();
                string correlative = get_correlative(series);
                document.NumAtCard = build_number_at_card(series, correlative);

              
                

                try
                {
                    
                  
                }
                catch (Exception)
                {
                    
                }

                document.Comments = entity.Comments;


                Document_Lines sapDocumentLines = document.Lines;
                entity.DocumentLines.ForEach((line, index, lastIteration) => set_line(line, lastIteration, ref sapDocumentLines));

                int documentEntry = document.Add();

                if (documentEntry == 0)
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
            recordSet.DoQuery("select nlc.\"Code\" from \"@BPVSN_NROLEGC\" as nlc left join \"@BPVSN_NROLEGO\" as nlo on nlo.\"Code\" = nlc.\"Code\" where nlo.\"U_BP_FRMSAP\" = '180'");
            return recordSet.GetColumnValue(0).ToString();
        }

        private string get_correlative(string series)
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery($"select T0.\"U_BP_NROACT\" from \"@BPVSN_NROLEGC\" as T0 left join \"@BPVSN_NROLEGO\" as T1 on T1.\"Code\" = T0.\"Code\" where T1.\"U_BP_FRMSAP\" = '180' and T0.\"Code\" = '{series}'");
            return recordSet.GetColumnValue(0).ToString();
            
        }

        private string build_number_at_card(string series, string correlative)
        {
            string[] splitSeries = series.Split('-');
            return $"{splitSeries[1]}{splitSeries[0]}-{correlative}";
        }


        private void set_line(RDN1 line, bool lastIteration, ref Document_Lines sapDocumentLines)
        {
            Document_Lines documentLines = sapDocumentLines;
            if (line.DocumentEntryReferenced != 0)
            {
                documentLines.BaseType = line.DocumentTypeReferenced;
                documentLines.BaseEntry = line.DocumentEntryReferenced;
                documentLines.BaseLine = line.LineNumberReferenced;
            }


            documentLines.UnitPrice = line.UnitPrice.ToDouble();
            documentLines.WarehouseCode = line.WarehouseCode;
            documentLines.Quantity = line.Quantity.ToDouble();
            documentLines.ItemCode = line.ItemCode;
            documentLines.UserFields.Fields.Item("U_tipoOpT12").Value = OTHERS_DEVOLUTION_TYPE;

            documentLines.CostingCode2 = line.CentroCosto2;
            documentLines.CostingCode3 = line.CentroCosto3;
            documentLines.CostingCode4 = line.CentroCosto4;
            documentLines.CostingCode = line.CentroCosto;

            documentLines.COGSCostingCode = line.COGSCostingCode;
            documentLines.COGSCostingCode2 = line.COGSCostingCode2;
            documentLines.COGSCostingCode3 = line.COGSCostingCode3;
            documentLines.COGSCostingCode4 = line.COGSCostingCode4;


            BatchNumbers sapBatchNumbers = documentLines.BatchNumbers;
            line.SelectedBatches.ForEach((batch, index, lastBatchIteration) => set_batch(batch, lastBatchIteration, ref sapBatchNumbers));
            
            lastIteration.IfFalse(() => documentLines.Add());
        }

        private void set_batch(SAPSelectedBatch batch, bool lastIteration, ref BatchNumbers sapBatchNumbers)
        {
            BatchNumbers batchNumbers = sapBatchNumbers;
            batchNumbers.BatchNumber = batch.BatchNumber;
            batchNumbers.Quantity = batch.Quantity.ToDouble();
            lastIteration.IfFalse(() => batchNumbers.Add());
        }

    }
}