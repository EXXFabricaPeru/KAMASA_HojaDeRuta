using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class VHR1
    {
        public const string ID = "VS_PD_VHR1";
        public const string DESCRIPTION = "Ventana_Relacionada";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_HRIN", "Inicio", BoDbTypes.Alpha, Size = 10)] //U_EXK_HRIN	U_EXK_HRFN
        public int StartHour { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_HRFN", "Fin", BoDbTypes.Alpha, Size = 10)]
        public int EndHour { get; set; }
    }
}