// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument

using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(OFTP.ID, 1)]
    public class FTP1 : BaseUDO
    {
        public const string ID = "VS_PD_FTP1";
        public const string DESCRIPTION = "Columnas archivo";

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

        [EnhancedColumn(5)]
        [FieldNoRelated(@"U_EXK_DEST", @"Tipo Celda", BoDbTypes.Alpha, Size = 2)]
        [Val(TargetTypes.EXCEL_VALUE_CODE, TargetTypes.EXCEL_VALUE_DESCRIPTION)]
        [Val(TargetTypes.DEFAULT_VALUE_CODE, TargetTypes.DEFAULT_VALUE_DESCRIPTION)]
        [Val(TargetTypes.QUERY_VALUE_CODE, TargetTypes.QUERY_VALUE_DESCRIPTION)]
        [Val(TargetTypes.EXCEL_INTERMEDIATE_VALUE_CODE, TargetTypes.EXCEL_INTERMEDIATE_VALUE_DESCRIPTION)]
        public string TargetType { get; set; }

        [EnhancedColumn(6)]
        [FieldNoRelated(@"U_EXK_FILS", @"Estructura SAP", BoDbTypes.Alpha, Size = 2)]
        [Val(SAPStructures.HEADER_CODE, SAPStructures.HEADER_DESCRIPTION)]
        [Val(SAPStructures.LINE_CODE, SAPStructures.LINE_DESCRIPTION)]
        public string SAPStructure { get; set; }

        [EnhancedColumn(7)]
        [FieldNoRelated(@"U_EXK_FILD", @"Valor SAP", BoDbTypes.Alpha, Size = 254)]
        public string SAPValue { get; set; }

        [EnhancedColumn(8)]
        [FieldNoRelated(@"U_EXK_FILC", @"Valor Defecto", BoDbTypes.Alpha, Size = 254)]
        public string DefaultValue { get; set; }

        [EnhancedColumn(9)]
        [FieldNoRelated(@"U_EXK_FILQ", @"Valor Query", BoDbTypes.Alpha, Size = 254)]
        public string QueryValue { get; set; }

        public string RetrieveTargetTypeDescription()
        {
            switch (TargetType)
            {
                case TargetTypes.EXCEL_VALUE_CODE: return TargetTypes.EXCEL_VALUE_DESCRIPTION;
                case TargetTypes.DEFAULT_VALUE_CODE: return TargetTypes.DEFAULT_VALUE_DESCRIPTION;
                case TargetTypes.QUERY_VALUE_CODE: return TargetTypes.QUERY_VALUE_DESCRIPTION;
                case TargetTypes.EXCEL_INTERMEDIATE_VALUE_CODE: return TargetTypes.EXCEL_INTERMEDIATE_VALUE_DESCRIPTION;
                default: return string.Empty;
            }
        }

        public static class SAPStructures
        {
            public const string HEADER_CODE = "HD";
            public const string HEADER_DESCRIPTION = "Cabecera";

            public const string LINE_CODE = "LN";
            public const string LINE_DESCRIPTION = "Linea Detalle";
        }

        public static class TargetTypes
        {
            public const string EXCEL_VALUE_CODE = "EV";
            public const string EXCEL_VALUE_DESCRIPTION = "Valor Excel";

            public const string EXCEL_INTERMEDIATE_VALUE_CODE = "EI";
            public const string EXCEL_INTERMEDIATE_VALUE_DESCRIPTION = "Valor Excel c/ Tabla Intermedia";

            public const string DEFAULT_VALUE_CODE = "DV";
            public const string DEFAULT_VALUE_DESCRIPTION = "Valor Defecto";

            public const string QUERY_VALUE_CODE = "QV";
            public const string QUERY_VALUE_DESCRIPTION = "Valor Query";
        }
    }
}