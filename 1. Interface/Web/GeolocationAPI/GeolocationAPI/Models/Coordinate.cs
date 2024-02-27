using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class Coordinate
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Description { get; set; }
        
        [JsonProperty("U_VS_PD_CTIP")]
        public string Type { get; set; }

        [JsonProperty("U_VS_PD_CVAL")]
        public decimal Value { get; set; }
    }
}