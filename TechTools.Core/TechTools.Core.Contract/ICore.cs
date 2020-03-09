using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTools.Core.Contract
{
    public interface ICore
    {
        /// <summary>
        /// Conecta al motor de base de datos
        /// </summary>
        void Connect();
        /// <summary>
        /// DesConecta del motor de base de datos
        /// </summary>
        void Disconect();
        /// <summary>
        /// Sentencias del timpo DML (crud)
        /// </summary>
        /// <param name="sql">Query a la base</param>
        void Execute(string sql);
        int GetIntScalarByQuery(string sql);
        string GetScalarByQuery(string sql);
    }
}
