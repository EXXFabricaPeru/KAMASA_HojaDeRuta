using System;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Local
{
    public class BatchAssignation
    {
        public string BatchId { get; set; }
        public string WareHouseId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime Expiry { get; set; }
        public DateTime Fabricate { get; set; }
        public int DocEntryRoute { get; set; }
        public int DocEntryOT { get; set; }
        public int DocEntryOrder { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Motive { get; set; }
        public decimal QuantityPrev { get; set; }
        public decimal Available { get; set; }
    }

}