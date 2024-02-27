using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Header
{
    public class JournalEntrySL
    {
        [JsonProperty("JdtNum")]
        public string JdtNum { get; set; }

        [JsonProperty("ReferenceDate")]
        public DateTime ReferenceDate { get; set; }

        [JsonProperty("DueDate")]
        public DateTime DueDate { get; set; }

        [JsonProperty("TaxDate")]
        public DateTime TaxDate { get; set; }

        [JsonProperty("Memo")]
        public string Memo { get; set; }

        [JsonProperty("ProjectCode")]
        public string ProjectCode { get; set; }

        [JsonProperty("TransactionCode")]
        public string TransactionCode { get; set; }

        [JsonProperty("Reference")]
        public string Reference { get; set; }

        [JsonProperty("Reference2")]
        public string Reference2 { get; set; }

        [JsonProperty("Reference3")]
        public string Reference3 { get; set; }

        [JsonProperty("JournalEntryLines")]
        public List<JournalEntryLineSL> JournalEntryLines { get; set; }
    }
}
