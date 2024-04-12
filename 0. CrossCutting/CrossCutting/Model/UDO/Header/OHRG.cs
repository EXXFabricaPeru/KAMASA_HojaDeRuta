// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ExplicitCallerInfoArgument
using System;
using System.Collections.Generic;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_MasterData, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = false,CanCancel =false)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OHRG : BaseUDO
    {
        public const string ID = "EX_HR_OHGR";
        public const string DESCRIPTION = "HOJA_DE_RUTA_ASIGNACION";

        public OHRG()
        {
            DetalleGuias = new List<HGR1>();
        }
        [FormColumn(0, Description = "Nro Doc."), ColumnProperty("Code"), FindColumn]
        public string SupplierId { get; set; }

        [FormColumn(1, Visible = false), ColumnProperty("Name"), FindColumn]
        public string Description { get; set; }

        [FormColumn(2), FieldNoRelated(@"U_EXK_COD", @"Hoja de Ruta", BoDbTypes.Alpha, Size = 100)]
        public string HojaRuta { get; set; }

        [FormColumn(3), FieldNoRelated(@"U_EXK_TRANSP", @"Transportista", BoDbTypes.Alpha, Size = 200)]
        public string Transportista { get; set; }

        [FormColumn(4), FieldNoRelated(@"U_EXK_CHOFER", @"Chofer", BoDbTypes.Alpha, Size = 150)]
        public string Chofer { get; set; }

        [FormColumn(5), FieldNoRelated(@"U_EXK_AUX1", @"Auxiliar 1", BoDbTypes.Alpha, Size = 150)]
        public string Auxiliar1 { get; set; }

        [FormColumn(6), FieldNoRelated(@"U_EXK_AUX2", @"Auxiliar 2", BoDbTypes.Alpha, Size = 150)]
        public string Auxiliar2 { get; set; }

        [FormColumn(7), FieldNoRelated(@"U_EXK_AUX3", @"Auxiliar 3", BoDbTypes.Alpha, Size = 150)]
        public string Auxiliar3 { get; set; }

        [FormColumn(8), FieldNoRelated(@"U_EXK_PLACA", @"Placa", BoDbTypes.Alpha, Size = 100)]
        public int? Placa { get; set; }

        [FormColumn(9), FieldNoRelated("U_EXK_FEINTRAS", "Inicio Traslado", BoDbTypes.Date, Size = 10)]
        public DateTime InicioTraslado { get; set; }

        [FormColumn(10), FieldNoRelated(@"U_EXK_FEFITRAS", @"Fin Traslado", BoDbTypes.Date, Size = 10)]
        public DateTime FinTraslado { get; set; }

        [FormColumn(11), FieldNoRelated(@"U_EXK_ZONDESP", @"Zonas de despacho", BoDbTypes.Alpha, Size = 254)]
        public string ZonaDespacho { get; set; }


        [FormColumn(12), FieldNoRelated(@"U_EXK_EST", @"Estado", BoDbTypes.Alpha, Size = 2,Default ="O")]
        [Val("O", "Abierto")]
        [Val("T","Terminado")]
        [Val("C", "Cancelado")]
        public string Estado { get; set; }


        [FormColumn(13), FieldNoRelated(@"U_EXK_ZONRUT", @"Zonas de la ruta", BoDbTypes.Alpha, Size = 254)]
        public string ZonaRuta { get; set; }


        [FormColumn(14), FieldNoRelated(@"U_EXK_PROG", @"Programados", BoDbTypes.Alpha, Size = 2)]
        [Val("N","No")]
        public string SupplierTransferModality { get; set; }


        [FormColumn(15), FieldNoRelated(@"U_EXK_DESDE", @"Desde", BoDbTypes.Date, Size = 10)]
        public DateTime Desde { get; set; }


        [FormColumn(16), FieldNoRelated(@"U_EXK_HASTA", @"Hasta", BoDbTypes.Date, Size = 10)]
        public DateTime Hasta { get; set; }

        [FormColumn(17), FieldNoRelated("U_EXK_CUTIL", "Carga Util", BoDbTypes.Quantity)]
        public decimal CargaUtil { get; set; }

        [FormColumn(18), FieldNoRelated(@"U_EXK_TOTCAR", @"Total Cargado", BoDbTypes.Quantity)]
        public decimal TotalCargado { get; set; }

        [FormColumn(19), FieldNoRelated(@"U_EXK_DIF", @"Diferencia", BoDbTypes.Quantity)]
        public decimal Diferencia { get; set; }

        [FormColumn(20), FieldNoRelated(@"U_EXK_BULTO", @"Cant. Bultos", BoDbTypes.Quantity)]
        public int CantBultos { get; set; }



        [ChildProperty(HGR1.ID)]
        public List<HGR1> DetalleGuias { get; set; }

        //[ChildProperty(TRD2.ID)]
        //public List<TRD2> Vehicles { get; set; }


       
    }
}
