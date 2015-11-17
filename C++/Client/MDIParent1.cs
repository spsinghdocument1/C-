using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Structure;
using System.IO;
using System.Net;
using System.Data.OleDb;
using AMS.Profile;
using Client.Spread;
using System.Reflection;
using Client.LZO_NanoData;

namespace Client
{
    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 0;
        Fo_Fo_mktwatch _fofomarket = null;
        frmMktWatch _frmMktWatch = null;
        delegate void OndatastopDelegate(Object o, ReadOnlyEventArgs<string> Stat);
        public MDIParent1()
        {
           
                InitializeComponent();
                this.WindowState = FormWindowState.Maximized;
               NNFHandler.eOrderORDER_CONFIRMATION_TR += new NNFHandler.RaiseEventDelegate(frmGenOrderBook.Instance.ORDER_CONFIRMATION_TR);
                NNFHandler.eOrderBATCH_ORDER_CANCEL += frmGenOrderBook.Instance.BATCH_ORDER_CANCEL;
                NNFHandler.eOrderFREEZE_TO_CONTROL += frmGenOrderBook.Instance.FREEZE_TO_CONTROL;
                NNFHandler.eOrderORDER_CANCEL_CONFIRM_OUT += frmGenOrderBook.Instance.ORDER_CANCEL_CONFIRM_OUT;
                NNFHandler.eOrderORDER_CANCEL_REJECT_TR += frmGenOrderBook.Instance.ORDER_CANCEL_REJECT_TR;
                NNFHandler.eOrderORDER_CONFIRMATION_OUT += frmGenOrderBook.Instance.ORDER_CONFIRMATION_OUT;

                NNFHandler.eOrderORDER_CXL_CONFIRMATION_TR += frmGenOrderBook.Instance.ORDER_CXL_CONFIRMATION_TR;
                NNFHandler.eOrderORDER_CXL_REJ_OUT += frmGenOrderBook.Instance.ORDER_CXL_REJ_OUT;
                NNFHandler.eOrderORDER_ERROR_OUT += frmGenOrderBook.Instance.ORDER_ERROR_OUT;
                NNFHandler.eOrderORDER_ERROR_TR += frmGenOrderBook.Instance.ORDER_ERROR_TR;

                NNFHandler.eOrderORDER_MOD_CONFIRM_OUT += frmGenOrderBook.Instance.ORDER_MOD_CONFIRM_OUT;
                NNFHandler.eOrderORDER_MOD_CONFIRMATION_TR += frmGenOrderBook.Instance.ORDER_MOD_CONFIRMATION_TR;
                NNFHandler.eOrderORDER_MOD_REJ_OUT += frmGenOrderBook.Instance.ORDER_MOD_REJ_OUT;
                NNFHandler.eOrderORDER_MOD_REJECT_TR += frmGenOrderBook.Instance.ORDER_MOD_REJECT_TR;
                NNFHandler.eOrderPRICE_CONFIRMATION += frmGenOrderBook.Instance.PRICE_CONFIRMATION;
                NNFHandler.eOrderTRADE_CANCEL_OUT += frmGenOrderBook.Instance.TRADE_CANCEL_OUT;
                NNFHandler.eOrderTRADE_CONFIRMATION_TR += frmGenOrderBook.Instance.TRADE_CONFIRMATION_TR;

                NNFHandler.eOrderTRADE_ERROR += frmGenOrderBook.Instance.TRADE_ERROR;

                NNFHandler.eOrderTWOL_ORDER_ERROR += frmGenOrderBook.Instance.TWOL_ORDER_ERROR;
                NNFHandler.eOrderTWOL_ORDER_CXL_CONFIRMATION += frmGenOrderBook.Instance.TWOL_ORDER_CXL_CONFIRMATION;

                NNFHandler.eOrderTWOL_ORDER_CONFIRMATION += frmGenOrderBook.Instance.TWOL_ORDER_CONFIRMATION;

                NNFHandler.eSpreadOrderConfirmation += SpdOrderTableMethods.SP_ORDER_CONFIRMATION;
                NNFHandler.eSpreadOrderCancelConfirmation += SpdOrderTableMethods.SP_ORDER_CANCEL_CONFIRMATION;
                NNFHandler.eSpreadOrderTradeonfirmation += SpdOrderTableMethods.SP_ORDER_TRD_CONFIRMATION;
                NNFHandler.eSpreadOrderTradeonfirmationconfirmation += SpdOrderTableMethods.TRADE_CONFIRMATION_TR;
                NNFHandler.eSpreadOrderModconfirmation += SpdOrderTableMethods.SP_ORDER_Mod_CONFIRMATION;
              //  Trade_Tracker.Instance.MdiParent = this;
                frmGenOrderBook.Instance.MdiParent = this;
                frmTradeBook.Instance.MdiParent = this;
                frmNetBook.Instance.MdiParent = this;

                Global.Instance.Relogin = false;
                Global.Instance.write = false;
           
            
        }

