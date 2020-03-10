using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TechTools.Core.Base
{
    public class Base
    {
        public DateTime GetDateTime(object me)
        {
            if (DBNull.Value.Equals(me))
            {
                return DateTime.MinValue;
            }
            return Convert.ToDateTime(me);
        }
        public bool GetBoolean(object me)
        {
            if (DBNull.Value.Equals(me))
            {
                return false;
            }
            return Convert.ToBoolean(me);
        }
        public Int32 GetInt(object me)
        {
            if (DBNull.Value.Equals(me))
            {
                return 0;
            }
            return Convert.ToInt32(me);
        }
        public decimal GetDecimal(object me)
        {
            if (DBNull.Value.Equals(me))
            {
                return 0;
            }
            var ms = Convert.ToDecimal(me);
            return decimal.Round(ms, 2);
        }
        /// <summary>
        /// Ej: token: "juan francisco espin"
        /// campos: {"ruc", "nombre", "conocidoComo"}
        /// putAndFirst: true
        /// 
        /// Resultado:
        ///and
        ///(
        /// (lower(nombre) like '%espin%' and lower(nombre) like '%juan%' and lower(nombre) like '%francisco%') or
        /// (lower(ruc) like '%espin%' and lower(ruc) like '%juan%' and lower(ruc) like '%francisco%') or
        /// (lower(CONOCIDOCOMO) like '%espin%' and lower(CONOCIDOCOMO) like '%juan%' and lower(CONOCIDOCOMO) like '%francisco%')
        ///)
        /// </summary>
        /// <param name="putAndFist">pone and al inicio si existe una condición where previa</param>
        /// <param name="campos">listado de campos en el query a evaluar</param>
        /// <param name="token">el valor registrado por el usuario para la búsqueda</param>
        /// <returns></returns>
        public string GetSearchCondition(bool putAndFist, List<string> campos, string token)
        {
            token = token.ToLower();
            var ms = putAndFist ? " and (" : string.Empty;
            var matrixToken = token.Split(new char[] { ' ' });
            var i = 0;
            var j = 0;
            campos.ForEach(campo => {
                if (i > 0)
                    ms += " or ";
                ms += "(";
                foreach (var miniToken in matrixToken)
                {
                    if (!string.IsNullOrEmpty(miniToken))
                    {
                        if (j > 0)
                            ms += " and ";
                        ms += string.Format(" lower({0}) like '%{1}%' ", campo, miniToken);
                        j++;
                    }

                }
                ms += ")";
                i++;
                j = 0;
            });
            ms += ")";

            return ms;
        }
    }
}
