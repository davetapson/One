using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using One.orders;
using One.Common;

namespace UnitTestProject
{
    [TestClass]
    public class LoadOrders
    {
        [TestMethod]
        public void CreateMarketOrderFromFile()
        {
            // create file
            List<string> trades = new List<string>();
            trades.Add("OrderType,Action,Symbol,Quantity,TimeInForce,Exchange,Currency");
            trades.Add("MKT,BUY,IBKR,1,DAY,SMART,USD");
            trades.Add("MKT,SELL,AAPL,1,DAY,SMART,USD");

            // save file
            string dir = @"C:\Automated Trading\Trades Files";
            Directory.CreateDirectory(dir);
            string fileName = dir + "\\MarketOrder_Test.csv";
            File.WriteAllLines(fileName, trades.ToArray());

            // read file into list of Market Order object
            FileLoader fileLoader = new FileLoader();
            fileLoader.LoadFile(fileName);
            List<OrderMarket> marketOrders = fileLoader.MarketOrders;

            // asser that they are there
            Assert.AreEqual(marketOrders.Count, 2);
            Assert.AreEqual(marketOrders[0].Action, GlobalValues.OrderAction.BUY);
            Assert.AreEqual(marketOrders[1].Action, GlobalValues.OrderAction.SELL);
        }
    }
}
