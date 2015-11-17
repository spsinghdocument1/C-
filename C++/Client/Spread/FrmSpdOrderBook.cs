using System;
using System.Data;
using System.Windows.Forms;
using CustomControls.AtsGrid;
using WeifenLuo.WinFormsUI.Docking;
using AtsApi.Common;

using System.Xml;
using System.IO;
using System.Drawing;
using OrderBook.AppClasses;
using Structure;
using System.Net;

namespace Client.Spread
{
    public partial class FrmSpdOrderBook : DockContent
    {
        #region Variables
        // private ContractInformation ContractInfo;
        // private ContractDetails cond;
        string orderprice = string.Empty;
        private readonly ToolStripMenuItem tlsmiCancelSelected;
        private readonly ToolStripMenuItem tlsmiCancelAll;
        private readonly ToolStripMenuItem tlsmiCancelBuy;
        private readonly ToolStripMenuItem tlsmiCancelSell;
        private readonly ToolStripMenuItem tlsmiModifySelected;

        DataView dvSpdOrderBook;

        #endregion


        public static int[] LoadFormLocationAndSize(Form xForm)
        {

            int[] t = { 0, 0, 300, 300 };
            if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "formorderclose.xml"))
                return t;
            DataSet dset = new DataSet();
            dset.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "formorderclose.xml");
            int[] LocationAndSize = new int[] { xForm.Location.X, xForm.Location.Y, xForm.Size.Width, xForm.Size.Height };
            //---//
            try
            {
                // Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                var AbbA = dset.Tables[0].Rows[0]["Input"].ToString().Split(';');
                //---//
                LocationAndSize[0] = Convert.ToInt32(AbbA[0]);
                LocationAndSize[1] = Convert.ToInt32(AbbA[1]);
                LocationAndSize[2] = Convert.ToInt32(AbbA[2]);
                LocationAndSize[3] = Convert.ToInt32(AbbA[3]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return LocationAndSize;
        }
 public static void SaveFormLocationAndSize(object sender, FormClosingEventArgs e)
        {
            Form xForm = sender as Form;
            var settings = new XmlWriterSettings { Indent = true };

            XmlWriter writer = XmlWriter.Create(Application.StartupPath + Path.DirectorySeparatorChar + "formorderclose.xml", settings);

            writer.WriteStartDocument();

            writer.WriteStartElement("Columns");


            string encodedXml = String.Format("{0};{1};{2};{3}", xForm.Location.X, xForm.Location.Y, xForm.Size.Width, xForm.Size.Height);
            writer.WriteStartElement("Column");
            writer.WriteAttributeString("Input", encodedXml);
            writer.WriteEndElement();

            writer.WriteEndElement();

            writer.WriteEndDocument();

            writer.Flush();

            writer.Close();

        }
       

        public FrmSpdOrderBook()
        {
            InitializeComponent();
            Load += FrmSpdOrderBook_Load;
            KeyDown += FrmSpdOrderBook_KeyDown;
            FormClosed += FrmSpdOrderBook_FormClosed;

            ToolStripSeparator tlsmod = new ToolStripSeparator();
            dgvSpdOrderBook.cmsColumn.Items.Insert(0, tlsmod);

            tlsmiModifySelected = new ToolStripMenuItem();
            tlsmiModifySelected.Name = "tlsmiModifySelected";
            tlsmiModifySelected.Text = "Modify Order";
            dgvSpdOrderBook.cmsColumn.Items.Insert(0, tlsmiModifySelected);

            ToolStripSeparator tls = new ToolStripSeparator();
            dgvSpdOrderBook.cmsColumn.Items.Add(tls);

            ToolStripMenuItem items = new ToolStripMenuItem();
            items.Name = "tlsmiCancel";
            items.Text = "Cancel Orders";

            tlsmiCancelSelected = new ToolStripMenuItem();
            tlsmiCancelSelected.Name = "tlsmiCancelSelected";
            tlsmiCancelSelected.Text = "Selected";

            tlsmiCancelAll = new ToolStripMenuItem();
            tlsmiCancelAll.Name = "tlsmiCancelAll";
            tlsmiCancelAll.Text = "All";

            tlsmiCancelBuy = new ToolStripMenuItem();
            tlsmiCancelBuy.Name = "tlsmiCancelBuy";
            tlsmiCancelBuy.Text = "All Buy";

            tlsmiCancelSell = new ToolStripMenuItem();
            tlsmiCancelSell.Name = "tlsmiCancelSell";
            tlsmiCancelSell.Text = "All Sell";

            items.DropDownItems.AddRange(new ToolStripItem[] { tlsmiCancelSelected, tlsmiCancelAll, tlsmiCancelBuy, tlsmiCancelSell });
            dgvSpdOrderBook.cmsColumn.Items.Add(items);

            tlsmiCancelAll.Click += new EventHandler(tlsmiCancelAll_Click);
            tlsmiCancelBuy.Click += new EventHandler(tlsmiCancelBuy_Click);
            tlsmiCancelSell.Click += new EventHandler(tlsmiCancelSell_Click);
            tlsmiCancelSelected.Click += new EventHandler(tlsmiCancelSelected_Click);
            tlsmiModifySelected.Click += new EventHandler(tlsmiModifySelected_Click);

            dgvSpdOrderBook.DataError += dgvSpdOrderBook_DataError;
        }

        void dgvSpdOrderBook_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        void FrmSpdOrderBook_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                    Close();
                bool Ismodifypanelvisible;
                dgvSpdOrderBook.Focus();
                dgvSpdOrderBook.Select();

                //    AppGlobal.OrderLogger.ShowToErrorMessageLog("Manual Trading Not Allowed.");


            }
            catch (Exception ex)
            {
                // AppGlobal.OrderLogger.WriteToLogFile(AtsMethods.GetErrorMessage(ex, "OrderBook_KeyDown"));
            }

        }

        void FrmSpdOrderBook_Load(object sender, EventArgs e)
        {
            dvSpdOrderBook = new DataView(CommonData.dtSpdOrderBook);
            dgvSpdOrderBook.BindSourceView = dvSpdOrderBook;
            dgvSpdOrderBook.UniqueName = "SpdOB";
            dgvSpdOrderBook.LoadSaveSettings();

            ((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[SpreadContract.Symbol1]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[SpreadContract.FillNumber]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[WatchConst.FullName]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[WatchConst.InstrumentName]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[WatchConst.ReasonCode]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[WatchConst.Status]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[WatchConst.Symbol]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[WatchConst.TokenNo]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[WatchConst.TransactionCode]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[WatchConst.filler]).FilteringEnabled = true;

            //((DataGridViewAutoFilterTextBoxColumn)dgvSpdOrderBook.Columns[SpreadContract.ExpiryDate]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.Series]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.StrikePrice]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.GatewayId]).FilteringEnabled = true;
            // ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.LoginId]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.IntOrderNo]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.InstrumentName]).FilteringEnabled = true;
            //((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.BookType]).FilteringEnabled = true;


            // dgvOrderBook.Columns[WatchConst.UniqueId].Visible = false;
            // dgvOrderBook.Sort(dgvOrderBook.Columns[WatchConst.IntOrderNo], System.ComponentModel.ListSortDirection.Descending);

            // string _filter = string.Empty;

            //if (!string.IsNullOrWhiteSpace(Preference.Instance.OpenOrderBookWith))
            //{
            //    _filter = WatchConst.OrderStatus + "='" + Preference.Instance.OpenOrderBookWith + "' ";
            //}
            /*
            * 
            GlobalFunctions.SetColumnDataFormat(ref dgvOrderBook);

          if (AppGlobal.frmMain.dockMain.ActiveContent != null
               && AppGlobal.frmMain.dockMain.ActiveContent == AppGlobal.frmMarketWatch
               && AppGlobal.frmMarketWatch != null
               && AppGlobal.frmMarketWatch.dgvMarketWatch.Rows.Count > 0
               && AppGlobal.frmMarketWatch.dgvMarketWatch.SelectedRows.Count > 0
               && AppGlobal.MarketWatch != null)
          {
              int rowIndex = AppGlobal.frmMarketWatch.dgvMarketWatch.SelectedRows[0].Index;
              if (AppGlobal.MarketWatch[rowIndex] != null)
              {
                  if (!string.IsNullOrWhiteSpace(_filter))
                      _filter += "AND ";

                  _filter += WatchConst.TokenNo + "='" + AppGlobal.MarketWatch[rowIndex].ContractInfo.TokenNo.Trim() + "' " +
                       " AND " + WatchConst.GatewayId + "='" + Convert.ToString(AppGlobal.MarketWatch[rowIndex].GatewayId) + "' " +
                       " AND " + WatchConst.Exchange + "='" + AppGlobal.MarketWatch[rowIndex].ContractInfo.Exchange.Trim() + "' ";

                  Text = "OrderBook : ";
                  Text += AppGlobal.MarketWatch[rowIndex].GatewayId;
                  Text += " " + AppGlobal.MarketWatch[rowIndex].ContractInfo.Symbol.Trim();
                  if (AppGlobal.MarketWatch[rowIndex].ContractInfo.ExpiryDate > 0)
                      Text += " " + AtsMethods.SecondsToDateTime(AppGlobal.MarketWatch[rowIndex].ContractInfo.ExpiryDate).ToString(AtsConst.DateFormatGrid);
              }
          }

          if (!String.IsNullOrEmpty(_filter))
          {
              //ComData.dtSpdOrderBook.DefaultView.RowFilter = _filter;
              dvOrderBook.RowFilter = _filter;
          }
         */

        }

        void FrmSpdOrderBook_FormClosed(object sender, FormClosedEventArgs e)
        {
            CommonData.frmSpdOrderBook = null;

            //CommonData.DisposeApp();
        }

        //void OrderpanelSetting(FrmOrderEntry orderpanel)
        //{
        //    if (dgvOrderBook.SelectedRows.Count == 1)
        //    {
        //        int index = dgvOrderBook.SelectedRows[0].Index;
        //        if (Convert.ToString(dgvOrderBook[WatchConst.BuySell, index].Value) == AtsEnums.BuySell.BUY.ToString())
        //            orderpanel.DefaultKey = Keys.F1;
        //        else
        //            orderpanel.DefaultKey = Keys.F2;

        //        if (Convert.ToString(dgvOrderBook[WatchConst.OrderStatus, index].Value) == AtsEnums.OrderStatus.EPending.ToString())
        //        {
        //            orderpanel.IsModifyPanelVisible = true;

        //            orderpanel.txtQtyDisclosed.Text = Convert.ToString(dgvOrderBook[WatchConst.QtyDisclosed, index].Value);
        //            orderpanel.txtOrderPrice.Text = Convert.ToString(dgvOrderBook[WatchConst.OrderPrice, index].Value);
        //            orderpanel.txtQty.Text = Convert.ToString(dgvOrderBook[WatchConst.QtyRemaining, index].Value);
        //            orderpanel.txtRemarks.Text = Convert.ToString(dgvOrderBook[WatchConst.UserRemarks, index].Value);
        //            orderpanel.txtTriggerPrice.Text = Convert.ToString(dgvOrderBook[WatchConst.TriggerPrice, index].Value);
        //            orderpanel.cmbBookType.Text = Convert.ToString(dgvOrderBook[WatchConst.BookType, index].FormattedValue);
        //            orderpanel.cmbValidity.Text = Convert.ToString(dgvOrderBook[WatchConst.ValidityType, index].FormattedValue);
        //            orderpanel.txtMarketProtection.Text = Convert.ToString(dgvOrderBook[WatchConst.MarketProtection, index].FormattedValue);
        //            if (orderpanel.cmbValidity.Text == Convert.ToString(AtsEnums.Validity.GTD))
        //                orderpanel.dtpGTD.Value = Convert.ToDateTime(dgvOrderBook[WatchConst.GtdTime, index].FormattedValue);
        //            else
        //                orderpanel.dtpGTD.Value = DateTime.Now;

        //            if (string.IsNullOrEmpty(Convert.ToString(dgvOrderBook[WatchConst.ClientCode, index].FormattedValue)))
        //                orderpanel.cmbAccount.Text = Convert.ToString(AtsEnums.AccountType.PRO);
        //            else
        //                orderpanel.cmbAccount.Text = Convert.ToString(dgvOrderBook[WatchConst.ClientCode, index].FormattedValue);

        //            if (orderpanel.cmbBookType.Text == Convert.ToString(AtsEnums.BookType.RL))
        //                orderpanel.cmbBookType.Enabled = false;
        //            else
        //                orderpanel.cmbBookType.Enabled = true;

        //            orderpanel.cmbAccount.Enabled = false;
        //        }
        //        else
        //        {
        //            orderpanel.IsModifyPanelVisible = false;
        //        }
        //    }
        //}

        void CreateOrderInfo()
        {/*
            try
            {
                if (dgvOrderBook.SelectedRows.Count == 1)
                {
                    int index = dgvOrderBook.SelectedRows[0].Index;
                    string gateway = Convert.ToString(dgvOrderBook[WatchConst.GatewayId, index].Value);

                    DataRow[] dr = ComData.DsContract.Tables[gateway].Select(WatchConst.TokenNo + " = '" + Convert.ToString(dgvOrderBook[WatchConst.TokenNo, index].Value) + "' ");

                    if (dr.Length > 0)
                    {
                        #region ContractDetails
                        cond = new ContractDetails();
                        if (dr[0][WatchConst.ClosePrice] != DBNull.Value)
                            cond.ClosePrice = Convert.ToDecimal(dr[0][WatchConst.ClosePrice]);

                        if (dr[0][WatchConst.DprHigh] != DBNull.Value)
                            cond.DprHigh = Convert.ToDecimal(dr[0][WatchConst.DprHigh]);

                        if (dr[0][WatchConst.DprLow] != DBNull.Value)
                            cond.DprLow = Convert.ToDecimal(dr[0][WatchConst.DprLow]);

                        if (dr[0][WatchConst.LotSize] != DBNull.Value)
                            cond.LotSize = Convert.ToInt32(dr[0][WatchConst.LotSize]);

                        if (dr[0][WatchConst.MaxSingleTransactionQty] != DBNull.Value)
                            cond.MaxQty = Convert.ToInt32(dr[0][WatchConst.MaxSingleTransactionQty]);

                        if (dr[0][WatchConst.MaxSingleTransactionValue] != DBNull.Value)
                            cond.MaxValue = Convert.ToDecimal(dr[0][WatchConst.MaxSingleTransactionValue]);

                        string PriceFormat;
                        AtsEnums.GatewayId curr = (AtsEnums.GatewayId)((uint)AtsEnums.GatewayGroup.CURRENCY);
                        AtsEnums.GatewayId selected = AtsMethods.StringToEnum<AtsEnums.GatewayId>(gateway);
                        if ((curr & selected) == selected)
                            PriceFormat = AtsConst.PriceFormatN4;
                        else
                            PriceFormat = AtsConst.PriceFormatN2;

                        string custF = PriceFormat == AtsConst.PriceFormatN2 ? "0.00" : "0.0000";

                        cond.PriceFormat = custF;
                        if (dr[0][WatchConst.PriceTick] != DBNull.Value)
                            cond.PriceTick = Convert.ToDecimal(dr[0][WatchConst.PriceTick]);
                        #endregion

                        #region ContractInformation
                        ContractInfo = new ContractInformation();
                        if (dr[0][WatchConst.Exchange] != DBNull.Value)
                            ContractInfo.Exchange = Convert.ToString(dr[0][WatchConst.Exchange]);

                        if (dr[0][WatchConst.InstrumentName] != DBNull.Value)
                            ContractInfo.InstrumentName = Convert.ToString(dr[0][WatchConst.InstrumentName]);

                        if (dr[0][WatchConst.Symbol] != DBNull.Value)
                            ContractInfo.Symbol = Convert.ToString(dr[0][WatchConst.Symbol]);

                        if (dr[0][WatchConst.TokenNo] != DBNull.Value)
                            ContractInfo.TokenNo = Convert.ToString(dr[0][WatchConst.TokenNo]);

                        if (dr[0][WatchConst.ExpiryDate] != DBNull.Value)
                            ContractInfo.ExpiryDate = AtsMethods.DateTimeToSecond(Convert.ToDateTime(dr[0][WatchConst.ExpiryDate]));

                        if (dr[0][WatchConst.PriceDivisor] != DBNull.Value)
                            ContractInfo.PriceDivisor = Convert.ToInt32(dr[0][WatchConst.PriceDivisor]);

                        if (dr[0][WatchConst.Multiplier] != DBNull.Value)
                            ContractInfo.Multiplier = Convert.ToDecimal(dr[0][WatchConst.Multiplier]);

                        if (dr[0][WatchConst.Series] != DBNull.Value)
                            ContractInfo.Series = Convert.ToString(dr[0][WatchConst.Series]);

                        if (dr[0][WatchConst.StrikePrice] != DBNull.Value)
                            ContractInfo.StrikePrice = Convert.ToInt32(Convert.ToDecimal(dr[0][WatchConst.StrikePrice]) * ContractInfo.PriceDivisor);
                        #endregion

                        orderprice = string.Empty;
                        if (dgvOrderBook.Rows[index].Cells[WatchConst.OrderPrice].Value != DBNull.Value)
                            orderprice = Convert.ToDecimal(dgvOrderBook.Rows[index].Cells[WatchConst.OrderPrice].Value).ToString();
                    }
                    else
                    {
                        cond = null;
                        ContractInfo = new ContractInformation();
                        AppGlobal.Logger.ShowToMessageLog("Contract Not Found");
                    }

                }

            }
            catch (Exception ex)
            {
                AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }*/
        }
        //            dr[SpreadContract.OrderId] 
        //            dr[SpreadContract.Token1]
        //            dr[SpreadContract.Token2]
        //            // dr[SpreadContract.BuySell1]
        //            //   dr[SpreadContract.BuySell2]
        //            // dr[SpreadContract.AccountNumber]
        //            dr[SpreadContract.Price1]
        //            dr[SpreadContract.Price2]

        //            dr[SpreadContract.BidQ]
        //            dr[SpreadContract.AskQ]

        //            dr[SpreadContract.BidQ_leg2]
        //            dr[SpreadContract.AskQ_leg2]

        //            dr[SpreadContract.Status] 
        //            dr[SpreadContract.TransactionCode] 
        //            dr[SpreadContract.BuySell2]
        //            dr[SpreadContract.BuySell1]
        //            dr[SpreadContract.Symbol1]
        //            dr[SpreadContract.Symbol]
        //            dr[SpreadContract.ExpiryDate1]
        //            dr[SpreadContract.Price_Diff]

        C_Spread_lot_In obj;
        void tlsmiModifySelected_Click(object sender, EventArgs e)
        {
            obj = new C_Spread_lot_In();
            try
            {
                int Price1, Volume1;
                foreach (DataGridViewRow row in dgvSpdOrderBook.SelectedRows)
                {
                    //compare here with the order stat and place cancel ok sir..
                    if (Convert.ToString(row.Cells[WatchConst.Status].Value) == AtsEnums.OrderStatus.EPending.ToString() || Convert.ToString(row.Cells[WatchConst.Status].Value) == AtsEnums.OrderStatus.MPending.ToString())
                    {

                        using (var frmord = new frmSpreadOrdEntry())
                        {
                            // frmord.lblSpdOrderMsg.Text = "BUY " + Dr.Cells[SpreadContract.Symbol1].Value + Dr.Cells[SpreadContract.ExpiryDate1].Value + Dr.Cells[SpreadContract.ExpiryDate1].Value;
                            //frmord.lblSpdOrderMsg.BackColor = Color.Green;
                            //frmord.BackColor = Color.SeaGreen;

                            //     if (row.Cells[SpreadContract.Price_Diff].Value.ToString())
                            frmord.cmbInstName1.Text = System.Text.ASCIIEncoding.ASCII.GetString(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value))].mS_SPD_OE_REQUEST.ms_oe_obj.InstrumentName);
                            frmord.cmbSymbol1.Text = System.Text.ASCIIEncoding.ASCII.GetString(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value))].mS_SPD_OE_REQUEST.ms_oe_obj.Symbol);
                            frmord.cmbOpType1.Text = System.Text.ASCIIEncoding.ASCII.GetString(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value))].mS_SPD_OE_REQUEST.ms_oe_obj.OptionType);
                            frmord.cmbStrikePrice1.Text = Convert.ToString(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value))].mS_SPD_OE_REQUEST.ms_oe_obj.StrikePrice);
                            frmord.cmbBuySell1.Text = IPAddress.HostToNetworkOrder(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value))].mS_SPD_OE_REQUEST.BuySell1) == 1 ? "BUY" : "Sell";
                            //  frmord.cmbExpiry1 = Convert.ToString(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId]))].mS_SPD_OE_REQUEST.ms_oe_obj.ExpiryDate);

                            frmord.cmbInstName2.Text = System.Text.ASCIIEncoding.ASCII.GetString(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value))].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.InstrumentName);
                            frmord.cmbSymbol2.Text = System.Text.ASCIIEncoding.ASCII.GetString(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value))].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.Symbol);
                            frmord.cmbOpType2.Text = System.Text.ASCIIEncoding.ASCII.GetString(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value))].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.OptionType);
                            frmord.cmbStrikePrice2.Text = Convert.ToString(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value))].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.StrikePrice);
                            frmord.cmbBuySell2.Text = IPAddress.HostToNetworkOrder(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value))].mS_SPD_OE_REQUEST.leg2.BuySell2) == 1 ? "BUY" : "Sell";
                            //  frmord.cmbExpiry1 = Convert.ToString(Holder.holderOrder[Convert.ToDouble(Convert.ToInt64(row.Cells[SpreadContract.OrderId]))].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.ExpiryDate);
                            
                            frmord.txtPrice1.Text = row.Cells[SpreadContract.Price_Diff].Value.ToString();
                            if (frmord.cmbBuySell1.Text == "Buy")
                            {
                               
                               frmord.txtTotalQty1.Text =Convert.ToString(Convert.ToInt32(row.Cells[SpreadContract.BidQ].Value) / Global.Instance.BoardLotDict[Convert.ToInt32(row.Cells[SpreadContract.Token1].Value)]);
                            }
                            else
                            {
                                frmord.txtTotalQty1.Text = Convert.ToString(Convert.ToInt32(row.Cells[SpreadContract.AskQ].Value) / Global.Instance.BoardLotDict[Convert.ToInt32(row.Cells[SpreadContract.Token1].Value)]);
                            }
                            // frmord.DesktopLocation = new Point(100, 100);
                            int x = (Screen.PrimaryScreen.WorkingArea.Width - frmord.Width) / 2;
                            int y = (Screen.PrimaryScreen.WorkingArea.Height - frmord.Height) - 50;
                            frmord.Location = new Point(x, y);

                            // var v = Convert.ToInt32(Dr.Cells[SpreadContract.ExpiryDate1].Value);
                            if (frmord.ShowDialog(this) == DialogResult.OK)
                            {

                                if (frmord.FormSpdDialogResult == (int)OrderEntryButtonCase.SUBMIT)
                                {
                                    if (MessageBox.Show(this, "Are you sure you Want to Place Buy Order?",
                                                      Application.ProductName, MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question) == DialogResult.Yes)
                                    {



                                        Price1 = Convert.ToInt32(Convert.ToSingle(frmord.txtPrice1.Text) * 100);
                                        Volume1 = Convert.ToInt32(frmord.txtTotalQty1.Text) * (Global.Instance.BoardLotDict[Convert.ToInt32(row.Cells[SpreadContract.Token1].Value)]);
                                        //pSpreadOrd.Volume2 = Convert.ToInt32(frmord.txtTotalQty2.Text) * Convert.ToInt32(Dr.Cells[SpreadContract.BoardLotQuantity1].Value);



                                        // NNFInOut.Instance.SP_BOARD_LOT_IN(pSpreadOrd);
                                        NNFInOut.Instance.SP_ORDER_MOD_IN(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value), Price1, Volume1);
                                    }

                                    //NNFInOut.Instance.BOARD_LOT_IN_TR(Convert.ToInt32(Dr.Cells["UniqueIdentifier"].Value),
                                    //      Dr.Cells["InstrumentName"].Value.ToString(),
                                    //     Dr.Cells["Symbol"].Value.ToString(),
                                    //     Convert.ToInt32(Dr.Cells["ExpiryDate"].Value),
                                    //      Convert.ToInt32(Dr.Cells["StrikePrice"].Value),
                                    //      Dr.Cells["OptionType"].Value.ToString(),
                                    //      1,
                                    //      frmord.LEG_SIZE * Convert.ToInt32(Dr.Cells["Lotsize"].Value),
                                    //        Convert.ToInt32(frmord.LEG_PRICE * 100));

                                }
                            }
                            // OrderProcess.CancelOrder(Convert.ToUInt32(row.Cells[WatchConst.IntOrderNo].Value), Convert.ToUInt16(row.Cells[WatchConst.UniqueId].Value));

                        }
                    }

                    else
                    {
                        AppGlobal.Logger.ShowToMessageLog("This Order Can not be Cancelled.", color: "Red");
                    }
                }
            }

            catch (Exception ex)
            {
                // AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }


            /*  if (dgvOrderBook.SelectedRows.Count == 1)
              {
                  int rowindex = dgvOrderBook.SelectedRows[0].Index;
                  Keys keycode = Keys.None;
                  if (Convert.ToString(dgvOrderBook[WatchConst.OrderStatus, rowindex].Value) == AtsEnums.OrderStatus.EPending.ToString() &&
                      Convert.ToString(dgvOrderBook[WatchConst.BuySell, rowindex].Value) == AtsEnums.BuySell.SELL.ToString())
                      keycode = Keys.F2;
                  if (Convert.ToString(dgvOrderBook[WatchConst.OrderStatus, rowindex].Value) == AtsEnums.OrderStatus.EPending.ToString() &&
                      Convert.ToString(dgvOrderBook[WatchConst.BuySell, rowindex].Value) == AtsEnums.BuySell.BUY.ToString())
                      keycode = Keys.F1;
                  if (keycode != Keys.None)
                  {
                      CreateOrderInfo();

                      bool Ismodifypanelvisible;

                      if (Convert.ToString(dgvOrderBook[WatchConst.OrderStatus, rowindex].Value) == AtsEnums.OrderStatus.EPending.ToString())
                          Ismodifypanelvisible = true;
                      else
                          Ismodifypanelvisible = false;

                      if (!string.IsNullOrEmpty(ContractInfo.TokenNo) && cond != null)
                      {
                          // if (ComData.StrategyCollection.Keys.Contains(AtsEnums.StrategyType.Manual))
                          {
                              FrmOrderEntry frmOrder = new FrmOrderEntry(keycode, ContractInfo, cond,
                                                 Convert.ToUInt32(AtsMethods.StringToEnum<AtsEnums.GatewayId>(dgvOrderBook[WatchConst.GatewayId, rowindex].Value.ToString())), orderprice, dgvOrderBook.SelectedRows);
                              frmOrder.ShowInTaskbar = false;
                              frmOrder.IsModifyPanelVisible = Ismodifypanelvisible;
                              OrderpanelSetting(frmOrder);
                              FormSetup.SetupForm(frmOrder);
                              frmOrder.ShowDialog(AppGlobal.frmMain);

                              dgvOrderBook.Focus();
                              dgvOrderBook.Select();
                          }
                          //else
                          //{
                          //    AppGlobal.OrderLogger.ShowToErrorMessageLog("Manual Trading Not Allowed.");
                          //}
                      }
                  }
                  else
                      AppGlobal.Logger.ShowToErrorMessageLog("You can not modify this order.");
              }*/
        }

        void tlsmiCancelSelected_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvSpdOrderBook.SelectedRows)
                {
                    //compare here with the order stat and place cancel ok sir..
                    if (Convert.ToString(row.Cells[WatchConst.Status].Value) == AtsEnums.OrderStatus.EPending.ToString())
                    {
                        // OrderProcess.CancelOrder(Convert.ToUInt32(row.Cells[WatchConst.IntOrderNo].Value), Convert.ToUInt16(row.Cells[WatchConst.UniqueId].Value));
                        NNFInOut.Instance.SP_ORDER_CANCEL_IN(Convert.ToInt64(row.Cells[SpreadContract.OrderId].Value));
                    }
                    else
                    {
                        AppGlobal.Logger.ShowToMessageLog("This Order Can not be Cancelled.", color: "Red");
                    }
                }
            }
            catch (Exception ex)
            {
                // AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
            }
        }

        void tlsmiCancelSell_Click(object sender, EventArgs e)
        {
            //  OrderProcess.CancelAllOrder((byte)AtsEnums.BuySell.SELL);
        }

        void tlsmiCancelBuy_Click(object sender, EventArgs e)
        {
            //  OrderProcess.CancelAllOrder((byte)AtsEnums.BuySell.BUY);
        }

        void tlsmiCancelAll_Click(object sender, EventArgs e)
        {
            /*
                        if (Preference.Instance.ConfirmOnCancelAll)
                        {
                            if (MessageBox.Show(AppGlobal.frmOrderBook, "Are You sure want to cancel all Order?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                  == DialogResult.No) return;
                        }
                        OrderProcess.CancelAllOrder();

                    }*/


        }
       
        private void FrmOrderBook_Load_1(object sender, EventArgs e)
        {
            var AbbA = LoadFormLocationAndSize(this);
            this.Location = new Point(AbbA[0], AbbA[1]);
            this.Size = new Size(AbbA[2], AbbA[3]);

            this.FormClosing += new FormClosingEventHandler(SaveFormLocationAndSize);
          
        }

        private void FrmSpdOrderBook_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }

        private void dgvSpdOrderBook_DataError_1(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}

