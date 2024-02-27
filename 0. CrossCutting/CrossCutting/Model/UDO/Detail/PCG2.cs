using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(OPCG.ID, 1)]
    public class PCG2
    {
        public const string ID = "VS_LT_PCG2";
        public const string DESCRIPTION = "Metadato de Columnas";

        [EnhancedColumn(Visible = false)]
        [ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false)]
        [ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(3)]
        [FieldNoRelated(@"U_EXK_CLAR", @"Código Columna", BoDbTypes.Alpha, Size = 150)]
        public string ExcelColumnAddress { get; set; }

        [EnhancedColumn(4)]
        [FieldNoRelated(@"U_EXK_COLU", @"Título Columna", BoDbTypes.Alpha, Size = 150)]
        public string ExcelColumnName { get; set; }
    }
}
