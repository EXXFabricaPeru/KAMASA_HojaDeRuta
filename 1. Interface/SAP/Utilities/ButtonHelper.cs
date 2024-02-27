using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class ButtonHelper
    {
        public static void Disable(this SAPbouiCOM.Button button)
        {
            SAPbouiCOM.Item item = button.Item;
            item.Enabled = false;
            GenericHelper.ReleaseCOMObjects(item);
        }

        public static void Enable(this SAPbouiCOM.Button button)
        {
            SAPbouiCOM.Item item = button.Item;
            item.Enabled = true;
            GenericHelper.ReleaseCOMObjects(item);
        }

        public static void SetEnableStatus(this SAPbouiCOM.Button button, bool status)
        {
            SAPbouiCOM.Item item = button.Item;
            item.Enabled = status;
            GenericHelper.ReleaseCOMObjects(item);
        }

        public static bool IsEnabled(this SAPbouiCOM.Button button)
        {
            SAPbouiCOM.Item item = button.Item;
            bool itemEnabled = item.Enabled;
            GenericHelper.ReleaseCOMObjects(item);
            return itemEnabled;
        }
    }
}