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
    public class HGR1 : BaseUDO
    {
        public const string ID = "EX_HR_HGR1";
        public const string DESCRIPTION = "Detalle de Guías";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(1), FieldNoRelated("U_EXK_SEL", "Selección", BoDbTypes.Alpha, Size = 50,Default ="N")]
        [Val("Y", "SI")]
        [Val("N", "NO")]
        public string Seleccion { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_EXK_GUIA", "G. Remisión", BoDbTypes.Alpha, Size = 50)]
        public string GuiaRemision { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_EXK_PESO", "Pedo Kg.", BoDbTypes.Quantity)]
        public double PesoKG { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_EXK_CANTB", "Cant. Bultos", BoDbTypes.Quantity)]
        public double CantBultos { get; set; }

        [EnhancedColumn(5), FieldNoRelated("U_EXK_PROG", "Programado", BoDbTypes.Alpha, Size = 1)]
        [Val("Y", "Si"), Val("N", "No")]
        public string Programado { get; set; }

        [EnhancedColumn(6), FieldNoRelated("U_EXK_ZONA", "Zona", BoDbTypes.Alpha, Size = 50)]
        public string Zona { get; set; }

        [EnhancedColumn(7), FieldNoRelated("U_EXK_DIREC", "Dirección Despacho", BoDbTypes.Alpha, Size = 254)]
        public string DireccionDespacho { get; set; }

        [EnhancedColumn(8), FieldNoRelated("U_EXK_DEPA", "Departamento - Provincia - Distrito", BoDbTypes.Alpha, Size = 254)]
        public string DepartamentoProvDis { get; set; }

    }
}
