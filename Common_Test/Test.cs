using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common_Test
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestGetRunnngDaysFromYear()
        {
            int runningDays;

            runningDays = Common.Utils.GetRunningDaysFromYear(new DateTime(2017, 12, 31));

            Assert.AreEqual(runningDays, 364);
        }

        [TestMethod]
        public void TestGetYearFromRunnngDays()
        {
            DateTime date;

            date = Common.Utils.GetDateFromRunningDays(364);

            Assert.AreEqual(date, new DateTime(2017,12,31));
        }

        [TestMethod]
        public void TestDays()
        {
            Common.Utils.TestDays();
        }
    }
}
