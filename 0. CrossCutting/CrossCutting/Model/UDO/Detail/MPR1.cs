using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class MPR1
    {
        public const string ID = "VS_PD_MPR1";
        public const string DESCRIPTION = "Distancias Relacionadas";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public string DocumentEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_COCL", "Código del Cliente", BoDbTypes.Alpha, Size = 50)]
        public string ClientId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DSCL", "Descripción del Cliente", BoDbTypes.Alpha, Size = 100)]
        public string ClientName { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_COEN", "Código de Dirección", BoDbTypes.Alpha, Size = 50)]
        public string AddressId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DREN", "Dirección del Cliente", BoDbTypes.Alpha, Size = 254)]
        public string AddressDescription { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DSDE", "Distancia", BoDbTypes.Quantity)]
        public decimal Distance { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DRLT", "Latidud", BoDbTypes.Quantity)]
        public decimal AddressLatitude { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_DRLG", "Longitud", BoDbTypes.Quantity)]
        public decimal AddressLongitude { get; set; }
    }
}