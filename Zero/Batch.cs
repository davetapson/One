using Common;
using System;
using System.Collections.Generic;
using System.Threading;
using static Data.DBUtils;
using static Zero.FileHandler;

namespace Zero
{
    class Batch
    {
        internal void Run()
        {
            string tradesFilesPath = GlobalValues.TradesFilesFolder;
            List<string> tradesFilesNames;

            Console.WriteLine("Zero Batch started: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            while (true)
            {
                tradesFilesNames = GetTradesFilesNames(tradesFilesPath);
                SaveTradesFilesToDB(tradesFilesNames);

                // debug
                //Console.WriteLine(DateTime.Now);

                Thread.Sleep(1000);
            }
        }

        private void SaveTradesFilesToDB(List<string> tradesFilesNames)
        {
            foreach (string tradesFileName in tradesFilesNames)
            {
                List<Trade> trades = GetTradesFromTradesFile(tradesFileName);

                SaveTradesFileToDB(trades);
            }
        }        
    }
}
