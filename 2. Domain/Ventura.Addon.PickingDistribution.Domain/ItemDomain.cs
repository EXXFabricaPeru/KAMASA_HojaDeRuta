using System;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class ItemDomain : BaseDomain, IItemDomain
    {
        public ItemDomain(SAPbobsCOM.Company company) 
            : base(company)
        {
        }

        #region Implementation of IItemDomain

        public decimal RetrieveSaleWeight(string itemCode)
        {
            OITM item = UnitOfWork.ItemsRepository.GetItem(itemCode);
            return item.SaleWeight;
        }

        public decimal RetrieveInventoryWeight(string itemCode)
        {
            OITM item = UnitOfWork.ItemsRepository.GetItem(itemCode);
            return item.InventoryWeight;
        }

        public OITM Retrieve(string itemCode)
        {
            OITM item = UnitOfWork.ItemsRepository.GetItem(itemCode);
            return item;
        }

        public OITM RetrieveByEDICode(string ediCode)
        {
            try
            {
                return UnitOfWork.ItemsRepository.FindByEDICode(ediCode);
            }
            catch (Exception)
            {
                throw new Exception($"El código EDI '{ediCode}' no esta configurado en el Maestro de Artículo (OITM).");
            }
        }

        public decimal RetrieveAvaiblebyWHS(string itemCode, DateTime fecha, string warehouse)
        {

            return UnitOfWork.ItemsRepository.AvaliableStockPerExpirationDate(itemCode, fecha, warehouse);

        }

        public decimal RetrieveAvaiblebyWHS_Wo(string itemCode, DateTime fecha, string warehouse)
        {

            return UnitOfWork.ItemsRepository.AvaliableStockPerExpirationDateWithoutReservation(itemCode, fecha, warehouse);

        }

        public decimal RetrieveAvaiblebyWHS_Wo_byBatch(string itemCode, DateTime fecha, string warehouse,string batch)
        {

            return UnitOfWork.ItemsRepository.AvaliableStockPerExpirationDateWithoutReservationByBatch(itemCode, fecha, warehouse,batch);

        }

        public decimal RetrieveAvaiblebyWHS_Wo_byBatch_WO_Date(string itemCode, DateTime fecha, string warehouse, string batch)
        {

            return UnitOfWork.ItemsRepository.AvaliableStockPerExpirationDateWithoutReservationByBatchWODate(itemCode, warehouse, batch);

        }


        #endregion


    }
}