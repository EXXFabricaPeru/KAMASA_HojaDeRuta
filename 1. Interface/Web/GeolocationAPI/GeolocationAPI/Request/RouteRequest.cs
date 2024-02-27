using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Request
{
    public class RouteRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("create")]
        public DateTime Create { get; set; }

        [JsonProperty("hasRefrigeration")]
        public bool HasRefrigeration { get; set; }

        [JsonProperty("deliveryControlMaxWeight")]
        public decimal DeliveryControlMaxWeight { get; set; }

        [JsonProperty("deliveryControlMaxVolume")]
        public decimal DeliveryControlMaxVolume { get; set; }

        [JsonProperty("totalWeight")]
        public decimal TotalWeight { get; set; }

        [JsonProperty("totalVolume")]
        public decimal TotalVolume { get; set; }

        [JsonProperty("totalFreezeWeight")]
        public decimal TotalFreezeWeight { get; set; }

        [JsonProperty("totalFreezeVolume")]
        public decimal TotalFreezeVolume { get; set; }

        [JsonProperty("transporterId")]
        public string TransporterId { get; set; }

        [JsonProperty("driverId")]
        public string DriverId { get; set; }

        [JsonProperty("vehicleId")]
        public string VehicleId { get; set; }

        [JsonProperty("auxiliaryId")]
        public string AuxiliaryId { get; set; }

        [JsonProperty("leaveAddressId")]
        public string LeaveAddressId { get; set; }

        [JsonProperty("latitude")]
        public decimal? Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal? Longitude { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("deliveryPointsRelated")]
        public List<DeliveryPointsRelated> DeliveryPointsRelated { get; set; }
    }

    public class DeliveryPointsRelated
    {
        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("transferOrderId")]
        public int TransferOrderId { get; set; }

        [JsonProperty("startHour")]
        public int StartHour { get; set; }

        [JsonProperty("travelTime")]
        public int TravelTime { get; set; }

        [JsonProperty("arriveHour")]
        public int ArriveHour { get; set; }

        [JsonProperty("leaveHour")]
        public int LeaveHour { get; set; }

        [JsonProperty("timeWindow")]
        public string TimeWindow { get; set; }

        [JsonProperty("deliveryControl")]
        public string DeliveryControl { get; set; }

        [JsonProperty("distance")]
        public decimal Distance { get; set; }

        [JsonProperty("weight")]
        public decimal Weight { get; set; }

        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        [JsonProperty("freezeWeight")]
        public decimal FreezeWeight { get; set; }

        [JsonProperty("freezeVolume")]
        public decimal FreezeVolume { get; set; }

        [JsonProperty("latitude")]
        public decimal? Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal? Longitude { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}