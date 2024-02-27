using System;
using System.IO;
using System.Reflection;
using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.DisposableBO;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class ApplicationInterfaceHelper
    {
        private const string SALE_ORDER_MENU_ID = "2050";
        private const string RETURN_REQUEST_MENU_ID = "2218";
        private const string ITEMS_MENU_ID = "3073";
        private const string TOOLS_MENU_ID = "4864";
        private const string DATA_MENU_ID = "1280";
        private const string LAST_RECORD_MENU_ACTION = "1291";
        private const string REFRESH_MENU_ACTION = "1304";
        private const string DEFAULT_FORM_MENU_ID = "47616";
        private const string PURCHARSE_ORDER_MENU_ID = "2305";

        public const string ADD_RECORD_MENU_ID = "1282";
        public const string CANCEL_MENU_ID = "1284";


        public static SafeRecordSet SafeRecordSet
        {
            get
            {
                var recordSet = Company
                    .GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx)
                    .To<SAPbobsCOM.RecordsetEx>();

                return new SafeRecordSet(recordSet);
            }
        }

        public static void RefreshRegister()
        {
            MenuItem refreshMenu = ApplicationInstance.Menus.Item(DATA_MENU_ID)
                .SubMenus.Item(REFRESH_MENU_ACTION);
            refreshMenu.Activate();
        }

        public static void LastRecord()
        {
            MenuItem refreshMenu = ApplicationInstance.Menus.Item(DATA_MENU_ID)
                .SubMenus.Item(LAST_RECORD_MENU_ACTION);
            refreshMenu.Activate();
        }

        public static Form OpenInventoryTransferRequestForm()
        {
            ApplicationInstance.ActivateMenuItem("3088");
            return ApplicationInstance.Forms.ActiveForm;
        }

        public static Form GetActiveForm()
        {
            return ApplicationInstance.Forms.ActiveForm;
        }

        public static Form OpenSystemForm(SAPbouiCOM.BoFormObjectEnum formObject, object documentEntry)
        {
            return ApplicationInstance.OpenForm(formObject, string.Empty, documentEntry.ToString());
        }

        public static Form OpenSaleOrderForm()
        {
            ApplicationInstance.ActivateMenuItem(SALE_ORDER_MENU_ID);
            return ApplicationInstance.Forms.ActiveForm;
        }

        public static Form OpenReturnRequestForm()
        {
            ApplicationInstance.ActivateMenuItem(RETURN_REQUEST_MENU_ID);
            return ApplicationInstance.Forms.ActiveForm;
        }

        public static Form OpenPurcharseOrderForm()
        {
            ApplicationInstance.ActivateMenuItem(PURCHARSE_ORDER_MENU_ID);
            return ApplicationInstance.Forms.ActiveForm;
        }

        public static Form OpenItemForm()
        {
            ApplicationInstance.ActivateMenuItem(ITEMS_MENU_ID);
            return ApplicationInstance.Forms.ActiveForm;
        }

        public static Form OpenUDOForm<T>() where T : BaseUDO
        {
            var distributionHistoryMenuId = get_menu_id<T>();
            ApplicationInstance.ActivateMenuItem(distributionHistoryMenuId.Item1);
            return ApplicationInstance.Forms.GetForm($"UDO_FT_{distributionHistoryMenuId.Item2}", 1);
        }

        private static Tuple<string, string> get_menu_id<T>() where T : BaseUDO
        {
            var defaultFormMenuSubMenus = ApplicationInstance.Menus.Item(TOOLS_MENU_ID)
                .SubMenus.Item(DEFAULT_FORM_MENU_ID).SubMenus;

            var udoId = GenericHelper.GetUdoId<T>();

            for (var i = 0; i < defaultFormMenuSubMenus.Count; i++)
            {
                var subMenu = defaultFormMenuSubMenus.Item(i);
                if (subMenu.String.Contains(udoId))
                    return Tuple.Create(subMenu.UID, udoId);
            }

            throw new Exception($"[ERROR] No existe el UDO '{udoId}'");
        }


        public static void ShowSuccessStatusBarMessage(string message, BoMessageTime messageTime = BoMessageTime.bmt_Short)
        {
            ShowStatusBarMessage(message, messageTime, BoStatusBarMessageType.smt_Success);
        }

        public static void ShowErrorStatusBarMessage(string message, BoMessageTime messageTime = BoMessageTime.bmt_Short)
        {
            ShowStatusBarMessage(message, messageTime, BoStatusBarMessageType.smt_Error);
        }

        public static void ShowMessageBox(string message)
        {
            ApplicationInstance.MessageBox(message);
        }

        public static void ShowDialogMessageBox(string message, Action isSuccess = null, Action isCancel = null)
        {
            var result = ApplicationInstance.MessageBox(message, Btn1Caption: "OK", Btn2Caption: "Cancelar");
            if (result == 1)
                isSuccess?.Invoke();
            else
                isCancel?.Invoke();
        }

        public static StatusBar ShowStatusBarMessage(string message, BoMessageTime messageTime,
            BoStatusBarMessageType messageType)
        {
            var statusBar = ApplicationInstance.StatusBar;
            statusBar.SetText(message, messageTime, messageType);
            return statusBar;
        }

        public static Form RetrieveOpenedForm<T>() where T : SAPbouiCOM.Framework.UserFormBase
        {
            var formType = typeof(T);
            var formAttribute = formType.GetCustomAttribute<SAPbouiCOM.Framework.FormAttribute>();
            return ApplicationInstance.Forms.GetForm(formAttribute.FormType, 1);
        }

        public static SAPbobsCOM.Company Company { get; set; }

        public static Application ApplicationInstance
            => SAPbouiCOM.Framework.Application.SBO_Application;

        public static MenuCreationParams MakeLocalMenu(string id, string description, BoMenuType type)
        {
            var menu = (MenuCreationParams) ApplicationInstance.CreateObject(
                BoCreatableObjectType.cot_MenuCreationParams);
            menu.Checked = false;
            menu.Enabled = true;
            menu.Type = type;
            menu.UniqueID = id;
            menu.String = description;
            return menu;
        }

        public static ChooseFromListCreationParams MakeChooseListParams(string id, string udoType, bool multiSelection = false)
        {
            var fromListCreationParams = ApplicationInstance
                .CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams)
                .To<ChooseFromListCreationParams>();

            fromListCreationParams.MultiSelection = multiSelection;
            fromListCreationParams.ObjectType = udoType;
            fromListCreationParams.UniqueID = id;
            return fromListCreationParams;
        }

        public static ChooseFromListCreationParams MakeChooseListParams(string id, int objectType, bool multiSelection)
        {
            var fromListCreationParams = ApplicationInstance
                .CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams)
                .To<ChooseFromListCreationParams>();

            fromListCreationParams.MultiSelection = multiSelection;
            fromListCreationParams.ObjectType = objectType.ToString();
            fromListCreationParams.UniqueID = id;
            return fromListCreationParams;
        }

        public static SAPbouiCOM.UserDataSource MakeUserDataSourceIfNotExist(this SAPbouiCOM.UserDataSources dataSources, string code, SAPbouiCOM.BoDataType dataType, int length = 254)
        {
            try
            {
                return dataSources.Item(code);
            }
            catch
            {
                return dataSources.Add(code, dataType, length);
            }
        }

        public static SAPbouiCOM.DataTable MakeDataTableIfNotExist(this SAPbouiCOM.DataTables dataTables, string code)
        {
            try
            {
                return dataTables.Item(code);
            }
            catch
            {
                return dataTables.Add(code);
            }
        }

        public static SAPbouiCOM.ChooseFromList MakeChooseFromListIfNotExist(this SAPbouiCOM.ChooseFromListCollection collection, string code, object type)
        {
            try
            {
                return collection.Item(code);
            }
            catch
            {
                var creationParams = ApplicationInstance
                    .CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)
                    .To<SAPbouiCOM.ChooseFromListCreationParams>();

                creationParams.MultiSelection = false;
                creationParams.ObjectType = type.ToString();
                creationParams.UniqueID = code;
                
                return collection.Add(creationParams);
            }
        }

        public static EditText AppendChooseFromList(this EditText editText, string chooseFromListId,
            string chooseFromListField,
            string editTextDataSourceId)
        {
            editText.DataBind.SetBound(true, "", editTextDataSourceId);
            editText.ChooseFromListUID = chooseFromListId;
            editText.ChooseFromListAlias = chooseFromListField;
            try
            {
                editText.ChooseFromListAfter += (sboObject, val) =>
                {
                    try
                    {
                        DataTable selectedObjects = val.To<ISBOChooseFromListEventArg>().SelectedObjects;
                        object value = selectedObjects.GetValue(chooseFromListField, 0);
                        editText.Value = value.ToString();
                    }
                    catch (Exception)
                    {
                    }
                };
            }
            catch (Exception ex)
            {
                editText.Value = "";
            }

            return editText;
        }

        public static string CurrentPhysicalPath
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().GetName().CodeBase;
                return new FileInfo(new Uri(codeBase).LocalPath).Directory?.FullName;
            }
        }

        public static Folder Hide(this Folder folder)
        {
            Item item = folder.Item;
            item.Visible = false;
            GenericHelper.ReleaseCOMObjects(item);
            return folder;
        }

        public static SAPbouiCOM.Folder SetWidth(this SAPbouiCOM.Folder folder, int width)
        {
            SAPbouiCOM.Item genericItem = null;
            try
            {
                genericItem = folder.Item;
                genericItem.SetWidth(120);
                return folder;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(genericItem);
            }
        }

        

        public static SAPbouiCOM.Item SetWidth(this SAPbouiCOM.Item item, int width)
        {
            item.Width = width;
            return item;
        }

        public static SAPbouiCOM.Item SetHeight(this SAPbouiCOM.Item item, int height)
        {
            item.Height = height;
            return item;
        }

        public static SAPbouiCOM.Item SetLeft(this SAPbouiCOM.Item item, int left)
        {
            item.Left = left;
            return item;
        }

        public static bool FormIsActive(string sapFormType)
        {
            Forms forms = null;
            Form form = null;
            try
            {
                forms = ApplicationInstance.Forms;
                form = forms.ActiveForm;
                return form.TypeEx == sapFormType;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(forms, form);
            }
        }
    }
}