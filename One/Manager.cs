using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using One.messages;
using One.db;

namespace One
{
    class Manager
    {
        private GatewayCredentials gatewayCredentials;
        private MainGUI mainGUI;
        EClientSocket clientSocket;

        static Responder responder;

        private int nextOrderNo;

        public int NextOrderNo
        {
            get {
                    nextOrderNo++;
                    return nextOrderNo;
                }
            set { nextOrderNo = value; }
        }


        public Manager(GatewayCredentials gatewayCredentials)
        {
            this.gatewayCredentials = gatewayCredentials;
        }

        internal void GetAccountPosition()
        {
            clientSocket.reqPositions();
        }

        private void showNextValidId(ConnectionStatusMessage obj)
        {
            if (obj.IsConnected) IsConnected = true;
        }

        internal void SubscribeAccount(string accountCode)
        {
            clientSocket.reqAccountUpdates(true, accountCode);
        }

        internal void RequestThisClientsOpenOrders(int clientId)
        {
            clientSocket.reqOpenOrders();
        }

        public Manager(GatewayCredentials gatewayCredentials, MainGUI mainGUI) : this(gatewayCredentials)
        {
            this.mainGUI = mainGUI;
        }

        public bool IsConnected { get; private set; }

        internal void Connect()
        {
            if (!IsConnected)
            {                
                try
                {
                    HandleErrorMessage(new ErrorMessage(-1, -1, "Connecting..."));

                    responder = new Responder();

                    clientSocket = responder.ClientSocket;
                    EReaderSignal readerSignal = responder.Signal;

                    clientSocket.eConnect(gatewayCredentials.Host, gatewayCredentials.Port, gatewayCredentials.ClientId);
                    
                    EReader reader = new EReader(clientSocket, readerSignal);

                    reader.Start();

                    new Thread(() => { while (clientSocket.IsConnected()) { readerSignal.waitForSignal();
                                                                                     reader.processMsgs(); } }) { IsBackground = true }.Start();

                    while (responder.NextOrderId <= 0)
                    {
                        
                    }

                    NextOrderNo = responder.NextOrderId;

                    IsConnected = true;

                    HandleErrorMessage(new ErrorMessage(-1, -1, "Connected."));

                }
                catch (Exception)
                {
                    HandleErrorMessage(new ErrorMessage(-1, -1, "Please check your connection attributes."));
                }
            }
            else
            {
                IsConnected = false;

                HandleErrorMessage(new ErrorMessage(-1, -1, "Disconnected."));
            }
        }

        internal void SubmitOrder(Contract contract, Order order)
        {
            clientSocket.placeOrder(NextOrderNo, contract, order);
        }

        private void ConnectToClient(IBClient ibClient)
        {
            // connect
            ibClient.ClientSocket.eConnect(gatewayCredentials.Host,
                                           gatewayCredentials.Port,
                                           gatewayCredentials.ClientId);
        }

        private void HandleErrorMessage(ErrorMessage message)
        {
            mainGUI.AddMessage("Request " + message.RequestId + ", Code: " + message.ErrorCode + " - " + message.Message + "\n");

            DBUtils.SaveErrorMessage(message);

           /* if (message.RequestId > MarketDataManager.TICK_ID_BASE && message.RequestId < DeepBookManager.TICK_ID_BASE)
                marketDataManager.NotifyError(message.RequestId);
            else if (message.RequestId > DeepBookManager.TICK_ID_BASE && message.RequestId < HistoricalDataManager.HISTORICAL_ID_BASE)
                deepBookManager.NotifyError(message.RequestId);
            else if (message.RequestId == ContractManager.CONTRACT_DETAILS_ID)
            {
                contractManager.HandleRequestError(message.RequestId);
                searchContractDetails.Enabled = true;
            }
            else if (message.RequestId == ContractManager.FUNDAMENTALS_ID)
            {
                contractManager.HandleRequestError(message.RequestId);
                fundamentalsQueryButton.Enabled = true;
            }
            else if (message.RequestId == OptionsManager.OPTIONS_ID_BASE)
            {
                optionsManager.Clear();
                queryOptionChain.Enabled = true;
            }
            else if (message.RequestId > OptionsManager.OPTIONS_ID_BASE)
            {
                queryOptionChain.Enabled = true;
            }
            if (message.ErrorCode == 202)
            {
            }*/
        }
        void ibClient_Error(int id, int errorCode, string str, Exception ex)
        {
            if (ex != null)
            {
                ErrorMessage e = new ErrorMessage(-1, -1, ex.Message);
                HandleErrorMessage(e);

                return;
            }

            if (id == 0 || errorCode == 0)
            {
                ErrorMessage e = new ErrorMessage(-1, -1, "Error: " + str + "\n");
                HandleErrorMessage(e);
                
                return;
            }

            ErrorMessage error = new ErrorMessage(id, errorCode, str);

            HandleErrorMessage(error);
        }
    }
}

