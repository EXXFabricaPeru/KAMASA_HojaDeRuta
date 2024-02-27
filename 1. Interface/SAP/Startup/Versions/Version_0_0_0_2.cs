using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Startup.Versions
{
    // ReSharper disable once InconsistentNaming
    public class Version_0_0_0_2 : Versioner
    {
        public static Versioner Make => new Version_0_0_0_2();

        private Version_0_0_0_2() : base("0.0.0.2")
        {
          
        }

        protected override void InitializeTables()
        {
            //CreateObject(typeof(OHOJ));
            CreateObject(typeof(OHRG));
        }
    }
}