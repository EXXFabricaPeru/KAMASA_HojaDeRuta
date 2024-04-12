// ReSharper disable InconsistentNaming
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header
{
    [SystemTable(nameof(OITM), FormType = "dummy")]
    public class OITM : BaseSAPTable
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string InventoryMeasureUnitCode { get; set; }
        public string SaleUnitMeasureDescription { get; set; }
        public decimal InventoryUnitPerSale { get; set; }
        public decimal SaleUnit { get; set; }
        public decimal BuyUnit { get; set; }
        public decimal SaleWeight { get; set; }
        public decimal InventoryWeight { get; set; }
        public string SaleMeasureUnitCode { get; set; }
        public decimal SaleVolume { get; set; }
        public bool IsInventory { get; set; }
        public bool IsSale { get; set; }

        [FieldNoRelated("VS_PD_TPRF", "Tipo de carga", BoDbTypes.Alpha, Size = 2)]
        [Val("RL", "Refrigerado"), Val("DL", "Seco"), Val("FL","Congelado")]
        public string LoadType { get; set; }

        [FieldNoRelated("VS_PD_PRCJ", "Porcentaje de caja", BoDbTypes.Price)]
        public decimal BoxPercent { get; set; }

        [FieldNoRelated(@"VS_PD_CFIL", @"Código Archivo EDI", BoDbTypes.Alpha, Size = 50)]
        public string EDICode { get; set; }
        
        public static class DistributionLoadType
        {
            public const string DRY = "DL";
            public const string REFRIGERATED = "RL";
        }

        [SAPColumn("U_VS_TIPITM",false)]
        [FieldNoRelated("U_VS_TIPITM", "Tipo de Item", BoDbTypes.Alpha, Size = 2)]
        [Val("PR", "Productos")]
        [Val("AC", "Activos")]
        [Val("SV", "Servicios")]
        [Val("CD", "Costos por Distribuir")]
        [Val("CP", "Costos de Producción")]
        [Val("CS", "Costos de Servicio")]
        [Val("GA", "Gastos Administrativos")]
        [Val("GV", "Gastos de Ventas")]
        [Val("GF", "Gastos Financieros")]
        public string TipoItem { get; set; }

        [SAPColumn("U_CL_TVU", false)]
        [FieldNoRelated("U_CL_TVU", "Solicitar TVU", BoDbTypes.Alpha, Size = 2)]
        [Val("N", "NO")]
        [Val("Y", "SI")]
        public string TVU { get; set; }
        
        //[SAPColumn(@"U_VS_CATEGORIALP", false)]
        //public string PriceListCategory { get; set; }
    }
}