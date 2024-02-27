using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public static class EmailHelper
    {

        public static bool SendMail(string email, string password, string host, string port, string subject, string body, List<MailAddress> LstTo, out string msgError, bool oIsBodyHtml = false, MailPriority oPriority = MailPriority.Normal, List<Attachment> LstAttachment = null, AlternateView Av = null)
        {
            var result = false;
            msgError = String.Empty;
            try
            {
                var fromAddress = new MailAddress(email);
                var fromPassword = password;


                //using (var message = new MailMessage())
                //{
                var message = new MailMessage();

                message.From = fromAddress;
                //To = toAddress,
                message.Subject = subject;
                if (oIsBodyHtml)
                    message.AlternateViews.Add(Av);
                else
                    message.Body = body;

                message.IsBodyHtml = oIsBodyHtml;
                message.Priority = oPriority;

                if (LstTo != null)
                    foreach (var oTo in LstTo)
                        message.To.Add(oTo);

                if (LstAttachment != null)
                    foreach (var item in LstAttachment)
                        message.Attachments.Add(item);


                //using (var smtp = new SmtpClient(oConfig?.U_MSS_MAIL_HOST))
                //{
                var smtp = new SmtpClient(host);
                smtp.Port = Convert.ToInt32(port);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);
                smtp.EnableSsl = Convert.ToBoolean(true);
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.Send(message);
                //}
                //}

                result = true;
            }
            catch (Exception ex)
            {
                msgError = ex.Message;
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                //log.Error(ex.Message, ex);
                //ex.SendEmailException();
            }
            finally
            {
            }
            return result;
        }

        public static class EmailCorreo
        {

            //public const string DesaprobacionExtemp = "\\\\192.168.10.17\\b1_shf\\PyD\\Email\\DesaprobacionExtempEmail.html";
            public const string DesaprobacionExtemp = "C:\\Users\\fbueno.SRV-SOR10-21\\Documents\\git2\\src\\1. Interface\\SAP\\Resources\\Email\\DesaprobacionExtempEmail.html";
            //    public const string RETIRARPP = "\\\\192.168.0.68\\file server sap\\ADDON DPP\\CORREO_FORMATOS\\QuitarPPEmail.html";
            //    public const string RIESGOPP = "\\\\192.168.0.68\\file server sap\\ADDON DPP\\CORREO_FORMATOS\\RiesgoQuitarPPEmail.html";
            //    public const string ESTADOCUENTA = "\\\\192.168.0.68\\file server sap\\ADDON DPP\\CORREO_FORMATOS\\ReporteEstadoCuentaEmail.html";
        }

        public static class CorreoVariables
        {
            public const string NumOrdenVenta = "{NumOrdenVenta}";
        }
    }
}
