using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class TimeWindowDispatch
    {
        [JsonProperty("Identificador")]
        public string Identificador { get; set; }
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public object Name { get; set; }

        [JsonProperty("VS_PD_VHR1Collection")]
        //[JsonProperty("VHR1Collection")]
        public List<TimeWindowDispatchRelatedHour> RelatedHours { get; set; }
    }

    public class TimeWindowDispatchRelatedHour
    {
        [JsonProperty("LineId")]
        public int LineId { get; set; }

        [JsonProperty("U_VS_PD_HRIN")]
        //[JsonProperty("U_H_INI")]
        public int StarHour { get; set; }

        [JsonProperty("U_VS_PD_HRFN")]
        //[JsonProperty("U_H_FIN")]
        public int EndHour { get; set; }

        [JsonProperty]
        public bool IsUsed { get; set; }
    }
}