using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class Velocity
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("U_VS_PD_HRIN")]
        public int StartHour { get; set; }

        [JsonProperty("U_VS_PD_HRFN")]
        public int EndHour { get; set; }

        [JsonProperty("U_VS_PD_VLPR")]
        public decimal AverageVelocity { get; set; }
    }
}