using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class BusinessPartner
    {
        [JsonProperty("CardCode")] public string CardCode { get; set; }

        [JsonProperty("CardName")] public string CardName { get; set; }

        [JsonProperty("BPAddresses")] public List<BusinessPartnerAddress> BusinessPartnerAddresses { get; set; }
    }

    public class BusinessPartnerAddress
    {
        [JsonProperty("RowNum")] public int RowNum { get; set; }

        [JsonProperty("AddressName")] public string AddressName { get; set; }

        [JsonProperty("AddressType")] public string AddressType { get; set; }

        [JsonProperty("Street")] public string Street { get; set; }

        [JsonProperty("U_VS_PD_GEOLOCX")] public decimal? Latitude { get; set; }

        [JsonProperty("U_VS_PD_GEOLOCY")] public decimal? Longitude { get; set; }
    }
}