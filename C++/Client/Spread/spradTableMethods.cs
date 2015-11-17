using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Structure;
using System.Net;
using LogWriter;
using System.Drawing;
using System.Collections.Concurrent;

namespace Client.Spread
{
    class spradTableMethods
    {
        private static readonly object LockTableOperation = new object();

        delegate void OnLZOArrivedDelegate(Object o, ReadOnlyEventArgs<MS_SPD_MKT_INFO_7211> Stat);
         
            private static DataGridViewCellStyle _makeItBlack;
            private static DataGridViewCellStyle _makeItBlue;
            private static DataGridViewCellStyle _makeItRed;
           
        public static void CreateOrderTable()
        {
            if (CommonData.dtSpreadMktWatch == null)
            {
                _makeItRed = new DataGridViewCellStyle();
                _makeItBlue = new DataGridViewCellStyle();
                _makeItBlack = new DataGridViewCellStyle();

                _makeItRed.BackColor = Color.LightPink;

                _makeItBlue.BackColor = Color.DeepSkyBlue;
              // _makeItBlack.BackColor = Color.Yellow;
                CommonData.dtSpreadMktWatch = new DataTable("spdMktWatch");
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.Symbol1, typeof(string));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.ExpiryDate1, typeof(string));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.ExpiryDate2, typeof(string));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.Bid, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.BidQ, typeof(int));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.Ask, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.AskQ, typeof(int));

                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.LTP, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.TradedVolume, typeof(decimal));

                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.TotalBuyQty, typeof(int));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.TotalSellQty, typeof(int));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.OpenPrice, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.HighPrice, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.LowPrice, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.ClosePrice, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.ATP, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.TotalTradedValue, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.PercentChange, typeof(decimal));

                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.LastActiveTime, typeof(DateTime));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.LastUpdateTime, typeof(DateTime));

                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.InstrumentName2, typeof(string));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.InstrumentName1, typeof(string));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.Token1, typeof(int));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.Token2, typeof(int));

                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.OptionType1, typeof(string));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.OptionType2, typeof(string));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.CALevel1, typeof(string));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.CALevel2, typeof(string));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.StrikePrice1, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.StrikePrice2, typeof(decimal));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.Symbol2, typeof(string));

                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.UnixExpiry1, typeof(Int32));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.UnixExpiry2, typeof(Int32));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.BoardLotQuantity1, typeof(Int32));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.BoardLotQuantity2, typeof(Int32));
                CommonData.dtSpreadMktWatch.Columns.Add(SpreadContract.Price_Diff, typeof(Int32));

               

            }
        }
        public static void InsertRecord(int a)
        {
            if (AppGlobal.frmSpreadWatch != null && AppGlobal.frmSpreadWatch.InvokeRequired)
            {
                AppGlobal.frmSpreadWatch.BeginInvoke((MethodInvoker)(() => InsertRecord(a)));
            }
            else
            {
                if (CommonData.dtSpreadMktWatch == null)
                    return;
                try
                {
                    DataRow drRec;
                    lock (LockTableOperation)
                        drRec = CommonData.dtSpreadMktWatch.NewRow();
                    DataRow[] spdContractResult = CommonData.dtSpreadContract.Select("Token1=" + a + " and Token2=" + a);
                    if (spdContractResult.Length > 0)
                    {
                        drRec[SpreadContract.Symbol1] = spdContractResult[0][SpreadContract.Symbol1];
                        drRec[SpreadContract.Symbol2] = spdContractResult[0][SpreadContract.Symbol2];
                        drRec[SpreadContract.StrikePrice1] = spdContractResult[0][SpreadContract.StrikePrice1];
                        drRec[SpreadContract.StrikePrice2] = spdContractResult[0][SpreadContract.StrikePrice2];
                        drRec[SpreadContract.OptionType1] = spdContractResult[0][SpreadContract.OptionType1];
                        drRec[SpreadContract.OptionType2] = spdContractResult[0][SpreadContract.OptionType2];
                        drRec[SpreadContract.InstrumentName1] = spdContractResult[0][SpreadContract.InstrumentName1];
                        drRec[SpreadContract.InstrumentName2] = spdContractResult[0][SpreadContract.InstrumentName2];
                        drRec[SpreadContract.ExpiryDate1] = spdContractResult[0][SpreadContract.ExpiryDate1];
                        drRec[SpreadContract.ExpiryDate2] = spdContractResult[0][SpreadContract.ExpiryDate2];
                        drRec[SpreadContract.CALevel2] = spdContractResult[0][SpreadContract.CALevel2];
                        drRec[SpreadContract.CALevel1] = spdContractResult[0][SpreadContract.CALevel1];
                    }

                    drRec[SpreadContract.Token1] = 1111;



                    lock (LockTableOperation)
                        CommonData.dtSpreadMktWatch.Rows.Add(drRec);

                }
                catch (Exception e)
                {

                }
            }

        }
        private static void SetData(DataGridViewCell DGCell, double ValueOne)
        {
            if (DGCell != null)
            {
                double ValueTwo = Convert.ToDouble(DGCell.Value==DBNull.Value?0:DGCell.Value);
                if (ValueOne > ValueTwo)
                {
                    DGCell.Style = _makeItBlue;
                }
                else if (ValueOne < ValueTwo)
                {
                    DGCell.Style = _makeItRed;
                }
                //else if (ValueOne == ValueTwo)
                //{
                //    DGCell.Style = _makeItBlack;
                //}
            }

            DGCell.Value = ValueOne;
        }
        public static void UpdateRecord(object sender, ReadOnlyEventArgs<MS_SPD_MKT_INFO_7211> Stat)
        {
            if (AppGlobal.frmSpreadWatch != null && AppGlobal.frmSpreadWatch.InvokeRequired)
            {
                AppGlobal.frmSpreadWatch.BeginInvoke(new OnLZOArrivedDelegate(UpdateRecord), sender, new ReadOnlyEventArgs<MS_SPD_MKT_INFO_7211>(Stat.Parameter));
            }
            else
            {
                if (CommonData.dtSpreadMktWatch == null)
                    return;
                lock (LockTableOperation)
                {
                    string Token = IPAddress.NetworkToHostOrder(Stat.Parameter.Token1) + "-" + IPAddress.NetworkToHostOrder(Stat.Parameter.Token2);
                    try
                    {
                        DataRow[] dr = CommonData.dtSpreadMktWatch.Select("Token1=" + IPAddress.NetworkToHostOrder(Stat.Parameter.Token1) + " and Token2=" + IPAddress.NetworkToHostOrder(Stat.Parameter.Token2));

                        if (Global.Instance._SprdwatchDict.ContainsKey(IPAddress.NetworkToHostOrder(Stat.Parameter.Token1).ToString() + IPAddress.NetworkToHostOrder(Stat.Parameter.Token2).ToString()))
                        {
                            SetData(Global.Instance._SprdwatchDict[(IPAddress.NetworkToHostOrder(Stat.Parameter.Token1)).ToString() + (IPAddress.NetworkToHostOrder(Stat.Parameter.Token2)).ToString()].Cells[SpreadContract.LTP], (IPAddress.NetworkToHostOrder(Stat.Parameter.LastTradedPriceDifference)) / 100);
                            SetData(Global.Instance._SprdwatchDict[(IPAddress.NetworkToHostOrder(Stat.Parameter.Token1)).ToString() + (IPAddress.NetworkToHostOrder(Stat.Parameter.Token2)).ToString()].Cells[SpreadContract.Bid],(IPAddress.NetworkToHostOrder(Stat.Parameter.mbpBuys[0].Price)) / 100);
                            SetData(Global.Instance._SprdwatchDict[(IPAddress.NetworkToHostOrder(Stat.Parameter.Token1)).ToString() + (IPAddress.NetworkToHostOrder(Stat.Parameter.Token2)).ToString()].Cells[SpreadContract.Ask], (IPAddress.NetworkToHostOrder(Stat.Parameter.mbpSells[0].Price)) / 100);
                           
                        } 
                        if (dr.Length > 0)
                        {
                            dr[0][SpreadContract.Bid] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.mbpBuys[0].Price)) / 100;
                            dr[0][SpreadContract.Ask] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.mbpSells[0].Price)) / 100;
                            dr[0][SpreadContract.BidQ] = IPAddress.NetworkToHostOrder(Stat.Parameter.mbpBuys[0].Volume);
                            dr[0][SpreadContract.AskQ] = IPAddress.NetworkToHostOrder(Stat.Parameter.mbpSells[0].Volume);
                            dr[0][SpreadContract.LTP] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.LastTradedPriceDifference)) / 100;
                            dr[0][SpreadContract.TotalSellQty] = IPAddress.NetworkToHostOrder(Stat.Parameter.MbpSell);
                            dr[0][SpreadContract.TotalSellQty] = IPAddress.NetworkToHostOrder(Stat.Parameter.MbpBuy);
                            dr[0][SpreadContract.OpenPrice] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.OpenPriceDifference)) / 100;
                            dr[0][SpreadContract.HighPrice] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.DayHighPriceDifference)) / 100;
                            dr[0][SpreadContract.LowPrice] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.DayLowPriceDifference)) / 100;
                            dr[0][SpreadContract.TotalTradedValue] = Stat.Parameter.TotalTradedValue;
                            dr[0][SpreadContract.TradedVolume] = IPAddress.NetworkToHostOrder(Stat.Parameter.TradedVolume);
                            dr[0][SpreadContract.LastUpdateTime] = CommonData.UnixTimeStampToDateTime(IPAddress.NetworkToHostOrder(Stat.Parameter.LastUpdateTime));
                            dr[0][SpreadContract.LastActiveTime] = CommonData.UnixTimeStampToDateTime(IPAddress.NetworkToHostOrder(Stat.Parameter.LastActiveTime));

                        }
                       
                       
                    }
                    catch (Exception e)
                    {
                        AppGlobal.Logger.WriteinFileWindowAndBox(e, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
                    }
                }
            }
        }
    }
}
