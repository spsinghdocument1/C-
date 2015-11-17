using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Structure;
using System.IO;
using csv;
using System.Reflection;
using System.Net;
using System.Configuration;
using System.Xml;
using Microsoft.VisualBasic;
using System.Threading;
using AMS.Profile;

namespace Client
{
    public partial class Fo_Fo_mktwatch : Form
    {

        private readonly Dictionary<int, DataGridViewRow> _mwatchDict2 = new Dictionary<int, DataGridViewRow>();
        private readonly Dictionary<int, Data1> _DataDict2 = new Dictionary<int, Data1>();
        private DataGridViewCellStyle _makeItBlack;
        private DataGridViewCellStyle _makeItBlue;
        private DataGridViewCellStyle _makeItRed;
        public  int str_price1;
        internal DataTable SpreadTable;
        string strv = "";
        internal event EventHandler _logoutstatus;
        private int portFolioCounter = 1;
        public Fo_Fo_mktwatch()
        {
            InitializeComponent();
            
        }

       public bool _foSpread__logoutstatus()
        {
            bool flag = false;
            foreach (DataGridViewRow row in DGV1.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    return false;
                }
            }
            return true;
        }


        private void SetDisplayRules(DataGridViewColumn dgvCol, String Value)
        {
            dgvCol.HeaderText = Value;
            dgvCol.ReadOnly = true;

        }

        private static void SpreadTable_NewRow(object sender, DataTableNewRowEventArgs e)
        {
          
        }

        public void eOrderTRADED_OUT(byte[] buffer)
        {
            //  var rowlist = SpreadTable.Rows.Cast<DataRow>().Where(x => Convert.ToInt32(x["Token1"]) == Stat.Parameter.Token || Convert.ToInt32(x["Token2"]) == Stat.Parameter.Token).ToList();
            //DGV1.Rows[0].Cells[""].Value = Global.Instance.OrdetTable.Compute("SUM(STATUS ='Traded')","Token1")
        }


