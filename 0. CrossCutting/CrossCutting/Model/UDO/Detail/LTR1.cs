using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(OLTR.ID, 1)]
    public class LTR1 : BaseUDO
    {
        public const string ID = "VS_LT_LTR1";
        public const string DESCRIPTION = "Detalle Liquidación";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public string DocEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_CDTD", "Código Tienda", BoDbTypes.Alpha, Size = 150)]
        public string CodigoTienda { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_PPAY", "Pasarela de Pago", BoDbTypes.Alpha, Size = 150)]
        public string PasarelaPago { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_CCTR", "Cuenta Transitoria", BoDbTypes.Alpha, Size = 150)]
        public string CuentaTransitoria { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_TDOC", "Tipo Doc.", BoDbTypes.Alpha, Size = 50)]
        public string TipoDocumento { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_NTKT", "Nro de Ticket", BoDbTypes.Alpha, Size = 150)]
        public string NroTicket { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_COCL", "Código de Cliente", BoDbTypes.Alpha, Size = 150)]
        public string CodigoCliente { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_NOCL", "Nombre de Cliente", BoDbTypes.Alpha, Size = 254)]
        public string NombreCliente { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_FEDO", "Fecha de Doc.", BoDbTypes.Date)]
        public DateTime FechaDocumento { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_IMDO", "Importe del Doc.", BoDbTypes.Price)]
        public double ImporteDocumento { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_MONE", "Moneda", BoDbTypes.Alpha, Size = 100)]
        public string Moneda { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_NSAP", "Nro. SAP del Cobro", BoDbTypes.Integer)]
        public int NroSAPCobro { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_NREF", "Nro. Referencia de Cobro", BoDbTypes.Alpha, Size = 150)]
        public string NroReferenciaCobro { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_TCAM", "Tipo de Cambio", BoDbTypes.Price)]
        public double TipoCambio { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_FECO", "Fecha Cobro", BoDbTypes.Date)]
        public DateTime FechaCobro { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_NTAR", "Nro.  de Tarjeta", BoDbTypes.Alpha, Size = 150)]
        public string NroTarjeta { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_IMTA", "Importe Cobrado Tarjeta", BoDbTypes.Price)]
        public double ImporteCobradoTarjeta { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_CONC", "Código Doc.", BoDbTypes.Integer)]
        public string CodigoDocumento{ get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_OMPA", "Otro Medio de Pago", BoDbTypes.Price)]
        public double OtroMedioPago { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_COMI", "Comisión", BoDbTypes.Price)]
        public double Comision { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_COEM", "Comisión Emisor", BoDbTypes.Price)]
        public double ComisionEmisor { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_COMP", "Comisión MCPeru", BoDbTypes.Price)]
        public double ComisionMCPeru { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_IGVT", "IGV", BoDbTypes.Price)]
        public double IGV { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_NTPA", "Neto Parcial", BoDbTypes.Price)]
        public double NetoParcial { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_MOAJ", "Motivos de Ajustes", BoDbTypes.Alpha, Size = 200)]
        public string MotivoAjuste { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_CCAS", "Cuenta Asignada", BoDbTypes.Alpha, Size = 100)]
        public string CuentaAsignada { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_SNAS", "Socio de Negocio Asociado", BoDbTypes.Alpha, Size = 150)]
        public string SocioNegocioAsociado { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_IMAJ", "Importe de Ajuste", BoDbTypes.Price)]
        public double ImporteAjuste { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_NROP", "Nro de Operación", BoDbTypes.Alpha, Size = 150)]
        public string NroOperacion { get; set; }

        [EnhancedColumn, FieldNoRelated("U_VS_LT_STAD", "Estado", BoDbTypes.Alpha, Size = 150)]
        public string Estado { get; set; }



    }
}
