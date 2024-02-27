using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail
{
    public class JournalEntryLineSL
    {
        [JsonProperty("AccountCode")]
        public string AccountCode { get; set; }

        [JsonProperty("Line_ID")]
        public int Line { get; set; }

        [JsonProperty("Debit")]
        public double Debit { get; set; }

        [JsonProperty("Credit")]
        public double Credit { get; set; }

        [JsonProperty("FCDebit")]
        public double FcDebit { get; set; }

        [JsonProperty("FCCredit")]
        public double FcCredit { get; set; }

        [JsonProperty("FCCurrency")]
        public string FcCurrency { get; set; }

        [JsonProperty("DueDate")]
        public DateTime DueDate { get; set; }

        [JsonProperty("ShortName")]
        public string ShortName { get; set; }

        [JsonProperty("LineMemo")]
        public string LineMemo { get; set; }

        [JsonProperty("Reference1")]
        public string Reference1 { get; set; }

        [JsonProperty("Reference2")]
        public string Reference2 { get; set; }

        [JsonProperty("AdditionalReference")]
        public string AdditionalReference { get; set; }

        [JsonProperty("ProjectCode")]
        public string ProjectCode { get; set; }

        [JsonProperty("CostingCode")]
        public string CostingCode { get; set; }

        [JsonProperty("U_INFOPE02")]
        public string UInfope02 { get; set; }

        [JsonProperty("CashFlowAssignments")]
        public List<CashFlowAssignment> CashFlowAssignments { get; set; }
    }

    public class CashFlowAssignment
    {
        //    [JsonProperty("CashFlowAssignmentsID")]
        //    public int CashFlowAssignmentsId { get; set; }

        [JsonProperty("CashFlowLineItemID")]
        public int CashFlowLineItemId { get; set; }

        [JsonProperty("Credit")]
        public double Credit { get; set; }

        //[JsonProperty("PaymentMeans")]
        //public object PaymentMeans { get; set; }

        //[JsonProperty("CheckNumber")]
        //public object CheckNumber { get; set; }

        [JsonProperty("AmountLC")]
        public double AmountLc { get; set; }

        [JsonProperty("AmountFC")]
        public double AmountFc { get; set; }

        //[JsonProperty("JDTLineId")]
        //public long JdtLineId { get; set; }
    }
}
