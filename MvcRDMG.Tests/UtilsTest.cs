using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRDMG.Generator.Helpers;

namespace MvcRDMG.Tests
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void GetRandomIntTest()
        {
            var x = Utils.GetRandomInt(1, 10);
            Assert.IsTrue(x > 0);
            Assert.IsTrue(x < 10);
        }

    }
}