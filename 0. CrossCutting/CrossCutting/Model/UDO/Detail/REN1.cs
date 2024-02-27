using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class REN1
    {
        public const string ID = "VS_PD_REN1";
        public const string DESCRIPTION = "Vehículo Relacionado";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_VHRF", "Codigo", BoDbTypes.Alpha, Size = 12)]
        public string ReferenceVehicle { get; set; }
    }
}
