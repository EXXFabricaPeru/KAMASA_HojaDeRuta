using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface ITransferOrderDomain : IBaseDomain
    {
        IEnumerable<ORTR> BuildTransferOrder(DateTime emissionDate, string distributionFlow);

        Tuple<bool, string, string> RegisterOrder(ORTR documentEntry , bool isOTRU=false);

        Tuple<bool, string> UpdateOrder(ORTR documentEntry);
        IEnumerable<ORTR> RetrieveOpenedTransferOrders();

        IEnumerable<ORTR> RetrieveOpenedTransferOrdersPickup(DateTime deliveryDate);

        IEnumerable<ORTR> RetrieveOpenedTransferOrdersPickupPlannedByDate(DateTime deliveryDate);

        IEnumerable<ORTR> OpenTransferOrders();

        IEnumerable<ORTR> OpenTransferOrdersByDate(DateTime deliveryDate);

        IEnumerable<ORTR> RetrieveTransferOrdersToDistribute();

        IEnumerable<ORTR> RetrievePlanningTransferOrders();

        IEnumerable<Tuple<bool, string>> Open(params ORTR[] documentEntries);

        IEnumerable<Tuple<bool, string>> Close(params ORTR[] documentEntries);

        IEnumerable<Tuple<bool, string>> Postponed(params ORTR[] documentEntries);

        Tuple<bool, string> OpeningOrder(int documentEntry);

        Tuple<bool, string> PlaningOrder(int documentEntry);

        Tuple<bool, string> RemoveOrder(int documentEntry);

        ORTR RetrieveOrder(int documentEntry);

        IEnumerable<ORTR> RetrieveTransferOrdersByRoute(int codRoute);
        Tuple<bool, string> DeleteBatchRelated(ORTR document);

        Tuple<bool, string> DeleteBatchRelatedByItem(ORTR document,RTR3 documentLine);

        Tuple<bool, string> AddBatchRelated(ORTR document);

        Tuple<bool, string> TransferItems(List<CrossCutting.Model.Local.Item> document);
    }
}
