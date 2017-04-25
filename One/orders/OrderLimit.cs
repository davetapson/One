using IBApi;
using One.Common;
using System;

namespace One.orders
{
    internal class OrderLimit
    {
        private GlobalValues.OrderType orderType;
        public OrderLimit()
        {
            orderType = GlobalValues.OrderType.LMT;
        }

        public GlobalValues.OrderAction Action { get; set; }
        public decimal LimitPrice { get; set; }
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
                if (LimitPrice <= 0) throw new Exception("OrderMarket: GetOrder error - invalid Limit Price");

                Order order = new Order();

                order.Action = this.Action.ToString();
                order.LmtPrice = Convert.ToDouble(this.LimitPrice);
                order.OrderType = this.OrderType.ToString();
                order.Tif = this.TimeInForce.ToString();
                order.TotalQuantity = this.Quantity;

                return order;

            }
        }
    }
}