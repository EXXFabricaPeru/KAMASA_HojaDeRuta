using System.Collections.Generic;

namespace Ventura.Addon.ComisionTarjetas.ApiBeetrack.Model
{
    public class Route
    {
        public int id { get; set; }
        public int route_id { get; set; }
        public string date { get; set; }
        public string dispatch_date { get; set; }
        public Truck truck { get; set; }
        public string truck_identifier { get; set; }
        public string driver_identifier { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string started_at { get; set; }
        public bool enable_estimations { get; set; }
        public IList<Dispatch> dispatches { get; set; }
    }
}
