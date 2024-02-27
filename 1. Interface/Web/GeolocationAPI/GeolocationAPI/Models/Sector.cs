using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class Sector
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Description { get; set; }
        
        [JsonProperty("VS_PD_PDG1Collection")]
        public List<CoordinateSector> Coordinates { get; set; }
    }

    public class CoordinateSector
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("LineId")]
        public int Line { get; set; }
        
        [JsonProperty("U_VS_PD_LATI")]
        public double Latitude { get; set; }

        [JsonProperty("U_VS_PD_LONG")]
        public double Longitude { get; set; }
    }
}