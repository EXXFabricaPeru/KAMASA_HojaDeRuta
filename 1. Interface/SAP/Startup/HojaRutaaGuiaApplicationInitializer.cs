using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Interface.Customizations;
using Exxis.Addon.HojadeRutaAGuia.Interface.Startup.Versions;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;
using static Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities.ElementTuple<string>;
namespace Exxis.Addon.HojadeRutaAGuia.Interface.Startup
{
    [ResourceNamespace("Exxis.Addon.HojadeRutaAGuia.Interface.Resources")]
    public class HojaRutaaGuiaApplicationInitializer : ApplicationInitializer
    {
     
        private HojaRutaaGuiaApplicationInitializer(SAPbouiCOM.Framework.Application application)
            : base(MakeTuple(ProductId, ProductDescription),MakeTuple(Constants.Menu.MAIN_APPLICATION, @"EXXIS-Addon Hoja de Ruta a Guías"), application)
        {
        }

        protected override void BuildApplicationVersion(VersionCollection versionCollection)
        {
            versionCollection.Add(Version_0_0_0_1.Make);
            versionCollection.Add(Version_0_0_0_2.Make);
            versionCollection.Add(Version_0_0_0_3.Make);
            versionCollection.Add(Version_0_0_0_4.Make);
            versionCollection.Add(Version_0_0_0_5.Make);
            versionCollection.Add(Version_0_0_0_6.Make);
            versionCollection.Add(Version_0_0_0_7.Make);

        }

        protected override void OnClickBaseMenuEvent(ref MenuEvent menuEvent, out bool handleEvent)
        {
            base.OnClickBaseMenuEvent(ref menuEvent, out handleEvent);
            if (handleEvent)
                CatchEvents.OnClickMenu(menuEvent, out handleEvent);
        }

        protected override void OnReferenceEvent(string formId, ref ItemEvent itemEvent, out bool handleEvent)
        {
            CatchEvents.OnGeneralApplicationEvents(formId, itemEvent, out handleEvent);
        }

        public static HojaRutaaGuiaApplicationInitializer MakeDevelopmentInitializer()
        {
            var application = new SAPbouiCOM.Framework.Application();
            return new HojaRutaaGuiaApplicationInitializer(application);
        }

        public static HojaRutaaGuiaApplicationInitializer MakeProductionInitializer(string connectionString)
        {
            var application = new SAPbouiCOM.Framework.Application(connectionString);
            return new HojaRutaaGuiaApplicationInitializer(application);
        }
    }
}
