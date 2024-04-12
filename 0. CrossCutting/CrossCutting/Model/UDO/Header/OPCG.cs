using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OPCG : BaseUDO
    {
        public const string ID = "VS_LT_OPCG";
        public const string DESCRIPTION = "Plantilla_de_carga";

        public OPCG()
        {
            ColumnDetail = new List<PCG1>();
        }


        [FormColumn(0, Description = "Código"), ColumnProperty("Code"), FindColumn]
        public string Code { get; set; }

        [FormColumn(1, Description = "Descripción"), ColumnProperty("Name"), FindColumn]
        public string Name { get; set; }

        [FormColumn(2), FieldNoRelated(@"U_VS_LT_CDPP", @"Pasarela de Pago", BoDbTypes.Alpha, Size = 100)]
        public string Pasarela { get; set; }

        [FormColumn(3), FieldNoRelated(@"U_VS_LT_FILE", @"Archivo de carga", BoDbTypes.Alpha, Size = 254)]
        public string FileName { get; set; }

        [FormColumn(4), FieldNoRelated("U_VS_LT_DTFR", "Formato fecha", BoDbTypes.Alpha, Size = 150)]
        public string DateFormat { get; set; }

        [FormColumn(5), FieldNoRelated(@"U_VS_LT_MAIN", @"Columna principal (Código)", BoDbTypes.Alpha, Size = 254)]
        public string MainColumnAddress { get; set; }

        [FormColumn(6), FieldNoRelated(@"U_VS_LT_MNTL", @"Columna principal (Título)", BoDbTypes.Alpha, Size = 254)]
        public string MainColumnTitle { get; set; }
     

        [ChildProperty(PCG1.ID)]
        public List<PCG1> ColumnDetail { get; set; }

        [ChildProperty(PCG2.ID)] 
        public List<PCG2> ColumnMetadata { get; set; }
    }
}
