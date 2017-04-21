using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi;
using System.Data.SqlClient;
using System.Data;
using One.Common;

namespace One.db
{
    static class DBUtils
    {
        static string connectionString = //"Data Source = DESKTOP-F67C4NI; Initial Catalog = AutoTrader; User ID = sa; Password=Makatini@1";
        "Server=DESKTOP-F67C4NI;Database=AutoTrader;User Id=sa;Password=Makatini@1";

        static public int SaveOrder(Order order)
        {
            int result = -1;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("SaveOrder", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@client_id", order.ClientId);
                sqlCommand.Parameters.AddWithValue("@order_id", order.OrderId);
                sqlCommand.Parameters.AddWithValue("@perm_id", order.PermId);

                sqlCommand.Parameters.AddWithValue("@action", order.Action);
                sqlCommand.Parameters.AddWithValue("@quantity", order.TotalQuantity);
                sqlCommand.Parameters.AddWithValue("@order_type", order.OrderType);
                sqlCommand.Parameters.AddWithValue("@limit_price", order.LmtPrice);
                sqlCommand.Parameters.AddWithValue("@aux_price", order.AuxPrice);
                sqlCommand.Parameters.AddWithValue("@time_in_force", order.Tif);
                sqlCommand.Parameters.AddWithValue("@active_start_time", order.ActiveStartTime);

                sqlCommand.Parameters.AddWithValue("@active_stop_time", order.ActiveStopTime);
                sqlCommand.Parameters.AddWithValue("@oca_group", order.OcaGroup);
                sqlCommand.Parameters.AddWithValue("@oca_type", order.OcaType);
                sqlCommand.Parameters.AddWithValue("@order_reference", order.OrderRef);
                sqlCommand.Parameters.AddWithValue("@transmit", order.Transmit);
                sqlCommand.Parameters.AddWithValue("@parent_id", order.ParentId);
                sqlCommand.Parameters.AddWithValue("@block_order", order.BlockOrder);
                sqlCommand.Parameters.AddWithValue("@sweep_to_fill", order.SweepToFill);
                sqlCommand.Parameters.AddWithValue("@display_size", order.DisplaySize);
                sqlCommand.Parameters.AddWithValue("@trigger_method", order.TriggerMethod);
                sqlCommand.Parameters.AddWithValue("@outside_rth", order.OutsideRth);
                sqlCommand.Parameters.AddWithValue("@hidden", order.Hidden);
                sqlCommand.Parameters.AddWithValue("@good_after_time", order.GoodAfterTime);
                sqlCommand.Parameters.AddWithValue("good_till_date", order.GoodTillDate);
                sqlCommand.Parameters.AddWithValue("@override_percentage_constraints", order.OverridePercentageConstraints);

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

        static public void SaveTradesFile(List<string> file)
        {

        }

        internal static int SaveExecutionDetails(int reqId, Contract contract, Execution execution)
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

                foreach (ComboLeg comboLeg in comboLegs) {
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

        internal static int SaveError(Exception e)
        {
            return SaveErrorMessage(e.Message, GlobalValues.ErrorMessageTypes.APIError);
        }
        internal static int SaveError(string error)
        {
            return SaveErrorMessage(error, GlobalValues.ErrorMessageTypes.UnspecifiedIBError);
        }
        private static int SaveErrorMessage(string errorMessage, GlobalValues.ErrorMessageTypes errorMessageType)
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
        internal static int SaveError(int id, int errorCode, string errorMsg)
        {
            Console.Write(errorCode + " " + errorMsg);
            return SaveErrorMessage(errorCode + ": " + errorMsg, GlobalValues.ErrorMessageTypes.TWSError);
        }

        internal static void SaveLog(string v)
        {
            throw new NotImplementedException();
        }

        internal static void SaveAccountSummary(int reqId, string account, string tag, string value, string currency)
        {
            throw new NotImplementedException();
        }

        internal static void SaveUpdateAccountValue(string key, string value, string currency, string accountName)
        {
            //throw new NotImplementedException();
        }

        internal static void SaveUpdatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealisedPNL, double realisedPNL, string accountName)
        {
            //throw new NotImplementedException();
        }

        internal static int SaveOrderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, 
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

        internal static int SaveOpenOrder(int orderId, Contract contract, Order order, OrderState orderState)
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

        internal static void SaveContractDetails(int reqId, ContractDetails contractDetails)
        {
            throw new NotImplementedException();
        }

        internal static int SaveCommissionReport(CommissionReport commissionReport)
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

        internal static void SavePosition(string account, Contract contract, double pos, double avgCost)
        {
            //throw new NotImplementedException();
        }

        internal static void SaveBondContractDetails(int requestId, ContractDetails contractDetails)
        {
            throw new NotImplementedException();
        }

        internal static void SaveErrorMessage(ErrorMessage message)
        {
            //throw new NotImplementedException();
        }

        internal static bool TestDB()
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
