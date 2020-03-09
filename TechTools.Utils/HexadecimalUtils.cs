using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechTools.Utils
{
    public class HexadecimalUtils
    {
        public static string HexStringToString(string HexString)
        {
            string stringValue = "";
            for (int i = 0; i < HexString.Length / 2; i++)
            {
                string hexChar = HexString.Substring(i * 2, 2);
                int hexValue = Convert.ToInt32(hexChar, 16);
                stringValue += Char.ConvertFromUtf32(hexValue);
            }
            return stringValue;
        }
        
    }
}
