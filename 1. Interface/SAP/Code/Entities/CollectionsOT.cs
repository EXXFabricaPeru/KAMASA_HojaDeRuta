using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Code.Entities
{



    public class CollectionsOT
    {
        public string id { get; set; }
        public DateTime create { get; set; }
        public List<Deliverypointsrelated> deliveryPointsRelated { get; set; }
        public bool hasRefrigeration { get; set; }
        public int deliveryControlMaxWeight { get; set; }
        public int deliveryControlMaxVolume { get; set; }
        public float totalWeight { get; set; }
        public int totalVolume { get; set; }
        public int totalFreezeWeight { get; set; }
        public int totalFreezeVolume { get; set; }
    }

    public class Deliverypointsrelated
    {
        public int transferOrderId { get; set; }
        public int startHour { get; set; }
        public int travelTime { get; set; }
        public int arriveHour { get; set; }
        public int leaveHour { get; set; }
        public string timeWindow { get; set; }
        public string deliveryControl { get; set; }
        public int distance { get; set; }
        public float weight { get; set; }
        public int volume { get; set; }
        public int freezeWeight { get; set; }
        public int freezeVolume { get; set; }
    }


}
