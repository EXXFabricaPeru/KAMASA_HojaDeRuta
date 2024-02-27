using SAPbouiCOM;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class ComboBoxHelper
    {
        public static void RemoveValidValues(this ComboBox comboBox)
        {
            var validValues = comboBox.ValidValues;
            while (validValues.Count > 0)
                validValues.Remove(0, BoSearchKey.psk_Index);
        }

        public static void RemoveValidValues(this ValidValues validValues)
        {
            while (validValues.Count > 0)
                validValues.Remove(0, BoSearchKey.psk_Index);
        }

        public static ComboBox SelectByValue(this ComboBox comboBox, string value)
        {
            comboBox.Select(value, BoSearchKey.psk_ByValue);
            return comboBox;
        }

        public static ComboBox SelectByValue(this ComboBox comboBox, int value)
        {
            comboBox.Select(value.ToString(), BoSearchKey.psk_ByValue);
            return comboBox;
        }
    }
}