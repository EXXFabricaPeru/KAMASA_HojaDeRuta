using System;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Menu;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Interface.Views;
//using Exxis.Addon.HojadeRutaAGuia.Interface.Views.Modal;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Startup
{
    public partial class ApplicationInitializer
    {
        protected virtual void OnClickBaseMenuEvent(ref MenuEvent menuEvent, out bool handleEvent)
        {
            handleEvent = true;
            try
            {
                switch (menuEvent.MenuUID)
                {
                    case "1281": //Buscar
                    case "1282": //Crear
                        if (!menuEvent.BeforeAction)
                        {
                            FormEventRedirect(menuEvent.MenuUID);
                        }

                        break;
                }

                SAPMenu sapMenu = _applicationMenus.FindMenuId(menuEvent.MenuUID);
                if (menuEvent.BeforeAction == false)
                {
                    MenuItem menu = _application.Menus.Item("47616");

                    if (menu.SubMenus.Count <= 0)
                        return;

                    for (var i = 0; i < menu.SubMenus.Count; i++)
                    {
                        if (menu.SubMenus.Item(i).String.Contains(sapMenu.TargetUDOId))
                            menu.SubMenus.Item(i).Activate();
                    }
                }
                else if (sapMenu.IsFormMenu)
                {
                    UserFormBase form = sapMenu.TargetForm;
                    form.Show();
                }
            }
            catch (ArgumentException argumentException)
            {
                return;
            }
            catch (Exception exception)
            {
                _application.SetStatusBarMessage(exception.Message);
                handleEvent = false;
            }
        }

        protected virtual void FormEventRedirect(string menuUID)
        {
            try
            {
                Form form = _application.Forms.ActiveForm;
                switch (form.TypeEx)
                {
                    case "Exxis.Addon.HojadeRutaAGuia.Interface.Views.FileUpload":
                        if (menuUID == "1282")
                        {
                            FileUpload_Crear(ref form);
                        }
                        else if (menuUID == "1281")
                        {
                            FileUpload_Buscar(ref form);
                        }

                        break;
                    case "Exxis.Addon.HojadeRutaAGuia.Interface.Views.FileTemplate":
                        FileTemplate_Alternar(ref form, menuUID == "1282");
                        break;
                }
            }
            catch
            {
            }
        }

        private void FileTemplate_Alternar(ref Form form, bool crear)
        {
            form.Items.Item("Item_6").Enabled = crear;
            form.Items.Item("Item_5").Enabled = crear;
            form.Items.Item("Item_7").Enabled = crear;
            form.Items.Item("Item_11").Enabled = crear;
            form.Items.Item("Item_13").Enabled = crear;
            form.Items.Item("Item_15").Enabled = crear;
            form.Items.Item("Item_8").Enabled = true;
        }


        private void FileUpload_Crear(ref Form form)
        {
            form.Items.Item("Item_1").Enabled = true;
            form.Items.Item("Item_8").Enabled = true;
            form.Items.Item("Item_9").Enabled = true;
            form.Items.Item("Item_5").Enabled = true;
            form.Items.Item("Item_14").Visible = false;
        }

        private void FileUpload_Buscar(ref Form form)
        {
            form.Items.Item("Item_1").Enabled = true;
            form.Items.Item("Item_8").Enabled = true;
            form.Items.Item("Item_9").Enabled = false;
            form.Items.Item("Item_5").Enabled = false;
            form.Items.Item("Item_14").Visible = false;
        }

        protected virtual void LifeCycleEvent(BoAppEventTypes eventTypes)
        {
            switch (eventTypes)
            {
                case BoAppEventTypes.aet_ShutDown:
                    System.Windows.Forms.Application.Exit();
                    break;
                case BoAppEventTypes.aet_CompanyChanged:
                    break;
                case BoAppEventTypes.aet_FontChanged:
                    break;
                case BoAppEventTypes.aet_LanguageChanged:
                    break;
                case BoAppEventTypes.aet_ServerTerminition:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventTypes), eventTypes, null);
            }
        }

        protected virtual void OnReferenceEvent(string formId, ref ItemEvent itemEvent, out bool handleEvent)
        {
            handleEvent = true;
        }
    }
}