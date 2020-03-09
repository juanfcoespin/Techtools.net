using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TechTools.Utils
{
    public class CodeUtils
    {
        public static string GetClassAndFunction()
        {
            var frame = new StackTrace();
            if (frame.FrameCount >= 2)
            {
                var frameInstance = frame.GetFrame(2);// 2 porque es en donde se dispara la invocación: 0 es GetErrorUbication; 1 es GetDeepErrorMessage
                var className = frameInstance.GetMethod().ReflectedType.Name;
                var functionName = frameInstance.GetMethod().ToString();
                var location = string.Format(
                    "Clase: {0}\r\nFunción: {1}\r\n",
                     className, functionName
                );
                return location;
            }
            return null;
        }
    }
}
