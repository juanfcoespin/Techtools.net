using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTools.Utils;
using System.Collections.Generic;

namespace TechTools.Utils.UnitTest
{
    [TestClass]
    public class StringUtilsUT
    {
        [TestMethod]
        public void GetSqlInStringFromList()
        {
            var list = new List<string>();
            list.Add("hola");
            list.Add("juan");
            list.Add("Francisco");
            var ms = StringUtils.GetSqlInStringFromList(list);
            Assert.AreEqual("'hola', 'juan', 'Francisco'", ms);
        }
        [TestMethod]
        public void GetSqlInStringFromList2()
        {
            var list = new List<string>();
            list.Add("hola");
            var ms = StringUtils.GetSqlInStringFromList(list);
            Assert.AreEqual("'hola'", ms);
        }
    }
}
