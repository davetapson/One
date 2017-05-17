using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data_Test
{
    [TestClass]
    public class TestData
    {
        [TestMethod]
        public void TestGetConfig()
        {
            string val =  Data.DBUtils.GetConfigValue("TestKey");

            Assert.AreEqual("TestValue", val);
        }

        [TestMethod]
        public void GetNextOrderNo()
        {
            int lastOrderNo = Convert.ToInt32( Data.DBUtils.GetConfigValue("NextOrderNo"));

            int nextOrderNo = Data.DBUtils.GetNextOrderNo();

            Assert.AreEqual(true, lastOrderNo <= nextOrderNo);
        }
    }
}
