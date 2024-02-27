using System;
using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using System.Linq;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class PurchaseOrderDomain : BaseDomain, IPurchaseOrderDomain
    {
        public PurchaseOrderDomain(Company company) : base(company)
        {
        }

        public void CloseDocument(int docentry)
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> RegisterPurchaseOrder(OPOR document)
        {
            return UnitOfWork.PurchaseOrderRepository.RegisterDocumentPurchaseOrder(document);
        }

        public Tuple<bool, string> RegisterRequestPurchaseOrder(OPRQ document)
        {
            return UnitOfWork.RequestPurchaseOrderRepository.RegisterDocumentRequestPurchaseOrder(document);
        }

        public Tuple<bool, string> SimpleRegisterRequestPurchaseOrder(OPRQ document)
        {
            return UnitOfWork.RequestPurchaseOrderRepository.SimpleRegisterDocumentRequestPurchaseOrder(document);
        }

        public OPRQ RetrieveRequestPurchaseOrder(int documentEntry)
        {
            return UnitOfWork.RequestPurchaseOrderRepository.RetrieveDocuments(t=>t.DocumentEntry== documentEntry).FirstOrDefault();
        }

        public IEnumerable<OPOR> Retrieve(DateTime deliveryDate)
        {
            throw new NotImplementedException();
        }

        public ORDR RetrievePurchaseOrder(int documentNumber)
        {
            throw new NotImplementedException();
        }

        public ORDR RetrievePurchaseOrderByEntry(int documentEntry)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RDR1> RetrievePurchaseOrderByRoute(int codeRoute)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomBatchesPurchaseOrder(OPOR purchaseOrder)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomFieldPurchaseOrder(OPOR purchaseOrder)
        {
            throw new NotImplementedException();
        }

        public void UpdateSystemFieldPurchaseOrder(OPOR purchaseOrder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OPOR> ValidateStock(IEnumerable<OPOR> PurchaseOrders)
        {
            throw new NotImplementedException();
        }
    }
}
