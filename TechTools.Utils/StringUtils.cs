using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;

namespace TechTools.Utils
{
    public class StringUtils
    {
        /// <summary>
        /// Sirve para hacer búsquedas de un arreglo de textos en un texto. Ej. "Pan de chocolate", contiene "pan" y "chocalate"
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static bool TextoContieneCriterios(string texto, string[] tokens)
        {
            var tieneToken = false;
            foreach (var token in tokens) //valida que tenga todos los criterios de busqueda
                tieneToken = CleanText(texto).Contains(CleanText(token));

            return tieneToken;
        }
        public static string CleanText(string item)
        {
            return ReemplazarTildes(item.ToLower().Trim());
        }
        public static string ReemplazarTildes(string item)
        {
            item = item.Replace("á", "a");
            item = item.Replace("é", "e");
            item = item.Replace("í", "i");
            item = item.Replace("ó", "o");
            item = item.Replace("ú", "u");
            item = item.Replace("ñ", "n");
            return item;
        }
        public static char PonerMayusculasQuitarTildes(char c)
        {
            Regex reg = new Regex("\\d");
            if (reg.IsMatch(c.ToString()))
                return Convert.ToChar(0);
            switch (c)
            {
                case 'á':
                    return 'A';
                case 'é':
                    return 'E';
                case 'í':
                    return 'I';
                case 'ó':
                    return 'O';
                case 'ú':
                    return 'U';
                case 'Á':
                    return 'A';
                case 'É':
                    return 'E';
                case 'Í':
                    return 'I';
                case 'Ó':
                    return 'O';
                case 'Ú':
                    return 'U';
                default:
                    return c.ToString().ToUpper()[0];

            }
        }
        public static string ConvertStringToNoAccents(string patter)
        {
            string mensajeSalida = patter.Trim().ToLower();

            mensajeSalida = mensajeSalida.Replace("á", "a");
            mensajeSalida = mensajeSalida.Replace("é", "e");
            mensajeSalida = mensajeSalida.Replace("í", "i");
            mensajeSalida = mensajeSalida.Replace("ó", "o");
            mensajeSalida = mensajeSalida.Replace("ú", "u");
            mensajeSalida = mensajeSalida.Replace("ñ", "n");

            return mensajeSalida;
        }
        /// <summary>
        /// Entrega una Cadena numérica aleatoria de un tamaño determinado
        /// </summary>
        /// <param name="cadenas">Cadenas que deben ser Ingresadas dentro del Parentesis</param>
        public static string GeneraCadenaNumericaAleatoria(int numeroCaracteres)
        {
            string cadena = "";
            Random random = new Random();
            for (int i = 0; i < numeroCaracteres; i++)
                cadena += random.Next(10).ToString();

            return cadena;
        }
        /// <summary>
        /// Convierte un texto en Título
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToTitle(string s)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            return textInfo.ToTitleCase(s.ToLower());
        }
        /// <summary>
        /// Entrega una Cadena del tipo: ("cadenas[0]", "cadenas[1]", "cadenas[2]",...)
        /// </summary>
        /// <param name="cadenas">Cadenas que deben ser Ingresadas dentro del Parentesis</param>
        public static string EntreParentesis(List<string> cadenas)
        {
            StringBuilder cadenaResultado = new StringBuilder();
            cadenaResultado.Append("(");
            int i = 1;
            foreach (string unaCadena in cadenas)
            {
                cadenaResultado.Append("'");
                cadenaResultado.Append(unaCadena);
                if (i != cadenas.Count)
                    cadenaResultado.Append("', ");
                else
                    cadenaResultado.Append("')");
                i++;
            }

            if (cadenas.Count == 0)
                cadenaResultado.Append("'')");

            return cadenaResultado.ToString();
        }
        /// <summary>
        /// Entrega una Cadena del tipo: ("cadenas[0].ToString()", "cadenas[1].ToString()", "cadenas[2].ToString()",...)
        /// </summary>
        /// <param name="cadenas">Cadenas que deben ser Ingresadas dentro del Parentesis</param>
        public static string EntreParentesis(List<int> elementos)
        {
            StringBuilder cadenaResultado = new StringBuilder();
            cadenaResultado.Append("(");
            int i = 1;
            foreach (int unElemento in elementos)
            {
                if (i != elementos.Count)
                    cadenaResultado.Append(unElemento.ToString().Trim() + ", ");
                else
                    cadenaResultado.Append(unElemento.ToString().Trim() + ")");
                i++;
            }

            if (elementos.Count == 0)
                cadenaResultado.Append("'')");

            return cadenaResultado.ToString();
        }
        /// <summary>
        /// Entrega una Cadena del tipo: ("cadenas[0]", "cadenas[1]", "cadenas[2]",...)
        /// </summary>
        /// <param name="cadenas">Cadenas que deben ser Ingresadas dentro del Parentesis</param>
        /// <param name="conApostrofes">Indica si las cadenas deberan llevar apostrofes dentro del parentesis</param>
        public static string EntreParentesis(string[] cadenas, bool conApostrofes)
        {
            StringBuilder cadenaResultado = new StringBuilder();
            cadenaResultado.Append("(");
            for (int i = 0; i < cadenas.Length; i++)
            {
                if (i != cadenas.Length - 1)
                {
                    if (!conApostrofes)
                        cadenaResultado.Append(cadenas[i] + ", ");
                    else
                        cadenaResultado.Append("'" + cadenas[i] + "', ");
                }
                else
                {
                    if (!conApostrofes)
                        cadenaResultado.Append(cadenas[i] + ")");
                    else
                        cadenaResultado.Append("'" + cadenas[i] + "')");
                }
            }
            return cadenaResultado.ToString();
        }
        public static char ReenplazarCaracterParecido(char c)
        {
            switch (c)
            {
                case 'á':
                    return 'a';
                case 'é':
                    return 'e';
                case 'í':
                    return 'i';
                case 'ó':
                    return 'o';
                case 'ú':
                    return 'u';
                case 'ñ':
                    return 'n';
                default:
                    return c;
            }
        }
        /// <summary>
        /// Sirve para preparar una cadena para hacer una búsqueda
        /// Por Ej. Espín -> espin
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ReemplazarPalabrasParecidas(string token)
        {
            token = token.ToLower().Trim();
            switch (token)
            {
                case "enrique":
                    return "enríquez";
                case "enríquez":
                    return "enrique";
                case "fernando":
                    return "fernández";
                case "fernández":
                    return "fernando";
                case "domingo":
                    return "domínguez";
                case "domínguez":
                    return "domingo";
                case "ramiro":
                    return "ramírez";
                case "ramírez":
                    return "ramiro";
                case "gonzalo":
                    return "gonzáles";
                case "ximena":
                    return "ximénez";
                case "ximénez":
                    return "ximena";
                case "xavier":
                    return "javier";
                case "javier":
                    return "xavier";
                case "quinones":
                    return "quiñonez";
            }
            return token;
        }
        public static string GetParametro_FromSplit(string texto, char delimitador, int posicion)
        {
            try
            {
                string[] mTexto = texto.Split(new char[] { delimitador });
                if (mTexto != null && mTexto.Length > posicion)
                    return mTexto[posicion];
            }
            catch{}
            
            return null;
        }
        /// <summary>
        /// Pasa cadenas del formato Gómez a G[óó]m[eé]z, necesario para hacer consultas en sql server considerando palabras con o sin tilde
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static string CadenaConAcento(string cadena)
        {
            cadena = cadena.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            cadena = reg.Replace(cadena, string.Empty);

            cadena = cadena
                .Replace("a", "[aá]")
                .Replace("e", "[eé]")
                .Replace("i", "[ií]")
                .Replace("o", "[oó]")
                .Replace("u", "[uú]");

            return cadena;
        }
        /// <summary>
        /// dgancino: 19/11/2013 - Remplaza Cadenas para escritos
        /// </summary>
        /// <param name="textoCompleto"></param>
        /// <param name="valores">Diccionario que contiene como id el valor a buscar y valor el valor a remplazar</param>
        /// <returns></returns>
        public static string ReemplazarTexto(string textoCompleto, Dictionary<string, string> valores)
        {
            foreach (var item in valores)
            {
                textoCompleto = textoCompleto.Replace(item.Key, item.Value);
                var nuevoTextoBusqueda = ReemplazarTildes(item.Key);
                if (!nuevoTextoBusqueda.Equals(item.Key))
                    textoCompleto = textoCompleto.Replace(nuevoTextoBusqueda, item.Value);
            }

            return textoCompleto;
        }
        public static string GetMoneyFormat(decimal me) {
            return string.Format("{0:C}", me);
        }
        /// <summary>
        /// Ejemplo: 001
        /// </summary>
        /// <param name="longitudNumero">3</param>
        /// <param name="numero">1</param>
        /// <returns>"001"</returns>
        public static string PonerCerosIzquierda(int longitudNumero, string numero)
        {
            int cerosQueFaltan = longitudNumero - numero.Length;
            while (cerosQueFaltan > 0)
            {
                numero = "0" + numero;
                cerosQueFaltan--;
            }
            return numero;
        }
        public static string QuitarCerosIzquierda(string num)
        {
            var indiceDondeCaracterEsDiferenteDeCero = 0;
            for (int i = 0; i < num.Length; i++)
            {
                if (num[i] != '0')
                {
                    indiceDondeCaracterEsDiferenteDeCero = i;
                    break;
                }
            }

            var ms = num.Substring(indiceDondeCaracterEsDiferenteDeCero,
                num.Length - indiceDondeCaracterEsDiferenteDeCero);
            return ms;
        }
        public static bool TieneCerosALaIzquierda(string num)
        {
            return num[0] == '0';
        }
        public static int GetIntValue(string me) {
            try
            {
                return Convert.ToInt32(me);
            }
            catch 
            {
                return 0;
            }
        }
        public static string QuitarEnter(string me) {
            if (!string.IsNullOrEmpty(me))
            {
                me = me.Replace(((char)13).ToString()+" ", null);//enter + espacio
                me = me.Replace(((char)13).ToString(), null);//enter
                me = me.Replace(((char)10).ToString()+" ", null);//salto de linea
                me = me.Replace(((char)10).ToString(), null);//salto de linea
            }
            return me;
        }
        public static string QuitarTabs(string me) {
            if (!string.IsNullOrEmpty(me))
            {
                //quita todos los espacios en blanco
                const string reduceMultiSpace = @"[ ]{2,}";
                me = Regex.Replace(me.Replace("\t", " "), reduceMultiSpace, " ");
                me = me.Replace("; ", ";");// separadores de archivo plano
                me = me.Replace("] ", "]");// separadores de archivo plano
            }
            return me;
        }
        public static string getTwoDigitNumber(int me)
        {
            if (me < 10)
                return "0" + me.ToString();
            return me.ToString();
        }
        /// <summary>
        /// retorna 00 si no se manda un int
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string getTwoDigitNumber(string me) {
            try
            {
                var intMe = Convert.ToInt32(me);
                return getTwoDigitNumber(intMe);
            }
            catch 
            {
                return "00";
            }
            
        }
    }
}
