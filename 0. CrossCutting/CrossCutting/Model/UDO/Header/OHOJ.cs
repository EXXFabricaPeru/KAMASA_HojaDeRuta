// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ExplicitCallerInfoArgument
using System;
using System.Collections.Generic;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OHOJ : BaseUDO
    {
        public const string ID = "EXK_HOJARUTA";
        public const string DESCRIPTION = "EXK - Hoja de Ruta";

        public OHOJ()
        {
            DetalleZonas = new List<HOJ1>();
        }
        [FormColumn(0, Description = "Nro Doc."), ColumnProperty("Code"), FindColumn]
        public string SupplierId { get; set; }

        [FormColumn(1, Visible = false), ColumnProperty("Name"), FindColumn]
        public string Description { get; set; }

        [FormColumn(2), FieldNoRelated(@"U_EXK_TRANSP", @"Transportista", BoDbTypes.Alpha, Size = 200)]
        public string Transportista { get; set; }

        [FormColumn(3), FieldNoRelated(@"U_EXK_CHOFER", @"Chofer", BoDbTypes.Alpha, Size = 150)]
        public string Chofer { get; set; }

        [FormColumn(4), FieldNoRelated(@"U_EXK_AUX1", @"Auxiliar 1", BoDbTypes.Alpha, Size = 150)]
        public string Auxiliar1 { get; set; }

        [FormColumn(5), FieldNoRelated(@"U_EXK_AUX2", @"Auxiliar 2", BoDbTypes.Alpha, Size = 150)]
        public string Auxiliar2 { get; set; }

        [FormColumn(6), FieldNoRelated(@"U_EXK_AUX3", @"Auxiliar 3", BoDbTypes.Alpha, Size = 150)]
        public string Auxiliar3 { get; set; }

        [FormColumn(7), FieldNoRelated(@"U_EXK_PLACA", @"Placa", BoDbTypes.Alpha, Size = 100)]
        public string Placa { get; set; }

        [FormColumn(8), FieldNoRelated("U_EXK_FEINTRAS", "Inicio Traslado", BoDbTypes.Date, Size = 10)]
        public DateTime InicioTraslado { get; set; }

        [FormColumn(9), FieldNoRelated(@"U_EXK_FEFITRAS", @"Fin Traslado", BoDbTypes.Date, Size = 10)]
        public DateTime FinTraslado { get; set; }

        [FormColumn(10), FieldNoRelated(@"U_EXK_FECHA", @"Fecha", BoDbTypes.Date, Size = 10)]
        public DateTime Fecha { get; set; }

        [FormColumn(11), FieldNoRelated(@"U_EXK_ACTUALIZAR", @"Actualizar GR", BoDbTypes.Alpha, Size = 10)]
        public string ActualizarGR { get; set; }

        [FormColumn(12), FieldNoRelated(@"U_EXK_ESTADO", @"Estado", BoDbTypes.Alpha, Size = 10)]
        public string Estado { get; set; }

        [ChildProperty(HOJ1.ID)]
        public List<HOJ1> DetalleZonas { get; set; }

        //[ChildProperty(TRD2.ID)]
        //public List<TRD2> Vehicles { get; set; }


       
    }
}
