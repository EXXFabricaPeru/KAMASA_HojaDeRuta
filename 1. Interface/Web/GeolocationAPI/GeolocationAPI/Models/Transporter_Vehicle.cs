using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class Transporter_Vehicle
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class VSPDOTRD
        {
            [JsonProperty("Code")]
            public string Code { get; set; }
            [JsonProperty("Priority")]
            public string Priority { get; set; }
        }

        public class VSPDOTRDVSPDTRD2Collection
        {
            [JsonProperty("Code")]
            public string Code { get; set; }

            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("U_VS_PD_CDVH")]
            public string UVSPDCDVH { get; set; }

            [JsonProperty("WeightCapacity")]
            public string WeightCapacity { get; set; }

            [JsonProperty("VolumeCapacity")]
            public string VolumeCapacity { get; set; }

            [JsonProperty("HasRefrigeration")]
            public string HasRefrigeration { get; set; }

            [JsonProperty("IsAvailable")]
            public string IsAvailable { get; set; }
        }

        public class Value_
        {
            [JsonProperty("VS_PD_OTRD")]
            public VSPDOTRD VSPDOTRD { get; set; }

            [JsonProperty("VS_PD_OTRD/VS_PD_TRD2Collection")]
            public VSPDOTRDVSPDTRD2Collection VSPDOTRDVSPDTRD2Collection { get; set; }
        }


        public List<Value_> Value { get; set; }





    }
}
