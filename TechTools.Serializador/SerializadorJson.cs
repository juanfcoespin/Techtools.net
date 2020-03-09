using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using TechTools.Utils;
using TechTools.Exceptions;

namespace TechTools.Serializador
{
    public class SerializadorJson
    {
        public static string Serializar(object objToSerialise)
        {
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return JsonConvert.SerializeObject(objToSerialise,settings);
            
        }
        public static void Serializar(object objToSerialise, string filePath)
        {
            string jsonValue = Serializar(objToSerialise);
            FileUtils.EscribirEnArchivo(filePath, jsonValue);
        }
        public static object Deserializar(Type type, string jsonValue)
        {
			if (jsonValue != null)
			{
				try
				{
					return JsonConvert.DeserializeObject(jsonValue, type);//da un error si es que fue serializado en xml
				}
				catch (Exception e)
				{
                    throw ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Utilities);
				}
			}
			return null;
        }
        public static object Deserializar(string filePathCatalogo, Type type)
        {
            string jsonValue = FileUtils.GetStrigText(filePathCatalogo);
            return Deserializar(type, jsonValue);
        }
        private static string ReplaceStringDateFunction(string me)
        {
            me = me.Replace("Date(", null);
            me = me.Replace(")", null);
            me = me.Replace("/", null);
            return me;
        }
    }
}
