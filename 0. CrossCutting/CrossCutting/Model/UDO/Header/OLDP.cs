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
    [UDOServices(FindServices.DefinedFields, CanDelete = true, CanCancel = true, CanClose = true)]
    [DefaultForm(SAPbobsCOM.BoYesNoEnum.tYES)]
    public class OLDP : BaseUDO
    {
        public const string ID = "VS_PD_OLDP";
        public const string DESCRIPTION = "Liquidación de Transportistas";

        [FormColumn(0, Description = "Código"), ColumnProperty("DocEntry"), FindColumn]
        public int DocumentEntry { get; set; }

        [FormColumn(1, Description = "Número"), ColumnProperty("DocNum")]
        public int DocumentNumber { get; set; }

        [FormColumn(2, Description = "Estado"), ColumnProperty("Status")]
        public string DocumentStatus { get; set; }

        [FormColumn(3), FieldNoRelated("U_EXK_CDPR", "Código Transportista", BoDbTypes.Alpha, Size = 50), FindColumn]
        public string SupplierId { get; set; }

        [FormColumn(4), FieldNoRelated("U_EXK_DSPR", "Razón Social Transportista", BoDbTypes.Alpha, Size = 254)]
        public string SupplierName { get; set; }

        [FormColumn(5), FieldNoRelated("U_EXK_FELQ", "Fecha de Liquidación", BoDbTypes.Date)]
        public DateTime? DistributionDate { get; set; }

        [FormColumn(6), FieldNoRelated("U_EXK_MTOT", "Monto Total Liquidar", BoDbTypes.Price)]
        public decimal TotalAmount { get; set; }
        
        [FormColumn(7), FieldNoRelated("U_EXK_STAD", "Estado", BoDbTypes.Alpha, Size = 2, Default = Status.PENDING)]
        [Val(Status.PENDING, Status.PENDING_DESCRIPTION)]
        [Val(Status.CLOSED, Status.CLOSED_DESCRIPTION)]
        public string PaymentStatus { get; set; }

        [FormColumn(8), FieldNoRelated("U_EXK_FTTP", "Factura Transportista", BoDbTypes.Alpha, Size = 30)]
        public string TransporterInvoice { get; set; }

        [FormColumn(9), FieldNoRelated("U_EXK_GTTP", "Guías Transportista", BoDbTypes.Alpha, Size = 254)]
        public string TransporterDelivery { get; set; }

        public string RouteStatusDescription
        {
            get
            {
                var property = GenericHelper.GetPropertyForExpression<OLDP, string>(t => t.PaymentStatus);
                return GenericHelper.GetDescriptionFromValue(property, PaymentStatus);
            }
        }

        [ChildProperty(LDP1.ID)] public List<LDP1> RelatedRoutes { get; set; }

        [ChildProperty(LDP2.ID)] public List<LDP2> RelatedMotives { get; set; }
        
        public static class Status
        {
            public const string PENDING = "PE";
            public const string PENDING_DESCRIPTION = "Pendiente";
            public const string CLOSED = "CL";
            public const string CLOSED_DESCRIPTION = "Cerrado";

        }
    }
}