        private void ShowNewForm(object sender, EventArgs e)
        {

            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();

        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
       // System.Timers.Timer timerforchecklogin;
        frmLogin _frmNewLogin;
        private void MDIParent1_Load(object sender, EventArgs e)
        {
            Thread myth = new System.Threading.Thread(delegate()
            {
                csv.CSV_Class.contract_fun();
            });
            myth.Start();
            Global.Instance.warningvar = false;
            Global.Instance.ReloginFarmloader = false;
            Global.Instance.SignInStatus = false;
            Global.Instance.Pass_bool = false;
            Global.Instance.Fopairbool = false;
            Global.Instance.stop_all = false;
            NNFInOut.Instance.OnDataAPPTYPEStatusChange += OnDataAPPTYPE;
            NNFInOut.Instance.OnStatusChange += Instance_OnStatusChange;
            NNFHandler.Instance.OnStatusChangeHeartBeatInfo += ChangeHeartBeatInfo;
            UDP_Reciever.Instance.OnDataStatusChange += Instance_OndatastopChange;
            NNFHandler.Instance.OnStatusChange += Instance_OnStatusChange;

           
            
            
            NNFHandler.Instance._socketfun();
            NNFHandler.Instance.RecieveDataAsClient();
            LzoNanoData.Instance.UDPReciever();
            _frmNewLogin = new frmLogin();
            _frmNewLogin.ShowDialog();

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
           timer.Tick += new System.EventHandler(timer_Tick);
            timer.Start();

            Client.Spread.CommonData.LoadSymbols("spd_contract.txt", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + Path.DirectorySeparatorChar + "ContractFile" + Path.DirectorySeparatorChar, Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + Path.DirectorySeparatorChar + "Settings" + Path.DirectorySeparatorChar + "spd.ini");
            //int c = 1;
            //while (Global.Instance.Pass_bool != false)
            //{

            //    _frmNewLogin.Hide();
               
            //    //_frmNewLogin.Dispose();
            //    //_frmNewLogin = new frmLogin();
            //    _frmNewLogin.ShowDialog();
            //    c++;
            //    if (c==3)
            //    {
            //        logout();
            //        break;
            //    }
            //}
            
            if (Global.Instance.SignInStatus == false)
            {
                MessageBox.Show("SignInStatus  :" + Global.Instance.SignInStatus);
                this.Hide();
                logout();
            }
          
                
              
            
        }
       
        private void timer_Tick(object sender, System.EventArgs e)
        {
            this.Text = Global.Instance.C_Type + "   ->:" + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        }

        void UpdateLabel(ReadOnlyEventArgs<HeartBeatInfo> e)
        {
           // this.lblClientQuee.Text =this.lblClientQuee.Name.ToString()+"=:"+ e.Parameter.ClientQueue.ToString();
              this.lblClientQueueCount.Text = e.Parameter.ClientQueue.ToString();
             this.lblServerqueueCount.Text = e.Parameter.tapQueue.ToString();
         //  this.lblTapStatus.Text = e.Parameter.tapStatus.ToString();
            
            if (e.Parameter.dataStatus == true)
            {
                this.lblDatastatus.BackColor = Color.Green;
            }
            else
            {
                this.lblDatastatus.BackColor = Color.Red;
                lblLastrecoverytimeonserver.Text = DateTime.Now.ToString();
           //   lblLastrecoverytimeonserver.Text
            }
            if (e.Parameter.tapStatus == true)
            {
                this.lblTapStatus.BackColor = Color.Green;
            }
            else
            {
                this.lblTapStatus.BackColor = Color.Red;
            }
        }

        void ChangeHeartBeatInfo(object sender, ReadOnlyEventArgs<HeartBeatInfo> e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((ThreadStart)delegate() { UpdateLabel(e); });

                return;
            }
            else
            {
                UpdateLabel(e);
            }
        }
        void Instance_OndatastopChange(object sender, ReadOnlyEventArgs<string> e)
        {
            try
            {
                switch (e.Parameter)
                {

                    case "STOP":
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new OndatastopDelegate(Instance_OndatastopChange), sender, new ReadOnlyEventArgs<string>(e.Parameter));
                            return;
                        }

                        this.lblData.BackColor = Color.Red;
                        break;
                    case "START":
                        if (this.InvokeRequired)
                        {

                            this.Invoke(new OndatastopDelegate(Instance_OndatastopChange), sender, new ReadOnlyEventArgs<string>(e.Parameter));
                            return;
                        }

                        this.lblData.BackColor = Color.Green;
                        break;
                }

            }
            catch (Exception ex)
            {

            }
            }
        

        
        void OnDataAPPTYPE(Object o, ReadOnlyEventArgs<string>str)
        {
          Global.Instance.C_Type = str.Parameter;
                this.Text = str.Parameter;
            
            if (this.Text == "FOFO")
            {
                toolsMenu.DropDownItems[3].Visible = false;
                
            }

            else if (this.Text == "TWOLEGOPT")
            {
                toolsMenu.DropDownItems[2].Visible = false;
            }
            Global.Instance.C_Type += " : " + " 1.9 A " + Global.Instance.ClientId;

            this.Text = Global.Instance.C_Type;
          
        }

        delegate void OnStatusChangeDelegate(Object o, ReadOnlyEventArgs<SYSTEMSTATUS> Stat);

        Re_Login _frmLogin;
        void Instance_OnStatusChange(object sender, ReadOnlyEventArgs<SYSTEMSTATUS> e)
        {

            switch (e.Parameter)
            { 
            
                case SYSTEMSTATUS.LOGGEDIN:

                   
                   // UDP_Reciever.Instance.UDPReciever(Global.Instance.LanIp, Global.Instance.McastIp,Convert.ToInt32( Global.Instance.Mcastport));
                   //   UDP_Reciever.Instance.UDPReciever(Global.Instance.LanIp, Global.Instance.McastIp, Convert.ToInt32(Global.Instance.Mcastport));  //LZO
                    if (Global.Instance.Relogin == false)
                    {
                    //    UDP_Reciever.Instance.UDPReciever(Global.Instance.LanIp, Global.Instance.McastIp, Convert.ToInt32(Global.Instance.Mcastport));  //LZO   
                    UDP_Reciever.Instance.UDPReciever(); 
                    }//DATASERVER
 
                   UDP_Reciever.Instance.OnStatusChange += Instance_OnStatusChange;

                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((ThreadStart)delegate() {
                            if (Global.Instance.ReloginFarmloader != true)
                            {
                                Loadchildform();

                               
                            }
                            this.lblOrder.BackColor = Color.Green;
                            
                        });

                        return;
                    }
                    else
                    {
                        if (Global.Instance.ReloginFarmloader != true)
                        {
                            Loadchildform();

                           
                        }
                        this.lblOrder.BackColor = Color.Green;

                        
                    }

                    break;
                    

                case SYSTEMSTATUS.LOGGEDOUT:
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new OnStatusChangeDelegate(Instance_OnStatusChange), sender, new ReadOnlyEventArgs<SYSTEMSTATUS>(e.Parameter));
                        return;
                    }

                    this.lblOrder.BackColor = Color.Red;
                    this.lblTapStatus.BackColor = Color.Red;
                    this.lblDatastatus.BackColor = Color.Red;

                    if (_frmLogin == null)
                    { 
                    using (_frmLogin = new Re_Login())
                    {

                      //  _frmLogin = new Re_Login();
                        _frmLogin.TopMost = true;
                        _frmLogin.ShowDialog();
                        _frmLogin = null;
                    }
                                           
                    
                    }

                    break;
                    

                case SYSTEMSTATUS.DATARUNNING:
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new OnStatusChangeDelegate(Instance_OnStatusChange), sender, new ReadOnlyEventArgs<SYSTEMSTATUS>(e.Parameter));
                        return;
                    }

                    this.lblData.BackColor = Color.Green;

                    break;
                   


                case SYSTEMSTATUS.DATASTOPPED:
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new OnStatusChangeDelegate(Instance_OnStatusChange), sender, new ReadOnlyEventArgs<SYSTEMSTATUS>(e.Parameter));
                        return;
                    }

                    this.lblData.BackColor = Color.Red;
                    break;

                case SYSTEMSTATUS.NONE:


                    if (this.InvokeRequired)
                    {
                        this.Invoke(new OnStatusChangeDelegate(Instance_OnStatusChange), sender, new ReadOnlyEventArgs<SYSTEMSTATUS>(e.Parameter));
                        return;
                    }

                    this.lblStatus.Text = "Some unknown event occured..";

                    break;
                case SYSTEMSTATUS.PASSERROR:


                    if (this.InvokeRequired)
                    {
                        this.Invoke(new OnStatusChangeDelegate(Instance_OnStatusChange), sender, new ReadOnlyEventArgs<SYSTEMSTATUS>(e.Parameter));
                        return;
                    }

                    this.lblStatus.Text = "Password error";

                    this.lblOrder.BackColor = Color.Red;



                    break;
                case SYSTEMSTATUS.PASSEXPIRE:


                    if (this.InvokeRequired)
                    {
                        this.Invoke(new OnStatusChangeDelegate(Instance_OnStatusChange), sender, new ReadOnlyEventArgs<SYSTEMSTATUS>(e.Parameter));
                        return;
                    }

                    this.lblStatus.Text = "Password expired";

                    this.lblOrder.BackColor = Color.Red;



                    break;
            }

        }
        private void loadbackfill_data()
        {
            DataSet ds = new DataSet();
        
            ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "001.xml");

            ds.Tables[0].Columns["INSTRUMENT"].ColumnName = "InstrumentName"; // "InstrumentName";
            ds.Tables[0].Columns["SYMBOL"].ColumnName = "Symbol";
           
            ds.Tables[0].Columns["TOKENNO"].ColumnName = "TokenNo";
          //  ds.Tables[0].Columns["TOKENNO"].DataType = typeof(Int32);
            ds.Tables[0].Columns["Buy_SellIndicator"].ColumnName = "Buy_SellIndicator";
            ds.Tables[0].Columns["OPTIONTYPE"].ColumnName = "OptionType";
            ds.Tables[0].Columns["STRIKEPRICE"].ColumnName = "StrikePrice";
         //   ds.Tables[0].Columns["PRICE"].DataType = typeof(double);
            ds.Tables[0].Columns["PRICE"].ColumnName = "Price";
            //ds.Tables[0].Columns["INSTRUMENT"].ColumnName = "FillPrice";
            ds.Tables[0].Columns["VOLUME"].ColumnName = "Volume";
            ds.Tables[0].Columns["STATUS"].ColumnName = "Status";
            ds.Tables[0].Columns["ACCOUNTNUMBER"].ColumnName = "AccountNumber"; 
            ds.Tables[0].Columns["BOOKTYPE"].ColumnName = "BookType";
            ds.Tables[0].Columns["BRANCHID"].ColumnName = "BranchId";
            ds.Tables[0].Columns["CLOSEOUTFLAG"].ColumnName = "CloseoutFlag";
            ds.Tables[0].Columns["EXPIRYDATE"].ColumnName = "ExpiryDate";
            ds.Tables[0].Columns["DISCLOSEDVOLUME"].ColumnName = "DisclosedVolume";
            ds.Tables[0].Columns["DISCLOSEDVOLUMEREMAINING"].ColumnName = "DisclosedVolumeRemaining";
            ds.Tables[0].Columns["ENTRYDATETIME"].ColumnName = "EntryDateTime";
            ds.Tables[0].Columns["FILLER"].ColumnName = "filler";
            ds.Tables[0].Columns["GOODTILLDATE"].ColumnName = "GoodTillDate";
            ds.Tables[0].Columns["LASTMODIFIED"].ColumnName = "LastModified";
            ds.Tables[0].Columns["LOGTIME"].ColumnName = "LogTime";
            ds.Tables[0].Columns["Modified_CancelledBy"].ColumnName = "Modified_CancelledBy";
            ds.Tables[0].Columns["NNFFIELD"].ColumnName = "nnffield";
            ds.Tables[0].Columns["OPEN_CLOSE"].ColumnName = "Open_Close";
            ds.Tables[0].Columns["OrderNumber"].ColumnName = "OrderNumber";
            //  ds.Tables[0].Columns["INSTRUMENT"].ColumnName = "RejectReason";
            ds.Tables[0].Columns["PRO_CLIENTINDICATOR"].ColumnName = "Pro_ClientIndicator";
            ds.Tables[0].Columns["REASONCODE"].ColumnName = "ReasonCode";
            ds.Tables[0].Columns["SETTLOR"].ColumnName = "Settlor";
            // ds.Tables[0].Columns["TIMESTAMP1"].ColumnName = "TimeStamp1";
            //ds.Tables[0].Columns["INSTRUMENT"].ColumnName = "TimeStamp2";
            ds.Tables[0].Columns["TOTALVOLUMEREMAINING"].ColumnName = "TotalVolumeRemaining";
            ds.Tables[0].Columns["TRADERID"].ColumnName = "TraderId";
            ds.Tables[0].Columns["TRANSACTIONCODE"].ColumnName = "TransactionCode";
            ds.Tables[0].Columns["USERID"].ColumnName = "UserId";
          //  ds.Tables[0].Columns["VOLUMEFILLEDTODAY"].DataType = typeof(Int32);
            ds.Tables[0].Columns["VOLUMEFILLEDTODAY"].ColumnName = "VolumeFilledToday";
            //  ds.Tables[0].Columns["INSTRUMENT"].ColumnName = "TimeStamp1";
            Global.Instance.OrdetTable = ds.Tables[0];
         

            DataRow[] dr_selectbal = Global.Instance.OrdetTable.Select("STATUS='Open'");

          //  double d=
            foreach (var item in dr_selectbal)
            {
                Order ord = new Order(1);
                ord.mS_OE_RESPONSE_TR.AccountNumber = System.Text.Encoding.ASCII.GetBytes(item["AccountNumber"].ToString());
                var a=(Convert.ToInt64( item["OrderNumber"]));
                ord.mS_OE_RESPONSE_TR.OrderNumber =  Convert.ToDouble(Convert.ToInt64( item["OrderNumber"]));//Convert.ToDouble(item["OrderNumber"].ToString());

                var c = (long)LogicClass.DoubleEndianChange(ord.mS_OE_RESPONSE_TR.OrderNumber);
                ord.mS_OE_RESPONSE_TR.Buy_SellIndicator =(short) IPAddress.HostToNetworkOrder(Convert.ToInt32( item["Buy_SellIndicator"] == "BUY"?"1":"2" ));
             //    MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
                Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(ord.mS_OE_RESPONSE_TR.OrderNumber), ord);
                Holder.holderOrder[LogicClass.DoubleEndianChange(ord.mS_OE_RESPONSE_TR.OrderNumber)].mS_OE_RESPONSE_TR = ord.mS_OE_RESPONSE_TR;
            }
             
   
        }

        private void marketWatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_frmMktWatch == null)
            {
                _frmMktWatch = new frmMktWatch();

                // _frmMktWatch.WindowState = FormWindowState.Minimized;
               
                //  UDP_Reciever.Instance.OnDataArrived += _frmMktWatch.OnDataArrived;
            }
            _frmMktWatch.MdiParent = this;
                _frmMktWatch.Show();
            
          

        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {           
            frmGenOrderBook.Instance.Show();
        }

        private void Logs_Click(object sender, EventArgs e)
        {
            frmLog.Instance.MdiParent = this;
            frmLog.Instance.Show();
        }

        private void ErrorLogs_Click(object sender, EventArgs e)
        {
            frmErrorLog.Instance.MdiParent = this;
            frmErrorLog.Instance.Show(); 

        }

        private void foFoMarketWatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fofomarket == null)
            {
                _fofomarket = new Fo_Fo_mktwatch();
                //_fofomarket.Width = this.Width - 100;
                //_fofomarket.Height = this.Height - 300;

                UDP_Reciever.Instance.OnDataArrived += _fofomarket.OnDataArrived;
                NNFHandler.eOrderTRADE_CONFIRMATION_TR += _fofomarket.TRADE_CONFIRMATION_TR;
            }
                _fofomarket.MdiParent = this;
                _fofomarket.Show(); 

        }
         
        private void NetBook_Click(object sender, EventArgs e)
        {
            

         //   frmNetBook.Instance.MdiParent = this;
            frmNetBook.Instance.Show(); 
        }

        private void TradeBook_Click(object sender, EventArgs e)
        {
           
            frmTradeBook.Instance.Show(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Re_Login relogin = new Re_Login();
            relogin.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //ProfileTrade_Book pfbook = new ProfileTrade_Book();
            //pfbook.MdiParent = this;
            //pfbook.Show();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

            Global.Instance.OrdetTable.Clear();
            int c = 0;
            try { 
            OpenFileDialog ob = new OpenFileDialog();
            ob.Filter = "excel files *.xlsx|*.*";

           if(    ob.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            DataSet ds = new DataSet();
            string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ob.FileName + ";Extended Properties=Excel 12.0;";
            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
                   DataTable Sheets = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                  
                foreach (DataRow dr in Sheets.Rows)
                   {
                       c++;
                       string sht = dr[2].ToString().Replace("'", "");
                       OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from [" + sht + "] ", connection);
                       dataAdapter.Fill(Global.Instance.OrdetTable);
                       break;
                   }
            }
           }
            }
               catch(Exception ex)
            {
                   MessageBox.Show("Excel Not Loaded "+ ex.Message);
               }
                //OleDbDataAdapter adop = new OleDbDataAdapter(command);
                //adop.Fill(Global.Instance.OrdetTable);
            //Buy_SellIndicator
            frmTradeBook.Instance.load_data();
            frmTradeBook.Instance.lblnooftrade.Text = "No Of Trade  =" + frmTradeBook.Instance.DGV.Rows.Count;
            frmTradeBook.Instance.lblb_V.Text = Global.Instance.OrdetTable.AsEnumerable().Where(r => r.Field<string>("Status") == "Traded" && r.Field<string>("Buy_SellIndicator") == "BUY").Sum(r =>Convert.ToDouble(r.Field<string>("FillPrice")) * Convert.ToDouble(r.Field<string>("Volume"))).ToString();
            frmTradeBook.Instance.lbls_v.Text = Global.Instance.OrdetTable.AsEnumerable().Where(r => r.Field<string>("Status") == "Traded" && r.Field<string>("Buy_SellIndicator") == "SELL").Sum(r => Convert.ToDouble(r.Field<string>("FillPrice")) * Convert.ToDouble(r.Field<string>("Volume"))).ToString();
            frmTradeBook.Instance.lblb_q.Text =Global.Instance.OrdetTable.AsEnumerable().Where(r => r.Field<string>("Status") == "Traded" && r.Field<string>("Buy_SellIndicator") == "BUY").Sum(r=>Convert.ToDouble(r.Field<string>("Volume"))).ToString();
            frmTradeBook.Instance.lbls_q.Text = Global.Instance.OrdetTable.AsEnumerable().Where(r => r.Field<string>("Status") == "Traded" && r.Field<string>("Buy_SellIndicator") == "SELL").Sum(r => Convert.ToDouble(r.Field<string>("Volume"))).ToString();
            frmTradeBook.Instance.N_V.Text = (Convert.ToDouble(frmTradeBook.Instance.lbls_v.Text) - Convert.ToDouble(frmTradeBook.Instance.lblb_V.Text)).ToString();
            frmNetBook.Instance.netposion(0,0);
            frmTradeBook.Instance.DGV.Refresh();
        }
      

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (Global.Instance.SignInStatus == false)
            {
                Re_Login rgn = new Re_Login();
                rgn.ShowDialog();
            }
            else
                MessageBox.Show("User AllReady Login", "AllReady Login");
        }

        private void spreadWatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fofospreadwatch _foSpread = new fofospreadwatch();
            _foSpread.Width = this.Width - 50;
            _foSpread.Height = this.Height - 200;
            _foSpread.MdiParent = this;
            UDP_Reciever.Instance.OnDataArrived += _foSpread.OnDataArrived;
         //   NNFHandler.eOrderTRADE_CONFIRMATION_TR += _foSpread.TRADE_CONFIRMATION_TR;

            _foSpread.Show();
          //frmTradeBook.Instance.Show();
          
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (Form childForm in MdiChildren)
            //{
            //    if(childForm.Text== "FO FO Market Watch")
            //    {
            //        MessageBox.Show("Close FO_Fo_mktwatch");
            //        return;
            //    }
               
            //}
            if (Global.Instance.Fopairbool == true)
            {
                MessageBox.Show("unsubscribe the Token");
                return;
            }
            NNFInOut.Instance.SIGN_OFF_REQUEST_IN();
           savechildform();
                
               // Thread.Sleep(3000);
               // if (Global.Instance.warningvar == true)
                //{
                    //bool flg = false;
                    //if (_fofomarket != null)
                    //    flg = _fofomarket._foSpread__logoutstatus();
                    //else
                    //    flg = true;
                    //if (flg == true)
                    //{
                   
                        this.Dispose();
                        Environment.Exit(0);
                    //}
                    //else
                    //    MessageBox.Show("Please Unsubscribe All Token", "Information");
              //  }
            
        }

        public void logout()
        {
            
            this.Dispose();
           Environment.Exit(0);
        }


        void Loadchildform()
        { 
         
                var config = new Config { GroupName = null };
                int iforms = Convert.ToInt32(config.GetValue("FORMS", "MAX"));
                for (int iOpen = 0; iOpen < iforms; iOpen++)
                {
                    string sFormTitle = (string)config.GetValue("FORMS", iOpen.ToString());
                    if (sFormTitle == "ORDER BOOK")
                    {
                        toolStripStatusLabel3_Click(new object(), new EventArgs());
                    }
                    else if (sFormTitle == "TRADE BOOK")
                    {
                        TradeBook_Click(new object(), new EventArgs());
                    }
                    else if (sFormTitle == "NET BOOK")
                    {
                        NetBook_Click(new object(), new EventArgs());
                    }
                    else if (sFormTitle == "Market Watch")
                    {
                        marketWatchToolStripMenuItem_Click(new object(), new EventArgs());
                    }
                    else if (sFormTitle == "FO FO Market Watch")
                    {
                        foFoMarketWatchToolStripMenuItem_Click(new object(), new EventArgs());
                    }

                    else if (sFormTitle == "Spread Market Watch")
                    {
                        spreadWatchCtrlF1ToolStripMenuItem_Click(new object(), new EventArgs());
                    }
                    else if (sFormTitle == "Spread Order Book")
                    {
                        spreadOrderBookToolStripMenuItem_Click(new object(), new EventArgs());
                    }
                    else if (sFormTitle == "Index")
                    {
                        indexToolStripMenuItem1_Click(new object(), new EventArgs());
                        
                    }
            }

        }

        void savechildform()
        {
            int iforms = 0;
            var config = new Config { GroupName = null };
            foreach (Form childForm in MdiChildren)
            {
              //MessageBox.Show(childForm.Text);
                config.SetValue("FORMS",iforms.ToString(),childForm.Text);
                childForm.Close();
                iforms++;
            }
            config.SetValue("FORMS", "MAX", iforms);

        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            All_DataGRD algrd = new All_DataGRD();
            algrd.Show();
        }
        public void ShowDialog()
        {
            Form prompt = new Form();
            prompt.StartPosition = FormStartPosition.Manual;
            prompt.Location = new Point(500,300);
            prompt.Width = 500;
            prompt.Height = 200;
            prompt.Text = "Warning";
          
            //  prompt.Font = new Font(prompt.Font, FontStyle.Bold);
          //  prompt.Font = new System.Drawing.Font(prompt.Font.FontFamily.Name, 10);
            Label textLabel = new Label() { Left = 200, Top = 20, Text = "Do you want exit"};
           

            //textLabel.Font = new System.Drawing.Font(textLabel.Font.FontFamily.Name, 10);
            //NumericUpDown inputBox = new NumericUpDown() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Exit", Left =100, Width = 100, Top = 70 };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            Button cancel = new Button() { Text = "cancel", Left = 350, Width = 100, Top = 70 };
            confirmation.Click += (sender, e) => { warningfun(); };
            cancel.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(cancel);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            //prompt.Controls.Add(inputBox);
            
            prompt.ShowDialog();
            //return (int)inputBox.Value;
        }
        private void MDIParent1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Global.Instance.Fopairbool == true)
            {
                MessageBox.Show("unsubscribe the Token");
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure you want to Exit", "Optimus", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }
            else
            {

                Task.Factory.StartNew(() => savechildform());
                warningfun();
            }

          //  if (Global.Instance.Fopairbool == true)
          //  {
          //      MessageBox.Show("unsubscribe the Token");
          //      return;
          //  }
            
          //Task.Factory.StartNew(()=>savechildform());
          // ShowDialog();
          
        }
        public void warningfun()
        {
            
                //savechildform();
                NNFInOut.Instance.SIGN_OFF_REQUEST_IN();
               Thread.Sleep(2000);
                bool flg = false;
                if (_fofomarket != null)
                    flg = _fofomarket._foSpread__logoutstatus();
                else
                    flg = true;
                if (flg == true)
                {
                    this.Dispose();
                    Environment.Exit(0);
                }
                else
                    MessageBox.Show("Please Unsubscribe All Token", "Information");
            
        }
        private void lblData_Click(object sender, EventArgs e)
        {

        }

        private void lblTapStatus_Click(object sender, EventArgs e)
        {

        }

        private void lblOrder_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void helpMenu_Click(object sender, EventArgs e)
        {

        }

        private void spreadWatchCtrlF1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AppGlobal.frmSpreadWatch = new frmSpreadMktWatch();
            //AppGlobal.frmSpreadWatch.MdiParent = this;
            //AppGlobal.frmSpreadWatch.Show();


            if (AppGlobal.frmSpreadWatch == null)
            {
                AppGlobal.frmSpreadWatch = new frmSpreadMktWatch();

                // _frmMktWatch.WindowState = FormWindowState.Minimized;

                //  UDP_Reciever.Instance.OnDataArrived += _frmMktWatch.OnDataArrived;
            }
            AppGlobal.frmSpreadWatch.MdiParent = this;
            AppGlobal.frmSpreadWatch.Show();
            AppGlobal.frmSpreadWatch.InitDict();
        }

        private void spreadOrderBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppGlobal.frmSpdOBook = new FrmSpdOrderBook();
            AppGlobal.frmSpdOBook.MdiParent = this;
            AppGlobal.frmSpdOBook.Show();
        }

        private void indexToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //AppGlobal.frmSpotIndex = new Spot.frmSpot();
            //AppGlobal.frmSpotIndex.MdiParent = this;
            //AppGlobal.frmSpotIndex.Show();
            //Global.Instance.CashSock.ListenMcastData(34074, "192.168.168.242", "233.1.2.5");

           // Child_Index.Instance.TopMost = true;
            Child_Index.Instance.MdiParent = this;
            Child_Index.Instance.Show();
            Child_Index.Instance._indexDict();
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            Trade_Tracker.Instance.Show();
        }

      

    }
}
