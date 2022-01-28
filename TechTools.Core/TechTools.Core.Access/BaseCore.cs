using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

using TechTools.Core.Base;

namespace TechTools.Core.Access
{
    public class BaseCore:TechTools.Core.Base.Base, ICore
    {
        private OleDbConnection cn;
        private string _BddPath;
        public BaseCore() {
            InitBdd(conf.Default.bddPath);
        }
        public BaseCore(string bddPath)
        {
            InitBdd(bddPath);
        }
        public void InitBdd(string bddPath) {
            _BddPath = bddPath;
            this.cn = new OleDbConnection();
            this.cn.ConnectionString =
                string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data source={0}", this._BddPath);
        }
        public void Execute(string sql)
        {
            try
            {
                Connect();
                var command = new OleDbCommand(sql, this.cn);
                command.ExecuteReader();
                command.Dispose();
                Disconect();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", e.Message, sql));
            }

        }
        public DataTable GetDataTableByQuery(string sql)
        {
            try
            {
                Connect();
                var dt = new DataTable();
                var da = new OleDbDataAdapter(sql, this.cn);
                da.Fill(dt);
                da.Dispose();
                Disconect();
                return dt;
            }
            catch (Exception ex)
            {
                Disconect();
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", ex.Message, sql));
            }
        }
        public void Disconect()
        {
            this.cn.Close();
        }
        public void Connect()
        {
            this.cn.Open();   
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
