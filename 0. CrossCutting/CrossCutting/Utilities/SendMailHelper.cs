using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public class SendMailHelper
    {
        public static bool SendMail(string subject, string body, List<MailAddress> LstTo, out string msgError, bool oIsBodyHtml = false, MailPriority oPriority = MailPriority.Normal, List<Attachment> LstAttachment = null, AlternateView Av = null)
        {
            var result = false;
            msgError = String.Empty;
            try
            {

                var U_MSS_MAIL_EMAIL = "rgamarra@vsperu.com";
                var U_MSS_MAIL_PASS = "ventura955790";
                var U_MSS_MAIL_HOST = "smtp.gmail.com";
                var U_MSS_MAIL_PORT = "25";


                //var oConfig = MaestroConfiguracionAddOnDataAccess.ObtenerConfiguracionCab().FirstOrDefault();

                //if (String.IsNullOrEmpty(oConfig?.U_MSS_MAIL_EMAIL)) throw new Exception("Email no encontrado en la configuración del AddOn");
                //if (String.IsNullOrEmpty(oConfig?.U_MSS_MAIL_PASS)) throw new Exception("Contraseña no encontrado en la configuración del AddOn");
                //if (String.IsNullOrEmpty(oConfig?.U_MSS_MAIL_HOST)) throw new Exception("Host no encontrado en la configuración del AddOn");
                //if (String.IsNullOrEmpty(oConfig?.U_MSS_MAIL_PORT)) throw new Exception("Puerto no encontrado en la configuración del AddOn");

                // TODO c´parametrizar las conexiones
                //var fromAddress = !String.IsNullOrEmpty(oConfig.U_MSS_MAIL_DISP) ? new MailAddress(oConfig?.U_MSS_MAIL_EMAIL, oConfig.U_MSS_MAIL_DISP) : new MailAddress(oConfig?.U_MSS_MAIL_EMAIL);
                //var fromPassword = oConfig?.U_MSS_MAIL_PASS;

                var fromAddress =  new MailAddress(U_MSS_MAIL_EMAIL);
                var fromPassword = U_MSS_MAIL_PASS;

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
                var smtp = new SmtpClient(U_MSS_MAIL_HOST);
                smtp.Port = Convert.ToInt32(U_MSS_MAIL_PORT);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);
                smtp.EnableSsl = true;//Convert.ToBoolean(U_MSS_MAIL_SSL?.TrueOrFalse());
                //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.Send(message);
                //}
                //}

                result = true;
            }
            catch (Exception ex)
            {
                msgError = ex.Message;
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                
            }
            finally
            {
            }
            return result;
        }
    }
}
