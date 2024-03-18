using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Interface.Views;
//using Exxis.Addon.HojadeRutaAGuia.Interface.Views.Modal;
using Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserObjectViews;

// ReSharper disable InconsistentNaming

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class LocalStorage
    {
        public static string FRM_DET_ORTR_TRANSFER_ORDER_ID { get; set; }



        public static Tuple<int, int> STORE_DOCUMENT_VALIDATING { get; set; }
        public static Tuple<int, string> STORE_DOCUMENT_VALIDATING_STATUS { get; set; }
        public static Tuple<int, string> STORE_DOCUMENT_VALIDATING_REASON { get; set; }

        public static int STORE_ORTR_DOCUMENT_INCLUDE { get; set; }

        //public static TransferOrderValidate FORM_ORTR_DOCUMENT_INCLUDE { get; set; }

        //public static DocumentValidating FORM_DOCUMENT_VALIDATING { get; set; }

        //public static TransferOrderValidating FORM_ORTR_VALIDATION { get; set; }

        //public static OARD STORE_OARD_CLS_ROTE { get; set; }

        ////public static UpdateClosingRoute FORM_CLS_RT_2 { get; set; }

        //public static RouteForm FORM_ROUTE_UDO { get; set; }

        //public static RouteFormModal MODAL_ROUTE_UDO { get; set; }
    }
}