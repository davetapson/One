using Common;
using IBApi;

using System;

namespace One.orders
{
    class OrderAuction
    {
        
        private GlobalValues.OrderType orderType;
        private GlobalValues.OrderTimeInForce timeInForce;

        public OrderAuction()
        {
            timeInForce = GlobalValues.OrderTimeInForce.AUC;
            orderType = GlobalValues.OrderType.MTL;
        }
        public GlobalValues.OrderAction Action { get; set; }
        public GlobalValues.OrderTimeInForce TimeInForce
        {
            get { return timeInForce; }
        }
        public GlobalValues.OrderType OrderType
        {
            get { return orderType; }
        }
        public int Quantity { get; set; }
        public decimal LimitPrice { get; set; }

        public Order GetOrder
        {
            get
            {
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
