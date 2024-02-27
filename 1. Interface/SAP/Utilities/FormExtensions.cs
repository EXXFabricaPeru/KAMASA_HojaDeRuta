using SAPbouiCOM.Framework;
using System;
using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Interface.Code.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class FormHelper
    {
        public static SAPFormAttribute GetSAPFormAttribute(this UserFormBase @base)
        {
            var formAttribute = @base.GetCustomAttribute<SAPFormAttribute>();
            if (formAttribute == null)
            {
            }

            return formAttribute;
        }

        public static SAPbouiCOM.Button RetrieveSpecificButton(this SAPbouiCOM.Item item)
        {
            return item.Specific.To<SAPbouiCOM.Button>();
        }

        public static SAPbouiCOM.Matrix RetrieveSpecificMatrix(this SAPbouiCOM.Item item)
        {
            return item.Specific.To<SAPbouiCOM.Matrix>();
        }

        //SAPbouiCOM.Matrix

        public static T GetDomain<T>() where T : BaseDomain
        {
            return (T) Activator.CreateInstance(typeof(T), ApplicationInterfaceHelper.Company);
        }

        public static IForm IfMenuNoExistsMakeIt(this IForm form, string id, string description)
        {
            if (!form.Menu.Exists(id))
                form.MakeSubMenu(id, description);
            return form;
        }

        public static IForm IfSeparatorNoExistsMakeIt(this IForm form, string id)
        {
            if (!form.Menu.Exists(id))
                form.MakeSeparatorMenu(id);
            return form;
        }

        public static IForm IfMenuExistsRemoveIt(this IForm form, string id)
        {
            if (form.Menu.Exists(id))
                form.RemoveSubMenu(id);
            return form;
        }

        public static void TryMakeSubMenu(this IForm form, string id, string description)
        {
            if (!form.Menu.Exists(id))
                form.Menu.AddEx(ApplicationInterfaceHelper.MakeLocalMenu(id, description, BoMenuType.mt_STRING));
        }

        public static void MakeSubMenu(this IForm form, string id, string description)
        {
            form.Menu.AddEx(ApplicationInterfaceHelper.MakeLocalMenu(id, description, BoMenuType.mt_STRING));
        }

        public static void MakeSeparatorMenu(this IForm form, string id)
        {
            form.Menu.AddEx(ApplicationInterfaceHelper.MakeLocalMenu(id, "x", BoMenuType.mt_SEPERATOR));
        }

        public static void TryRemoveSubMenu(this IForm form, string id)
        {
            if (form.Menu.Exists(id))
                form.Menu.RemoveEx(id);
        }

        public static void RemoveSubMenu(this IForm form, string id)
        {
            try
            {
                form.Menu.RemoveEx(id);
            }
            catch (Exception)
            {
                
            }
            
        }

        public static void Maximize(this IForm form)
        {
            form.State = BoFormStateEnum.fs_Maximized;
        }

        public static bool IsMaximized(this IForm form)
        {
            return form.State == BoFormStateEnum.fs_Maximized;
        }

        public static void FreezeAction(this IForm form, Action action)
        {
            try
            {
                form.Freeze(true);
                action();
                form.Freeze(false); 
            }
            catch (Exception )

            {
                form.Freeze(false);
                throw;
            }
        }

        public static void FreezeActionUsingFlag(this IForm form, Action action, ref bool flag)
        {
            form.Freeze(true);
            flag = true;
            action();
            flag = false;
            form.Freeze(false);
        }

        public static void ChangeToAddMode(this IForm form)
        {
            form.Mode = BoFormMode.fm_ADD_MODE;
        }

        public static bool IsAddMode(this IForm form)
        {
            return form.Mode == BoFormMode.fm_ADD_MODE;
        }

        public static void ChangeToSearchMode(this IForm form)
        {
            form.Mode = BoFormMode.fm_FIND_MODE;
        }

        public static bool IsSearchMode(this IForm form)
        {
            return form.Mode == BoFormMode.fm_FIND_MODE;
        }

        public static void ChangeToUpdateMode(this IForm form)
        {
            form.Mode = BoFormMode.fm_UPDATE_MODE;
        }

        public static bool IsUpdateMode(this IForm form)
        {
            return form.Mode == BoFormMode.fm_UPDATE_MODE;
        }

        public static void SetSupportedModes(this IForm form, SAPbouiCOM.BoFormMode mode)
        {
            form.SupportedModes = (int) mode;
        }
    }
}