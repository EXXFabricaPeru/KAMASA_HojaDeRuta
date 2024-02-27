using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oPurchaseDeliveryNotes)]
    public class OPDN : SAPDocument<PDN1>
    {
        public object Naturaleza { get; set; }
        public object TipoDocumento { get; set; }
        public object SerieDocumento { get; set; }
        public object CorrelativoDocumento { get; set; }
    }
}