using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
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
        public static bool Send(string destinationMails, string subject, string msg, ref string error, List<string> attachFilePaths = null) {
            try
            {
                var smtpServer = conf.Default.smtpMailServer;
                var port = conf.Default.smtpPort;
                var pwd = conf.Default.senderPwd;
                var senderMail = conf.Default.senderMail;
                

                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress("JB", senderMail));
                mensaje.Subject = subject;
                var destinatarios = destinationMails.Split(new char[] { ',', ';' });
                var correosCorrectos = 0;
                foreach (var destinatario in destinatarios)
                {
                    if (!string.IsNullOrEmpty(destinatario)) {
                        var correo = destinatario.Trim();
                        if (!string.IsNullOrEmpty(correo)) {
                            mensaje.To.Add(MailboxAddress.Parse(destinatario));
                            correosCorrectos++;
                        }
                        
                    }
                }
                if (correosCorrectos == 0)
                    throw new Exception("No existe ningun correo correcto para enviar en:" + destinationMails);
                var htmlPart = new TextPart("html") { Text = msg };
                var body = new Multipart("mixed");
                body.Add(htmlPart);
                if (attachFilePaths != null) {
                    attachFilePaths.ForEach(path => {
                        var adjunto = new MimePart()
                        {
                            Content = new MimeContent(System.IO.File.OpenRead(path)),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = System.IO.Path.GetFileName(path)
                        };
                        body.Add(adjunto);
                    });
                }
                mensaje.Body = body;
                
                using (var cliente = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Evita el error de validación del certificado (solo si confías en el servidor)
                    cliente.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
                    cliente.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    cliente.CheckCertificateRevocation = false;
                    cliente.Connect(smtpServer, port, SecureSocketOptions.StartTls);
                    cliente.Authenticate(senderMail, pwd);
                    cliente.Send(mensaje);
                    cliente.Disconnect(true);
                    return true;
                }
            }
            catch (Exception e)
            {
                error=e.Message + e.StackTrace;
                return false;
            }
        }
    }
}
