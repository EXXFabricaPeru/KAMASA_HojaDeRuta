using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{

    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_Document, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = false, CanManageSeries = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OLTR : BaseUDO
    {
        public const string ID = "VS_LT_OLTR";
        public const string DESCRIPTION = "Liquidacion_de_Tarjetas";

        [FormColumn(0, Description = "Código"), ColumnProperty("DocEntry"), FindColumn]
        public int DocumentEntry { get; set; }

        [FormColumn(1, Description = "Número"), ColumnProperty("DocNum"), FindColumn]
        public int DocumentNumber { get; set; }

        [FormColumn(2, Description = "Serie"), ColumnProperty("Series")]
        public int Series { get; set; }

        [FormColumn(3), FieldNoRelated("U_VS_LT_CDTD", "Código de Tienda", BoDbTypes.Alpha, Size = 100)]
        public string CodigoTienda { get; set; }

        [FormColumn(4), FieldNoRelated("U_VS_LT_DSTD", "Nombre de Tienda", BoDbTypes.Alpha, Size = 200)]
        public string NombreTienda { get; set; }

        [FormColumn(5), FieldNoRelated("U_VS_LT_PPAY", "Pasarela de Pago", BoDbTypes.Alpha, Size = 200)]
        public string PasarelaPago { get; set; }

        [FormColumn(6), FieldNoRelated("U_VS_LT_CCTR", "Cuenta Transitoria", BoDbTypes.Alpha, Size = 100)]
        public string CuentaTransitoria { get; set; }

        [FormColumn(7), FieldNoRelated("U_VS_LT_FEIV", "Fecha Inicio de Ventas", BoDbTypes.Date), FindColumn]
        public DateTime? FechaInicio { get; set; }

        [FormColumn(8), FieldNoRelated("U_VS_LT_FEFV", "Fecha Fin de Ventas", BoDbTypes.Date), FindColumn]
        public DateTime? FechaFin { get; set; }

        [FormColumn(9), FieldNoRelated("U_VS_LT_MONE", "Moneda", BoDbTypes.Alpha, Size = 100)]
        public string Moneda { get; set; }

        [FormColumn(10), FieldNoRelated("U_VS_LT_ARCH", "Archivo", BoDbTypes.Alpha, Size = 254)]
        public string Archivo { get; set; }

        [FormColumn(11), FieldNoRelated("U_VS_LT_FEAB", "Fecha Abono", BoDbTypes.Date), FindColumn]
        public DateTime? FechaAbono { get; set; }

        [FormColumn(12), FieldNoRelated("U_VS_LT_BANK", "Banco", BoDbTypes.Alpha, Size = 100)]
        public string Banco { get; set; }

        [FormColumn(13), FieldNoRelated("U_VS_LT_FLCJ", "Flujo de Caja", BoDbTypes.Alpha, Size = 200)]
        public string FlujoCaja { get; set; }

        [FormColumn(14), FieldNoRelated("U_VS_LT_NROP", "Nro Operación", BoDbTypes.Alpha, Size = 200)]
        public string NroOperacion { get; set; }

        [FormColumn(15), FieldNoRelated("U_VS_LT_TCAM", "T. Cambio", BoDbTypes.Price)]
        public double? TipoCambio { get; set; }

        [FormColumn(16), FieldNoRelated("U_VS_LT_CCCO", "Cuenta Contable", BoDbTypes.Alpha, Size = 100)]
        public string CuentaContable { get; set; }

        [FormColumn(17), FieldNoRelated("U_VS_LT_IMCO", "Importe Cobrado", BoDbTypes.Price)]
        public double? ImporteCobradoTotal { get; set; }

        [FormColumn(18), FieldNoRelated("U_VS_LT_OMPA", "Otro Medio de Pago", BoDbTypes.Price)]
        public double? OtroMedioPagoTotal { get; set; }

        [FormColumn(19), FieldNoRelated("U_VS_LT_COMI", "Comisión", BoDbTypes.Price)]
        public double? ComisionTotal { get; set; }

        [FormColumn(20), FieldNoRelated("U_VS_LT_COEM", "Comisión Emisor", BoDbTypes.Price)]
        public double? ComisionEmisorTotal { get; set; }

        [FormColumn(21), FieldNoRelated("U_VS_LT_COMP", "Comisión MCPeru", BoDbTypes.Price)]
        public double? ComisionMCPeruTotal { get; set; }

        [FormColumn(22), FieldNoRelated("U_VS_LT_IGVT", "IGV", BoDbTypes.Price)]
        public double? IGVTotal { get; set; }

        [FormColumn(23), FieldNoRelated("U_VS_LT_NTPA", "Neto Parcial", BoDbTypes.Price)]
        public double? NetoParcialTotal { get; set; }

        [FormColumn(24), FieldNoRelated("U_VS_LT_SMCO", "Suma de Comsiones", BoDbTypes.Price)]
        public double? SumaComisiones { get; set; }

        [FormColumn(25), FieldNoRelated("U_VS_LT_IMDE", "Importe Depositado", BoDbTypes.Price)]
        public double? ImporteDepositadoTotal { get; set; }

        [FormColumn(26), FieldNoRelated("U_VS_LT_IMAJ", "Importe por Ajuste", BoDbTypes.Price)]
        public double? ImporteAjusteTotal { get; set; }

        [FormColumn(27), FieldNoRelated("U_VS_LT_TOGE", "Total General", BoDbTypes.Price)]
        public double? TotalGeneral { get; set; }


        [ChildProperty(LTR1.ID)] public List<LTR1> RelatedLines { get; set; }

    }
}
