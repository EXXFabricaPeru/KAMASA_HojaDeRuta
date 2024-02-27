using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(OHRG.ID, 1)]
    public class HOJ1 : BaseUDO
    {
        public const string ID = "EX_HR_HOJ1";
        public const string DESCRIPTION = "Detalle de Guías";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(1), FieldNoRelated("U_EXK_ZONA", "Zona Despacho", BoDbTypes.Alpha, Size = 50)]
        public string ZonaDespacho { get; set; }



    }
}
