using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class Vehicle
    {
        [JsonProperty("Code")] public string Code { get; set; }

        [JsonProperty("Name")] public string Name { get; set; }

        [JsonProperty("U_VS_FROZEN")] public string IsAvailable { get; set; }

        [JsonProperty("U_VS_PD_CPDC")] public decimal? WeightCapacity { get; set; }
        
        [JsonProperty("U_VS_PD_VLDC")] public decimal? VolumeCapacity { get; set; }

        [JsonProperty("U_VS_PD_TPDS")] public string DistributionType { get; set; }

        [JsonProperty("U_VS_PD_SRVH")] public string HasRefrigeration { get; set; }

        [JsonProperty("U_BPP_VEPL")] public string PlateID { get; set; }
        public int? Priority { get; set; }
    }
}