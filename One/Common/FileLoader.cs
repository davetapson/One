using One.orders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static One.Common.GlobalValues;

namespace One.Common
{
    public class FileLoader
    {
        public FileLoader()
        {
            marketOrders = new List<OrderMarket>();
        }
        public string FileName { get; set; }
        private List<OrderMarket> marketOrders;

        public List<OrderMarket> MarketOrders
        {
            get { return marketOrders; }
            set { marketOrders = value; }
        }

        public void LoadFile(string FileName)
        {
            string[] lines = File.ReadAllLines(FileName);       

            string line1;
            string[] headers;
            string[] values;
            int orderTypeCol = -1;

            if (lines.Count() > 0)
            {
                line1 = lines[0];
                headers = line1.Split(',');

                if (headers.Contains("OrderType"))
                {
                    orderTypeCol = Array.IndexOf(headers, "OrderType");

                    for (int i = 1; i < lines.Count(); i++)
                    {
                        values = lines[i].Split(',');

                        switch (values[orderTypeCol])
                        {
                            case "MKT":
                                marketOrders.Add(CreateMarketOrder(headers, values));
                                break;
                            default:
                                break;
                        }
                    }
                }              
            }
        }

        private OrderMarket CreateMarketOrder(string[] headers, string[] values)
        {
            int index;
            OrderMarket order = new OrderMarket();

            try
            {
                index = Array.IndexOf(headers, "Action");
                if (index > -1) order.Action = (OrderAction)Enum.Parse(typeof(OrderAction), values[index].ToString(), true);

                index = Array.IndexOf(headers, "Quantity");
                if (index > -1) order.Quantity = Convert.ToInt32(values[index]);

                index = Array.IndexOf(headers, "Quantity");
                if (index > -1) order.TimeInForce = (OrderTimeInForce)Enum.Parse(typeof(OrderTimeInForce), values[index].ToString(), true); ;

            }
            catch (Exception)
            {
                throw;
            }

            return order;
        }
    }
}
