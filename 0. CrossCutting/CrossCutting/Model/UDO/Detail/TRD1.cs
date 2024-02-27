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
    [UDOFatherReference(OTRD.ID, 1)]
    public class TRD1 : BaseUDO
    {
        public const string ID = "VS_PD_TRD1";
        public const string DESCRIPTION = "Conductor para Distribución";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(1), FieldNoRelated("U_EXK_CDCD", "Código", BoDbTypes.Alpha, Size = 50)]
        public string DriverId { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_NMCD", "Nombre", BoDbTypes.Alpha, Size = 50)]
        public string DriverName { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXK_LCCD", "Licencia", BoDbTypes.Alpha, Size = 50)]
        public string DriveLicense { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_EXK_LCVC", "Vencimiento Licencia", BoDbTypes.Date, Size = 10)]
        public DateTime? LicenseExpiry { get; set; }

        [EnhancedColumn(5), FieldNoRelated("U_EXK_STDC", "Estado", BoDbTypes.Alpha, Size = 1)]
        [Val("Y", "Si"), Val("N", "No")]
        public string IsAvailable { get; set; }
    }
}