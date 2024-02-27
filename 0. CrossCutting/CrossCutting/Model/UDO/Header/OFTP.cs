using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)] //Actualizar el UDO como formulario por defecto en caso de no generarse
    public class OFTP : BaseUDO
    {
        public const string ID = "VS_PD_OFTP";
        public const string DESCRIPTION = "Plantilla de carga";

        public OFTP()
        {
            ColumnDetail = new List<FTP1>();
        }

        [EnhancedColumn(1), ColumnProperty("Code"), FindColumn]
        public string Code { get; set; }

        [EnhancedColumn(2), ColumnProperty("Name"), FindColumn]
        public string Name { get; set; }

        //no se usa
        [EnhancedColumn(3), FieldNoRelated(@"U_EXK_CDSN", @"Codigo SN", BoDbTypes.Alpha, Size = 15)]
        public string CardCode { get; set; }

        [EnhancedColumn(4), FieldNoRelated(@"U_EXK_FILE", @"Archivo de carga", BoDbTypes.Alpha, Size = 254)]
        public string FileName { get; set; }

        [EnhancedColumn(5), FieldNoRelated(@"U_EXK_MAIN", @"Columna principal (Código)", BoDbTypes.Alpha, Size = 254)]
        public string MainColumnAddress { get; set; }

        [EnhancedColumn(6), FieldNoRelated(@"U_EXK_MNTL", @"Columna principal (Título)", BoDbTypes.Alpha, Size = 254)]
        public string MainColumnTitle { get; set; }

        //no deberia usarse
        [EnhancedColumn(7), FieldNoRelated("U_EXK_ADFR", "Formato entrega", BoDbTypes.Alpha, Size = 150)]
        public string AddressFormat { get; set; }

        [EnhancedColumn(8), FieldNoRelated("U_EXK_DTFR", "Formato fecha", BoDbTypes.Alpha, Size = 150)]
        public string DateFormat { get; set; }

        //no deberia usarse
        [EnhancedColumn(9), FieldNoRelated("U_EXK_TAXC", "Codigo Impuesto", BoDbTypes.Alpha, Size = 8)]
        public string TaxCode { get; set; }

        [ChildProperty(FTP1.ID)] public List<FTP1> ColumnDetail { get; set; }

        [ChildProperty(FTP2.ID)] public List<FTP2> ColumnMetadata { get; set; }
    }
}