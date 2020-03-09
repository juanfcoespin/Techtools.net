using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using TechTools.Msg;
using TechTools.DelegatesAndEnums;
using TechTools.Serializador;
using TechTools.Utils;
using TechTools.Exceptions;

namespace TechTools.Logs
{
    public class LogUtils
    {
        /// <summary>
        /// Agrega log en archivo plano
        /// </summary>
        /// <param name="msg"></param>
        public void AddLog(string msg)
        {
            try
            {
                string logFileName = GetLogFileName();
                FileUtils.AddDataToFile(logFileName, DateTime.Now.ToString() + ": " + msg); //+ Environment.NewLine);
            }
            catch {}
        }
        /// <summary>
        /// Agrega log en formato JSON
        /// </summary>
        /// <param name="me"></param>
        public  void AddLog(LogMsg me)
        {
            try
            {
                var logs = new List<LogMsg>();
                Monitor.Enter(logs);
                var logFile = GetLogFileName(true);
                if (System.IO.File.Exists(logFile))
                    logs = (List<LogMsg>)SerializadorJson.Deserializar(logFile, typeof(List<LogMsg>));

                var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                me.date = currentDate;
                if (!logs.Exists(p => p.date == me.date && p.msg == me.msg && p.type == me.type))
                {
                    logs.Add(me);
                    SerializadorJson.Serializar(logs, logFile);
                }
                Monitor.Exit(logs);
            }
            catch (Exception e)
            {
                e = ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Utilities);
                AddLog("error: " + e.Message);
            }

        }
        /// <summary>
        /// Get the logs location
        /// </summary>
        /// <param name="jsonExtension">when true <pahtlog>/log.json otherwise <pahtlog>/log.txt</param>
        /// <returns></returns>
        public  string GetLogFileName(bool jsonExtension = false)
        {
            var ms= GetLogFileNameByDate(
                DateTime.Now.Year.ToString(),
                StringUtils.getTwoDigitNumber(DateTime.Now.Month),
                StringUtils.getTwoDigitNumber(DateTime.Now.Day)
                , jsonExtension);
            return ms;
        }
        private  string GetLogFileNameByDate(string year, string month, string day, bool jsonExtension = false)
        {
            var ms = String.Format(@"{0}\logs\{1}\Log_{2}.txt",
                    conf.Default.logsPathFolder,
                    string.Format("{0}-{1}", year, month),
                    string.Format("{0}-{1}-{2}", year, month, day)
              );
            if (jsonExtension)
                ms = ms.Replace(".txt", ".json");
            return ms;

        }
        public void AddError(string msg)
        {
            AddLog("******************** Error ******************");
            AddLog(msg);
            AddLog("****************** Fin Error ****************");
            Show(msg);
        }
        public void AddLogAndShow(string msg)
        {
            AddLog(msg);
            Show(msg);
        }
        public  void Show(string msg)
        {
            Console.WriteLine(msg);
        }
        ///// <summary>
        ///// Get logs of particular day
        ///// </summary>
        ///// <param name="me">Date must be in format: yyyy-mm-dd</param>
        ///// <returns></returns>
        public  List<LogMsg> GetLogsByDate(DateTime me)
        {
            var ms = new List<LogMsg>();
            try
            {
                if (me != null)
                {
                    string fileName = GetFileNameByDateString(me.ToString("yyyy-MM-dd"));
                    if (System.IO.File.Exists(fileName))
                        ms = (List<LogMsg>)SerializadorJson.Deserializar(fileName, typeof(List<LogMsg>));
                }
            }
            catch (Exception e)
            {
                e = ExceptionManager.GetDeepErrorMessage(e, ExceptionManager.eCapa.Utilities);
                ms.Add(new LogMsg { type = eTypeLog.Error, msg = e.Message });
            }
            return ms;
        }
        ///// <summary>
        ///// Get log file name and location
        ///// </summary>
        ///// <param name="me">Must be on format yyyy-mm-dd</param>
        ///// <returns></returns>
        public  string GetFileNameByDateString(string me)
        {
            var matrixDate = me.Split(new char[] { '-' });
            if (matrixDate.Length == 3)
            {
                var year = matrixDate[0];
                var month = matrixDate[1];
                var day = matrixDate[2];
                return GetLogFileNameByDate(year, month, day, true);
            }
            return null;
        }
    }
}
