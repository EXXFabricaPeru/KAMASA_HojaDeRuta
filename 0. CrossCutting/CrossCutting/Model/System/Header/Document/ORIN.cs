using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oCreditNotes)]
    public class ORIN : SAPDocument<RIN1>
    {
        public int ReferenceInvoiceDocumentEntry { get; set; }

        public bool IsInvoiceOrigin => ReferenceInvoiceDocumentEntry != default(int);
    }
}