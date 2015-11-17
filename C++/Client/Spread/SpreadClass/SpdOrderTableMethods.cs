
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrderBook.CommonData;
using Structure;
using System.Net;
using AtsCommon;
using AtsApi.Common;

namespace Client.Spread
{
    public class SpdOrderTableMethods
    {
        public static Dictionary<int, Dictionary<BUYSELL, float>> _PriceDiff = new Dictionary<int, Dictionary<BUYSELL, float>>(100);

        private static readonly object LockTableOperation = new object();
        public static void CreateOrderTable()
        {
            if (CommonData.dtSpdOrderBook == null)
            {
                CommonData.dtSpdOrderBook = new DataTable("SpdOrderTable");
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Symbol, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Symbol1, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.ExpiryDate, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.ExpiryDate1, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.OrderId, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.BranchId, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.BrokerId, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.CloseoutFlag, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.DisclosedVolumeRemaining, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.EntryDateTime, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.GoodTillDate, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.LastModified, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.LogTime, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Modified_CancelledBy, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Open_Close, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Pro_ClientIndicator, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Settlor, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.TotalVolumeRemaining, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.TraderId, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.TransactionCode, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Unique_id, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.UserId, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.VolumeFilledToday, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.filler, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.OptionType1, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.OptionType2, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.CALevel1, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.CALevel2, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.StrikePrice1, typeof(decimal));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.StrikePrice2, typeof(decimal));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.UnixExpiry1, typeof(Int32));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.UnixExpiry2, typeof(Int32));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.BoardLotQuantity1, typeof(Int32));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.BoardLotQuantity2, typeof(Int32));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Token1, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Token2, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.BidQ, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.AskQ, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.InstrumentName2, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.InstrumentName1, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Status, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.BuySell1, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.BuySell2, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Buy_SellIndicator, typeof(string));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Price1, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Price2, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.BidQ_leg2, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.AskQ_leg2, typeof(int));
                CommonData.dtSpdOrderBook.Columns.Add(SpreadContract.Price_Diff, typeof(float));



            }
        }
        public struct OrderDetails
        {

        }

        public static void SP_ORDER_CONFIRMATION(byte[] buffer)//2124
        {
            if (CommonData.frmSpdOrderBook != null && CommonData.frmSpdOrderBook.InvokeRequired)
            {
                CommonData.frmSpdOrderBook.BeginInvoke((MethodInvoker)(() => SP_ORDER_CONFIRMATION(buffer)));
            }
            else
            {
                if (CommonData.dtSpdOrderBook == null)
                    return;

                try
                {
                    MS_SPD_OE_REQUEST spdObj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
                    Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(spdObj.OrderNumber1), new Order((int)_Type.MS_SPD_OE_REQUEST));
                    Holder.holderOrder[LogicClass.DoubleEndianChange(spdObj.OrderNumber1)].mS_SPD_OE_REQUEST = spdObj;

                    DataRow dr = CommonData.dtSpdOrderBook.NewRow();

                    dr[SpreadContract.OrderId] = (long)LogicClass.DoubleEndianChange((spdObj.OrderNumber1));
                    dr[SpreadContract.Token1] = IPAddress.HostToNetworkOrder(spdObj.Token1);
                    dr[SpreadContract.Token2] = IPAddress.HostToNetworkOrder(spdObj.leg2.token);
                    // dr[SpreadContract.BuySell1] = IPAddress.HostToNetworkOrder(spdObj.BuySell1);
                    //   dr[SpreadContract.BuySell2] = IPAddress.HostToNetworkOrder(spdObj.leg2.BuySell2);
                    // dr[SpreadContract.AccountNumber] = IPAddress.HostToNetworkOrder(spdObj.AccountNumber1);
                    dr[SpreadContract.Price1] = IPAddress.HostToNetworkOrder(spdObj.Price1);
                    dr[SpreadContract.Price2] = IPAddress.HostToNetworkOrder(spdObj.leg2.Price2);

                    dr[SpreadContract.BidQ] = IPAddress.HostToNetworkOrder(spdObj.BuySell1) == (short)BUYSELL.BUY ? IPAddress.HostToNetworkOrder(spdObj.Volume1) : 0;
                    dr[SpreadContract.AskQ] = IPAddress.HostToNetworkOrder(spdObj.BuySell1) == (short)BUYSELL.SELL ? IPAddress.HostToNetworkOrder(spdObj.Volume1) : 0;

                    dr[SpreadContract.BidQ_leg2] = IPAddress.HostToNetworkOrder(spdObj.leg2.BuySell2) == (short)BUYSELL.BUY ? IPAddress.HostToNetworkOrder(spdObj.leg2.Volume2) : 0;
                    dr[SpreadContract.AskQ_leg2] = IPAddress.HostToNetworkOrder(spdObj.leg2.BuySell2) == (short)BUYSELL.SELL ? IPAddress.HostToNetworkOrder(spdObj.leg2.Volume2) : 0;

                    dr[SpreadContract.Status] = AtsEnums.OrderStatus.EPending;
                    dr[SpreadContract.TransactionCode] = 2124;
                    dr[SpreadContract.BuySell2] = (BUYSELL)IPAddress.HostToNetworkOrder(spdObj.leg2.BuySell2);//== (short)BUYSELL.BUY ? BUYSELL.BUY : BUYSELL.SELL;
                    dr[SpreadContract.BuySell1] = (BUYSELL)IPAddress.HostToNetworkOrder(spdObj.BuySell1);//== (short)BUYSELL.BUY ? BUYSELL.SELL : BUYSELL.BUY ;
                    dr[SpreadContract.Symbol1] = Encoding.ASCII.GetString(spdObj.leg2.ms_oe_obj.Symbol);
                    dr[SpreadContract.Symbol] = Encoding.ASCII.GetString(spdObj.ms_oe_obj.Symbol);
                    dr[SpreadContract.ExpiryDate] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(spdObj.ms_oe_obj.ExpiryDate));
                    dr[SpreadContract.ExpiryDate1] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(spdObj.leg2.ms_oe_obj.ExpiryDate));
                    dr[SpreadContract.Price_Diff] = _PriceDiff[IPAddress.HostToNetworkOrder(spdObj.Token1)][(BUYSELL)IPAddress.HostToNetworkOrder(spdObj.BuySell1)];

                    CommonData.dtSpdOrderBook.Rows.Add(dr);

                  



                }
                catch (Exception ex)
                {
                  //  MessageBox.Show("Order Book -  Funtion Name-  SP_ORDER_CONFIRMATION  " + ex.Message);
                }
            }

        }



        public static void SP_ORDER_TRD_CONFIRMATION(byte[] buffer)//2125
        {
            if (CommonData.frmSpdOrderBook != null && CommonData.frmSpdOrderBook.InvokeRequired)
            {
                CommonData.frmSpdOrderBook.BeginInvoke((MethodInvoker)(() => SP_ORDER_TRD_CONFIRMATION(buffer)));
            }
            else
            {
                if (CommonData.dtSpdOrderBook == null)
                    return;

                try
                {
                    MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
                    DataRow[] _drResult = CommonData.dtSpdOrderBook.Select("OrderId=" + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1));

                    if (_drResult.Length > 0)
                    {
                        _drResult[0][SpreadContract.Status] = AtsApi.Common.AtsEnums.OrderStatus.Executed;
                        _drResult[0][SpreadContract.TransactionCode] = 2125;
                    }
                    else
                    {
                        //MessageBox.Show("Traded Order Id not found in Spread OrderBook. Please confirm trade from RMS");
                    }



                }
                catch (Exception Ex)
                {
                  //  MessageBox.Show("Some error occured updating Trade confirmaion of spread order " + Environment.NewLine + Ex.StackTrace.ToString());
                }
            }
        }
        public static void SP_ORDER_CANCEL_CONFIRMATION(byte[] buffer)//2130
        {
            if (CommonData.frmSpdOrderBook != null && CommonData.frmSpdOrderBook.InvokeRequired)
            {
                CommonData.frmSpdOrderBook.BeginInvoke((MethodInvoker)(() => SP_ORDER_CANCEL_CONFIRMATION(buffer)));
            }
            else
            {
                if (CommonData.dtSpdOrderBook == null)
                    return;

                try
                {
                    MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
                    var ob = new Order((int)_Type.MS_SPD_OE_REQUEST);
                    Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber1), out ob);
                    DataRow[] _drResult = CommonData.dtSpdOrderBook.Select("OrderId=" + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1));

                    if (_drResult.Length > 0)
                    {
                        _drResult[0][SpreadContract.Status] = AtsApi.Common.AtsEnums.OrderStatus.ECancelled;
                        _drResult[0][SpreadContract.TransactionCode] = 2130;
                    }
                    else
                    {
                       // MessageBox.Show("Cancelled Order Id not found in Spread OrderBook. Please confirm trade from RMS");
                    }



                }
                catch (Exception Ex)
                {
                  //  MessageBox.Show("Some error occured updating Cancel confirmaion of spread order " + Environment.NewLine + Ex.StackTrace.ToString());
                }
            }
        }

        public static void SP_ORDER_Mod_CONFIRMATION(byte[] buffer)//2136
        {
            if (CommonData.frmSpdOrderBook != null && CommonData.frmSpdOrderBook.InvokeRequired)
            {
                CommonData.frmSpdOrderBook.BeginInvoke((MethodInvoker)(() => SP_ORDER_Mod_CONFIRMATION(buffer)));
            }
            else
            {
                if (CommonData.dtSpdOrderBook == null)
                    return;

                try
                {
                    MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
                    
                    Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber1)].mS_SPD_OE_REQUEST = obj;
                    DataRow[] _drResult = CommonData.dtSpdOrderBook.Select("OrderId=" + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1));

                    if (_drResult.Length > 0)
                    {

                        _drResult[0][SpreadContract.Status] = AtsApi.Common.AtsEnums.OrderStatus.MPending;
                        _drResult[0][SpreadContract.TransactionCode] = 2136;
                        _drResult[0][SpreadContract.Price_Diff] = IPAddress.HostToNetworkOrder(obj.PriceDiff)/100;
                    }
                    else
                    {
                        //MessageBox.Show("Cancelled Order Id not found in Spread OrderBook. Please confirm trade from RMS");
                    }



                }
                catch (Exception Ex)
                {
                   // MessageBox.Show("Some error occured updating Cancel confirmaion of spread order " + Environment.NewLine + Ex.StackTrace.ToString());
                }
            }
        }
        public static void TRADE_CONFIRMATION_TR(byte[] buffer) //-- 20222
        {
            try
            {
                object ob1 = new object();
                lock (ob1)
                {
                    var obj = (MS_TRADE_CONFIRM_TR)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_CONFIRM_TR));
                    int ch = 0;
                    if (Holder.holderOrder.ContainsKey(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber)))
                        ch = Holder.holderOrder[LogicClass.DoubleEndianChange(obj.ResponseOrderNumber)].GetType();
                    switch (ch)
                    {
                        case 1:
                            {
                                var ob = new Order((int)_Type.MS_OE_REQUEST);
                                break;
                            }
                        case 2:
                            {
                                if (CommonData.frmSpdOrderBook != null && CommonData.frmSpdOrderBook.InvokeRequired)
                                {
                                    CommonData.frmSpdOrderBook.BeginInvoke((MethodInvoker)(() => TRADE_CONFIRMATION_TR(buffer)));
                                }
                                else
                                {
                                    if (CommonData.dtSpdOrderBook == null)
                                        return;

                                    try
                                    {
                                       


                                    }
                                    catch (Exception Ex)
                                    {
                                      //  MessageBox.Show("Some error occured updating Trade confirmaion of spread order " + Environment.NewLine + Ex.StackTrace.ToString());
                                    }
                                }

                                break;
                            }
                        case 3:
                            {


                                if (CommonData.frmSpdOrderBook != null && CommonData.frmSpdOrderBook.InvokeRequired)
                                {
                                    CommonData.frmSpdOrderBook.BeginInvoke((MethodInvoker)(() => TRADE_CONFIRMATION_TR(buffer)));
                                }
                                else
                                {
                                    if (CommonData.dtSpdOrderBook == null)
                                        return;

                                    try

                                    {
                                        var ob = new Order((int)_Type.MS_SPD_OE_REQUEST);
                                       
                                       // Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber), out ob);
                                        
                                        DataRow[] _drResult = CommonData.dtSpdOrderBook.Select("OrderId=" + (long)LogicClass.DoubleEndianChange(obj.ResponseOrderNumber));
                                        if (_drResult.Length > 0)
                                        {
                                            _drResult[0][SpreadContract.Status] = AtsApi.Common.AtsEnums.OrderStatus.ESTrade;
                                            _drResult[0][SpreadContract.TransactionCode] = 20222;
                                            _drResult[0][SpreadContract.Volume] =IPAddress.HostToNetworkOrder(obj.VolumeFilledToday);
                                        }
                                        else
                                        {
                                           // MessageBox.Show("Traded Order Id not found in Spread OrderBook. Please confirm trade from RMS");
                                        }



                                    }
                                    catch (Exception Ex)
                                    {
                                       // MessageBox.Show("Some error occured updating Trade confirmaion of spread order " + Environment.NewLine + Ex.StackTrace.ToString());
                                    }
                                }


                                break;
                            }









                    }
                }


            }
            catch (Exception ex)
            {

            }
        }

        public static void InsertOrder(OrderDetails order)
        {
            if (CommonData.frmSpdOrderBook != null && CommonData.frmSpdOrderBook.InvokeRequired)
            {
                CommonData.frmSpdOrderBook.BeginInvoke((MethodInvoker)(() => InsertOrder(order)));
            }
            else
            {
                if (CommonData.dtSpdOrderBook == null)
                    return;
                try
                {
                    DataRow[] drExist;
                    lock (LockTableOperation) ;

                    //drExist = CommonData.dtSpdOrderBook.Select(SpreadContract.OrderId + " = '" + order.OrderID + "' And "
                    //            + WatchConst.ClOrderNo + " = '" + order.OrderMessage.ClOrdID + "' ");
                    //if (drExist == null && drExist.Length > 0) return;

                    /*  DataRow drOrder;

                      lock (LockTableOperation)
                      drOrder = CommonData.dtSpdOrderBook.NewRow();
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
                          CommonData.dtSpdOrderBook.Rows.Add(drOrder);*/

                }
                catch (Exception ex)
                {
                    //AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
                }
            }
        }

        public static void UpdateOrder(OrderDetails order)
        {
            if (CommonData.frmSpdOrderBook != null && CommonData.frmSpdOrderBook.InvokeRequired)
            {
                CommonData.frmSpdOrderBook.BeginInvoke((MethodInvoker)(() => UpdateOrder(order)));
            }
            else
            {/*
                lock (LockTableOperation)
                {
                    try
                    {
                        if (CommonData.dtSpdOrderBook == null) return;
                        DataRow[] drOrder = CommonData.dtSpdOrderBook.Select(WatchConst.ExchOrderNo + " = '" + order.OrderMessage.OrderID + "' And "
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
