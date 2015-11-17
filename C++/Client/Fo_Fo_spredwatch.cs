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
using Microsoft.VisualBasic;
using System.Xml;
using System.Threading;

namespace Client
{
    public partial class fofospreadwatch : Form
    {
       
        private readonly Dictionary<int, DataGridViewRow> _mwatchDict2 = new Dictionary<int, DataGridViewRow>();
        private readonly Dictionary<int, Data1> _DataDict2 = new Dictionary<int, Data1>();
        private DataGridViewCellStyle _makeItBlack;
        private DataGridViewCellStyle _makeItBlue;
        private DataGridViewCellStyle _makeItRed;
        public  int str_price1;
        internal DataTable SpreadTable;
        Scroller.IniFile _inifile = null;
       // TextBox txt = new TextBox();

        //private static readonly Fo_Fo_mktwatch instance = new Fo_Fo_mktwatch();
        //public static Fo_Fo_mktwatch Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}
        // this.gvw1.Columns[0].HeaderText = "The new header";
       
        private int portFolioCounter = 1;
        public fofospreadwatch()
        {
            InitializeComponent();
            _inifile = new Scroller.IniFile(Application.StartupPath+ Path.DirectorySeparatorChar+"lastcloseini.ini");

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

        }

        public void TRADE_CONFIRMATION_TR(byte[] buffer) //-- 20222  
        {
            var obj = (MS_TRADE_CONFIRM_TR)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_CONFIRM_TR));
          

