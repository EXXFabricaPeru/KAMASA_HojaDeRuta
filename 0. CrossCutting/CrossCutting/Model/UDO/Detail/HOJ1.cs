using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(OHRG.ID, 1)]
    public class HOJ1 : BaseUDO
    {
        public const string ID = "EXK_DHOJARUTA";
        public const string DESCRIPTION = "EXK - Hoja Ruta Linea";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(1), FieldNoRelated("U_EXK_CODZONDESP", "Cod. Zona Despacho", BoDbTypes.Alpha, Size = 10)]
        public string CodZonaDespacho { get; set; }
        
        [EnhancedColumn(2), FieldNoRelated("U_EXK_ZONDESP", "Zona Despacho", BoDbTypes.Alpha, Size = 100)]
        public string ZonaDespacho { get; set; }



    }
}
