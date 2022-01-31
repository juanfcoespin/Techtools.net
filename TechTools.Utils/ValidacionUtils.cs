using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

namespace TechTools.Utils
{
    public class ValidacionUtils
    {
        //public static bool EsMoneda(decimal me) {
        //    Regex reg = new Regex(@"09[8-9]\d{7,7}$");
        //    return reg.IsMatch(celular);
        //}

        
        
        public static bool CelularValido(string celular)
        {
            return ValidarExpresionRegular(@"09[0-9]\d{7,7}$", celular);
        }
        public static bool ValidarExpresionRegular(string regEx, string itemToValidate) {
            if (string.IsNullOrEmpty(itemToValidate))
                return false;
            Regex reg = new Regex(regEx);
            return reg.IsMatch(itemToValidate);
        }
        public static bool Celular_O_ConvencionalValido(string telefono) 
        {
            if (CelularValido(telefono))
                return true;
            if (TelefonoConvencionalValido(telefono))
                return true;
            return false;
        }
        /// <summary>
        /// Valida un número de cédula de Ecuador
        /// </summary>
        /// <param name="cedulaAValidar">Cédula a ser validada</param>
        /// <returns>True o False dependiendo de la validez</returns>
        public static bool ValidaCedula_Ecuador(string cedulaAValidar)
        {
            if (cedulaAValidar == null)
                return false;
            int i = 0;
            int j = 0;
            int valor = 0;

            if (cedulaAValidar.Length != 10)
                return false;

            while (i < cedulaAValidar.Length - 1)
            {
                if (!char.IsNumber(cedulaAValidar[i]))
                    return false;
                j = int.Parse(cedulaAValidar[i].ToString());
                if ((i + 1) % 2 != 0)
                {
                    j = j * 2;
                    if (j > 9)
                        j -= 9;
                }
                valor += j;
                i++;
            }

            if (valor % 10 != 0)
                valor = 10 - (valor % 10);
            else
                valor = 0;

            if ((int.Parse(cedulaAValidar[i].ToString())) != valor)
                return false;

            return true;
        }
        /// <summary>
        /// Método de validación de un número de Ruc en el Ecuador
        /// </summary>
        /// <param name="rucAValidar">Número RUC a validar</param>
        /// <returns>True o False dependiendo de la validez</returns>
        public static bool ValidaRUC_Ecuador(string rucAValidar)
        {
            if (rucAValidar == null)
                return false;
            try
            {
                int i = 0;
                int j = 0;
                string vector;
                int contador = 0;

                int digito = 0;

                if (rucAValidar.Length != 13)
                    return false;
                while (i < rucAValidar.Length)
                {
                    if (Char.IsLetter(rucAValidar[i]))
                        return false;
                    i++;
                }
                if (rucAValidar[10] != '0' || rucAValidar[11] != '0' || rucAValidar[12] != '1')
                    return false;
                if (rucAValidar[2] < '6')
                {
                    string subcadena = rucAValidar.Substring(0, 10);
                    return ValidacionUtils.ValidaCedula_Ecuador(subcadena);
                }
                else
                {
                    if (rucAValidar[2] == '6')
                    {
                        if (rucAValidar[9] != '0')
                            return false;
                        vector = "32765432";
                        contador = 8;
                    }
                    else
                    {
                        contador = 9;
                        vector = "432765432";
                    }
                    for (i = 0; i < contador; ++i)
                        j += Int16.Parse(rucAValidar[i].ToString()) * Int16.Parse(vector[i].ToString());

                    digito = j % 11;
                    digito = 11 - digito;

                    if (digito == 11)
                        digito = 0;
                    return (rucAValidar[contador].ToString() == digito.ToString());
                }
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Se valida que al menos tenga 8 caracteres
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        public static bool PasaporteValido(string documento, ref string msg)
        {
            var minimunLength = 8;
            if (!string.IsNullOrEmpty(documento)) {
                var hasMinimunLength = documento.Length >= minimunLength;
                if (!hasMinimunLength) {
                    msg = string.Format("El pasaporte debe tener al menos {0} caracteres", minimunLength);
                    return false;
                }
                if (MismoCaracterRepetido(documento, ref msg))
                    return false;
                return true;
            }
            return false;
        }

        public static bool TieneNumerosYLetras(string me, ref string msg)
        {
            if (!ValidarExpresionRegular(@"(\d+[A-Z]+[A-Z0-9]*)|([A-z]+\d+[A-Z0-9]*)", me)) {
                msg = string.Format("La expresión '{0}' debe tener números y letras en mayúsculas!!",me);
                return false;
            }
            return true;
        }

        /// <summary>
        /// se asume como caracter repetido si se manda una cadena vacía o con un solo caracter
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static bool MismoCaracterRepetido(string me, ref string msg)
        {
            if (!string.IsNullOrEmpty(me) && me.Length>0) {
                char caracter = me[0];
                foreach (var item in me)
                {
                    if (item != caracter)
                        return false;
                }
            }
            msg = "No se permite el mismo caracter repetido!!";
            return true;    
        }

        public static bool IsIntValue(string valor)
        {
            try
            {
                int intValue = Convert.ToInt32(valor);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool FechaNacimientoValida(DateTime fechaNacimiento,int edadMaxima)
        {
            var edadCalculada = FechaUtils.CalculoEdad(fechaNacimiento);
            if (!(edadCalculada > 0 && edadCalculada < edadMaxima))
            {
                return false;
            }
            return true;
        }
        public static bool TelefonoConvencionalValido(string telefonoConvencional)
        {
            return ValidarExpresionRegular(@"0[1-7]\d{7,7}$", telefonoConvencional);
        }
        public static bool EmailValid(string eMail)
        {
            if (string.IsNullOrEmpty(eMail))
                return false;
            return ValidarExpresionRegular(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", eMail);
            //try
            //{
            //    var addr = new System.Net.Mail.MailAddress(eMail);
            //    return addr.Address == eMail;
            //}
            //catch
            //{
            //    return false;
            //}
        }
    }
}
