using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserViews
{
    [FormAttribute("Exxis.Addon.HojadeRutaAGuia.Interface.Views.UserViews.Form1", "Views/UserViews/Form1.b1f")]
    class Form1 : UserFormBase
    {

        public const string ID = "PRUEBAS";
        public const string DESCRIPTION = "PRUEBAS123";
        public Form1()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.ButtonCombo0 = ((SAPbouiCOM.ButtonCombo)(this.GetItem("Item_0").Specific));
            this.ButtonCombo0.ComboSelectAfter += new SAPbouiCOM._IButtonComboEvents_ComboSelectAfterEventHandler(this.ButtonCombo0_ComboSelectAfter);
            this.ButtonCombo0.ClickAfter += new SAPbouiCOM._IButtonComboEvents_ClickAfterEventHandler(this.ButtonCombo0_ClickAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.ButtonCombo ButtonCombo0;

        private void OnCustomInitialize()
        {

            ButtonCombo0.ValidValues.Add("1", "Imprimir GR");
            ButtonCombo0.ValidValues.Add("2", "Imprimir HR");
            ButtonCombo0.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
        }

        bool eventButtonCombo = false;
        private void ButtonCombo0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {

                if (eventButtonCombo)
                    eventButtonCombo = false;
                else
                {
                    if (ButtonCombo0.Selected.Value == "1")
                        imprimirGR();
                    else
                        imprimirHR();
                }

            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }

        private void ButtonCombo0_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                eventButtonCombo = true;
                var x = pVal;
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
            }

        }

        private void imprimirGR()
        {
            ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("imprimirGR");
        }

        private void imprimirHR()
        {
            CrystalReportViewer crystalReportViewer = new CrystalReportViewer();

            // Crear una instancia del informe de Crystal Reports
            ReportDocument reportDocument = new ReportDocument();
          
            try
            {
                PrinterSettings printerSettings = new PrinterSettings();

                // Verifica si hay una impresora predeterminada configurada
                var impresoraDefecto = "";
                if (printerSettings.IsDefaultPrinter)
                {
                    impresoraDefecto = printerSettings.PrinterName;
                }
                else
                {
                    throw new Exception("No hay una impresora predeterminada configurada.");
                }

                var sPath = System.Windows.Forms.Application.StartupPath;
                // Cargar el archivo del informe de Crystal Reports
                try
                {
                 
                    reportDocument.Load(@"\\192.168.1.215\Compartida\ReporteAsignacionRuta.rpt");
                    //reportDocument.Load(@"C:\Reporte\ReporteAsignacionRuta.rpt"); // Reemplaza "ruta\al\informe.rpt" con la ruta real de tu informe

                }
                catch (Exception)
                {
                    var bin = sPath + "\\Reports\\ReporteAsignacionRuta.rpt";
                    //reportDocument.Load(@"\\192.168.1.215\Instaladores\ReporteAddon.rpt");
                    reportDocument.Load(bin);
                }


                // Agregar parámetros al informe

                reportDocument.SetParameterValue(0, "2024-004");
                // Asignar el informe al visor de informes
                crystalReportViewer.ReportSource = reportDocument;

                reportDocument.PrintOptions.PrinterName = impresoraDefecto;
                reportDocument.PrintToPrinter(1, true, 1, 1);

                //// Agregar el visor de informes al formulario
                //form.Controls.Add(crystalReportViewer);

                //// Mostrar el formulario
                //System.Windows.Forms.Application.Run(form);
            }
            catch (Exception ex)
            {
                ApplicationInterfaceHelper.ShowErrorStatusBarMessage(ex.Message);
                Console.WriteLine($"Error al abrir el informe: {ex.Message}");
            }
            finally
            {
                // Liberar recursos
                reportDocument.Close();
                reportDocument.Dispose();
            }
            ApplicationInterfaceHelper.ShowSuccessStatusBarMessage("imprimirHR");
        }

    }
}
