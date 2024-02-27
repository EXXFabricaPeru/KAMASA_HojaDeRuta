using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        private IntPtr _hwnd;

        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }
    }
    class FileDialog
    {
        public string ruta;
        string filtro = "Archivos Excel|*.xls;*.xlsx";

        public string FindFile(string filtroOpen)
        {
            filtro = filtroOpen;
            System.Threading.Thread ShowFolderBrowserThread = default(System.Threading.Thread);
            try
            {
                //int f = Int32.Parse("12gh5");
                ShowFolderBrowserThread = new System.Threading.Thread(ShowOpenFile);
                if (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    ShowFolderBrowserThread.SetApartmentState(System.Threading.ApartmentState.STA);
                    ShowFolderBrowserThread.Start();
                }
                else if (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    ShowFolderBrowserThread.Start();
                    ShowFolderBrowserThread.Join();
                }

                while (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    System.Windows.Forms.Application.DoEvents();
                }

                if (!string.IsNullOrEmpty(ruta))
                {
                    return ruta;
                }
            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }
        public string FindRuta()
        {
            try
            {
                //System.Threading.Thread ShowFolderBrowserThread = default;
                System.Threading.Thread ShowFolderBrowserThread = new System.Threading.Thread(ShowFolderBrowser);
                if (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    ShowFolderBrowserThread.SetApartmentState(System.Threading.ApartmentState.STA);
                    ShowFolderBrowserThread.Start();
                }
                else if (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    ShowFolderBrowserThread.Start();
                    ShowFolderBrowserThread.Join();
                }
                else
                {

                    return "";
                }

                while (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    System.Windows.Forms.Application.DoEvents();
                }

                if (!string.IsNullOrEmpty(ruta))
                {
                    return ruta;
                }
            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }

        public string SaveFile(string filtroOpen)
        {
            filtro = filtroOpen;
            System.Threading.Thread ShowFolderBrowserThread = default(System.Threading.Thread);
            try
            {
                ShowFolderBrowserThread = new System.Threading.Thread(ShowSaveFile);
                if (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    ShowFolderBrowserThread.SetApartmentState(System.Threading.ApartmentState.STA);
                    ShowFolderBrowserThread.Start();
                }
                else if (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    ShowFolderBrowserThread.Start();
                    ShowFolderBrowserThread.Join();
                }

                while (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    System.Windows.Forms.Application.DoEvents();
                }

                if (!string.IsNullOrEmpty(ruta))
                {
                    return ruta;
                }
            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }

        public void ShowSaveFile()
        {
            System.Diagnostics.Process[] myProcess;
            
            var dialog = new SaveFileDialog();

            try
            {
                dialog.InitialDirectory = @"C:\";
                dialog.Title = @"Guardar Reporte Excel";
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "xlsx";
                dialog.Filter = filtro;
                dialog.FilterIndex = 0;
                dialog.RestoreDirectory = true;
                
                myProcess = System.Diagnostics.Process.GetProcessesByName("SAP Business One");

                if (myProcess.Length >= 0)
                {
                    WindowWrapper windowWrapper = new WindowWrapper(myProcess[0].MainWindowHandle);
                    DialogResult ret = dialog.ShowDialog(windowWrapper);
                    
                    if (ret == DialogResult.OK)
                    {
                        ruta = dialog.FileName;
                    }
                    else
                    {
                        if (ret == System.Windows.Forms.DialogResult.Cancel)
                            ruta = "xxx";
                        System.Windows.Forms.Application.ExitThread();
                    }
                }
            }
            catch { }
        }

        public void ShowOpenFile()
        {
            System.Diagnostics.Process[] MyProcs;


            OpenFileDialog OpenFile = new OpenFileDialog();

            try
            {
                OpenFile.Multiselect = false;
                OpenFile.Filter = filtro;
                int filterindex = 0;


                OpenFile.FilterIndex = filterindex;
                OpenFile.RestoreDirectory = true;
                MyProcs = System.Diagnostics.Process.GetProcessesByName("SAP Business One");


                if (MyProcs.Length >= 0)
                {
                    WindowWrapper MyWindow = new WindowWrapper(MyProcs[0].MainWindowHandle);
                    DialogResult ret = OpenFile.ShowDialog(MyWindow);


                    if (ret == DialogResult.OK)
                    {
                        ruta = OpenFile.FileName;
                    }
                    else
                    {
                        if (ret == System.Windows.Forms.DialogResult.Cancel)
                            ruta = "";
                        System.Windows.Forms.Application.ExitThread();

                    }
                }
            }
            catch { }
        }

        public void ShowFolderBrowser()
        {

            System.Diagnostics.Process[] MyProcs = null;
            ruta = "";
            System.Windows.Forms.FolderBrowserDialog OpenFile = new System.Windows.Forms.FolderBrowserDialog();

            try
            {
                MyProcs = System.Diagnostics.Process.GetProcessesByName("SAP Business One");
                if (MyProcs.Length >= 1)
                {
                    System.Diagnostics.Process sbo = GetProcesoUsuarioActual("SAP Business One");
                    if (sbo != null)
                    {
                        WindowWrapper MyWindow = new WindowWrapper(sbo.MainWindowHandle);

                        System.Windows.Forms.DialogResult ret = OpenFile.ShowDialog(MyWindow);

                        if (ret == System.Windows.Forms.DialogResult.OK)
                        {
                            ruta = OpenFile.SelectedPath;
                            OpenFile.Dispose();
                        }
                        else
                        {
                            if (ret == System.Windows.Forms.DialogResult.Cancel)
                                ruta = "";
                            System.Windows.Forms.Application.ExitThread();
                        }
                    }
                    else
                    {

                    }

                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                OpenFile.Dispose();
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        static uint TOKEN_QUERY = 0x0008;

        public System.Diagnostics.Process GetProcesoUsuarioActual(String processName)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName(processName))
            {
                IntPtr ph = IntPtr.Zero;
                try
                {
                    OpenProcessToken(p.Handle, TOKEN_QUERY, out ph);
                    WindowsIdentity wi = new WindowsIdentity(ph);
                    if (wi.Name == Environment.UserDomainName + "\\" + Environment.UserName)
                    {
                        return p;
                    }
                }
                catch (Exception xcp)
                {

                }
                finally
                {
                    if (ph != IntPtr.Zero) { CloseHandle(ph); }
                }
            }
            return null;
        }
    }
}
