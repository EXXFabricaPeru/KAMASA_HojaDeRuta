using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class DirectionsMapBox
    {
        [JsonProperty("routes")]
        public List<RouteMapBox> Routes { get; set; }
    }

    public class RouteMapBox
    {
        [JsonProperty("weight_name")]
        public string WeightName { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("distance")]
        public decimal Distance { get; set; }
        
        [JsonProperty("geometry")]
        public string Geometry { get; set; }
    }
}