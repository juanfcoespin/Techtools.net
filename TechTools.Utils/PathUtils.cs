using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTools.Utils
{
    public class PathUtils
    {
        public static string GetFileName(string filePath)
        {
            var matrix = filePath.Split(new char[] { '\\' });
            if (matrix.Length > 0)
                return matrix[matrix.Length - 1];
            return filePath;
        }
    }
}
