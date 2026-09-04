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
        public void Execute(string sql, Dictionary<string, object> parameters)
        {
            try
            {
                Connect();
                ExecuteQuery(sql,parameters);
                Disconect();
            }
            catch (Exception e)
            {

                throw new Exception(sql+e.Message+e.StackTrace);
            }
            
        }
        private void ExecuteQuery(string sql, Dictionary<string, object> parameters)
        {
            using (var cm = new HanaCommand(sql, this.cn))
            {
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        cm.Parameters.AddWithValue(p.Key, p.Value);
                    }
                }

                cm.ExecuteNonQuery();
            }
        }
        private void SetSchema(string schema=null) // no need to be closed because is called with Connect() function
        {
            if (schema == null)
                schema = conf.Default.DefaultSchema;
            var sql = string.Format("set schema \"{0}\";", schema);
            ExecuteQuery(sql,null);
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
        /*public DataTable GetDataTableByQuery(string sql, Dictionary<string, object> parameters, string schema=null)
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
        }*/
        public DataTable GetDataTableByQuery(string sql, Dictionary<string, object> parameters, string schema = null)
        {
            try
            {
                Connect(schema);

                using (var cmd = new HanaCommand(sql, this.cn))
                {
                    // 🔒 Agregar parámetros de forma segura
                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                        {
                            cmd.Parameters.AddWithValue(p.Key, p.Value);
                        }
                    }
                    using (var da = new HanaDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error ejecutando query: {sql}\n{e.Message+e.StackTrace}");
            }
            finally
            {
                Disconect();
            }
        }
        public int GetIntScalarByQuery(string sql, Dictionary<string, object> parametros)
        {
            try
            {
                var dt = this.GetDataTableByQuery(sql, parametros);
                if (dt == null || dt.Rows.Count == 0)
                    return 0;
                return this.GetInt(dt.Rows[0][0]);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", e.Message, sql));
            }
        }
        public decimal GetDecimalScalarByQuery(string sql, Dictionary<string, object> parametros)
        {
            try
            {
                var dt = this.GetDataTableByQuery(sql, parametros);
                if (dt == null || dt.Rows.Count == 0)
                    return 0m;
                return this.GetDecimal(dt.Rows[0][0]);
            }
            catch (Exception e)
            {
                e = ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Core);
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", e.Message, sql));
            }
        }
        public double GetDoubleScalarByQuery(string sql, Dictionary<string, object> parametros)
        {
            try
            {
                var dt = this.GetDataTableByQuery(sql, parametros);
                if (dt == null || dt.Rows.Count == 0)
                    return 0d;
                return this.GetDouble(dt.Rows[0][0]);
            }
            catch (Exception e)
            {
                e = ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Core);
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", e.Message, sql));
            }
        }
        public string GetScalarByQuery(string sql, Dictionary<string,object> parametros)
        {
            try
            {
                var dt = this.GetDataTableByQuery(sql, parametros);
                if (dt == null || dt.Rows.Count==0)
                    return null;
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                Disconect();
                ex = ExceptionManager.GetDeepErrorMessage(ex, ExceptionManager.eCapa.Core);
                throw new Exception(string.Format("{0}\r\nSql:\r\n{1}", ex.Message, sql));
            }
        }

        /*****          INICIO TRANSACCIONES          *****/
        // esta seccion es para poder usar transacciones, si algo falla entonces se podra hacer rollback
        private HanaTransaction transaction;
        public void BeginTransaction(){
            if (cn == null || cn.State != ConnectionState.Open)
                Connect();
            Connect();
            transaction = cn.BeginTransaction();
        }

        public void Commit(){
            transaction?.Commit();
            transaction = null;
            Disconect();
        }

        public void ExecuteQueryTransaction(string sql, Dictionary<string, object> parameters)
        {
            try{
                if (cn == null || cn.State != ConnectionState.Open)
                    throw new Exception("No existe una conexión activa.");

                if (transaction == null)
                    throw new Exception("No existe una transacción activa.");

                using (var cm = new HanaCommand(sql, this.cn))
                {
                    if (transaction != null)
                        cm.Transaction = transaction;

                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                        {
                            cm.Parameters.AddWithValue(p.Key, p.Value);
                        }
                    }

                    cm.ExecuteNonQuery();
                }
            }
            catch (Exception e){
                throw new Exception($"Error ejecutando query:\r\n{sql}\r\n{e.Message}", e);
            }
        }

        public DataTable GetDataTableTransaction(string sql, Dictionary<string, object> parameters)
        {
            try{
                if (cn == null || cn.State != ConnectionState.Open)
                    throw new Exception("No existe una conexión activa.");

                using (var cmd = new HanaCommand(sql, this.cn)){
                    if (transaction != null)
                        cmd.Transaction = transaction;

                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                        {
                            cmd.Parameters.AddWithValue(p.Key, p.Value);
                        }
                    }

                    using (var da = new HanaDataAdapter(cmd)){
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception e){
                throw new Exception(
                    $"Error ejecutando query:\r\n{sql}\r\n{e.Message}",
                    e);
            }
        }

        public string GetScalarTransaction(string sql, Dictionary<string, object> parametros){
            var dt = GetDataTableTransaction(sql, parametros);

            if (dt == null || dt.Rows.Count == 0)
                return null;

            return dt.Rows[0][0]?.ToString();
        }
        public int GetIntScalarTransaction(string sql, Dictionary<string, object> parametros){
            var dt = GetDataTableTransaction(sql, parametros);
            if (dt == null || dt.Rows.Count == 0)
                return 0;
            return GetInt(dt.Rows[0][0]);
        }

        public void Rollback(){
            transaction?.Rollback();
            transaction = null;
            Disconect();
        }

        /*****          FIN TRANSACCIONES          *****/
    }
    }
