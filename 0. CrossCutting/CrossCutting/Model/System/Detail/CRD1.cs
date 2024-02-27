using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail
{
    [SystemTable("CRD1", FormType = "dummy")]
    public class CRD1
    {
        [SAPColumn("Address")]
        public string Code { get; set; }

        [SAPColumn("AdresType")]
        public string Type { get; set; }

        [SAPColumn("Street")]
        public string Street { get; set; }

        [SAPColumn("Address2")]
        public string SecondAddress { get; set; }

        [FieldNoRelated("U_EXK_GEOLOCX", "Latitud", BoDbTypes.Quantity)]
        public decimal Latitude { get; set; }

        [FieldNoRelated("U_EXK_GEOLOCY", "Longitud", BoDbTypes.Quantity)]
        public decimal Longitude { get; set; }

        [FieldNoRelated("U_EXK_CREN", "Código Restricción Entrega", BoDbTypes.Alpha, Size = 3)]
        public string DeliveryControl { get; set; }

        [FieldNoRelated("U_EXK_VTHR", "Ventana Horaria", BoDbTypes.Alpha, Size = 50)]
        public string DispatchTimeWindow { get; set; }

        [FieldNoRelated("U_EXK_DLCD", "Dirección Archivo EDI", BoDbTypes.Alpha, Size = 50)]
        public string DeliveryCode { get; set; }

        [SAPColumn("County")]
        public string Distrito { get; set; }

        [SAPColumn("State")]
        public string Departamento { get; set; }

        [SAPColumn("Country")]
        public string Pais { get; set; }

        [SAPColumn("City")]
        public string Provincia { get; set; }

        [SAPColumn("GlblLocNum")]
        public string Ubigeo { get; set; }


        public static class AddressType
        {
            public const string SHIPPING = "S";
            public const string BILLING = "B";

        }
    }
}