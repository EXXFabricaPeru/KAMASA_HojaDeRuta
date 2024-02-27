using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class Warehouse
    {
        [JsonProperty("WarehouseCode")] public string Code { get; set; }

        [JsonProperty("WarehouseName")] public string Name { get; set; }

        [JsonProperty("U_VS_PD_LOCX")] public decimal Latitude { get; set; }

        [JsonProperty("U_VS_PD_LOCY")] public decimal Longitude { get; set; }
    }
}