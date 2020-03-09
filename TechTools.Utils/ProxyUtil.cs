using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace TechTools.Utils
{
    public class ProxyUtil
    {
        public static bool ExistConectionToService(string urlService)
        {
            //Ej. urlService: http://pymeservices/Facturar.svc
            var host = GetHostFromUrlService(urlService);
            if (host != null) {
                return MakePing(host);
            }
            return false;
        }
        public static bool MakePing(string host)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(host);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
                return false;
            }
            return pingable;
        }
        private static string GetHostFromUrlService(string urlService)
        {
            //Ej. urlService: http://pymeservices/Facturar.svc
            //retorna: pymeservices
            var matriz = urlService.Split(new char[] { '/'});
            if (matriz.Length > 3) 
                return matriz[2];
            return null;
        }
    }
}
