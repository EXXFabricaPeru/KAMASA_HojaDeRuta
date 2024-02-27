using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Query
{
    [ResourceNamespace("Exxis.Addon.HojadeRutaAGuia.Interface.Resources")]
    public class ComisionTarjetasQueryManager : SAPQueryManager
    {
        public ComisionTarjetasQueryManager(QueryType queryType) : base("BPVS - Hoja de Ruta a Guia", queryType)
        {
        }

        public virtual ElementTuple<string> VS_LT_BUSCAPASARELA { get; set; }
        public virtual ElementTuple<string> VS_LT_BUSCACUENTAPASARELA {get; set; }
        public virtual ElementTuple<string> VS_LT_BUSCAMONEDA { get; set; }

        public virtual ElementTuple<string> VS_LT_BUSCATIENDAS { get; set; }
        public virtual ElementTuple<string> VS_LT_CUENTA_BANCO { get; set; }

        public virtual ElementTuple<string> VS_LT_BUSCACODDOCUMENTO { get; set; }
        public virtual ElementTuple<string> VS_LT_MOTIVO_AJUSTE { get;  set; }
    }
}