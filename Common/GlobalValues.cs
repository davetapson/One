using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class GlobalValues
    {
        private static string tradesFilesFolder = @"C:\Automated Trading\Trades Files\";
        private static string tradesFilesProcessedFolder = @"processed\";
        private static string tradesFilesExceptionsFolder = @"failed\";

        public static string TradesFilesFolder { get { return tradesFilesFolder; } }
        public static string TradesFilesProcessedFolder { get { return tradesFilesProcessedFolder; }  }
        public static string TradesFilesExceptionsFolder { get { return tradesFilesExceptionsFolder; } }

        public enum ErrorMessageTypes { APIError = 1, UnspecifiedIBError = 2, TWSError = 3, ApplicationError = 4}
        public enum OrderType {Undefined, LMT, MIT, MKT, MTL}
        public enum OrderSecurityType { Undefined, BOND, CFD, EFP, CASH, FUND, FUT, FOP, OPT, STK, WAR }
        public enum OrderSide { Undefined, BOT, SLD }
        public enum OrderAction { Undefined, BUY, SELL, SSHORT }
        public enum OrderTimeInForce { Undefined, AUC, DAY, GTC, IOC, GTD, OPG, FOK, DTC }

    }
}
