using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    public class PDG1
    {
        public const string ID = "VS_PD_PDG1";
        public const string DESCRIPTION = "Coordenadas limitantes";

        [EnhancedColumn(Visible = false), ColumnProperty("Code")]
        public string Code { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_LATI", "Latitud", BoDbTypes.Price)]
        public decimal Latitude { get; set; }

        [EnhancedColumn, FieldNoRelated("U_EXK_LONG", "Longitud", BoDbTypes.Price)]
        public decimal Longitude { get; set; }
    }
}