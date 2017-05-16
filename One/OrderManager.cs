using System.Collections.Generic;
using static Common.GlobalValues;
using System;
using One.orders;
using IBApi;
using Common;
using static Data.DBUtils;
using Data;


namespace One
{
    class OrderManager
    {
        public int NextOrderId { get; set; }
        public bool doProcessOrders { get; set; }

        public void CreateOrdersFromTrades(int nextValidId, IBApi.EClient ibClient)
        {
            List<Trade> unprocessedTrades = new List<Trade>();
            unprocessedTrades = DBUtils.GetUnprocessedTrades();
            
            foreach(Trade unprocessedTrade in unprocessedTrades)
            {
                 DBUtils.UpdateTrade(unprocessedTrade, CreateMarketOrder(unprocessedTrade, nextValidId, ibClient));
                
            } 
                
            }
        

        private bool CreateMarketOrder(Trade unprocessedTrade, int nextValidId, IBApi.EClient clientSocket)
        {
            bool result = false;

            try
            {
                OrderMarket marketOrder = new OrderMarket();
                marketOrder.Action = unprocessedTrade.Action;
                marketOrder.Quantity = unprocessedTrade.Quantity;
                marketOrder.TimeInForce = unprocessedTrade.TimeInForce;

                Contract contract = new Contract();
                contract.Symbol = unprocessedTrade.Symbol;
                contract.SecType = unprocessedTrade.SecurityType.ToString();
                contract.Currency = unprocessedTrade.Currency;
                contract.Exchange = unprocessedTrade.Exchange;
                if (!string.IsNullOrEmpty(unprocessedTrade.PrimaryExchange))
                {
                    contract.PrimaryExch = unprocessedTrade.PrimaryExchange;
                }else
                {
                    string primaryExchange = CheckPrimaryExchangeForSymbol(contract.Symbol);
                    if (!string.IsNullOrEmpty(primaryExchange)) contract.PrimaryExch = primaryExchange;
                }

                Strategy strategy = new Strategy();
                strategy.ID = unprocessedTrade.StrategyId;

                DBUtils.SavePlaceOrder(nextValidId, contract, marketOrder.GetOrder);//, strategy);
                clientSocket.placeOrder(nextValidId, contract, marketOrder.GetOrder);

                result = true;
            }
            catch (Exception e)
            {
                SaveError("CreateMarketOrder error: " + e.Message);
                result = false;
            }

            return result;
        }

        public void ProcessOrders(EClientSocket socket)
        {
            List<Tuple<Contract, Order>> unprocessedOrders = GetUnProcessedOrders();
            int nextOrderId = 0;

            foreach (Tuple<Contract, Order> order in unprocessedOrders)
            {
                nextOrderId = Manager.NextOrderNo;
                DBUtils.SavePlaceOrder(nextOrderId, order.Item1, order.Item2);
                socket.placeOrder(nextOrderId, order.Item1, order.Item2);                
            }
        }

        internal List<Tuple<Contract, Order>> GetUnProcessedOrders()
        {
            List<Tuple<Contract, Order>> orders = new List<Tuple<Contract, Order>>();

            //while (doProcessOrders)
            //{
                List<Trade> unprocessedTrades = DBUtils.GetUnprocessedTrades();

                foreach (Trade trade in unprocessedTrades)
                {
                    switch (trade.OrderType)
                    {
                        case OrderType.MKT:
                            orders.Add(MakeMarketOrder(trade));
                            break;
                        case OrderType.LMT:
                            orders.Add(MakeLimitOrder(trade));
                            break;
                        default:
                            throw new Exception("One.OrderManager.ProcessOrders Error: Unrecognised Order Type");
                    }
                }
          //  }

            return orders;
        }

        private Tuple<Contract, Order> MakeMarketOrder(Trade trade)
        {
            try
            {
                Contract contract = new Contract();
                contract.Symbol = trade.Symbol;
                contract.SecType = trade.SecurityType.ToString();
                contract.Currency = trade.Currency;
                contract.Exchange = trade.Exchange;
                if (!string.IsNullOrEmpty(trade.PrimaryExchange)) contract.PrimaryExch = trade.PrimaryExchange;

                Order order = new Order();
                order.ClientId = trade.StrategyId;
                order.OrderType = trade.OrderType.ToString();
                order.Action = trade.Action.ToString();
                order.TotalQuantity = trade.Quantity;
                order.Tif = trade.TimeInForce.ToString();

                Tuple<Contract, Order> tuple = new Tuple<Contract, Order>(contract, order);
                return tuple;
            }
            catch (Exception ex)
            {
                throw new Exception("One.OrderManager.MakeMarketOrder Error: " + ex.Message);
            }
        }

        private Tuple<Contract, Order> MakeLimitOrder(Trade trade)
        {
            Contract contract = new Contract();
            contract.Symbol = trade.Symbol;
            contract.SecType = trade.SecurityType.ToString();
            contract.Currency = trade.Currency;
            contract.Exchange = trade.Exchange;
            if (!string.IsNullOrEmpty(trade.PrimaryExchange)) contract.PrimaryExch = trade.PrimaryExchange;

            Order order = new Order();
            order.OrderType = trade.OrderType.ToString();
            order.Action = trade.Action.ToString();
            order.TotalQuantity = trade.Quantity;
            order.LmtPrice = (double)trade.LimitPrice;
            order.Tif = trade.TimeInForce.ToString();

            Tuple<Contract, Order> tuple = new Tuple<Contract, Order>(contract, order);
            return tuple;
        }
              

        private string CheckPrimaryExchangeForSymbol(string symbol)
        {
            string result = string.Empty;

            // todo db-ise this
            switch (symbol){
                case "MSFT":
                    result = "NASDAQ";
                    break;
            }

            return result;
        }
    }
}
