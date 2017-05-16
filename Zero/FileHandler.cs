using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using static Common.GlobalValues;
using static Common.TradeTemplates.StockMarketOrder;

namespace Zero
{
    static class FileHandler
    {
        internal static List<string> GetTradesFilesNames(string tradesFilesFolder)
        {
            List<string> result = new List<string>();

            if (Directory.Exists(tradesFilesFolder))
            {
                string[] fileEntries = Directory.GetFiles(tradesFilesFolder);

                foreach (string fileName in fileEntries)
                {
                    if (File.Exists(fileName) &&
                        Path.GetExtension(fileName) == ".csv") result.Add(fileName);
                }
            }
            else
            {
                throw new Exception("Zero.FileHandler.GetTradesFilesNames: Invalid trades files folder path");
            }

            return result;
        }

        internal static List<Trade> GetTradesFromTradesFile(string tradesFileName)
        {
            List<Trade> trades = new List<Trade>();
            List<string> badTrades = new List<string>();

            string[] lines = File.ReadAllLines(tradesFileName);

            string[] headers;

            if (lines.Length > 0)
            {
                headers = lines[0].Split(',');

                for (int i = 1; i < lines.Length; i++)
                {
                    Trade trade = CreateTradeRecord(headers,
                                                 lines[i].Split(','),
                                                 Path.GetFileName(tradesFileName),
                                                 1 /*loaded from file*/);
                    if (trade != null)
                    {
                        trades.Add(trade);
                    }
                    else
                    {
                        if (badTrades.Count == 0) badTrades.Add(string.Join(",", headers));
                        badTrades.Add(lines[i].ToString());

                        //throw new Exception("Zero.FileHandler.GetTradesFromFile: Invalid trade");
                    }
                }

                ArchiveTradesFile(tradesFileName, TradesFilesProcessedFolder);

                if (badTrades.Count > 1) SaveFile(Path.GetDirectoryName(tradesFileName) + "//" + TradesFilesExceptionsFolder +
                                                  Path.GetFileNameWithoutExtension(tradesFileName) + " Exceptions " + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(tradesFileName),
                                                  badTrades);        
            }
            return trades;
        }

        private static void SaveFile(string tradesFileName, List<string> fileContents)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(tradesFileName));
            File.WriteAllLines(tradesFileName, fileContents);
        }

        private static Trade CreateTradeRecord(string[] headers,
                                                string[] values,
                                                string fileName,
                                                int statusId)
        {
            Trade trade = PopulateTradeObject(headers, values, fileName, statusId);

            if (!trade.IsValid()) trade = null;
           
            return trade;          
        }

        private static Trade PopulateTradeObject(string[] headers, string[] values, string fileName, int status_id)
        {
            Trade trade = new Trade();
            int index;

            trade.FileName = fileName;
            StatusId = status_id;

            try
            {
                index = Array.IndexOf(headers, "StrategyId");
                if (index > -1) trade.StrategyId = Convert.ToInt32(values[index]);

                index = Array.IndexOf(headers, "Symbol");
                if (index > -1) trade.Symbol = values[index].ToString();

                index = Array.IndexOf(headers, "Quantity");
                if (index > -1) trade.Quantity = Convert.ToInt32(values[index]);

                index = Array.IndexOf(headers, "SecurityType");
                if (index > -1) trade.SecurityType = (OrderSecurityType)Enum.Parse(typeof(OrderSecurityType), values[index].ToString(), true);

                index = Array.IndexOf(headers, "Action");
                if (index > -1) trade.Action = (OrderAction)Enum.Parse(typeof(OrderAction), values[index].ToString(), true);

                index = Array.IndexOf(headers, "OrderType");
                if (index > -1) trade.OrderType = (OrderType)Enum.Parse(typeof(OrderType), values[index].ToString(), true);

                index = Array.IndexOf(headers, "Currency");
                if (index > -1) trade.Currency = values[index].ToString();

                index = Array.IndexOf(headers, "Exchange");
                if (index > -1) trade.Exchange = values[index].ToString();

                index = Array.IndexOf(headers, "PrimaryExchange");
                if (index > -1) trade.PrimaryExchange = values[index].ToString();

                index = Array.IndexOf(headers, "TimeInForce");
                if (index > -1) trade.TimeInForce = (OrderTimeInForce)Enum.Parse(typeof(OrderTimeInForce), values[index].ToString(), true);

                index = Array.IndexOf(headers, "LimitPrice");
                if (index > -1) trade.LimitPrice = Convert.ToDecimal(values[index], CultureInfo.InvariantCulture);

                index = Array.IndexOf(headers, "AuxPrice");
                if (index > -1) trade.AuxPrice = Convert.ToDecimal(values[index].ToString(), CultureInfo.InvariantCulture);

                index = Array.IndexOf(headers, "TrailingPercent");
                if (index > -1) trade.TrailingPercent = Convert.ToDecimal(values[index], CultureInfo.InvariantCulture);

                index = Array.IndexOf(headers, "TrailStopPrice");
                if (index > -1) trade.TrailStopPrice = Convert.ToDecimal(values[index], CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                return new Trade();
                // to do - log or save in exceptions file
                //throw new Exception("Zero.FileHandler.LoadTrade: Error - " + ex);
            }

            return trade;
        }

        public static void ArchiveTradesFile(string fileName, string toDirectoryName)
        {
            try
            {
                string toFolder = Path.GetDirectoryName(fileName) + "\\" + toDirectoryName;
                Directory.CreateDirectory(toFolder);

                string newFileName = toFolder + Path.GetFileNameWithoutExtension(fileName) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(fileName);

                File.Move(fileName, newFileName);
            }
            catch (Exception ex)
            {
                // to do - log or save in exceptions file
                throw new Exception("Zero.FileHandler.ArchiveTradesFile: Error - " + ex);
            }
        }

        public static void MoveTradesFile(string fileName, string toDirectoryName)
        {
            try
            {
                string toFolder = Path.GetDirectoryName(fileName) + "\\" + toDirectoryName;
                Directory.CreateDirectory(toFolder);

                string toFileName = toFolder + Path.GetFileName(fileName);

                File.Move(fileName, toFileName);
            }
            catch (Exception ex)
            {
                // to do - log or save in exceptions file
                throw new Exception("Zero.FileHandler.MoveTradesFile: Error - " + ex);
            }
        }
    }
}
