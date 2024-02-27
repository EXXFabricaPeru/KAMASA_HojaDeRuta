using System;
using System.Collections.Generic;
using System.Linq;

namespace GeolocationAPI.Models
{
    public class Route
    {
        public Route()
        {
            DeliveryPointsRelated = new List<RouteDetail>();
        }

        public string Id { get; set; }
        public DateTime Create { get; set; }
        public List<RouteDetail> DeliveryPointsRelated { get; set; }
        public bool HasRefrigeration => DeliveryPointsRelated.Sum(t => t.FreezeWeight) > 0;
        public decimal DeliveryControlMaxWeight { get; set; }
        public decimal DeliveryControlMaxVolume { get; set; }
        public decimal TotalWeight => DeliveryPointsRelated.Sum(t => t.Weight);
        public decimal TotalVolume => DeliveryPointsRelated.Sum(t => t.Volume);
        public decimal TotalFreezeWeight => DeliveryPointsRelated.Sum(t => t.FreezeWeight);
        public decimal TotalFreezeVolume => DeliveryPointsRelated.Sum(t => t.FreezeVolume);
        public string LeaveAddressId { get; set; }
        public string TransporterId { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public string AuxiliaryId { get; set; }
        public int ReferenceCounter { get; set; }
    }

    public class RouteDetail
    {
        public int TransferOrderId { get; set; }
        public int StartHour { get; set; }
        public int TravelTime { get; set; }
        public int ArriveHour { get; set; }
        public int LeaveHour { get; set; }
        public string TimeWindow { get; set; }
        public string DeliveryControl { get; set; }
        public decimal Distance { get; set; }
        public decimal Weight { get; set; }
        public decimal Volume { get; set; }
        public decimal FreezeWeight { get; set; }
        public decimal FreezeVolume { get; set; }
    }
}