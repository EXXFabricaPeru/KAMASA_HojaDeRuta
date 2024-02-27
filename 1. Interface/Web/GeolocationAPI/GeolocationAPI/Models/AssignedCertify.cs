using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class AssignedCertify
    {
        [JsonProperty("Code")]
        public string Code { get; set; }
        
        [JsonProperty("U_VS_PD_CDET")]
        public string ReferenceEntityId { get; set; }

        [JsonProperty("U_VS_PD_TPET")]
        public string ReferenceEntityType { get; set; }

        [JsonProperty("U_VS_PD_NMET")]
        public string ReferenceEntityName { get; set; }

        [JsonProperty("VS_PD_ACD1Collection")]
        public List<ReferenceCertify> Certifies { get; set; }
    }

    public class ReferenceCertify
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("LineId")]
        public int LineId { get; set; }
        
        [JsonProperty("U_VS_PD_CDCT")]
        public string Id { get; set; }

        [JsonProperty("U_VS_PD_TPCT")]
        public string Type { get; set; }

        [JsonProperty("U_VS_PD_DSCT")]
        public string Description { get; set; }
    }
}