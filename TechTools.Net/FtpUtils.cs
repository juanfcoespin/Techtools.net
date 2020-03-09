using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TechTools.DelegatesAndEnums;
using TechTools.Utils;

namespace TechTools.Net
{
    public class FtpUtils
    {
        public event dStringParameter MessageEnvent;
        public class Credencials {
            public string Url { get; set; }
            public string User { get; set; }
            public string Pwd { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credenciales"></param>
        /// <param name="filePath">
        /// You must past parameter on format like example:  c:\\folder\file.ext or @c:\folder\file.ext
        /// </param>
        public static bool UploadFile(Credencials credenciales, string filePath) {
            using (var client = new WebClient())
            {
                try{
                    var fileName = PathUtils.GetFileName(filePath);
                    var target = string.Format("{0}/{1}", credenciales.Url, fileName);
                    client.Credentials = new NetworkCredential(credenciales.User, credenciales.Pwd);
                    client.UploadFile(target, WebRequestMethods.Ftp.UploadFile, filePath);
                    return true;
                }
                catch{
                    return false;
                }
            }
        }

        public List<string> GetFileNamesFromPath(Credencials cred) {
            var ms = new List<string>();
            try
            {
                var request = GetRequestByMethod(cred, WebRequestMethods.Ftp.ListDirectory);
                var reader = new StreamReader(request.GetResponse().GetResponseStream());
                if (reader != null)
                {
                    var filesString = reader.ReadToEnd();
                    reader.Close();
                    var filesVector = filesString.Split(new char[] { '\r', '\n' });
                    foreach (var file in filesVector)
                    {
                        // A veces trae todo el path del archivo
                        var fileOnly = RemoveFilePath(file);
                        //que el archivo tenga una extensión una longitud minima de 3 caracteres
                        if (!string.IsNullOrEmpty(fileOnly) && fileOnly.Length > 3)
                        {
                            MessageEnvent?.Invoke(string.Format("Encontrado Archivo: {0}", fileOnly));
                            ms.Add(fileOnly);
                        }
                    }
                }
                request.Abort();
            }
            catch (Exception e)
            {
                MessageEnvent?.Invoke(string.Format("Error: {0}", e.Message));
            }
            return ms;
        }
        /// <summary>
        /// Ej: /REPORTES/CER_20190321_175738.TXT
        /// </summary>
        /// <param name="fileWithPaht">Ej: /REPORTES/CER_20190321_175738.TXT</param>
        /// <returns>Ej: CER_20190321_175738.TXT</returns>
        private string RemoveFilePath(string fileWithPath)
        {
            var vecFile = fileWithPath.Split(new char[] {'/'});
            return vecFile[vecFile.Length - 1];
        }

        /// <summary>
        /// Elimina un archivo en una ubicación FTP
        /// </summary>
        /// <param name="cred">en la ruta (cred.Url) solo se debe espesificar la carpeta contenedora del archivo</param>
        /// <returns></returns>
        public bool DeleteFile(Credencials cred, string fileName) {
            try
            {
                var request = GetRequestByMethod(cred, WebRequestMethods.Ftp.DeleteFile,fileName);
                MessageEnvent?.Invoke(string.Format("Eliminando archivo: {0}", fileName));
                var response = (FtpWebResponse) request.GetResponse();
                MessageEnvent?.Invoke(string.Format("respuesta del servidor: {0}", response.StatusDescription));
                response.Close();
                request.Abort();
                return true;
            }
            catch(Exception e)
            {
                MessageEnvent?.Invoke(string.Format("Error: {0}", e.Message));
                return false;
            }
        }
        private FtpWebRequest GetRequestByMethod(Credencials cred, string method, string fileName=null) {
            var url = (fileName == null) ? cred.Url : string.Format("{0}/{1}", cred.Url, fileName);
            var request = (FtpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = method;
            request.Credentials = new NetworkCredential(cred.User, cred.Pwd);
            return request;
        }
        
    }
}
