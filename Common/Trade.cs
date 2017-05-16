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
        public decimal DiscretionaryAmount { get; set; }
        public decimal AuxPrice { get;  set; }
        public decimal TrailingPercent { get;  set; }
        public decimal TrailStopPrice { get;  set; }

        public bool IsValid()
        {
            bool isValidTrade = false;

            try
            {
                switch (OrderType)
                {                    
                    case GlobalValues.OrderType.LMT:
                        if (TimeInForce == OrderTimeInForce.AUC)
                        {
                            isValidTrade = TestDiscretionaryOrder();
                        }
                        else
                        {
                            isValidTrade = TestLimitOrder();
                        }                        
                        break;
                    case GlobalValues.OrderType.MIT:
                        isValidTrade = TestMarketIfTouchedOrder();
                        break;
                    case GlobalValues.OrderType.MKT:
                        if (TimeInForce == OrderTimeInForce.OPG)
                        {
                            isValidTrade = TestMarketOnOpenOrder();
                        }
                        else
                        {
                            isValidTrade = TestMarketOrder();
                        }                        
                        break;
                    case GlobalValues.OrderType.MTL:
                        if (TimeInForce == OrderTimeInForce.AUC)
                        {
                            isValidTrade = TestAuctionOrder();
                        }
                        break;
                    case GlobalValues.OrderType.MOC:
                        isValidTrade = TestMarketOnCloseOrder();
                        break;
                    case GlobalValues.OrderType.STP:
                        isValidTrade = TestStopOrder();
                        break;
                    case GlobalValues.OrderType.TRAIL:
                        if (TrailingPercent > 0)
                        {
                            isValidTrade = TestTrailingStopPercentOrder();

                        } else if (TrailStopPrice > 0)
                        {
                            isValidTrade = TestTrailingStopPriceOrder();
                        }                        
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

        private bool TestAuctionOrder()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                LimitPrice <= 0 ||
                OrderType != OrderType.MTL ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce != OrderTimeInForce.AUC) result = false;

            return result;
        }

        private bool TestDiscretionaryOrder() {

            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                LimitPrice <= 0 ||
                OrderType != OrderType.LMT ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce != OrderTimeInForce.AUC ||
                DiscretionaryAmount <= 0) result = false;

            return result;
        }
        private bool TestLimitOrder()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                LimitPrice <= 0 ||
                OrderType != OrderType.LMT ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce == OrderTimeInForce.Undefined) result = false;

            return result;
        }

        /// <summary>
        /// A Market order is an order to buy or sell at the market bid or offer price. A market order may increase the likelihood of a fill 
        /// and the speed of execution, but unlike the Limit order a Market order provides no price protection and may fill at a price far 
        /// lower/higher than the current displayed bid/ask.
        /// Products: BOND, CFD, EFP, CASH, FUND, FUT, FOP, OPT, STK, WAR
        /// </summary>
        private bool TestMarketOrder()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                OrderType != OrderType.MKT ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce == OrderTimeInForce.Undefined) result = false;

            return result;

        }

        /// <summary>
        /// A Market if Touched (MIT) is an order to buy (or sell) a contract below (or above) the market. Its purpose is to take advantage 
        /// of sudden or unexpected changes in share or other prices and provides investors with a trigger price to set an order in motion. 
        /// Investors may be waiting for excessive strength (or weakness) to cease, which might be represented by a specific price point. 
        /// MIT orders can be used to determine whether or not to enter the market once a specific price level has been achieved. This order 
        /// is held in the system until the trigger price is touched, and is then submitted as a market order. An MIT order is similar to a 
        /// stop order, except that an MIT sell order is placed above the current market price, and a stop sell order is placed below
        /// Products: BOND, CFD, CASH, FUT, FOP, OPT, STK, WAR
        /// </summary>
        private bool TestMarketIfTouchedOrder()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                OrderType != OrderType.MIT ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce != OrderTimeInForce.DAY ||
                AuxPrice <= 0) result = false;

            return result;
        }

        /// <summary>
        /// A Market-on-Close (MOC) order is a market order that is submitted to execute as close to the closing price as possible.
        /// Products: CFD, FUT, STK, WAR
        /// </summary>
        public bool TestMarketOnCloseOrder()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                OrderType != OrderType.MOC ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol)) result = false;

            return result;
        }

        /// <summary>
        /// A Market-on-Open (MOO) order combines a market order with the OPG time in force to create an order that is automatically
        /// submitted at the market's open and fills at the market price.
        /// Products: CFD, STK, OPT, WAR
        /// </summary>
        private bool TestMarketOnOpenOrder()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                OrderType != OrderType.MKT ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce != OrderTimeInForce.OPG) result = false;

            return result;
        }      

        private bool TestStopOrder()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                OrderType != OrderType.STP ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce == OrderTimeInForce.Undefined ||
                AuxPrice <= 0) result = false;

            return result;
        }

        /// <summary>
        /// A sell trailing stop order sets the stop price at a fixed amount below the market price with an attached "trailing" amount. As the 
        /// market price rises, the stop price rises by the trail amount, but if the stock price falls, the stop loss price doesn't change, 
        /// and a market order is submitted when the stop price is hit. This technique is designed to allow an investor to specify a limit on the 
        /// maximum possible loss, without setting a limit on the maximum possible gain. "Buy" trailing stop orders are the mirror image of sell 
        /// trailing stop orders, and are most appropriate for use in falling markets.
        /// Products: CFD, CASH, FOP, FUT, OPT, STK, WAR
        /// </summary>
        private bool TestTrailingStopPercentOrder()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                OrderType != OrderType.TRAIL ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce == OrderTimeInForce.Undefined ||
                TrailingPercent <= 0 ) result = false;

            return result;
        }

        /// <summary>
        /// A sell trailing stop order sets the stop price at a fixed amount below the market price with an attached "trailing" amount. As the 
        /// market price rises, the stop price rises by the trail amount, but if the stock price falls, the stop loss price doesn't change, 
        /// and a market order is submitted when the stop price is hit. This technique is designed to allow an investor to specify a limit on the 
        /// maximum possible loss, without setting a limit on the maximum possible gain. "Buy" trailing stop orders are the mirror image of sell 
        /// trailing stop orders, and are most appropriate for use in falling markets.
        /// Products: CFD, CASH, FOP, FUT, OPT, STK, WAR
        /// </summary>
        private bool TestTrailingStopPriceOrder()
        {
            bool result = true;

            if (StrategyId < 0 ||
                Action == OrderAction.Undefined ||
                string.IsNullOrEmpty(Currency) ||
                string.IsNullOrEmpty(Exchange) ||
                string.IsNullOrEmpty(FileName) ||
                OrderType != OrderType.TRAIL ||
                Quantity <= 0 ||
                string.IsNullOrEmpty(Symbol) ||
                TimeInForce == OrderTimeInForce.Undefined ||
                TrailStopPrice <= 0) result = false;

            return result;
        }
    }
}