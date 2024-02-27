using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Local
{
    public class Item
    {
        public Item()
        {
            Batches = new List<BatchAssignation>();
        }

        public int DocEntryRoute { get; set; }
        public string RouteId { get; set; }
        public int DocEntryOT { get; set; }
        public int DocEntryOrder { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public int LineNumber { get; set; }
        public int BaseLine { get; set; }
        public string WhsId { get; set; }
        public DateTime ShipDate { get; set; }
        public List<BatchAssignation> Batches { get; set; }
        public string Pendiente { get; set; }
    }
}
