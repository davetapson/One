using Common;
using One;

using One.orders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class Utils
    {
        public static string GetTestTradesFilesFolder()
        {
            // create file
            string tempSourceFolder = @"C:\temp\tests\";
            Directory.CreateDirectory(tempSourceFolder);
            return tempSourceFolder;
        }
     /*   public static List<Trade> GetTradesFromFile(string fileName)
        {
            // read file into list of Trade object
            FileLoader fileLoader = new FileLoader();
            return fileLoader.LoadFile(fileName);

        }*/
        public static bool CreateMarketOrderFile(string fileName)
        {
            bool result = false;

            try
            {
                // create file
                List<string> trades = new List<string>();
                trades.Add("StrategyId,OrderType,Action,Symbol,Quantity,TimeInForce,Exchange,Currency,SecurityType");
                trades.Add("5,MKT,BUY,TESTSYMBOL1,1,DAY,SMART,USD,STK");
                trades.Add("5,MKT,SELL,TESTSYMBOL2,1,DAY,SMART,USD,STK");

                // save file
                string folder = Path.GetDirectoryName(fileName);
                Directory.CreateDirectory(folder);
                File.WriteAllLines(fileName, trades.ToArray());

                result = true;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        public static string GetFileNameCsv(string fileName)
        {
            return fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        }
    }
}
