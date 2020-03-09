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
        public static bool Send(string destinationMails, string subject, string body)
        {
            try
            {
                if (string.IsNullOrEmpty(destinationMails))
                    throw new Exception("No existe destinatarios del correo");
                if(string.IsNullOrEmpty(body))
                    throw new Exception("El cuerpo del correo está vacío");
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(conf.Default.smtpMailServer);

                mail.From = new MailAddress(conf.Default.senderMail);
                var recipients = destinationMails.Split(new char[] { ',',';'});
                var numRecipients = 0;
                foreach (var recipient in recipients) {
                    var recipientWithOutWhiteSpace = recipient.Trim();
                    if (!ValidacionUtils.EmailValid(recipientWithOutWhiteSpace))
                        throw new Exception(string.Format("El mail {0} es incorrecto, no se pudo enviar el correo a este destinatario",recipient));
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
                SmtpServer.Credentials = new System.Net.NetworkCredential(conf.Default.senderUser, conf.Default.senderPwd);
                SmtpServer.EnableSsl = conf.Default.smtpSupportSSL;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                var msg = string.Format("No se pudo enviar el correo error: {0}",ex.Message);
                throw new Exception(msg);
            }
        }
    }
}
