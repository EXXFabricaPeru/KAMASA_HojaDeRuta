using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;
using System.IO;
using System.Reflection;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Licencia
{
    public class AddonVentura
    {
        public bool isOK = false;
        public AddonVentura(string productCode, string productDescription, SAPbobsCOM.Company _company, Application _application)
        {
            try
            {
                AddResources();

                //Application application = Global.sboApplication;// GetApplication();
                //SAPbobsCOM.Company company = Global.sboCompany;//GetCompany();

                string version = GetVersion();

                VersionManager versionManager = new VersionManager(_company);
                int result = -1;
                versionManager.ValidarVersion(productCode, productDescription, version, out result);
                Global.isLicenciaValida = false;
                LicenceGUI licenceMngr = new LicenceGUI(_application, _company, productCode, productDescription);

                //EventFilters eventFilters = new EventFilters();
                //EventFilter eventFilter = null;
                //eventFilter = eventFilters.Add(BoEventTypes.et_ALL_EVENTS);
                //eventFilter.AddEx("FrmLic");

                //application.SetFilter(eventFilters);

                licenceMngr.CargarLicencia += (sender, e) =>
                {
                    if (licenceMngr.LicenciaValida)
                    {
                        //if (result == 1)
                        //CrearEstructuraDatos();
                        isOK = true;
                        Global.isLicenciaValida = true;
                        //StartAddOn();
                    }
                    else
                    {
                        //var x = Global.isLicenciaValida;
                        if (!Global.isLicenciaValida)
                        {
                            isOK = false;

                            _application.MessageBox("La licencia ingresada no es válida");
                            System.Windows.Forms.Application.Exit();
                        }
                        
                    }
                };

                licenceMngr.GestionarLicencia();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private void AddResources()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string currentDirectory = Environment.CurrentDirectory;

                string resourceFolder = Path.Combine(currentDirectory, "rec");
                string[] destinationPaths = null;

                int indexDest = 0;

                string[] resourceNames = assembly.GetManifestResourceNames();
                if (resourceNames.Length > 0)
                    destinationPaths = new string[resourceNames.Length];

                foreach (string resourceName in resourceNames)
                {
                    string[] parts = resourceName.Split('.');
                    string res = parts[parts.Length - 2] + "." + parts[parts.Length - 1];

                    destinationPaths[indexDest] = Path.Combine(currentDirectory, "rec", res);

                    using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
                    {
                        if (resourceStream != null)
                        {
                            string destinationDirectory = Path.GetDirectoryName(destinationPaths[indexDest]);
                            if (!Directory.Exists(destinationDirectory))
                            {
                                Directory.CreateDirectory(destinationDirectory);
                            }

                            using (FileStream fileStream = new FileStream(destinationPaths[indexDest], FileMode.Create))
                            {
                                resourceStream.CopyTo(fileStream);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Resource not found.");
                        }
                    }

                    indexDest++;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetVersion()
        {
            Assembly callingAssembly = Assembly.GetEntryAssembly();
            Version version = callingAssembly.GetName().Version;
            return version.ToString();
        }

    }
}
