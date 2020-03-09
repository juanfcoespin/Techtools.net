using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechTools.Utils
{
    public class FechaUtils
    {
        public static DateTime ConvertToEcuadorContinental(DateTime me)
        {
            var quitoTimeZone = TimeZoneInfo.CreateCustomTimeZone("QuitoTimeZone", new TimeSpan(-5, 0, 0), "Quito Time Zone", "(UTC-05:00) Bogotá, Lima, Quito, Rio Branco");
            return TimeZoneInfo.ConvertTime(me, quitoTimeZone);
        }
        /// <summary>
        /// Prepara una Fecha para que sea incluida como Fecha de Inicio de una consulta SQL que contenga una sentencia BETWEEN
        /// </summary>
        /// <param name="fecha">la Fecha</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvierteAFechaInicioParaBusqueda(DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0);
        }
        public static string GetMesName(int month)
        {
            switch (month) {
                case 1:
                    return "Enero";
                case 2:
                    return "Febrero";
                case 3:
                    return "Marzo";
                case 4:
                    return "Abril";
                case 5:
                    return "Mayo";
                case 6:
                    return "Junio";
                case 7:
                    return "Julio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Septiembre";
                case 10:
                    return "Octubre";
                case 11:
                    return "Noviembre";
                case 12:
                    return "Diciembre";
                default:
                    return null;
            }
            
        }
        public static object GetHora()
        {
            return String.Format("{0:T}", DateTime.Now);
        }
        /// <summary>
        /// Prepara una Fecha para que sea incluida como Fecha de Fin de una consulta SQL que contenga una sentencia BETWEEN
        /// </summary>
        /// <param name="fecha">la Fecha</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvierteAFechaFinParaBusqueda(DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, fecha.Day, 23, 59, 59);
        }
        /// <summary>
        /// Entrega una lista que representa los Dias de la semana en español ordenados de Lunes a Domingo
        /// </summary>
        /// <returns>List string</returns>
        public static List<string> DiasDeLaSemana_Español()
        {
            List<string> dias = new List<string>();

            dias.Add(TraduceDayOfWeek_Español(DayOfWeek.Monday));
            dias.Add(TraduceDayOfWeek_Español(DayOfWeek.Tuesday));
            dias.Add(TraduceDayOfWeek_Español(DayOfWeek.Wednesday));
            dias.Add(TraduceDayOfWeek_Español(DayOfWeek.Thursday));
            dias.Add(TraduceDayOfWeek_Español(DayOfWeek.Friday));
            dias.Add(TraduceDayOfWeek_Español(DayOfWeek.Saturday));
            dias.Add(TraduceDayOfWeek_Español(DayOfWeek.Sunday));

            return dias;
        }
        /// <summary>
        /// Entrega una cadena que representa el nombre en Español de un Dia de la semana
        /// </summary>
        /// <param name="dia">Dia (DayOfWeek) del que se necesita saber su nombre en español</param>
        /// <returns>string</returns>
        public static string TraduceDayOfWeek_Español(DayOfWeek dia)
        {
            switch (dia)
            {
                case DayOfWeek.Friday: return "Viernes";
                case DayOfWeek.Monday: return "Lunes";
                case DayOfWeek.Saturday: return "Sabado";
                case DayOfWeek.Sunday: return "Domingo";
                case DayOfWeek.Thursday: return "Jueves";
                case DayOfWeek.Tuesday: return "Martes";
                case DayOfWeek.Wednesday: return "Miercoles";
                default: return "Dia No Reconocido...";
            }
        }
        /// <summary>
        /// Entrega una cadena que representa el nombre en Español de un Mes del año
        /// </summary>
        /// <param name="mes">Número (1-12) de Mes del que se necesita saber su nombre en español</param>
        /// <returns>string</returns>
        public static string TraduceMes_Español(int mes)
        {
            switch (mes)
            {
                case 1: return "Enero";
                case 2: return "Febrero";
                case 3: return "Marzo";
                case 4: return "Abril";
                case 5: return "Mayo";
                case 6: return "Junio";
                case 7: return "Julio";
                case 8: return "Agosto";
                case 9: return "Septiembre";
                case 10: return "Octubre";
                case 11: return "Noviembre";
                case 12: return "Diciembre";
                default: return "Mes No Reconocido...";
            }
        }
        ///// <summary>
        ///// Dado una fecha devuelve la fecha del último Día del año
        ///// </summary>
        ///// <param name="unaFecha">la fecha</param>
        ///// <returns>DateTime</returns>
        public static DateTime FinAnio(DateTime unaFecha)
        {
            DateTime fechaFinAnio = new DateTime(unaFecha.Year, 12, 31);
            return fechaFinAnio;
        }
        ///// <summary>
        ///// Dado dos Fechas devuelve la diferencia en Dias
        ///// </summary>
        ///// <param name="fechaInicio">Fecha Inicio</param>
        ///// <param name="fechaFin">Fecha Fin</param>
        ///// <param name="soloLaborables">Solo Laborables</param>
        ///// <returns>int</returns>
        public static int DevuelveDiferenciaDias(DateTime fechaInicio, DateTime fechaFin, bool soloLaborales)
        {
            int dias = 0;

            TimeSpan diferenciaEnDias = fechaFin.Date - fechaInicio.Date;
            if (soloLaborales)
            {
                int diasLaborables = 0;
                for (int i = 0; i < diferenciaEnDias.Days; i++)
                {
                    if (!(fechaInicio.AddDays(i + 1).DayOfWeek == DayOfWeek.Saturday || fechaInicio.AddDays(i + 1).DayOfWeek == DayOfWeek.Sunday))
                        diasLaborables++;
                }

                dias = diasLaborables;
            }
            else
                dias = diferenciaEnDias.Days;

            return dias + 1;
        }
        /// <summary>
        /// Trae la fecha en el formato: (2010-1-5 16:13:57.283)
        /// </summary>
        /// <returns></returns>
        public static string getDB_formatDate()
        {
            return DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
        }
        public static DateTime GetFechaInicioDefensoria()
        {
            return new DateTime(2007, 01, 01);
        }
        public static bool FechaInferior_InicioDefensoria(DateTime fecha, ref string msg)
        {
            if (fecha == DateTime.MinValue || fecha < GetFechaInicioDefensoria() || fecha > DateTime.Now)
            {
                StringBuilder sbMsg = new StringBuilder();
                msg += "\r\n\r\n(Tenga en cuenta que esta fecha debe ser superior a la fecha de inicio de la defensoría " + GetFechaInicioDefensoria().ToShortDateString() + " y menor o igual a la fecha actual)";
                return true;
            }
            return false;
        }
        /// <summary>
        /// Dado una fecha devuelve la fecha del primer Día del Mes
        /// </summary>
        /// <param name="unaFecha">la fecha</param>
        /// <returns>DateTime</returns>
        public static DateTime InicioMes(DateTime unaFecha)
        {
            return new DateTime(unaFecha.Year, unaFecha.Month, 1);
        }
        /// <summary>
        /// Dado una fecha devuelve la fecha del último Día del mes
        /// </summary>
        /// <param name="unaFecha">la fecha</param>
        /// <returns>DateTime</returns>
        public static DateTime FinMes(DateTime unaFecha)
        {
            TimeSpan dias = new TimeSpan((DateTime.DaysInMonth(unaFecha.Year, unaFecha.Month) - unaFecha.Day), 0, 0, 0);
            return unaFecha.Add(dias);

        }
        /// <summary>
        /// Usualmente se utiliza para calcular la edad de la persona
        /// </summary>
        /// <param name="fechaInicio">Usualmente la fecha de nacimiento</param>
        /// <param name="fechaFin">Usualmente la fecha actual</param>
        /// <param name="mostrarFechaCompleta"></param>
        /// <param name="numerosFormatoTexto"></param>
        /// <returns></returns>
        public static string CalculoFechaFutura(DateTime fechaInicio, DateTime fechaFin, bool mostrarFechaCompleta, bool numerosFormatoTexto)
        {
            string mensajeSalida = string.Empty;

            int anio = 0;
            int mes = 0;
            int dia = 0;

            DateTime tmpFechaNacimiento = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);
            DateTime tmpFechaFutura = new DateTime(fechaFin.Year, fechaFin.Month, 1);

            while (tmpFechaNacimiento.AddYears(anio).AddMonths(mes) < tmpFechaFutura)
            {
                mes++;
                if (mes > 12)
                {
                    anio++;
                    mes = mes - 12;
                }
            }

            if (fechaFin.Day >= fechaInicio.Day)
                dia = dia + fechaFin.Day - fechaInicio.Day;
            else
            {
                mes--;
                if (mes < 0)
                {
                    anio--;
                    mes = mes + 12;
                }
                dia += DateTime.DaysInMonth(fechaFin.AddMonths(-1).Year, fechaFin.AddMonths(-1).Month) + fechaFin.Day - fechaInicio.Day;
            }

            // Agrega un día extra si la fecha de nacimiento es día bisiesto
            if (DateTime.IsLeapYear(fechaInicio.Year) && fechaInicio.Month == 2 && fechaInicio.Day == 29)
            {
                //Solo si la fecha futura es menor que 1st Marzo
                if (fechaFin >= new DateTime(fechaFin.Year, 3, 1))
                    dia++;
            }

            string strAnio = "[A]";
            string strMes = "[M]";
            string strDia = "[D]";

            if (numerosFormatoTexto)
            {
                if (mostrarFechaCompleta)
                    mensajeSalida = string.Format("{0} {1}, {2} {3}, {4} {5} ", anio, strAnio, mes, strMes, dia, strDia);
                else
                    mensajeSalida = string.Format("{0} {1}", anio, strAnio);
            }
            else
            {
                if (mostrarFechaCompleta)
                    mensajeSalida = string.Format("{0}|{1}|{2}", anio, mes, dia);
                else
                    mensajeSalida = string.Format("{0}", anio);
            }

            return mensajeSalida;
        }
        public static int CalculoEdad(DateTime fechaNacimiento) {
            string sEdad = CalculoFechaFutura(fechaNacimiento, DateTime.Now, false, true);
            if (!string.IsNullOrEmpty(sEdad))
                sEdad = sEdad.Replace("[A]", null);
            return MathUtils.IntValue(sEdad);
        }
        public static bool FechaNacimientoCorrecta(DateTime fechaNacimiento, int minEdad, int maxEdad)
        {
            var edad = DateTime.Now.Year - fechaNacimiento.Year;
            return (edad >= minEdad && edad <= maxEdad);
        }
        public static bool EsElMismoDia(DateTime dt1, DateTime dt2)
        {
            return (dt1.ToShortDateString() == dt2.ToShortDateString()) ;
        }

        public static bool FechaMayorQueHoy(DateTime me)
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            return me > date;
        }

        public static void LansarExecpcionSiFechaMayorQueHoy(DateTime me, string source)
        {
            if (FechaUtils.FechaMayorQueHoy(me))
            {
                throw new Exception(string.Format("La fecha {0} de {1} no puede ser mayor a la fecha de hoy ({2})", me,source, DateTime.Now));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="me"></param>
        /// <param name="format">puede ser 'yyyy/mm/dd','yyyy/mm', 'dd-mm-yyyy', 'YYYY-DD-MM'</param>
        /// <returns>string correspondiente a la fecha en el formato solicitado</returns>
        public static string GetStringDate(DateTime me, string format)
        {
            return me.ToString(format);
        }
    }
}
