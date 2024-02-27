using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail
{
    public class ReconciliationLinesSL
    {
        [JsonProperty("Selected")]
        public string Selected { get; set; }

        [JsonProperty("ShortName")]
        public string ShortName { get; set; }

        [JsonProperty("TransId")]
        public int TransId { get; set; }

        [JsonProperty("TransRowId")]
        public int TransRowId { get; set; }

        [JsonProperty("SrcObjTyp")]
        public string SrcObjTyp { get; set; }

        [JsonProperty("SrcObjAbs")]
        public int SrcObjAbs { get; set; }

        [JsonProperty("CreditOrDebit")]
        public string CreditOrDebit { get; set; }

        [JsonProperty("ReconcileAmount")]
        public double ReconcileAmount { get; set; }
    }
}
