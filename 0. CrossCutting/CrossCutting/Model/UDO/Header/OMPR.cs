// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantArgumentDefaultValue

using System;
using System.Collections.Generic;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_Document, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true, CanManageSeries = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OMPR : BaseUDO
    {
        public const string ID = "VS_PD_OMPR";
        public const string DESCRIPTION = "Mapa_Puntos_de_Ruteo";

        [FormColumn(0, Description = "Código"), ColumnProperty("DocEntry")]
        public int DocumentEntry { get; set; }

        [FormColumn(1, Description = "Número"), ColumnProperty("DocNum")]
        public int DocumentNumber { get; set; }

        [FormColumn(2, Description = "Serie"), ColumnProperty("Series")]
        public int Series { get; set; }

        [FormColumn(3), FieldNoRelated("U_EXK_COCL", "Código del Cliente", BoDbTypes.Alpha, Size = 50)]
        public string ClientId { get; set; }

        [FormColumn(4), FieldNoRelated("U_EXK_DSCL", "Descripción del Cliente", BoDbTypes.Alpha, Size = 100)]
        public string ClientName { get; set; }

        [FormColumn(5), FieldNoRelated("U_EXK_COEN", "Código de Entrega", BoDbTypes.Alpha, Size = 50)]
        public string AddressId { get; set; }

        [FormColumn(6), FieldNoRelated("U_EXK_DREN", "Punto de Entrega", BoDbTypes.Alpha, Size = 254)]
        public string AddressDescription { get; set; }

        [FormColumn(7), FieldNoRelated("U_EXK_DRLT", "Latidud de Entrega", BoDbTypes.Quantity)]
        public decimal AddressLatitude { get; set; }

        [FormColumn(8), FieldNoRelated("U_EXK_DRLG", "Longitud de Entrega", BoDbTypes.Quantity)]
        public decimal AddressLongitude { get; set; }

        [ChildProperty(MPR1.ID)]
        public List<MPR1> RelatedDistances { get; set; }
    }
}