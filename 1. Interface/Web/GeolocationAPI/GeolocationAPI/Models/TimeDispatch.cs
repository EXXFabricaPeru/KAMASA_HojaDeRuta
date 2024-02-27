using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class TimeDispatch
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("U_VS_PD_CLVT")]
        public string SaleChannel { get; set; }

        [JsonProperty("U_VS_PD_TPDS")]
        public int Duration { get; set; }
    }
}