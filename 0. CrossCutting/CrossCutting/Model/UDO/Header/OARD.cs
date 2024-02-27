using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Newtonsoft.Json;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_Document, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = false, CanManageSeries = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OARD : BaseUDO
    {
        public const string ID = "VS_PD_OARD";
        public const string DESCRIPTION = "Rutas";

        [FormColumn(0, Description = "Código"), ColumnProperty("DocEntry"), FindColumn]
        public int DocumentEntry { get; set; }

        [FormColumn(1, Description = "Número"), ColumnProperty("DocNum"), FindColumn]
        public int DocumentNumber { get; set; }

        [FormColumn(2, Description = "Serie"), ColumnProperty("Series")]
        public int Series { get; set; }

        [FormColumn(3), FieldNoRelated("U_EXK_FEDS", "Fecha de Distribución", BoDbTypes.Date), FindColumn]
        public DateTime? DistributionDate { get; set; }

        [FormColumn(4), FieldNoRelated("U_EXK_HRIN", "Hora de Inicio", BoDbTypes.Hour)]
        public DateTime? StartHour { get; set; }

        [FormColumn(5), FieldNoRelated("U_EXK_HRFN", "Hora de Fin", BoDbTypes.Hour)]
        public DateTime? EndHour { get; set; }

        [FormColumn(6), FieldNoRelated("U_EXK_PTOT", "Peso Total", BoDbTypes.Price)]
        public double? TotalWeight { get; set; }

        [FormColumn(7), FieldNoRelated("U_EXK_VTOT", "Volumen Total", BoDbTypes.Price)]
        public double? TotalVolume { get; set; }

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

        [FormColumn(14), FieldNoRelated("U_EXK_STAD", "Estado", BoDbTypes.Alpha, Size = 2, Default = Status.PENDING)]
        [Val(Status.FINISHED, Status.FINISHED_DESCRIPTION)]
        [Val(Status.UNFINISHED, Status.UNFINISHED_DESCRIPTION)]
        [Val(Status.WITH_OBSERVATION, Status.WITH_OBSERVATION_DESCRIPTION)]
        [Val(Status.IN_PROGRESS, Status.IN_PROGRESS_DESCRIPTION)]
        [Val(Status.PROGRAMED, Status.PROGRAMED_DESCRIPTION)]
        [Val(Status.PENDING, Status.PENDING_DESCRIPTION)]
        [Val(Status.LIQUIDATE, Status.LIQUIDATE_DESCRIPTION)]
        [Val(Status.CLOSED, Status.CLOSED_DESCRIPTION)]
        public string RouteStatus { get; set; }

        [JsonIgnore]
        public string RouteStatusDescription
        {
            get
            {
                var property = GenericHelper.GetPropertyForExpression<OARD, string>(t => t.RouteStatus);
                return GenericHelper.GetDescriptionFromValue(property, RouteStatus);
            }
        }

        [FormColumn(15), FieldNoRelated("U_EXK_FJOR", "Flujo de Origen", BoDbTypes.Alpha, Size = 2)]
        [Val(DistributionFlow.ITEM_SALE, DistributionFlow.ITEM_SALE_DESCRIPTION)]
        [Val(DistributionFlow.SERVICE_SALE, DistributionFlow.SERVICE_SALE_DESCRIPTION)]
        [Val(DistributionFlow.INVENTORY_TRANSFER, DistributionFlow.INVENTORY_TRANSFER_DESCRIPTION)]
        [Val(DistributionFlow.RETURN, DistributionFlow.RETURN_DESCRIPTION)]
        [Val(DistributionFlow.PURCHASE, DistributionFlow.PURCHASE_DESCRIPTION)]
        public string OriginFlow { get; set; }

        [JsonIgnore]
        public string OriginFlowDescription
        {
            get
            {
                var property = GenericHelper.GetPropertyForExpression<OARD, string>(t => t.OriginFlow);
                return GenericHelper.GetDescriptionFromValue(property, OriginFlow);
            }
        }

        [FormColumn(16), FieldNoRelated("U_EXK_RFPE", "Código Liquidación Referenciada", BoDbTypes.Integer)]
        public int ReferencePaymentEntry { get; set; }

        [FormColumn(17), FieldNoRelated("U_EXK_RFPN", "Número Liquidación Referenciada", BoDbTypes.Integer)]
        public int ReferencePaymentNumber { get; set; }

        [FormColumn(18),
         FieldNoRelated("U_EXK_STBT", "Estado Sincronización Beetrack", BoDbTypes.Alpha, Size = 2, Default = StatusBeetrack.NO_SYNC)]
        [Val(StatusBeetrack.NO_SYNC, StatusBeetrack.NO_SYNC_DESCRIPTION)]
        [Val(StatusBeetrack.SYNC, StatusBeetrack.SYNC_DESCRIPTION)]
        public string BeetrackStatus { get; set; }

        [FormColumn(19), FieldNoRelated("U_EXK_IDBT", "Código Beetrack", BoDbTypes.Alpha, Size = 50)]
        public string BeetrackId { get; set; }

        [FormColumn(20), FieldNoRelated("U_EXK_USMV", "Usuario Móvil", BoDbTypes.Alpha, Size = 50)]
        public string UserMobile { get; set; }

        [FormColumn(21), FieldNoRelated("U_EXK_NMCR", "Contador", BoDbTypes.Integer)]
        public int ReferenceCounter { get; set; }

        [FormColumn(22), FieldNoRelated("U_EXK_CDRF", "Comentario", BoDbTypes.Alpha, Size = 150)]
        public string CodigoTemporalReferencia { get; set; }

        [FormColumn(23), FieldNoRelated(@"U_EXK_TPTR", @"Tarifario Referenciado", BoDbTypes.Alpha, Size = 2)]
        [Val(TransporterTariffRateType.FLATT, TransporterTariffRateType.FLATT_DESCRIPTION)]
        [Val(TransporterTariffRateType.DEMAND, TransporterTariffRateType.DEMAND_DESCRIPTION)]
        public string RateType { get; set; }

        [FormColumn(24), FieldNoRelated(@"U_EXK_CDTR", @"Código/Puntos Referencia", BoDbTypes.Alpha, Size = 254)]
        public string CodeType { get; set; }

        [FormColumn(25), FieldNoRelated(@"U_EXK_CDAX", @"Código Auxiliar", BoDbTypes.Alpha, Size = 254)]
        public string AuxiliarCode { get; set; }

        [FormColumn(26), FieldNoRelated(@"U_EXK_RTID", @"Código Ruta Referencial", BoDbTypes.Alpha, Size = 50)]
        public string RouteId { get; set; }

        [FormColumn(27), FieldNoRelated(@"U_EXK_CDA2", @"Código Auxiliar Extra", BoDbTypes.Alpha, Size = 254)]
        public string AuxiliarCodeExtra { get; set; }

        [FormColumn(28), FieldNoRelated("U_EXK_STAP", "Estado Impresión Hoja Picking", BoDbTypes.Alpha, Size = 2, Default = "N")]
        [Val("Y", "Si")]
        [Val("N", "No")]
        public string StatusPrintPickingSheet { get; set; }

        [FormColumn(29), FieldNoRelated("U_EXK_STAR", "Estado Impresión Reporte Picking", BoDbTypes.Alpha, Size = 2, Default = "N")]
        [Val("Y", "Si")]
        [Val("N", "No")]
        public string StatusPrintPickingReport { get; set; }

        [FormColumn(30), FieldNoRelated("U_EXK_PRCG", "Prioridad de Carga", BoDbTypes.Integer)]
        public int PrioridadCarga { get; set; }

        [ChildProperty(ARD1.ID)] public List<ARD1> RelatedTransferOrders { get; set; }

        [ChildProperty(ARD2.ID)] public List<ARD2> RelatedMarketingDocuments { get; set; }

        [ChildProperty(ARD3.ID)] public List<ARD3> RelatedDeliveredArticles { get; set; }

        [ChildProperty(ARD4.ID)] public List<ARD4> RelatedTransshippingDocuments { get; set; }

        public static string RetrieveSAPName<T>(Expression<Func<OARD, T>> expression)
        {
            PropertyInfo property = GenericHelper.GetPropertyForExpression(expression);
            var fieldNoRelated = property.GetCustomAttribute<FieldNoRelated>();
            if (fieldNoRelated != null)
                return fieldNoRelated.sAliasID;

            var columnProperty = property.GetCustomAttribute<ColumnProperty>();
            if (columnProperty != null)
                return columnProperty.ColumnName;

            throw new Exception();
        }

        public static class Status
        {
            public const string FINISHED = "FI";
            public const string FINISHED_DESCRIPTION = "Finalizado Correcto";
            public const string UNFINISHED = "UF";
            public const string UNFINISHED_DESCRIPTION = "Sin Finalizar";
            public const string WITH_OBSERVATION = "WO";
            public const string WITH_OBSERVATION_DESCRIPTION = "Finalizado Observado";
            public const string IN_PROGRESS = "IP";
            public const string IN_PROGRESS_DESCRIPTION = "En Ejecución";
            public const string PROGRAMED = "PR";
            public const string PROGRAMED_DESCRIPTION = "Programado";
            public const string PENDING = "PE";
            public const string PENDING_DESCRIPTION = "Pendiente";
            public const string LIQUIDATE = "LI";
            public const string LIQUIDATE_DESCRIPTION = "Liquidado";
            public const string CLOSED = "CL";
            public const string CLOSED_DESCRIPTION = "Cerrado";
        }

        public static class StatusBeetrack
        {
            public const string NO_SYNC = "NS";
            public const string NO_SYNC_DESCRIPTION = "No Sincronizado";
            public const string SYNC = "SB";
            public const string SYNC_DESCRIPTION = "Sincronizado";
        }
    }
}