using System.Collections.Generic;
using static Common.GlobalValues;
using System;
using One.orders;
using IBApi;
using Common;
using static Data.DBUtils;
using Data;
using Data;

namespace One
{
    class OrderManager
    {
        public int NextOrderId { get; set; }

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

                DBUtils.SavePlaceOrder(nextValidId, contract, marketOrder.GetOrder, strategy);
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
