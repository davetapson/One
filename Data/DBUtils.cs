
using Common;
using Common.Orders;
using IBApi;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Common.GlobalValues;
using Common.messages;

namespace Data
{
    public static class DBUtils
    {
        static string connectionString = //"Data Source = DESKTOP-F67C4NI; Initial Catalog = AutoTrader; User ID = sa; Password=Makatini@1";
        "Server=DESKTOP-F67C4NI;Database=AutoTrader;User Id=sa;Password=Makatini@1";

        public static List<Trade> GetUnprocessedTrades()
        {
            List<Trade> unprocessedTrades = new List<Trade>();

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //SqlCommand sqlCommand = new SqlCommand("GetUnprocessedTrades", sqlConnection);
            //sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlConnection.Open();
            /*sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();*/


            using (var sqlCommand = new SqlCommand("GetUnprocessedTrades", sqlConnection))
            {
                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var trade = new Trade();

                        trade.Action = (OrderAction)Enum.Parse(typeof(OrderAction), reader["Action"].ToString());
                        trade.Currency = reader["Currency"].ToString();
                        trade.Exchange = reader["Exchange"].ToString();
                        trade.FileName = reader["file_name"].ToString();
                        trade.Id = Convert.ToInt32(reader["Id"]);
                        trade.OrderType = (OrderType)Enum.Parse(typeof(OrderType), reader["Order_Type"].ToString());
                        trade.PrimaryExchange = reader["Primary_Exchange"].ToString();
                        trade.Quantity = Convert.ToInt32(reader["Quantity"]);
                        trade.SecurityType = (OrderSecurityType)Enum.Parse(typeof(OrderSecurityType), reader["Security_Type"].ToString());
                        trade.StatusId = Convert.ToInt32(reader["Status_Id"]);
                        trade.StrategyId = Convert.ToInt32(reader["Strategy_Id"]);
                        trade.Symbol = reader["Symbol"].ToString();
                        if (reader["Time_In_Force"] != null) trade.TimeInForce = (OrderTimeInForce)Enum.Parse(typeof(OrderTimeInForce), reader["Time_In_Force"].ToString());

                        unprocessedTrades.Add(trade);
                    }
                }
            }
            sqlConnection.Close();
            return unprocessedTrades;
        }

        public static string GetConfigValue(string keyValue)
        {
            string value;

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            using (var sqlCommand = new SqlCommand("GetConfigValue", sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@key", keyValue);

                value = sqlCommand.ExecuteScalar().ToString();
            }
            sqlConnection.Close();

            return value;
        }

        public static int GetNextOrderNo()
        {
            int NextOrderNo;

            string lastOrderNo = GetConfigValue("NextOrderNo");

            int runningDays = Convert.ToInt32( lastOrderNo.Substring(0, 4));

            if (Common.Utils.GetDateFromRunningDays(runningDays).ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                NextOrderNo = Convert.ToInt32(lastOrderNo) + 1;
                SetConfigValue("NextOrderNo", NextOrderNo.ToString());
            }
            else
            {
                NextOrderNo = Convert.ToInt32(Common.Utils.GetRunningDaysFromYear(DateTime.Now).ToString() + "0");
                SetConfigValue("NextOrderNo", NextOrderNo.ToString());
            }

             return NextOrderNo;
        }

        public static int SetConfigValue(string key, string value)
        {
            int id;

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            using (var sqlCommand = new SqlCommand("setConfigValue", sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@key", key);
                sqlCommand.Parameters.AddWithValue("@value", value);

                id = Convert.ToInt32(sqlCommand.ExecuteNonQuery());
            }
            sqlConnection.Close();

            return id;
        }
        public static void UpdateTrade(Trade unprocessedTrade, bool success)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("UpdateTrades", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@id", unprocessedTrade.Id);
                sqlCommand.Parameters.AddWithValue("@success", success);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errMessage = "UpdateTrade failed: " + ex;
                Console.Write(errMessage);
            }
        }

        static public int SaveOrder(Order order)
        {
            int result = -1;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveOrder", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // do order interface methods here
                sqlCommand.Parameters.AddWithValue("@client_id", order.ClientId);
                sqlCommand.Parameters.AddWithValue("@order_id", order.OrderId);
                //sqlCommand.Parameters.AddWithValue("@perm_id", order.PermId);
                sqlCommand.Parameters.AddWithValue("@action", order.Action);
                sqlCommand.Parameters.AddWithValue("@quantity", order.TotalQuantity);
                sqlCommand.Parameters.AddWithValue("@order_type", order.OrderType);
                //sqlCommand.Parameters.AddWithValue("@limit_price", Convert.ToDecimal(order.LmtPrice));
                //sqlCommand.Parameters.AddWithValue("@aux_price", Convert.ToDecimal(order.AuxPrice));
                sqlCommand.Parameters.AddWithValue("@time_in_force", order.Tif);
                //sqlCommand.Parameters.AddWithValue("@active_start_time", order.ActiveStartTime);
                //sqlCommand.Parameters.AddWithValue("@active_stop_time", order.ActiveStopTime);
                //sqlCommand.Parameters.AddWithValue("@oca_group", order.OcaGroup);
                //sqlCommand.Parameters.AddWithValue("@oca_type", order.OcaType);
                //sqlCommand.Parameters.AddWithValue("@order_reference", order.OrderRef);
                //sqlCommand.Parameters.AddWithValue("@transmit", order.Transmit);
                //sqlCommand.Parameters.AddWithValue("@parent_id", order.ParentId);
                //sqlCommand.Parameters.AddWithValue("@block_order", order.BlockOrder);
                //sqlCommand.Parameters.AddWithValue("@sweep_to_fill", order.SweepToFill);
                //sqlCommand.Parameters.AddWithValue("@display_size", order.DisplaySize);
                //sqlCommand.Parameters.AddWithValue("@trigger_method", order.TriggerMethod);
               // sqlCommand.Parameters.AddWithValue("@outside_rth", order.OutsideRth);
                //sqlCommand.Parameters.AddWithValue("@hidden", order.Hidden);
                //sqlCommand.Parameters.AddWithValue("@good_after_time", order.GoodAfterTime);
                //sqlCommand.Parameters.AddWithValue("@good_till_date", order.GoodTillDate);
                //sqlCommand.Parameters.AddWithValue("@override_percentage_constraints", order.OverridePercentageConstraints);

                sqlConnection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "SaveOrder insert failed: " + ex;
                Console.Write(errorMessage);
                SaveErrorMessage(errorMessage, GlobalValues.ErrorMessageTypes.ApplicationError);
            }

            return result;
        }

        public static int SavePlaceOrder(int nextOrderId, Contract contract, Order order)//, Strategy strategy)
        {
            int result = -1;

            int contractId = SaveContract(contract);
            int orderId = SaveOrder(order);

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand("SavePlaceOrder", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();

            try
            {
                sqlCommand.Parameters.AddWithValue("@orderId", nextOrderId);
                sqlCommand.Parameters.AddWithValue("@strategy_id", order.ClientId);
                sqlCommand.Parameters.AddWithValue("@contract_id", contractId);
                sqlCommand.Parameters.AddWithValue("@order_id", orderId);
                sqlCommand.Parameters.AddWithValue("@status_id", 9);  // unprocessed

                result = Convert.ToInt32(sqlCommand.ExecuteScalar());

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int UpdatePlacedOrder(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld)
        {
            int result = -1;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("UpdatePlacedOrder", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@order_id", orderId);
                sqlCommand.Parameters.AddWithValue("@status_id", 11); // updated
                sqlCommand.Parameters.AddWithValue("@status", status);
                sqlCommand.Parameters.AddWithValue("@filled", filled);
                sqlCommand.Parameters.AddWithValue("@remaining", remaining);
                sqlCommand.Parameters.AddWithValue("@average_fill_price", avgFillPrice);
                sqlCommand.Parameters.AddWithValue("@perm_id", permId);
                sqlCommand.Parameters.AddWithValue("@parent_id", parentId);
                sqlCommand.Parameters.AddWithValue("@last_filled_price", lastFillPrice);
                sqlCommand.Parameters.AddWithValue("@client_id", clientId);
                sqlCommand.Parameters.AddWithValue("@why_held", whyHeld);

                sqlConnection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "SaveOrderStatus insert failed: " + ex;
                Console.Write(errorMessage);
                SaveErrorMessage(errorMessage, GlobalValues.ErrorMessageTypes.ApplicationError);
            }

            return result; 
        }

        public static void SaveOrders(List<OrderMarket> marketOrders)
        {
            throw new NotImplementedException();
        }

        static public void SaveTradesFileToDB( List<Trade> trades)
        {

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand("SaveTrades", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();

            foreach (Trade trade in trades)
            {
                try
                {
                    sqlCommand.Parameters.Clear();

                    if (!string.IsNullOrEmpty(trade.FileName)) sqlCommand.Parameters.AddWithValue("@file_name", trade.FileName);
                    sqlCommand.Parameters.AddWithValue("@strategy_id", trade.StrategyId);
                    if (!string.IsNullOrEmpty(trade.Symbol)) sqlCommand.Parameters.AddWithValue("@symbol", trade.Symbol);
                    sqlCommand.Parameters.AddWithValue("@quantity", trade.Quantity);
                    if (!string.IsNullOrEmpty(trade.SecurityType.ToString())) sqlCommand.Parameters.AddWithValue("@security_type", trade.SecurityType.ToString());
                    if (!string.IsNullOrEmpty(trade.Action.ToString())) sqlCommand.Parameters.AddWithValue("@action", trade.Action.ToString());
                    if (!string.IsNullOrEmpty(trade.OrderType.ToString())) sqlCommand.Parameters.AddWithValue("@order_type", trade.OrderType.ToString());
                    if (!string.IsNullOrEmpty(trade.Currency)) sqlCommand.Parameters.AddWithValue("@currency", trade.Currency);
                    if (!string.IsNullOrEmpty(trade.Exchange)) sqlCommand.Parameters.AddWithValue("@exchange", trade.Exchange);
                    if (!string.IsNullOrEmpty(trade.PrimaryExchange)) sqlCommand.Parameters.AddWithValue("@primary_exchange", trade.PrimaryExchange);
                    if (trade.TimeInForce != OrderTimeInForce.Undefined) sqlCommand.Parameters.AddWithValue("@time_in_force", trade.TimeInForce.ToString());
                    if (trade.LimitPrice > 0) sqlCommand.Parameters.AddWithValue("@limit_price", trade.LimitPrice);
                    if (trade.DiscretionaryAmount > 0) sqlCommand.Parameters.AddWithValue("@discretionary_amount", trade.DiscretionaryAmount);
                    if (trade.AuxPrice > 0) sqlCommand.Parameters.AddWithValue("@aux_price", trade.AuxPrice);
                    if(trade.TrailingPercent > 0) sqlCommand.Parameters.AddWithValue("@trailing_percent", trade.TrailingPercent);
                    if (trade.TrailStopPrice > 0) sqlCommand.Parameters.AddWithValue("@trail_stop_price", trade.TrailStopPrice);
                    sqlCommand.Parameters.AddWithValue("@status_id", 1);

                    int result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw new Exception("Data.DBUtils.SaveTradesFileToDB: Error " + ex.Message);
                }
            }

            sqlConnection.Close();

        }

        public static int SaveExecutionDetails(int reqId, Contract contract, Execution execution)
        {
            int result = -1;
            try
            {
                int contract_id = SaveContract(contract);
                int execution_id = SaveExecution(execution);

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveExecutionDetails", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@req_id", reqId);
                sqlCommand.Parameters.AddWithValue("@contract_id", contract_id);
                sqlCommand.Parameters.AddWithValue("@execution_id", execution_id);

                sqlConnection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "SaveExecutionDetails insert failed: " + ex;
                Console.Write(errorMessage);
                SaveErrorMessage(errorMessage, GlobalValues.ErrorMessageTypes.ApplicationError);
            }

            return result;
        }

        private static int SaveExecution(Execution execution)
        {
            int result = -1;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveExecution", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@order_id", execution.OrderId);
                sqlCommand.Parameters.AddWithValue("@client_id", execution.ClientId);
                sqlCommand.Parameters.AddWithValue("@exec_id", execution.ExecId);
                sqlCommand.Parameters.AddWithValue("@server_timestamp", execution.Time);
                sqlCommand.Parameters.AddWithValue("@account_number", execution.AcctNumber);
                sqlCommand.Parameters.AddWithValue("@exchange", execution.Exchange);
                sqlCommand.Parameters.AddWithValue("@side", execution.Side);
                sqlCommand.Parameters.AddWithValue("@quanitity", execution.Shares);
                sqlCommand.Parameters.AddWithValue("@price", execution.Price);
                sqlCommand.Parameters.AddWithValue("@perm_id", execution.PermId);
                sqlCommand.Parameters.AddWithValue("@liquidation", execution.Liquidation);
                sqlCommand.Parameters.AddWithValue("@cumulative_quantity", execution.CumQty);
                sqlCommand.Parameters.AddWithValue("@average_price", execution.AvgPrice);
                sqlCommand.Parameters.AddWithValue("@order_reference", execution.OrderRef);
                sqlCommand.Parameters.AddWithValue("@ev_rule", execution.EvRule);
                sqlCommand.Parameters.AddWithValue("@ev_multiplier", execution.EvMultiplier);
                sqlCommand.Parameters.AddWithValue("@model_code", execution.ModelCode);

                sqlConnection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "SaveExecution insert failed: " + ex;
                Console.Write(errorMessage);
                SaveErrorMessage(errorMessage, GlobalValues.ErrorMessageTypes.ApplicationError);
            }

            return result;
        }

        private static int SaveContract(Contract contract)
        {
            int result = -1;
            try
            {
                int underCompId = -1;
                if (contract.UnderComp != null) underCompId = SaveUnderComp(contract.UnderComp);
                int comboLegsId = -1;
                if (contract.ComboLegs != null &&
                    contract.ComboLegs.Count > 0) comboLegsId = SaveComboLegs(contract.ComboLegs);

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveContract", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@contract_id", contract.ConId);
                sqlCommand.Parameters.AddWithValue("@symbol", contract.Symbol);
                sqlCommand.Parameters.AddWithValue("@security_type", contract.SecType);
                sqlCommand.Parameters.AddWithValue("@last_trade_date_or_contract_month", contract.LastTradeDateOrContractMonth);
                sqlCommand.Parameters.AddWithValue("@strike", contract.Strike);
                sqlCommand.Parameters.AddWithValue("@right", contract.Right);
                sqlCommand.Parameters.AddWithValue("@multiplier", contract.Multiplier);
                sqlCommand.Parameters.AddWithValue("@exchange", contract.Exchange);
                sqlCommand.Parameters.AddWithValue("@currency", contract.Currency);
                sqlCommand.Parameters.AddWithValue("@local_symbol", contract.LocalSymbol);
                sqlCommand.Parameters.AddWithValue("@primary_exchange", contract.PrimaryExch);
                sqlCommand.Parameters.AddWithValue("@trading_class", contract.TradingClass);
                sqlCommand.Parameters.AddWithValue("@include_expired", contract.IncludeExpired);
                sqlCommand.Parameters.AddWithValue("@security_id_type", contract.SecIdType);
                sqlCommand.Parameters.AddWithValue("@security_id", contract.SecId);
                sqlCommand.Parameters.AddWithValue("@combo_legs_description", contract.ComboLegsDescription);
                sqlCommand.Parameters.AddWithValue("@under_comp_id", underCompId);

                sqlConnection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "SaveExecution insert failed: " + ex;
                Console.Write(errorMessage);
                SaveErrorMessage(errorMessage, GlobalValues.ErrorMessageTypes.ApplicationError);
            }

            return result;

        }

        private static int SaveComboLegs(List<ComboLeg> comboLegs)
        {
            int result = -1;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveComboLegs", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                foreach (ComboLeg comboLeg in comboLegs)
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.Parameters.AddWithValue("@contract_id", comboLeg.ConId);
                    sqlCommand.Parameters.AddWithValue("@ratio", comboLeg.Ratio);
                    sqlCommand.Parameters.AddWithValue("@action", comboLeg.Action);
                    sqlCommand.Parameters.AddWithValue("@exchange", comboLeg.Exchange);
                    sqlCommand.Parameters.AddWithValue("@open_close", comboLeg.OpenClose);
                    sqlCommand.Parameters.AddWithValue("@short_sale_slot", comboLeg.ShortSaleSlot);
                    sqlCommand.Parameters.AddWithValue("@designated_location", comboLeg.DesignatedLocation);
                    sqlCommand.Parameters.AddWithValue("@exempt_code", comboLeg.ExemptCode);

                    sqlConnection.Open();
                    result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "SaveComboLegs insert failed: " + ex;
                Console.Write(errorMessage);
                SaveErrorMessage(errorMessage, GlobalValues.ErrorMessageTypes.ApplicationError);
            }

            return result;
        }

        private static int SaveUnderComp(UnderComp underComp)
        {
            throw new NotImplementedException();
        }

        public static int SaveError(Exception e)
        {
            return SaveErrorMessage(e.Message, GlobalValues.ErrorMessageTypes.APIError);
        }
        public static int SaveError(string error)
        {
            return SaveErrorMessage(error, GlobalValues.ErrorMessageTypes.UnspecifiedIBError);
        }
        public static int SaveErrorMessage(string errorMessage, GlobalValues.ErrorMessageTypes errorMessageType)
        {
            int result = -1;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveErrorMessage", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@errormessage_type_id", errorMessageType);
                sqlCommand.Parameters.AddWithValue("@errormessage", errorMessage);

                sqlConnection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errMessage = "SaveErrorMessage insert failed: " + ex;
                Console.Write(errMessage);
            }

            return result;
        }
        public static int SaveError(int id, int errorCode, string errorMsg)
        {
            Console.Write(errorCode + " " + errorMsg);
            return SaveErrorMessage(errorCode + ": " + errorMsg, GlobalValues.ErrorMessageTypes.TWSError);
        }

        public static void SaveLog(string v)
        {
            throw new NotImplementedException();
        }

        public static void SaveAccountSummary(int reqId, string account, string tag, string value, string currency)
        {
            throw new NotImplementedException();
        }

        public static void SaveUpdateAccountValue(string key, string value, string currency, string accountName)
        {
            //throw new NotImplementedException();
        }

        public static void SaveUpdatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealisedPNL, double realisedPNL, string accountName)
        {
            //throw new NotImplementedException();
        }

        public static int SaveOrderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId,
                                             double lastFillPrice, int clientId, string whyHeld)
        {
            int result = -1;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveOrderStatus", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@order_id", orderId);
                sqlCommand.Parameters.AddWithValue("@status", status);
                sqlCommand.Parameters.AddWithValue("@filled", filled);
                sqlCommand.Parameters.AddWithValue("@remaining", remaining);
                sqlCommand.Parameters.AddWithValue("@average_fill_price", avgFillPrice);
                sqlCommand.Parameters.AddWithValue("@perm_id", permId);
                sqlCommand.Parameters.AddWithValue("@parent_id", parentId);
                sqlCommand.Parameters.AddWithValue("@last_filled_price", lastFillPrice);
                sqlCommand.Parameters.AddWithValue("@client_id", clientId);
                sqlCommand.Parameters.AddWithValue("@why_held", whyHeld);

                sqlConnection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "SaveOrderStatus insert failed: " + ex;
                Console.Write(errorMessage);
                SaveErrorMessage(errorMessage, GlobalValues.ErrorMessageTypes.ApplicationError);
            }

            return result;
        }

        public static int SaveOpenOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            int contractId = SaveContract(contract);
            int order_Id = SaveOrder(order);
            int orderStateId = SaveOrderState(orderState);

            int result = -1;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveOpenOrder", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@orderId", orderId);
                sqlCommand.Parameters.AddWithValue("@contract_id", contractId);
                sqlCommand.Parameters.AddWithValue("@order_id", order_Id);
                sqlCommand.Parameters.AddWithValue("@order_state_id", orderStateId);

                sqlConnection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "SaveOpenOrder insert failed: " + ex;
                Console.Write(errorMessage);
                SaveErrorMessage(errorMessage, GlobalValues.ErrorMessageTypes.ApplicationError);
            }

            return result;

        }

        private static int SaveOrderState(OrderState orderState)
        {
            int result = -1;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveOrderState", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@status", orderState.Status);
                sqlCommand.Parameters.AddWithValue("@initial_margin", orderState.InitMargin);
                sqlCommand.Parameters.AddWithValue("@maintenance_margin", orderState.MaintMargin);
                sqlCommand.Parameters.AddWithValue("@equity_with_loan", orderState.EquityWithLoan);
                sqlCommand.Parameters.AddWithValue("@commission", orderState.Commission);
                sqlCommand.Parameters.AddWithValue("@minumum_commission", orderState.MinCommission);
                sqlCommand.Parameters.AddWithValue("@maximum_commission", orderState.MaxCommission);
                sqlCommand.Parameters.AddWithValue("@commission_currency", orderState.CommissionCurrency);
                sqlCommand.Parameters.AddWithValue("@warning_text", orderState.WarningText);

                sqlConnection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "SaveOrderState insert failed: " + ex;
                Console.Write(errorMessage);
                SaveErrorMessage(errorMessage, GlobalValues.ErrorMessageTypes.ApplicationError);
            }

            return result;
        }

        public static void SaveContractDetails(int reqId, ContractDetails contractDetails)
        {
            throw new NotImplementedException();
        }

        public static int SaveCommissionReport(CommissionReport commissionReport)
        {
            int result = -1;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveCommissionReport", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@execution_id", commissionReport.ExecId);
                sqlCommand.Parameters.AddWithValue("@commission", commissionReport.Commission);
                sqlCommand.Parameters.AddWithValue("@currency", commissionReport.Currency);
                sqlCommand.Parameters.AddWithValue("@realised_pnl", commissionReport.RealizedPNL);
                sqlCommand.Parameters.AddWithValue("@yield", commissionReport.Yield);
                sqlCommand.Parameters.AddWithValue("@yield_redemption_date", commissionReport.YieldRedemptionDate);

                sqlConnection.Open();
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "SaveCommissionReport insert failed: " + ex;
                Console.Write(errorMessage);
                SaveErrorMessage(errorMessage, GlobalValues.ErrorMessageTypes.ApplicationError);
            }

            return result;
        }

        public static void SavePosition(string account, Contract contract, double pos, double avgCost)
        {
            //throw new NotImplementedException();
        }

        public static void SaveBondContractDetails(int requestId, ContractDetails contractDetails)
        {
            throw new NotImplementedException();
        }

        public static void SaveErrorMessage(ErrorMessage message)
        {
            //throw new NotImplementedException();
        }

        static public int CountTrades()
        {
            int result;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand("CountTrades", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();
            try
            {
                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public static bool TestDB()
        {
            bool result = false;

            SqlConnection cnn;
            cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
                Console.Write("Connection Open!\n");
                result = true;
                cnn.Close();
            }
            catch (Exception ex)
            {
                Console.Write("Can not open connection !" + ex.Message);
            }
            return result;
        }
    }
}
