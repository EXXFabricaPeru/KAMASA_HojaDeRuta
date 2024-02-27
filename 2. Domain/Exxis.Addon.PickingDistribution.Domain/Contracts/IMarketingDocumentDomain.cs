using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IMarketingDocumentDomain : IBaseDomain
    {
        Tuple<bool, string> UpdateMarketingDocumentCustomFields<T>(T document) where T : SAPDocument;

        Tuple<bool, string> UpdateORTRReference(ORDR document);

        Tuple<bool, string> UpdateORTRReferenceFR(ORDR document, List<RDR1> lines);

        Tuple<bool, string> UpdateORTRReference(OWTQ document);

        Tuple<bool, string> UpdateReturnRequestCustomFields(ORRR document);

        Tuple<bool, string, string> RegisterDelivery(ODLN document, DocumentObjectTypeEnum obj , string FlowSource, ORDR order, bool isFR, OARD codRutaEntry, ORTR ordenTraslado, bool serieFija = false,bool isReprocesar =false);

        Tuple<bool, string, string> RegisterDocumentReturn(ODLN document, DocumentObjectTypeEnum obj, string FlowSource);

        Tuple<bool, string, string> RegisterDocumentCreditNote(ODLN document, DocumentObjectTypeEnum obj, string FlowSource);

        Tuple<bool, string, string> RegisterInvoice(ODLN document, int deliveryEntry,bool utilizarSerieFija=false);

        Tuple<bool, string> RegisterIventoryTransfer(OWTR document);

        object GetDocument(string item2);

        IEnumerable<ORRR> RetrieveReturnRequest(Expression<Func<ORRR, bool>> expression);

        IEnumerable<ORIN> RetrieveCreditNote(Expression<Func<ORIN, bool>> expression);

        IEnumerable<ODLN> RetrieveDelivery(Expression<Func<ODLN, bool>> expression);

        IEnumerable<OQUT> RetrieveQuotation(Expression<Func<OQUT, bool>> expression);

        Tuple<bool, string> CancelDocumentDelivery(int entry);

        Tuple<bool, string> CancelDocumentInvoice(int entry);

        IEnumerable<ODLN> GetDocumentDelivery(string item2);

        OINV RetrieveInvoiceByCode(int entry);

        ODPI RetrieveDownPaymentInvoiceByCode(int entry);

        IEnumerable<ORDR> GetDocumentSalesOrder(string item2);
        
        Tuple<bool, string> RegisterByPicking(List<CrossCutting.Model.Local.Item> items,DateTime fecha);

        IEnumerable<OWTQ> RetrieveInventoryTransferRequests(Expression<Func<OWTQ, bool>> expression = null);

        IEnumerable<OWTR> RetrieveInventoryTransfer(Expression<Func<OWTR, bool>> expression = null);

        Tuple<bool, string> RetrieveAnexoInvoice(int referenceDocumentEntryInvoice);
        Tuple<bool, string> RetrieveAnexoDelivery(int referenceDocumentEntry);

        IEnumerable<ORDN> RetrieveDocumentsDraft(Expression<Func<ORDN, bool>> expression = null);
    }
}
