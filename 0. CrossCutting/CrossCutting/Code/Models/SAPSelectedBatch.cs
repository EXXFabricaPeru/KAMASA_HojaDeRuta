using System;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models
{
    [Serializable]
    public class SAPSelectedBatch
    {
        public string ItemCode { get; set; }
        public string BatchNumber { get; set; }
        public string WarehouseCode { get; set; }
        public decimal Quantity { get; set; }
        public DateTime Expiry { get; set; }
    }
}