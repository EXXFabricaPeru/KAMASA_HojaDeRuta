using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class MapRoute
    {
        [JsonProperty("DocNum")] public int DocNum { get; set; }

        [JsonProperty("DocEntry")] public int DocEntry { get; set; }

        [JsonProperty("U_VS_PD_COCL")] public string ClientId { get; set; }

        [JsonProperty("U_VS_PD_DSCL")] public string ClientName { get; set; }

        [JsonProperty("U_VS_PD_COEN")] public string AddressId { get; set; }

        [JsonProperty("U_VS_PD_DREN")] public string AddressContent { get; set; }

        [JsonProperty("U_VS_PD_DRLT")] public decimal AddressLatitude { get; set; }

        [JsonProperty("U_VS_PD_DRLG")] public decimal AddressLongitude { get; set; }

        [JsonIgnore] public string OriginFlow { get; set; }

        [JsonProperty("VS_PD_MPR1Collection")] public List<MapRouteAddressDistanceRelated> AddressDistanceRelated { get; set; }
    }

    public class MapRouteAddressDistanceRelated
    {
        [JsonProperty("DocEntry")] public int DocEntry { get; set; }

        [JsonProperty("LineId")] public int? LineId { get; set; }

        [JsonProperty("U_VS_PD_COCL")] public string ClientId { get; set; }

        [JsonProperty("U_VS_PD_DSCL")] public string ClientName { get; set; }

        [JsonProperty("U_VS_PD_COEN")] public string AddressId { get; set; }

        [JsonProperty("U_VS_PD_DREN")] public string AddressContent { get; set; }

        [JsonProperty("U_VS_PD_DSDE")] public decimal? AddressDistance { get; set; }

        [JsonProperty("U_VS_PD_DRLT")] public decimal AddressLatitude { get; set; }

        [JsonProperty("U_VS_PD_DRLG")] public decimal AddressLongitude { get; set; }
    }
}