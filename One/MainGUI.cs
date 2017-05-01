using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IBApi;
using Data;

using One.orders;

using static Common.GlobalValues;

namespace One
{
    public partial class MainGUI : Form
    {
        Manager manager;
        GatewayCredentials gatewayCredentials;
       

        public MainGUI()
        {
            InitializeComponent();

            Text = "One";

            gatewayCredentials = new GatewayCredentials();
            manager = new Manager(gatewayCredentials, this);
            Connect();
        }

        private void Connect()
        {
            manager.Connect();
            manager.SubscribeAccount("DU271448");
            //manager.GetAccountPosition();
            manager.RequestThisClientsOpenOrders(gatewayCredentials.ClientId);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        internal void AddMessage(string v)
        {
            txtMessages.AppendText(v);
        }

        private void MainGUI_Load(object sender, EventArgs e)
        {
            
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            MakeOrder(OrderType.MKT, OrderAction.BUY);
        }

        private void MakeOrder(OrderType orderType, OrderAction orderAction)
        {
            Contract contract = new Contract();
            IBApi.Order order = new IBApi.Order();

            switch (orderType)
            {
                case OrderType.MTL:

                    // AUC
                    contract.Symbol = "IBKR";
                    contract.SecType = "STK";
                    contract.Currency = "USD";
                    contract.Exchange = "SMART";

                    OrderAuction orderAuction = new OrderAuction();
                    orderAuction.Action = orderAction;
                    orderAuction.LimitPrice = 10;
                    orderAuction.Quantity = 1;

                    order = orderAuction.GetOrder;                    

                    break;
                case OrderType.MKT:
                    
                    
                    contract.Symbol = "IBKR";
                    contract.SecType = "STK";
                    contract.Currency = "USD";
                    contract.Exchange = "SMART";

                    OrderMarket orderMarket = new OrderMarket();
                    orderMarket.Action = orderAction;
                    orderMarket.Quantity = 1;
                    orderMarket.TimeInForce = OrderTimeInForce.GTC;

                    order = orderMarket.GetOrder;

                    break;

                case OrderType.LMT:
                    
                    contract.Symbol = "IBKR";
                    contract.SecType = "STK";
                    contract.Currency = "USD";
                    contract.Exchange = "SMART";

                    OrderLimit orderLimit = new OrderLimit();

                    orderLimit.Action = orderAction;
                    orderLimit.LimitPrice = 10;
                    orderLimit.Quantity = 1;
                    orderLimit.TimeInForce = OrderTimeInForce.GTC;

                    order = orderLimit.GetOrder;

                    break;
            }

            manager.SubmitOrder(contract, order);
        }

        private void btnOrderLimit_Click(object sender, EventArgs e)
        {
            MakeOrder(OrderType.LMT, OrderAction.BUY);
        }

        private void btnOrderMKTShort_Click(object sender, EventArgs e)
        {
            MakeOrder(OrderType.MKT, OrderAction.SELL);
        }

        private void btnOrderLMTShort_Click(object sender, EventArgs e)
        {
            MakeOrder(OrderType.LMT, OrderAction.SELL);
        }

        private void btnTestDB_Click(object sender, EventArgs e)
        {
            TestDB();
        }

        private static void TestDB()
        {
            TestDB();
        }

        private void buttonOrderAUC_Click(object sender, EventArgs e)
        {
            MakeOrder(OrderType.MTL, OrderAction.BUY);
        }

        private void btnProcessFiles_Click(object sender, EventArgs e)
        {
            manager.ProcessFiles();
        }
    }
}
