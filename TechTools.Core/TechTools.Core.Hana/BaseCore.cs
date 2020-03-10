using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TechTools.Core.Base;
using Sap.Data.Hana;
using System.Data;

namespace TechTools.Core.Hana
{
    public class BaseCore : TechTools.Core.Base.Base, ICore
    {
        private HanaConnection cn;
        public void Connect()
        {
            this.cn = new HanaConnection(conf.Default.ConnectionString);
            this.cn.Open();
            SetSchema();
        }

        public void Disconect()
        {
            if (this.cn != null)
                this.cn.Close();
        }

        public void Execute(string sql)
        {
            Connect();
            ExecuteQuery(sql);
        }

        private void ExecuteQuery(string sql)
        {
            var cm = new HanaCommand(sql, this.cn);
            cm.ExecuteNonQuery();
        }

        private void SetSchema()
        {
            var sql = string.Format("set schema \"{0}\";",conf.Default.DefaultSchema);
            ExecuteQuery(sql);
        }

        public DataTable GetDataTableByQuery(string sql)
        {
            Connect();
            var da = new HanaDataAdapter(sql, this.cn);
            var ms = new DataTable();
            da.Fill(ms);
            Disconect();
            if(ms==null)
                ms = new DataTable();
            return ms;
        }
        public int GetIntScalarByQuery(string sql)
        {
            throw new NotImplementedException();
        }

        public string GetScalarByQuery(string sql)
        {
            throw new NotImplementedException();
        }
    }
}
