using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail;
using System;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header
{
    [SystemTable(nameof(ORDR_), FormType = "dummy")]
    public class ORDR_ : BaseSAPTable
    {
        public string Identifier { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CardCode { get; set; } = string.Empty;
        public string CardName { get; set; } = string.Empty;
        public string LicTradNum { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public string ShipToAdr { get; set; }
        public string AddressFormat { get; set; } = string.Empty;
        public List<RDR1_> Detalle { get; set; } = new List<RDR1_>();
        public Dictionary<string, string> UserFields { get; set; } = new Dictionary<string, string>();
        public string TipoSN { get; set; }
        public string TipoItem { get; set; }
        public string TipoPrice { get; set; }
    }
}
