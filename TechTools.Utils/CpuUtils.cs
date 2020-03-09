using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTools.Utils
{
    
    public class CpuUtils
    {
        public enum eCpuType
        {
            Run32Bits,
            Run64Bits,
            NotDefined
        }
        public static eCpuType GetCpuType() {
            if (IntPtr.Size == 4)
                return eCpuType.Run32Bits;
            if (IntPtr.Size == 8)
                return eCpuType.Run64Bits;
            return eCpuType.NotDefined;
        }
    }
}
