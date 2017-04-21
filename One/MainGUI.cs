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
using One.db;

namespace One
{
    public partial class MainGUI : Form
    {
        Manager manager;
        GatewayCredentials gatewayCredentials;
        enum OrderType { MKT, LMT}
        enum OrderSide { BOT, SLD}
        enum OrderAction { BUY, SELL, SSHORT}
        enum TimeInForce { DAY, GTC, IOC, GTD, OPG, FOK, DTC}

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
                case OrderType.MKT:
                    
                    contract.Symbol = "IBKR";
                    contract.SecType = "STK";
                    contract.Currency = "USD";
                    contract.Exchange = "SMART";

                    order.Action = orderAction.ToString();
                    order.OrderType = "MKT";
                    order.TotalQuantity = 1;
                    order.Tif = TimeInForce.GTC.ToString();

                    break;

                case OrderType.LMT:
                    
                    contract.Symbol = "IBKR";
                    contract.SecType = "STK";
                    contract.Currency = "USD";
                    contract.Exchange = "SMART";


                    order.Action = orderAction.ToString();
                    order.OrderType = "LMT";
                    order.LmtPrice = 10;
                    order.TotalQuantity = 1;
                    order.Tif = TimeInForce.GTC.ToString();
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
            DBUtils.TestDB();
        }
    }
}
