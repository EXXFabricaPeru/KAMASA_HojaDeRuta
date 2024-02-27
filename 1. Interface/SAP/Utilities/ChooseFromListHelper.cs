namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class ChooseFromListHelper
    {
        public static SAPbouiCOM.ChooseFromList ClearConditions(this SAPbouiCOM.ChooseFromList chooseFromList)
        {
            var conditions = ApplicationInterfaceHelper.ApplicationInstance
                .CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                .To<SAPbouiCOM.Conditions>();

            chooseFromList.SetConditions(conditions);
            return chooseFromList;
        }
    }
}