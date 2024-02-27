using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine
{
    public class RIN1 : SAPDocumentLine
    {
        public int DocumentTypeReferenced { get; set; }
        public int DocumentEntryReferenced { get; set; }
        public int LineNumberReferenced { get; set; }
    }
}