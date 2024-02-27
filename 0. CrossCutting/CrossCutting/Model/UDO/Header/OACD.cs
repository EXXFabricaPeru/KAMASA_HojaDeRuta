using System.Collections.Generic;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OACD : BaseUDO
    {
        public const string ID = "VS_PD_OACD";
        public const string DESCRIPTION = "Asignacion_Certificados";

        [FormColumn(0, Description = "Id"), ColumnProperty("Code"), FindColumn]
        public string Code { get; set; }

        [FormColumn(1), FieldNoRelated("U_EXK_CDET", "Código Entidad", BoDbTypes.Alpha, Size = 50), FindColumn]
        public string EntityId { get; set; }

        [FormColumn(2), FieldNoRelated("U_EXK_TPET", "Tipo Entidad", BoDbTypes.Alpha, Size = 2)]
        [Val("CD", "Conductores"), Val("AX", "Auxiliares"), Val("CL", "Clientes")]
        public string EntityType { get; set; }

        [FormColumn(3), FieldNoRelated("U_EXK_NMET", "Nombre Entidad", BoDbTypes.Alpha, Size = 150), FindColumn]
        public string EntityName { get; set; }

        [ChildProperty(ACD1.ID)]
        public List<ACD1> Certificates { get; set; }

        public class EntityTypes
        {
            public const string DRIVER = "CD";
            public const string AUXILIARY = "AX";
            public const string CLIENT = "CL";
        }
    }
}