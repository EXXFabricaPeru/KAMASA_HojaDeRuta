using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using VSVersionControl.FlagElements.Attributes;
using VSVersionControl.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header
{
    [SystemTable(@"OWHS", FormType = "dummy")]
    public class OWHS : BaseSAPTable
    {
        [SAPColumn("WhsCode")]
        public string Code { get; set; }

        [SAPColumn("WhsName")]
        public string Name { get; set; }

        [SAPColumn("Street")]
        public string Street { get; set; }

        [SAPColumn("StreetNo")]
        public string StreetNumber { get; set; }

        [FieldNoRelated(@"VS_PD_LOCX", @"Latitud", BoDbTypes.Quantity)]
        public decimal Latitude { get; set; }

        [FieldNoRelated(@"VS_PD_LOCY", @"Longitud", BoDbTypes.Quantity)]
        public decimal Longitude { get; set; }

        [FieldNoRelated(@"VS_PD_PRNL", @"Principal", BoDbTypes.Alpha, Size = 1)]
        [Val("Y", "Si"), Val("N", "No")]
        public bool IsPrincipal { get; set; }

        [FieldNoRelated(@"VS_PD_VRTL", @"Virtual", BoDbTypes.Alpha, Size = 1)]
        [Val("Y", "Si"), Val("N", "No")]
        public bool IsVirtual { get; set; }

        [FieldNoRelated(@"U_EXK_RSEN", @"Restricción para la Entrega", BoDbTypes.Alpha, Size = 3)]
        public string DeliveryControl { get; set; }

        [FieldNoRelated(@"U_EXK_TEMP", @"Código Almacén Temporal", BoDbTypes.Alpha, Size = 10)]
        public string CodigoAlmacenTemporal { get; set; }
    } 
}
