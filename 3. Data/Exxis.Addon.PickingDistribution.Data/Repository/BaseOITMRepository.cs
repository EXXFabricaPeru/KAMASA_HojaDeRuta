using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    public abstract class BaseOITMRepository : Code.Repository<OITM>
    {
        protected BaseOITMRepository(Company company) : base(company)
        {
        }

        public abstract OITM GetItem(string id);

        public abstract OITM FindByEDICode(string ediCode);

        public abstract decimal AvaliableStock(string itemCode);

        public abstract decimal AvaliableStockPerExpirationDate(string itemCode, DateTime expiration,string whscode);

        public abstract decimal AvaliableStockPerExpirationDateWithoutReservation(string itemCode, DateTime expiration, string whscode);

        public abstract decimal AvaliableStockPerExpirationDateWithoutReservationWithoutDate(string itemCode, string whscode);

        public abstract decimal AvaliableStockPerExpirationDateWithoutReservationByBatch(string itemCode, DateTime expiration, string whscode, string batch);

        public abstract decimal AvaliableStockPerExpirationDateWithoutReservationByBatchWODate(string itemCode, string whscode, string batch);

    }
}