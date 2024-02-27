using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class Setting
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("U_VS_PD_CTIP")]
        public string Type { get; set; }

        [JsonProperty("U_VS_PD_CVAL")]
        public string Value { get; set; }
    }
}