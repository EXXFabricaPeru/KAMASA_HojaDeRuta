using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Header
{
    public class ReconciliationSL
    {
        [JsonProperty("CardOrAccount")]
        public string CardOrAccount { get; set; }

        [JsonProperty("ReconDate")]
        public DateTime ReconDate { get; set; }

        [JsonProperty("InternalReconciliationOpenTransRows")]
        public List<ReconciliationLinesSL> InternalReconciliationOpenTransRows { get; set; }

    }
}
