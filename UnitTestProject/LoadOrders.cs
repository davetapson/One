using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;


using static UnitTestProject.Utils;
using One;
using Common;
using Data;

namespace UnitTestProject
{
    [TestClass]
    public class LoadOrders
    {
        [TestMethod]
        public void ArchiveTradesFile()
        {
            string tempSourceFolder = Utils.GetTestTradesFilesFolder();
            string fileName = "TestArchiveTradesFile" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            bool result = CreateMarketOrderFile(tempSourceFolder + fileName);
            // create test folder

            string tempTargetDirectory = @"processed\";

            // get file counts
            int tradesFilesCount = Directory.GetFiles(tempSourceFolder).Length;
            Directory.CreateDirectory(tempSourceFolder + tempTargetDirectory); // this needs to exist, although should be created by called proc
            int archivedTradesFilesCount = Directory.GetFiles(tempSourceFolder + tempTargetDirectory).Length;

            // archive file
            //FileLoader.MoveTradesFile(tempSourceFolder + fileName, tempTargetDirectory);

            // assert
            Assert.AreEqual(Directory.GetFiles(tempSourceFolder).Length, tradesFilesCount - 1);
            Assert.AreEqual(Directory.GetFiles(tempSourceFolder + tempTargetDirectory).Length, archivedTradesFilesCount + 1);
        }        

        [TestMethod]
        public void CreateMarketOrderFromTrades()
        {
            // create file
            
            // convert file to market orders            
            

            // asser that they are there
            
        }

        [TestMethod]
        public void SaveMarketTradesFromFile()
        {
            string tempSourceFolder = GetTestTradesFilesFolder();
            string fileName = GetFileNameCsv("SaveMarketOrderFromFile");
            fileName = tempSourceFolder + fileName;
            // create file
            bool result = CreateMarketOrderFile(fileName);

            // convert file to Market Orders
  //          List<Trade> trades = GetTradesFromFile(fileName);

            int countBefore = DBUtils.CountTrades();
            
            // load to db
//DBUtils.SaveTradesFile(trades);

   //         int countAfter = DBUtils.CountTrades();

            // assert extra rows in db = number of trades in file
  //          Assert.AreEqual(countAfter - countBefore, trades.Count);
        }

        
    }
}
