using System;
using System.Collections.Generic;
using System.Linq;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class ReturnOrderDomain : BaseDomain, IReturnOrderDomain
    {
        public ReturnOrderDomain(Company company)
            : base(company)
        {
        }

        public IEnumerable<ORRR> Retrieve(DateTime deliveryDate)
        {
            return UnitOfWork
                .ReturnRequestOrderRepository
                .RetrieveDocuments(doc => doc.DocumentDeliveryDate == deliveryDate && doc.DocumentStatus == "O" && doc.DocumentCancelled == "N");
        }

        public ORRR RetrieveByEntry(int entry)
        {
            return UnitOfWork
                .ReturnRequestOrderRepository
                .RetrieveDocuments(doc => doc.DocumentEntry == entry)
                .Single();
        }

        public IEnumerable<ORRR> ValidateDocuments(IEnumerable<ORRR> documents)
        {
            IList<ORRR> result = new List<ORRR>();
            //foreach (ORRR document in documents)
            //{
            //    if (document.DistributionValidationCode == SAPDocument.DistributionValidationStatus.NON_VALIDATE ||
            //        document.DistributionValidationCode == SAPDocument.DistributionValidationStatus.DISAPPROVED)
            //    {
            //        Tuple<bool, string> validation = validate_document_line(document);
            //        document.DistributionValidationCode = validation.Item1 ? SAPDocument.DistributionValidationStatus.APPROVED : SAPDocument.DistributionValidationStatus.DISAPPROVED;
            //        document.DistributionValidationMessage = validation.Item2;
            //    }

            //    result.Add(document);
            //}

            return result;
        }

        private Tuple<bool, string> validate_document_line(ORRR document)
        {
            foreach (RRR1 line in document.DocumentLines)
            {
                if (line.SelectedBatches == null || !line.SelectedBatches.Any())
                    return Tuple.Create(false, $"[Error validación] La linea '{line.LineNumber + 1}' del documento no posee lotes referenciados.");
            }
            
            return Tuple.Create(true, string.Empty);
        }

    }
}