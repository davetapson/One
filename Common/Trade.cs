using System;
using static Common.GlobalValues;

namespace Common
{
    public class Trade
    {
        public Trade()
        {

        }
        public int Id { get; set; }
        public int StrategyId { get; set; }
        public OrderAction Action { get; set; }
        public string Currency { get; set; }
        public string Exchange { get; set; }
        public string FileName { get; set; }
        public decimal LimitPrice { get; set; }
        public OrderType OrderType { get; set; }
        public string PrimaryExchange { get; set; }
        public int Quantity { get; set; }
        public OrderSecurityType SecurityType { get; set; }
        public int StatusId { get; set; }
        public string Symbol { get; set; }
        public OrderTimeInForce TimeInForce { get; set; }

        public bool IsValid()
        {
            bool isValidTrade = false;

            try
            {
                switch (OrderType)
                {
                    case GlobalValues.OrderType.LMT:
                        isValidTrade = TestLMTTrade();
                        break;
                    case GlobalValues.OrderType.MIT:
                        break;
                    case GlobalValues.OrderType.MKT:
                        isValidTrade = TestMKTTrade();
                        break;
                    case GlobalValues.OrderType.MTL:
                        break;
                    default:
                        isValidTrade = false;
                        break;
                }

                return isValidTrade;
            }
            catch (Exception ex)
            {
                // todo - log or save error
                throw new Exception("Common.Trade.IsValid error: " + ex);
            }
        }

        private bool TestMKTTrade()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                OrderType == OrderType.Undefined ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce == OrderTimeInForce.Undefined) result = false;

            return result;

        }

        private bool TestLMTTrade()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                LimitPrice <= 0 ||
                OrderType == OrderType.Undefined ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce == OrderTimeInForce.Undefined) result = false;

            return result;
        }
    }
}