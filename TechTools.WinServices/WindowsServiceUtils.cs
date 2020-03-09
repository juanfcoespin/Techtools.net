using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceProcess;

namespace TechTools.WinServices
{
    public class WindowsServiceUtils
    {
        private ServiceController service;
        /// <summary>
        /// Controla start y Stop de un servicio de windows
        /// </summary>
        /// <param name="serviceName">Nombre del servicio no nombre a mostrar</param>
        public WindowsServiceUtils(string serviceName) {
            this.service = new ServiceController(serviceName);
        }
        public string StopService()
        {
            try
            {
                if (this.service != null && this.service.Status == ServiceControllerStatus.Running)
                {
                    // se espera máximo un minuto para que inicie el servicio
                    this.service.Stop();
                    this.service.WaitForStatus(ServiceControllerStatus.Stopped);
                    this.service.Close();
                    return "ok";
                }
                return service.Status.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string StartService()
        {
            try
            {
                if (this.service!=null && this.service.Status != ServiceControllerStatus.Running)
                {
                    // se espera máximo un minuto para que inicie el servicio
                    this.service.Start();
                    this.service.WaitForStatus(ServiceControllerStatus.Running);
                    this.service.Close();
                    return "ok";
                }
                return this.service.Status.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public bool IsRunning() {
            return this.service.Status == ServiceControllerStatus.Running ? true : false;
        }
    }
}
