﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTools.Core.Base
{
    public interface IEntity<T>:ICore
    {
        
        List<T> GetListByQuery(string sql);
    }
}
