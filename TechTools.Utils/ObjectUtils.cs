using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace TechTools.Utils
{
    public class ObjectUtils
    {
        public static byte[] GetByteArrayFromObject(object me)
        {
            if (me == null)
                return null;
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, me);
                return ms.ToArray();
            }
        }
        public static string GetMyHashCode(object me) {
            var ms = string.Empty;
            if (me == null)
                return ms;
            var objByteArray = (me.GetType() == typeof(byte[])) ?
                (byte[])me : GetByteArrayFromObject(me);
            
            if (objByteArray == null || objByteArray.Length == 0)
                return ms;
            if (objByteArray.Length <= 6)
                for(var i=0; i<objByteArray.Length; i++)
                {
                    if (i > 0)
                        ms += "-";
                    ms += ((int)objByteArray[i]).ToString();
                }
            else
            {
                var cabecera = GetCabecera(objByteArray);
                var cola = GetCola(objByteArray);
                var cuerpo = string.Empty;
                var longitudCuerpo = objByteArray.Length - 6;
                int muestra = longitudCuerpo / 3;
                if (muestra > 0) {
                    cuerpo = string.Format("{0}-{1}-{2}",
                        objByteArray[4],
                        objByteArray[4+muestra],
                        objByteArray[objByteArray.Length-4]
                        );
                }
                ms = string.Format("{0}-{1}-{2}", cabecera, cuerpo, cola);
            }
            return ms;
        }
        private static object GetCola(byte[] me)
        {
            var tam = me.Length-1;
            if (me.Length < 3)
                return null;
            return string.Format("{0}-{1}-{2}", me[tam-2], me[tam-1], me[tam]);
        }
        private static string GetCabecera(byte[] me)
        {
            if (me.Length < 3)
                return null;
            return string.Format("{0}-{1}-{2}",me[0],me[1],me[2]);
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
        public static List<string> GetMemberNames(Type type)
        {
            List<string> memberNames = new List<string>();

            // Get all public properties and fields of the type
            MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance);

            foreach (var member in members)
            {
                if (member.MemberType == MemberTypes.Property || member.MemberType == MemberTypes.Field)
                {
                    memberNames.Add(member.Name);
                }
            }

            return memberNames;
        }
    }
}
