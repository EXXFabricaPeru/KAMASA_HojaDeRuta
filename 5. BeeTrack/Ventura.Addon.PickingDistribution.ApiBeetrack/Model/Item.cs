using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventura.Addon.ComisionTarjetas.ApiBeetrack.Model
{
    public class Item
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public int original_quantity { get; set; }
        public int delivered_quantity { get; set; }
        public string price { get; set; }
        public string unit_price { get; set; }
        public IList<Extra> extras { get; set; }
    }
}
