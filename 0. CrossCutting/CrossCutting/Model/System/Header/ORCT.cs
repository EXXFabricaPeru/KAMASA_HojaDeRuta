using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header
{
    [SystemTable(nameof(ORCT), FormType = "dummy")]
    public class ORCT : BaseSAPTable
    {


        [FieldNoRelated("U_VS_LT_STAD", "Liquidado?", BoDbTypes.Alpha, Size = 2)]
        [Val("N", "NO")]
        [Val("Y", "SI")]
        public string EstadoLiquidacion { get; set; }
    }
}
