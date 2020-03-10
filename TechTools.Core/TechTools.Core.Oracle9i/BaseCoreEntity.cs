using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OracleClient;
using TechTools.Core.Base;
using System.Data;

namespace TechTools.Core.Oracle9i
{
    public class BaseCoreEntity<T>:BaseCore,IEntity<T>
    {
        public delegate List<T> dTranslateDataToList(DataTable dt);
        public event dTranslateDataToList TranslateDataToListEvent;
        public BaseCoreEntity():base() {}
        /// <summary>
        /// Para sentencias sql tipo select
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public  List<T> GetListByQuery(string sql)
        {
            if (TranslateDataToListEvent == null)
                throw new Exception("No se ha implementado el método TranslateDataToListEvent ");
            return TranslateDataToListEvent.Invoke(GetDataTableByQuery(sql));
        }
    }
}
