// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument
using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(ORTU.ID, 1)]
    public class RTU1 : BaseUDO
    {
        public const string ID = "VS_PD_RTU1";
        public const string DESCRIPTION = "Detalle archivo";

        [EnhancedColumn(Visible = false), ColumnProperty("Docentry")]
        public string DocumentEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(1), FieldNoRelated("U_EXK_SELE", "Seleccionado", BoDbTypes.Alpha, Size = 1)]
        public string Selected { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_IDOR", "Identificador", BoDbTypes.Alpha, Size = 150)]
        public string OrderID { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXK_CCOD", "Código SN", BoDbTypes.Alpha, Size = 15)]
        public string CardCode { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXK_CNAM", "Razon Social", BoDbTypes.Alpha, Size = 100)]
        public string CardName { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_EXK_DATE", "Fecha Distribución", BoDbTypes.Date)]
        public string DocDate { get; set; }

        [EnhancedColumn(5), FieldNoRelated("U_EXK_DDAT", "Fecha Creación", BoDbTypes.Date)]
        public string DocDueDate { get; set; }

        [EnhancedColumn(6), FieldNoRelated("U_EXK_SHIP", "Dirección Envío", BoDbTypes.Integer)]
        public string ShipToAddress { get; set; }

        [EnhancedColumn(7), FieldNoRelated("U_EXK_DETRY", "DocEntry", BoDbTypes.Integer)]
        public string DocEntry { get; set; }

        [EnhancedColumn(8), FieldNoRelated("U_EXK_STAT", "Estado", BoDbTypes.Alpha, Size = 1)]
        public string Status { get; set; }
        [EnhancedColumn(9), FieldNoRelated("U_EXK_ERRM", "Mensaje Error", BoDbTypes.Alpha, Size = 254)]
        public string ErrorMessage { get; set; }
        [EnhancedColumn(10), FieldNoRelated("U_EXK_NDOC", "Número Documentos", BoDbTypes.Alpha, Size = 254)]
        public string NumDocument { get; set; }
    }
}
