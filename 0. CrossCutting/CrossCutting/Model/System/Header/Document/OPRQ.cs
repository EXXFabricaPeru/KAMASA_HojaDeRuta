// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document
{
    [Serializable]
    [SAPObject(BoObjectTypes.oPurchaseRequest)]
    public class OPRQ : SAPDocument<PRQ1>
    {
        
    }
}