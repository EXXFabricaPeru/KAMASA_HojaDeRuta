using System;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;
using MotiveTypeCodes = Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant.MotiveType;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(OLDP.ID, 1)]
    public class LDP2 : BaseUDO
    {
        public const string ID = "VS_PD_LDP2";
        public const string DESCRIPTION = "@VS_PD_OLDP.LDP2";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public int DocEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn, FieldNoRelated(@"U_EXK_CDRT", @"Código Ruta", BoDbTypes.Integer)]
        public int RouteEntry { get; set; }

        [EnhancedColumn, FieldNoRelated(@"U_EXK_NMRT", @"Número Ruta", BoDbTypes.Integer)]
        public int RouteNumber { get; set; }

        [EnhancedColumn, FieldNoRelated(@"U_EXK_CDMT", @"Código Motivo", BoDbTypes.Alpha, Size = 50)]
        public string MotiveCode { get; set; }

        [EnhancedColumn, FieldNoRelated(@"U_EXK_DSMT", @"Descripción Motivo", BoDbTypes.Alpha, Size = 254)]
        public string MotiveDescription { get; set; }

        [EnhancedColumn, FieldNoRelated(@"U_EXK_TPMT", @"Tipo Motivo", BoDbTypes.Alpha, Size = 2)]
        [Val(MotiveTypeCodes.EXTRA_CODE, MotiveTypeCodes.EXTRA_DESCRIPTION)]
        [Val(MotiveTypeCodes.PENALTY_CODE, MotiveTypeCodes.PENALTY_DESCRIPTION)]
        public string MotiveType { get; set; }

        public string MotiveTypeDescription
        {
            get
            {
                PropertyInfo property = GenericHelper.GetPropertyForExpression<LDP2, string>(t => t.MotiveType);
                return GenericHelper.GetDescriptionFromValue(property, MotiveType);
            }
        }

        [EnhancedColumn, FieldNoRelated(@"U_EXK_AMNT", @"Monto Aplicado", BoDbTypes.Price)]
        public decimal MotiveAmount { get; set; }

        [EnhancedColumn, FieldNoRelated(@"U_EXK_CMNT", @"Comentarios", BoDbTypes.Alpha, Size = 254)]
        public string Comments { get; set; }
    }
}