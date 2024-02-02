using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TechTools.Core.Base;
using Sap.Data.Hana;
using System.Data;
using TechTools.Exceptions;

namespace TechTools.Core.Hana
{
    public class BaseCore : TechTools.Core.Base.Base, ICore
    {
        private HanaConnection cn;
        public void Connect(string schema=null)
        {
            this.cn = new HanaConnection(conf.Default.ConnectionString);
            this.cn.Open();
            SetSchema(schema);
        }
        public static string GetBddName()
        {
            return conf.Default.DefaultSchema;
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
            Disconect();
        }
        private void ExecuteQuery(string sql)
        {
            var cm = new HanaCommand(sql, this.cn);
            cm.ExecuteNonQuery();
        }
        private void SetSchema(string schema=null) // no need to be closed because is called with Connect() function
        {
            if (schema == null)
                schema = conf.Default.DefaultSchema;
            var sql = string.Format("set schema \"{0}\";", schema);
            ExecuteQuery(sql);
        }
        public string GetBooleanSAP(bool me)
        {
            switch (me)
            {
                case true:
                    return "1";
                case false:
                    return "2";
            }
            return "null";
        }
        public DataTable GetDataTableByQuery(string sql, string schema=null)
        {
            try
            {
                Connect(schema);
                var da = new HanaDataAdapter(sql, this.cn);
                var ms = new DataTable();
                da.Fill(ms);
                Disconect();
                if (ms == null)
                    ms = new DataTable();
                return ms;
            }
            catch (Exception e)
            {
                Disconect();
                throw new Exception(e.Message+" "+sql);
            }
        }
        public int GetIntScalarByQuery(string sql)
        {
            try
            {
                return this.GetInt(this.GetScalarObjectByQuery(sql));
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", e.Message, sql));
            }
        }
        public decimal GetDecimalScalarByQuery(string sql)
        {
            try
            {
                return this.GetDecimal(this.GetScalarObjectByQuery(sql));
            }
            catch (Exception e)
            {
                e = ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Core);
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", e.Message, sql));
            }
        }
        public double GetDoubleScalarByQuery(string sql)
        {
            try
            {
                return this.GetDouble(this.GetScalarObjectByQuery(sql));
            }
            catch (Exception e)
            {
                e = ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Core);
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", e.Message, sql));
            }
        }
        public string GetScalarByQuery(string sql)
        {
            try
            {
                var ms = this.GetScalarObjectByQuery(sql);
                if (ms == null)
                    return null;
                return ms.ToString();
            }
            catch (Exception ex)
            {
                Disconect();
                ex = ExceptionManager.GetDeepErrorMessage(ex, ExceptionManager.eCapa.Core);
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", ex.Message, sql));
            }
        }
        public dynamic GetScalarObjectByQuery(string sql)
        {
            try
            {
                Connect();
                var dt = new DataTable();
                var da = new HanaDataAdapter(sql, this.cn);
                da.Fill(dt);
                da.Dispose();
                Disconect();
                return (dt.Rows.Count == 0 ? null : dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                Disconect();
                throw;
            }
        }
    }
}
