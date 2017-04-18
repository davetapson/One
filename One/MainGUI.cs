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
            MakeOrder();
        }

        private void MakeOrder()
        {
            IBApi.Contract contract = new IBApi.Contract();
            contract.Symbol = "IBKR";
            contract.SecType = "STK";
            contract.Currency = "USD";
            contract.Exchange = "SMART";

            Order order = new Order();
            order.Action = "BUY";
            order.OrderType = "MKT";
            order.TotalQuantity = 1;
            
            manager.SubmitOrder(contract, order);
        }
    }
}
