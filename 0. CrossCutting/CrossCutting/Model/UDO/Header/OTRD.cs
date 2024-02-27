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
    public class OTRD: BaseUDO
    {
        public const string ID = "VS_PD_OTRD";
        public const string DESCRIPTION = "Transportista";
               public OTRD()
        {
            Drivers = new List<TRD1>();
            Vehicles = new List<TRD2>();
        }
        [FormColumn(0, Description = "Código"), ColumnProperty("Code"), FindColumn]
        public string SupplierId { get; set; }

        [FormColumn(1, Visible = false), ColumnProperty("Name"), FindColumn]
        public string Description { get; set; }

        [FormColumn(2), FieldNoRelated(@"U_EXK_COD", @"Hoja de Ruta", BoDbTypes.Alpha, Size = 254)]
        public string SupplierName { get; set; }

        [FormColumn(3), FieldNoRelated(@"U_EXK_TRANSP", @"Transportista", BoDbTypes.Alpha, Size = 1)]
        public string SupplierDocumentType { get; set; }

        [FormColumn(4), FieldNoRelated(@"U_EXK_CHOFER", @"Chofer", BoDbTypes.Alpha, Size = 20)]
        public string SupplierDocumentId { get; set; }

        [FormColumn(5), FieldNoRelated(@"U_EXK_AUX1", @"Auxiliar 1", BoDbTypes.Alpha, Size = 20)]
        public string SupplierPhone { get; set; }

        [FormColumn(6), FieldNoRelated(@"U_EXK_AUX2", @"Auxiliar 2", BoDbTypes.Alpha, Size = 150)]
        public string SupplierEmail { get; set; }

        [FormColumn(7), FieldNoRelated(@"U_EXK_AUX3", @"Auxiliar 3", BoDbTypes.Integer)]
        public int? SelectionPriority { get; set; }

        [FormColumn(7), FieldNoRelated(@"U_EXK_PLACA", @"Placa", BoDbTypes.Integer)]
        public int? Placa { get; set; }

        [FormColumn(14), FieldNoRelated("U_EXK_FEDS", "Inicio Traslado", BoDbTypes.Date, Size = 10)]
        public DateTime InicioTraslado { get; set; }

        [FormColumn(7), FieldNoRelated(@"U_EXK_PLACA", @"Fin Traslado", BoDbTypes.Date, Size = 10)]
        public DateTime FinTraslado { get; set; }

        [FormColumn(6), FieldNoRelated(@"U_EXK_ZONDESP", @"Zonas de despacho", BoDbTypes.Alpha, Size = 150)]
        public string SupplisserEmail { get; set; }


        [FormColumn(8), FieldNoRelated(@"U_EXK_TPTR", @"Estado", BoDbTypes.Alpha, Size = 2)]
        [Val(TransporterTariffRateType.FLATT, TransporterTariffRateType.FLATT_DESCRIPTION)]
        [Val(TransporterTariffRateType.DEMAND, TransporterTariffRateType.DEMAND_DESCRIPTION)]
        public string Estad { get; set; }

        [FormColumn(9), FieldNoRelated(@"U_EXK_CDTR", @"Código de Tarifario - Defecto", BoDbTypes.Alpha, Size = 254)]
        public string CodeType { get; set; }


        [FormColumn(10), FieldNoRelated(@"U_EXK_MDTR", @"Modalidad Traslado", BoDbTypes.Alpha, Size = 2)]
        [Val(TransferModality.PUBLIC, TransferModality.PUBLIC_DESCRIPTION)]
        [Val(TransferModality.PRIVATE, TransferModality.PRIVATE_DESCRIPTION)]
        public string SupplierTransferModality { get; set; }

        //[FormColumn(11), FieldNoRelated(@"U_EXK_ADTR", @"Dirección Transportista", BoDbTypes.Alpha, Size = 254)]
        //public string SupplierAddress { get; set; }

        [FormColumn(11), FieldNoRelated(@"U_EXK_MTTR", @"Registro MTC", BoDbTypes.Alpha, Size = 100)]
        public string SupplierMTC { get; set; }
    

        [ChildProperty(TRD1.ID)]
        public List<TRD1> Drivers { get; set; }

        [ChildProperty(TRD2.ID)]
        public List<TRD2> Vehicles { get; set; }
        public string RateType { get; set; }

        public static class TransferModality
        {
            public const string PRIVATE = "02";
            public const string PUBLIC = "01";
            public const string PRIVATE_DESCRIPTION = "Transporte Privado";
            public const string PUBLIC_DESCRIPTION = "Transporte Público";
        }
    }
}
