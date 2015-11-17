using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AtsApi.Common;
using LogWriter;
using AtsCommon;
using Structure;
using Client;
using System.Net;
using System.Data;
using Client.LZO_NanoData;


namespace Client.Spread
{
    public partial class FrmMarketPicture : Form
    {
        #region Variables
        private int oldLtp;
        private int oldlasttradetime;
        #endregion

        #region Constructor

        delegate void OnLZOArrivedDelegate(Object o, ReadOnlyEventArgs<Client.MS_SPD_MKT_INFO_7211> Stat);
        public FrmMarketPicture()
        {
            InitializeComponent();

            //ContractPanel.DsContractCollection = ComData.DsContract;
            //ContractPanel.AllowedGateway = ComData.AllowedGateway;
           // ContractPanel.btnAdd.Visible = false;

            pbCollExp.Click += pbCollExp_Click;
           
            FormClosing += frmBestFive_FormClosing;
            Load += frmBestFive_Load;
            FormClosed += frmBestFive_FormClosed;
            KeyDown += frmBestFive_KeyDown;
          //  CreateColoumn();
        }

        #endregion

        #region Form Events
        void frmBestFive_Load(object sender, EventArgs e)
        {
            try
            {

                //if (AppGlobal.frmMain.dockMain.ActiveContent != null
                //    && AppGlobal.frmMain.dockMain.ActiveContent == AppGlobal.frmMarketWatch
                //    && AppGlobal.frmMarketWatch != null
                //    && AppGlobal.frmMarketWatch.dgvMarketWatch.Rows.Count > 0
                //    && AppGlobal.frmMarketWatch.dgvMarketWatch.SelectedRows.Count > 0
                //    && AppGlobal.MarketWatch != null)
                //{
                //    int rowIndex = AppGlobal.frmMarketWatch.dgvMarketWatch.SelectedRows[0].Index;
                //    if (AppGlobal.MarketWatch[rowIndex] != null)
                //        ContractPanel.SetData(AppGlobal.MarketWatch[rowIndex].GatewayId,
                //                                        AppGlobal.MarketWatch[rowIndex].ContractInfo.TokenNo,
                //                                        AppGlobal.MarketWatch[rowIndex].ContractInfo.Exchange);

                //    SetData(AppGlobal.MarketWatch[rowIndex].MarketPicture);
                //}

              
                //AppGlobal.BestFiveCounter++;
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
            catch (Exception ex2)
            {
                //AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }
        }

        void frmBestFive_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
               // AppGlobal.BestFiveCounter = AppGlobal.BestFiveCounter - 1;
                //foreach (BroadcastConfig Config in ComData.AtsBroadcastList)
                {
                 //   Config.broadcast.OnMarketPictureReceived -= broadcast_OnMarketPictureReceived;
                 //   Config.broadcast.OnDPRChangedReceived -= broadcast_OnDPRChangedReceived;
                }
            }
            catch (Exception ex)
            {
               // AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }
        }

        void frmBestFive_FormClosed(object sender, FormClosedEventArgs e)
        {
            AppGlobal.frmMarketPicture = null;
        }

        void frmBestFive_KeyDown(object sender, KeyEventArgs e)
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
        #endregion

        #region Contract panel event
        void ContractPanel_OnComboChange(object sender, CustomControls.ContractArgs args)
        {
            try
            {
               
                dgvMarketPicture.Rows.Clear();
                lblResetText();

                //int item = ComData.AtsBroadcastList.ToList().FindIndex(x => x.GatewayID == (AtsEnums.GatewayId)ContractPanel.ContractData.GatewayId);

                //if (item >= 0)
                //{
                //    ComData.AtsBroadcastList[item].broadcast.TokenRequest(ContractPanel.ContractData.ContractInfo, ContractPanel.ContractData.GatewayId);
                                                          
                //}

               
            }
            catch (Exception ex)
            {
               // AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }
        }
        #endregion

        #region Brodcast data
         MS_SPD_MKT_INFO_7211 data=new MS_SPD_MKT_INFO_7211();
         delegate void OnLZOArrivedmktDelegate(Object o, ReadOnlyEventArgs<MS_SPD_MKT_INFO_7211> Stat);
         public void UpdateRecord(object sender, ReadOnlyEventArgs<MS_SPD_MKT_INFO_7211> Stat)
         {
           //  data = Stat.Parameter;
             if (dgvMarketPicture.InvokeRequired)
             {
                 dgvMarketPicture.Invoke(new OnLZOArrivedmktDelegate(UpdateRecord), sender, new ReadOnlyEventArgs<MS_SPD_MKT_INFO_7211>(Stat.Parameter));
                 return;
             }
             else
             {
                 SetData(Stat.Parameter);
                 //try
                 //{
                 //    if (data != null && dgvMarketPicture.Rows.Count == 5)
                 //    {
                 //        dgvMarketPicture.Rows.Clear();
                 //        for (int i = 0; i < 5; i++)
                 //        {
                 //            dgvMarketPicture.Rows.Insert(i);
                 //            dgvMarketPicture.Rows[i].Height = 20;
                 //        }

                 //        if (dgvMarketPicture.Rows.Count <= 0)
                 //            return;

                 //        for (int i = 0; i < 5; i++)
                 //        {
                 //            dgvMarketPicture[Constants.BuyOrders, i].Value = IPAddress.HostToNetworkOrder(data.mbpBuys[i].NoOrders);
                 //            dgvMarketPicture[Constants.BQty, i].Value = IPAddress.HostToNetworkOrder(data.mbpBuys[i].Volume);
                 //            dgvMarketPicture[Constants.BuyPrice, i].Value = IPAddress.HostToNetworkOrder(data.mbpBuys[i].Price) / 100; // PriceDivisor;
                 //            dgvMarketPicture[Constants.SellPrice, i].Value = IPAddress.HostToNetworkOrder(data.mbpSells[i].Price) / 100; // / PriceDivisor;
                 //            dgvMarketPicture[Constants.SQty, i].Value = IPAddress.HostToNetworkOrder(data.mbpSells[i].Volume);
                 //            dgvMarketPicture[Constants.SellOrders, i].Value = IPAddress.HostToNetworkOrder(data.mbpSells[i].NoOrders);
                 //        }

                 //        //lblLastTradeTime.Text = AtsMethods.SecondsToDateTime(data.LastTradeTime).ToString(AtsConst.TimeFormatGrid);
                 //        //lblLastTrdQty.Text = Convert.ToString(data.LastTradedQty) + "@" + Convert.ToDecimal(data.LastTradedPrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                 //        //lblLastUpdt.Text = AtsMethods.SecondsToDateTime(data.Header.ExchangeTimeStamp).ToString(AtsConst.TimeFormatGrid);

                 //        //lblAvgTradePrice.Text = (data.AverageTradedPrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                 //        //lblLtHigh.Text = (data.YearlyHigh / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                 //        // lblLtlLow.Text = (data.YearlyLow / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                 //        //lblPrevClose.Text = (data.ClosePrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                 //        //if (data.ClosePrice != 0)
                 //        //    lblPercentage.Text = (((data.LastTradedPrice - data.ClosePrice) / (decimal)data.ClosePrice) * AtsConst.PriceDivisor100).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                 //        lblTotalBuyQuantity.Text = data.MbpBuy.ToString(AtsConst.PriceFormatN0);
                 //        lblTotalSellQuantity.Text = data.MbpSell.ToString(AtsConst.PriceFormatN0);
                 //        //lblOI.Text = Convert.ToString(data.CurrentOpenInterest);
                 //        lblTotalTrades.Text = data.TotalTradedValue.ToString(AtsConst.PriceFormatN0);


                 //        lblVolume.Text = (data.totalOrdVolume.Buy + data.totalOrdVolume.Sell).ToString(AtsConst.PriceFormatN0);


                 //    }
                 //}
                 //catch (Exception ex)
                 //{
                 //    // AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
                 //}
             }
         }
        //void broadcast_OnMarketPictureReceived(AtsBCastPackets.MarketPicture marketPicture)
        //{
        //    try
        //    {
        //        if (InvokeRequired)
        //        {
        //            BeginInvoke((MethodInvoker)(() => broadcast_OnMarketPictureReceived(marketPicture)));
        //        }
        //        else
        //        {
        //            SetData(marketPicture);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //      //  AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
        //    }
        //}

        //void broadcast_OnDPRChangedReceived(AtsBCastPackets.DprNotification dprChanged)
        //{
        //    try
        //    {
        //        if (InvokeRequired)
        //        {
        //            BeginInvoke((MethodInvoker)(() => broadcast_OnDPRChangedReceived(dprChanged)));
        //        }
        //        else
        //        {
        //            if (ContractPanel.ContractData.ContractInfo.TokenNo == dprChanged.TokenNo &&
        //                ContractPanel.ContractData.ContractInfo.Exchange == dprChanged.Header.Exchange &&
        //                ContractPanel.ContractData.GatewayId == dprChanged.Header.GatewayID)
        //            {
        //                if (dprChanged.PriceDivisor != 0)
        //                {
        //                    lblDprLow.Text = Convert.ToDecimal(dprChanged.LowPrice / (decimal)dprChanged.PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
        //                    lblDprHigh.Text = Convert.ToDecimal(dprChanged.HighPrice / (decimal)dprChanged.PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //       // AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
        //    }
        //}
        
        #endregion

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

        private void SetData(MS_SPD_MKT_INFO_7211 data)
        
        {
            if(token.Text!=IPAddress.HostToNetworkOrder(data.Token1).ToString()+IPAddress.HostToNetworkOrder(data.Token2).ToString())
            {
                return;
            }
            try
            {
                if (data.mbpBuys != null && data.mbpSells != null && dgvMarketPicture.Rows.Count == 5)
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
                         dgvMarketPicture[Constants.BuyOrders, i].Value = IPAddress.HostToNetworkOrder(data.mbpBuys[i].NoOrders);
                         dgvMarketPicture[Constants.BQty, i].Value = IPAddress.HostToNetworkOrder(data.mbpBuys[i].Volume);
                         dgvMarketPicture[Constants.BuyPrice, i].Value = (decimal)IPAddress.HostToNetworkOrder(data.mbpBuys[i].Price)/100; // PriceDivisor;
                         dgvMarketPicture[Constants.SellPrice, i].Value =(decimal) IPAddress.HostToNetworkOrder(data.mbpSells[i].Price)/100; // / PriceDivisor;
                         dgvMarketPicture[Constants.SQty, i].Value = IPAddress.HostToNetworkOrder(data.mbpSells[i].Volume);
                         dgvMarketPicture[Constants.SellOrders, i].Value = IPAddress.HostToNetworkOrder(data.mbpSells[i].NoOrders);
                     }

                     //lblLastTradeTime.Text = AtsMethods.SecondsToDateTime(data.LastTradeTime).ToString(AtsConst.TimeFormatGrid);
                     //lblLastTrdQty.Text = Convert.ToString(data.TradedVolume) + "@" + Convert.ToDecimal(data.LastTradedPriceDifference / 100).ToString();//ContractPanel.ContractData.ContractDetail.PriceFormat);
                     //lblLastUpdt.Text = AtsMethods.SecondsToDateTime(data.mHeader.ExchangeTimeStamp).ToString(AtsConst.TimeFormatGrid);

                     //lblAvgTradePrice.Text = (data.AverageTradedPrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                     //lblLtHigh.Text = (data.YearlyHigh / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                     //lblLtlLow.Text = (data.YearlyLow / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                     //lblPrevClose.Text = (data.ClosePrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                    
                     lblLastTradeTime.Text = AtsMethods.SecondsToDateTime(IPAddress.HostToNetworkOrder(data.LastActiveTime)).ToString(AtsConst.TimeFormatGrid);
                     lblLastTrdQty.Text = Convert.ToString(IPAddress.HostToNetworkOrder(data.TradedVolume)) + "@" + Convert.ToDecimal(IPAddress.HostToNetworkOrder(data.LastTradedPriceDifference) / 100).ToString();//ContractPanel.ContractData.ContractDetail.PriceFormat);
                     lblLastUpdt.Text = AtsMethods.SecondsToDateTime(IPAddress.HostToNetworkOrder(data.LastUpdateTime)).ToString(AtsConst.TimeFormatGrid);
                    //lblAvgTradePrice.Text = (data.AverageTradedPrice /100).ToString();
                   
                     lblLtHigh.Text = (IPAddress.HostToNetworkOrder(data.DayHighPriceDifference)/ 100).ToString();
                     lblLtlLow.Text = (IPAddress.HostToNetworkOrder(data.DayLowPriceDifference) / 100).ToString();

                     //lblPrevClose.Text = (data.ClosePrice / PriceDivisor).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);
                   
                     //if (data.ClosePrice != 0)
                     //    lblPercentage.Text = (((data.LastTradedPrice - data.ClosePrice) / (decimal)data.ClosePrice) * AtsConst.PriceDivisor100).ToString(ContractPanel.ContractData.ContractDetail.PriceFormat);

                     lblTotalBuyQuantity.Text = data.MbpBuy.ToString(AtsConst.PriceFormatN0);
                     lblTotalSellQuantity.Text = data.MbpSell.ToString(AtsConst.PriceFormatN0);
                     //lblOI.Text = Convert.ToString(data.CurrentOpenInterest);
                     lblTotalTrades.Text = ((long)LogicClass.DoubleEndianChange(data.TotalTradedValue)).ToString();//AtsConst.PriceFormatN0);


                     lblVolume.Text = ((long)(LogicClass.DoubleEndianChange(data.totalOrdVolume.Buy) + LogicClass.DoubleEndianChange(data.totalOrdVolume.Sell))).ToString();//AtsConst.PriceFormatN0);


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

        #region Picture event
        void pbCollExp_Click(object sender, EventArgs e)
        {
            try
            {
                if (pbCollExp.Tag.ToString() == "Expanded")
                {
                    pbCollExp.Tag = "Collapsed";
                   // pbCollExp.BackgroundImage = Resources.ExpandRight;
                    pbCollExp.Cursor = Cursors.PanEast;
                    Width = 335;
                    pbCollExp.Left = 296;
                }
                else
                {
                    pbCollExp.Tag = "Expanded";
                  //  pbCollExp.BackgroundImage = Resources.ExpandLeft;
                    pbCollExp.Cursor = Cursors.PanWest;
                    Width = 850;
                    pbCollExp.Left = 810;
                }
            }
            catch (Exception ex)
            {
              //  AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }
        }
        #endregion

        private void FrmMarketPicture_Load(object sender, EventArgs e)
        {
            CreateColoumn();
           // var distinctInstNames = (from row in CommonData.dtSpreadContract.AsEnumerable() select row.Field<string>("InstrumentName1")).Distinct().ToArray();
           // cmbInstrument.Items.AddRange(distinctInstNames);
            //if (cmbInstrument.Items.Count>0)
              //  cmbInstrument.SelectedIndex = 0;
            LzoNanoData.Instance.OnSpreadDataChange += UpdateRecord;
        }

        private void ContractPanel_Load(object sender, EventArgs e)
        {

        }
        int selectedTkn1, selectedTkn2;
        private void cmbExpirty_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime userDate = DateTime.ParseExact(((cmbExpirty.SelectedItem.ToString().Split('-')[0]).ToUpper()), "ddMMMyy", null);
            DateTime dateValue = new DateTime(userDate.Year, userDate.Month, userDate.Day, 14, 30, 0, 0);
            selectedExp1 = CommonData.UnixTimestampFromDateTime(dateValue);
            userDate = DateTime.ParseExact(((cmbExpirty.SelectedItem.ToString().Split('-')[1]).ToUpper()), "ddMMMyy", null);
            dateValue = new DateTime(userDate.Year, userDate.Month, userDate.Day, 14, 30, 0, 0);
            selectedExp2 = CommonData.UnixTimestampFromDateTime(dateValue);


           // cmbSymbol.Items.Clear();
            var distiSymbol = (from row in CommonData.dtSpreadContract.AsEnumerable()
                               where row.Field<string>("InstrumentName1") == cmbInstrument.SelectedItem.ToString()
                               && row.Field<string>("Symbol1") == cmbSymbol.Text.ToString()
                               && row.Field<int>("StrikePrice1") == Convert.ToInt32(cmbStrikePrice.SelectedItem.ToString())
                               && row.Field<string>("OptionType1") == cmbSeries.SelectedItem.ToString()
                               && row.Field<int>("ExpiryDate1") == selectedExp1
                               && row.Field<int>("ExpiryDate2") == selectedExp2
                               select new
                               {
                                   sym1 = row.Field<string>("Symbol1"),
                                   sym2 = row.Field<string>("Symbol2"),
                                   tk1 = row.Field<int>("Token1"),
                                   tk2 = row.Field<int>("Token2")
                               });

            foreach (var i in distiSymbol)
            {
                var combinedSym = (i.sym1 + cmbExpirty.SelectedItem.ToString().Split('-')[0]).ToUpper() + (i.sym2 + cmbExpirty.SelectedItem.ToString().Split('-')[1]).ToUpper();
               // cmbSymbol.Items.Add(combinedSym);
                selectedTkn1 = i.tk1;
                selectedTkn2 = i.tk2;
            }
            token.Text = selectedTkn1.ToString() + selectedTkn2.ToString();

            LZO_NanoData.LzoNanoData.Instance.Subscribe = Convert.ToInt64(selectedTkn1.ToString() + selectedTkn2.ToString());
            Global.Instance.Data_With_Nano.AddOrUpdate(Convert.ToInt64(selectedTkn1.ToString() + selectedTkn2.ToString()), ClassType.SPREAD, (k, v) => ClassType.SPREAD);
            if (cmbSymbol.Items.Count > 0)
            {
                //cmbSymbol.SelectedIndex = 0;
            }
              
        }

        private void cmbInstrument_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            cmbSymbol.Items.Clear();
            cmbSymbol.Text = "";
            var distinctSymbol = (from m in CommonData.dtSpreadContract.AsEnumerable() where m.Field<string>("InstrumentName1") ==cmbInstrument.SelectedItem.ToString() select m.Field<string>("Symbol1")).Distinct().ToArray();
            cmbSymbol.Items.AddRange(distinctSymbol);
            if (cmbSymbol.Items.Count > 0)
            {
             //   cmbSymbol.SelectedIndex = 0;
            }
        }

        private void cmbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbStrikePrice.Items.Clear();
            var distinctStrikePrice = (from row in CommonData.dtSpreadContract.AsEnumerable()
                                       where row.Field<string>("InstrumentName1") == cmbInstrument.SelectedItem.ToString()
                                       && row.Field<string>("Symbol1") == cmbSymbol.Text.ToString()
                                       && row.Field<string>("OptionType1") == cmbSeries.SelectedItem.ToString()
                                       select row.Field<Int32>("StrikePrice1").ToString()).Distinct().ToArray();

            cmbStrikePrice.Items.AddRange(distinctStrikePrice);
            if (distinctStrikePrice.Length > 0)
                cmbStrikePrice.SelectedIndex = 0;
        }

        private void cmbSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSeries.Items.Clear();
            cmbExpirty.Items.Clear();
            cmbSeries.Items.Clear();

            var distinctOpType = (from row in CommonData.dtSpreadContract.AsEnumerable()
                                  where row.Field<string>("InstrumentName1") ==cmbInstrument.SelectedItem.ToString()
                                  && row.Field<string>("Symbol1") == cmbSymbol.Text.ToString()
                                  select row.Field<string>("OptionType1")).Distinct().ToArray();

         //   cmbSymbol.Text = cmbSymbol.SelectedItem.ToString();
            cmbSeries.Items.AddRange(distinctOpType);
            if (distinctOpType.Length > 0)
                cmbSeries.SelectedIndex = 0;
        }
        long selectedExp1, selectedExp2;
        private void cmbStrikePrice_SelectedIndexChanged(object sender, EventArgs e)
        {

            cmbExpirty.Items.Clear();
            selectedExp1 = 0;
            selectedExp2 = 0;
            var distinctExpiry = (from row in CommonData.dtSpreadContract.AsEnumerable()
                                  where row.Field<string>("InstrumentName1") ==cmbInstrument.SelectedItem.ToString()
                                  && row.Field<string>("Symbol1") == cmbSymbol.Text.ToString()
                                  && row.Field<int>("StrikePrice1") == Convert.ToInt32(cmbStrikePrice.SelectedItem.ToString())
                                  && row.Field<string>("OptionType1") ==cmbSeries.SelectedItem.ToString()
                                  select new
                                  {
                                      dt1 = row.Field<int>("ExpiryDate1"),
                                      dt2 = row.Field<int>("ExpiryDate2")
                                  });

            foreach (var i in distinctExpiry)
            {
                selectedExp1 = i.dt1;
                selectedExp2 = i.dt2;

                var combinedExpiry = (CommonData.UnixTimeStampToDateTime(i.dt1).ToString("ddMMMyy-").ToUpper() + CommonData.UnixTimeStampToDateTime(i.dt2).ToString("ddMMMyy").ToUpper());
                cmbExpirty.Items.Add(combinedExpiry);
            }
            if (cmbExpirty.Items.Count > 0)
                cmbExpirty.SelectedIndex = 0;
        }

        private void cmbExpirty_Enter(object sender, EventArgs e)
        {
            token.Text = selectedTkn1.ToString() + selectedTkn2.ToString();

            LZO_NanoData.LzoNanoData.Instance.Subscribe = Convert.ToInt64(selectedTkn1.ToString() + selectedTkn2.ToString());
            Global.Instance.Data_With_Nano.AddOrUpdate(Convert.ToInt64(selectedTkn1.ToString() + selectedTkn2.ToString()), ClassType.SPREAD, (k, v) => ClassType.SPREAD);
       
        }

        
    }
}
