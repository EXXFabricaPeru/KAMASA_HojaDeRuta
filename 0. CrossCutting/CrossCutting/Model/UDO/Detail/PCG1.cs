using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [UDOFatherReference(OPCG.ID, 1)]
    public class PCG1 : BaseUDO
    {
        public const string ID = "VS_LT_PCG1";
        public const string DESCRIPTION = "Columnas archivo";

        [EnhancedColumn(Visible = false)]
        [ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false)]
        [ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(3)]
        [FieldNoRelated(@"U_VS_LT_FILD", @"Valor SAP", BoDbTypes.Alpha, Size = 254)]
        public string SAPValue { get; set; }

        [EnhancedColumn(4)]
        [FieldNoRelated(@"U_VS_LT_CLAR", @"Código Columna", BoDbTypes.Alpha, Size = 150)]
        public string ExcelColumnAddress { get; set; }

        [EnhancedColumn(5)]
        [FieldNoRelated(@"U_VS_LT_COLU", @"Título Columna", BoDbTypes.Alpha, Size = 150)]
        public string ExcelColumnName { get; set; }


      
    }
}
