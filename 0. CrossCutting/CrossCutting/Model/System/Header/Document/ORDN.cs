// ReSharper disable InconsistentNaming

using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oReturns)]
    public class ORDN : SAPDocument<RDN1>
    {
        public object MotivoTrasladoSoraya { get; set; }
        public object TipoDocumento { get; set; }
    }
}