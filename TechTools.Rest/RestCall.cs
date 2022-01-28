using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization.Json;
using System.Net;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.ServiceModel.Web;
using TechTools.Serializador;
using TechTools.Exceptions;

namespace TechTools.Rest
{
    public class RestCall
    {
        public object Result = null;
        public string ErrorMessage = null;
        public class AuthenticationMe {
            public string User { get; set; }
            public string Pwd { get; set; }
            public eAuthType AuthType { get; set; }
        }
        public enum eRestMethod { 
            PUT,
            GET,
            POST
        }
        public enum eAuthType {
            Basic,
            NTLM
        }
        #region events & delegates
        public delegate void dDataArrived(object result, string errorMessage);
        public event dDataArrived DataArrived;
        #endregion

        /// <summary>
        /// Cuando se requiere enviar como parámetro un objeto complejo
        /// </summary>
        /// <param name="urlService"></param>
        /// <param name="parameter"> El objeto a pasarse como parámetro</param>
        /// <param name="resultType"></param>
        /// <returns></returns>
        public  void SendPostOrPutAsync(string urlService, Type resultType,object parameter,Type parameterType, eRestMethod typeRest, AuthenticationMe auth = null)
        {
            if (typeRest == eRestMethod.GET)
                throw new Exception("Only Put or Post method is allowed");
            RunAsync(urlService,resultType, typeRest, parameterType, parameter, auth);
        }
        
        public  void SendGetAsync(string urlServiceWithParameters, Type typeOfResult, AuthenticationMe auth = null)
        {
            RunAsync(urlServiceWithParameters, typeOfResult, eRestMethod.GET,null,null,auth);
        }
        private  void RunAsync(string urlService, Type resultType, eRestMethod TypeSend,Type parameterType=null, object parameter = null, AuthenticationMe auth = null)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += delegate {
                ExecuteRest(urlService, resultType, TypeSend, parameterType, parameter,auth);
            };
            worker.RunWorkerCompleted +=(s,e)=> DataArrived?.Invoke(Result, ErrorMessage);
            worker.RunWorkerAsync();
        }
        private  void ExecuteRest(string urlService, Type resultType, eRestMethod typeRest, Type parameterType, object parameter, AuthenticationMe auth = null)
        {
            try
            {
                Result = null;
                Result=(typeRest==eRestMethod.GET)? 
                    SendGet(urlService, resultType,auth):
                    SendPostOrPut(urlService, resultType, parameter, parameterType, typeRest,auth);
            }
            catch (Exception e)
            {
                ErrorMessage =
                    ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.PresentationUtilitites).Message;
            }
        }
        public object SendGet(string url, Type resultType, AuthenticationMe auth=null)
        {
            try
            {
                ErrorMessage = null;
                DataContractJsonSerializer serializador;
                var request = GetRequest(url,eRestMethod.GET,auth);
                var response = request.GetResponse();
                serializador = new DataContractJsonSerializer(resultType);
                var resultStream = response.GetResponseStream();
                return ReturnValueFromStream(resultType, resultStream);
            }
            catch (Exception e)
            {
                e = ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Utilities);
                ErrorMessage = e.Message;
                return null;
            }
            
        }

        

        public object SendPostOrPut(string urlService, Type resultType, object parameter, Type parameterType, eRestMethod method, AuthenticationMe auth=null)
        {
            try
            {
                ErrorMessage = null;
                var serializador = new DataContractJsonSerializer(parameterType);
                var request = GetRequest(urlService, method, auth);
                request.ContentType = "application/json";
                
                serializador.WriteObject(request.GetRequestStream(), parameter);
                var response = request.GetResponse();
                return ReturnValueFromStream(resultType, response.GetResponseStream());
            }
            catch (Exception e)
            {
                e = ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Utilities);
                ErrorMessage = e.Message + ", parametro: "+ SerializadorJson.Serializar(parameter); ;
                return null;
            }

        }
        private static WebRequest GetRequest(string url, eRestMethod method, AuthenticationMe auth)
        {
            var request = WebRequest.Create(url);
            request.Method = method.ToString();
            if (auth != null)
            {
                var authHeader = Convert.ToBase64String(
                    ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", auth.User, auth.Pwd)));
                request.Headers.Add("Authorization", auth.AuthType.ToString() + " " + authHeader);
            }
            return request;
        }
        private  void WriteStream(ref WebRequest request, string jsonValue)
        {
            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(jsonValue);
            }
        }
        private  object ReturnValueFromStream(Type typeOfResult, System.IO.Stream resultStream)
        {
            string value = GetStringJsonFromStream(resultStream);
            if (string.IsNullOrEmpty(value))
                return null;
            else
                try
                {
                    if (typeOfResult == typeof(string))
                        return value;
                    return SerializadorJson.Deserializar(typeOfResult, value);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("No se pudo deserealizar el objeto con el formato JSON:\r\n{0} {1}",value,e.Message));
                }
                
        }
        private  string GetStringJsonFromStream(Stream stream)
        {
            //stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        
    }
}
