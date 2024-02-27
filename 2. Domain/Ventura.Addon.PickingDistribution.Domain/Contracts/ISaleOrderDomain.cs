using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface ISaleOrderDomain : IBaseDomain
    {
        IEnumerable<ORDR> Retrieve(DateTime deliveryDate);
        IEnumerable<ORDR> RetrieveFR(DateTime deliveryDate);

        IEnumerable<ORDR> ValidateStock(IEnumerable<ORDR> salesOrders);

        /// <summary>
        /// Retrieve a specific Sale Order (works with the DocNum)
        /// </summary>
        /// <param name="documentNumber">Sale Order's Document number</param>
        /// <returns></returns>
        ORDR RetrieveSaleOrder(int documentNumber);

        ORDR RetrieveSaleOrderByEntry(int documentEntry);

        void UpdateCustomFieldSaleOrder(ORDR saleOrder);

        void UpdateCustomFieldsClearTransferOrderreference(ORDR saleOrder);

        void UpdateCustomFieldsComments(ORDR saleOrder);
        
        void UpdateSystemFieldSaleOrder(ORDR saleOrder);

        IEnumerable<RDR1> RetrieveSaleOrderByRoute(int codeRoute);

        void UpdateCustomBatchesSaleOrder(ORDR saleOrder);
        void CloseDocument(int docentry);
    }
}
