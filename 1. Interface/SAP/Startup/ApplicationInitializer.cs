using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAPbouiCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Menu;
using Exxis.Addon.HojadeRutaAGuia.Interface.Startup.Versions;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;
using VSVersionControl;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.Interface.Licencia;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Startup
{
    public abstract partial class ApplicationInitializer
    {
        private const string BASE_CLASS_TYPE = "BaseMenu";
        private const string SAP_MODULE_MENU_ID = "43520";
        private readonly string APPLICATION_ID;
        private readonly string APPLICATION_DESCRIPTION;
        private readonly string DOMAIN_NAMESPACE;

        private IList<BaseMenu> _applicationMenus;
        private readonly SAPMenu _rootMenu;
        private MenuItem _rootSAPMenu;
        private IList<FlowMenu> _applicationMenusTest;
        private readonly SAPbouiCOM.Framework.Application _sapApplication;
        private readonly Application _application;
        private readonly SAPbobsCOM.Company _company;
        public static string ProductId = "COMT2DEV";
        public static string ProductDescription = "Add-On Hoja De Ruta a Guía";


        protected ApplicationInitializer(ElementTuple<string> application, ElementTuple<string> menu, SAPbouiCOM.Framework.Application sapApplication)
        {
            _rootMenu = new SAPMenu { Id = menu.Id, Description = menu.Description };
            _sapApplication = sapApplication;
            _application = SAPbouiCOM.Framework.Application.SBO_Application;
            _company = _application.Company.GetDICompany().As<SAPbobsCOM.Company>();
            _applicationMenus = new List<BaseMenu>();
            DOMAIN_NAMESPACE = get_menu_namespace();
            APPLICATION_ID = application.Id;
            APPLICATION_DESCRIPTION = application.Description;
        }

        private string get_menu_namespace()
        {
            var resourceNamespace = this.GetCustomAttribute<ResourceNamespace>();
            return $"{resourceNamespace.NameSpace}.Menu";
        }

        public SAPbouiCOM.Framework.Application Build()
        {
            ApplicationInterfaceHelper.Company = _company;

            //Global.msj = "";
            //AddonVentura _ad = new AddonVentura(ProductId, ProductDescription,_company, _application);
            //if (_ad.isOK)
            //{
            charge_menus();
            try_make_base_menu().IfTrue(() => _applicationMenus.ForEach(build_sap_menu));
            append_versions();

            set_events();


            //}
            //else
            //{

            //}




            return _sapApplication;
        }

        #region build menus

        private void charge_menus()
        {
            foreach (Type type in get_derived_class())
            {
                var menu = GenericHelper.MakeInstance(type).To<BaseMenu>();
                _applicationMenus.Add(menu);
            }

            //var menu = GenericHelper.MakeInstance(type).To<FlowMenu>();
            //_applicationMenusTest.Add()

            _applicationMenus = _applicationMenus.OrderBy(t => t.RootSAPMenu.Id).ToList();
        }

        private IEnumerable<Type> get_derived_class()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.Namespace == DOMAIN_NAMESPACE && (type.BaseType?.Name.Contains(BASE_CLASS_TYPE) ?? false));
        }

        private bool try_make_base_menu()
        {
            try
            {
                //Constants.Menu.MAIN_APPLICATION,
                _rootSAPMenu = append_sap_menu(_application.Menus.Item(SAP_MODULE_MENU_ID).SubMenus, Constants.Menu.MAIN_APPLICATION, @"EXXIS-Addon Hoja de Ruta a Guías", BoMenuType.mt_POPUP);
                return true;
            }
            catch
            {
                return false;
            }
        }
         
        private void build_sap_menu(BaseMenu baseMenu)
        {
            append_sap_root_menu(baseMenu.RootSAPMenu.Id, baseMenu.RootSAPMenu.Description);
            foreach (SAPMenu item in baseMenu)
                append_sap_sub_menu(baseMenu.RootSAPMenu.Id, item.Id, item.Description);
        }

        private void append_sap_root_menu(string id, string description)
        {
            try
            {
                //cambiar a popup para 3 niveles
                append_sap_menu(_rootSAPMenu.SubMenus, id, description, BoMenuType.mt_STRING);
            }
            catch
            {
                //ignored
            }
        }

        private void append_sap_sub_menu(string rootMenuId, string id, string description)
        {
            try
            {
                //Menu de 3 niveles
                //MenuItem menuItem = _application.Menus.Item(rootMenuId);
                //append_sap_menu(menuItem.SubMenus, id, description, BoMenuType.mt_STRING);
            }
            catch
            {
                // ignored
            }
        }

        private MenuItem append_sap_menu(Menus menus, string id, string description, BoMenuType menuType)
        {
            MenuCreationParams packageRootMenu = make_sap_menu(id, description, menuType);
            return menus.AddEx(packageRootMenu);
        }

        private MenuCreationParams make_sap_menu(string id, string description, BoMenuType menuType)
        {
            var packageRootMenu = (MenuCreationParams)_application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams);
            packageRootMenu.Type = menuType;
            packageRootMenu.UniqueID = id;
            packageRootMenu.String = description;
            packageRootMenu.Enabled = true;
            packageRootMenu.Position = -1;
            return packageRootMenu;
        }

        #endregion

        #region append versions

        private void append_versions()
        {
            VersionCollection versions = get_versions_to_install();
            //var updater = new Updater(versions.GetLastVersion(), APPLICATION_ID, APPLICATION_DESCRIPTION, _company);
            //var updater = new Updater(versions.GetLastVersion(), APPLICATION_ID, APPLICATION_DESCRIPTION, _company);
            var updater = new Updater(versions.GetLastVersion(), ProductId, ProductDescription, _company);
            for (var i = 0; i < versions.Count; i++)
            {
                Versioner versioner = versions[i];
                if (versioner.HasFormattedSearch)
                    versioner = versioner.AppendFormattedSearch(_company);

                updater.addFlag(versioner.CurrentFlag);
            }

            updater.OpenIntallerForm();
        }

        private VersionCollection get_versions_to_install()
        {
            var versionCollection = new VersionCollection();
            BuildApplicationVersion(versionCollection);
            return versionCollection;
        }

        protected abstract void BuildApplicationVersion(VersionCollection versionCollection);

        #endregion

        private void set_events()
        {
            _application.MenuEvent += OnClickBaseMenuEvent;
            _application.AppEvent += LifeCycleEvent;
            _application.ItemEvent += OnReferenceEvent;
        }

        private void show_message(string message)
        {
            _application.MessageBox(message);
        }
    }
}