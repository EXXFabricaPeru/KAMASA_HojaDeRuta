using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class TransferOrder
    {
        [JsonProperty("DocNum")] public int DocNum { get; set; }

        [JsonProperty("Series")] public int Series { get; set; }

        [JsonProperty("DocEntry")] public int DocEntry { get; set; }

        [JsonProperty("U_VS_PD_COCL")] public string ClientId { get; set; }

        [JsonProperty("U_VS_PD_DSCL")] public string ClientName { get; set; }

        [JsonProperty("U_VS_PD_COEN")] public string DeliveryAddressId { get; set; }

        [JsonProperty("U_VS_PD_DREN")] public string DeliveryAddress { get; set; }

        [JsonProperty("U_VS_PD_COSA")] public string LeaveAddressId { get; set; }

        [JsonProperty("U_VS_PD_DRSA")] public string LeaveAddress { get; set; }

        [JsonProperty("U_VS_PD_FJOR")] public string OriginFlow { get; set; }

        [JsonProperty("U_VS_PD_STAD")] public string Status { get; set; }

        [JsonProperty("U_VS_PD_STMT")] public string StatusMotiveId { get; set; }

        [JsonProperty("U_VS_PD_STDS")] public string StatusMotiveDescription { get; set; }

        [JsonProperty("U_VS_PD_FEDS")] public DateTime DistributionDate { get; set; }

        [JsonProperty("U_VS_PD_HRDS")] public int? DistributionHour { get; set; }

        [JsonProperty("U_VS_PD_CNDC")] public int DocumentQuantity { get; set; }

        [JsonProperty("U_VS_PD_CNAR")] public double ArticleQuantity { get; set; }

        [JsonProperty("U_VS_PD_PTOT")] public decimal TotalWeight { get; set; }

        [JsonProperty("U_VS_PD_VTOT")] public decimal TotalVolume { get; set; }

        [JsonProperty("U_VS_PD_MTOT")] public double TotalAmount { get; set; }

        [JsonProperty("U_VS_PD_PRDS")] public string DispatchPriority { get; set; }

        [JsonProperty("U_VS_PD_TPCR")] public string LoadType { get; set; }

        [JsonProperty("U_VS_PD_CLVT")] public string SaleChannel { get; set; }

        [JsonProperty("U_VS_PD_OBJT")] public int SAPObject { get; set; }

        [JsonProperty("U_VS_PD_CVRF")] public int ReferenceQuotationId { get; set; }

        [JsonProperty("U_VS_PD_ODCV")] public int ReferenceQuotationDistributionOrder { get; set; }

        [JsonProperty("U_VS_PD_RSEN")] public string DeliveryControl { get; set; }

        [JsonProperty("U_VS_PD_TMDS")] public int? DispatchTime { get; set; }

        [JsonProperty("U_VS_PD_VTHR")] public string TimeWindowDispatch { get; set; }

        [JsonProperty("U_VS_PD_PRTO")] public decimal? TotalFreezeWeight { get; set; }

        [JsonProperty("U_VS_PD_VRTO")] public decimal? TotalFreezeVolume { get; set; }

        [JsonIgnore] public bool IsAssigned { get; set; }
        [JsonIgnore] public int? DocEntryReturn { get; set; }

        [JsonProperty("VS_PD_RTR1Collection")] public List<TransferOrderRelatedDocument> RelatedSAPDocuments { get; set; }

        [JsonProperty("VS_PD_RTR2Collection")] public List<TransferOrderRelatedDocumentLines> RelatedSAPDocumentLines { get; set; }

        [JsonProperty("VS_PD_RTR3Collection")] public List<TransferOrderRelatedBatchLines> RelatedBatchLines { get; set; }
    }

    public class TransferOrderRelatedDocument
    {
        [JsonProperty("DocEntry")] public int DocEntry { get; set; }

        [JsonProperty("LineId")] public int LineId { get; set; }

        [JsonProperty("U_VS_PD_DCEN")] public int UVSPDDCEN { get; set; }

        [JsonProperty("U_VS_PD_DCNM")] public int UVSPDDCNM { get; set; }

        [JsonProperty("U_VS_PD_DCCC")] public string UVSPDDCCC { get; set; }

        [JsonProperty("U_VS_PD_DCFE")] public string UVSPDDCFE { get; set; }

        [JsonProperty("U_VS_PD_DCST")] public string UVSPDDCST { get; set; }

        [JsonProperty("U_VS_PD_PRDS")] public string UVSPDPRDS { get; set; }

        [JsonProperty("U_VS_PD_STAD")] public string UVSPDSTAD { get; set; }

        [JsonProperty("U_VS_PD_STMT")] public string UVSPDSTMT { get; set; }

        [JsonProperty("U_VS_PD_STDS")] public string UVSPDSTDS { get; set; }

        [JsonProperty("U_VS_PD_CNAR")] public double UVSPDCNAR { get; set; }

        [JsonProperty("U_VS_PD_PTOT")] public double UVSPDPTOT { get; set; }

        [JsonProperty("U_VS_PD_VTOT")] public double UVSPDVTOT { get; set; }

        [JsonProperty("U_VS_PD_MTOT")] public double UVSPDMTOT { get; set; }

        [JsonProperty("U_VS_PD_OBJT")] public int UVSPDOBJT { get; set; }
    }

    public class TransferOrderRelatedDocumentLines
    {
        [JsonProperty("DocEntry")] public int DocEntry { get; set; }

        [JsonProperty("LineId")] public int LineId { get; set; }

        [JsonProperty("VisOrder")] public int VisOrder { get; set; }

        [JsonProperty("Object")] public string Object { get; set; }

        [JsonProperty("LogInst")] public object LogInst { get; set; }

        [JsonProperty("U_VS_PD_DCEN")] public int UVSPDDCEN { get; set; }

        [JsonProperty("U_VS_PD_DCNM")] public int UVSPDDCNM { get; set; }

        [JsonProperty("U_VS_PD_DCLN")] public int UVSPDDCLN { get; set; }

        [JsonProperty("U_VS_PD_CDAR")] public string UVSPDCDAR { get; set; }

        [JsonProperty("U_VS_PD_DSAR")] public string UVSPDDSAR { get; set; }

        [JsonProperty("U_VS_PD_TVAR")] public int UVSPDTVAR { get; set; }

        [JsonProperty("U_VS_PD_TPCR")] public string UVSPDTPCR { get; set; }

        [JsonProperty("U_VS_PD_CDUM")] public string UVSPDCDUM { get; set; }

        [JsonProperty("U_VS_PD_DSUM")] public string UVSPDDSUM { get; set; }

        [JsonProperty("U_VS_PD_DCCA")] public double UVSPDDCCA { get; set; }

        [JsonProperty("U_VS_PD_DCCP")] public double UVSPDDCCP { get; set; }

        [JsonProperty("U_VS_PD_PEAR")] public double UVSPDPEAR { get; set; }

        [JsonProperty("U_VS_PD_PSAR")] public double UVSPDPSAR { get; set; }

        [JsonProperty("U_VS_PD_VLAR")] public double UVSPDVLAR { get; set; }

        [JsonProperty("U_VS_PD_VTAR")] public double UVSPDVTAR { get; set; }

        [JsonProperty("U_VS_PD_PCAR")] public double UVSPDPCAR { get; set; }

        [JsonProperty("U_VS_PD_PTAR")] public double UVSPDPTAR { get; set; }

        [JsonProperty("U_VS_PD_OBJT")] public int UVSPDOBJT { get; set; }
    }

    public class TransferOrderRelatedBatchLines
    {
        [JsonProperty("DocEntry")] public int DocEntry { get; set; }

        [JsonProperty("LineId")] public int LineId { get; set; }

        [JsonProperty("VisOrder")] public int VisOrder { get; set; }

        [JsonProperty("Object")] public string Object { get; set; }

        [JsonProperty("LogInst")] public object LogInst { get; set; }

        [JsonProperty("U_VS_PD_DCEN")] public int UVSPDDCEN { get; set; }

        [JsonProperty("U_VS_PD_DCNM")] public int UVSPDDCNM { get; set; }

        [JsonProperty("U_VS_PD_DCLN")] public int UVSPDDCLN { get; set; }

        [JsonProperty("U_VS_PD_CDAR")] public string UVSPDCDAR { get; set; }

        [JsonProperty("U_VS_PD_CDAL")] public string UVSPDCDAL { get; set; }

        [JsonProperty("U_VS_PD_CDLT")] public string UVSPDCDLT { get; set; }

        [JsonProperty("U_VS_PD_FCFA")] public string UVSPDFCFA { get; set; }

        [JsonProperty("U_VS_PD_FCEX")] public string UVSPDFCEX { get; set; }

        [JsonProperty("U_VS_PD_DCCA")] public double UVSPDDCCA { get; set; }

        [JsonProperty("U_VS_PD_CADI")] public double UVSPDCADI { get; set; }

        [JsonProperty("U_VS_PD_CADE")] public double UVSPDCADE { get; set; }

        [JsonProperty("U_VS_PD_OBJT")] public int UVSPDOBJT { get; set; }
    }
}