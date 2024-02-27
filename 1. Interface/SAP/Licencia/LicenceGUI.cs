using SAPbouiCOM;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Licencia
{
    public class LicenceGUI
    {
        public SAPbouiCOM.Application oAplication { get; set; }
        public SAPbobsCOM.Company oCompany { get; set; }

        private SAPbouiCOM.Form formLicencia;
        private const string rutaFormulario = "rec//FormLicencias.srf";
        private const string tipo = "FrmLic";
        private const string id = "FrmLic0909";

        private string FilePath = string.Empty;

        public event EventHandler CargarLicencia;

        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public bool LicenciaValida { get; set; }

        public LicenceGUI(SAPbouiCOM.Application app, SAPbobsCOM.Company comp, string codigo, string descripcion)
        {
            this.oAplication = app;
            this.oCompany = comp;
            this.CodigoProducto = codigo;
            this.Descripcion = descripcion;

            oAplication.ItemEvent += new _IApplicationEvents_ItemEventEventHandler(FormLicencia_ItemEvent);
        }

        protected virtual void OnLicenceUploaded(EventArgs e)
        {
            CargarLicencia?.Invoke(this, e);
        }

        private void FormLicencia_ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                switch (pVal.EventType)
                {
                    case BoEventTypes.et_ITEM_PRESSED:

                        switch (pVal.ItemUID)
                        {
                            case "2":
                                LicenciaValida = false;
                                OnLicenceUploaded(EventArgs.Empty);

                                break;

                            case "Item_8":

                                if (!pVal.BeforeAction)
                                {
                                    FilePicker();
                                    if (!string.IsNullOrEmpty(FilePath))
                                        formLicencia.DataSources.UserDataSources.Item("UD_0").Value = FilePath;
                                }

                                break;

                            case "btnReg":

                                if (pVal.BeforeAction)
                                {
                                    string licenseEncrypted = GetEncryptedText(FilePath);
                                    Licencia licencia = new Licencia();
                                    LicenciaValida = EsLicenciaValida(licenseEncrypted, out licencia);


                                    if (LicenciaValida)
                                    {
                                        ActualizarLicencia(licencia);
                                        formLicencia.Close();
                                        OnLicenceUploaded(EventArgs.Empty);
                                    }
                                    else
                                    {
                                        oAplication.MessageBox("No es una licencia válida para el producto " + CodigoProducto);
                                    }
                                }

                                break;

                            default:
                                break;
                        }

                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ActualizarLicencia(Licencia licencia)
        {
            SAPbobsCOM.Recordset oRecordSet = null;
            try
            {
                oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string query = $"UPDATE \"@VS_PRD1\" SET \"U_VS_LICENC\" = '{licencia.Raw}', \"U_VS_FECVIG\" = '{licencia.Fechavigencia.ToString("yyyyMMdd")}' WHERE \"U_VS_CODPRD\" = '{CodigoProducto}'";
                oRecordSet.DoQuery(query);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (oRecordSet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordSet);
                oRecordSet = null;
                GC.Collect();
            }
        }

        private string GetEncryptedText(string filePath)
        {
            try
            {
                StreamReader sr = new StreamReader(filePath);
                while (!sr.EndOfStream)
                {
                    string linea = sr.ReadLine();
                    return linea;
                }

                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FilePicker()
        {
            System.Threading.Thread ShowFolderBrowserThread = new System.Threading.Thread(ShowFolderBrowser);

            if (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Unstarted)
            {
                ShowFolderBrowserThread.SetApartmentState(System.Threading.ApartmentState.STA);
                ShowFolderBrowserThread.Start();
            }

            if (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                ShowFolderBrowserThread.Start();
                ShowFolderBrowserThread.Join();
            }

            while (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Running)
                System.Windows.Forms.Application.DoEvents();
        }

        private void ShowFolderBrowser()
        {
            Process[] myProcs;
            OpenFileDialog openFolder = new OpenFileDialog();
            System.Windows.Forms.Form formBusqueda = new System.Windows.Forms.Form();

            myProcs = Process.GetProcessesByName("SAP Business One");

            if (myProcs.Length >= 1)
            {
                formBusqueda.MaximizeBox = false;
                formBusqueda.MinimizeBox = false;
                formBusqueda.Width = 1;
                formBusqueda.Height = 1;
                formBusqueda.StartPosition = FormStartPosition.CenterScreen;
                formBusqueda.Activate();
                formBusqueda.BringToFront();
                formBusqueda.Visible = true;
                formBusqueda.TopMost = true;
                formBusqueda.Focus();
                openFolder.Filter = "Text|*.txt";
                DialogResult ret = openFolder.ShowDialog(formBusqueda);
                formBusqueda.Hide();

                if (ret == DialogResult.OK)
                {
                    FilePath = openFolder.FileName;
                }
                else
                {
                    System.Windows.Forms.Application.ExitThread();
                }
            }
        }

        public void GestionarLicencia()
        {
            try
            {
                string licenciaEncriptada = GetLicenciaFromSAP(CodigoProducto);

                if (string.IsNullOrEmpty(licenciaEncriptada)) // mandamos al registro
                {
                    AbrirFormularioRegistroLicencias();
                }
                else //analizamos validez
                {
                    Licencia licencia = new Licencia();
                    if (EsLicenciaValida(licenciaEncriptada, out licencia))
                    {
                        LicenciaValida = true;
                        //var fecha = licencia.Fechavigencia;
                        //var fech2 = oCompany.GetCompanyDate().AddDays(0);
                        //var fech3 = licencia.Fechavigencia.CompareTo(fech2);
                        var addday = licencia.Fechavigencia - oCompany.GetCompanyDate();
                        if (addday.Days<15)
                        {
                            if (addday.Days == 0)
                            {
                                var msj = Descripcion+": La licencia está por vencer en hoy, contacte con soporte." + "\t Fecha de vencimiento: " + licencia.Fechavigencia.ToShortDateString();
                                Global.msj = msj;
                                oAplication.StatusBar.SetText(msj, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning);

                            }
                            else
                            {
                                var msj = Descripcion+": La licencia está por vencer en " + addday.Days + " días, contacte con soporte." + "\t Fecha de vencimiento: " + licencia.Fechavigencia.ToShortDateString();
                                Global.msj = msj;
                                oAplication.StatusBar.SetText(msj, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning);
                            }
                            
                        }
                        

                        OnLicenceUploaded(EventArgs.Empty);
                    }
                    else
                    {
                        AbrirFormularioRegistroLicencias();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool EsLicenciaValida(string licenciaEncripted, out Licencia licencia)
        {
            licencia = null;
            if (licenciaEncripted == "")
                return false;

            string licenciaDecripted = Criptor.Desencriptar(licenciaEncripted);

            try
            {
                licencia = new Licencia(licenciaDecripted);
                licencia.Raw = licenciaEncripted;
            }
            catch (Exception ex)
            {
                return false;
            }

            if (licencia.CodProducto != CodigoProducto) return false;
            if (licencia.InstllNumber != oAplication.Company.InstallationId) return false;
            if (licencia.Fechavigencia.CompareTo(oCompany.GetCompanyDate()) < 0) return false;

            return true;
        }

        private void AbrirFormularioRegistroLicencias()
        {
            try
            {
                if (tipo != null && id != null && rutaFormulario != null)
                {
                    XmlDocument xmlDocument = new XmlDocument();

                    FormCreationParams CreationPackage = (FormCreationParams)oAplication.CreateObject(BoCreatableObjectType.cot_FormCreationParams);
                    xmlDocument.Load(rutaFormulario);
                    CreationPackage.XmlData = xmlDocument.InnerXml;
                    CreationPackage.FormType = tipo;
                    CreationPackage.UniqueID = id;
                    formLicencia = oAplication.Forms.AddEx(CreationPackage);

                    formLicencia.Title = "[" + Descripcion + "] Archivo de licencia";

                    if (oAplication.ClientType == BoClientType.ct_Desktop)
                    {
                        formLicencia.Left = (oAplication.Desktop.Width - formLicencia.Width) / 2;
                        formLicencia.Top = (oAplication.Desktop.Height - formLicencia.Height) / 2;
                    }

                    StaticText label = (StaticText)formLicencia.Items.Item("Item_3").Specific;
                    label.Caption = string.Format(label.Caption, oAplication.Company.InstallationId, Descripcion);

                    formLicencia.Visible = true;
                }
            }
            catch
            {
                throw;
            }
        }

        private string GetLicenciaFromSAP(string codigoProducto)
        {
            try
            {
                string licencia = string.Empty;

                if (string.IsNullOrEmpty(codigoProducto))
                    throw new Exception("Debe indicar un código de producto");

                SAPbobsCOM.Recordset oRecordSet = null;

                try
                {
                    oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string query = oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB ? $"SELECT TOP 1 IFNULL(\"U_VS_LICENC\",'') \"licencia\" FROM \"@VS_PRD1\" T0 WHERE \"U_VS_CODPRD\" = '{codigoProducto}'" : $"SELECT TOP 1 ISNULL(U_VS_LICENC,'') [licencia] FROM [dbo].[@VS_PRD1] T0 WHERE U_VS_CODPRD = '{codigoProducto}'";
                    oRecordSet.DoQuery(query);

                    if (oRecordSet.RecordCount == 0)
                        throw new Exception($"El producto {codigoProducto} no ha sido registrado. Debe realizar este paso antes de validar una licencia");

                    licencia = oRecordSet.Fields.Item(0).Value.ToString();
                }
                catch (Exception ex)
                {
                    throw;
                }

                return licencia;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
