// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable IdentifierTypo

using DisposableSAPBO.RuntimeMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [Udo(ID, SAPbobsCOM.BoUDOObjType.boud_Document, Description = DESCRIPTION)]
    [UDOServices(FindServices.DefinedFields, CanDelete = false, CanManageSeries = true,CanCancel =true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class ORTR : BaseUDO
    {
        public const string ID = "VS_PD_ORTR";
        public const string DESCRIPTION = "Orden de Traslado";

        public ORTR()
        {
            RelatedSAPDocuments = new List<RTR1>();
            RelatedSAPDocumentLines = new List<RTR2>();
            RelatedBatchLines = new List<RTR3>();
        }

        [FormColumn(0, Description = "Código"), ColumnProperty("DocEntry"), FindColumn]
        public int DocumentEntry { get; set; }

        public int TempDocumentEntry { get; set; }

        [FormColumn(1, Description = "Número"), ColumnProperty("DocNum"), FindColumn]
        public int DocumentNumber { get; set; }

        [FormColumn(2, Description = "Serie"), ColumnProperty("Series")]
        public int Serie { get; set; }

        [FormColumn(3, Description = "Fecha de Creación"), ColumnProperty("CreateDate")]
        public DateTime Create { get; set; }
        
        [FormColumn(4), FieldNoRelated("U_EXK_COCL", "Código del Cliente", BoDbTypes.Alpha, Size = 50)]
        public string COCL { get; set; }

        [FormColumn(5), FieldNoRelated("U_EXK_DSCL", "Descripción del Cliente", BoDbTypes.Alpha, Size = 100)]
        public string DSCL { get; set; }

        [FormColumn(6), FieldNoRelated("U_EXK_COEN", "Código de Entrega", BoDbTypes.Alpha, Size = 50)]
        public string COEN { get; set; }

        [FormColumn(7), FieldNoRelated("U_EXK_DREN", "Punto de Entrega", BoDbTypes.Alpha, Size = 254)]
        public string DREN { get; set; }

        [FormColumn(8), FieldNoRelated("U_EXK_COSA", "Código de Partida", BoDbTypes.Alpha, Size = 50)]
        public string COSA { get; set; }

        [FormColumn(9), FieldNoRelated("U_EXK_DRSA", "Punto de Partida", BoDbTypes.Alpha, Size = 254)]
        public string DRSA { get; set; }

        [FormColumn(10), FieldNoRelated("U_EXK_FJOR", "Flujo de Origen", BoDbTypes.Alpha, Size = 2)]
        [Val(DistributionFlow.ITEM_SALE, DistributionFlow.ITEM_SALE_DESCRIPTION)]
        [Val(DistributionFlow.SERVICE_SALE, DistributionFlow.SERVICE_SALE_DESCRIPTION)]
        [Val(DistributionFlow.INVENTORY_TRANSFER, DistributionFlow.INVENTORY_TRANSFER_DESCRIPTION)]
        [Val(DistributionFlow.RETURN, DistributionFlow.RETURN_DESCRIPTION)]
        [Val(DistributionFlow.PURCHASE, DistributionFlow.PURCHASE_DESCRIPTION)]
        public string FJOR { get; set; }

        public string FlowDescription
        {
            get
            {
                PropertyInfo property = GenericHelper.GetPropertyForExpression<ORTR, string>(t => t.FJOR);
                return GenericHelper.GetDescriptionFromValue(property, FJOR);
            }
        }

        [SAPColumn("U_EXK_STAD", false)]
        [FormColumn(11), FieldNoRelated("U_EXK_STAD", "Estado", BoDbTypes.Alpha, Size = 2)]
        [Val("AP", "Aprobado"), Val("DI", "Desaprobado"), Val("O", "Abierto"), Val("PS", "Pospuesto"), Val("PL", "Planeado"), Val("CN", "Confirmado"),
         Val("PR", "Impreso"), Val("CL", "Cerrado"), Val("MA", @"Aprobado manualmente"), Val("MD", @"Desaprobado manualmente")]
        public string STAD { get; set; }

        public string StatusDescription
        {
            get
            {
                PropertyInfo property = GenericHelper.GetPropertyForExpression<ORTR, string>(t => t.STAD);
                return GenericHelper.GetDescriptionFromValue(property, STAD);
            }
        }

        [FormColumn(12), FieldNoRelated("U_EXK_STMT", "Motivo de Estado", BoDbTypes.Alpha, Size = 10)]
        public string STMT { get; set; }

        public string ReasonDescription { get; set; }

        [FormColumn(13), FieldNoRelated("U_EXK_STDS", "Descripción del Motivo", BoDbTypes.Text, Size = 254)]
        public string STDS { get; set; }

        [FormColumn(14), FieldNoRelated("U_EXK_FEDS", "Fecha de Distribución", BoDbTypes.Date, Size = 10)]
        public DateTime FEDS { get; set; }

        [FormColumn(15), FieldNoRelated("U_EXK_HRDS", "Hora de Distribución", BoDbTypes.Hour, Size = 10)]
        public int? HRDS { get; set; }

        [FormColumn(16), FieldNoRelated("U_EXK_CNDC", "Cantidad de Documentos", BoDbTypes.Integer)]
        public int CNDC { get; set; }

        [FormColumn(17), FieldNoRelated("U_EXK_CNAR", "Cantidad de Artículos", BoDbTypes.Price)]
        public decimal CNAR { get; set; }

        [FormColumn(18), FieldNoRelated("U_EXK_PTOT", "Peso Total - Planificado", BoDbTypes.Price)]
        public decimal PTOT { get; set; }

        [FormColumn(19), FieldNoRelated("U_EXK_PPTO", "Peso Total - Solicitado", BoDbTypes.Price)]
        public decimal PPTO { get; set; }

        [FormColumn(20), FieldNoRelated("U_EXK_VTOT", "Volumen Total", BoDbTypes.Price)]
        public decimal VTOT { get; set; }

        [FormColumn(21), FieldNoRelated("U_EXK_MTOT", "Monto Total", BoDbTypes.Price)]
        public decimal MTOT { get; set; }

        [FormColumn(22), FieldNoRelated("U_EXK_PRDS", "Prioridad de Despacho", BoDbTypes.Alpha, Size = 2)]
        public string PRDS { get; set; }

        [FormColumn(23), FieldNoRelated("U_EXK_TPCR", "Tipo de Carga", BoDbTypes.Alpha)]
        public string TPCR { get; set; }

        [FormColumn(24), FieldNoRelated("U_EXK_CLVT", "Canal de Venta", BoDbTypes.Alpha)]
        public string CLVT { get; set; }

        [FormColumn(25), FieldNoRelated("U_EXK_OBJT", "Tipo de objeto", BoDbTypes.Integer, Size = 11)]
        public int OBJT { get; set; }

        [FormColumn(26), FieldNoRelated("U_EXK_CVRF", "Cotización de Venta de Ref.", BoDbTypes.Integer)]
        public int CVRF { get; set; }

        [FormColumn(27), FieldNoRelated("U_EXK_ODCV", "Orden de Distribución de CV Ref.", BoDbTypes.Integer)]
        public int ODCV { get; set; }

        [FormColumn(28), FieldNoRelated("U_EXK_RSEN", "Restricción para la Entrega", BoDbTypes.Alpha, Size = 3)]
        public string RSEN { get; set; }

        [FormColumn(29), FieldNoRelated("U_EXK_TMDS", "Tiempo de Despacho", BoDbTypes.Integer)]
        public int? TMDS { get; set; }

        [FormColumn(30), FieldNoRelated("U_EXK_VTHR", "Ventana Horaria de Despacho", BoDbTypes.Alpha, Size = 50)]
        public string VTHR { get; set; }

        [FormColumn(31), FieldNoRelated("U_EXK_PRTO", "Peso Total Refrigerado", BoDbTypes.Price)]
        public decimal PRTO { get; set; }

        [FormColumn(32), FieldNoRelated("U_EXK_VRTO", "Volumen Total Refrigerado", BoDbTypes.Price)]
        public decimal VRTO { get; set; }

        [FormColumn(33), FieldNoRelated("U_EXK_TPDS", "Tipo de Distribución", BoDbTypes.Alpha, Size = 3)]
        public string TPDS { get; set; }

        [FormColumn(34), FieldNoRelated("U_EXK_VARE", "Recojo en Almacén", BoDbTypes.Alpha, Size = 1, Default = "N")]
        [Val("Y", "SI"), Val("N", "NO")]
        public string isPickup { get; set; }

        [FormColumn(35), FieldNoRelated("U_EXK_PTOP", "Peso Total Previo", BoDbTypes.Price)]
        public decimal PTOP { get; set; }

        [FormColumn(36, Description = "Canceled"), ColumnProperty("Canceled")]
        public string Canceled { get; set; }

        [FormColumn(37, Description = "Status"), ColumnProperty("Status")]
        public string _status { get; set; }

        [FormColumn(38), FieldNoRelated("U_EXK_DTOR", "Código Orden de Traslado Referencia", BoDbTypes.Integer)]
        public int? DocEntryTransferOrderReference { get; set; }


        public bool isFR { get; set; }

        public string validarReprogrmado
        {
            get
            {
                if (DocEntryTransferOrderReference == null)
                    return "No";
                else
                    return "Si";
            }
        }
         
        public string StatusPrevious { get; set; }

        public string StatusPreviousDescription
        {
            get
            {
                PropertyInfo property = GenericHelper.GetPropertyForExpression<ORTR, string>(t => t.STAD);
                return GenericHelper.GetDescriptionFromValue(property, StatusPrevious);
            }
        }

        public bool IsFullyOpenned => STAD == "O" && (PTOP == decimal.Zero || PTOT == PTOP);

        [ChildProperty(RTR1.ID)] public List<RTR1> RelatedSAPDocuments { get; set; }

        [ChildProperty(RTR2.ID)] public List<RTR2> RelatedSAPDocumentLines { get; set; }

        [ChildProperty(RTR3.ID)] public List<RTR3> RelatedBatchLines { get; set; }

        public static class Status
        {
            public const string APPROVED = "AP";
            public const string DISAPPROVED = "DI";
            public const string POSTPONED = "PS";
            public const string OPEN = "O";
            public const string OPEN_DESCRIPTION = "Abierto"; 
            public const string CLOSED = "CL";
            public const string PLANNING = "PL";
            public const string CONFIRMED = "CN";
            public const string PRINTED = "PR";
            public const string APPROVED_MANUAL = "MA";
            public const string DISAPPROVED_MANUAL = "MD";
        }
    }
}