using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class CheckboxHelper
    {
        public static SAPbouiCOM.CheckBox Check(this SAPbouiCOM.CheckBox checkBox)
        {
            if (!checkBox.Checked)
                checkBox.Checked = true;
            return checkBox;
        }

        public static SAPbouiCOM.CheckBox UnCheck(this SAPbouiCOM.CheckBox checkBox)
        {
            if (checkBox.Checked)
                checkBox.Checked = false;
            return checkBox;
        }

        public static SAPbouiCOM.CheckBox SetEnableStatus(this SAPbouiCOM.CheckBox checkBox, bool status)
        {
            SAPbouiCOM.Item item = checkBox.Item;
            item.Enabled = status;
            GenericHelper.ReleaseCOMObjects(item);
            return checkBox;
        }

        public static SAPbouiCOM.CheckBox UnAffect(this SAPbouiCOM.CheckBox checkBox)
        {
            SAPbouiCOM.Item item = checkBox.Item;
            item.AffectsFormMode = false;
            GenericHelper.ReleaseCOMObjects(item);
            return checkBox;
        }

        public static SAPbouiCOM.CheckBox Disable(this SAPbouiCOM.CheckBox checkBox)
        {
            return SetEnableStatus(checkBox, false);
        }

        public static SAPbouiCOM.CheckBox BindUserDataSource(this SAPbouiCOM.CheckBox checkBox, SAPbouiCOM.UserDataSource userDataSource)
        {
            return BindUserDataSource(checkBox, userDataSource.UID);
        }

        public static SAPbouiCOM.CheckBox BindUserDataSource(this SAPbouiCOM.CheckBox checkBox, string userDataSourceId)
        {
            SAPbouiCOM.DataBind dataBind = checkBox.DataBind;
            dataBind.SetBound(true, string.Empty, userDataSourceId);
            GenericHelper.ReleaseCOMObjects(dataBind);
            return checkBox;
        }
    }
}