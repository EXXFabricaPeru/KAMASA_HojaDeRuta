using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class DeliveryControl
    {
        [JsonProperty("Code")] public string Code { get; set; }

        [JsonProperty("Name")] public object Name { get; set; }

        [JsonProperty("U_VS_PD_CPDC")] public decimal WeightLoad { get; set; }

        [JsonProperty("U_VS_PD_VLDC")] public decimal VolumeLoad { get; set; }

        [JsonProperty("U_VS_PD_TPDS")] public object DistributionType { get; set; }

        [JsonProperty("VS_PD_REN1Collection")]
        public List<DeliveryControlVehicleRelated> DeliveryControlVehicleRelated { get; set; }
    }

    public class DeliveryControlVehicleRelated
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("LineId")]
        public int LineId { get; set; }

        [JsonProperty("U_VS_PD_VHRF")]
        public string PlateId { get; set; }
    }
}