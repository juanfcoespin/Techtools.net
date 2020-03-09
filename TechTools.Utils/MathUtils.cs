using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechTools.Utils
{
    public class MathUtils
    {
        /// <summary>
        /// Raliza un cast de un objeto a entero, si no se puede transformar retorna 0
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static int IntValue(object me)
        {
            if (me != null)
            {
                try
                {
                    return Convert.ToInt32(me);
                }
                catch { }
            }
            return 0;
        }

        public static bool IsIntValue(object me) {
            return (IntValue(me) > 0);
        }

        /// <summary>
        /// Devuelve la potencia de un Valor Decimal
        /// </summary>
        /// <param name="valor">el Valor a elevar a potencia</param>
        /// <param name="veces">la Potencia</param>
        /// <returns>decimal</returns>
        public static decimal Potencia(decimal valor, int veces)
        {

            decimal resultado = 1;
            int i = 0;
            while (i < veces)
            {
                resultado = resultado * valor;
                i++;
            }

            return resultado;
        }
        
        /// <summary>
        /// Devuelve un Valor Decimal redondeado a un número de decimales
        /// </summary>
        /// <param name="valor">el Valor a redondear</param>
        /// <param name="numeroDecimales">el Número de decimales</param>
        /// <returns>decimal</returns>
        public static decimal Round(decimal valor, int numeroDecimales)
        {

            decimal resultado = decimal.Round(valor, numeroDecimales);
            if ((resultado < valor) && (valor - resultado >= (5 / Potencia(10, (1 + numeroDecimales)))))
                resultado = resultado + (1 / Potencia(10, numeroDecimales));

            return resultado;
        }

        /// <summary>
        /// Dado una valor devuelve una cadena
        /// </summary>
        /// <param name="unValor">el valor a decodificar</param>
        /// <param name="enDolares">si imprime la palabara dolares</param>
        /// <returns>string</returns>
        public static string ValorACadena(decimal unValor)
        {
            int i, j;
            decimal valore;
            decimal valayu;
            decimal valrev;
            int unidad;
            int decena;
            int centena;
            int centenaMillon;
            int decenaMillon;
            int millon;


            string cadena = "";
            string cadena1 = "";
            string centavos = "00/100";
            string cadenaResultante;

            Hashtable regdis = new Hashtable();
            Hashtable regcen = new Hashtable();
            Hashtable regdec = new Hashtable();
            Hashtable regsal = new Hashtable();

            regdis.Add(0, "DOLARES");
            regdis.Add(1, "UNO");
            regdis.Add(2, "DOS");
            regdis.Add(3, "TRES");
            regdis.Add(4, "CUATRO");
            regdis.Add(5, "CINCO");
            regdis.Add(6, "SEIS");
            regdis.Add(7, "SIETE");
            regdis.Add(8, "OCHO");
            regdis.Add(9, "NUEVE");
            regdis.Add(10, "DIEZ");
            regdis.Add(11, "ONCE");
            regdis.Add(12, "DOCE");
            regdis.Add(13, "TRECE");
            regdis.Add(14, "CATORCE");
            regdis.Add(15, "QUINCE");
            regdis.Add(16, "DIECISEIS");
            regdis.Add(17, "DIECISIETE");
            regdis.Add(18, "DIECIOCHO");
            regdis.Add(19, "DIECINUEVE");


            regdec.Add(0, " ");
            regdec.Add(1, "DIEZ ");
            regdec.Add(2, "VEINTE");
            regdec.Add(3, "TREINTA");
            regdec.Add(4, "CUARENTA");
            regdec.Add(5, "CINCUENTA");
            regdec.Add(6, "SESENTA");
            regdec.Add(7, "SETENTA");
            regdec.Add(8, "OCHENTA");
            regdec.Add(9, "NOVENTA");

            regcen.Add(0, " ");
            regcen.Add(1, "CIENTO");
            regcen.Add(2, "DOSCIENTOS");
            regcen.Add(3, "TRESCIENTOS");
            regcen.Add(4, "CUATROSCIENTOS");
            regcen.Add(5, "QUINIENTOS");
            regcen.Add(6, "SEISCIENTOS");
            regcen.Add(7, "SETECIENTOS");
            regcen.Add(8, "OCHOCIENTOS");
            regcen.Add(9, "NOVECIENTOS");

            valore = unValor;
            i = 0;

            decena = (int)(valore / 10000000000);
            valrev = MathUtils.Round(valore / 10000000000, 2);
            if (decena > valrev)
                decena--;
            valayu = decena;
            valore = valore - (valayu * 10000000000);

            unidad = (int)(valore / 1000000000);
            valrev = MathUtils.Round(valore / 1000000000, 2);
            if (unidad > valrev)
                unidad--;
            valayu = unidad;
            valore = valore - (valayu * 1000000000);


            centenaMillon = (int)(valore / 100000000);
            valrev = MathUtils.Round(valore / 100000000, 2);
            if (centenaMillon > valrev)
                centenaMillon--;
            valayu = centenaMillon;
            valore = valore - (centenaMillon * 100000000);

            decenaMillon = (int)(valore / 10000000);
            valrev = MathUtils.Round(valore / 10000000, 2);
            if (decenaMillon > valrev)
                decenaMillon--;
            valayu = decenaMillon;
            valore = valore - (decenaMillon * 10000000);

            millon = (int)(valore / 1000000);
            valrev = MathUtils.Round(valore / 1000000, 2);
            if (millon > valrev)
                millon--;
            valayu = millon;
            valore = valore - (millon * 1000000);



            if (decena != 0)
            {
                if (decena < 2)
                    if (centenaMillon != 0 || decenaMillon != 0 || millon != 0)
                        cadenaResultante = String.Format("{0} MIL ", regdis[10 + unidad]);
                    else
                        cadenaResultante = String.Format("{0} MIL MILLONES ", regdis[10 + unidad]);

                else
                    if (unidad != 0)
                        cadenaResultante = String.Format("{0} Y ", regdec[decena]);
                    else
                        if (centenaMillon != 0 || decenaMillon != 0 || millon != 0)
                            cadenaResultante = String.Format("{0} MIL ", regdec[decena]);
                        else
                            cadenaResultante = String.Format("{0} MIL MILLONES ", regdec[decena]);


                regsal.Add(i, cadenaResultante);
                i++;

            }


            if (decena != 1)
                if (unidad != 0)
                {
                    if (unidad == 1)
                        if (centenaMillon != 0 || decenaMillon != 0 || millon != 0)
                            cadenaResultante = String.Format("UN MIL ");
                        else
                            cadenaResultante = String.Format("UN MIL MILLONES");
                    else
                        if (centenaMillon != 0 || decenaMillon != 0 || millon != 0)
                            cadenaResultante = String.Format("{0} MIL ", regdis[unidad]);
                        else
                            cadenaResultante = String.Format("{0} MIL MILLONES ", regdis[unidad]);


                    regsal.Add(i, cadenaResultante);

                    i++;
                }





            if (centenaMillon != 0)
            {
                if (centenaMillon == 1)
                    if (decenaMillon != 0 || millon != 0)
                        cadenaResultante = String.Format("CIENTO ");
                    else
                        cadenaResultante = String.Format("CIEN MILLONES ");
                else
                    if (decenaMillon != 0 || millon != 0)
                        cadenaResultante = String.Format("{0} ", regcen[centenaMillon]);
                    else
                        cadenaResultante = String.Format("{0} MILLONES ", regcen[centenaMillon]);



                regsal.Add(i, cadenaResultante);

                i++;
            }



            if (decenaMillon != 0)
            {
                if (decenaMillon < 2)
                    cadenaResultante = String.Format("{0} MILLONES ", regdis[10 + millon]);
                else
                    if (millon != 0)
                        cadenaResultante = String.Format("{0} Y ", regdec[decenaMillon]);
                    else
                        cadenaResultante = String.Format("{0} MILLONES ", regdec[decenaMillon]);
                regsal.Add(i, cadenaResultante);

                i++;
            }


            if (decenaMillon != 1)
                if (millon != 0)
                {
                    if (millon == 1)
                        cadenaResultante = String.Format(" UN MILLON ");
                    else
                        cadenaResultante = String.Format("{0} MILLONES ", regdis[millon]);

                    regsal.Add(i, cadenaResultante);

                    i++;

                }

            centena = (int)(valore / 100000);
            valrev = MathUtils.Round(valore / 100000, 2);
            if (centena > valrev)
                centena--;
            valayu = centena;
            valore = valore - (valayu * 100000);



            decena = (int)(valore / 10000);
            valrev = MathUtils.Round(valore / 10000, 2);
            if (decena > valrev)
                decena--;
            valayu = decena;
            valore = valore - (valayu * 10000);

            unidad = (int)(valore / 1000);
            valrev = MathUtils.Round(valore / 1000, 2);
            if (unidad > valrev)
                unidad--;
            valayu = unidad;
            valore = valore - (valayu * 1000);


            if (centena != 0)
            {
                if (centena == 1)
                    if (decena != 0 || unidad != 0)
                        cadenaResultante = String.Format("CIENTO ");
                    else
                        cadenaResultante = String.Format("CIEN MIL ");
                else
                    if (decena != 0 || unidad != 0)
                        cadenaResultante = String.Format("{0} ", regcen[centena]);
                    else
                        cadenaResultante = String.Format("{0} MIL ", regcen[centena]);

                regsal.Add(i, cadenaResultante);

                i++;
            }

            if (decena != 0)
            {
                if (decena < 2)
                    cadenaResultante = String.Format("{0} MIL ", regdis[10 + unidad]);
                else
                    if (unidad != 0)
                        cadenaResultante = String.Format("{0} Y ", regdec[decena]);
                    else
                        cadenaResultante = String.Format("{0} MIL ", regdec[decena]);

                regsal.Add(i, cadenaResultante);
                i++;

            }


            if (decena != 1)
            {
                if (unidad != 0)
                {
                    if (unidad == 1)
                        cadenaResultante = String.Format("UN MIL ");
                    else
                        cadenaResultante = String.Format("{0} MIL ", regdis[unidad]);

                    regsal.Add(i, cadenaResultante);

                    i++;
                }
            }

            centena = (int)(valore / 100);
            valrev = MathUtils.Round(valore / 100, 2);
            if (centena > valrev)
                centena--;
            valayu = centena;
            valore = valore - (valayu * 100);



            decena = (int)(valore / 10);
            valrev = MathUtils.Round(valore / 10, 2);
            if (decena > valrev)
                decena--;
            valayu = decena;
            valore = valore - (valayu * 10);

            unidad = (int)(valore / 1);
            valrev = MathUtils.Round(valore / 1, 2);
            if (unidad > valrev)
                unidad--;
            valayu = unidad;
            valore = valore - (valayu * 1);


            if (centena != 0)
            {
                if (centena == 1)
                    if (decena != 0 || unidad != 0)
                    {
                        cadenaResultante = String.Format("CIENTO ");
                        regsal.Add(i, cadenaResultante);
                        i++;

                    }
                    else
                    {
                        cadenaResultante = String.Format("CIEN ");
                        regsal.Add(i, cadenaResultante);
                        i++;

                    }
                else
                {
                    cadenaResultante = String.Format("{0} ", regcen[centena]);
                    regsal.Add(i, cadenaResultante);
                    i++;

                }

            }


            if (decena != 0)
            {
                if (decena < 2)
                    cadenaResultante = String.Format("{0} ", regdis[10 + unidad]);
                else
                    if (unidad != 0)
                        cadenaResultante = String.Format("{0} Y ", regdec[decena]);
                    else
                        cadenaResultante = String.Format("{0}  ", regdec[decena]);

                regsal.Add(i, cadenaResultante);
                i++;

            }


            if (decena != 1)
                if (unidad != 0)
                {
                    if (unidad == 1)
                        cadenaResultante = String.Format("UN ");
                    else
                        cadenaResultante = String.Format("{0} ", regdis[unidad]);

                    regsal.Add(i, cadenaResultante);

                    i++;
                }


            if (valore != 0)
            {
                valore = valore * 100;
                centavos = String.Format(" {0}/100", (int)valore);
            }


            j = 0;
            while (j < i)
            {
                if (regsal.ContainsKey(j))
                {
                    cadena1 = cadena;
                    cadena = String.Format("{0} {1}", cadena1, regsal[j]);
                }
                j++;
            }

            cadena = cadena + " " + centavos;

            return cadena;
        }
        public static int GetRandomNumber(int min, int max)
        {
            return new Random().Next(min, max);
        }
        public static decimal Get2PointDecimal(decimal me)
        {
            return Convert.ToDecimal(string.Format("{0:0.00}", me));
        }
    }
}
