// ReSharper disable InconsistentNaming

using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class DCR1 : BaseUDO
    {
        public const string ID = "VS_PD_DCR1";
        public const string DESCRIPTION = "@VS_PD_ODCR.DCR1";

        [EnhancedColumn(Visible = false), ColumnProperty(@"DocEntry")]
        public int DocumentEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty(@"LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated(@"U_EXK_NMCR", @"Contador", BoDbTypes.Integer)]
        public int Counter { get; set; }

        [EnhancedColumn, FieldNoRelated(@"U_EXK_CDRT", @"Código Ruta", BoDbTypes.Integer)]
        public int RouteEntry { get; set; }

        [EnhancedColumn, FieldNoRelated(@"U_EXK_NMRT", @"Número Ruta", BoDbTypes.Integer)]
        public int RouteNumber { get; set; }
    }
}