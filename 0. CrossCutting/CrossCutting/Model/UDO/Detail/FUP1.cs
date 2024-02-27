// ReSharper disable InconsistentNaming
// ReSharper disable ExplicitCallerInfoArgument
using System;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail
{
    [Serializable]
    [UserDefinedTable(ID, DESCRIPTION)]
    [UDOFatherReference(OFUP.ID, 1)]
    public class FUP1 : BaseUDO
    {
        public const string ID = "VS_PD_FUP1";
        public const string DESCRIPTION = "Detalle archivo";

        [EnhancedColumn(Visible = false), ColumnProperty("DocEntry")]
        public string DocumentEntry { get; set; }

        [EnhancedColumn(Visible = false), ColumnProperty("LineId")]
        public int LineId { get; set; }

        [EnhancedColumn(1), FieldNoRelated(@"U_EXK_SELE", @"Seleccionado", BoDbTypes.Alpha, Size = 1)]
        public string Selected { get; set; }

        [EnhancedColumn(2), FieldNoRelated(@"U_EXK_IDOR", @"Identificador", BoDbTypes.Alpha, Size = 254)]
        public string GroupedIdentifier { get; set; }

        [EnhancedColumn(3), FieldNoRelated(@"U_EXK_CCOD", @"Código SN", BoDbTypes.Alpha, Size = 254)]
        public string ShippingAddressCode { get; set; }

        [EnhancedColumn(4), FieldNoRelated(@"U_EXK_CNAM", @"Razon Social", BoDbTypes.Alpha, Size = 150)]
        public string ShippingAddressName { get; set; }

        [EnhancedColumn(5), FieldNoRelated(@"U_EXK_CADD", @"Direccion Envio", BoDbTypes.Alpha, Size = 254)]
        public string ShippingAddress { get; set; }

        [EnhancedColumn(6), FieldNoRelated(@"U_EXK_ITCD", @"Codigo Articulo", BoDbTypes.Alpha, Size = 150)]
        public string ItemCode { get; set; }

        [EnhancedColumn(7), FieldNoRelated(@"U_EXK_ITNM", @"Nombre Articulo", BoDbTypes.Alpha, Size = 254)]
        public string ItemName { get; set; }

        [EnhancedColumn(8), FieldNoRelated(@"U_EXK_ITPR", @"Precio Articulo", BoDbTypes.Price)]
        public decimal ItemPrice { get; set; }

        [EnhancedColumn(9), FieldNoRelated(@"U_EXK_ITQT", @"Cantidad Articulo", BoDbTypes.Price)]
        public decimal ItemQuantity { get; set; }

        [EnhancedColumn(10), FieldNoRelated(@"U_EXK_STAT", @"Estado", BoDbTypes.Alpha, Size = 1)]
        [Val(Statuses.PENDING_CODE, Statuses.PENDING_DESCRIPTION)]
        [Val(Statuses.ERROR_CODE, Statuses.ERROR_DESCRIPTION)]
        [Val(Statuses.SUCCESS_CODE, Statuses.SUCCESS_DESCRIPTION)]
        public string Status { get; set; }


        public string StatusDescription
        {
            get
            {
                PropertyInfo property = GenericHelper.GetPropertyForExpression<FUP1, string>(t => t.Status);
                return GenericHelper.GetDescriptionFromValue(property, Status);
            }
        }

        [EnhancedColumn(11), FieldNoRelated(@"U_EXK_ERRM", @"Mensaje Error", BoDbTypes.Alpha, Size = 254)]
        public string ErrorMessage { get; set; }

        [EnhancedColumn(12), FieldNoRelated(@"U_EXK_DETRY", @"DocEntry", BoDbTypes.Integer)]
        public int ReferenceDocumentEntry { get; set; }
        
        [EnhancedColumn(13), FieldNoRelated(@"U_EXK_DNUM", @"DocNumber", BoDbTypes.Integer)]
        public int ReferenceDocumentNumber { get; set; }

        [EnhancedColumn(14), FieldNoRelated(@"U_EXK_ITLN", @"Linea", BoDbTypes.Integer)]
        public int ItemLine { get; set; }
        
        //[EnhancedColumn(14), FieldNoRelated("U_EXK_DDAT", "Fecha Entrega", BoDbTypes.Date)]
        //public DateTime DocDueDate { get; set; }

        //[EnhancedColumn(15), FieldNoRelated("U_EXK_NDOC", "Número Documentos", BoDbTypes.Alpha, Size = 254)]
        //public string NumDocument { get; set; }

        public static class Statuses
        {
            public const string ERROR_CODE = "E";
            public const string ERROR_DESCRIPTION = "Error";

            public const string SUCCESS_CODE = "C";
            public const string SUCCESS_DESCRIPTION = "Correcto";

            public const string PENDING_CODE = "P";
            public const string PENDING_DESCRIPTION = "Pendiente";
        }
    }
}
