using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTools.Utils;

namespace TechTools.Utils.UnitTest
{
    [TestClass]
    public class ValidacionesUT
    {
        [TestMethod]
        public void Celular()
        {
            Assert.AreEqual(true, ValidacionUtils.CelularValido("0984176766"));
        }
        [TestMethod]
        public void TelfConvencional()
        {
            Assert.AreEqual(true, ValidacionUtils.TelefonoConvencionalValido("032460156"));
        }
        [TestMethod]
        public void mailValido()
        {
            //diego_paulja@hotmail.com
            Assert.AreEqual(true, ValidacionUtils.EmailValid("marcosmiguel_17@hotmail.com"));
        }
    }
}
