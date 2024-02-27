using System;
using SAPbouiCOM.Framework;
using System.Reflection;
using System.Diagnostics;
//using SAPbobsCOM;
//using SAPbouiCOM;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.Interface.Licencia;
using B1SLayer;
namespace Exxis.Addon.HojadeRutaAGuia.Interface.Startup
{
    public class SAPApplication
    {

        public static string ProductId = "COMT2DEV";
        public static string ProductDescription = "Add-On Hoja de Ruta a Guía";

        ////private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //private SAPbobsCOM.Company sboCompany = null;
        //private SAPbouiCOM.Application sboApplication = null;
        //public SAPApplication(string productCode, string productDescription) : base(productCode, productDescription)
        //{
        //}

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {

                ApplicationInitializer initializer = args.Length < 1
                    ? HojaRutaaGuiaApplicationInitializer.MakeDevelopmentInitializer()
                    : HojaRutaaGuiaApplicationInitializer.MakeProductionInitializer(args[0]);

                //ApplicationInitializer initializer = ComisionTarjetasApplicationInitializer.MakeDevelopmentInitializer();
                //if (args.Length == 1)
                //{
                Application sapApplication = initializer.Build();

                Application.SBO_Application.StatusBar.SetText(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);

                //if (!string.IsNullOrEmpty(Global.msj))
                //    Application.SBO_Application.StatusBar.SetText(Global.msj, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                sapApplication.Run();
                //}



            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }



        }
    }
}