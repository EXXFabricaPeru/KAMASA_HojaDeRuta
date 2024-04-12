// ReSharper disable InconsistentNaming
using System;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header
{
    [SystemTable("OBTN", FormType = "dummy")]
    public class OIBT : BaseSAPTable
    {
        public int AbsEntry { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string BatchNumber { get; set; }
        public string WarehouseCode { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public decimal Quantity { get; set; }

        [FieldNoRelated("VS_PD_CNRV", "Cantidad reservada - Distribución", BoDbTypes.Price, Default = "0")]
        public decimal ReservedAmount { get; set; }
    }
}