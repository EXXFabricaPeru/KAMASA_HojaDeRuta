using System;
using System.Collections.Generic;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_Document, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = true, CanManageSeries = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OARP : BaseUDO
    {
        public const string ID = "VS_PD_OARD";
        public const string DESCRIPTION = "Rutas";

        [FormColumn(0, Description = "Código"), ColumnProperty("DocEntry"), FindColumn]
        public int DocumentEntry { get; set; }

        [FormColumn(1, Description = "Número"), ColumnProperty("DocNum")]
        public int DocumentNumber { get; set; }

        [FormColumn(2, Description = "Serie"), ColumnProperty("Series")]
        public int Series { get; set; }

        [FormColumn(3), FieldNoRelated("U_EXK_FEDS", "Fecha de Distribución", BoDbTypes.Date)]
        public DateTime? DistributionDate { get; set; }

        [FormColumn(4), FieldNoRelated("U_EXK_HRIN", "Hora de Inicio", BoDbTypes.Hour)]
        public DateTime? StartHour { get; set; }

        [FormColumn(5), FieldNoRelated("U_EXK_HRFN", "Hora de Fin", BoDbTypes.Hour)]
        public DateTime? EndHour { get; set; }

        [FormColumn(6), FieldNoRelated("U_EXK_PTOT", "Peso Total", BoDbTypes.Price)]
        public double TotalWeight { get; set; }

        [FormColumn(7), FieldNoRelated("U_EXK_VTOT", "Volumen Total", BoDbTypes.Price)]
        public double TotalVolume { get; set; }

        [FormColumn(8), FieldNoRelated("U_EXK_CDPR", "Código Transportista", BoDbTypes.Alpha, Size = 50)]
        public string SupplierId { get; set; }

        [FormColumn(9), FieldNoRelated("U_EXK_DSPR", "Razón Social Transportista", BoDbTypes.Alpha, Size = 254)]
        public string SupplierName { get; set; }

        [FormColumn(10), FieldNoRelated("U_EXK_CDCD", "Código Conductor", BoDbTypes.Alpha, Size = 50)]
        public string DriverId { get; set; }

        [FormColumn(11), FieldNoRelated("U_EXK_NMCD", "Nombre Conductor", BoDbTypes.Alpha, Size = 254)]
        public string DriverName { get; set; }

        [FormColumn(12), FieldNoRelated("U_EXK_CDVH", "Código Vehículo", BoDbTypes.Alpha, Size = 50)]
        public string VehicleId { get; set; }

        [FormColumn(13), FieldNoRelated("U_EXK_PLVH", "Placa Vehículo", BoDbTypes.Alpha, Size = 50)]
        public string VehicleLisencePlate { get; set; }

        [FormColumn(14), FieldNoRelated("U_EXK_STAD", "Estado", BoDbTypes.Alpha, Size = 2)]
        [Val("FI", "Finalizado Correcto"), Val("UF", "Sin Finalizar"), Val("WO", "Finalizado Observado"), Val("IP", "En Ejecución")]
        public string RouteStatus { get; set; }

        public string RouteStatusDescription
        {
            get
            {
                var property = GenericHelper.GetPropertyForExpression<OARD, string>(t => t.RouteStatus);
                return GenericHelper.GetDescriptionFromValue(property, RouteStatus);
            }
        }

        [FormColumn(15), FieldNoRelated("U_EXK_FJOR", "Flujo de Origen", BoDbTypes.Alpha, Size = 2)]
        [Val("SA", "Flujo de Venta"), Val("SE", "Flujo de Servicio"), Val("TR", "Flujo de Traslado"), Val("RE", "Flujo de Devolución")]
        public string OriginFlow { get; set; }

        public string OriginFlowDescription
        {
            get
            {
                var property = GenericHelper.GetPropertyForExpression<OARD, string>(t => t.OriginFlow);
                return GenericHelper.GetDescriptionFromValue(property, OriginFlow);
            }
        }

        [ChildProperty(ARD1.ID)]
        public List<ARD1> RelatedTransferOrders { get; set; }

        [ChildProperty(ARD2.ID)]
        public List<ARD2> RelatedMarketingDocuments { get; set; }
        
        public static class Status
        {
            public const string FINISHED = "FI";
            public const string UNFINISHED = "UF";
            public const string WITH_OBSERVATION = "WO";
            public const string IN_PROGRESS = "IP";
        }
    }
}

