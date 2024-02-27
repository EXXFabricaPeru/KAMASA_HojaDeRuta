using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;


namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IPurchaseOrderDomain :IBaseDomain
    {
        IEnumerable<OPOR> Retrieve(DateTime deliveryDate);

        IEnumerable<OPOR> ValidateStock(IEnumerable<OPOR> PurchaseOrders);

        /// <summary>
        /// Retrieve a specific Purchase Order (works with the DocNum)
        /// </summary>
        /// <param name="documentNumber">Purchase Order's Document number</param>
        /// <returns></returns>
        ORDR RetrievePurchaseOrder(int documentNumber);

        ORDR RetrievePurchaseOrderByEntry(int documentEntry);

        void UpdateCustomFieldPurchaseOrder(OPOR purchaseOrder);

        void UpdateSystemFieldPurchaseOrder(OPOR purchaseOrder);

        IEnumerable<RDR1> RetrievePurchaseOrderByRoute(int codeRoute);

        void UpdateCustomBatchesPurchaseOrder(OPOR purchaseOrder);
        void CloseDocument(int docentry);

        Tuple<bool, string> RegisterPurchaseOrder(OPOR document);

        Tuple<bool, string> RegisterRequestPurchaseOrder(OPRQ document);

        Tuple<bool, string> SimpleRegisterRequestPurchaseOrder(OPRQ document);

        OPRQ RetrieveRequestPurchaseOrder(int docentry);
    }
}
