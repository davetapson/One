using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One.Common
{
    public static class GlobalValues
    {
        public enum ErrorMessageTypes { APIError = 1, UnspecifiedIBError = 2, TWSError = 3, ApplicationError = 4}
        public enum OrderType { LMT, MIT, MKT, MTL}
        public enum OrderSecurityType { BOND, CFD, EFP, CASH, FUND, FUT, FOP, OPT, STK, WAR }
        public enum OrderSide { BOT, SLD }
        public enum OrderAction { BUY, SELL, SSHORT }
        public enum OrderTimeInForce { AUC, DAY, GTC, IOC, GTD, OPG, FOK, DTC }

    }
}
