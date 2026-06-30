using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TechTools.Utils
{
    public static class Logger
    {
        private static readonly object _lock = new object();
        private static string _logPath;

        static Logger()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string logDir = Path.Combine(basePath, "log");

                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                string fileName = $"log_{DateTime.Now:yyyyMMdd}.txt";
                _logPath = Path.Combine(logDir, fileName);
            }
            catch
            {
                _logPath = "fallback_log.txt";
            }
        }

        public static void Info(string message)
        {
            Write("INFO", message);
        }

        public static void Error(string message)
        {
            Write("ERROR", message);
        }

        public static void Error(Exception ex)
        {
            Write("ERROR", ex.ToString());
        }

        private static void Write(string level, string message)
        {
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(_logPath,
                        $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}{Environment.NewLine}",
                        Encoding.UTF8);
                }
            }
            catch
            {
                // evitar crash por logger
            }
        }
    }
}
