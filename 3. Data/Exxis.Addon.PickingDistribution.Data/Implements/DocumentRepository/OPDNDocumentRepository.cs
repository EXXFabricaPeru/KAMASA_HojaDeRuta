// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository
{
    public class OPDNDocumentRepository : BaseSAPDocumentRepository<OPDN, PDN1>
    {
        public OPDNDocumentRepository(Company company) : base(company)
        {
        }

        public override Tuple<bool, string> RegisterDocument(OPDN entity)
        {
            try
            {
                Company.StartTransaction();
                var document = Company.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes).To<Documents>();
                document.CardCode = entity.CardCode;
                document.DocDate = entity.DocumentDate;
                document.DocDueDate = entity.DocumentDeliveryDate;
                document.JournalMemo = entity.JrnlMemo;
                Fields userFields = document.UserFields.Fields;
                userFields.Item("U_VS_AFEDET").Value = "N";
                userFields.Item("U_VS_NATNUM").Value = entity.Naturaleza;
                userFields.Item("U_BPP_MDTD").Value = entity.TipoDocumento;
                userFields.Item("U_BPP_MDSD").Value = entity.SerieDocumento;
                userFields.Item("U_BPP_MDCD").Value = entity.CorrelativoDocumento;
                document.NumAtCard= entity.NumberAtCard;

                Document_Lines documentLines = document.Lines;
                entity.DocumentLines.ForEach((line, index, lastIteration) =>
                {
                    documentLines.BaseType = line.BaseType;
                    documentLines.BaseEntry = line.BaseEntry;
                    documentLines.BaseLine = line.BaseLine;
                    documentLines.Quantity = line.Quantity.ToDouble();
                    documentLines.ItemCode = line.ItemCode;
                    documentLines.WarehouseCode = line.WarehouseCode;
                    documentLines.CostingCode2 = line.CentroCosto2;//TODO
                    BatchNumbers batchNumbers = documentLines.BatchNumbers;
                    line.SelectedBatches.ForEach((batch, batchIndex, batchLastIteration) =>
                    {
                        batchNumbers.BatchNumber = batch.BatchNumber;
                        batchNumbers.Quantity = batch.Quantity.ToDouble();
                        batchNumbers.ExpiryDate = batch.Expiry;
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
    }
}