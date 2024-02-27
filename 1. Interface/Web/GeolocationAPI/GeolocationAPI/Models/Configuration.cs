using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class Configuration
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("U_VS_PD_CTIP")]
        public string Tipo { get; set; }

        [JsonProperty("U_VS_PD_CVAL")]
        public string Valor { get; set; }

        
    }
}
