using AtsApi.Common;
using AtsCommon;
using csv;
using Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace Client
{
    public partial class frmMarketDepth : Form
    {

        public frmMarketDepth()
        {
            InitializeComponent();

            Load += frmBestFive_Load;
            FormClosed += frmBestFive_FormClosed;
            KeyDown += frmBestFive_KeyDown;
        }

        private void frmBestFive_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                    Close();


            }
            catch (Exception ex)
            {
                // AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);

            }
        }

        void frmBestFive_Load(object sender, EventArgs e)
        {
            try
            {
                cmbInstrument.Items.Clear();
                string[] dis = CSV_Class.cimlist.Where(ab => ab.InstrumentName != "" && ab.InstrumentName != null).Select(a => a.InstrumentName).Distinct().ToArray();
                cmbInstrument.Items.AddRange(dis);

                dgvMarketPicture.DefaultCellStyle.SelectionBackColor = dgvMarketPicture.BackgroundColor;


                // if (data.Best5Buy != null && data.Best5Sell != null && dgvMarketPicture.Rows.Count != 5)
                {
                    dgvMarketPicture.Rows.Clear();
                    for (int i = 0; i < 5; i++)
                    {
                        dgvMarketPicture.Rows.Insert(i);
                        dgvMarketPicture.Rows[i].Height = 20;
                    }
                }

                //  foreach (BroadcastConfig Config in ComData.AtsBroadcastList)
                {
                    //Config.broadcast.OnMarketPictureReceived += broadcast_OnMarketPictureReceived;
                    //Config.broadcast.OnDPRChangedReceived += broadcast_OnDPRChangedReceived;
                }

            }
            catch (Exception ex)
            {
                //AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }
        }

        void frmBestFive_FormClosed(object sender, FormClosedEventArgs e)
        {
            Client.Spread.AppGlobal.frmMarketDpth = null;
        }

        #region Private methods
        private void CreateColoumn()
        {
            try
            {
                dgvMarketPicture.Columns.Add(Constants.BuyOrders, Constants.BuyOrders);
                dgvMarketPicture.Columns.Add(Constants.BQty, Constants.BQty);
                dgvMarketPicture.Columns.Add(Constants.BuyPrice, Constants.BuyPrice);
                dgvMarketPicture.Columns.Add(Constants.SellPrice, Constants.SellPrice);
                dgvMarketPicture.Columns.Add(Constants.SQty, Constants.SQty);
                dgvMarketPicture.Columns.Add(Constants.SellOrders, Constants.SellOrders);

                dgvMarketPicture.Columns[Constants.BuyOrders].DefaultCellStyle.SelectionForeColor = dgvMarketPicture.Columns[Constants.BuyOrders].DefaultCellStyle.ForeColor = Color.Blue;
                dgvMarketPicture.Columns[Constants.BuyOrders].Width = 38;

                dgvMarketPicture.Columns[Constants.BQty].DefaultCellStyle.SelectionForeColor = dgvMarketPicture.Columns[Constants.BQty].DefaultCellStyle.ForeColor = Color.Blue;
                dgvMarketPicture.Columns[Constants.BQty].Width = 49;

                dgvMarketPicture.Columns[Constants.BuyPrice].DefaultCellStyle.SelectionForeColor = dgvMarketPicture.Columns[Constants.BuyPrice].DefaultCellStyle.ForeColor = Color.Blue;
                dgvMarketPicture.Columns[Constants.BuyPrice].Width = 74;

                dgvMarketPicture.Columns[Constants.SellPrice].DefaultCellStyle.SelectionForeColor = dgvMarketPicture.Columns[Constants.SellPrice].DefaultCellStyle.ForeColor = Color.Red;
                dgvMarketPicture.Columns[Constants.SellPrice].Width = 74;

                dgvMarketPicture.Columns[Constants.SQty].DefaultCellStyle.SelectionForeColor = dgvMarketPicture.Columns[Constants.SQty].DefaultCellStyle.ForeColor = Color.Red;
                dgvMarketPicture.Columns[Constants.SQty].Width = 49;

                dgvMarketPicture.Columns[Constants.SellOrders].DefaultCellStyle.SelectionForeColor = dgvMarketPicture.Columns[Constants.SellOrders].DefaultCellStyle.ForeColor = Color.Red;
                dgvMarketPicture.Columns[Constants.SellOrders].Width = 38;

                for (int i = 0; i < dgvMarketPicture.Columns.Count; i++)
                {
                    dgvMarketPicture.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvMarketPicture.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

            }
            catch (Exception ex)
            {
                // AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }
        }

        private void SetData(INTERACTIVE_ONLY_MBP data)
        {
            try
            {
                if (data.RecordBuffer != null && dgvMarketPicture.Rows.Count != 5)
                {
                    dgvMarketPicture.Rows.Clear();
                    for (int i = 0; i < 5; i++)
                    {
                        dgvMarketPicture.Rows.Insert(i);
                        dgvMarketPicture.Rows[i].Height = 20;
                    }

                    if (dgvMarketPicture.Rows.Count <= 0)
                        return;

                    for (int i = 0; i < 5; i++)
                    {
                        dgvMarketPicture[Constants.BuyOrders, i].Value = IPAddress.HostToNetworkOrder(data.RecordBuffer[i].NumberOfOrders);
                        dgvMarketPicture[Constants.BQty, i].Value = IPAddress.HostToNetworkOrder(data.RecordBuffer[i].Quantity);
                        dgvMarketPicture[Constants.BuyPrice, i].Value = IPAddress.HostToNetworkOrder(data.RecordBuffer[i].Price) / 100; // PriceDivisor;

                        dgvMarketPicture[Constants.SellPrice, i + 5].Value = IPAddress.HostToNetworkOrder(data.RecordBuffer[i].Price) / 100; // / PriceDivisor;
                        dgvMarketPicture[Constants.SQty, i + 5].Value = IPAddress.HostToNetworkOrder(data.RecordBuffer[i].Quantity);
                        dgvMarketPicture[Constants.SellOrders, i + 5].Value = IPAddress.HostToNetworkOrder(data.RecordBuffer[i].NumberOfOrders);
                    }

                    lblLastTradeTime.Text = AtsMethods.SecondsToDateTime(data.LastTradeTime).ToString(AtsConst.TimeFormatGrid);
                    lblLastTrdQty.Text = Convert.ToString(data.LastTradeQuantity) + "@" + Convert.ToDecimal(data.LastTradedPrice / 100).ToString();
                    //lblLastUpdt.Text = AtsMethods.SecondsToDateTime(data.Header.ExchangeTimeStamp).ToString(AtsConst.TimeFormatGrid);

                    lblAvgTradePrice.Text = (data.AverageTradePrice / 100).ToString();
                    lblHigh.Text = (data.HighPrice / 100).ToString(); ;
                    lbllow.Text = (data.LowPrice / 100).ToString();
                    //lblLtHigh.Text = (data.YearlyHigh / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    // lblLtlLow.Text = (data.YearlyLow / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                    //lblPrevClose.Text = (data.ClosePrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    lblOpen.Text = (data.OpenPrice / 100).ToString();
                    if (data.ClosingPrice != 0)
                        lblPercentage.Text = (((data.LastTradedPrice - data.ClosingPrice) / (decimal)data.ClosingPrice) * AtsConst.PriceDivisor100).ToString();

                    lblTotalBuyQuantity.Text = data.TotalBuyQuantity.ToString();
                    lblTotalSellQuantity.Text = data.TotalSellQuantity.ToString();
                    //lblOI.Text = Convert.ToString(data.CurrentOpenInterest);
                    //lblTotalTrades.Text = 

                    //lblValue.Text = (data.TotalTradedValue / 100).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    lblVolume.Text = data.VolumeTradedToday.ToString();


                }

                // if (ContractPanel != null && ContractPanel.ContractData != null && ContractPanel.ContractData.ContractInfo != null)
                {
                    //    if (ContractPanel.ContractData.ContractInfo.TokenNo == data.TokenNo
                    //        && ContractPanel.ContractData.ContractInfo.Exchange == data.Header.Exchange
                    //        && ContractPanel.ContractData.GatewayId == data.Header.GatewayID
                    //        )
                    //    {
                    //        if (data.Best5Buy != null && data.Best5Sell != null && dgvMarketPicture.Rows.Count != 5)
                    //        {
                    //            dgvMarketPicture.Rows.Clear();
                    //            for (int i = 0; i < 5; i++)
                    //            {
                    //                dgvMarketPicture.Rows.Insert(i);
                    //                dgvMarketPicture.Rows[i].Height = 20;
                    //            }
                    //        }
                    //        decimal PriceDivisor = data.PriceDivisor;
                    //        if (PriceDivisor == 0) return;

                    //        if (dgvMarketPicture.Rows.Count <= 0)
                    //            return;

                    //        for (int i = 0; i < 5; i++)
                    //        {
                    //            dgvMarketPicture[Constants.BuyOrders, i].Value = data.Best5Buy[i].TotalNumberOfOrders;
                    //            dgvMarketPicture[Constants.BQty, i].Value = data.Best5Buy[i].Quantity;
                    //            dgvMarketPicture[Constants.BuyPrice, i].Value = data.Best5Buy[i].OrderPrice / PriceDivisor;
                    //            dgvMarketPicture[Constants.SellPrice, i].Value = data.Best5Sell[i].OrderPrice / PriceDivisor;
                    //            dgvMarketPicture[Constants.SQty, i].Value = data.Best5Sell[i].Quantity;
                    //            dgvMarketPicture[Constants.SellOrders, i].Value = data.Best5Sell[i].TotalNumberOfOrders;
                    //        }

                    //        lblLastTradeTime.Text = AtsMethods.SecondsToDateTime(data.LastTradeTime).ToString(AtsConst.TimeFormatGrid);
                    //        lblLastTrdQty.Text = Convert.ToString(data.LastTradedQty) + "@" + Convert.ToDecimal(data.LastTradedPrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    //        lblLastUpdt.Text = AtsMethods.SecondsToDateTime(data.Header.ExchangeTimeStamp).ToString(AtsConst.TimeFormatGrid);

                    //        lblAvgTradePrice.Text = (data.AverageTradedPrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    //        lblHigh.Text = (data.HighPrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    //        lbllow.Text = (data.LowPrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    //        lblLtHigh.Text = (data.YearlyHigh / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    //        lblLtlLow.Text = (data.YearlyLow / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                    //        lblPrevClose.Text = (data.ClosePrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    //        lblOpen.Text = (data.OpenPrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    //        if (data.ClosePrice != 0)
                    //            lblPercentage.Text = (((data.LastTradedPrice - data.ClosePrice) / (decimal)data.ClosePrice) * AtsConst.PriceDivisor100).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                    //        lblTotalBuyQuantity.Text = data.TotalBuyQty.ToString(AtsConst.PriceFormatN0);
                    //        lblTotalSellQuantity.Text = data.TotalSellQty.ToString(AtsConst.PriceFormatN0);
                    //        lblOI.Text = Convert.ToString(data.CurrentOpenInterest);
                    //        lblTotalTrades.Text = data.TotalTrades.ToString(AtsConst.PriceFormatN0);

                    //        lblValue.Text = (data.TotalTradedValue / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    //        lblVolume.Text = (data.TotalQtyTraded).ToString(AtsConst.PriceFormatN0);

                    //        if (oldlasttradetime != data.LastTradeTime
                    //                    && oldLtp != 0 && data.LastTradedPrice != 0)
                    //        {
                    //            if (data.LastTradedPrice > oldLtp)
                    //            {
                    //               // pbtrend.BackgroundImage = Resources.TriangleGreen;
                    //            }
                    //            else if (data.LastTradedPrice < oldLtp)
                    //            {
                    //              //  pbtrend.BackgroundImage = Resources.TriangleRed;
                    //            }
                    //            else
                    //            {
                    //               // pbtrend.BackgroundImage = Resources.Pause;
                    //            }

                    //        }
                    //        oldLtp = data.LastTradedPrice;
                    //        oldlasttradetime = data.LastTradeTime;
                    //    }
                }
            }
            catch (Exception ex)
            {
                // AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }
        }

        private void lblResetText()
        {
            try
            {
                lblLastTrdQty.Text = string.Empty;
                lblLastTradeTime.Text = string.Empty;
                lblVolume.Text = string.Empty;
                lblValue.Text = string.Empty;
                lblAvgTradePrice.Text = string.Empty;
                lblPercentage.Text = string.Empty;
                lblTotalTrades.Text = string.Empty;
                lblOpen.Text = string.Empty;
                lblPrevClose.Text = string.Empty;
                lblHigh.Text = string.Empty;
                lbllow.Text = string.Empty;
                lblLtHigh.Text = string.Empty;
                lblLtlLow.Text = string.Empty;
                lblOI.Text = string.Empty;
                lblTotalBuyQuantity.Text = string.Empty;
                lblTotalSellQuantity.Text = string.Empty;
                lblLastUpdt.Text = string.Empty;
                // pbtrend.BackgroundImage = Resources.Pause;
            }
            catch (Exception ex)
            {
                // AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }
        }


        #endregion

        private void frmMarketDepth_Load(object sender, EventArgs e)
        {
            CreateColoumn();
            LZO_NanoData.LzoNanoData.Instance.OnDataChange += M_dpthOnOnDataChange;
            // ReadyDatatable(); 

        }


        private INTERACTIVE_ONLY_MBP _dataMbp;
        private double DoubleIndianChange(double value)
        {
            return BitConverter.ToDouble(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
        }

        delegate void OnLZOArrivedmktDelegate(Object o, ReadOnlyEventArgs<INTERACTIVE_ONLY_MBP> data);
        private void M_dpthOnOnDataChange(object sender, ReadOnlyEventArgs<INTERACTIVE_ONLY_MBP> data)
        {
            if (Token.Text != IPAddress.HostToNetworkOrder(data.Parameter.Token).ToString())
                return;
            if (dgvMarketPicture.Rows.Count == 0)
            {
                return;
            }
            if (dgvMarketPicture.InvokeRequired)
            {
                dgvMarketPicture.Invoke(new OnLZOArrivedmktDelegate(M_dpthOnOnDataChange), sender,new ReadOnlyEventArgs<INTERACTIVE_ONLY_MBP>(data.Parameter));
                return;
            }
            try
            {
                if (data.Parameter.RecordBuffer != null && dgvMarketPicture.Rows.Count == 5)
                {
                    dgvMarketPicture.Rows.Clear();
                    for (int i = 0; i < 5; i++)
                    {
                        dgvMarketPicture.Rows.Insert(i);
                        dgvMarketPicture.Rows[i].Height = 20;
                    }

                    if (dgvMarketPicture.Rows.Count <= 0)
                        return;

                    for (int i = 0; i < 5; i++)
                    {

                        dgvMarketPicture[Constants.BuyOrders, i].Value = IPAddress.HostToNetworkOrder(data.Parameter.RecordBuffer[i].NumberOfOrders);
                        dgvMarketPicture[Constants.BQty, i].Value = IPAddress.HostToNetworkOrder(data.Parameter.RecordBuffer[i].Quantity);
                        dgvMarketPicture[Constants.BuyPrice, i].Value =(decimal) IPAddress.HostToNetworkOrder(data.Parameter.RecordBuffer[i].Price) / 100; // PriceDivisor;
                         
                        dgvMarketPicture[Constants.SellPrice, i].Value = (decimal)IPAddress.HostToNetworkOrder(data.Parameter.RecordBuffer[i+5].Price) / 100; // / PriceDivisor;
                        dgvMarketPicture[Constants.SQty, i].Value = IPAddress.HostToNetworkOrder(data.Parameter.RecordBuffer[i+5].Quantity);
                        dgvMarketPicture[Constants.SellOrders, i].Value = IPAddress.HostToNetworkOrder(data.Parameter.RecordBuffer[i+5].NumberOfOrders);
                  
                    }

                    lblLastTradeTime.Text = AtsMethods.SecondsToDateTime(IPAddress.HostToNetworkOrder(data.Parameter.LastTradeTime)).ToString(AtsConst.TimeFormatGrid);
                    lblLastTrdQty.Text = Convert.ToString(IPAddress.HostToNetworkOrder(data.Parameter.LastTradeQuantity)) + "@" + Convert.ToDecimal(IPAddress.HostToNetworkOrder(data.Parameter.LastTradedPrice) / 100).ToString();
                    //lblLastUpdt.Text = AtsMethods.SecondsToDateTime(data.Header.ExchangeTimeStamp).ToString(AtsConst.TimeFormatGrid);

                    lblAvgTradePrice.Text = (IPAddress.HostToNetworkOrder(data.Parameter.AverageTradePrice) / 100).ToString();
                    lblHigh.Text = (IPAddress.HostToNetworkOrder(data.Parameter.HighPrice) / 100).ToString(); ;
                    lbllow.Text = (IPAddress.HostToNetworkOrder(data.Parameter.LowPrice) / 100).ToString();
                    //lblLtHigh.Text = (data.YearlyHigh / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    // lblLtlLow.Text = (data.YearlyLow / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                    //lblPrevClose.Text = (data.ClosePrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    lblOpen.Text = (IPAddress.HostToNetworkOrder(data.Parameter.OpenPrice) / 100).ToString();
                    if (IPAddress.HostToNetworkOrder(data.Parameter.ClosingPrice) != 0)
                        lblPercentage.Text = (((IPAddress.HostToNetworkOrder(data.Parameter.LastTradedPrice) - IPAddress.HostToNetworkOrder(data.Parameter.ClosingPrice)) / (decimal)IPAddress.HostToNetworkOrder(data.Parameter.ClosingPrice)) * AtsConst.PriceDivisor100).ToString();

                    lblTotalBuyQuantity.Text = DoubleIndianChange(data.Parameter.TotalBuyQuantity).ToString();
                    lblTotalSellQuantity.Text = DoubleIndianChange(data.Parameter.TotalSellQuantity).ToString();
                    //lblOI.Text = Convert.ToString(data.Parameter.CurrentOpenInterest);
                    //lblTotalTrades.Text = 

                    //                    lblValue.Text = (data.Parameter.TotalTradedValue / 100).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    lblVolume.Text = IPAddress.HostToNetworkOrder(data.Parameter.VolumeTradedToday).ToString();


                }
            }
            catch (Exception ex)
            {

            }







        }
        public static DateTime ConvertFromTimestamp(long timstamp)
        {
            DateTime datetime = new DateTime(1980, 1, 1, 0, 0, 0, 0);
            return datetime.AddSeconds(timstamp);
        }
        List<long> T = new List<long>();
        private void cmbSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbExpirty.Text = "";
            cmbExpirty.Items.Clear();

            // var exp = CSV_Class.cimlist.Where(a => a.Symbol == SYMcomboBox4.Text && a.InstrumentName == INSTcomboBox3.Text).OrderBy(r => r.ExpiryDate).Select(d=>d.ExpiryDate).Distinct().ToList();
            T = CSV_Class.cimlist.Where(a => a.Symbol == cmbSymbol.Text && a.InstrumentName == cmbInstrument.Text).OrderBy(s => s.ExpiryDate).Select(d => d.ExpiryDate).Distinct().ToList();

            //   T = Holder.clliest_contractfile.Where(a => a.Symbol == SYMcomboBox4.Text && a.InstrumentName == INSTcomboBox3.Text).OrderBy(s => s.ExpiryDate).Select(d => d.ExpiryDate).Distinct().ToList();

            // IEnumerable<long> exp = CSV_Class.cimlist.Where(a => a.Symbol == SYMcomboBox4.Text && a.InstrumentName == INSTcomboBox3.Text).Select(r => r.ExpiryDate).Distinct().ToList();

            foreach (long ex in T)
            {

                string on = ConvertFromTimestamp(ex).ToShortDateString();

                cmbExpirty.Items.Add(on);
                //  date = ex;

            }

            cmbSeries.Items.Clear();
            // cmbStrikePrice.Items.Clear();
            cmbStrikePrice.Text = "";
            cmbSeries.Text = "";
        }

        private void cmbInstrument_SelectedIndexChanged(object sender, EventArgs e)
        {

            cmbSymbol.Text = "";
            cmbSymbol.Items.Clear();
            cmbExpirty.Text = "";
            cmbSeries.Text = "";
            string[] symm = CSV_Class.cimlist.Where(a => a.InstrumentName == cmbInstrument.Text && a.InstrumentName != "" && a.InstrumentName != null).Select(q => q.Symbol).Distinct().ToArray();


            cmbSymbol.Items.AddRange(symm);
            cmbSeries.Enabled = true;
            cmbStrikePrice.Enabled = true;
            cmbExpirty.Items.Clear();
            cmbSeries.Items.Clear();
            cmbStrikePrice.Text = "";

            if (cmbInstrument.Text == "FUTIVX" || cmbInstrument.Text == "FUTIDX" || cmbInstrument.Text == "FUTSTK")
            {
                cmbSeries.Enabled = false;
                cmbStrikePrice.Enabled = false;
                cmbExpirty.Items.Clear();
                cmbSeries.Items.Clear();
                //cmbStrikePrice.Items.Clear();
                cmbStrikePrice.Text = "";
            }
        }
        long date = 0;
        private void cmbExpirty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = cmbExpirty.SelectedIndex;
                if (i <= 0)
                    return;
                var t=this.T;
               // T = CSV_Class.cimlist.Where(a => a.Symbol == cmbSymbol.Text && a.InstrumentName == cmbInstrument.Text).OrderBy(s => s.ExpiryDate).Select(d => d.ExpiryDate).Distinct().ToList();
                date = T[i];
                cmbSeries.Text = "";
                cmbSeries.Items.Clear();
                // STRIKecomboBox7.Items.Clear();
                cmbStrikePrice.Text = "";
                string df = cmbInstrument.Text;

                string[] op = CSV_Class.cimlist.Where(a => a.ExpiryDate == date && a.InstrumentName == cmbInstrument.Text && a.Symbol == cmbSymbol.Text).Select(s => s.OptionType).Distinct().ToArray();
                // string[] op = Holder.clliest_contractfile.Where(a => a.ExpiryDate == date && a.InstrumentName == INSTcomboBox3.Text && a.Symbol == SYMcomboBox4.Text).Select(s => s.OptionType).Distinct().ToArray();
               
                var tokenw = CSV_Class.cimlist.Where(a => a.ExpiryDate == date && a.Symbol == cmbSymbol.Text).First().Token;


                //int t = CSV_Class.cimlist.FindIndex(q => q.Symbol == cmbSymbol.Text && q.ExpiryDate == this.date && q.InstrumentName == cmbInstrument.Text);

                //tokenw = CSV_Class.cimlist[t].Token;
                //token = tokenw.ToString();
                cmbSeries.Items.AddRange(op);
            }
            catch(Exception ex)
            {

            }
        }

        private void cmbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            var p = CSV_Class.cimlist.Where(a => a.ExpiryDate == date && a.InstrumentName == cmbInstrument.Text && a.Symbol == cmbSymbol.Text && a.OptionType == cmbSeries.Text).OrderBy(p1 => p1.StrikePrice).Select(a => a.StrikePrice / 100).Distinct().ToList();
            cmbStrikePrice.DataSource = p;
            cmbStrikePrice.DisplayMember = "StrikePrice";
        }

        private void cmbExpirty_KeyDown(object sender, KeyEventArgs e)
        {
            if (!cmbInstrument.Text.Substring(0, 3).Equals("OPT".ToUpper()))
            {
            if (e.KeyCode==Keys.Down || e.KeyCode==Keys.Up)
                
                    AddSymbolFirstTime(e);
                }
        }
        public void AddSymbolFirstTime(KeyEventArgs e)
                {
            try
            {
                int token = 0;
                if (cmbSymbol.Text == "" && cmbInstrument.Text == "")
                {
                    MessageBox.Show("Selected Token not find ");
                    return;
                }
                if (cmbInstrument.Text == "FUTIVX" || cmbInstrument.Text == "FUTIDX" || cmbInstrument.Text == "FUTSTK")
                {
                     int i ;
                     if (e.KeyCode != Keys.Up)
                     {
                         i = cmbExpirty.SelectedIndex + 1;
                     }
                     else 
                     {
                         i = cmbExpirty.SelectedIndex-1;
                     }
                     
                    if (i < 0)
                        return;

                   // T = CSV_Class.cimlist.Where(a => a.Symbol == cmbSymbol.Text && a.InstrumentName == cmbInstrument.Text).OrderBy(s => s.ExpiryDate).Select(d => d.ExpiryDate).Distinct().ToList();
                    date = T[i];
                    int t = CSV_Class.cimlist.FindIndex(q => q.Symbol == cmbSymbol.Text && q.ExpiryDate == this.date && q.InstrumentName == cmbInstrument.Text);
                    token = CSV_Class.cimlist[t].Token;
                }
                else
                {
                    if (cmbSymbol.Text == "" || cmbExpirty.Text == "" || cmbSeries.Text == "" || cmbInstrument.Text == "" || cmbStrikePrice.Text == "")
                    {
                        MessageBox.Show("Please Select All Fiels ...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int t = CSV_Class.cimlist.FindIndex(q => q.Symbol == cmbSymbol.Text && q.ExpiryDate == this.date && q.InstrumentName == cmbInstrument.Text && q.OptionType == cmbSeries.Text && q.StrikePrice == Convert.ToInt32(cmbStrikePrice.Text) * 100);
                    token = CSV_Class.cimlist[t].Token;
                }
                LZO_NanoData.LzoNanoData.Instance.Subscribe = token;
                Token.Text = Convert.ToString(token);
                Global.Instance.Data_With_Nano.AddOrUpdate(token, ClassType.MARKETWTCH, (k, v) => ClassType.MARKETWTCH);
            }
            catch(Exception ex)
            {

            }

        }

        private void cmbStrikePrice_KeyDown(object sender, KeyEventArgs e)
         {
            if (e.KeyCode == Keys.Enter)
                if (!cmbInstrument.Text.Substring(1, 3).Equals("OPT"))
                    AddSymbolFirstTime(e);
         }

        private void frmMarketDepth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void dgvMarketPicture_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
