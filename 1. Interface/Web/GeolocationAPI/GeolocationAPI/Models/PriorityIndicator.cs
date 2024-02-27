using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class PriorityIndicator
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("U_VS_PD_VALR")]
        public decimal Value { get; set; }

        [JsonProperty("U_VS_PD_TIPO")]
        public string Type { get; set; }
    }
}