using IBApi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Orders
{
    public class OrderMarket
    {
        private GlobalValues.OrderType orderType;
        public OrderMarket()
        {
            orderType = GlobalValues.OrderType.MKT;
        }

        public GlobalValues.OrderAction Action { get; set; }
        public GlobalValues.OrderType OrderType
        {
            get { return orderType; }
        }
        public int Quantity { get; set; }
        public GlobalValues.OrderTimeInForce TimeInForce { get; set; }

        public Order GetOrder
        {
            get
            {
                if (Quantity <= 0) throw new Exception("OrderMarket: GetOrder error - invalid Quantity");

                Order order = new Order();

                order.Action = this.Action.ToString();
                order.OrderType = this.OrderType.ToString();
                order.Tif = this.TimeInForce.ToString();
                order.TotalQuantity = this.Quantity;

                return order;

            }
        }
    }
}
