﻿using System;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(OFTP.ID, 1)]
    public class FTP2
    {
        public const string ID = "VS_PD_FTP2";
        public const string DESCRIPTION = "Metadato de Columnas";

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
    }
}