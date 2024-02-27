using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository
{
    // ReSharper disable once InconsistentNaming
    public class ORRRDocumentRepository : BaseSAPDocumentRepository<ORRR, RRR1>
    {
        private const string TAX_CODE = "I18";
        private const string TIPO_OPERACION = "24";

        public ORRRDocumentRepository(Company company) : base(company)
        {
        }

        public override Tuple<bool, string> RegisterDocument(ORRR entity)
        {
            try
            {
                Company.StartTransaction();
                var document = Company.GetBusinessObject(BoObjectTypes.oReturnRequest).To<Documents>();
                document.CardCode = entity.CardCode;
                document.DocDate = entity.DocumentDate;
                document.DocDueDate = entity.DocumentDeliveryDate;
                document.NumAtCard = entity.NumberAtCard;

                Document_Lines documentLines = document.Lines;
                entity.DocumentLines.ForEach((line, lineIndex, lastLineIteration) =>
                {
                    documentLines.ItemCode = line.ItemCode;
                    documentLines.Quantity = line.Quantity.ToDouble();
                    documentLines.UnitPrice = line.UnitPrice.ToDouble();
                    documentLines.TaxCode = TAX_CODE;
                    documentLines.UserFields.Fields.Item("U_tipoOpT12").Value = TIPO_OPERACION;
                    documentLines.BaseType = line.DocumentTypeReferenced;
                    documentLines.BaseEntry = line.DocumentEntryReferenced;
                    documentLines.BaseLine = line.LineNumberReferenced;

                    //cambiar de almacén temporal a original;
                    documentLines.WarehouseCode = line.WarehouseCode;

                    BatchNumbers batchNumbers = documentLines.BatchNumbers;
                    line.SelectedBatches.ForEach((batch, batchIndex, lastBatchIteration) =>
                    {
                        batchNumbers.BatchNumber = batch.BatchNumber;
                        batchNumbers.Quantity = batch.Quantity.ToDouble();
                        lastBatchIteration.IfFalse(() => batchNumbers.Add());
                    });

                    lastLineIteration.IfFalse(() => documentLines.Add());
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

        public override IEnumerable<ORRR> RetrieveDocuments(Expression<Func<ORRR, bool>> expression)
        {
            IList<ORRR> documents = base.RetrieveDocuments(expression).ToList();
            foreach (ORRR document in documents)
            foreach (RRR1 line in document.DocumentLines)
                line.SelectedBatches = retrieve_batches(document.DocumentEntry, line.VisOrder, line.WarehouseCode);
            
            return documents;
        }

        private IEnumerable<SAPSelectedBatch> retrieve_batches(int requestEntry, int lineNumber, string warehouseCode)
        {
            IList<SAPSelectedBatch> result = new List<SAPSelectedBatch>();
            var documents = Company.GetBusinessObject(BoObjectTypes.oReturnRequest).To<Documents>();
            documents.GetByKey(requestEntry);
            Document_Lines documentLines = documents.Lines;
            documentLines.SetCurrentLine(lineNumber);
            BatchNumbers batchNumbers = documentLines.BatchNumbers;
            for (var i = 0; i < batchNumbers.Count; i++)
            {
                batchNumbers.SetCurrentLine(i);
                result.Add(new SAPSelectedBatch
                {
                    BatchNumber = batchNumbers.BatchNumber,
                    ItemCode = batchNumbers.ItemCode,
                    Quantity = batchNumbers.Quantity.ToDecimal(),
                    WarehouseCode = warehouseCode
                });
            }

            return result;
        }
    }
}