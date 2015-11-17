
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrderBook.CommonData;

namespace OrderBook.AppClasses
{
    public class OrderTableMethods
    {
        private static readonly object LockTableOperation = new object();

        public static void CreateOrderTable()
        {
            if(CommonData.CommonData.dtOrderBook ==null)
            {
                CommonData.CommonData.dtOrderBook =new DataTable("OrderTable");
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.FullName, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.InstrumentName, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.Symbol, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.TokenNo, typeof(int));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.ExpiryDate, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.OptionType, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.StrikePrice, typeof(int));
                
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.Buy_SellIndicator, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.Status, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.Price, typeof(decimal));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.Volume, typeof(int));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.FillNumber, typeof(int));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.FillPrice, typeof(decimal));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.nnffield, typeof(long));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.ReasonCode, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.RejectReason, typeof(string));

                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.AccountNumber, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.BookType, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.BranchId, typeof(int));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.BrokerId, typeof(int));
                
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.CloseoutFlag, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.DisclosedVolumeRemaining, typeof(int));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.EntryDateTime, typeof(string));
                

                
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.GoodTillDate, typeof(string));
                
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.LastModified, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.LogTime, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.Modified_CancelledBy, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.Open_Close, typeof(string));

                
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.Pro_ClientIndicator, typeof(string));

                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.Settlor, typeof(string));


                
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.TimeStamp1, typeof(string));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.TimeStamp2, typeof(string));

                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.TotalVolumeRemaining, typeof(int));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.TraderId, typeof(int));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.TransactionCode, typeof(int));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.Unique_id, typeof(int));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.UserId, typeof(string));
                
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.VolumeFilledToday, typeof(int));
                CommonData.CommonData.dtOrderBook.Columns.Add(WatchConst.filler, typeof(int));

            }
        }


       public struct OrderDetails
        { }

        public static void InsertOrder(OrderDetails order)
        {
            if (CommonData.CommonData.frmOrderBook != null && CommonData.CommonData.frmOrderBook.InvokeRequired)
            {
                CommonData.CommonData.frmOrderBook.BeginInvoke((MethodInvoker)(() => InsertOrder(order)));
            }
            else
            {
                if (CommonData.CommonData.dtOrderBook == null)
                    return;
                try
                {
                    //DataRow[] drExist;
                    //lock (LockTableOperation)
                    //    drExist = CommonData.dtOrderBook.Select(WatchConst.ExchOrderNo + " = '" + order.OrderMessage.OrderID + "' And "
                    //                  + WatchConst.ClOrderNo + " = '" + order.OrderMessage.ClOrdID + "' ");
                    //if (drExist == null && drExist.Length > 0) return;

                  /*  DataRow drOrder;

                    lock (LockTableOperation)
                    drOrder = CommonData.dtOrderBook.NewRow();
                    DataRow[] result = CommonData.dtMcxContractFile.Select("InstrumentIdentifier="+ order.OrderMessage.Instrument);
                    
                    drOrder[WatchConst.Exchange] = Enum.GetName(typeof(Enums.ExchangeType), order.OrderMessage.ExchangeType);
                    drOrder[WatchConst.ClOrderNo] = order.OrderMessage.ClOrdID;
                    drOrder[WatchConst.OriglClOrderNo] = order.OrderMessage.OrigClOrdID;
                    drOrder[WatchConst.ExchOrderNo] = order.OrderMessage.OrderID;
                    drOrder[WatchConst.OrderStatus] = Enum.GetName(typeof(Enums.OrdStatus), order.OrderMessage.OrderStatus);
                    drOrder[WatchConst.BuySell] = Enum.GetName(typeof(Enums.BuySell), order.OrderMessage.Side);
                    drOrder[WatchConst.OrderPrice] = order.OrderMessage.Price;
                    drOrder[WatchConst.TradePrice] = order.OrderMessage.LastPx;
                    drOrder[WatchConst.Qty] = order.OrderMessage.OrderQty;
                    drOrder[WatchConst.QtyMinFill] = order.OrderMessage.LastShare;
                    drOrder[WatchConst.QtyRemaining] = order.OrderMessage.LeavesQty;
                    drOrder[WatchConst.QtyTraded] = order.OrderMessage.LastShare;
                    drOrder[WatchConst.QtyTradedTotal] = order.OrderMessage.CumQty;
                    drOrder[WatchConst.QtyDisclosed] = order.OrderMessage.DisclosedQty;
                    drOrder[WatchConst.ExchRemarks] = order.OrderMessage.Text;
                    drOrder[WatchConst.StrategyId] = order.OrderMessage.StrategyId;
                    drOrder[WatchConst.StsNo] = order.OrderMessage.StrategySeqNo;

                    if (result.Length > 0 && order.OrderMessage.ExchangeType==1)
                    {
                        drOrder[WatchConst.Symbol] = result[0][ContractFields.InstrumentCode];//order.OrderMessage.Instrument;
                        drOrder[WatchConst.InstrumentName] = result[0][ContractFields.InstrumentName];
                        drOrder[WatchConst.Multiplier] = result[0][ContractFields.LotSize];
                        drOrder[WatchConst.PriceDivisor] = result[0][ContractFields.DecimalLocator];
                        drOrder[WatchConst.MemberId] = order.OrderMessage.BrokerId;
                        drOrder[WatchConst.ClientCode] = order.OrderMessage.ClientId;
                        drOrder[WatchConst.CtclId] = order.OrderMessage.TerminalInfo;
                        
                    } 
                    else
                    {
                        drOrder[WatchConst.Symbol] = order.OrderMessage.Instrument;
                    }

                    lock (LockTableOperation)
                        CommonData.dtOrderBook.Rows.Add(drOrder);*/

                }catch(Exception ex)
                {
                    //AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
                }
            }
        }

        public static void UpdateOrder(OrderDetails order)
        {
            if (CommonData.CommonData.frmOrderBook != null && CommonData.CommonData.frmOrderBook.InvokeRequired)
            {
                CommonData.CommonData.frmOrderBook.BeginInvoke((MethodInvoker)(() => UpdateOrder(order)));
            }
            else
            {/*
                lock (LockTableOperation)
                {
                    try
                    {
                        if (CommonData.dtOrderBook == null) return;
                        DataRow[] drOrder = CommonData.dtOrderBook.Select(WatchConst.ExchOrderNo + " = '" + order.OrderMessage.OrderID + "' And "
                                                                     + WatchConst.ClOrderNo + " = '" + order.OrderMessage.ClOrdID + "' ");

                        if (drOrder.Length > 0)
                        {
                            #region New Order Entery
                            DataRow data = drOrder[0];

                            data[WatchConst.CtclId] = "";

                            data[WatchConst.Exchange] = Enum.GetName(typeof(Enums.ExchangeType), order.OrderMessage.ExchangeType);
                            data[WatchConst.BuySell] = Enum.GetName(typeof(Enums.BuySell), order.OrderMessage.Side);
                            data[WatchConst.ClOrderNo] = order.OrderMessage.ClOrdID;
                            data[WatchConst.TokenNo] = order.OrderMessage.Instrument;
                            data[WatchConst.Symbol] = order.OrderMessage.Instrument;

                            data[WatchConst.OrderStatus] = Enum.GetName(typeof(Enums.OrdStatus), order.OrderMessage.OrderStatus);
                            data[WatchConst.OriglClOrderNo] = order.OrderMessage.OrigClOrdID;

                            data[WatchConst.Qty] = order.OrderMessage.OrderQty;
                            data[WatchConst.QtyDisclosed] = order.OrderMessage.DisclosedQty;

                            data[WatchConst.QtyMinFill] = order.OrderMessage.LastShare;
                            data[WatchConst.QtyRemaining] = order.OrderMessage.LeavesQty;
                            data[WatchConst.QtyTraded] = order.OrderMessage.LastShare;
                            data[WatchConst.QtyTradedTotal] = order.OrderMessage.CumQty;
                            data[WatchConst.StrategyId] = order.OrderMessage.StrategyId;
                            data[WatchConst.StsNo] = order.OrderMessage.StrategySeqNo;
                            data[WatchConst.UserRemarks] = order.OrderMessage.Text;

                            #endregion

                        }
                    }
                    catch (Exception ex)
                    {
                        //AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
                    }
                }*/
            }

        }
    }
}
