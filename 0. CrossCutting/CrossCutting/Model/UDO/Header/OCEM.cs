using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{

    [Serializable]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    [UserDefinedTable(ID, DESCRIPTION)]
    public class OCEM : BaseUDO
    {

        public const string ID = "CL_CODEMP";
        public const string DESCRIPTION = "Codigo_Empresa";

        [EnhancedColumn(0), ColumnProperty("Code", ColumnName = "Code")]
        public string Code { get; set; }

        [EnhancedColumn(1), ColumnProperty("Name", ColumnName = "Name")]
        public string Name { get; set; }

        [EnhancedColumn(2), FieldNoRelated("U_CL_CODCLIE", "Codigo Cliente", BoDbTypes.Alpha, Size = 15)]
        public string CodigoCliente { get; set; }

        [EnhancedColumn(3), FieldNoRelated("U_CL_CODANEXO", "Codigo Anexo", BoDbTypes.Alpha, Size = 5)]
        public string CodigoAnexo { get; set; }

        [EnhancedColumn(4), FieldNoRelated("U_CL_DIREMP", "Direccion Empresa", BoDbTypes.Alpha, Size = 100)]
        public string DireccionEmpreasa { get; set; }

        [EnhancedColumn(5), FieldNoRelated("U_CL_CODUBIG", "Codigo Ubigeo", BoDbTypes.Alpha, Size = 10)]
        public string CodigoUbigeo { get; set; }

        [EnhancedColumn(6), FieldNoRelated("U_CL_TIPOTIEN", "Tipo Tienda", BoDbTypes.Alpha, Size = 2)]
        public string TipoTienda { get; set; }

        [EnhancedColumn(7), FieldNoRelated("U_CL_DESTIPOTIEN", "Descripcion Tipo Tienda", BoDbTypes.Alpha, Size = 30)]
        public string DescripcionTipoTienda { get; set; }

        [EnhancedColumn(8), FieldNoRelated("U_CL_ACTIVO", "Activo", BoDbTypes.Alpha, Size = 1)]
        [Val("1", "Activo"), Val("2", "Inactivo")]
        public string Activo { get; set; }

        [EnhancedColumn(9), FieldNoRelated("U_CL_CORREO", "Email", BoDbTypes.Alpha, Size = 50)]
        public string Email { get; set; }

        [EnhancedColumn(10), FieldNoRelated("U_CL_CODESTA", "Codigo Establecimiento", BoDbTypes.Alpha, Size = 5)]
        public string CodigoEstablecimiento { get; set; }

        [EnhancedColumn(11), FieldNoRelated("U_CL_IDZONA", "ID Zona", BoDbTypes.Alpha, Size = 2)]
        public string IDZona { get; set; }

        [EnhancedColumn(12), FieldNoRelated("U_CL_CONFIRMAR", "Segunda Confirmación", BoDbTypes.Alpha, Size = 1,Default ="N")]
        [Val("Y", "SI"), Val("N", "NO")]
        public string SegundaConfirmacion { get; set; }

        [EnhancedColumn(13), FieldNoRelated("U_CL_REPORTERIA", "Flat de Reporteria", BoDbTypes.Alpha, Size = 1)]
        [Val("Y", "SI"), Val("N", "NO")]
        public string FlatReporteria { get; set; }

    }
}
