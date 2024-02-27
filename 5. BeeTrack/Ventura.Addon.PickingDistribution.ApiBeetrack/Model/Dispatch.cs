using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventura.Addon.ComisionTarjetas.ApiBeetrack.Model
{
    public class Dispatch
    {
        public string identifier { get; set; }
        public string contact_name { get; set; }
        public string contact_address { get; set; }
        public string contact_phone { get; set; }
        public string contact_id { get; set; }
        public string contact_identifier { get; set; }
        public string contact_email { get; set; }
        public bool is_trunk { get; set; }
        public bool is_pickup { get; set; }
        public object pickup_address { get; set; }
        public string arrived_at { get; set; }
        //string modificador porsiacaso DateTime
        public DateTime estimated_at { get; set; }
        //string modificador porsiacaso
        public string min_delivery_time { get; set; }
        //string modificador porsiacaso DateTime
        public string max_delivery_time { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string beecode { get; set; }
        public int slot { get; set; }
        public int mode { get; set; }
        public bool pincode_enabled { get; set; }

        //public string status { get; set; }      // DESCRIPCIO DE GUIA
        //public int status_id { get; set; }      // ID GUIA
        //public int status_id { get; set; }
        //public string substatus { get; set; }
        //public string substatus_code { get; set; }
        public IList<Tag> tags { get; set; }
        public IList<Item> items { get; set; }
    }
}
