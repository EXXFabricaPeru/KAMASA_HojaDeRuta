using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class ServiceLayerError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public ServiceLayerErrorMessage Message { get; set; }
    }

    public class ServiceLayerErrorMessage
    {
        [JsonProperty("lang")]
        public string Language { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}