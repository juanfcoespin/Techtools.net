using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using TechTools.Utils;

namespace TechTools.Net
{
    public class MailUtils
    {
        /// <summary>
        /// Send mail, check conf settings to set server and account parameters
        /// </summary>
        /// <param name="destinationMails">[,] or [;] destination delimiter Ex: jespin@yahoo.com,jespin@jbp.com.ec</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool Send(string destinationMails, string subject, string body, string attchFilePath=null)
        {
            try
            {
                SendWithOutReturn(destinationMails, subject, body, attchFilePath);
                return true;
            }
            catch(Exception e)
            {
                var errorMsg = e.Message;
                return false;
            }
        }
        public static bool SendAndGetError(ref string error, string destinationMails, string subject, string body, string attchFilePath = null)
        {
            try
            {
                SendWithOutReturn(destinationMails, subject, body, attchFilePath);
                return true;
            }
            catch(Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        private static void SendWithOutReturn(string destinationMails, string subject, string body, string attchFilePath = null)
        {
            
            if (string.IsNullOrEmpty(destinationMails))
                throw new Exception("No existe destinatarios del correo");
            if (string.IsNullOrEmpty(body))
                throw new Exception("El cuerpo del correo está vacío");
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(conf.Default.smtpMailServer);

            mail.From = new MailAddress(conf.Default.senderMail);
            var recipients = destinationMails.Split(new char[] { ',', ';' });
            var numRecipients = 0;
            foreach (var recipient in recipients)
            {
                var recipientWithOutWhiteSpace = recipient.Trim();
                if (!ValidacionUtils.EmailValid(recipientWithOutWhiteSpace))
                    throw new Exception(string.Format("El mail {0} es incorrecto, no se pudo enviar el correo a este destinatario", recipient));
                else
                {
                    mail.To.Add(recipientWithOutWhiteSpace);
                    numRecipients++;
                }
            }
            if (numRecipients == 0)
                throw new Exception("No existen destinatarios de correo válidos");
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpServer.Port = conf.Default.smtpPort;
            SmtpServer.EnableSsl = conf.Default.smtpSupportSSL;
            SmtpServer.Credentials = new System.Net.NetworkCredential(conf.Default.senderUser, conf.Default.senderPwd);
            
            if (attchFilePath != null)
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(attchFilePath);
                mail.Attachments.Add(attachment);
            }
            SmtpServer.Send(mail);
        }
    }
}
