using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class MarketingDocumentDomain : BaseDomain, IMarketingDocumentDomain
    {
        public MarketingDocumentDomain(Company company) : base(company)
        {
        }

        public Tuple<bool, string> UpdateMarketingDocumentCustomFields<T>(T document) 
            where T : SAPDocument
        {
            var saleOrder = document as ORDR;
            if (saleOrder != null)
            {
                return UpdateORTRReference(saleOrder);
            }

            var inventoryTransfer = document as OWTQ;
            if (inventoryTransfer != null)
            {
                return UpdateORTRReference(inventoryTransfer);
            }

            var returnOrder = document as ORRR;
            if (returnOrder != null)
            {
                return UpdateReturnRequestCustomFields(returnOrder);
            }

            var purchaseOrder = document as OPOR;
            if (purchaseOrder != null)
            {
                return UpdateORTRReference(purchaseOrder);
            }

            var invoice = document as OINV;
            if (invoice != null)
            {
                return UpdateORTRReference(invoice);
            }

            return Tuple.Create(false, @"[Internal Error] No se puede procesar este tipo de documento");
        }

        public Tuple<bool, string> UpdateORTRReference(ORDR document)
        {
            return UnitOfWork.SaleOrderRepository.UpdateCustomFieldsFromDocument(document);
        }

        public Tuple<bool, string> UpdateORTRReferenceFR(ORDR document, List<RDR1> lines)
        {
            return UnitOfWork.SaleOrderRepository.UpdateCustomFieldsFromDocumentFR(document,lines);
        }

        public Tuple<bool, string> UpdateORTRReference(OWTQ document)
        {
            return UnitOfWork.InventoryTransferRequestRepository.UpdateCustomFieldsFromDocument(document);
        }

        public Tuple<bool, string> UpdateReturnRequestCustomFields(ORRR document)
        {
            return UnitOfWork.ReturnRequestOrderRepository.UpdateCustomFieldsFromDocument(document);
        }

        public Tuple<bool, string> UpdateORTRReference(OPOR document)
        {
            return UnitOfWork.PurchaseOrderRepository.UpdateCustomFieldsFromDocument(document);
        }

        public Tuple<bool, string> UpdateORTRReference(OINV document)
        {
            return UnitOfWork.InvoiceRepository.UpdateCustomFieldsFromDocument(document);
        }



        public Tuple<bool, string, string> RegisterDocumentReturn(ODLN document, DocumentObjectTypeEnum obj, string FLowSource)
        {
            return UnitOfWork.DeliveryRepository.RegisterDocumentReturn(document, obj, FLowSource);
        }

        public Tuple<bool, string, string> RegisterDocumentCreditNote(ODLN document, DocumentObjectTypeEnum obj, string FLowSource)
        {
            return UnitOfWork.DeliveryRepository.RegisterDocumentCreditNote(document, obj, FLowSource);
        }

        public Tuple<bool, string, string> RegisterInvoice(ODLN document, int deliveryEntry,bool seriefija=false)
        {
            return UnitOfWork.DeliveryRepository.RegisterDocumentInvoice(document, deliveryEntry,seriefija);
        }

        public IEnumerable<ORRR> RetrieveReturnRequest(Expression<Func<ORRR, bool>> expression)
        {
            return UnitOfWork.ReturnRequestOrderRepository.RetrieveDocuments(expression);
        }

        public IEnumerable<ORIN> RetrieveCreditNote(Expression<Func<ORIN, bool>> expression)
        {
            return UnitOfWork.CreditNoteRepository.RetrieveDocuments(expression);
        }

        public Tuple<bool, string> CancelDocumentDelivery(int entry)
        {
            try
            {
                UnitOfWork.DeliveryRepository.CancelDocument(entry);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, exception.Message);
            }
        }
        Tuple<bool, string> IMarketingDocumentDomain.CancelDocumentInvoice(int entry)
        {
            try
            {
                UnitOfWork.InvoiceRepository.CancelDocument(entry);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, exception.Message);
            }
        }
        //Tuple<bool, string> CancelDocumentInvoice(int entry)
        //{
        //    try
        //    {
        //        UnitOfWork.InvoiceRepository.CancelDocument(entry);
        //        return Tuple.Create(true, string.Empty);
        //    }
        //    catch (Exception exception)
        //    {
        //        return Tuple.Create(false, exception.Message);
        //    }
        //}

       
        public IEnumerable<ODLN> GetDocumentDelivery(string item2)
        {
            var cod = int.Parse(item2);
            return UnitOfWork.DeliveryRepository.RetrieveDocuments(t => t.DocumentEntry == cod);
        }

        public ODPI RetrieveDownPaymentInvoiceByCode(int entry)
        {
            return UnitOfWork.DownPaymentInvoiceRepository.RetrieveDocuments(t => t.DocumentEntry == entry).Single();
        }

        public OINV RetrieveInvoiceByCode(int entry)
        {
            return UnitOfWork.InvoiceRepository.RetrieveDocuments(t => t.DocumentEntry == entry).Single();
        }

        public IEnumerable<ORDR> GetDocumentSalesOrder(string item2)
        {
            var cod = int.Parse(item2);
            return UnitOfWork.SaleOrderRepository.RetrieveDocuments(t => t.DocumentEntry == cod);
        }

        public object GetDocument(string item2)
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> RegisterByPicking(List<CrossCutting.Model.Local.Item> items,DateTime fecha)
        {
            return UnitOfWork.InventoryTransferRequestRepository.RegisterByPicking(items,fecha);
        }

        public IEnumerable<OWTQ> RetrieveInventoryTransferRequests(Expression<Func<OWTQ, bool>> expression = null)
        {
            return UnitOfWork.InventoryTransferRequestRepository.RetrieveDocuments(expression);
        }

        public IEnumerable<OWTR> RetrieveInventoryTransfer(Expression<Func<OWTR, bool>> expression = null)
        {
            return UnitOfWork.TransferItemRepository.Retrieve(expression);
        }

        public IEnumerable<ODLN> RetrieveDelivery(Expression<Func<ODLN, bool>> expression)
        {
            return UnitOfWork.DeliveryRepository.RetrieveDocuments(expression);
        }

        public IEnumerable<OQUT> RetrieveQuotation(Expression<Func<OQUT, bool>> expression)
        {
            return UnitOfWork.SaleQuotationRepository.RetrieveDocuments(expression);
        }

        public Tuple<bool, string> RegisterIventoryTransfer(OWTR document)
        {
            return UnitOfWork.TransferItemRepository.Register(document);
        }


        public Tuple<bool, string> RetrieveAnexoInvoice(int referenceDocumentEntryInvoice)
        {
            return UnitOfWork.InvoiceRepository.RetrieveAnexoDocument(referenceDocumentEntryInvoice);
        }
        public Tuple<bool, string> RetrieveAnexoDelivery(int referenceDocumentEntryInvoice)
        {
            return UnitOfWork.InvoiceRepository.RetrieveAnexoDocumentDelivery(referenceDocumentEntryInvoice);
        }

        

        public IEnumerable<ORDN> RetrieveDocumentsDraft(Expression<Func<ORDN, bool>> expression = null)
        {
            return UnitOfWork.ReturnOrderRepository.RetrieveDocumentsDraft(expression);
        }

    }
}