            try
            {
              //  Client.LogWriterClass.logwritercls.logs("TRADE_CONFIRMATION_TR", "Value Check update in gridview");
               
                DataGridViewRow row = DGV1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Token1"].Value.ToString().Equals(Convert.ToString(IPAddress.HostToNetworkOrder(obj.Token)))).First();
                if (row ==  null)
                {
                    return;
                }
                if (IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1)
                {
                    DGV1.Rows[row.Index].Cells["ATP(B)"].Value = Global.Instance.OrdetTable.AsEnumerable().Where(ab => ab.Field<string>("status") == "Traded" && ab.Field<string>("Buy_SellIndicator") == "BUY" && ab.Field<string>("TokenNo") == IPAddress.HostToNetworkOrder(obj.Token).ToString()).Sum(av => Convert.ToDouble(av.Field<string>("Price")) / Convert.ToDouble(av.Field<string>("Volume"))).ToString();
                }
                else
                {
                    DGV1.Rows[row.Index].Cells["ATP(S)"].Value = Global.Instance.OrdetTable.AsEnumerable().Where(ab => ab.Field<string>("status") == "Traded" && ab.Field<string>("Buy_SellIndicator") == "SELL" && ab.Field<string>("TokenNo") == IPAddress.HostToNetworkOrder(obj.Token).ToString() ).Sum(av => Convert.ToDouble(av.Field<string>("Price")) / Convert.ToDouble(av.Field<string>("Volume"))).ToString();
                }

               
            }
            catch (Exception ex)
            {
               // Client.LogWriterClass.logwritercls.logs("ErrorValuecheckavg", "Value Check update in gridview" + ex.Message);
            }
            Task.Factory.StartNew(() => Fillqty_ingrd(IPAddress.HostToNetworkOrder(obj.Token), IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator),obj));
          

        }

        
        public void Fillqty_ingrd(int tokenno, int buy_sell, MS_TRADE_CONFIRM_TR obj)
        {
            try
            {
                string strbuysell = buy_sell ==1 ? "Buy":"Sell" ;
                object ob = new object();
                lock (ob)
                {                   
                    DataGridViewRow row = DGV1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Token1"].Value.ToString().Equals(Convert.ToString(tokenno))).FirstOrDefault();
                    if (row == null)
                    {          
                        return;
                    }
                    string _neartokenside = DGV1.Rows[row.Index].Cells["Tok1B_S"].Value.ToString();
                   //LogWriterClass.logwritercls.logs("Fillqty_ingrd", strbuysell);
                    if (_neartokenside.Trim() == strbuysell)
                    {
                        if (DGV1.Rows[row.Index].Cells["TRDQTY(B)"].Value == null || Convert.ToString(DGV1.Rows[row.Index].Cells["TRDQTY(B)"].Value) == "")
                        {
                            DGV1.Rows[row.Index].Cells["TRDQTY(B)"].Value = 0;
                        }
                        DGV1.Rows[row.Index].Cells["TRDQTY(B)"].Value = Convert.ToInt32(DGV1.Rows[row.Index].Cells["TRDQTY(B)"].Value) + 1;

                       
                    }
                    else
                    {
                        if (DGV1.Rows[row.Index].Cells["TRDQTY(S)"].Value == null || Convert.ToString(DGV1.Rows[row.Index].Cells["TRDQTY(S)"].Value) == "")
                        {
                            DGV1.Rows[row.Index].Cells["TRDQTY(S)"].Value = 0;
                        }
                        DGV1.Rows[row.Index].Cells["TRDQTY(S)"].Value = Convert.ToInt32(DGV1.Rows[row.Index].Cells["TRDQTY(S)"].Value) + 1;

                       
                    }


              
                }
            } 
            catch (Exception ex)
            {
               // Client.LogWriterClass.logwritercls.logs("ErrorValue_check", "Value Check update in gridview" + ex.Message);

            }
        }
        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            if (portFolioCounter==0)
             portFolioCounter = 1;
            using (AddSpreadToken _AddToken = new AddSpreadToken())
            {
                _AddToken.txtpfName.Text = portFolioCounter.ToString();
                _AddToken.lblPfName.Visible = true;
                _AddToken.txtpfName.Visible = true;
                _AddToken.Text = "Add Near Month Token";
                _AddToken.button1.Text = "Add Token";
                if (_AddToken.ShowDialog() == DialogResult.OK)
                {
                     DataRow dr = SpreadTable.NewRow();

                    dr["PF"] = _AddToken._objOut2.PFName;
                    dr["NEAR"] = _AddToken._objOut2.Desc1;
                    dr["Token1"] = _AddToken._objOut2.Token1;

                    dr["FAR"] = _AddToken._objOut2.Desc2;
                    dr["Token2"] = _AddToken._objOut2.Token2;
                    dr["tok3"] = _AddToken._objOut2.Desc3;
                    dr["Token3"] = _AddToken._objOut2.Token3;
                    dr["Token4"] = _AddToken._objOut2.Token4;
                    dr["Calc_type"] = _AddToken._objOut2.Calc_type;

                    dr["ratio1"] = Convert.ToString(_AddToken._objOut2.ratio1) == "" ? "0" : Convert.ToString(_AddToken._objOut2.ratio1);
                    dr["ratio2"] = Convert.ToString(_AddToken._objOut2.ratio2) == "" ? "0" : Convert.ToString(_AddToken._objOut2.ratio2);
                    dr["ratio3"] = Convert.ToString(_AddToken._objOut2.ratio3) == "" ? "0" : Convert.ToString(_AddToken._objOut2.ratio3);
                    dr["ratio4"] = Convert.ToString(_AddToken._objOut2.ratio4) == "" ? "0" : Convert.ToString(_AddToken._objOut2.ratio4);

                    dr["Tok1B_S"] = Convert.ToString(_AddToken._objOut2.buy_sell) == "" ? "0" : Convert.ToString(_AddToken._objOut2.buy_sell);
                    dr["Tok2B_S"] = Convert.ToString(_AddToken._objOut2.buy_sell2) == "" ? "0" : Convert.ToString(_AddToken._objOut2.buy_sell2);
                    dr["Tok3B_S"] = Convert.ToString(_AddToken._objOut2.buy_sell3) == "" ? "0" : Convert.ToString(_AddToken._objOut2.buy_sell3);
                    dr["Tok4B_S"] = Convert.ToString(_AddToken._objOut2.buy_sell4) == "" ? "0" : Convert.ToString(_AddToken._objOut2.buy_sell4);

                    dr["tok1inst"] = _AddToken._objOut2.tok1_inst;
                    dr["tok2inst"] = _AddToken._objOut2.tok2_inst;
                   
                    dr["F_BID"] = 0;
                    dr["F_ASK"] = 0;

                    dr["F_LTP"] = 0;
                    dr["NBID"] = 0;

                    dr["NASK"] = 0;
                    dr["NLTP"] = 0;

                    dr["FBID"] = 0;
                    dr["FASK"] = 0;

                    dr["FLTP"] = 0;

                  //  dr["ratio2"] = Convert.ToString(_AddToken._objOut2.ratio2) == "" ? "0" : Convert.ToString(_AddToken._objOut2.ratio2);

                   
                    SpreadTable.Rows.Add(dr);
                    //dr["BFSNDIFF"] =  0.0000;
                    //dr["BNSFDIFF"]=0.0000;

                    //dr["BNSFMNQ"] = 0.0000;
                    //dr["BFSNMNQ"] = 0.0000;

                    //dr["BNSFMXQ"] = 0.0000;
                    //dr["BFSNMXQ"] = 0.0000;

                    DGV1.Rows[DGV1.Rows.Count - 1].Cells["BUYPRICE"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count - 1].Cells["SELLPRICE"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count - 1].Cells["ORDQTY(B)"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count - 1].Cells["ORDQTY(S)"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count - 1].Cells["TOTALQTY(B)"].Value = 0.00;
                    DGV1.Rows[DGV1.Rows.Count-1].Cells["TOTALQTY(S)"].Value = 0.00;


                   
                    
                    UDP_Reciever.Instance.Subscribe = _AddToken._objOut2.Token1;
                    UDP_Reciever.Instance.Subscribe = _AddToken._objOut2.Token2;
                    UDP_Reciever.Instance.Subscribe = _AddToken._objOut2.Token3;
                    UDP_Reciever.Instance.Subscribe = _AddToken._objOut2.Token4;

               


                    
                 //   FOPAIRDIFF
                    
                    portFolioCounter++;
                   
                }
   
            }

        
            
        }

        private void btnprofile_Click(object sender, EventArgs e)
        {
            var frmprf = new frmProfile();

            foreach (DataGridViewColumn dc in DGV1.Columns)
            {
             //   frmprf.lbxPrimary.Items.Add(dc.HeaderText);
                if (!frmprf.lbxSecondary.Items.Contains(dc.HeaderText))
                {
                    frmprf.lbxPrimary.Items.Add(dc.HeaderText);
                   
                }
               
            }
            if (frmprf.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewColumn dc in DGV1.Columns)
                {
                    this.DGV1.Columns[dc.HeaderText.Replace(" ", "")].Visible = true;
                }
                String GetProfileName = frmprf.GetProfileName();

                DataSet ds = new DataSet();
                ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + GetProfileName + ".xml");
                if (ds.Tables.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                     string st = ds.Tables[0].Rows[i]["Input"].ToString();
                    this.DGV1.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ","")].Visible = false;
                }
            }
            else
            {
                String GetProfileName = frmprf.GetProfileName();

                DataSet ds = new DataSet();
                ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + "MarketCol.xml");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string st = ds.Tables[0].Rows[i]["Input"].ToString();
                    this.DGV1.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = true;
                }
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
                
                var rowlist = SpreadTable.Rows.Cast<DataRow>().Where(x => Convert.ToInt32(x["Token1"]) == Stat.Parameter.Token || Convert.ToInt32(x["Token2"]) == Stat.Parameter.Token).ToList();// || Convert.ToInt32(x["Token3"]) == Stat.Parameter.Token).ToList();//there is doubt in leg4.......
                try
                {
                    
                    foreach (var i in rowlist)
                    {
                        if (DGV1.Rows.Count == 0)
                        {
                            return;
                        }



                        if (Convert.ToInt32(i["Token1"]) == Stat.Parameter.Token)
                        {


                            i["NBID"] = Math.Round(Convert.ToDouble(Stat.Parameter.MAXBID) / 100, 4);
                            i["NASK"] = Math.Round(Convert.ToDouble(Stat.Parameter.MINASK) / 100, 4);
                            i["NLTP"] = Math.Round(Convert.ToDouble(Stat.Parameter.LTP) / 100, 4);

                            i["t1"] = GetExpectedProdPrice(i["Tok1B_S"].ToString(), Stat, Convert.ToInt32(i["ratio1"]));
                            i["t3"] = GetExpectedProdPrice(i["Tok1B_S"].ToString(), Stat, Convert.ToInt32(i["ratio1"]), true);


                            i["tok1_cost"] = Convert.ToString(i["Tok1B_S"]) == "Buy" ?
                                i["tok1inst"].ToString() == "OPTIDX"
                                ? Convert.ToDouble(i["NBID"]) * Convert.ToInt32(i["ratio1"]) * 2 * 0.0007 :
                                i["tok1inst"].ToString() == "OPTSTK" ? Convert.ToDouble(i["NBID"]) * Convert.ToInt32(i["ratio1"]) * 2 * 0.0007
                                : Convert.ToDouble(i["NBID"]) * Convert.ToInt32(i["ratio1"]) * 2 * 0.0009 :
                                //else

                                 i["tok1inst"].ToString() == "OPTIDX"
                                ? Convert.ToDouble(i["NASK"]) * Convert.ToInt32(i["ratio1"]) * 2 * 0.0007 :
                                i["tok1inst"].ToString() == "OPTSTK" ? Convert.ToDouble(i["NASK"]) * Convert.ToInt32(i["ratio1"]) * 2 * 0.0007
                                : Convert.ToDouble(i["NASK"]) * 2 * Convert.ToInt32(i["ratio1"]) * 0.0009;




                            i["cost"] = Convert.ToDouble(i["tok1_cost"] == DBNull.Value ? "0" : i["tok1_cost"]) + Convert.ToDouble(i["tok2_cost"] == DBNull.Value ? "0" : i["tok2_cost"]);


                        }
                        if (Convert.ToInt32(i["Token2"]) == Stat.Parameter.Token)
                        {

                            i["FBID"] = Math.Round(Convert.ToDouble(Stat.Parameter.MAXBID) / 100, 4);
                            i["FASK"] = Math.Round(Convert.ToDouble(Stat.Parameter.MINASK) / 100, 4);
                            i["FLTP"] = Math.Round(Convert.ToDouble(Stat.Parameter.LTP) / 100, 4);

                            i["t2"] = GetExpectedProdPrice(i["Tok2B_S"].ToString(), Stat, Convert.ToInt32(i["ratio2"]));
                            i["t4"] = GetExpectedProdPrice(i["Tok2B_S"].ToString(), Stat, Convert.ToInt32(i["ratio2"]), true);



                            i["tok2_cost"] = Convert.ToString(i["Tok2B_S"]) == "Buy" ?
                                i["tok2inst"].ToString() == "OPTIDX"
                                ? Convert.ToDouble(i["FBID"]) * Convert.ToInt32(i["ratio2"]) * 2 * 0.0007 :
                                i["tok2inst"].ToString() == "OPTSTK" ? Convert.ToDouble(i["FBID"]) * Convert.ToInt32(i["ratio2"]) * 2 * 0.0007
                                : Convert.ToDouble(i["FBID"]) * Convert.ToInt32(i["ratio2"]) * 2 * 0.0009 :
                                //else

                                 i["tok2inst"].ToString() == "OPTIDX"
                                ? Convert.ToDouble(i["NASK"]) * Convert.ToInt32(i["ratio2"]) * 2 * 0.0007 :
                                i["tok2inst"].ToString() == "OPTSTK" ? Convert.ToDouble(i["NASK"]) * Convert.ToInt32(i["ratio2"]) * 2 * 0.0007
                                : Convert.ToDouble(i["NASK"]) * Convert.ToInt32(i["ratio2"]) * 2 * 0.0009;



                            i["cost"] = Convert.ToDouble(i["tok1_cost"] == DBNull.Value ? "0" : i["tok1_cost"]) + Convert.ToDouble(i["tok2_cost"] == DBNull.Value ? "0" : i["tok2_cost"]);
                        }
                        i["CMP(B)"] = Convert.ToDouble(i["t1"] == DBNull.Value ? "0" : i["t1"]) + Convert.ToDouble(i["t2"] == DBNull.Value ? "0" : i["t2"]);
                        i["CMP(S)"] = Math.Abs(Convert.ToDouble(i["t4"] == DBNull.Value ? "0" : i["t4"]) + Convert.ToDouble(i["t3"] == DBNull.Value ? "0" : i["t3"]));


                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(" Exception " + Ex.StackTrace.ToString());
                }


            }
            catch (DataException a)
            {
                MessageBox.Show("From Live Data fill " + Environment.NewLine + a.Message);
            }
        }
        private double GetExpectedProdPrice(string BS, ReadOnlyEventArgs<FinalPrice> FP, int Ratio, bool reverse = false)
        {

            double RetVal = 0;

            if (!reverse)
            {
                // THis case calculates the price to generate buy spread
                //Math.Round(Convert.ToDouble(Stat.Parameter.MAXBID) / 100, 4)

                RetVal = BS == "Buy" ? (Math.Round(Convert.ToDouble(FP.Parameter.MINASK) / 100, 4) * Ratio * -1) : (Math.Round(Convert.ToDouble(FP.Parameter.MAXBID) / 100, 4) * Ratio);
            }
            else
            {
                // Here in case of sale actual stg with buy mode token will be sold just to make a complete trade
                RetVal = BS == "Buy" ? (Math.Round(Convert.ToDouble(FP.Parameter.MAXBID) / 100, 4) * Ratio) : (Math.Round(Convert.ToDouble(FP.Parameter.MINASK) / 100, 4) * Ratio * -1);
            }
            return RetVal;

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
            if (this.InvokeRequired)
            {
                this.BeginInvoke((ThreadStart)delegate() { applyFun(); });

                return;
            }
            else
            {
                applyFun();
            }
           
            //if (DGV1.Rows.Count > 0)
            //{
            //    if (MessageBox.Show("Want To Save This portfolio", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
            //    {

            //        btnsaveMktwatch_Click(sender, e);
            //    }
            //}
            //this.Dispose();
            //e.Cancel = true;
            //this.Hide();
        }


        private void applyFun()
        {
            if (DGV1.Rows.Count == 0)
                return;

           
           
                DataTable dsave = (DataTable)DGV1.DataSource;
                dsave.WriteXml(Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + "IOCDefault.xml");

            
            DataTable dt_save = new DataTable("lastvalue");
            dt_save.Columns.Add("ORDQTY(B)", typeof(Double));
            dt_save.Columns.Add("TOTALQTY(B)", typeof(Double));
            dt_save.Columns.Add("BUYPRICE", typeof(Double));
            dt_save.Columns.Add("SELLPRICE", typeof(Double));
            dt_save.Columns.Add("ORDQTY(S)", typeof(int));
            dt_save.Columns.Add("TOTALQTY(S)", typeof(int));
            foreach (DataGridViewRow row in DGV1.Rows)
            {
                DataRow dRow = dt_save.NewRow();
                dRow["ORDQTY(B)"] = row.Cells["ORDQTY(B)"].Value == null || row.Cells["ORDQTY(B)"].Value==DBNull.Value? 0 : row.Cells["ORDQTY(B)"].Value;
                dRow["TOTALQTY(B)"] = row.Cells["TOTALQTY(B)"].Value == null ? 0 :row.Cells["TOTALQTY(B)"].Value;
                dRow["BUYPRICE"] = row.Cells["BUYPRICE"].Value == null ? 0 :row.Cells["BUYPRICE"].Value;
                dRow["SELLPRICE"] = row.Cells["SELLPRICE"].Value == null ? 0 : row.Cells["SELLPRICE"].Value;
                dRow["ORDQTY(S)"] = row.Cells["ORDQTY(S)"].Value == null ? 0 : row.Cells["ORDQTY(S)"].Value;
                dRow["TOTALQTY(S)"] = row.Cells["TOTALQTY(S)"].Value == null ? 0 :row.Cells["TOTALQTY(S)"].Value;
                dt_save.Rows.Add(dRow);
            }
            string p_last = Application.StartupPath + Path.DirectorySeparatorChar + "Lastvalue1.xml";
            dt_save.WriteXml(p_last);
            _inifile.IniWriteValue("MasterPath", "LastUpdate", p_last);
        }

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
                DataTable dsave = (DataTable)DGV1.DataSource;
                dsave.WriteXml(savd.FileName);

            }
            DataTable dt_save = new DataTable("lastvalue");
            dt_save.Columns.Add("ORDQTY(B)", typeof(Double));
            dt_save.Columns.Add("TOTALQTY(B)", typeof(Double));
            dt_save.Columns.Add("BUYPRICE", typeof(Double));
            dt_save.Columns.Add("SELLPRICE", typeof(Double));
            dt_save.Columns.Add("ORDQTY(S)", typeof(int));
            dt_save.Columns.Add("TOTALQTY(S)", typeof(int));
            foreach (DataGridViewRow row in DGV1.Rows)
            {
                DataRow dRow = dt_save.NewRow();
              dRow["ORDQTY(B)"] = row.Cells["ORDQTY(B)"].Value==null?0:Convert.ToDouble(row.Cells["ORDQTY(B)"].Value);
              dRow["TOTALQTY(B)"] = row.Cells["TOTALQTY(B)"].Value == null ? 0 : Convert.ToDouble(row.Cells["TOTALQTY(B)"].Value);
              dRow["BUYPRICE"] = row.Cells["BUYPRICE"].Value == null ? 0 : Convert.ToDouble(row.Cells["BUYPRICE"].Value);
              dRow["SELLPRICE"] = row.Cells["SELLPRICE"].Value == null ? 0 : Convert.ToDouble(row.Cells["SELLPRICE"].Value);
              dRow["ORDQTY(S)"] = row.Cells["ORDQTY(S)"].Value == null ? 0 : Convert.ToInt32(row.Cells["ORDQTY(S)"].Value);
              dRow["TOTALQTY(S)"] = row.Cells["TOTALQTY(S)"].Value == null ? 0 : Convert.ToInt32(row.Cells["TOTALQTY(S)"].Value);
                dt_save.Rows.Add(dRow);
            }
            string p_last = Application.StartupPath + Path.DirectorySeparatorChar + "Lastvalue1.xml";
            dt_save.WriteXml(p_last);
            _inifile.IniWriteValue("MasterPath","LastUpdate",p_last ) ;
        }

        private void btnLoadMktWatch_Click(object sender, EventArgs e)
        {


            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (opn.ShowDialog() == DialogResult.OK)
            {
                SpreadTable.Clear();
                DataSet ds_set = new DataSet();
                ds_set.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Lastvalue1.xml");
                SpreadTable.ReadXml(opn.FileName);
                //  SpreadTable.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "FOWATCH.xml");
                for (int i = 0; i < ds_set.Tables[0].Rows.Count; i++)
                {
                    DGV1.Rows[i].Cells["ORDQTY(B)"].Value = ds_set.Tables[0].Rows[i]["ORDQTY(B)"] == null ? 0 : ds_set.Tables[0].Rows[i]["ORDQTY(B)"];
                    DGV1.Rows[i].Cells["TOTALQTY(B)"].Value = ds_set.Tables[0].Rows[i]["TOTALQTY(B)"] == null ? 0 : ds_set.Tables[0].Rows[i]["TOTALQTY(B)"];
                    DGV1.Rows[i].Cells["BUYPRICE"].Value = ds_set.Tables[0].Rows[i]["BUYPRICE"] == null ? 0 : ds_set.Tables[0].Rows[i]["BUYPRICE"]; 
                    DGV1.Rows[i].Cells["SELLPRICE"].Value = ds_set.Tables[0].Rows[i]["SELLPRICE"] == null ? 0 : ds_set.Tables[0].Rows[i]["SELLPRICE"];
                    DGV1.Rows[i].Cells["ORDQTY(S)"].Value = ds_set.Tables[0].Rows[i]["ORDQTY(S)"] == null ? 0 : ds_set.Tables[0].Rows[i]["ORDQTY(S)"];
                    DGV1.Rows[i].Cells["TOTALQTY(S)"].Value = ds_set.Tables[0].Rows[i]["TOTALQTY(S)"] == null ? 0 : ds_set.Tables[0].Rows[i]["TOTALQTY(S)"];
                }
            }

           // _SelectionOut _objOut = new _SelectionOut();

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
            if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "formclose1.xml"))
                return t;
            DataSet dset = new DataSet();
            dset.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "formclose1.xml");
            int[] LocationAndSize = new int[] { xForm.Location.X, xForm.Location.Y, xForm.Size.Width, xForm.Size.Height };

            try
            {
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
            //---//
            return LocationAndSize;
        }

        public static void SaveFormLocationAndSize(object sender, FormClosingEventArgs e)
        {


            Form xForm = sender as Form;

            //   ini.IniWriteValue("FOFOFORM","Location", String.Format("{0};{1};{2};{3}", xForm.Location.X, xForm.Location.Y, xForm.Size.Width, xForm.Size.Height));

            var settings = new XmlWriterSettings { Indent = true };

            XmlWriter writer = XmlWriter.Create(Application.StartupPath + Path.DirectorySeparatorChar + "formclose1.xml", settings);

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

            var AbbA = LoadFormLocationAndSize(this);
            this.Location = new Point(AbbA[0], AbbA[1]);
            this.Size = new Size(AbbA[2], AbbA[3]);

            this.FormClosing += new FormClosingEventHandler(SaveFormLocationAndSize);
           

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
            SpreadTable.Columns.Add("Token3", typeof(Int32));
            SpreadTable.Columns.Add("Token4", typeof(Int32));

            SpreadTable.Columns.Add("Tok1B_S", typeof(string));
            SpreadTable.Columns.Add("Tok2B_S", typeof(string));
            SpreadTable.Columns.Add("Tok3B_S", typeof(string));
            SpreadTable.Columns.Add("Tok4B_S", typeof(string));
            SpreadTable.Columns.Add("NEAR", typeof(String));
            SpreadTable.Columns.Add("FAR", typeof(String));
            SpreadTable.Columns.Add("tok3", typeof(String));
            SpreadTable.Columns.Add("NBID", typeof(Double));
            SpreadTable.Columns.Add("NASK", typeof(Double));
            SpreadTable.Columns.Add("NLTP", typeof(Double));
            SpreadTable.Columns.Add("FBID", typeof(Double));
            SpreadTable.Columns.Add("FASK", typeof(Double));
            SpreadTable.Columns.Add("FLTP", typeof(Double));
            SpreadTable.Columns.Add("SPREADBUY", typeof(Double));
            SpreadTable.Columns.Add("SPREADSELL", typeof(Double));

            SpreadTable.Columns.Add("tok1_cost", typeof(double));
            SpreadTable.Columns.Add("tok2_cost", typeof(double));
            SpreadTable.Columns.Add("cost", typeof(double));
            SpreadTable.Columns.Add("tok1inst", typeof(string));
            SpreadTable.Columns.Add("tok2inst", typeof(string));

            SpreadTable.Columns.Add("F_BID", typeof(Double));
            SpreadTable.Columns.Add("F_ASK", typeof(Double));
            SpreadTable.Columns.Add("F_LTP", typeof(Double));

            SpreadTable.Columns.Add("ratio1", typeof(Int32));
            SpreadTable.Columns.Add("ratio2", typeof(Int32));
            SpreadTable.Columns.Add("ratio3", typeof(Int32));
            SpreadTable.Columns.Add("ratio4", typeof(Int32));
          
            SpreadTable.Columns.Add("t1", typeof(double));
            SpreadTable.Columns.Add("t2", typeof(double));
            SpreadTable.Columns.Add("t3", typeof(double));
            SpreadTable.Columns.Add("t4", typeof(double));

            SpreadTable.Columns.Add("Calc_type", typeof(string));

            SpreadTable.Columns.Add("BUY_Price", typeof(Int32));//, "(NBID*ratio1) + (FBID*ratio2)+(F_BID*ratio3)-(NASK*ratio1)+(FASK*ratio2)+(F_ASK*ratio3)");
           // SpreadTable.Columns.Add("BUY_Price", typeof(Int32), "((NBID*ratio1) + (FBID*ratio2))-((NASK*ratio1)+(FASK*ratio2))");
            SpreadTable.Columns.Add("CMP(B)", typeof(Double));//, "FASK-NASK");
            SpreadTable.Columns.Add("NHD", typeof(Double), "FBID -NASK");
            SpreadTable.Columns.Add("CMP(S)", typeof(Double));//, "FBID-NBID");
            SpreadTable.Columns.Add("FHD", typeof(Double), "FASK-NBID");
            SpreadTable.Columns.Add("BUY_price_leg2", typeof(Double));

            DGV1.DataSource = SpreadTable;


            DGV1.Columns["Token1"].Visible = false;
            DGV1.Columns["Token2"].Visible = false;
            DGV1.Columns["Token3"].Visible = false;
            DGV1.Columns["Token4"].Visible = false;


            DGV1.Columns["FAR"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["NEAR"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["NBID"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["tok3"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["NBID"].SortMode = DataGridViewColumnSortMode.NotSortable;

            DGV1.Columns["PF"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["FLTP"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["CMP(B)"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["CMP(S)"].SortMode = DataGridViewColumnSortMode.NotSortable;

            DGV1.Columns["NHD"].SortMode = DataGridViewColumnSortMode.NotSortable;
            DGV1.Columns["FHD"].SortMode = DataGridViewColumnSortMode.NotSortable;


            SetDisplayRules(this.DGV1.Columns["PF"], "PF");

            SetDisplayRules(this.DGV1.Columns["NEAR"], "NEAR");
  

            SetDisplayRules(this.DGV1.Columns["FAR"], "FAR");
            SetDisplayRules(this.DGV1.Columns["NBID"], "N BID");
            SetDisplayRules(this.DGV1.Columns["NASK"], "N ASK");
            SetDisplayRules(this.DGV1.Columns["NLTP"], "N LTP");
            SetDisplayRules(this.DGV1.Columns["FBID"], "F BID");
            SetDisplayRules(this.DGV1.Columns["FASK"], "F ASK");   // Token2Ask
            SetDisplayRules(this.DGV1.Columns["FLTP"], "F LTP");   // Token2Ltp

            SetDisplayRules(this.DGV1.Columns["CMP(B)"], "CMP(B)");   //NearBidDiff
            SetDisplayRules(this.DGV1.Columns["NHD"], "NHD");   // NearHitDiff
            SetDisplayRules(this.DGV1.Columns["CMP(S)"], "CMP(S)");   // FarBidDiff
            SetDisplayRules(this.DGV1.Columns["FHD"], "FHD");   //FarHitDiff


            this.DGV1.Columns["CMP(B)"].DefaultCellStyle.Format = "0.00##";
            this.DGV1.Columns["NHD"].DefaultCellStyle.Format = "0.0#";
            this.DGV1.Columns["CMP(S)"].DefaultCellStyle.Format = "0.00##";
            this.DGV1.Columns["FHD"].DefaultCellStyle.Format = "0.0#";
            this.DGV1.Columns["cost"].DefaultCellStyle.Format = "0.0#";
            this.DGV1.Columns["tok1_cost"].DefaultCellStyle.Format = "0.0#";
            this.DGV1.Columns["tok2_cost"].DefaultCellStyle.Format = "0.0#";
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ORDQTY(B)",
                HeaderText = "ORDQTY(B)",


            });
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TOTALQTY(B)",
                HeaderText = "TOTALQTY(B)",


            });
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "BUYPRICE",
                HeaderText = "BUYPRICE",               

            });
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "SELLPRICE",
                HeaderText = "SELLPRICE",

            });

            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TOTALQTY(S)",

                HeaderText = "TOTALQTY(S)",

            });
         
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ORDQTY(S)",
                HeaderText = "ORDQTY(S)",

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
                Name = "TRDQTY(B)",
                HeaderText = "TRDQTY(B)",
                ReadOnly = true
            }
           );
            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TRDQTY(S)",
                HeaderText = "TRDQTY(S)",
                ReadOnly = true
            }
              );


            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ATP(B)",
                HeaderText = "ATP(B)",
                ReadOnly = true
            }
               );

            this.DGV1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ATP(S)",
                HeaderText = "ATP(S)",
                ReadOnly = true
            }
           );
            DGV1.Columns["BUYPRICE"].DefaultCellStyle.NullValue = 0.00;
            DGV1.Columns["SELLPRICE"].DefaultCellStyle.NullValue = 0.00;
            DGV1.Columns["ORDQTY(B)"].DefaultCellStyle.NullValue = 0.00;
            DGV1.Columns["ORDQTY(S)"].DefaultCellStyle.NullValue = 0.00;
            DGV1.Columns["TOTALQTY(B)"].DefaultCellStyle.NullValue = 0.00;
            DGV1.Columns["TOTALQTY(S)"].DefaultCellStyle.NullValue = 0.000;

            DGV1.Columns["BUY_Price"].ReadOnly = true;

            _makeItRed = new DataGridViewCellStyle();
            _makeItBlue = new DataGridViewCellStyle();
            _makeItBlack = new DataGridViewCellStyle();

            _makeItRed.BackColor = Color.Red;

            _makeItBlue.BackColor = Color.Blue;
            _makeItBlack.BackColor = Color.Black;
            SpreadTable.TableNewRow += new DataTableNewRowEventHandler(SpreadTable_NewRow);
          //  NNFHandler.eOrderTRADE_ERROR += Fillqty_ingrd;  

            btnStopAll.Enabled = true;
            btnStartAll.Enabled = true;

            // read only columns 
            DGV1.Columns["FAR"].ReadOnly = true;
            DGV1.Columns["NEAR"].ReadOnly = true;
            DGV1.Columns["NBID"].ReadOnly = true;
            DGV1.Columns["tok3"].ReadOnly = true;
            DGV1.Columns["NBID"].ReadOnly = true;

            DGV1.Columns["PF"].ReadOnly = true;
            DGV1.Columns["FLTP"].ReadOnly = true;
            DGV1.Columns["CMP(B)"].ReadOnly = true;
            DGV1.Columns["CMP(S)"].ReadOnly = true;

            DGV1.Columns["NHD"].ReadOnly = true;
            DGV1.Columns["FHD"].ReadOnly = true;


            DGV1.Columns["ratio1"].ReadOnly = true;
            DGV1.Columns["ratio2"].ReadOnly = true;
            DGV1.Columns["ratio3"].ReadOnly = true;
            DGV1.Columns["ratio4"].ReadOnly = true;

            DGV1.Columns["Cost"].ReadOnly = true;
            DGV1.Columns["Calc_type"].ReadOnly = true;

            //  end read only columns 


            foreach (DataGridViewColumn dc in DGV1.Columns)
            {
                this.DGV1.Columns[dc.HeaderText.Replace(" ", "")].Visible = true;
            }

            DataSet ds = new DataSet();
            if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + "nkb.xml"))
            {
                ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + "nkb.xml");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    this.DGV1.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = false;
                }
            }
            DGV1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Type controlType = DGV1.GetType();
            PropertyInfo pi = controlType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(DGV1, true, null);
           // Thread.Sleep(1000);
            if (this.InvokeRequired)
            {
                this.BeginInvoke((ThreadStart)delegate() { defaultLoadfun(); });
              //  this.BeginInvoke((ThreadStart)delegate() { Temp(); });
                return;
            }
            else
            {
                defaultLoadfun();
              //  Temp();
            }
           

        }
        private void defaultLoadfun()
        {

            if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar +System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy")+ "IOCDefault.xml"))
                return;
                SpreadTable.Clear();
                DataSet ds_set = new DataSet();
                ds_set.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Lastvalue1.xml");
                SpreadTable.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + "IOCDefault.xml");
                //  SpreadTable.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "FOWATCH.xml");
                for (int i = 0; i < ds_set.Tables[0].Rows.Count; i++)
                {
                    DGV1.Rows[i].Cells["ORDQTY(B)"].Value = ds_set.Tables[0].Rows[i]["ORDQTY(B)"] == null ? 0 : ds_set.Tables[0].Rows[i]["ORDQTY(B)"];
                    DGV1.Rows[i].Cells["TOTALQTY(B)"].Value = ds_set.Tables[0].Rows[i]["TOTALQTY(B)"] == null ? 0 : ds_set.Tables[0].Rows[i]["TOTALQTY(B)"];
                    DGV1.Rows[i].Cells["BUYPRICE"].Value = ds_set.Tables[0].Rows[i]["BUYPRICE"] == null ? 0 : ds_set.Tables[0].Rows[i]["BUYPRICE"];
                    DGV1.Rows[i].Cells["SELLPRICE"].Value = ds_set.Tables[0].Rows[i]["SELLPRICE"] == null ? 0 : ds_set.Tables[0].Rows[i]["SELLPRICE"];
                    DGV1.Rows[i].Cells["ORDQTY(S)"].Value = ds_set.Tables[0].Rows[i]["ORDQTY(S)"] == null ? 0 : ds_set.Tables[0].Rows[i]["ORDQTY(S)"];
                    DGV1.Rows[i].Cells["TOTALQTY(S)"].Value = ds_set.Tables[0].Rows[i]["TOTALQTY(S)"] == null ? 0 : ds_set.Tables[0].Rows[i]["TOTALQTY(S)"];
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
                return;
          
        }
        public void Temp()
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (opn.ShowDialog() == DialogResult.OK)
            {
                SpreadTable.Clear();
                DataSet ds_set = new DataSet();
                ds_set.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Lastvalue1.xml");
                SpreadTable.ReadXml(opn.FileName);
                //  SpreadTable.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "FOWATCH.xml");
                for (int i = 0; i < ds_set.Tables[0].Rows.Count; i++)
                {
                    DGV1.Rows[i].Cells["ORDQTY(B)"].Value = ds_set.Tables[0].Rows[i]["ORDQTY(B)"] == null ? 0 : ds_set.Tables[0].Rows[i]["ORDQTY(B)"];
                    DGV1.Rows[i].Cells["TOTALQTY(B)"].Value = ds_set.Tables[0].Rows[i]["TOTALQTY(B)"] == null ? 0 : ds_set.Tables[0].Rows[i]["TOTALQTY(B)"];
                    DGV1.Rows[i].Cells["BUYPRICE"].Value = ds_set.Tables[0].Rows[i]["BUYPRICE"] == null ? 0 : ds_set.Tables[0].Rows[i]["BUYPRICE"];
                    DGV1.Rows[i].Cells["SELLPRICE"].Value = ds_set.Tables[0].Rows[i]["SELLPRICE"] == null ? 0 : ds_set.Tables[0].Rows[i]["SELLPRICE"];
                    DGV1.Rows[i].Cells["ORDQTY(S)"].Value = ds_set.Tables[0].Rows[i]["ORDQTY(S)"] == null ? 0 : ds_set.Tables[0].Rows[i]["ORDQTY(S)"];
                    DGV1.Rows[i].Cells["TOTALQTY(S)"].Value = ds_set.Tables[0].Rows[i]["TOTALQTY(S)"] == null ? 0 : ds_set.Tables[0].Rows[i]["TOTALQTY(S)"];
                }
            }

            // _SelectionOut _objOut = new _SelectionOut();

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
        private void DGV1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            Console.WriteLine(""+anError.ToString());
        }

        private void DGV1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
         //DGV1_CellValueChanged( sender,  e);
         btnApply_Click(e.RowIndex, e.ColumnIndex);
        }


        private void btnApply_Click(int RowIndex, int ColumnIndex)
        {
            if (RowIndex <= -1)
                return;


            if (DGV1.Rows[RowIndex].Cells[ColumnIndex] is DataGridViewButtonCell)
            {
              

                double _buy = Convert.ToDouble(DGV1.Rows[RowIndex].Cells["BUYPRICE"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["BUYPRICE"].Value);
                double _sell = Convert.ToDouble(DGV1.Rows[RowIndex].Cells["SELLPRICE"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["SELLPRICE"].Value);
                FOPAIRDIFFLEG2 _frmDIff = new FOPAIRDIFFLEG2
             {

                 PORTFOLIONAME = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["PF"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["PF"].Value),
                 BuyMin = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["ORDQTY(B)"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["ORDQTY(B)"].Value),
                 BuyMax = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["TOTALQTY(B)"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["TOTALQTY(B)"].Value),               
                 SPREADBUY = Convert.ToInt32(_buy * 100),
                 SPREADSELL = Convert.ToInt32(_sell * 100),

                 SellMin = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["ORDQTY(S)"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["ORDQTY(S)"].Value),
                 
                 SellMax  = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["TOTALQTY(S)"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["TOTALQTY(S)"].Value),


                 Token1Ratio = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["ratio1"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["ratio1"].Value),
                 Token2Ratio = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["ratio2"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["ratio2"].Value),
                 Token3Ratio = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["ratio3"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["ratio3"].Value),
                 Token4Ratio = Convert.ToInt32(DGV1.Rows[RowIndex].Cells["ratio4"].Value == DBNull.Value ? "0" : DGV1.Rows[RowIndex].Cells["ratio4"].Value)


             };
                
                
                NNFHandler.Instance.Publisher(MessageType.IOCPAIRDIFF, DataPacket.RawSerialize(_frmDIff));
                Task.Factory.StartNew(() => applyFun());
                
            }
        }


        private void btnStartAll_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in DGV1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = !(chk.Value == null ? false : (bool)chk.Value); //because chk.Value is initialy null
            }

            //foreach (DataGridViewRow  VARIABLE in DGV1.Rows)
            //{
            //    DataGridViewCheckBoxCell cb = (VARIABLE.Cells["Enable"]) as DataGridViewCheckBoxCell;
                
            //    cb.Value = true;
          //  }

            btnStopAll.Enabled = true;
            btnStartAll.Enabled = false;
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow VARIABLE in DGV1.Rows)
            {
                DataGridViewCheckBoxCell cb = (VARIABLE.Cells["Enable"]) as DataGridViewCheckBoxCell;
                
                cb.Value = false;
            }
            btnStopAll.Enabled = false;
            btnStartAll.Enabled = true;
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
            DataGridViewCheckBoxCell checkCell =(DataGridViewCheckBoxCell)DGV1.Rows[e.RowIndex].Cells["Enable"];

             

            if ((bool) checkCell.Value == true)
            {
               

                  Client.Csv_Struct _lotsize = new Csv_Struct();
                FOPAIRLEG2 v;
                byte[] buffer = DataPacket.RawSerialize(v=new FOPAIRLEG2()
                {
                    //Tok1B_S
                    PORTFOLIONAME = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["PF"].Value == DBNull.Value ? "0" : DGV1.Rows[e.RowIndex].Cells["PF"].Value),
                    Token1 = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token1"].Value == DBNull.Value ? "0" : DGV1.Rows[e.RowIndex].Cells["Token1"].Value),
                    Token2 = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token2"].Value== DBNull.Value ? "0" : DGV1.Rows[e.RowIndex].Cells["Token2"].Value),
                    Token3 = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token3"].Value== DBNull.Value ? "0" : DGV1.Rows[e.RowIndex].Cells["Token3"].Value),
                   Token4 = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token4"].Value== DBNull.Value ? "0" : DGV1.Rows[e.RowIndex].Cells["Token4"].Value),
                    Token1side = Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["Tok1B_S"].Value.ToString() == "Buy" ? 1 : 2),
                    Token2side = Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["Tok2B_S"].Value.ToString() == "Buy" ? 1 : 2),
                    Token3side = Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["Tok3B_S"].Value.ToString() == "Buy" ? 1 : 2),
                    Token4side = Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["Tok4B_S"].Value.ToString() == "Buy" ? 1 : 2),
                    CALCTYPE = Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["Calc_type"].Value.ToString() == "BaseDiff" ? 2 : 1),
                });
                NNFHandler.Instance.Publisher(MessageType.IOCPAIR, buffer);

                int _buycount = 0;
                int _sellcount = 0;

                DataRow[] drr = Global.Instance.OrdetTable.Select("status='Traded'  and TokenNo='" + v.Token1 + "'  ");
                if (drr.Length > 0)
                {
                    var _v = drr.AsEnumerable().Where(a => a.Field<string>("Buy_SellIndicator") == "BUY").ToList();
                    _buycount = _v.Count();
                    var _v1 = drr.AsEnumerable().Where(a => a.Field<string>("Buy_SellIndicator") == "SELL").ToList();
                    _sellcount = _v1.Count();

                }

                //DataRow[] drr1 = Global.Instance.OrdetTable.Select("status='Traded'  and TokenNo='" + v.Token2 + "'  ");
                //if (drr1.Length > 0)
                //{
                //    var _v = drr1.Where(a => a.Field<string>("Buy_SellIndicator") == "BUY").ToList();
                //    _buycount = _buycount+ _v.Count();
                //    var _v1 = drr1.Where(a => a.Field<string>("Buy_SellIndicator") == "SELL").ToList();
                //    _sellcount =_sellcount+ _v1.Count();
                 
                //}
                DGV1.Rows[e.RowIndex].Cells["TRDQTY(B)"].Value = _buycount;

                DGV1.Rows[e.RowIndex].Cells["TRDQTY(S)"].Value = _sellcount; 
              
                //_lotsize.lotsize = CSV_Class.cimlist.Where(q => q.Token ==v.Token1 ).Select(a => a.BoardLotQuantity).First();
                if (Holder._DictLotSize.ContainsKey(v.Token1) == false  && v.Token1 != 0 )
                { 
                Holder._DictLotSize.TryAdd(v.Token1, new Csv_Struct() 
                {
                    lotsize=CSV_Class.cimlist.Where(q => q.Token ==v.Token1 ).Select(a => a.BoardLotQuantity).First()
                }
                );
                }

                if (Holder._DictLotSize.ContainsKey(v.Token2) == false && v.Token2 != 0)
                {
                    Holder._DictLotSize.TryAdd(v.Token2, new Csv_Struct()
                    {
                        lotsize = CSV_Class.cimlist.Where(q => q.Token == v.Token2).Select(a => a.BoardLotQuantity).First()
                    }
                    );
                }

                if (Holder._DictLotSize.ContainsKey(v.Token3) == false && v.Token3 != 0)
                {
                    Holder._DictLotSize.TryAdd(v.Token3, new Csv_Struct()
                    {
                        lotsize = CSV_Class.cimlist.Where(q => q.Token == v.Token3).Select(a => a.BoardLotQuantity).First()
                    }
                    );
                }



              


            }
            else 
            {
                FOPAIRLEG2 v; 
                byte[] buffer = DataPacket.RawSerialize(v=new FOPAIRLEG2()
                {
                    PORTFOLIONAME = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["PF"].Value == DBNull.Value ? "0" : DGV1.Rows[e.RowIndex].Cells["PF"].Value),
                    Token1 = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token1"].Value == DBNull.Value ? "0" : DGV1.Rows[e.RowIndex].Cells["Token1"].Value),
                    Token2 = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token2"].Value == DBNull.Value ? "0" : DGV1.Rows[e.RowIndex].Cells["Token2"].Value),
                    Token3 = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token3"].Value == DBNull.Value ? "0" : DGV1.Rows[e.RowIndex].Cells["Token3"].Value),
                    Token4 = Convert.ToInt32(DGV1.Rows[e.RowIndex].Cells["Token4"].Value == DBNull.Value ? "0" : DGV1.Rows[e.RowIndex].Cells["Token4"].Value),
                    Token1side = Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["Tok1B_S"].Value.ToString() == "Buy" ? 1 : 2),
                    Token2side = Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["Tok2B_S"].Value.ToString() == "Buy" ? 1 : 2),
                    Token3side = Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["Tok3B_S"].Value.ToString() == "Buy" ? 1 : 2),
                    Token4side = Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["Tok4B_S"].Value.ToString() == "Buy" ? 1 : 2),
                    CALCTYPE = Convert.ToInt16(DGV1.Rows[e.RowIndex].Cells["Calc_type"].Value.ToString() == "BaseDiff" ? 2 : 1),
                });
                NNFHandler.Instance.Publisher(MessageType.IOCPAIRUNSUB, buffer);

                if (Holder._DictLotSize.ContainsKey(v.Token1) == false || v.Token1 != 0)
                {
                    Csv_Struct o = new Csv_Struct();
                    Holder._DictLotSize.TryRemove(v.Token1, out o);

                }

                if (Holder._DictLotSize.ContainsKey(v.Token2) == false || v.Token2 != 0)
                {
                    Csv_Struct o = new Csv_Struct();
                    Holder._DictLotSize.TryRemove(v.Token2, out o);
                }
            }
            
            }
        }

        private void DGV1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode ==  Keys.Delete)
            {
              //  --portFolioCounter;
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
            if (DGV1.IsCurrentCellDirty)
            {
                DGV1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                //this.valueChanged = true;
                //this.DGV1.NotifyCurrentCellDirty(true);
            }
        }
        
         //TextBox txt1 = new TextBox();
         //TextBox txt2 = new TextBox();
         //TextBox txt3 = new TextBox();
         //TextBox txt4 = new TextBox();
         //TextBox txt5 = new TextBox();
        //private void DGV1_MouseDoubleClick(object sender, MouseEventArgs e)
        //{

            

        //    DataGridView _dgLoc = sender as DataGridView;

        //    if (_dgLoc.CurrentCell.ColumnIndex == 1)// "BNSFDIFF")
        //    {
        //        DGV1.Controls.Add(txt);
        //        txt.Location = this.DGV1.GetCellDisplayRectangle(_dgLoc.CurrentRow.Cells[_dgLoc.CurrentCell.ColumnIndex].ColumnIndex, _dgLoc.CurrentRow.HeaderCell.RowIndex, false).Location;
        //        //  dataGridView1.Rows[_dgLoc.CurrentRow.HeaderCell.RowIndex].Cells[_dgLoc.CurrentRow.Cells["Apply"].ColumnIndex].Value = dateTimePicker1.Value.Date.ToShortDateString();
        //        txt.Width = DGV1.Columns[0].Width;

        //        string st = _dgLoc.CurrentRow.HeaderCell.RowIndex + "  " + _dgLoc.CurrentRow.Cells["Apply"].ColumnIndex;
        //    }

        //}
        string strv = "";
        private void DGV1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6)// "BNSFDIFF")
            {
                tv = 1;
               
             
             //   strv = e.ColumnIndex.ToString() + "," + e.RowIndex.ToString();
                txt.Show();
              
                txt.Location = this.DGV1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                DGV1.Controls.Add(txt);
                txt.Width = DGV1.Columns[e.ColumnIndex].Width;
                txt.Text = Convert.ToString(DGV1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                txt.Focus();
            }
           
        }

        int tv = 1;
        private void DGV1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            { 
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6)// "BNSFDIFF")
            {
              //  if (tv > 2) return;
                DGV1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = txt.Text;
                tv++;
              
                txt.Hide();             
               
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
            txt.Focus();
            //BeginInvoke((Action)delegate
            //{

            //    txt.SelectAll();
            //});
        }

        private void DGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DGV1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DGV1_Sorted(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
           byte[] buf =ASCIIEncoding.ASCII.GetBytes("46042");

          // TRADE_CONFIRMATION_TR("46042");
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            if (!Information.IsNumeric(txt.Text))
            {
                if (txt.Text.Length > 1)
                {
                    MessageBox.Show("Please Insert Numeric Value", "Information");
                    txt.Clear();
                    txt.Text = "0";
                }
                txt.Focus();
            }
        }

        private void txt_Leave(object sender, EventArgs e)
        {
            this.txt.Hide();
        }

        private void DGV1_Scroll(object sender, ScrollEventArgs e)
        {
          //  string st = sender.ToString();
                
            txt.Hide();
            DGV1.Focus();
         
        }
   
        //public void TRADE_CONFIRMATION_TR(string tokenn) //-- 20222  
        //{
        //    try
        //    {


        //        DataRow[] dr = SpreadTable.Select("Token1 ='" + tokenn + "'");
        //        if (dr.Length > 0)
        //        {
        //            DataGridViewRow row = DGV1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Token1"].Value.ToString().Equals(Convert.ToString(tokenn))).First();
        //            string _fartoken = DGV1.Rows[row.Index].Cells["Token2"].Value.ToString();
        //            DataRow[] drNeartoken = Global.Instance.OrdetTable.Select("TokenNo = '" + dr[0]["Token1"].ToString() + "'");
        //            DataRow[] drFortoken = Global.Instance.OrdetTable.Select("TokenNo = '" + _fartoken + "'");

        //            DGV1.Rows[1].Cells["BFSNTD"].Value = Convert.ToDouble(drNeartoken[drNeartoken.Length - 1]["Price"]) - Convert.ToDouble(drFortoken[drNeartoken.Length - 1]["Price"]);
        //        }
        //    }
        //    catch { }

        //}
    
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