using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace TechTools.Exceptions
{
    public class ExceptionManager
    {
        public enum eCapa { 
            Business,
            Presentation,
            PresentationUtilitites,
            PresentationViewModels,
            Proxy,
            Core,
            Utilities
        }
        public static Exception GetDeepErrorMessage(Exception e,eCapa capa,string errorUbication=null, bool firstEntry=true)
        {
            if(firstEntry)
                errorUbication=GetErrorUbication(capa);
            if (e.InnerException == null)
            {
                var errorDetail = string.Format("{0}\r\nError:\r\n{1}", errorUbication,e.Message);
                return new Exception(errorDetail);
            }
            return GetDeepErrorMessage(e.InnerException,capa,errorUbication,false);
        }

        private static string GetErrorUbication(eCapa capa)
        {
            var frame = new StackTrace();
            if (frame.FrameCount >= 2)
            {
                var frameInstance = frame.GetFrame(2);// 2 porque es en donde se dispara la invocación: 0 es GetErrorUbication; 1 es GetDeepErrorMessage
                var className = frameInstance.GetMethod().ReflectedType.Name;
                var functionName = frameInstance.GetMethod().ToString();
                var errorUbication = string.Format(
                    "Capa: {0}\r\nClase: {1}\r\nFunción: {2}\r\n",
                    capa, className, functionName
                );
                return errorUbication;
            }
            return null;
        }

        
    }
}
