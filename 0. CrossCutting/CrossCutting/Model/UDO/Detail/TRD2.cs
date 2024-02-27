// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
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
    [UDOFatherReference(OTRD.ID, 2)]
    public class TRD2 : BaseUDO
    {
        public const string ID = "VS_PD_TRD2";
        public const string DESCRIPTION = "Vehículo para Distribución";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(1), FieldNoRelated("U_EXK_CDVH", "Código", BoDbTypes.Alpha, Size = 50)]
        public string Identifier { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_VEPL", "Placa", BoDbTypes.Alpha, Size = 50)]
        public string LisencePlate { get; set; }
        
        [EnhancedColumn(3), FieldNoRelated("U_EXK_CPDC", "Capacidad de Carga", BoDbTypes.Price)]
        public decimal LoadCapacity { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_EXK_VLDC", "Volumen de Carga", BoDbTypes.Price)]
        public decimal VolumeCapacity { get; set; }

        [EnhancedColumn(5), FieldNoRelated("U_EXK_TPDS", "Tipo de Distribución", BoDbTypes.Alpha, Size = 3)]
        public string DistributionType { get; set; }

        [EnhancedColumn(6), FieldNoRelated("U_EXK_SRVH", "Posee Refrigeración", BoDbTypes.Alpha, Size = 1)]
        [Val("Y", "Si"), Val("N", "No")]
        public string HasRefrigeration { get; set; }

        [EnhancedColumn(7), FieldNoRelated("U_EXK_ACVH", "Esta activo", BoDbTypes.Alpha, Size = 1)]
        [Val("Y", "Si"), Val("N", "No")]
        public string IsAvailable { get; set; }


    }
}
