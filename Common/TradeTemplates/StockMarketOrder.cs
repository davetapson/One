using static Common.GlobalValues;

namespace Common.TradeTemplates
{
    public static class StockMarketOrder
    {
        public static int Id { get; set; }
        public static int StrategyId { get; set; }
        public static OrderAction Action { get; set; }
        public static string Currency { get; set; }
        public static string Exchange { get; set; }
        public static string FileName { get; set; }
        public static OrderType OrderType { get; set; }
        public static string PrimaryExchange { get; set; }
        public static int Quantity { get; set; }
        public static OrderSecurityType SecurityType { get; set; }
        public static int StatusId { get; set; }
        public static string Symbol { get; set; }
        public static OrderTimeInForce TimeInForce { get; set; }

        

        public static bool TestTrade(Trade trade)
        {
            if (!string.IsNullOrEmpty(Currency) &&
               !string.IsNullOrEmpty(Exchange) &&
               Quantity > 0 &&
               !string.IsNullOrEmpty(Symbol))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