        public void TRADE_CONFIRMATION_TR(byte[] buffer)//--20222  
        {
            try
            {
                var v = buffer.Length == 153 ? 0 : buffer[153];
                if (v == 77)
                    return;
               var obj = (MS_TRADE_CONFIRM_TR)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_CONFIRM_TR));
                         DataRow[] dr = SpreadTable.Select("Token1 =" + IPAddress.HostToNetworkOrder(obj.Token) + "");
                if (dr.Length > 0)
                {
                  DataGridViewRow row = null;
                  //LogWriterClass.logwritercls.logs("tradeconfermation" , (IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator)).ToString());
                 // DataGridViewRow row = DGV1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Token1"].Value.ToString()==(Convert.ToString(IPAddress.HostToNetworkOrder(obj.Token)))).First();
                  foreach(DataGridViewRow _rw in DGV1.Rows)
                  {  if(_rw.Cells["Token1"].Value.ToString()==(Convert.ToString(IPAddress.HostToNetworkOrder(obj.Token))))
                      {
                          row = _rw;
                          break;
                      }
                  }
                                        
                  string _fartoken = DGV1.Rows[row.Index].Cells["Token2"].Value.ToString();
                 string[] drNeartoken = Global.Instance.OrdetTable.AsEnumerable().Where(a => a.Field<string>("status") == "Traded" && a.Field<string>("TokenNo") == Convert.ToString(_fartoken)) // _fartoken)
                                          .Select(av => av.Field<string>("FillPrice")).ToArray();
                    if (drNeartoken.Length > 0)
                    {
                    if (IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1)
                    {                       
                            DGV1.Rows[row.Index].Cells["BNSFTD"].Value = Convert.ToDouble(drNeartoken[drNeartoken.Length - 1]) - ((IPAddress.HostToNetworkOrder(obj.FillPrice))/100.00);
                    }
                    else
                    {                       
                        DGV1.Rows[row.Index].Cells["BFSNTD"].Value = ((IPAddress.HostToNetworkOrder(obj.FillPrice))/100.00) - Convert.ToDouble(drNeartoken[drNeartoken.Length-1]);
                    }
                    }
                   
                }
            }
            catch(Exception ex)
            {           
              //Client.LogWriterClass.logwritercls.logs("ErrorValuecheck.txt", "Value Check update in gridview" + ex.Message);
            }
        Fillqty_ingrd(buffer);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            if (DGV1.Rows.Count > 0)

                portFolioCounter = DGV1.Rows.Cast<DataGridViewRow>().Max(r => Convert.ToInt32(r.Cells["PF"].Value));
            else
                portFolioCounter = 0;

           
            portFolioCounter++;
            using (AddToken _AddToken = new AddToken())
            {
                _AddToken.txtpfName.Text = portFolioCounter.ToString();
                _AddToken.lblPfName.Visible = true;
                _AddToken.txtpfName.Visible = true;
                _AddToken.Text = "Add Near Month Token";
                _AddToken.button1.Text = "Add Next Token";
                if (_AddToken.ShowDialog() == DialogResult.OK)
                {
                     DataRow dr = SpreadTable.NewRow();

                    dr["PF"] = _AddToken._objOut.PFName;
                    dr["NEAR"] = _AddToken._objOut.Desc1;
                    dr["Token1"] = _AddToken._objOut.Token1;

                    dr["FAR"] = _AddToken._objOut.Desc2;
                    dr["Token2"] = _AddToken._objOut.Token2;
                    dr["TOK1_QTY"] = _AddToken._objOut.tok1_qty1;
                    dr["TOK2_QTY"] = _AddToken._objOut.tok2_qty2;
                    

                    SpreadTable.Rows.Add(dr);
                    DGV1.Rows[DGV1.Rows.Count-1].Cells["BNSFDIFF"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count-1].Cells["BFSNDIFF"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count-1].Cells["BNSFMNQ"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count-1].Cells["BFSNMNQ"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count-1].Cells["BNSFMXQ"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count-1].Cells["BFSNMXQ"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count - 1].Cells["TICKS"].Value = 0;


                   
                    
                    UDP_Reciever.Instance.Subscribe = _AddToken._objOut.Token1;
                    UDP_Reciever.Instance.Subscribe = _AddToken._objOut.Token2;
                  
                 
                }
   
            }

        
            
        }

        private void btnprofile_Click(object sender, EventArgs e)
        {
            var frmprf = new frmProfile();
            foreach (DataGridViewColumn dc in DGV1.Columns)
            {
                
                if (!frmprf.lbxSecondary.Items.Contains(dc.HeaderText))
                {
                    frmprf.lbxPrimary.Items.Add(dc.HeaderText);
                   
                }
              
            }
           
            if (frmprf.ShowDialog() == DialogResult.OK)
            {
                var config = new Config { GroupName = null };
                foreach (DataGridViewColumn dc in DGV1.Columns)
                {
                    this.DGV1.Columns[dc.HeaderText.Replace(" ", "")].Visible = true;                   
                }
                String GetProfileName = frmprf.GetProfileName();
               
                DataSet ds = new DataSet();
                ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + GetProfileName + ".xml");
                if(ds.Tables.Count ==  0)
                {
                    return;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                     string st = ds.Tables[0].Rows[i]["Input"].ToString();
                     if (st==null||st==DBNull.Value.ToString())
                         continue;
                    this.DGV1.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ","")].Visible = false;
                }
                config.SetValue("Fo_FO_Profile", Convert.ToString(0),GetProfileName);
               
            }
            else
            {
                //String GetProfileName = frmprf.GetProfileName();
                //DataSet ds = new DataSet();
                //ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + "MarketCol.xml");
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    string st = ds.Tables[0].Rows[i]["Input"].ToString();
                //    this.DGV1.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = true;
                //}
            }
           
        }

        delegate void OnDataArrivedDelegate(Object o, ReadOnlyEventArgs<FinalPrice> Stat);
        public void OnDataArrived(Object o, ReadOnlyEventArgs<FinalPrice> Stat)
        {
            
            try
            {
                if (DGV1.InvokeRequired)
                {
                    DGV1.Invoke(new OnDataArrivedDelegate(OnDataArrived), o, new ReadOnlyEventArgs<FinalPrice>(Stat.Parameter));
                    return;
                }
         
                var rowlist = SpreadTable.Rows.Cast<DataRow>().Where(x => Convert.ToInt32(x["Token1"]) == Stat.Parameter.Token || Convert.ToInt32(x["Token2"]) == Stat.Parameter.Token).ToList();
               
                    foreach (var i in rowlist)
                    {
                        if (DGV1.Rows.Count==0)
                        {
                            return;
                        }
                       
                        if (Convert.ToInt32(i["Token1"]) == Stat.Parameter.Token)
                        {
                            i["NBID"] = Math.Round(Convert.ToDouble(Stat.Parameter.MAXBID) / 100, 4);
                            i["NASK"] = Math.Round(Convert.ToDouble(Stat.Parameter.MINASK) / 100, 4);
                            i["NLTP"] = Math.Round(Convert.ToDouble(Stat.Parameter.LTP) / 100, 4);

                            i["S1"] = Convert.ToInt32(i["TOK1_QTY"] == DBNull.Value ? 0 : i["TOK1_QTY"]) * Convert.ToDouble(i["NASK"]);
                            i["V2"] = Convert.ToInt32(i["TOK1_QTY"] == DBNull.Value ? 0 : i["TOK1_QTY"]) * Convert.ToDouble(i["NBID"]);
                        //if(Global.Instance.write==true)
                        //{
                        //    StreamWriter wr = new StreamWriter(Application.StartupPath + Path.DirectorySeparatorChar+"ClientRECV_Token1", true);
                        //    wr.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + "," + Stat.Parameter.Token + "," + Stat.Parameter.LTP + "," + Stat.Parameter.MAXBID + "," + Stat.Parameter.MINASK);
                        //    wr.Close();
                        //}
                        }
                        else if (Convert.ToInt32(i["Token2"]) == Stat.Parameter.Token)
                        {
                                       
                            i["FBID"] = Math.Round(Convert.ToDouble(Stat.Parameter.MAXBID) / 100, 4);
                            i["FASK"] = Math.Round(Convert.ToDouble(Stat.Parameter.MINASK) / 100, 4);
                            i["FLTP"] = Math.Round(Convert.ToDouble(Stat.Parameter.LTP) / 100, 4);
                            i["S2"] = Convert.ToInt32(i["TOK2_QTY"] == DBNull.Value ? 0 : i["TOK2_QTY"]) * Convert.ToDouble(i["FASK"]);
                            i["V1"] = Convert.ToInt32(i["TOK2_QTY"] == DBNull.Value ? 0 : i["TOK2_QTY"]) * Convert.ToDouble(i["FBID"]);
                            //if (Global.Instance.write == true)
                            //{
                            //    StreamWriter wr = new StreamWriter(Application.StartupPath + Path.DirectorySeparatorChar + "ClientRECV_Token2", true);
                            //    wr.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + "," + Stat.Parameter.Token + "," + Stat.Parameter.LTP + "," + Stat.Parameter.MAXBID + "," + Stat.Parameter.MINASK);
                            //    wr.Close();
                            //}
                        }
                        i["BEQ"] = Math.Round((Convert.ToDouble(i["V2"] == DBNull.Value ? 0 : i["V2"]) - Convert.ToDouble(i["V1"] == DBNull.Value ? 0 : i["V1"])) / Convert.ToDouble(i["NBID"] == DBNull.Value ? 1 : i["NBID"]),4);
                        i["SEQ"] =Math.Round((Convert.ToDouble(i["S2"] == DBNull.Value ? 0 : i["S2"]) - Convert.ToDouble(i["S1"] == DBNull.Value ? 0 : i["S1"])) / Convert.ToDouble(i["NASK"] == DBNull.Value ? 1 : i["NASK"]),4);
                        Global.Instance.MTMDIct.AddOrUpdate(Stat.Parameter.Token, Stat.Parameter.LTP, (k, v) => Stat.Parameter.LTP);
                    }
                


            }
            catch (DataException a)
            {
                MessageBox.Show("From Live Data fill " + Environment.NewLine + a.Message);
            }
        }

        private void SetData(DataGridViewCell DGCell, double ValueOne)
        {
            if (DGCell != null)
            {
                double ValueTwo = DGCell.Value == DBNull.Value || String.IsNullOrWhiteSpace(DGCell.Value.ToString()) ? 0 : Convert.ToDouble(DGCell.Value);           //Convert.ToDouble(DGCell.Value);
                if (ValueOne > ValueTwo)
                {
                    DGCell.Style = _makeItBlue;
                }
                else if (ValueOne < ValueTwo)
                {
                    DGCell.Style = _makeItRed;
                }
                else if (ValueOne == ValueTwo)
                {
                    DGCell.Style = _makeItBlack;
                }
            }

            DGCell.Value = ValueOne;


        }



        private void SetData2(DataGridViewCell DGCell, double ValueOne)
        {
          
            if (DGCell != null)
            {
                double ValueTwo = DGCell.Value == DBNull.Value || String.IsNullOrWhiteSpace(DGCell.Value.ToString()) ? 0 : Convert.ToDouble(DGCell.Value);          
            }

            DGCell.Value = ValueOne;
            Console.WriteLine(ValueOne + "  " + DGCell);
           
        }

        private void Fo_Fo_mktwatch_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            //applyFun();
        }
        DataGridViewRow row = null;
        TradeTrac trd_obj;

        private List<SelectListItem> first_tok=new List<SelectListItem>();
        private List<SelectListItem> second_tok=new List<SelectListItem>();
        
        public  void Fillqty_ingrd(byte[] buffer)
        {
            try
            {
                object ob = new object();
                lock (ob)
                {

                    var obj = (MS_TRADE_CONFIRM_TR)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_CONFIRM_TR));
                    foreach(DataGridViewRow row2 in DGV1.Rows)
                    {
                        if (Convert.ToString(row2.Cells["Token1"].Value) == Convert.ToString(IPAddress.HostToNetworkOrder(obj.Token)))
                        {
                            row = row2;
                            break;
                        }
                    }

                  //  DataGridViewRow row = DGV1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Token1"].Value.ToString().Equals(Convert.ToString(IPAddress.HostToNetworkOrder(obj.Token)))).FirstOrDefault();
                    if (row == null)
                    {
                     //row = DGV1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Token2"].Value.ToString().Equals(Convert.ToString(IPAddress.HostToNetworkOrder(obj.Token)))).FirstOrDefault();
                     return;
                    }

                 

                     first_tok=frmNetBook.Instance.query1.Where(a => a.TokenNo == Convert.ToInt32(DGV1.Rows[row.Index].Cells["Token1"].Value)).ToList();
                     second_tok = frmNetBook.Instance.query1.Where(a => a.TokenNo == Convert.ToInt32(DGV1.Rows[row.Index].Cells["Token2"].Value)).ToList();
                    string _neartoken = DGV1.Rows[row.Index].Cells["Token1"].Value.ToString();
                    int _first_Tok = Convert.ToInt32(DGV1.Rows[row.Index].Cells["Token1"].Value);
                  //  string _fartoken = DGV1.Rows[row.Index].Cells["Token2"].Value.ToString();
                    if (_neartoken == Convert.ToString(IPAddress.HostToNetworkOrder(obj.Token)))
                    {
                        if (IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1)
                        {
                            if (DGV1.Rows[row.Index].Cells["BNSFTQ"].Value == null || Convert.ToString(DGV1.Rows[row.Index].Cells["BNSFTQ"].Value) == "")
                            {
                                DGV1.Rows[row.Index].Cells["BNSFTQ"].Value = 0;
                            }
                          //  if (IPAddress.HostToNetworkOrder(obj.RemainingVolume) <= 0)
                           // {
                                DGV1.Rows[row.Index].Cells["BNSFTQ"].Value = Convert.ToInt32(DGV1.Rows[row.Index].Cells["BNSFTQ"].Value) + 1;
                                /* this.DGV1.Columns["Buy_Avg"].DefaultCellStyle.Format = "0.##";
                                this.DGV1.Columns["Sell_Avg"].DefaultCellStyle.Format = "0.##";*/
                            //}
                            if (DGV1.Rows[row.Index].Cells["Buy_Avg"].Value == null || Convert.ToString(DGV1.Rows[row.Index].Cells["Buy_Avg"].Value) == "")
                            {
                                DGV1.Rows[row.Index].Cells["Buy_Avg"].Value = 0;
                            }
                            
                            DGV1.Rows[row.Index].Cells["Buy_Avg"].Value =((first_tok.Count() ==0 ?0 : first_tok.FirstOrDefault().BuyAvg) - (second_tok.Count()==0?0:second_tok.FirstOrDefault().SellAvg));

                            if (Global.Instance.TradeTrac_dict.ContainsKey(_first_Tok))
                            {
                                trd_obj = new TradeTrac();
                                trd_obj = Global.Instance.TradeTrac_dict[_first_Tok];


                                trd_obj.ACTUALPRICE = Convert.ToDouble(DGV1.Rows[row.Index].Cells["BNSFTD"].Value);
                                trd_obj.B_S = IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator);
                                trd_obj.QTy = IPAddress.HostToNetworkOrder(obj.FillQuantity);
                                trd_obj.SYMBOL = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.Symbol);
                                trd_obj.TIME = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss.fff");
                                Global.Instance.TradeTrac_dict.AddOrUpdate(IPAddress.HostToNetworkOrder(obj.Token), trd_obj, (k, v1) => trd_obj);
                                DataRow dr2 = Global.Instance.TradeTracker.NewRow();

                                //  dr2["Sno2"] = trd_ob.ToString();
                                dr2["PF_ID"] = Convert.ToString(trd_obj.PF_ID);

                                dr2["B/S"] = trd_obj.B_S == 1 ? "BUY" : "SELL";
                                dr2["QTY"] = Convert.ToString(trd_obj.QTy);
                                dr2["ACTUALPRICE"] = Convert.ToString(trd_obj.ACTUALPRICE);
                                dr2["GIVENPRICEBUY"] = Convert.ToString(trd_obj.Given_Price_Buy);
                                dr2["SYMBOL"] = Convert.ToString(trd_obj.SYMBOL);

                                dr2["TIME"] = Convert.ToString(trd_obj.TIME);
                                
                                //if ((trd_obj.Given_Price_Buy/100) > trd_obj.ACTUALPRICE)
                                //{
                                //    DGV1.Rows[row.Index].Cells["WTC"].Value = Convert.ToInt32(DGV1.Rows[row.Index].Cells["WTC"].Value) + 1;
                                //}
                                
                                Global.Instance.TradeTracker.Rows.Add(dr2);

                                //DGV1.Rows[row.Index].Cells["_con_WTC"].Value = Convert.ToInt32(DGV1.Rows[row.Index].Cells["_con_WTC"].Value) + 1;
                               
                                //if (Convert.ToInt32(DGV1.Rows[row.Index].Cells["WTC"].Value) == Convert.ToInt32(WTC_txt.Text) && Convert.ToInt32(DGV1.Rows[row.Index].Cells["_con_WTC"].Value) == Convert.ToInt32(DGV1.Rows[row.Index].Cells["WTC"].Value))
                                //{
                                //    DGV1.Rows[row.Index].Cells["WTC"].Value = 0;
                                //    DGV1.Rows[row.Index].Cells["_con_WTC"].Value = 0;
                                //    DGV1.Rows[row.Index].Cells["Enable"].Value = false;
                                //}
                                //if (Convert.ToInt32(DGV1.Rows[row.Index].Cells["WTC"].Value) != Convert.ToInt32(DGV1.Rows[row.Index].Cells["_con_WTC"].Value))
                                //{
                                //    DGV1.Rows[row.Index].Cells["WTC"].Value = 0;
                                //    DGV1.Rows[row.Index].Cells["_con_WTC"].Value = 0;
                                //}

                            }
                        }
                        else
                        {
                            if (DGV1.Rows[row.Index].Cells["BFSNTQ"].Value == null || Convert.ToString(DGV1.Rows[row.Index].Cells["BFSNTQ"].Value) == "")
                            {
                                DGV1.Rows[row.Index].Cells["BFSNTQ"].Value = 0;
                            }
                           // if (IPAddress.HostToNetworkOrder(obj.RemainingVolume) <= 0)
                            //{
                                DGV1.Rows[row.Index].Cells["BFSNTQ"].Value = Convert.ToInt32(DGV1.Rows[row.Index].Cells["BFSNTQ"].Value) + 1;
                            //}
                            if (DGV1.Rows[row.Index].Cells["Sell_Avg"].Value == null || Convert.ToString(DGV1.Rows[row.Index].Cells["Sell_Avg"].Value) == "")
                            {
                                DGV1.Rows[row.Index].Cells["Sell_Avg"].Value = 0;
                            }
                            DGV1.Rows[row.Index].Cells["Sell_Avg"].Value = (second_tok.Count()==0?0:second_tok.FirstOrDefault().BuyAvg) - (first_tok.Count()==0?0:first_tok.FirstOrDefault().SellAvg); 
                            if (Global.Instance.TradeTrac_dict.ContainsKey(_first_Tok))
                            {
                                trd_obj = new TradeTrac();
                                trd_obj = Global.Instance.TradeTrac_dict[IPAddress.HostToNetworkOrder(obj.Token)];
                                trd_obj.ACTUALPRICE = Convert.ToDouble(DGV1.Rows[row.Index].Cells["BFSNTD"].Value);
                                trd_obj.B_S = IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator);
                                trd_obj.QTy = IPAddress.HostToNetworkOrder(obj.FillQuantity);
                                trd_obj.SYMBOL = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.Symbol);
                                trd_obj.TIME = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss.fff");
                                Global.Instance.TradeTrac_dict.AddOrUpdate(IPAddress.HostToNetworkOrder(obj.Token), trd_obj, (k, v1) => trd_obj);
                                DataRow dr2 = Global.Instance.TradeTracker.NewRow();
                                //  dr2["Sno2"] = trd_ob.ToString();
                                dr2["PF_ID"] = Convert.ToString(trd_obj.PF_ID);
                                dr2["B/S"] = trd_obj.B_S == 1 ? "BUY" : "SELL";
                                dr2["QTY"] = Convert.ToString(trd_obj.QTy);
                                dr2["ACTUALPRICE"] = Convert.ToString(trd_obj.ACTUALPRICE);
                                dr2["GIVENPRICESEll"] = Convert.ToString(trd_obj.Given_Price_Sell);
                                dr2["SYMBOL"] = Convert.ToString(trd_obj.SYMBOL);
                                dr2["TIME"] = Convert.ToString(trd_obj.TIME);


                                Global.Instance.TradeTracker.Rows.Add(dr2);
                                // _sell_con_WTC_sell_WTC
                                //if (((trd_obj.Given_Price_Buy/100)-(trd_obj.ACTUALPRICE))<0)
                                //if (((IPAddress.HostToNetworkOrder(obj.FillPrice) / 100) - (trd_obj.Given_Price_Buy / 100)) > ((IPAddress.HostToNetworkOrder(obj.FillPrice) / 100) + trd_obj.ACTUALPRICE))
                                //{
                                //    DGV1.Rows[row.Index].Cells["_sell_WTC"].Value = Convert.ToInt32(DGV1.Rows[row.Index].Cells["_sell_WTC"].Value) + 1;
                                //}
                                //var _T_v=(IPAddress.HostToNetworkOrder(obj.FillPrice) / 100) + trd_obj.ACTUALPRICE;
                                //DGV1.Rows[row.Index].Cells["_sell_con_WTC"].Value = Convert.ToInt32(DGV1.Rows[row.Index].Cells["_sell_con_WTC"].Value) + 1;
                                //if (Convert.ToInt32(DGV1.Rows[row.Index].Cells["_sell_WTC"].Value) == Convert.ToInt32(WTC_txt.Text) && Convert.ToInt32(DGV1.Rows[row.Index].Cells["_sell_con_WTC"].Value) == Convert.ToInt32(DGV1.Rows[row.Index].Cells["_sell_WTC"].Value))
                                //{
                                //    DGV1.Rows[row.Index].Cells["_sell_WTC"].Value = 0;
                                //    DGV1.Rows[row.Index].Cells["_sell_con_WTC"].Value = 0;
                                //    DGV1.Rows[row.Index].Cells["Enable"].Value = false;
                                //}

                                //if (Convert.ToInt32(DGV1.Rows[row.Index].Cells["_sell_WTC"].Value) != Convert.ToInt32(DGV1.Rows[row.Index].Cells["_sell_con_WTC"].Value))
                                //{
                                //    DGV1.Rows[row.Index].Cells["_sell_WTC"].Value = 0;
                                //    DGV1.Rows[row.Index].Cells["_sell_con_WTC"].Value = 0;
                                //}
                            }

                        }
                    }
                    //else if (_fartoken == Convert.ToString(IPAddress.HostToNetworkOrder(obj.Token)))
                    //{
                    //    if(IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1)
                    //    { 
                    //    if (DGV1.Rows[row.Index].Cells["BFSNTQ"].Value == null || Convert.ToString(DGV1.Rows[row.Index].Cells["BFSNTQ"].Value) == "")
                    //    {
                    //        DGV1.Rows[row.Index].Cells["BFSNTQ"].Value = 0;
                    //    }
                    //    DGV1.Rows[row.Index].Cells["BFSNTQ"].Value = Convert.ToInt32(DGV1.Rows[row.Index].Cells["BFSNTQ"].Value) + 1;
                    //    }
                    //    else
                    //    {
                    //        if (DGV1.Rows[row.Index].Cells["BNSFTQ"].Value == null || Convert.ToString(DGV1.Rows[row.Index].Cells["BNSFTQ"].Value) == "")
                    //        {
                    //            DGV1.Rows[row.Index].Cells["BNSFTQ"].Value = 0;
                    //        }
                    //        DGV1.Rows[row.Index].Cells["BNSFTQ"].Value = Convert.ToInt32(DGV1.Rows[row.Index].Cells["BNSFTQ"].Value) + 1;                            
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
               // Client.LogWriterClass.logwritercls.logs("ErrorValue_check.txt", "Value Check update in gridview" + ex.Message);

            }
        }

        //private void DGV1_KeyDown(object sender, KeyEventArgs e)  
        //{
        //    if (e.Modifiers == Keys.Shift)
        //    {
        //        if (e.KeyCode == Keys.F2)
        //        {
        //            using (frmDiff _frmDIff = new frmDiff())
        //            { 
                        
        //              //_frmDIff._FOPairDiff.TokenNear = DGV1.SelectedRows[]
       


        //                _frmDIff._FOPairDiff.BFSNDIFF = Convert.ToDouble(Convert.IsDBNull(DGV1.SelectedRows[0].Cells["BFSNDIFF"].Value) ? 0 : DGV1.SelectedRows[0].Cells["BFSNDIFF"].Value);
        //                _frmDIff._FOPairDiff.BNSFDIFF = Convert.ToDouble(Convert.IsDBNull(DGV1.SelectedRows[0].Cells["BNSFDIFF"].Value) ? 0 : DGV1.SelectedRows[0].Cells["BNSFDIFF"].Value);
        //                _frmDIff._FOPairDiff.MINQTY = Convert.ToInt32(Convert.IsDBNull(DGV1.SelectedRows[0].Cells["MINQTY"].Value) ? 0 : DGV1.SelectedRows[0].Cells["MINQTY"].Value); 
        //                _frmDIff._FOPairDiff.MAXQTY=  Convert.ToInt32( Convert.IsDBNull(DGV1.SelectedRows[0].Cells["MAXQTY"].Value)  ? 0 :DGV1.SelectedRows[0].Cells["MAXQTY"].Value);


        //                if (_frmDIff.ShowDialog() == DialogResult.OK)
        //                {
        //                    DGV1.SelectedRows[0].Cells["BFSNDIFF"].Value = _frmDIff._FOPairDiff.BFSNDIFF;
        //                    DGV1.SelectedRows[0].Cells["BNSFDIFF"].Value = _frmDIff._FOPairDiff.BNSFDIFF;
        //                    DGV1.SelectedRows[0].Cells["MINQTY"].Value = _frmDIff._FOPairDiff.MINQTY;
        //                    DGV1.SelectedRows[0].Cells["MAXQTY"].Value = _frmDIff._FOPairDiff.MAXQTY;


        //                    _frmDIff._FOPairDiff.PORTFOLIONAME =Convert.ToInt32( DGV1.SelectedRows[0].Cells["PF"].Value);
        //                    _frmDIff._FOPairDiff.TokenNear =Convert.ToInt32( DGV1.SelectedRows[0].Cells["Token1"].Value);
        //                    _frmDIff._FOPairDiff.TokenFar =Convert.ToInt32( DGV1.SelectedRows[0].Cells["Token2"].Value);

        //                    byte[] buffer = DataPacket.RawSerialize(_frmDIff._FOPairDiff);
        //                    NNFHandler.Instance.Publisher("FOPAIRDIFF", buffer);

        //                }

        //            }
        //        }
        //    }

        //}

        private void btnsaveMktwatch_Click(object sender, EventArgs e)
        {
            if (DGV1.Rows.Count == 0)
                return;
            SaveFileDialog savd = new SaveFileDialog();
            savd.AddExtension = true;
            savd.DefaultExt = "xml";
            savd.Filter = "*.xml|*.*";
            
            if(savd.ShowDialog() == DialogResult.OK)
            {
                SpreadTable = (DataTable)DGV1.DataSource;
                savd.DefaultExt = ".xml";
                SpreadTable.WriteXml(savd.FileName );
                var config = new Config { GroupName = null };
                config.SetValue("PF_Profile", Convert.ToString(0), savd.FileName);
                //var config = new Config { GroupName = null };
                //var iforms = config.GetValue("Fo_FO_Profile", Convert.ToString(0));
             
            }

            DataTable dt_save = new DataTable("lastvalue");
            dt_save.Columns.Add("BNSFDIFF", typeof(Double));
            dt_save.Columns.Add("BFSNDIFF", typeof(Double));
            dt_save.Columns.Add("BNSFMNQ", typeof(int));
            dt_save.Columns.Add("BFSNMNQ", typeof(int));
            dt_save.Columns.Add("BNSFMXQ", typeof(int));
            dt_save.Columns.Add("BFSNMXQ", typeof(int));
            dt_save.Columns.Add("TICKS", typeof(int));

            foreach (DataGridViewRow row in DGV1.Rows)
            {
                DataRow dRow = dt_save.NewRow();
                dRow["BNSFDIFF"] = row.Cells["BNSFDIFF"].Value == null || row.Cells["BNSFDIFF"].Value==DBNull.Value ? 0 : row.Cells["BNSFDIFF"].Value;
                dRow["BFSNDIFF"] = row.Cells["BFSNDIFF"].Value == null || row.Cells["BFSNDIFF"].Value == DBNull.Value ? 0 : row.Cells["BFSNDIFF"].Value;
                dRow["BNSFMNQ"] = row.Cells["BNSFMNQ"].Value == null || row.Cells["BNSFMNQ"].Value == DBNull.Value ? 0 : row.Cells["BNSFMNQ"].Value;
                dRow["BFSNMNQ"] = row.Cells["BFSNMNQ"].Value == null || row.Cells["BFSNMNQ"].Value == DBNull.Value ? 0 : row.Cells["BFSNMNQ"].Value;
                dRow["BNSFMXQ"] = row.Cells["BNSFMXQ"].Value == null || row.Cells["BNSFMXQ"].Value == DBNull.Value ? 0 : row.Cells["BNSFMXQ"].Value;
                dRow["BFSNMXQ"] = row.Cells["BFSNMXQ"].Value == null || row.Cells["BFSNMXQ"].Value == DBNull.Value ? 0 : row.Cells["BFSNMXQ"].Value;
                dRow["TICKS"] = row.Cells["TICKS"].Value == null || row.Cells["TICKS"].Value == DBNull.Value ? 0 : row.Cells["TICKS"].Value;
                dt_save.Rows.Add(dRow);
            }

             dt_save.WriteXml(Application.StartupPath+Path.DirectorySeparatorChar+"Lastvalue.xml");
           
        }

        private void btnLoadMktWatch_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();

            opn.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if(opn.ShowDialog() == DialogResult.OK)
            {
                SpreadTable.Clear();
                DataSet ds_set = new DataSet();
                if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "Lastvalue.xml"))
                    return;
                ds_set.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Lastvalue.xml");
               
                SpreadTable.ReadXml(opn.FileName);
          //  SpreadTable.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "FOWATCH.xml");
                for (int i = 0; i < ds_set.Tables[0].Rows.Count; i++)
                {
                    DGV1.Rows[i].Cells["BNSFDIFF"].Value = ds_set.Tables[0].Rows[i]["BNSFDIFF"];
                    DGV1.Rows[i].Cells["BFSNDIFF"].Value = ds_set.Tables[0].Rows[i]["BFSNDIFF"];
                    DGV1.Rows[i].Cells["BNSFMNQ"].Value = ds_set.Tables[0].Rows[i]["BNSFMNQ"];
                    DGV1.Rows[i].Cells["BFSNMNQ"].Value = ds_set.Tables[0].Rows[i]["BFSNMNQ"];
                    DGV1.Rows[i].Cells["BNSFMXQ"].Value = ds_set.Tables[0].Rows[i]["BNSFMXQ"];
                    DGV1.Rows[i].Cells["BFSNMXQ"].Value = ds_set.Tables[0].Rows[i]["BFSNMXQ"];
                    DGV1.Rows[i].Cells["TICKS"].Value = ds_set.Tables[0].Rows[i]["TICKS"];
                }
            }

            _SelectionOut _objOut = new _SelectionOut();

            for (int i = 0; i <SpreadTable.Rows.Count ; i++)
            {
                UDP_Reciever.Instance.Subscribe = Convert.ToInt32(SpreadTable.Rows[i]["Token1"].ToString());
                UDP_Reciever.Instance.Subscribe = Convert.ToInt32(SpreadTable.Rows[i]["Token2"].ToString());
                portFolioCounter++;

            }
            if(DGV1.Rows.Count==0)
            { return; }
           
            portFolioCounter  = Convert.ToInt32( SpreadTable.Compute("MAX(PF)", ""))+1;          
        }
        public static int[] LoadFormLocationAndSize(Form xForm)
        {
            int[] t = { 0, 0, 900, 300 };
            if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "formclose.xml"))
                return t;
            DataSet dset = new DataSet();
            dset.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "formclose.xml");
            int[] LocationAndSize = new int[] { xForm.Location.X, xForm.Location.Y, xForm.Size.Width, xForm.Size.Height };
           
            try
            {              
                var AbbA =   dset.Tables[0].Rows[0]["Input"].ToString().Split(';');
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
         //   ini.IniWriteValue("FOFOFORM","Location", String.Format("{0};{1};{2};{3}", xForm.Location.X, xForm.Location.Y, xForm.Size.Width, xForm.Size.Height));

            var settings = new XmlWriterSettings { Indent = true };

            XmlWriter writer = XmlWriter.Create(Application.StartupPath + Path.DirectorySeparatorChar + "formclose.xml", settings);

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
        private void Fo_Fo_mktwatch_Load(object sender, EventArgs e)
        {
            Global.Instance.Best_Bid = true;
            Global.Instance.WTC_cnt = 0;
            var AbbA = LoadFormLocationAndSize(this);
            this.Location = new Point(AbbA[0], AbbA[1]);
            this.Size = new Size(AbbA[2], AbbA[3]);
          
            this.FormClosing += new FormClosingEventHandler(SaveFormLocationAndSize);
            DataGridViewColumnSelector cs = new DataGridViewColumnSelector(DGV1);
            cs.MaxHeight = 200;
            cs.Width = 150;
            Insert_txt.Visible = false;
            _makeItRed = new DataGridViewCellStyle();
            _makeItBlue = new DataGridViewCellStyle(); 
            _makeItBlack = new DataGridViewCellStyle();

            _makeItRed.BackColor = Color.Red;

            _makeItBlue.BackColor = Color.Blue;
            _makeItBlack.BackColor = Color.Black;
          SpreadTable = new DataTable("SPREADFO");
         

            SpreadTable.Columns.Add("PF", typeof(String));
            SpreadTable.Columns.Add("Token1", typeof(Int32));
            SpreadTable.Columns.Add("Token2", typeof(Int32));
            SpreadTable.Columns.Add("NEAR", typeof(String));
            SpreadTable.Columns.Add("FAR", typeof(String));
            SpreadTable.Columns.Add("NBID", typeof(Double));
            SpreadTable.Columns.Add("NASK", typeof(Double));
            SpreadTable.Columns.Add("NLTP", typeof(Double));
            SpreadTable.Columns.Add("FBID", typeof(Double));
            SpreadTable.Columns.Add("FASK", typeof(Double));
            SpreadTable.Columns.Add("FLTP", typeof(Double));

            SpreadTable.Columns.Add("BEQ", typeof(Double));
            SpreadTable.Columns.Add("SEQ", typeof(Double));
            SpreadTable.Columns.Add("V1", typeof(Double));
            SpreadTable.Columns.Add("V2", typeof(Double));

            SpreadTable.Columns.Add("S1", typeof(Double));
            SpreadTable.Columns.Add("S2", typeof(Double));

            SpreadTable.Columns.Add("TOK1_QTY", typeof(int));
            SpreadTable.Columns.Add("TOK2_QTY", typeof(int));
          //  SpreadTable.Columns.Add("SEQ", typeof(Double));

            SpreadTable.Columns.Add("NBD", typeof(Double), "FASK-NASK");
            SpreadTable.Columns.Add("NHD", typeof(Double), "FBID -NASK");
            SpreadTable.Columns.Add("FBD", typeof(Double), "FBID-NBID");
            SpreadTable.Columns.Add("FHD", typeof(Double), "FASK-NBID");
           
            DGV1.DataSource = SpreadTable;
        

            DGV1.Columns["Token1"].Visible = false;
           
            DGV1.Columns["Token2"].Visible = false;

            DGV1.Columns["FAR"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["NEAR"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["NBID"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["NBID"].SortMode = DataGridViewColumnSortMode.NotSortable;

            DGV1.Columns["PF"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["FLTP"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["NBD"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["FBD"].SortMode = DataGridViewColumnSortMode.NotSortable;

            DGV1.Columns["NHD"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["FHD"].SortMode = DataGridViewColumnSortMode.NotSortable;

            DGV1.Columns["Token1"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["Token2"].SortMode = DataGridViewColumnSortMode.NotSortable;


            DGV1.Columns["NASK"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["NLTP"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["FBID"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["FASK"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["BEQ"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["SEQ"].SortMode = DataGridViewColumnSortMode.NotSortable;

            SetDisplayRules(this.DGV1.Columns["Token1"], "Token1");
            SetDisplayRules(this.DGV1.Columns["Token2"], "Token2");

            SetDisplayRules(this.DGV1.Columns["BEQ"], "BEQ");
            SetDisplayRules(this.DGV1.Columns["SEQ"], "SEQ");
            SetDisplayRules(this.DGV1.Columns["V1"], "V1");
            SetDisplayRules(this.DGV1.Columns["V2"], "V2");
            SetDisplayRules(this.DGV1.Columns["S1"], "S1");
            SetDisplayRules(this.DGV1.Columns["S2"], "S2");
            SetDisplayRules(this.DGV1.Columns["TOK1_QTY"], "TOK1_QTY");
            SetDisplayRules(this.DGV1.Columns["TOK2_QTY"], "TOK2_QTY");


            SetDisplayRules(this.DGV1.Columns["PF"], "PF");

            SetDisplayRules(this.DGV1.Columns["NEAR"], "NEAR");


            SetDisplayRules(this.DGV1.Columns["FAR"], "FAR");
            
            SetDisplayRules(this.DGV1.Columns["NBID"], "NBID");
            SetDisplayRules(this.DGV1.Columns["NASK"], "NASK");
            
            SetDisplayRules(this.DGV1.Columns["NLTP"], "NLTP"); 
            SetDisplayRules(this.DGV1.Columns["FBID"], "FBID");
       
            SetDisplayRules(this.DGV1.Columns["FASK"], "FASK");   // Token2Ask
            SetDisplayRules(this.DGV1.Columns["FLTP"], "FLTP");   // Token2Ltp

            SetDisplayRules(this.DGV1.Columns["NBD"], "NBD");   //NearBidDiff
            SetDisplayRules(this.DGV1.Columns["NHD"], "NHD");   // NearHitDiff
          
            SetDisplayRules(this.DGV1.Columns["FBD"], "FBD");   // FarBidDiff
            SetDisplayRules(this.DGV1.Columns["FHD"], "FHD");   //FarHitDiff

            this.DGV1.Columns["NBD"].DefaultCellStyle.Format = "0.0000##";
            this.DGV1.Columns["NHD"].DefaultCellStyle.Format = "0.0000##";
            this.DGV1.Columns["FBD"].DefaultCellStyle.Format = "0.0000##";
            this.DGV1.Columns["FHD"].DefaultCellStyle.Format = "0.0000##";

            /////////////////////////////////////////////////////////////////////////////////////////////////////
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BNSFDIFF",
                HeaderText = "BNSFDIFF",               

            });
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BFSNDIFF",
                HeaderText = "BFSNDIFF",

            });


           
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BNSFMNQ",
                HeaderText = "BNSFMNQ",
            });
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BFSNMNQ",
                HeaderText = "BFSNMNQ",
            });
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BNSFMXQ",
                HeaderText = "BNSFMXQ",
            });
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {       
                Name = "BFSNMXQ",                
                HeaderText = "BFSNMXQ",
            });

            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TICKS",
                HeaderText = "TICKS",
            });
      //////////////////////////////////////////////////////////////////////////////////////////////////////
           this.DGV1.Columns.Add(new DataGridViewButtonColumn()
            {
                Name = "Apply",
                HeaderText = "Apply",
                Text = "Apply",
                UseColumnTextForButtonValue = true
            });

           
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BNSFTQ",
                HeaderText = "BNSFTQ",
                ReadOnly = true
            });
          

            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BNSFTD",
                HeaderText = "BNSFTD",
                ReadOnly = true
            });

            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BFSNTQ",
                HeaderText = "BFSNTQ",
                ReadOnly = true
            });
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BFSNTD",
                HeaderText = "BFSNTD",
                ReadOnly = true
            });

            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Buy_Avg",
                HeaderText = "Buy_Avg",
                ReadOnly = true
            });
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Sell_Avg",
                HeaderText = "Sell_Avg",
                ReadOnly = true
            });

            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "WTC",
                HeaderText = "WTC",

            });
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "_con_WTC",
                HeaderText = "_con_WTC",

            });

            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "_sell_WTC",
                HeaderText = "_sell_WTC",

            });
           
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "_sell_con_WTC",
                HeaderText = "_sell_con_WTC",

            });
            DGV1.Columns["WTC"].DefaultCellStyle.NullValue = 0;
            DGV1.Columns["_con_WTC"].DefaultCellStyle.NullValue = 0;
            DGV1.Columns["_sell_WTC"].DefaultCellStyle.NullValue = 0;
            DGV1.Columns["_sell_con_WTC"].DefaultCellStyle.NullValue = 0;


            DGV1.Columns["WTC"].ReadOnly = true;
            DGV1.Columns["_con_WTC"].ReadOnly = true;
            DGV1.Columns["_sell_WTC"].ReadOnly = true;
            DGV1.Columns["_sell_con_WTC"].ReadOnly = true;
                        
            DGV1.Columns["BNSFDIFF"].DefaultCellStyle.NullValue = 0.00;
            DGV1.Columns["BFSNDIFF"].DefaultCellStyle.NullValue = 0.00;
            DGV1.Columns["BNSFMNQ"].DefaultCellStyle.NullValue = 0.00;
            DGV1.Columns["BFSNMNQ"].DefaultCellStyle.NullValue = 0.00;
            DGV1.Columns["BNSFMXQ"].DefaultCellStyle.NullValue = 0.00;
            DGV1.Columns["BFSNMXQ"].DefaultCellStyle.NullValue = 0.000;
            DGV1.Columns["TICKS"].DefaultCellStyle.NullValue = 0;

            this.DGV1.Columns["BNSFTD"].DefaultCellStyle.Format = "0.##";
            this.DGV1.Columns["BFSNTD"].DefaultCellStyle.Format = "0.##";

            this.DGV1.Columns["Buy_Avg"].DefaultCellStyle.Format = "0.##";
            this.DGV1.Columns["Sell_Avg"].DefaultCellStyle.Format = "0.##";

            _makeItRed = new DataGridViewCellStyle();
            _makeItBlue = new DataGridViewCellStyle();
            _makeItBlack = new DataGridViewCellStyle();
            _makeItRed.BackColor = Color.Red;
            _makeItBlue.BackColor = Color.Blue;
            _makeItBlack.BackColor = Color.Black;
            SpreadTable.TableNewRow += new DataTableNewRowEventHandler(SpreadTable_NewRow);
         //   NNFHandler.eOrderTRADE_ERROR += Fillqty_ingrd;  

            btnStopAll.Enabled = true;
            btnStartAll.Enabled = true;
            DGV1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Type controlType = DGV1.GetType();
            PropertyInfo pi = controlType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(DGV1, true, null);
            try
            { 
                foreach (DataGridViewColumn dc in DGV1.Columns)
                {

                    this.DGV1.Columns[dc.HeaderText.Replace(" ", "")].Visible = true;
                }

                DataSet ds = new DataSet();
                //config.SetValue("Fo_FO_Profile", Convert.ToString(0), GetProfileName);
                var config = new Config { GroupName = null };
                var iforms = config.GetValue("Fo_FO_Profile", Convert.ToString(0));
                if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + iforms+".xml"))
                {
                    ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + iforms + ".xml");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string st = ds.Tables[0].Rows[i]["Input"].ToString();
                        this.DGV1.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = false;
                    }
                }

            }
            catch
            {
                MessageBox.Show("Defauft Profile Not Create" , "Error");
            }
            Task.Factory.StartNew(() =>
            {

                //Thread.Sleep(100);



                if (this.InvokeRequired)
                {
                    this.BeginInvoke((ThreadStart)delegate() { defaultLoadfun(); });

                    return;
                }
                else
                {
                    this.BeginInvoke((ThreadStart)delegate() { defaultLoadfun(); });
                }


            });
        }

        //SpreadTable.Clear();
        //        DataSet ds_set = new DataSet();
        //        ds_set.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Lastvalue.xml");
        //        SpreadTable.ReadXml(opn.FileName);
         private void defaultLoadfun()
        {
            try
            {
                var config = new Config { GroupName = null };
                string iforms = Convert.ToString(config.GetValue("PF_Profile", Convert.ToString(0)));
                //if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + "FOFODefault.xml"))
                if (File.Exists(iforms))
                {
                    SpreadTable.Clear();
                    DataSet ds_set = new DataSet();
                    ds_set.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Lastvalue.xml");
                    //SpreadTable.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + "FOFODefault.xml");
                    SpreadTable.ReadXml(iforms);
                    for (int i = 0; i < ds_set.Tables[0].Rows.Count; i++)
                    {
                        DGV1.Rows[i].Cells["BNSFDIFF"].Value = ds_set.Tables[0].Rows[i]["BNSFDIFF"];
                        DGV1.Rows[i].Cells["BFSNDIFF"].Value = ds_set.Tables[0].Rows[i]["BFSNDIFF"];
                        DGV1.Rows[i].Cells["BNSFMNQ"].Value = ds_set.Tables[0].Rows[i]["BNSFMNQ"];
                        DGV1.Rows[i].Cells["BFSNMNQ"].Value = ds_set.Tables[0].Rows[i]["BFSNMNQ"];
                        DGV1.Rows[i].Cells["BNSFMXQ"].Value = ds_set.Tables[0].Rows[i]["BNSFMXQ"];
                        DGV1.Rows[i].Cells["BFSNMXQ"].Value = ds_set.Tables[0].Rows[i]["BFSNMXQ"];
                        DGV1.Rows[i].Cells["TICKS"].Value = ds_set.Tables[0].Rows[i]["TICKS"];
                    }
                    for (int i = 0; i < SpreadTable.Rows.Count; i++)
                    {
                        UDP_Reciever.Instance.Subscribe = Convert.ToInt32(SpreadTable.Rows[i]["Token1"].ToString());
                        UDP_Reciever.Instance.Subscribe = Convert.ToInt32(SpreadTable.Rows[i]["Token2"].ToString());
                        portFolioCounter++;
                    }
                    if (DGV1.Rows.Count == 0)
                    { return; }
                    portFolioCounter = Convert.ToInt32(SpreadTable.Compute("MAX(PF)", "")) + 1;
                }
            }
             catch(Exception ex)
            {

            }
         }

        private void DGV1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
           
        }

        private void DGV1_CellClick(object sender, DataGridViewCellEventArgs e)
        {        
            btnApply_Click(e.RowIndex, e.ColumnIndex);
        }


        private void btnApply_Click(int RowIndex, int ColumnIndex)
        {
            if (RowIndex <= -1)
                return;

            //if (WTC_txt.Text == "")
            //{
            //    WTC_txt.BackColor = Color.Red;
            //    return;
            //}
            //else
            //{
            //    WTC_txt.BackColor = Color.White;
            //}
            if (DGV1.Rows[RowIndex].Cells[ColumnIndex] is DataGridViewButtonCell)
            {

                //MessageBox.Show( DGV1.Rows[RowIndex].Cells["D_B"].Value.ToString());
                using (frmDiff _frmDIff = new frmDiff())
                {
                   
                    
                    _frmDIff._FOPairDiff.BFSNDIFF = Convert.ToDouble(DGV1.Rows[RowIndex].Cells["BFSNDIFF"].Value);
                    _frmDIff._FOPairDiff.BNSFDIFF = Convert.ToDouble(DGV1.Rows[RowIndex].Cells["BNSFDIFF"].Value);

                    _frmDIff._FOPairDiff.BNSFMNQ = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["BNSFMNQ"].Value);
                    _frmDIff._FOPairDiff.BFSNMNQ = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["BFSNMNQ"].Value);

                    _frmDIff._FOPairDiff.BNSFMXQ = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["BNSFMXQ"].Value);
                    _frmDIff._FOPairDiff.BFSNMXQ = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["BFSNMXQ"].Value);

                    _frmDIff._FOPairDiff.PORTFOLIONAME = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["PF"].Value);
                    _frmDIff._FOPairDiff.TokenNear = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["Token1"].Value);
                    _frmDIff._FOPairDiff.TokenFar = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["Token2"].Value);
                    _frmDIff._FOPairDiff.Depth_Best =Convert.ToBoolean(DGV1.Rows[RowIndex].Cells["D_B"].Value) == true ? Convert.ToInt16(db.Best) : Convert.ToInt16(db.Depth);
                    //decimal d;
                    //if (decimal.TryParse(DGV1.Rows[RowIndex].Cells["TICKS"].Value.ToString(), out d))
                    //{
                    //    MessageBox.Show("Please insert valid value");
                    //    return;
                    //}
                    if (Convert.ToDouble(DGV1.Rows[RowIndex].Cells["TICKS"].Value) % 1 != 0)
                    {
                       MessageBox.Show("Please insert valid value");
                        return;
                    }

                    TradeTrac trd_struct = new TradeTrac();
                    trd_struct.PF_ID = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["PF"].Value);
                    
                    trd_struct.Given_Price_Buy = Convert.ToDouble(DGV1.Rows[RowIndex].Cells["BNSFDIFF"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["BNSFDIFF"].Value);
                    trd_struct.Given_Price_Sell = Convert.ToDouble(DGV1.Rows[RowIndex].Cells["BFSNDIFF"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["BFSNDIFF"].Value);
                    //trd_struct.B_S = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["PF"].Value);
                    Global.Instance.TradeTrac_dict.AddOrUpdate(Convert.ToInt32(DGV1.Rows[RowIndex].Cells["Token1"].Value), trd_struct, (k, v1) => trd_struct);

                    TradeTrac trd_struct2 = new TradeTrac();
                    trd_struct2.PF_ID = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["PF"].Value);
                    //trd_struct2.B_S = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["PF"].Value);

                    trd_struct2.Given_Price_Buy = Convert.ToDouble(DGV1.Rows[RowIndex].Cells["BNSFDIFF"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["BNSFDIFF"].Value);
                    trd_struct2.Given_Price_Sell = Convert.ToDouble(DGV1.Rows[RowIndex].Cells["BFSNDIFF"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["BFSNDIFF"].Value);
                    Global.Instance.TradeTrac_dict.AddOrUpdate(Convert.ToInt32(DGV1.Rows[RowIndex].Cells["Token2"].Value), trd_struct2, (k, v1) => trd_struct2);
                   


                    _frmDIff._FOPairDiff.TickCount =(int)Math.Round(Convert.ToDouble(DGV1.Rows[RowIndex].Cells["TICKS"].Value));
                    byte[] buffer = DataPacket.RawSerialize(_frmDIff._FOPairDiff);
                    NNFHandler.Instance.Publisher( MessageType.FOPAIRDIFF , buffer);
                    Global.Instance.write = true;
                    if (Convert.ToBoolean(DGV1.Rows[RowIndex].Cells["Enable"].Value) == true)
                    {
                        DGV1.Rows[RowIndex].Cells["Enable"].Style.BackColor = Color.Green;
                    }
                     //Task.Factory.StartNew(() => applyFun());
                }
            }
        }

        private void applyFun()
        {
            if (DGV1.Rows.Count == 0)
                return;
            SpreadTable = (DataTable)DGV1.DataSource;
            SpreadTable.WriteXml(Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + "FOFODefault.xml");
            DataTable dt_save = new DataTable("lastvalue");
            dt_save.Columns.Add("BNSFDIFF", typeof(Double));
            dt_save.Columns.Add("BFSNDIFF", typeof(Double));
            dt_save.Columns.Add("BNSFMNQ", typeof(int));
            dt_save.Columns.Add("BFSNMNQ", typeof(int));
            dt_save.Columns.Add("BNSFMXQ", typeof(int));
            dt_save.Columns.Add("BFSNMXQ", typeof(int));
            dt_save.Columns.Add("TICKS", typeof(int));
            foreach (DataGridViewRow row in DGV1.Rows)
            {
                DataRow dRow = dt_save.NewRow();
                dRow["BNSFDIFF"] = row.Cells["BNSFDIFF"].Value == null ? 0 : row.Cells["BNSFDIFF"].Value;
                dRow["BFSNDIFF"] = row.Cells["BFSNDIFF"].Value == null ? 0 : row.Cells["BFSNDIFF"].Value;
                dRow["BNSFMNQ"] = row.Cells["BNSFMNQ"].Value == null ? 0 : row.Cells["BNSFMNQ"].Value;
                dRow["BFSNMNQ"] = row.Cells["BFSNMNQ"].Value == null ? 0 : row.Cells["BFSNMNQ"].Value;
                dRow["BNSFMXQ"] = row.Cells["BNSFMXQ"].Value == null ? 0 : row.Cells["BNSFMXQ"].Value;
                dRow["BFSNMXQ"] = row.Cells["BFSNMXQ"].Value == null ? 0 : row.Cells["BFSNMXQ"].Value;
                dRow["TICKS"] = row.Cells["TICKS"].Value == null || row.Cells["TICKS"].Value == DBNull.Value ? 0 :(int)Math.Round(Convert.ToDouble(row.Cells["TICKS"].Value));
                dt_save.Rows.Add(dRow);
            }


            dt_save.WriteXml(Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + "FOFO.xml");
        }

        private void btnStartAll_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in DGV1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = true;// !(chk.Value == null ? false : (bool)chk.Value); //because chk.Value is initialy null
            }

            btnStopAll.Enabled = true;
            btnStartAll.Enabled = false;
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {

            byte[] buffer = DataPacket.RawSerialize(new C_LotIN());
            NNFHandler.Instance.Publisher(MsgType.STOP_ALL, buffer);
            Global.Instance.stop_all = true;
            foreach (DataGridViewRow VARIABLE in DGV1.Rows)
            {
                DataGridViewCheckBoxCell cb = (VARIABLE.Cells["Enable"]) as DataGridViewCheckBoxCell;

                cb.Value = false;
            }
            btnStopAll.Enabled = false;

            btnStartAll.Enabled = true;
            Global.Instance.stop_all = false;
        }
        private bool valueChanged;
        public virtual bool EditingControlValueChanged
        {
            get
            {
                return this.valueChanged;
            }
            set
            {
                this.valueChanged = value;
            }
        }
        private void DGV1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DGV1.IsCurrentCellDirty)
            {
              
              DGV1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DGV1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1) return;

            

            if (DGV1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewCheckBoxCell)
            {
                if (Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells[e.ColumnIndex].ColumnIndex) == 1)
                    return;

            DataGridViewCheckBoxCell checkCell =(DataGridViewCheckBoxCell)DGV1.Rows[e.RowIndex].Cells["Enable"];
           // DataGridViewCheckBoxCell checkCell2 = (DataGridViewCheckBoxCell)DGV1.Rows[e.RowIndex].Cells["D_B"];
          

            FOPAIR v;
            if (checkCell.Value == null)
                return;

            if ((bool) checkCell.Value == true)
            {
                
                byte[] buffer = DataPacket.RawSerialize(v=new FOPAIR()
                {
                    PORTFOLIONAME = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["PF"].Value),
                    TokenNear = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token1"].Value),
                    TokenFar = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token2"].Value)
                });
              //  Global.Instance.Fopairbool = true;
                NNFHandler.Instance.Publisher(MessageType.FOPAIR, buffer);

                NNFHandler.Instance._subscribeSocket.Subscribe(
                                           BitConverter.GetBytes((short)MessageType.ORDER).Concat(BitConverter.GetBytes(Global.Instance.ClientId).Concat(BitConverter.GetBytes(Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["PF"].Value)))).ToArray());
              //  NNFHandler.Instance._subscribeSocket.Subscribe(BitConverter.GetBytes(Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["PF"].Value)).Concat(
                    //                      BitConverter.GetBytes((short)MessageType.ORDER)).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).ToArray());

                if (Holder._DictLotSize.ContainsKey(v.TokenNear) == false || v.TokenNear != 0)
                {
                    Holder._DictLotSize.TryAdd(v.TokenNear, new Csv_Struct()
                    {
                        lotsize = CSV_Class.cimlist.Where(q => q.Token == v.TokenNear).Select(a => a.BoardLotQuantity).First()
                    }
                    );
                }

                if (Holder._DictLotSize.ContainsKey(v.TokenFar) == false || v.TokenFar != 0)
                {
                    Holder._DictLotSize.TryAdd(v.TokenFar, new Csv_Struct()
                    {
                        lotsize = CSV_Class.cimlist.Where(q => q.Token == v.TokenFar).Select(a => a.BoardLotQuantity).First()
                    }
                    );
                }

                btnStopAll.Enabled = true;
            }
            else
            {
                //Global.Instance.Fopairbool = false;
                DGV1.Rows[e.RowIndex].Cells["Enable"].Style.BackColor = Color.Red;
                if (Global.Instance.stop_all == true)
                    return;
                byte[] buffer = DataPacket.RawSerialize(v = new FOPAIR()
                {
                    PORTFOLIONAME = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["PF"].Value),
                    TokenNear = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token1"].Value),
                    TokenFar = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token2"].Value)
                });
                
                
                NNFHandler.Instance.Publisher( MessageType.FOPAIRUNSUB, buffer);
                NNFHandler.Instance._subscribeSocket.Unsubscribe(
                                          BitConverter.GetBytes((short)MessageType.ORDER).Concat(BitConverter.GetBytes(Global.Instance.ClientId).Concat(BitConverter.GetBytes(Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["PF"].Value)))).ToArray());
             
                //if (Holder._DictLotSize.ContainsKey(v.TokenNear) == false || v.TokenNear != 0)
                //{
                //    Csv_Struct o = new Csv_Struct();
                //    Holder._DictLotSize.TryRemove(v.TokenNear, out o);                   
                //}

                //if (Holder._DictLotSize.ContainsKey(v.TokenFar) == false || v.TokenFar != 0)
                //{
                //    Csv_Struct o = new Csv_Struct();
                //    Holder._DictLotSize.TryRemove(v.TokenFar, out o);
                //}
            }
            
            }
        }

        private void DGV1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode ==  Keys.Delete)
            {
                if (DGV1.RowCount <= 0)
                    return;
                
                DataGridViewRow VARIABLE = DGV1.SelectedRows[0];
               
                    DataGridViewCheckBoxCell cb = (VARIABLE.Cells["Enable"]) as DataGridViewCheckBoxCell;

                    if (Convert.ToBoolean(cb.Value) == true)
                    {
                        MessageBox.Show("unsubscribe the Token");
                        return;
                    }
                

                //if (Global.Instance.Fopairbool == true)
                //{
                    
                //}
                
                //--portFolioCounter;
                DGV1.Rows.RemoveAt(DGV1.SelectedRows[0].Index);

                DataTable dt = SpreadTable;
            }
          else  if (e.KeyCode == Keys.Enter)
            {
                DataGridView _dgLoc = sender as DataGridView;
                if (_dgLoc.CurrentCell.EditedFormattedValue.ToString() == "Apply")
                {
                  btnApply_Click(_dgLoc.CurrentRow.HeaderCell.RowIndex, _dgLoc.CurrentRow.Cells["Apply"].ColumnIndex);
                }
            }
        }

        private void DGV1_SelectionChanged(object sender, EventArgs e)
        {
          //  DGV1.ClearSelection();
        }

        private void DGV1_CurrentCellDirtyStateChanged_1(object sender, EventArgs e)
        {
            if (!this.valueChanged)
            {             
                this.valueChanged = true;
                this.DGV1.NotifyCurrentCellDirty(true);
            }
        }
        private void DGV1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           // if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7)// "BNSFDIFF")
                if (e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 8)// "BNSFDIFF")
            {                
                Insert_txt.Show();
                strv = e.ColumnIndex.ToString() + "," + e.RowIndex.ToString();
                Insert_txt.Text =Convert.ToString( DGV1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                DGV1.Controls.Add(Insert_txt);
                Insert_txt.Location = this.DGV1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                                        
                Insert_txt.Width = DGV1.Columns[0].Width;
                Insert_txt.Focus();             
            }           
        }
       

        private void DGV1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7)// "BNSFDIFF")
                if (e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 8)
                {
                if (Information.IsNumeric(Insert_txt.Text) == true)
                DGV1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Insert_txt.Text;
                Insert_txt.Focus();
                Insert_txt.Hide(); 
            }
           }
            catch { }
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("Enter key pressed");
            }
        }

        private void txt_MouseClick(object sender, MouseEventArgs e)
        {
            Insert_txt.Focus();
        }

        private void DGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DGV1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
           // if (!Information.IsNumeric(txt.Text) && Convert.ToDouble(txt.Text) % 1 != 0)
            if (!Information.IsNumeric(Insert_txt.Text))
            {

                if (Insert_txt.Text.Length > 1)
                {
                    MessageBox.Show("Please Insert Numeric Value", "Information");
                    Insert_txt.Clear();
                    Insert_txt.Text = "0";
                }
                else
                {
                  Insert_txt.Focus();
                }


            }
        }

        private void txt_Leave(object sender, EventArgs e)
        {
          //  if (!Information.IsNumeric(txt.Text) && Convert.ToDouble(txt.Text)%1!=0)
            if (!Information.IsNumeric(Insert_txt.Text))
            {

                if (Insert_txt.Text.Length > 0)
                {
                    MessageBox.Show("Please Insert Numeric Value", "Information");
                    Insert_txt.Clear();
                    Insert_txt.Text = "0";
                }
                else
                    Insert_txt.Focus();
            }
        }

        private void ApplyAll_Click(object sender, EventArgs e)
        {
            //if (DGV1.Rows[RowIndex].Cells[ColumnIndex] is DataGridViewButtonCell)

            //if (WTC_txt.Text == "")
            //{
            //    WTC_txt.BackColor = Color.Red;
            //    return;
            //}
            //else
            //{
            //    WTC_txt.BackColor = Color.White;
            //}
            foreach(DataGridViewRow dr in DGV1.Rows)
            {
                   using (frmDiff _frmDIff = new frmDiff())
                {
                    _frmDIff._FOPairDiff.BFSNDIFF = Convert.ToDouble(dr.Cells["BFSNDIFF"].Value);
                    _frmDIff._FOPairDiff.BNSFDIFF = Convert.ToDouble(dr.Cells["BNSFDIFF"].Value);
                    _frmDIff._FOPairDiff.BNSFMNQ = Convert.ToInt32(dr.Cells["BNSFMNQ"].Value);
                    _frmDIff._FOPairDiff.BFSNMNQ = Convert.ToInt32(dr.Cells["BFSNMNQ"].Value);
                    _frmDIff._FOPairDiff.BNSFMXQ = Convert.ToInt32(dr.Cells["BNSFMXQ"].Value);
                    _frmDIff._FOPairDiff.BFSNMXQ = Convert.ToInt32(dr.Cells["BFSNMXQ"].Value);
                    _frmDIff._FOPairDiff.PORTFOLIONAME = Convert.ToInt32(dr.Cells["PF"].Value);
                    _frmDIff._FOPairDiff.TokenNear = Convert.ToInt32(dr.Cells["Token1"].Value);
                    _frmDIff._FOPairDiff.TokenFar = Convert.ToInt32(dr.Cells["Token2"].Value);
                    _frmDIff._FOPairDiff.Depth_Best = Convert.ToBoolean(dr.Cells["D_B"].Value) == true ? Convert.ToInt16(db.Best) : Convert.ToInt16(db.Depth);
                    //decimal d;
                    //if (decimal.TryParse(DGV1.Rows[RowIndex].Cells["TICKS"].Value.ToString(), out d))
                    //{
                    //    MessageBox.Show("Please insert valid value");
                    //    return;
                    //}
                    if (Convert.ToDouble(dr.Cells["TICKS"].Value) % 1 != 0)
                    {
                        MessageBox.Show("Please insert valid value");
                        return;
                    }
                    if (Convert.ToBoolean(dr.Cells["Enable"].Value) == true)
                    {
                        dr.Cells["Enable"].Style.BackColor = Color.Green;
                    }
                   
                    _frmDIff._FOPairDiff.TickCount = (int)Math.Round(Convert.ToDouble(dr.Cells["TICKS"].Value));
                    byte[] buffer = DataPacket.RawSerialize(_frmDIff._FOPairDiff);
                    NNFHandler.Instance.Publisher(MessageType.FOPAIRDIFF, buffer);
                    Global.Instance.write = true;
                    //Task.Factory.StartNew(() => applyFun());
                }
            }
           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (Global.Instance.Best_Bid == true)
            {
                foreach (DataGridViewRow row in DGV1.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[1];
                    chk.Value = true;// !(chk.Value == null ? false : (bool)chk.Value); //because chk.Value is initialy null
                }
                Global.Instance.Best_Bid = false;
            }
            else
            {
                foreach (DataGridViewRow row in DGV1.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[1];
                    chk.Value = false;// !(chk.Value == null ? false : (bool)chk.Value); //because chk.Value is initialy null
                }
                Global.Instance.Best_Bid = true;
            }
        }

        private void WTC_txt_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.WTC_txt.Text == "" || Convert.ToInt32(this.WTC_txt.Text) <= 0)
            //    {
            //        this.WTC_txt.BackColor = Color.Red;
            //        return;
            //    }
            //    else
            //    {
            //        this.WTC_txt.BackColor = Color.White;
            //    }
            //}
            //catch(Exception ex)
            //{

            //}
           
           
        }
    }

    public class Data1
    {
        public int Tok1;
        public int Tok2;
        public int pricediff;

    }
}


/*
DataRow[] drow = SpreadTable.Select("PF = '"+DGV1.SelectedRows[0].Cells["PF"].Value+"'");
                drow[0]["BFSNDIFF"] = DGV1.SelectedRows[0].Cells["BFSNDIFF"].Value;
                drow[0]["BFSNDIFF"] = DGV1.SelectedRows[0].Cells["BFSNDIFF"].Value;

                drow[0]["BNSFMNQ"] = DGV1.SelectedRows[0].Cells["BNSFMNQ"].Value;
                drow[0]["BFSNMNQ"] = DGV1.SelectedRows[0].Cells["BFSNMNQ"].Value;

                drow[0]["BNSFMXQ"] = DGV1.SelectedRows[0].Cells["BNSFMXQ"].Value;
                drow[0]["BFSNMXQ"] = DGV1.SelectedRows[0].Cells["BFSNMXQ"].Value;
*/