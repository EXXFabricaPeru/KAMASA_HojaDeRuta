// ReSharper disable InconsistentNaming
using System;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine
{
    [Serializable]
    public class PRQ1 : SAPDocumentLine
    {
        public string ProviderCode { get; set; }
    }
}