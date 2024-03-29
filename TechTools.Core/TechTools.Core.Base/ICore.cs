﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TechTools.Core.Base
{
    public interface ICore
    {
        /// <summary>
        /// Conecta al motor de base de datos
        /// </summary>
        void Connect(string schema);
        /// <summary>
        /// DesConecta del motor de base de datos
        /// </summary>
        void Disconect();
        /// <summary>
        /// Sentencias del timpo DML (crud)
        /// </summary>
        /// <param name="sql">Query a la base</param>
        void Execute(string sql);
        /// <summary>
        /// Consultas a la base de datos
        /// </summary>
        /// <param name="sql">Query a la base</param>
        /// <returns></returns>
        DataTable GetDataTableByQuery(string sql, string schema);
        int GetIntScalarByQuery(string sql);
        string GetScalarByQuery(string sql);
    }
}
