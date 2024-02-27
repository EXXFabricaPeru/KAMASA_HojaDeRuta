using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Query;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Startup.Versions
{
    // ReSharper disable once InconsistentNaming
    public class Version_0_0_0_3 : Versioner
    {
        public static Versioner Make => new Version_0_0_0_3();

        public Version_0_0_0_3() : base("0.0.0.3")
        {
        }

        protected override void InitializeTables()
        {
            //CreateObject(typeof(OLTR));
            //CreateObject(typeof(OMAJ));
            //CreateObject(typeof(OPCG));
            //SyncSystemTable(typeof(OACT));
            //SyncSystemTable(typeof(ORCT));
            //CreateObject(typeof(OPAP));

        }
        protected override void InitializeFormattedSearch()
        {
            //CreateFormattedSearch<ComisionTarjetasQueryManager, OPCG>(query => query.VS_LT_BUSCAPASARELA, udo => udo.Pasarela);
            //CreateFormattedSearch<ComisionTarjetasQueryManager, OPAP>(query => query.VS_LT_BUSCACUENTAPASARELA, udo => udo.Cuenta);
            //CreateFormattedSearch<ComisionTarjetasQueryManager, OPAP>(query => query.VS_LT_BUSCAMONEDA, udo => udo.Moneda);
            //CreateFormattedSearch<ComisionTarjetasQueryManager, OLTR>(query => query.VS_LT_BUSCATIENDAS, udo => udo.CodigoTienda);
            //CreateFormattedSearch<ComisionTarjetasQueryManager, OLTR>(query => query.VS_LT_CUENTA_BANCO, udo => udo.CuentaContable);

        }
    }
}