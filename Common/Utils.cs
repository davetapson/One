using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Utils
    {
        public static int GetRunningDaysFromYear(DateTime date)
        {
            int result;

            result = Convert.ToInt32((date - new DateTime(2014, 1, 1)).TotalDays);

            return result;
        }

        public static DateTime GetDateFromRunningDays(int runningDays)
        {
            DateTime result;

            DateTime startDate = new DateTime(2014, 1, 1);

            result = startDate.AddDays(runningDays);

            return result;
        }

        public static void TestDays()
        {
            DateTime startDate = new DateTime(2014, 1, 1, 22, 03, 1);

            DateTime endDate = startDate.AddDays(1000);

            double days = (endDate - startDate).TotalDays;

        }

       
    }
}
