using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TechTools.Exceptions;
using TechTools.Core.Base;
using System.Data.OracleClient;
using System.Data;

namespace TechTools.Core.Oracle9i
{
    public class BaseCore:TechTools.Core.Base.Base, ICore
    {
        public OracleConnection DbConnection;

        public void Connect()
        {
            //var tipoCpu = CPU.GetCpuType();
            DbConnection = new OracleConnection();
            DbConnection.ConnectionString = conf.Default.bddStringConnection;
            DbConnection.Open();
        }

        public void Disconect()
        {
            DbConnection.Close();
            DbConnection.Dispose();
        }
        /// <summary>
        /// Para sentencias sql tipo select
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        
        public DataTable GetDataTableByQuery(string sql)
        {
            try
            {
                Connect();
                var dt = new DataTable();
                var da = new OracleDataAdapter(sql, DbConnection);
                da.Fill(dt);
                da.Dispose();
                Disconect();
                return dt;
            }
            catch (Exception ex)
            {
                Disconect();
                ex = ExceptionManager.GetDeepErrorMessage(ex, ExceptionManager.eCapa.Core);
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", ex.Message, sql));
            }
        }
        public int GetIntScalarByQuery(string sql)
        {
            try
            {
                var ms = GetScalarByQuery(sql);
                return ms != null ? Convert.ToInt32(ms) : 0;
            }
            catch (Exception e)
            {
                e = ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Core);
                throw new Exception( string.Format("{0}\r\nSql:\r\n{1}", e.Message,sql));
            }
            
        }
        public string GetScalarByQuery(string sql)
        {
            try
            {
                Connect();
                var dt = new DataTable();
                var da = new OracleDataAdapter(sql, DbConnection);
                da.Fill(dt);
                da.Dispose();
                Disconect();
                return (dt.Rows.Count == 0 ? null : dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                Disconect();
                ex = ExceptionManager.GetDeepErrorMessage(ex, ExceptionManager.eCapa.Core);
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", ex.Message, sql));
            }
        }
        /// <summary>
        /// Para sentencias sql como insert, update, delete
        /// </summary>
        /// <param name="sql"></param>
        
        public void Execute(string sql)
        {
            try
            {
                sql = ReeplaceToAscci(sql);
                Connect();
                var command = new OracleCommand(sql, this.DbConnection);
                command.ExecuteReader();
                command.Dispose();
                Disconect();
            }
            catch (Exception e)
            {
                e = ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Core);
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", e.Message, sql));
            }

        }

        /// <summary>
        /// reemplaza los caracteres que se espesifiquen en el archivo de configuración
        /// por sus respectivos ascci
        /// </summary>
        /// <param name="sql">Ej; la Ñ por chr(209)</param>
        /// <returns></returns>
        private string ReeplaceToAscci(string sql)
        {
            //Ñ,209;Á,193;É,201;Í,205;Ó,211;Ú,218;ñ,241;á,225;é,233;í,237;ó,243;ú,250
            var matriz = conf.Default.acciReplace.Split(new char[] { ';' });
            if (matriz != null) {
                foreach (string vector in matriz) {
                    var token = vector.Split(new char[] { ','});
                    if (sql.Contains(token[0])) {
                        // sql = sql.Replace("Ñ", "'||chr(209)||'");
                        sql = sql.Replace(token[0], string.Format("'||chr({0})||'", token[1]));
                    }
                }
            }
            return sql;
        }

        public void Connect(string schema)
        {
            throw new NotImplementedException();
        }

        public DataTable GetDataTableByQuery(string sql, string schema)
        {
            throw new NotImplementedException();
        }
    }
}
