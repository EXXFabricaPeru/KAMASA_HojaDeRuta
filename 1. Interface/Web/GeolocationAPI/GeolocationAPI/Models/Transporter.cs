using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeolocationAPI.Models
{
    public class Transporter
    {
        
        [JsonProperty("Code")] public string Code { get; set; }

        [JsonProperty("Name")] public string Name { get; set; }

        [JsonProperty("U_VS_PD_DSPR")] public string Description { get; set; }

        [JsonProperty("U_VS_PD_TPDC")] public string DocumentType { get; set; }

        [JsonProperty("U_VS_PD_NMDC")] public string DocumentId { get; set; }

        [JsonProperty("U_VS_PD_NMTL")] public string Phone { get; set; }

        [JsonProperty("U_VS_PD_CRET")] public string Email { get; set; }

        [JsonProperty("U_VS_PD_PRDS")] public int? Priority { get; set; }

        [JsonProperty("U_VS_PD_ACVH")] public string IsActive { get; set; }

        [JsonProperty("VS_PD_TRD1Collection")] public List<TransporterDriver> Drivers { get; set; }

        [JsonProperty("VS_PD_TRD2Collection")] public List<TransporterVehicle> Vehicles { get; set; }
    }

    public class TransporterDriver
    {
        [JsonProperty("Code")] public string Code { get; set; }

        [JsonProperty("LineId")] public int LineId { get; set; }

        [JsonProperty("U_VS_PD_CDCD")] public string ReferenceId { get; set; }

        [JsonProperty("U_VS_PD_LCCD")] public string License { get; set; }

        [JsonProperty("U_VS_PD_NMCD")] public string Name { get; set; }

        [JsonProperty("U_VS_PD_LCVC")] public DateTime? LicenseExpiry { get; set; }

        [JsonProperty("U_VS_PD_STDC")] public string IsAvailable { get; set; }
    }

    public class TransporterVehicle
    {
        [JsonProperty("Code")] public string Code { get; set; }

        [JsonProperty("LineId")] public int LineId { get; set; }

        [JsonProperty("U_VS_PD_CDVH")] public string ReferenceId { get; set; }

        [JsonProperty("U_VS_PD_VEPL")] public string Plate { get; set; }

        [JsonProperty("U_VS_PD_CPDC")] public double LoadCapacity { get; set; }

        [JsonProperty("U_VS_PD_VLDC")] public double VolumeCapacity { get; set; }

        [JsonProperty("U_VS_PD_TPDS")] public string DistributionType { get; set; }

        [JsonProperty("U_VS_PD_SRVH")] public string HasFreeze { get; set; }

        [JsonProperty("U_VS_PD_ACVH")] public string IsAvailable { get; set; }

        public int Priority { get; set; }
    }
}