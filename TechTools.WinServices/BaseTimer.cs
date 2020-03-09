using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Timers;
using TechTools.DelegatesAndEnums;

namespace TechTools.WinServices
{
    public class BaseTimer
    {
        public class InitAt
        {
            public int Hour { get; set; }
            public int Minute { get; set; }
        }
        public InitAt initAt;
        private Timer timer;
        public long loopOnSeconds;
        public event dVoid ProcessEvent;

        public BaseTimer()
        {
            this.timer = new Timer();
        }
        ~BaseTimer()
        {
            if (IsRunning())
            {
                Stop();
            }
        }
        /// <summary>
        /// Inicia el timer
        /// </summary>
        /// <param name="loopOnSeconds"></param>
        public virtual void Start(long loopOnSeconds)
        {
            if (!IsRunning())
            {
                this.loopOnSeconds = loopOnSeconds;
                this.timer = new Timer(loopOnSeconds * 1000);
                this.timer.Elapsed += Timer_Elapsed;
                this.timer.AutoReset = true;
                this.timer.Enabled = true;
                this.timer.Start();
                if (initAt == null)//cuado el servicio se corre de forma forma periódica
                    Process();
            }
        }
        /// <summary>
        /// Inicia el timer a una hora en espesífico
        /// </summary>
        /// <param name="initAt"></param>
        public virtual void StartAt(InitAt initAt)
        {
            if (initAt != null)
            {
                this.initAt = initAt;
            }
            Start(30);//verifica la hora de ejecución del servicio cada 30 seg
        }
        /// <summary>
        /// Para el servicio
        /// </summary>
        public virtual void Stop()
        {
            if (IsRunning())
            {
                this.timer.Stop();
                this.timer.Elapsed -= Timer_Elapsed;
                this.timer.Enabled = false;
            }
        }
        public virtual bool IsRunning()
        {
            return this.timer.Enabled;
        }
        public virtual void Process()
        {
            this.ProcessEvent?.Invoke();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var currenTime = GetCurrentTime();
            if (initAt == null || (initAt.Hour == currenTime.Hour && initAt.Minute == currenTime.Minute))
                Process();
        }
        private InitAt GetCurrentTime()
        {
            return new InitAt
            {
                Hour = DateTime.Now.Hour,
                Minute = DateTime.Now.Minute
            };
        }
    }
}
