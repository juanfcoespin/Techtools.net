﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using TechTools.Utils;
using System.Net;

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
        public static bool Send(string destinationMails, string subject, string body, List<string> attchFilePaths=null)
        {
            try
            {
                SendWithOutReturn(destinationMails, subject, body, attchFilePaths);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public static object Send(object to, object subject, object body)
        {
            throw new NotImplementedException();
        }

        public static bool SendAndGetError(ref string error, string destinationMails, string subject, string body, List<string> attchFilePaths = null)
        {
            try
            {
                SendWithOutReturn(destinationMails, subject, body, attchFilePaths);
                return true;
            }
            catch(Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        private static void SendWithOutReturn(string destinationMails, string subject, string body, List<string> attchFilePaths = null)
        {
            
            if (string.IsNullOrEmpty(destinationMails))
                throw new Exception("No existe destinatarios del correo");
            if (string.IsNullOrEmpty(body))
                throw new Exception("El cuerpo del correo está vacío");
            MailMessage mail = new MailMessage();

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
            if (attchFilePaths != null)
            {
                attchFilePaths.ForEach(file => {
                    mail.Attachments.Add(new System.Net.Mail.Attachment(file));
                });
            }

            var smtpServer = conf.Default.smtpMailServer;
            var port = conf.Default.smtpPort;
            var user = conf.Default.senderUser;
            var pwd = conf.Default.senderPwd;
            var ssl= conf.Default.smtpSupportSSL;

            /*var smtpServer = "smtp.office365.com";
            var port = 587;
            var user = "sistemas@jbp.com.ec";
            var pwd = "Ph4rm4Br0wn$Adm1n%";//"xxvddxytmycsdzhk";
            var ssl = true;*/

            SmtpClient clienteSmtp = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(user, pwd),
                EnableSsl = ssl // Activa TLS (SSL)
            };
            // Enviar el correo
            string ms = null;
            clienteSmtp.Send(mail);
        }
        private static void SendWithOutReturnBk(string destinationMails, string subject, string body, List<string> attchFilePaths = null)
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
            if (attchFilePaths != null)
            {
                attchFilePaths.ForEach(file => {
                    mail.Attachments.Add(new System.Net.Mail.Attachment(file));
                });
            }

            SmtpServer.Port = conf.Default.smtpPort;
            SmtpServer.EnableSsl = conf.Default.smtpSupportSSL;
            SmtpServer.Credentials = new System.Net.NetworkCredential(conf.Default.senderUser, conf.Default.senderPwd);


            SmtpServer.Send(mail);
        }
    }
}
