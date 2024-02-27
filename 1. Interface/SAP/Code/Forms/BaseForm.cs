using System;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Interface.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Code.Forms
{
    public abstract class BaseForm : UserFormBase
    {
        protected Action<object, SBOItemEventArg> ClosingForm
            => (@object, eventArg) => UIAPIRawForm.Close();

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {

            OnComponentsInitialize();
            OnCustomInitialize();
            form_initialize();
        }

        protected abstract void OnComponentsInitialize();

        protected abstract void OnCustomInitialize();

        private void form_initialize()
        {
            var formAttribute = GetType().GetCustomAttribute<SAPFormAttribute>();
            if (formAttribute != null)

                UIAPIRawForm.Title = formAttribute.Description;

        }

        protected Button GetItemButton(string id)
            => GetItem(id).Specific.To<Button>();

        protected StaticText GetItemStaticText(string id)
            => GetItem(id).Specific.To<StaticText>();

        protected EditText GetItemEditText(string id)
            => GetItem(id).Specific.To<EditText>();

        protected Matrix GetItemMatrix(string id)
            => GetItem(id).Specific.To<Matrix>();

        protected ComboBox GetItemComboBox(string id)
            => GetItem(id).Specific.To<ComboBox>();
        protected PictureBox GetItemPictureBox(string id)
            => GetItem(id).Specific.To<PictureBox>();

        protected T GetDomain<T>() where T : BaseDomain
        {
            return (T)Activator.CreateInstance(typeof(T), ApplicationInterfaceHelper.Company);
        }

        protected void MakeSubMenu(string id, string description)
        {
            UIAPIRawForm.Menu.AddEx(ApplicationInterfaceHelper.MakeLocalMenu(id, description, BoMenuType.mt_STRING));
        }

        protected void RemoveSubMenu(string id)
        {
            UIAPIRawForm.Menu.RemoveEx(id);
        }

        protected void ChangeForm(Action action)
        {
            UIAPIRawForm.Freeze(true);
            action();
            UIAPIRawForm.Freeze(false);
        }

        protected ChooseFromList MakeSingleSelectionChooseFromList(string id, int objectType)
        {
            ChooseFromListCreationParams chooseListParams = ApplicationInterfaceHelper.MakeChooseListParams(id, objectType,false);
            return UIAPIRawForm.ChooseFromLists.Add(chooseListParams);
        }

        protected ChooseFromList MakeMultiSelectionChooseFromList(string id, int objectType)
        {
            ChooseFromListCreationParams chooseListParams = ApplicationInterfaceHelper.MakeChooseListParams(id, objectType,true);
            return UIAPIRawForm.ChooseFromLists.Add(chooseListParams);
        }
    }
}
