using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DgvFilterPopup;
using System.Net;
using Structure;
using csv;
using System.Diagnostics;
using System.Xml;
using Client.Properties;
using System.Reflection;
using System.Windows.Threading;
using System.Threading;
using System.Data.OleDb;
using AMS.Profile;

namespace Client
{

    public enum MsgType
    {
        CANCELALL = (byte)'R',
        STOP_ALL = (byte)'S',
        Spread = (byte)'T'
    }
    delegate void On_DataPaintdDelegate(Object o, DataGridViewRowPrePaintEventArgs e);
    public partial class frmGenOrderBook : Form
    {
        private static readonly frmGenOrderBook instance = new frmGenOrderBook();
        public static frmGenOrderBook Instance
        {
            get
            {
                return instance;
            }
        }
        private frmGenOrderBook()
        {
            InitializeComponent();
          
         }

        ~frmGenOrderBook()
        {

        }
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
                //MessageBox.Show(ex.Message);
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

        private void frmGenOrderBook_Load(object sender, EventArgs e)
        {
           
            try
            {

                //if (Settings.Default.Window_LocationOBook != null)
                //{
                //    this.Location = Settings.Default.Window_LocationOBook;
                //}

                //// Set window size
                //if (Settings.Default.Window_SizeOBook != null)
                //{
                //    this.Size = Settings.Default.Window_SizeOBook;
                //}
                var AbbA = LoadFormLocationAndSize(this);
                this.Location = new Point(AbbA[0], AbbA[1]);
                this.Size = new Size(AbbA[2], AbbA[3]);

                this.FormClosing += new FormClosingEventHandler(SaveFormLocationAndSize);
                DataView dv1 = Global.Instance.OrdetTable.AsEnumerable().Where(a => a.Field<string>("Status") == orderStatus.Open.ToString() || a.Field<string>("Status") == orderStatus.Modified.ToString()).AsDataView();
                DGV.DataSource = dv1;

                DataView dv2 = Global.Instance.OrdetTable.AsEnumerable().Where(a => a.Field<string>("Status") == orderStatus.Traded.ToString() || a.Field<string>("Status") == orderStatus.Cancel.ToString() || a.Field<string>("Status") == orderStatus.Rejected.ToString()).AsDataView();
                DGV2.DataSource = dv2;
                dv2.Sort = "LOGTIME DESC";




                DataGridViewColumnSelector cs = new DataGridViewColumnSelector(DGV);
                cs.MaxHeight = 200;
                cs.Width = 150;


                // new DgvFilterManager(DGV);


                profile_load();

                Type controlType = DGV.GetType();
                PropertyInfo pi = controlType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(DGV, true, null);

                controlType = DGV2.GetType();
                pi = controlType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(DGV2, true, null);

                DataSet ds = new DataSet();
               // string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + " Default.xlsx" + ";Extended Properties=Excel 12.0;";
                /*string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + Path.DirectorySeparatorChar + "31march.xlsx" + ";Extended Properties=Excel 12.0;";  
               
                using (OleDbConnection connection = new OleDbConnection(con))
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
                    DataTable Sheets = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    foreach (DataRow dr in Sheets.Rows)
                    {
                        string sht = dr[2].ToString().Replace("'", "");
                        OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from [" + sht + "] where status = 'Traded'", connection);
                        dataAdapter.Fill(Global.Instance.OrdetTable);
                        break;
                    }
                    connection.Close();
                }*/
                //Task.Factory.StartNew(() => T());
                
            }
            catch(Exception Ex)
            {
              //  Console.WriteLine(Ex.StackTrace.ToString());
            }
          
           
        }

      //public void T()
      //  {
          
      //      while (true)
      //      {
      //          random_Method();
               
      //          Thread.Sleep(100);
      //      }
      //  }
        public void profile_load()
        {
            try
            {
              

                // DGV.Columns["FullName"].Visible = true;
               //   DGV2.Columns["FullName"].Visible = true;

                DataSet ds1 = new DataSet();
                var config = new Config { GroupName = null };
                var iforms = config.GetValue("GenOrderBook", Convert.ToString(0));
                if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "Order_Profiles" + Path.DirectorySeparatorChar + iforms + ".xml"))
                {
                       ds1.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Order_Profiles" + Path.DirectorySeparatorChar + iforms + ".xml");
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                     
                        if (ds1.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "") == "FullName")
                        { continue; }
                        DGV.Columns[ds1.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = false;
                        //  DGV.Columns[ds1.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].DisplayIndex = i+1;
                        DGV2.Columns[ds1.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = false;
                    }
                }
               

                

            }
            catch (Exception ex)
            {
               // MessageBox.Show( ex.Message);
                
            }
         //   ds1.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")
        }




        #region NNF OUT Messages


        public void ORDER_ERROR_TR(byte[] buffer) //-- 20231
        {
            try
            {
                MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));

                //frmErrorLog.Instance.tbelog.Text = IPAddress.HostToNetworkOrder(obj.ErrorCode) == 0 ? frmErrorLog.Instance.tbelog.Text :
                //            frmErrorLog.Instance.tbelog.Text + Environment.NewLine +
                //             " Error: " + IPAddress.HostToNetworkOrder(obj.ErrorCode) +
                //              ": " + Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.ErrorCode)) +
                //              " Order No: " + (long)LogicClass.DoubleEndianChange((obj.OrderNumber))
                //              ;
            }
            catch (Exception ex)
            {

              //  MessageBox.Show(ex.Message);
            }

        }

        public void ON_STOP_NOTIFICATION(byte[] buffer)
        {
            MS_TRADE_INQ_DATA obj = (MS_TRADE_INQ_DATA)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_INQ_DATA));

        }

        public void ORDER_MOD_REJECT_TR(byte[] buffer) //-- 20042
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ORDER_ERROR_OUTDel(ORDER_MOD_REJECT_TR), buffer);
                    return;
                }
                MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
                DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString() + "'");
                if (dr.Length > 0)
                {
                    if (dr[0]["Status"].ToString()!= orderStatus.Traded.ToString())
                    {
                        dr[0]["Status"] = orderStatus.Rejected.ToString();
                        dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price)) / 100.00;
                        dr[0]["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
                        dr[0]["RejectReason"] = Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
                        //dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
                    }
                   // else
                   // {
                  //      LogWriterClass.logwritercls.logs("20042 trasactioncode", ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString());
                  //  }
                    //   dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price))/100.00;
                }
                //if (!DGV.InvokeRequired)
                //{
                //    DGV.Refresh();
                //}



                if (!frmErrorLog.Instance.tbelog.InvokeRequired)
                {
                    frmErrorLog.Instance.tbelog.Text = IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode) == 0 ? frmErrorLog.Instance.tbelog.Text :
                            frmErrorLog.Instance.tbelog.Text + Environment.NewLine +
                             " Error while modify order: " + IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode) +
                              ": " + Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode)) +
                              " Order No: " + (long)LogicClass.DoubleEndianChange((obj.OrderNumber))
                              ;
                }
            }
            catch (Exception ex)
            {

              //  MessageBox.Show("Order Book -  Funtion Name-  ORDER_MOD_REJ_OUT  " + ex.Message);
            }

        }

        public void ORDER_CANCEL_REJECT_TR(byte[] buffer) //-- 20072
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ORDER_ERROR_OUTDel(ORDER_CANCEL_REJECT_TR), buffer);
                    return;
                }
                MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
                DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString() + "'");
                if (dr.Length > 0)
                {
                    if (dr[0]["Status"].ToString() != orderStatus.Traded.ToString())
                    {
                        dr[0]["Status"] = orderStatus.Rejected.ToString();
                        dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price)) / 100.00;
                        dr[0]["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
                        dr[0]["RejectReason"] = Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
                        //dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
                    }
                  //  else
                   // {
                    //    LogWriterClass.logwritercls.logs("20072 trasactioncode", ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString());
                    //}
                    //   dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price))/100.00;
                }
                //if (!DGV.InvokeRequired)
                //{
                //    DGV.Refresh();
                //}



                if (!frmErrorLog.Instance.tbelog.InvokeRequired)
                {
                    frmErrorLog.Instance.tbelog.Text = IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode) == 0 ? frmErrorLog.Instance.tbelog.Text :
                            frmErrorLog.Instance.tbelog.Text + Environment.NewLine +
                             " Error while modify order: " + IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode) +
                              ": " + Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode)) +
                              " Order No: " + (long)LogicClass.DoubleEndianChange((obj.OrderNumber))
                              ;
                }
            }
            catch (Exception ex)
            {

            //    MessageBox.Show("Order Book -  Funtion Name-  ORDER_MOD_REJ_OUT  " + ex.Message);
            }
        }

     /*   public void AddRecordsTWOLEG(byte[] buffer) //-- 2125
        {
            try
            {
                int val = 0;

                object ob = new object();
                lock (ob)
                {

                    MS_TRADE_CONFIRM_TR obj = (MS_TRADE_CONFIRM_TR)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_CONFIRM_TR));

                    //  var d = Global.Instance.Ratio;
                    //  var v = Global.Instance.Ratio.Where(a => a.Key == (IPAddress.HostToNetworkOrder(obj.Token1).ToString() + IPAddress.HostToNetworkOrder(obj.ms_oe_obj.StrikePrice).ToString() + System.Text.ASCIIEncoding.UTF8.GetString(obj.ms_oe_obj.OptionType))).Select(b => b.Value).ToList();
                    // var val = v.FirstOrDefault().ToString();
                  


                    int lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.Token)].lotsize;   // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.Token1)).Select(a => a.BoardLotQuantity).First();
                    DataRow dr = Global.Instance.OrdetTable.NewRow();




                    dr["Status"] = orderStatus.Traded.ToString();
                    dr["FillPrice"] = IPAddress.HostToNetworkOrder(obj.FillPrice) / 100.00;
                    dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.RemainingVolume) / lotSize;
                    //          dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber1);
                    //       dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType1));
                    //        dr["BranchId"] = IPAddress.HostToNetworkOrder(obj.BranchId1);
                    dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1 ? "BUY" : "SELL";
                    //      dr["CloseoutFlag"] = Convert.ToChar(obj.OpenClose1);
                    dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.ExpiryDate));
                    //    dr["InstrumentName"] = Encoding.ASCII.GetString(obj.ms_oe_obj.InstrumentName);
                    //    dr["OptionType"] = Encoding.ASCII.GetString(obj.ms_oe_obj.OptionType);
                    //   dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.ms_oe_obj.StrikePrice);
                    dr["Symbol"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.Symbol);
                    dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume) / lotSize;
                    dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) / lotSize;
                    //        dr["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.EntryDateTime1));
                    //       dr["filler"] = IPAddress.HostToNetworkOrder(obj.filler1);
                    //    dr["GoodTillDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.GoodTillDate1));
                    // dr["LastModified"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.));
                    dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
                    //         dr["Modified_CancelledBy"] = Convert.ToChar(obj.ModCxlBy1);
                    //   dr["nnffield"] = (long)LogicClass.DoubleEndianChange((obj.NnfField));
                    //   dr["Open_Close"] = Convert.ToChar(obj.OpenClose1);
                    dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber));
                    dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                    //     dr["Pro_ClientIndicator"] = IPAddress.HostToNetworkOrder(obj.ProClient1);
                    //dr["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.);
                    //    dr["Settlor"] = Encoding.ASCII.GetString(obj.Settlor1);
                    //dr["TimeStamp1"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
                    //dr["TimeStamp2"] = Convert.ToChar(obj.TimeStamp2);
                    dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.Token);
                    //  dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj) / lotSize;
                    dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId);
                    dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                    //  dr["UserId"] = IPAddress.HostToNetworkOrder(obj);
                    dr["Volume"] = IPAddress.HostToNetworkOrder(obj.FillQuantity);
                    dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
                    dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.Token)).Name);
                    dr["Unique_Id"] = ((long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.Token)).ToString();

                    Global.Instance.OrdetTable.Rows.Add(dr);







                    //   Global.Instance.OrdetTable.Rows.Add(dr);
                    if (!DGV.InvokeRequired)
                    {
                        DGV.Refresh();
                    }





                    frmNetBook.Instance.netposion();




                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Order Book -  Funtion Name-  TWOL_ORDER_CONFIRMATION  " + ex.Message);
            }

        }*/
           public void ORDER_CONFIRMATION_TR(byte[] buffer) //-- 20073
            {

                if (this.InvokeRequired)
                {
                    this.Invoke(new ORDER_ERROR_OUTDel(ORDER_CONFIRMATION_TR), buffer);
                    return;
                }

                try
                {

                    object ob1 = new object();
                    lock (ob1)
                    {
                        MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
                        if (Holder.holderOrder.ContainsKey(LogicClass.DoubleEndianChange(obj.OrderNumber)))
                            return;
                        Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber), new Order((int)_Type.MS_OE_RESPONSE_TR));
                        Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].mS_OE_RESPONSE_TR = obj;

                        int lotSize =   Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.TokenNo)].lotsize;  // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.TokenNo)).Select(a => a.BoardLotQuantity).First();
                        DataRow dr = Global.Instance.OrdetTable.NewRow();
                        dr["Status"] = orderStatus.Open.ToString();
                        dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber);
                        dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType));
                        dr["BranchId"] = IPAddress.HostToNetworkOrder(obj.BranchId);
                        dr["BrokerId"] = Encoding.ASCII.GetString(obj.BrokerId);
                        dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1 ? "BUY" : "SELL";
                        dr["CloseoutFlag"] = Convert.ToChar(obj.CloseoutFlag);
                        dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.ExpiryDate));
                        dr["InstrumentName"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.InstrumentName);
                        dr["OptionType"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.OptionType);
                        dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.StrikePrice);
                        dr["Symbol"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.Symbol);
                        dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume) / lotSize;
                        dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) / lotSize;
                        dr["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.EntryDateTime));
                        dr["filler"] = IPAddress.HostToNetworkOrder(obj.filler);
                        dr["GoodTillDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.GoodTillDate));
                        dr["LastModified"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LastModified));
                        dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
                        dr["Modified_CancelledBy"] = Convert.ToChar(obj.Modified_CancelledBy);
                        dr["nnffield"] = (long)LogicClass.DoubleEndianChange((obj.nnffield));
                        dr["Open_Close"] = Convert.ToChar(obj.Open_Close);
                        dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.OrderNumber));
                        dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                        dr["Pro_ClientIndicator"] = IPAddress.HostToNetworkOrder(obj.Pro_ClientIndicator);
                        dr["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
                        dr["Settlor"] = Encoding.ASCII.GetString(obj.Settlor);
                        dr["TimeStamp1"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
                        dr["TimeStamp2"] = Convert.ToChar(obj.TimeStamp2);
                        dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.TokenNo);
                        dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.TotalVolumeRemaining) / lotSize;
                        dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId);
                        dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                        dr["UserId"] = IPAddress.HostToNetworkOrder(obj.UserId);
                        dr["Volume"] = IPAddress.HostToNetworkOrder(obj.Volume) / lotSize;
                        dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
                        dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.TokenNo)).Name);
                        dr["Unique_id"] = ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString();
                       //DataRow[] d_r = Global.Instance.OrdetTable.Select("OrderNumber = '" +Convert.ToString( (long)LogicClass.DoubleEndianChange((obj.OrderNumber))) + "'");
                         Global.Instance.OrdetTable.Rows.Add(dr);
                        //if (!DGV.InvokeRequired)
                        //{
                        //    DGV.Refresh();

                        //}

                        //if (!DGV2.InvokeRequired)
                        //{
                        //    DGV2.Refresh();

                        //}
                        //if (!frmLog.Instance.tbelog.InvokeRequired)
                        //{
                        //    frmLog.Instance.tbelog.Text = IPAddress.HostToNetworkOrder(obj.ErrorCode) != 0 ? frmLog.Instance.tbelog.Text :
                        //     frmLog.Instance.tbelog.Text + Environment.NewLine +
                        //      " New order have placed successfully  Order No:" + (long)LogicClass.DoubleEndianChange((obj.OrderNumber));
                        //}
                    }
                }
                catch (Exception ex)
                {

                   // MessageBox.Show("Order Book -  Funtion Name-  ORDER_CONFIRMATION_TR  " + ex.Message);
                }


            }

           Random r = new Random();
           long ord;
        int tok;
        Dictionary<long,int> tord=new Dictionary<long,int>();
           public void random_Method()
           {
               tok = r.Next(1, 1000);
               ord = r.Next(1, 1000);
               if (tord.ContainsKey(ord))
                   return;
               tord.Add(ord, tok);
               DataRow dr = Global.Instance.OrdetTable.NewRow();

               dr["Status"] = orderStatus.Open.ToString();
               dr["AccountNumber"] ="123456";
               dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes),1);
               dr["BranchId"] = 12468;
               dr["BrokerId"] =1234568;
               dr["Buy_SellIndicator"] = "BUY";
               dr["CloseoutFlag"] ='O';
              
               dr["InstrumentName"] ="FUTSTK";
               dr["OptionType"] = "CE";
               dr["StrikePrice"] =8000;
               dr["Symbol"] ="BOSCHO";
               dr["DisclosedVolume"] = 25;
               dr["DisclosedVolumeRemaining"] = 50;
              // dr["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.EntryDateTime));
             //  dr["filler"] = IPAddress.HostToNetworkOrder(obj.filler);
              // dr["GoodTillDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.GoodTillDate));
               //dr["LastModified"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LastModified));
               //dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
               dr["Modified_CancelledBy"] = Convert.ToChar('T');
               dr["nnffield"] = 101002250100;
               dr["Open_Close"] ='O';
               
               dr["OrderNumber"] = ord;
               dr["Price"] = 100;
              // dr["Pro_ClientIndicator"] = IPAddress.HostToNetworkOrder(obj.Pro_ClientIndicator);
              // dr["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
              // dr["Settlor"] = Encoding.ASCII.GetString(obj.Settlor);
             //  dr["TimeStamp1"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
              // dr["TimeStamp2"] = Convert.ToChar(obj.TimeStamp2);
               
               dr["TokenNo"] = tok;
              // dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.TotalVolumeRemaining) / lotSize;
              // dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId);
               dr["TransactionCode"] =20074;
               //dr["UserId"] = IPAddress.HostToNetworkOrder(obj.UserId);
               //dr["Volume"] = IPAddress.HostToNetworkOrder(obj.Volume) / lotSize;
               //dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
               //dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.TokenNo)).Name);
              // dr["Unique_id"] = r.Next(1,100).ToString();
               dr["Unique_id"] = ((long)ord).ToString() + (tok).ToString();
               //   DataRow[] d_r = Global.Instance.OrdetTable.Select("OrderNumber = '" +Convert.ToString( (long)LogicClass.DoubleEndianChange((obj.OrderNumber))) + "'");


               Global.Instance.OrdetTable.Rows.Add(dr);
              
           }
        public void ORDER_MOD_CONFIRMATION_TR(byte[] buffer) //-- 20074
        {
            try
            {

                if (this.InvokeRequired)
                {
                    this.Invoke(new ORDER_ERROR_OUTDel(ORDER_MOD_CONFIRMATION_TR), buffer);
                    return;
                }
                object ob1 = new object();
                lock (ob1)
                {
                    MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
                    Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber), new Order((int)_Type.MS_OE_RESPONSE_TR));
                    Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].mS_OE_RESPONSE_TR = obj;

                    int lotSize =   Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.TokenNo)].lotsize;  ; // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.TokenNo)).Select(a => a.BoardLotQuantity).First();
                    DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString() + "'");
                    if (dr.Length == 0)
                    {
                       // LogWriterClass.logwritercls.logs("20074trasactioncode", ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString());
                        return;
                    }
                       
                    dr[0]["Status"] = orderStatus.Modified.ToString();
                  //  dr[0]["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume) / lotSize;
                  //  dr[0]["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) / lotSize;
                    dr[0]["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.EntryDateTime));
                    dr[0]["LastModified"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LastModified));
                    dr[0]["Modified_CancelledBy"] = Convert.ToChar(obj.Modified_CancelledBy);
                    dr[0]["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                    dr[0]["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.TotalVolumeRemaining) / lotSize;
                    dr[0]["Volume"] = IPAddress.HostToNetworkOrder(obj.Volume) / lotSize;
                    dr[0]["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
                    dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                    //  Global.Instance.OrdetTable.Rows.Add(dr);
                    //if (!DGV.InvokeRequired)
                    //{
                    //    DGV.Refresh();

                    //}

                    //if (!DGV2.InvokeRequired)
                    //{
                    //    DGV2.Refresh();

                    //}
                    //if (!frmLog.Instance.tbelog.InvokeRequired)
                    //{
                    //    frmLog.Instance.tbelog.Text = IPAddress.HostToNetworkOrder(obj.ErrorCode) != 0 ? frmLog.Instance.tbelog.Text :
                    //      frmLog.Instance.tbelog.Text + Environment.NewLine +
                    //       " Order modify successfully  Order No:" + (long)LogicClass.DoubleEndianChange((obj.OrderNumber));
                    //}
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show("Order Book -  Funtion Name-  ORDER_MOD_CONFIRMATION_TR  " + ex.Message);
            }

        }
      
        public void ORDER_CXL_CONFIRMATION_TR(byte[] buffer) //-- 20075
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ORDER_ERROR_OUTDel(ORDER_CXL_CONFIRMATION_TR), buffer);
                    return;
                }

                object ob1 = new object();
                lock (ob1)
                {

                    MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
                    int ch = 0;
                    if (Holder.holderOrder.ContainsKey(LogicClass.DoubleEndianChange(obj.OrderNumber)))
                        ch = Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].GetType();
                    switch (ch)
                    {
                        case 1:
                            {
                                var ob = new Order((int)_Type.MS_OE_REQUEST);
                                Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber), out ob);
                                break;
                            }
                        case 2:
                            {
                                var ob = new Order((int)_Type.MS_OE_RESPONSE_TR);
                                Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber), out ob);
                                //   int lotSize = CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.TokenNo)).Select(a => a.BoardLotQuantity).First();
                                DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString() + "'");
                                if (dr.Length == 0)
                                    return;
                                dr[0]["Status"] = orderStatus.Cancel.ToString();
                                //  dr[0]["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume) / lotSize;
                                //   dr[0]["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) / lotSize;
                                dr[0]["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.EntryDateTime));
                                dr[0]["LastModified"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LastModified));
                                dr[0]["Modified_CancelledBy"] = Convert.ToChar(obj.Modified_CancelledBy);
                                dr[0]["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
                                  dr[0]["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                                //    dr[0]["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.TotalVolumeRemaining) / lotSize;
                                //    dr[0]["Volume"] = IPAddress.HostToNetworkOrder(obj.Volume) / lotSize;
                                //      dr[0]["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
                                dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                                //if (!DGV.InvokeRequired)
                                //{
                                //    DGV.Refresh();
                                //}

                                //if (!DGV2.InvokeRequired)
                                //{
                                //    DGV2.Refresh();

                                //}
                                //if (!frmLog.Instance.tbelog.InvokeRequired)
                                //{
                                //    frmLog.Instance.tbelog.Text = IPAddress.HostToNetworkOrder(obj.ErrorCode) != 0 ? frmLog.Instance.tbelog.Text :
                                //         frmLog.Instance.tbelog.Text + Environment.NewLine +
                                //             " Order cancel successfully  Order No:" + (long)LogicClass.DoubleEndianChange((obj.OrderNumber));
                                //}
                                break;
                            }
                        case 3:
                            {
                                var ob = new Order((int)_Type.MS_SPD_OE_REQUEST);
                                Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber), out ob);
                                //if (!frmLog.Instance.tbelog.InvokeRequired)
                                //{
                                //    frmLog.Instance.tbelog.Text = IPAddress.HostToNetworkOrder(obj.ErrorCode) != 0 ? frmLog.Instance.tbelog.Text :
                                //         frmLog.Instance.tbelog.Text + Environment.NewLine +
                                //             " Order cancel successfully  Order No:" + (long)LogicClass.DoubleEndianChange((obj.OrderNumber));
                                //}

                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {

              //MessageBox.Show("Order Book -  Funtion Name-  ORDER_CXL_CONFIRMATION_TR  " + ex.Message);
            }
        }

        public void PRICE_CONFIRMATION_TR(byte[] buffer) //-- 20012
        {
            MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
        }

        public void TRADE_CONFIRMATION_TR(byte[] buffer) //-- 20222
        {
            int lotSize=0;
            try
            {
              if (this.InvokeRequired)
                {
                    this.Invoke(new ORDER_ERROR_OUTDel(TRADE_CONFIRMATION_TR), buffer);
                    return;
                }

                object ob1 = new object();
                lock (ob1)
                {
                  var obj = (MS_TRADE_CONFIRM_TR)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_CONFIRM_TR));
                    
                    int ch = 0;
                    if (Holder.holderOrder.ContainsKey(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber)))
                        ch = Holder.holderOrder[LogicClass.DoubleEndianChange(obj.ResponseOrderNumber)].GetType();
             //else
              //ch = 3;
                    switch (ch)
                    {
                        case 1:
                            {
                                var ob = new Order((int)_Type.MS_OE_REQUEST);
                             //   Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber), out ob);
                                break;
                            }
                        case 2:
                            {
                                Order ob;
                               lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.Token)].lotsize;    // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.Token)).Select(a => a.BoardLotQuantity).First();
                               //DataRow[] dr2 = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.Token)).ToString() + "'");
                               //dr2[0]["Status"] = orderStatus.Cancel.ToString();
                               //DataRow dr = Global.Instance.OrdetTable.NewRow();
                               //lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.Token)].lotsize;
                               //dr["Status"] = orderStatus.Traded.ToString();
                               //dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber);
                               //dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType));
                               //dr["BrokerId"] = Encoding.ASCII.GetString(obj.BrokerId);
                               //dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1 ? "BUY" : "SELL";
                               //dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.ExpiryDate));
                               //dr["InstrumentName"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.InstrumentName);
                               //dr["OptionType"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.OptionType);
                               //dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.StrikePrice);
                               //dr["Symbol"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.Symbol);
                               //dr["GoodTillDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.GoodTillDate));
                               //dr["Open_Close"] = Convert.ToChar(obj.OpenClose);
                               //dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.Token);
                               //dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId);
                               //dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.Token)).Name);
                               //dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume) / lotSize;
                               //dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) / lotSize;
                               //dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                               //dr["FillPrice"] = IPAddress.HostToNetworkOrder(obj.FillPrice) / 100.00;
                               //dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.RemainingVolume) / lotSize;
                               //dr["Volume"] = IPAddress.HostToNetworkOrder(obj.FillQuantity); // lotSize;
                               //dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
                               //dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss.fff");
                               //dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                               //dr["FillNumber"] = IPAddress.HostToNetworkOrder(obj.FillNumber);
                               //dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber));
                               //dr["Unique_id"] = IPAddress.HostToNetworkOrder((obj.FillNumber)).ToString();
                               //Global.Instance.OrdetTable.Rows.Add(dr);
                              // MessageBox.Show(IPAddress.HostToNetworkOrder(obj.RemainingVolume).ToString());
                               if (IPAddress.HostToNetworkOrder(obj.RemainingVolume) <= 0)
                               {
                                   ob = new Order((int)_Type.MS_OE_RESPONSE_TR);
                                   Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber), out ob);
                                   lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.Token)].lotsize;    // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.Token)).Select(a => a.BoardLotQuantity).First();
                                   DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.Token)).ToString() + "'");
                                   dr[0]["Status"] = orderStatus.Traded.ToString();
                                   dr[0]["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume) / lotSize;
                                   dr[0]["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) / lotSize;
                                   dr[0]["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                                   dr[0]["FillPrice"] = IPAddress.HostToNetworkOrder(obj.FillPrice) / 100.00;
                                   dr[0]["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.RemainingVolume) / lotSize;
                                   dr[0]["Volume"] = IPAddress.HostToNetworkOrder(obj.FillQuantity); // lotSize;
                                   dr[0]["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
                                   dr[0]["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss.fff");
                                   dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                                   dr[0]["FillNumber"] = IPAddress.HostToNetworkOrder(obj.FillNumber);

                               }
                               else
                               {
                                   DataRow dr = Global.Instance.OrdetTable.NewRow();
                                   lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.Token)].lotsize;
                                   dr["Status"] = orderStatus.Traded.ToString();
                                   dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber);
                                   dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType));
                                   dr["BrokerId"] = Encoding.ASCII.GetString(obj.BrokerId);
                                   dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1 ? "BUY" : "SELL";
                                   dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.ExpiryDate));
                                   dr["InstrumentName"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.InstrumentName);
                                   dr["OptionType"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.OptionType);
                                   dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.StrikePrice);
                                   dr["Symbol"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.Symbol);
                                   dr["GoodTillDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.GoodTillDate));
                                   dr["Open_Close"] = Convert.ToChar(obj.OpenClose);
                                   dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.Token);
                                   dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId);
                                   dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.Token)).Name);
                                   dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume) / lotSize;
                                   dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) / lotSize;
                                   dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                                   dr["FillPrice"] = IPAddress.HostToNetworkOrder(obj.FillPrice) / 100.00;
                                   dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.RemainingVolume) / lotSize;
                                   dr["Volume"] = IPAddress.HostToNetworkOrder(obj.FillQuantity); // lotSize;
                                   dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
                                   dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss.fff");
                                   dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                                   dr["FillNumber"] = IPAddress.HostToNetworkOrder(obj.FillNumber);
                                   dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber));
                                   dr["Unique_id"] = IPAddress.HostToNetworkOrder((obj.FillNumber)).ToString();
                                   Global.Instance.OrdetTable.Rows.Add(dr);
                               }
                               //Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber), out ob);
                               //if (!DGV.InvokeRequired)
                               //{
                               //    DGV.Refresh();
                               //}
                               //if (!DGV2.InvokeRequired)
                               //{
                               //    DGV2.Refresh();
                               //}
                               if (!frmLog.Instance.tbelog.InvokeRequired)
                                   frmLog.Instance.tbelog.Text = frmLog.Instance.tbelog.Text + Environment.NewLine +
                                            " Order Traded  Order No:" + (long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber));
                                break;
                            }
                        case 3:
                            {
                               // MessageBox.Show(System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.Token)).Name));
                                var ob = new Order((int)_Type.MS_SPD_OE_REQUEST);
                                Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber), out ob);
                               if (Holder._DictLotSize.ContainsKey(IPAddress.HostToNetworkOrder(obj.Token)) == false || IPAddress.HostToNetworkOrder(obj.Token) != 0)
                                {
                                    Holder._DictLotSize.TryAdd(IPAddress.HostToNetworkOrder(obj.Token), new Csv_Struct()
                                    {
                                        lotsize = CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.Token)).Select(a => a.BoardLotQuantity).First()
                                    }
                                    );
                                }
                               // var v = Global.Instance.Ratio.Where(a => a.Key == (IPAddress.HostToNetworkOrder(obj.Token).ToString() + IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.StrikePrice).ToString() + System.Text.ASCIIEncoding.UTF8.GetString(obj.Contr_dec_tr_Obj.OptionType))).Select(b => b.Value).ToList();
                              //var val =Convert.ToInt32(v.FirstOrDefault().ToString());
                              lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.Token)].lotsize;    // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.Token)).Select(a => a.BoardLotQuantity).First();
                             //DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.Token)).ToString() + "'");
                             DataRow dr = Global.Instance.OrdetTable.NewRow();
                             dr["Unique_id"] = ((long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.Token)).ToString() + (IPAddress.HostToNetworkOrder(obj.FillPrice)).ToString() + (IPAddress.HostToNetworkOrder(obj.LogTime)).ToString();
                             dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.Token);
                             dr["Status"] = orderStatus.Traded.ToString();
                             dr["Buy_SellIndicator"] =((BUYSELL) IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator)).ToString() ;
                             dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume);// / lotSize;
                             dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining);// / lotSize;
                             dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                             dr["FillPrice"] = IPAddress.HostToNetworkOrder(obj.FillPrice) / 100.00;
                             dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.RemainingVolume);// / lotSize;
                             //  dr[0]["Volume"] = ((IPAddress.HostToNetworkOrder(obj.FillQuantity)/lotSize)*(val*lotSize))/(val*lotSize);
                             dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday); // lotSize;
                             dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss.fff");
                             dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                             dr["Symbol"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.Symbol);
                             dr["Volume"] = IPAddress.HostToNetworkOrder(obj.FillQuantity) / lotSize;
                             dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.ExpiryDate));
                             dr["InstrumentName"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.InstrumentName);
                             dr["OptionType"] = Encoding.ASCII.GetString(obj.Contr_dec_tr_Obj.OptionType);
                             dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.StrikePrice);
                             dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.Token)).Name);
                             Global.Instance.OrdetTable.Rows.Add(dr);
                             //Global.Instance.MTMDIct.AddOrUpdate(Stat.Parameter.Token, Stat.Parameter.LTP, (k, v) => Stat.Parameter.LTP);
                             /*Global.Instance.Ratio.AddOrUpdate((IPAddress.HostToNetworkOrder(obj.Token).ToString() + IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.StrikePrice).ToString() + System.Text.ASCIIEncoding.UTF8.GetString(obj.Contr_dec_tr_Obj.OptionType))
                             ,);*/
                             //if(!DGV.InvokeRequired)
                             //{
                             //DGV.Refresh();
                             //}
                             //if(!DGV2.InvokeRequired)
                             //{
                             //DGV2.Refresh();
                             //}
                             //if (!frmLog.Instance.tbelog.InvokeRequired)
                             //frmLog.Instance.tbelog.Text = frmLog.Instance.tbelog.Text + Environment.NewLine +
                             //" Order Traded  Order No:" + (long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber));
                                break;
                            }
                           
                    }

                    if (!this.InvokeRequired)
                    {
                        MethodInvoker del = delegate
                        {
                            if (lotSize == 0)
                                goto l;
                            frmNetBook.Instance.netposion(IPAddress.HostToNetworkOrder(obj.Token), lotSize);
                            frmTradeBook.Instance.DGV.Refresh();
                            frmTradeBook.Instance.lblnooftrade.Text = "No Of Trade  =" + frmTradeBook.Instance.DGV.Rows.Count;
                            if (IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1)
                                frmTradeBook.Instance.lblb_q.Text = (Convert.ToDouble(frmTradeBook.Instance.lblb_q.Text == "0" ? "0" : frmTradeBook.Instance.lblb_q.Text) + IPAddress.HostToNetworkOrder(obj.FillQuantity) / lotSize).ToString();
                            else
                                frmTradeBook.Instance.lbls_q.Text = (Convert.ToDouble(frmTradeBook.Instance.lbls_q.Text == "0" ? "0" : frmTradeBook.Instance.lbls_q.Text) + IPAddress.HostToNetworkOrder(obj.FillQuantity) / lotSize).ToString();

                            l:
                            if (IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1)
                            {

                                frmTradeBook.Instance.lblb_V.Text = (Convert.ToDouble(frmTradeBook.Instance.lblb_V.Text == "0" ? "0" : frmTradeBook.Instance.lblb_V.Text) + ((IPAddress.HostToNetworkOrder(obj.FillPrice)) * (IPAddress.HostToNetworkOrder(obj.FillQuantity))) / 100).ToString();
                                // frmTradeBook.Instance.lblb_V.Text = Global.Instance.OrdetTable.AsEnumerable().Where(r => r.Field<string>("Status") == "Traded" && r.Field<string>("Buy_SellIndicator") == "BUY").Sum(r => r.Field<Double>("FillPrice") * Convert.ToDouble(r.Field<string>("Volume"))).ToString();
                            }
                            else
                            {
                                frmTradeBook.Instance.lbls_v.Text = (Convert.ToDouble(frmTradeBook.Instance.lbls_v.Text == "0" ? "0" : frmTradeBook.Instance.lbls_v.Text) + ((IPAddress.HostToNetworkOrder(obj.FillPrice)) * (IPAddress.HostToNetworkOrder(obj.FillQuantity)))/100).ToString();
                             //   frmTradeBook.Instance.lbls_v.Text = Global.Instance.OrdetTable.AsEnumerable().Where(r => r.Field<string>("Status") == "Traded" && r.Field<string>("Buy_SellIndicator") == "SELL").Sum(r => r.Field<Double>("FillPrice") * Convert.ToDouble(r.Field<string>("Volume"))).ToString();
                            }

                            frmTradeBook.Instance.lbln_v.Text = (Convert.ToDouble(frmTradeBook.Instance.lbls_q.Text) - Convert.ToDouble(frmTradeBook.Instance.lblb_q.Text)).ToString();
                            frmTradeBook.Instance.N_V.Text = ((Convert.ToDouble(frmTradeBook.Instance.lbls_v.Text) - Convert.ToDouble(frmTradeBook.Instance.lblb_V.Text))/100).ToString();
                            string str = "BUY_Quantity =\t" + frmTradeBook.Instance.lblb_q.Text + "\t Sell_Quantity =\t" + frmTradeBook.Instance.lbls_q.Text + "\t Buy_Value=\t" + frmTradeBook.Instance.lblb_V.Text + "\tSell_Value=\t" + frmTradeBook.Instance.lbls_v.Text + "\tNet_Value =\t" + frmTradeBook.Instance.N_V.Text;
                         //   LogWriterClass.logwritercls.logs(Global.Instance.Net_File,str);
                           // StreamWriter w = new StreamWriter(Environment.CurrentDirectory + Path.DirectorySeparatorChar + Global.Instance.Net_File + ".doc", true);
                          //w.WriteLine("lblb_q =\t" + frmTradeBook.Instance.lbls_q.Text + "\t lbls_q =\t" + frmTradeBook.Instance.lbls_q.Text + "\tlblb_V=\t" + frmTradeBook.Instance.lblb_V.Text + "\t lbls_v =\t" + frmTradeBook.Instance.lbls_v.Text + "N_V.Text " + frmTradeBook.Instance.N_V.Text);

                            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Global.Instance.Net_File+".txt");
                            File.WriteAllText(destPath,str);

                            string Trade20222 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Trade_20222.txt");
                            
                          //  File.WriteAllText(Trade20222,"Token "+Convert.ToString(IPAddress.HostToNetworkOrder(obj.Token))+"");
                            LogWriterClass.logwritercls.logs(Trade20222, "\tToken\t" + Convert.ToString(IPAddress.HostToNetworkOrder(obj.Token)) + " FillPrice \t" + Convert.ToString(IPAddress.HostToNetworkOrder(obj.FillPrice)) + "FillQuantity\t" + Convert.ToString(IPAddress.HostToNetworkOrder(obj.FillQuantity)) + " Buy_SellIndicator " + Convert.ToString(IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator)));
                        };
                        this.Invoke(del);
                    }
                  }
            }
            catch (Exception ex)
            {
              
               // MessageBox.Show("Order Book -  Funtion Name-  TRADE_CONFIRMATION_TR  " + ex.Message);
            }
     }


        //Order and Trade Management

        delegate void ORDER_ERROR_OUTDel(byte[] buffer);

        public void ORDER_ERROR_OUT(byte[] buffer) //-- 2231
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new ORDER_ERROR_OUTDel(ORDER_ERROR_OUT),buffer);
                return;
            }
            try
            { 
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
            int lotSize =  Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.TokenNo)].lotsize;  ; // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.TokenNo)).Select(a => a.BoardLotQuantity).First();
            DataRow dr = Global.Instance.OrdetTable.NewRow();
           // Console.WriteLine(IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
           // MessageBox.Show(IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode).ToString());
                dr["Status"] = orderStatus.Rejected.ToString();
            dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber);
         //   dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType));
        //    dr["BranchId"] = IPAddress.HostToNetworkOrder(obj.BranchId);
         ////   dr["BrokerId"] = Encoding.ASCII.GetString(obj.BrokerId);
            dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1 ? "BUY" : "SELL";
         //   dr["CloseoutFlag"] = Convert.ToChar(obj.CloseoutFlag);
            dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.contract_obj.ExpiryDate));
         //   dr["InstrumentName"] = Encoding.ASCII.GetString(obj.contract_obj.InstrumentName);
         //   dr["OptionType"] = Encoding.ASCII.GetString(obj.contract_obj.OptionType);
       //     dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.contract_obj.StrikePrice);
            dr["Symbol"] = Encoding.ASCII.GetString(obj.contract_obj.Symbol);
      //      dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume) / lotSize;
      //      dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) / lotSize;
            dr["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.EntryDateTime));
     //       dr["filler"] = IPAddress.HostToNetworkOrder(obj.filler1);
     //       dr["GoodTillDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.GoodTillDate));
            dr["LastModified"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LastModified));
            dr["LogTime"] = System.DateTime.Now.ToShortTimeString();
            dr["Modified_CancelledBy"] = Convert.ToChar(obj.Modified_CancelledBy);
            dr["nnffield"] = (long)LogicClass.DoubleEndianChange((obj.nnffield));
  //          dr["Open_Close"] = Convert.ToChar(obj.Open_Close);
            dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.OrderNumber));
           dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
    //        dr["Pro_ClientIndicator"] = IPAddress.HostToNetworkOrder(obj.Pro_ClientIndicator);
            dr["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
   //         dr["Settlor"] = Encoding.ASCII.GetString(obj.Settlor);
   //         dr["TimeStamp1"] = System.DateTime.Now.ToShortTimeString();
            dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.TokenNo);
    //        dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.TotalVolumeRemaining) / lotSize;
   //         dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId);
            dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
            dr["Volume"] = IPAddress.HostToNetworkOrder(obj.Volume) / lotSize;
       //     dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
            dr["RejectReason"] = Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
            dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.TokenNo)).Name);
            dr["Unique_id"] = ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString();
                Global.Instance.OrdetTable.Rows.Add(dr);

           
                //if(!DGV.InvokeRequired)
                //{
                //    DGV.Refresh();

                //}

                //if (!DGV2.InvokeRequired)
                //{
                //    DGV2.Refresh();

                //}
          

                //if (!frmErrorLog.Instance.tbelog.InvokeRequired)
                //{ 
                // frmErrorLog.Instance.tbelog.Text = IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode) == 0 ? frmErrorLog.Instance.tbelog.Text :
                //     frmErrorLog.Instance.tbelog.Text + Environment.NewLine +
                //      " Error while place new order: " + IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode) +
                //      ": " + Enum.GetName(typeof(Enums.Error_Codes),IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode))+
                //       " Order No: " + (long)LogicClass.DoubleEndianChange((obj.OrderNumber))
                //       ;
                //}
            }
            catch (Exception ex)
            {

                //MessageBox.Show("Order Book -  Funtion Name-  TRADE_CONFIRMATION_TR  " + ex.Message);
            }
        }



        public void PRICE_CONFIRMATION(byte[] buffer) //-- 2012
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
        }

      
        public void ORDER_CONFIRMATION_OUT(byte[] buffer) //-- 2073
        {
            try { 
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
            Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber), new Order((int)_Type.MS_OE_REQUEST));
            Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].mS_OE_REQUEST = obj;

            //if (!frmErrorLog.Instance.tbelog.InvokeRequired)
            //{
            //    frmErrorLog.Instance.tbelog.Text = IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode) == 0 ? frmErrorLog.Instance.tbelog.Text :
            //           frmErrorLog.Instance.tbelog.Text + Environment.NewLine +
            //            " Error while Cancel order: " + IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode) +
            //             ": " + Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode)) +
            //             " Order No: " + (long)LogicClass.DoubleEndianChange((obj.OrderNumber))
            //             ;
            //}
            }
            catch (Exception ex)
            {

              //  MessageBox.Show("Order Book -  Funtion Name-  ORDER_CONFIRMATION_OUT  " + ex.Message);
            }

        }
        public void FREEZE_TO_CONTROL(byte[] buffer) //-- 2170
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
        }
        public void ORDER_MOD_REJ_OUT(byte[] buffer) //-- 2042
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ORDER_ERROR_OUTDel(ORDER_MOD_REJ_OUT), buffer);
                    return;
                }
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
            DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString() + "'");
            if (dr.Length>0)
            {
                if (dr[0]["Status"].ToString() != orderStatus.Traded.ToString() )
                { 
                dr[0]["Status"] = orderStatus.Rejected.ToString();
                dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price)) / 100.00;
               dr[0]["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
              dr[0]["RejectReason"] = Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
               dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder (obj.header_obj.TransactionCode);
                }
               // else
                //{
                  ///  LogWriterClass.logwritercls.logs("2042trasactioncode", ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString());
                //}
         //   dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price))/100.00;
            }
            //if (!DGV.InvokeRequired)
            //{
            //    DGV.Refresh();
            //}

          

            if (!frmErrorLog.Instance.tbelog.InvokeRequired)
            {
                frmErrorLog.Instance.tbelog.Text = IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode) == 0 ? frmErrorLog.Instance.tbelog.Text :
                        frmErrorLog.Instance.tbelog.Text + Environment.NewLine +
                         " Error while modify order: " + IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode) +
                          ": " + Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode)) +
                          " Order No: " + (long)LogicClass.DoubleEndianChange((obj.OrderNumber))
                          ;
            }
            }
            catch (Exception ex)
            {

                //MessageBox.Show("Order Book -  Funtion Name-  ORDER_MOD_REJ_OUT  " + ex.Message);
            }
        }

        public void ORDER_MOD_CONFIRM_OUT(byte[] buffer) //-- 2074
        {
            try
            { 
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
            Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber), new Order((int)_Type.MS_OE_REQUEST));
            Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].mS_OE_REQUEST = obj;
            }
            catch (Exception ex)
            {

               // MessageBox.Show("Order Book -  Funtion Name-  ORDER_MOD_CONFIRM_OUT  " + ex.Message);
            }
        }
        public void ORDER_CANCEL_CONFIRM_OUT(byte[] buffer) //-- 2075
        {
            try{
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
            var ob = new Order((int)_Type.MS_OE_REQUEST);
            Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber), out ob);
             }
            catch (Exception ex)
            {

//                MessageBox.Show("Order Book -  Funtion Name-  ORDER_CANCEL_CONFIRM_OUT  " + ex.Message);
            }
        }

        public void BATCH_ORDER_CANCEL(byte[] buffer) //-- 9002
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));

            DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString() + "'");
             dr[0]["Status"] = orderStatus.Cancel.ToString();
             dr[0]["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
            dr[0]["RejectReason"] = Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
            dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
          
            //if (!DGV.InvokeRequired)
            //{
            //    DGV.Refresh();

            //}

            //if (!DGV2.InvokeRequired)
            //{
            //    DGV2.Refresh();

            //}

        }

        public void ORDER_CXL_REJ_OUT(byte[] buffer) //-- 2072
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ORDER_ERROR_OUTDel(ORDER_CXL_REJ_OUT), buffer);
                    return;
                }
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));

            DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString() + "'");
            if (dr.Length > 0)
            {
                if (dr[0]["Status"].ToString() != orderStatus.Traded.ToString())
                  //  || dr[0]["Status"].ToString() != orderStatus.Modified.ToString())
                {
                    dr[0]["Status"] = orderStatus.Rejected.ToString();
                    dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price)) / 100.00;
                    dr[0]["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
                    dr[0]["RejectReason"] = Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
                //    dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
                }
            }
           
            //    if (!DGV.InvokeRequired)
            //{
            //    DGV.Refresh();

            //}

            }
            catch (Exception ex)
            {

              //  MessageBox.Show("Order Book -  Funtion Name-  ORDER_CXL_REJ_OUT  " + ex.Message);
            }
        }


        public void TRADE_ERROR(byte[] buffer) //-- 2223
        {
            MS_TRADE_INQ_DATA obj = (MS_TRADE_INQ_DATA)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_INQ_DATA));
        }

        public void TRADE_CANCEL_OUT(byte[] buffer) //-- 5441
        {
            MS_TRADE_INQ_DATA obj = (MS_TRADE_INQ_DATA)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_INQ_DATA));
        }



        public void SP_ORDER_CONFIRMATION(byte[] buffer)//2124
        {
     try{
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
            Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber1), new Order((int)_Type.MS_SPD_OE_REQUEST));
            Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber1)].mS_SPD_OE_REQUEST = obj;

            Console.WriteLine(
                " Token1  : " + IPAddress.HostToNetworkOrder(obj.Token1) +
                ", OrderNumber1 : " + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1) +
                ", BuySell1 : " + IPAddress.HostToNetworkOrder(obj.BuySell1) +
                ", PriceDiff : " + IPAddress.HostToNetworkOrder(obj.PriceDiff) +
                ", Price1 : " + IPAddress.HostToNetworkOrder(obj.Price1) +
                ", Volume1 : " + IPAddress.HostToNetworkOrder(obj.Volume1) +
                ", TriggerPrice1 : " + IPAddress.HostToNetworkOrder(obj.TriggerPrice1) +
                ", TotalVolRemaining1 : " + IPAddress.HostToNetworkOrder(obj.TotalVolRemaining1)
                );

             }
            catch (Exception ex)
            {

              //  MessageBox.Show("Order Book -  Funtion Name-  SP_ORDER_CONFIRMATION  " + ex.Message);
            }

        }
        public void SP_ORDER_MOD_CON_OUT(byte[] buffer)//2136
        {
            try
            {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
            Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber1)].mS_SPD_OE_REQUEST = obj;

            Console.WriteLine(
                " Token1  : " + IPAddress.HostToNetworkOrder(obj.Token1) +
                ", OrderNumber1 : " + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1) +
                ", BuySell1 : " + IPAddress.HostToNetworkOrder(obj.BuySell1) +
                ", PriceDiff : " + IPAddress.HostToNetworkOrder(obj.PriceDiff) +
                ", Price1 : " + IPAddress.HostToNetworkOrder(obj.Price1) +
                ", Volume1 : " + IPAddress.HostToNetworkOrder(obj.Volume1) +
                ", TriggerPrice1 : " + IPAddress.HostToNetworkOrder(obj.TriggerPrice1) +
                ", TotalVolRemaining1 : " + IPAddress.HostToNetworkOrder(obj.TotalVolRemaining1)
                );
               }
            catch (Exception ex)
            {

                //MessageBox.Show("Order Book -  Funtion Name-  SP_ORDER_CONFIRMATION  " + ex.Message);
            }
        }

        public void SP_ORDER_MOD_REJ_OUT(byte[] buffer)//2133
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }

        public void SP_ORDER_CXL_CONFIRMATION(byte[] buffer)//2130
        {
            try
            {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));

            MS_SPD_OE_REQUEST o1 = (MS_SPD_OE_REQUEST)obj;

            Console.WriteLine(
                " Token1  : " + IPAddress.HostToNetworkOrder(obj.Token1) +
                ", OrderNumber1 : " + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1) +
                ", BuySell1 : " + IPAddress.HostToNetworkOrder(obj.BuySell1) +
                ", PriceDiff : " + IPAddress.HostToNetworkOrder(obj.PriceDiff) +
                ", Price1 : " + IPAddress.HostToNetworkOrder(obj.Price1) +
                ", Volume1 : " + IPAddress.HostToNetworkOrder(obj.Volume1) +
                ", TriggerPrice1 : " + IPAddress.HostToNetworkOrder(obj.TriggerPrice1) +
                ", TotalVolRemaining1 : " + IPAddress.HostToNetworkOrder(obj.TotalVolRemaining1)
                );
              }
            catch (Exception ex)
            {

             //   MessageBox.Show("Order Book -  Funtion Name-  SP_ORDER_CXL_CONFIRMATION  " + ex.Message);
            }
        }


        public void SP_ORDER_CXL_REJ_OUT(byte[] buffer)//2127
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }


        public void SP_ORDER_ERROR_out(byte[] buffer)//2154
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }

        public void TWOL_ORDER_CONFIRMATION(byte[] buffer) //-- 2125
        {
            try {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ORDER_ERROR_OUTDel(TWOL_ORDER_CONFIRMATION), buffer);
                    return;
                }
                object ob = new object();
                lock(ob)
                {
                MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
               // LogWriterClass.logwritercls.logs("==TWOL_ORDER_CONFIRMATION==", " Price1 =" + IPAddress.HostToNetworkOrder(obj.Price1).ToString() + "=>>>>" + "   leg2.Price2" + IPAddress.HostToNetworkOrder(obj.leg2.Price2).ToString() + "=>>>>" + "   TransactionCode" + IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode).ToString());
              //  var d = Global.Instance.Ratio;
             //  var v = Global.Instance.Ratio.Where(a => a.Key == (IPAddress.HostToNetworkOrder(obj.Token1).ToString() + IPAddress.HostToNetworkOrder(obj.ms_oe_obj.StrikePrice).ToString() + System.Text.ASCIIEncoding.UTF8.GetString(obj.ms_oe_obj.OptionType))).Select(b => b.Value).ToList();
            // var val = v.FirstOrDefault().ToString();
                    if (Holder.holderOrder.ContainsKey(LogicClass.DoubleEndianChange(obj.OrderNumber1)))
                    return;
                Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber1), new Order((int)_Type.MS_SPD_OE_REQUEST));
                Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber1)].mS_SPD_OE_REQUEST = obj;
                   
                int lotSize =  Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.Token1)].lotsize;   // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.Token1)).Select(a => a.BoardLotQuantity).First();
                DataRow dr = Global.Instance.OrdetTable.NewRow();

                dr["Status"] = orderStatus.Open.ToString();
      //          dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber1);
         //       dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType1));
        //        dr["BranchId"] = IPAddress.HostToNetworkOrder(obj.BranchId1);
                dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.BuySell1) == 1 ? "BUY" : "SELL";
          //      dr["CloseoutFlag"] = Convert.ToChar(obj.OpenClose1);
                dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.ms_oe_obj.ExpiryDate));
            //    dr["InstrumentName"] = Encoding.ASCII.GetString(obj.ms_oe_obj.InstrumentName);
            //    dr["OptionType"] = Encoding.ASCII.GetString(obj.ms_oe_obj.OptionType);
             //   dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.ms_oe_obj.StrikePrice);
                dr["Symbol"] = Encoding.ASCII.GetString(obj.ms_oe_obj.Symbol);
               dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVol1) / lotSize;
                dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolRemaining1) / lotSize;
        //        dr["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.EntryDateTime1));
         //       dr["filler"] = IPAddress.HostToNetworkOrder(obj.filler1);
            //    dr["GoodTillDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.GoodTillDate1));
                dr["LastModified"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LastModified1));
                dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.header_obj.LogTime)).ToString("HH:mm:ss");
       //         dr["Modified_CancelledBy"] = Convert.ToChar(obj.ModCxlBy1);
             //   dr["nnffield"] = (long)LogicClass.DoubleEndianChange((obj.NnfField));
             //   dr["Open_Close"] = Convert.ToChar(obj.OpenClose1);
                dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.OrderNumber1));
                dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price1) / 100.00;
           //     dr["Pro_ClientIndicator"] = IPAddress.HostToNetworkOrder(obj.ProClient1);
                dr["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode1);
            //    dr["Settlor"] = Encoding.ASCII.GetString(obj.Settlor1);
                //dr["TimeStamp1"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
                //dr["TimeStamp2"] = Convert.ToChar(obj.TimeStamp2);
                dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.Token1);
                dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.TotalVolRemaining1) / lotSize;
                dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId1);
                dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
                //  dr["UserId"] = IPAddress.HostToNetworkOrder(obj);
                dr["Volume"] = IPAddress.HostToNetworkOrder(obj.Volume1) / lotSize;
                dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday1) / lotSize;
                dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.Token1)).Name);
                dr["Unique_Id"] = ((long)LogicClass.DoubleEndianChange((obj.OrderNumber1))).ToString() + (IPAddress.HostToNetworkOrder(obj.Token1)).ToString();

                Global.Instance.OrdetTable.Rows.Add(dr);


                // 2 nd leg --------------------------------------------------------------------------------------------------

                int lotSize1 = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.leg2.token)].lotsize; 
                DataRow drleg2 = Global.Instance.OrdetTable.NewRow();
                drleg2["Status"] = orderStatus.Open.ToString();
           //     drleg2["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber1);
          //      drleg2["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType1));
          //      drleg2["BranchId"] = IPAddress.HostToNetworkOrder(obj.BranchId1);
                drleg2["Buy_SellIndicator"] = ((BUYSELL)IPAddress.HostToNetworkOrder(obj.leg2.BuySell2)).ToString(); // == 1 ? "BUY" : "SELL";
         //       drleg2["CloseoutFlag"] = Convert.ToChar(obj.leg2.OpenClose2);
                drleg2["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg2.ms_oe_obj.ExpiryDate));
        //        drleg2["InstrumentName"] = Encoding.ASCII.GetString(obj.leg2.ms_oe_obj.InstrumentName);
        //        drleg2["OptionType"] = Encoding.ASCII.GetString(obj.leg2.ms_oe_obj.OptionType);
       //         drleg2["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.leg2.ms_oe_obj.StrikePrice);
                drleg2["Symbol"] = Encoding.ASCII.GetString(obj.leg2.ms_oe_obj.Symbol);
                drleg2["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.leg2.DisclosedVol2) / lotSize1;
                drleg2["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.leg2.DisclosedVolRemaining2) / lotSize1;
     //           drleg2["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg2.ms_oe_obj.ExpiryDate));
      //          drleg2["filler"] = IPAddress.HostToNetworkOrder(obj.leg2.Fillerx2);
     //           drleg2["GoodTillDate"] = 0;
     //           drleg2["LastModified"] = 0; // LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg2.LastModified2));
                drleg2["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.header_obj.LogTime)).ToString("HH:mm:ss");
     //           drleg2["Modified_CancelledBy"] = 0; // Convert.ToChar(obj.leg2.ModCxlBy1);
              //  drleg2["nnffield"] = (long)LogicClass.DoubleEndianChange((obj.NnfField));
              //  drleg2["Open_Close"] = Convert.ToChar(obj.leg2.OpenClose2);
                drleg2["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.OrderNumber1));

                drleg2["Price"] = IPAddress.HostToNetworkOrder(obj.Price1) / 100.00;


          //      drleg2["Pro_ClientIndicator"] = IPAddress.HostToNetworkOrder(obj.ProClient1);
                drleg2["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode1);
          //      drleg2["Settlor"] = Encoding.ASCII.GetString(obj.Settlor1);
                //  drleg2["TimeStamp1"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
                //drleg2["TimeStamp2"] = Convert.ToChar(obj.TimeStamp2);
                drleg2["TokenNo"] = IPAddress.HostToNetworkOrder(obj.leg2.token);


                drleg2["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.leg2.TotalVolRemaining2) / lotSize1;
             //   drleg2["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId1);
                drleg2["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
                //  drleg2["UserId"] = IPAddress.HostToNetworkOrder(obj);
                drleg2["Volume"] = IPAddress.HostToNetworkOrder(obj.leg2.Volume2) / lotSize1;

                drleg2["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.leg2.VolumeFilledToday2) / lotSize1;

                drleg2["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.leg2.token)).Name);
                drleg2["Unique_Id"] = ((long)LogicClass.DoubleEndianChange((obj.OrderNumber1))).ToString() + (IPAddress.HostToNetworkOrder(obj.leg2.token)).ToString();
                Global.Instance.OrdetTable.Rows.Add(drleg2);
                //--------------------------------------------------------------------------------------------------------
                //=======leg 3 =========================================================================================================
                //dr["Status"] = orderStatus.Open.ToString();
                //dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber1);
                //dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType1));
                //dr["BranchId"] = IPAddress.HostToNetworkOrder(obj.BranchId1);


                //dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.leg3.BuySell2) == 1 ? "BUY" : "SELL";

                //dr["CloseoutFlag"] = Convert.ToChar(obj.leg3.OpenClose2);
                //dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg3.ms_oe_obj.ExpiryDate));
                //dr["InstrumentName"] = Encoding.ASCII.GetString(obj.leg3.ms_oe_obj.InstrumentName);
                //dr["OptionType"] = Encoding.ASCII.GetString(obj.leg3.ms_oe_obj.OptionType);
                //dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.leg3.ms_oe_obj.StrikePrice);
                //dr["Symbol"] = Encoding.ASCII.GetString(obj.leg3.ms_oe_obj.Symbol);
                //dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.leg3.DisclosedVol2) / lotSize;
                //dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.leg3.DisclosedVolRemaining2) / lotSize;
                //dr["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg3.ms_oe_obj.ExpiryDate));
                //dr["filler"] = IPAddress.HostToNetworkOrder(obj.leg3.Fillerx2);
                //dr["GoodTillDate"] = 0;
                //dr["LastModified"] = 0; // LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg2.LastModified2));
                ////   dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
                //dr["Modified_CancelledBy"] = 0; // Convert.ToChar(obj.leg2.ModCxlBy1);
                //dr["nnffield"] = (long)LogicClass.DoubleEndianChange((obj.NnfField));
                //dr["Open_Close"] = Convert.ToChar(obj.leg3.OpenClose2);
                //dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.OrderNumber1));

                //dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price1) / 100.00;


                //dr["Pro_ClientIndicator"] = IPAddress.HostToNetworkOrder(obj.ProClient1);
                //dr["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode1);
                //dr["Settlor"] = Encoding.ASCII.GetString(obj.Settlor1);
                ////  dr["TimeStamp1"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
                ////dr["TimeStamp2"] = Convert.ToChar(obj.TimeStamp2);
                //dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.Token1);


                //dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.leg3.TotalVolRemaining2) / lotSize;
                //dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId1);
                //// dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                ////  dr["UserId"] = IPAddress.HostToNetworkOrder(obj);
                //dr["Volume"] = IPAddress.HostToNetworkOrder(obj.leg3.Volume2) / lotSize;

                //dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.leg3.VolumeFilledToday2) / lotSize;

                //dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.leg3.token)).Name);
                //dr["Unique_Id"] = ((long)LogicClass.DoubleEndianChange((obj.OrderNumber1))).ToString() + (IPAddress.HostToNetworkOrder(obj.leg3.token)).ToString();
                //Global.Instance.OrdetTable.Rows.Add(dr);
                //======================================================================================================================




                //   Global.Instance.OrdetTable.Rows.Add(dr);
                //if (!DGV.InvokeRequired)
                //{
                //    DGV.Refresh();
                //}
               
           
           

          




            Console.WriteLine(
                " Token1  : " + IPAddress.HostToNetworkOrder(obj.Token1) +
                ", OrderNumber1 : " + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1) +
                ", BuySell1 : " + IPAddress.HostToNetworkOrder(obj.BuySell1) +
                ", PriceDiff : " + IPAddress.HostToNetworkOrder(obj.PriceDiff) +
                ", Price1 : " + IPAddress.HostToNetworkOrder(obj.Price1) +
                ", Volume1 : " + IPAddress.HostToNetworkOrder(obj.Volume1) +
                ", TriggerPrice1 : " + IPAddress.HostToNetworkOrder(obj.TriggerPrice1) +
                ", TotalVolRemaining1 : " + IPAddress.HostToNetworkOrder(obj.TotalVolRemaining1)
            );
            }
            }
            catch (Exception ex)
            {

              //  MessageBox.Show("Order Book -  Funtion Name-  TWOL_ORDER_CONFIRMATION  " + ex.Message);
            }

        }

        public void THRL_ORDER_CONFIRMATION(byte[] buffer) //-- 2126
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }


        public void TWOL_ORDER_ERROR(byte[] buffer) //-- 2155
        {

            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        // Task.Factory.StartNew(()=>   LogWriterClass.logwritercls.logs("2155tcodeRejectedResion","Resion Code   "+  (IPAddress.HostToNetworkOrder(obj.ReasonCode1)).ToString()) );
            
           try
           {
               object ob = new object();
               lock(ob)
               {
           // MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
            int lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.Token1)].lotsize;  // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.Token1)).Select(a => a.BoardLotQuantity).First();
            DataRow dr = Global.Instance.OrdetTable.NewRow();
            dr["Status"] = orderStatus.Rejected.ToString();
            dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber1);
            dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType1));
            dr["BranchId"] = IPAddress.HostToNetworkOrder(obj.BranchId1);


            dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.BuySell1) == 1 ? "BUY" : "SELL";

            dr["CloseoutFlag"] = Convert.ToChar(obj.OpenClose1);
            dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.ms_oe_obj.ExpiryDate));
            dr["InstrumentName"] = Encoding.ASCII.GetString(obj.ms_oe_obj.InstrumentName);
            dr["OptionType"] = Encoding.ASCII.GetString(obj.ms_oe_obj.OptionType);
            dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.ms_oe_obj.StrikePrice);
            dr["Symbol"] = Encoding.ASCII.GetString(obj.ms_oe_obj.Symbol);
            dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVol1) / lotSize;
            dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolRemaining1) / lotSize;
            dr["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.EntryDateTime1));
            dr["filler"] = IPAddress.HostToNetworkOrder(obj.filler1);
            dr["GoodTillDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.GoodTillDate1));
            dr["LastModified"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LastModified1));
              dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.header_obj.LogTime)).ToString("HH:mm:ss");
            dr["Modified_CancelledBy"] = Convert.ToChar(obj.ModCxlBy1);
            dr["nnffield"] = (long)LogicClass.DoubleEndianChange((obj.NnfField));
            dr["Open_Close"] = Convert.ToChar(obj.OpenClose1);
            dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.OrderNumber1));

            dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price1) / 100.00;


            dr["Pro_ClientIndicator"] = IPAddress.HostToNetworkOrder(obj.ProClient1);
            dr["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode1);
            dr["Settlor"] = Encoding.ASCII.GetString(obj.Settlor1);
            //  dr["TimeStamp1"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
            //dr["TimeStamp2"] = Convert.ToChar(obj.TimeStamp2);
            dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.Token1);


            dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.TotalVolRemaining1) / lotSize;
            dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId1);
            // dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
            //  dr["UserId"] = IPAddress.HostToNetworkOrder(obj);
            dr["Volume"] = IPAddress.HostToNetworkOrder(obj.Volume1) / lotSize;

            dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday1) / lotSize;

            dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.Token1)).Name);
            dr["Unique_Id"] = ((long)LogicClass.DoubleEndianChange((obj.OrderNumber1))).ToString() + (IPAddress.HostToNetworkOrder(obj.Token1)).ToString();

            Global.Instance.OrdetTable.Rows.Add(dr);

            // 2 nd leg --------------------------------------------------------------------------------------------------

            int lotSize1 = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.leg2.token)].lotsize;
            dr["Status"] = orderStatus.Rejected.ToString();
            dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber1);
            dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType1));
            dr["BranchId"] = IPAddress.HostToNetworkOrder(obj.BranchId1);
            dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.leg2.BuySell2) == 1 ? "BUY" : "SELL";
            dr["CloseoutFlag"] = Convert.ToChar(obj.leg2.OpenClose2);
            dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg2.ms_oe_obj.ExpiryDate));
            dr["InstrumentName"] = Encoding.ASCII.GetString(obj.leg2.ms_oe_obj.InstrumentName);
            dr["OptionType"] = Encoding.ASCII.GetString(obj.leg2.ms_oe_obj.OptionType);
            dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.leg2.ms_oe_obj.StrikePrice);
            dr["Symbol"] = Encoding.ASCII.GetString(obj.leg2.ms_oe_obj.Symbol);
            dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.leg2.DisclosedVol2) / lotSize1;
            dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.leg2.DisclosedVolRemaining2) / lotSize1;
            dr["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg2.ms_oe_obj.ExpiryDate));
            dr["filler"] = IPAddress.HostToNetworkOrder(obj.leg2.Fillerx2);
            dr["GoodTillDate"] = 0;
            dr["LastModified"] = 0; // LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg2.LastModified2));
            dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj .header_obj.LogTime)).ToString("HH:mm:ss");
            dr["Modified_CancelledBy"] = 0; // Convert.ToChar(obj.leg2.ModCxlBy1);
            dr["nnffield"] = (long)LogicClass.DoubleEndianChange((obj.NnfField));
            dr["Open_Close"] = Convert.ToChar(obj.leg2.OpenClose2);
            dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.OrderNumber1));
            dr["Price"] = IPAddress.HostToNetworkOrder(obj.leg2.Price2) / 100.00;
            dr["Pro_ClientIndicator"] = IPAddress.HostToNetworkOrder(obj.ProClient1);
            dr["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode1);
            dr["Settlor"] = Encoding.ASCII.GetString(obj.Settlor1);         
            dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.leg2.token);
            dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.leg2.TotalVolRemaining2) / lotSize1;
            dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId1);         
            dr["Volume"] = IPAddress.HostToNetworkOrder(obj.leg2.Volume2) / lotSize1;
            dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.leg2.VolumeFilledToday2) / lotSize1;

            dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.leg2.token)).Name);
            dr["Unique_Id"] = ((long)LogicClass.DoubleEndianChange((obj.OrderNumber1))).ToString() + (IPAddress.HostToNetworkOrder(obj.leg2.token)).ToString();
            Global.Instance.OrdetTable.Rows.Add(dr);

            #region comment leg 3
            //--------------------------------------------------------------------------------------------------------
            //=======leg 3 =========================================================================================================
            //dr["Status"] = orderStatus.Rejected.ToString();
            //dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber1);
            //dr["BookType"] = Enum.GetName(typeof(Enums.BookTypes), IPAddress.HostToNetworkOrder(obj.BookType1));
            //dr["BranchId"] = IPAddress.HostToNetworkOrder(obj.BranchId1);


            //dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.leg3.BuySell2) == 1 ? "BUY" : "SELL";

            //dr["CloseoutFlag"] = Convert.ToChar(obj.leg3.OpenClose2);
            //dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg3.ms_oe_obj.ExpiryDate));
            //dr["InstrumentName"] = Encoding.ASCII.GetString(obj.leg3.ms_oe_obj.InstrumentName);
            //dr["OptionType"] = Encoding.ASCII.GetString(obj.leg3.ms_oe_obj.OptionType);
            //dr["StrikePrice"] = IPAddress.HostToNetworkOrder(obj.leg3.ms_oe_obj.StrikePrice);
            //dr["Symbol"] = Encoding.ASCII.GetString(obj.leg3.ms_oe_obj.Symbol);
            //dr["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.leg3.DisclosedVol2) / lotSize;
            //dr["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.leg3.DisclosedVolRemaining2) / lotSize;
            //dr["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg3.ms_oe_obj.ExpiryDate));
            //dr["filler"] = IPAddress.HostToNetworkOrder(obj.leg3.Fillerx2);
            //dr["GoodTillDate"] = 0;
            //dr["LastModified"] = 0; // LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.leg2.LastModified2));
            ////   dr["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
            //dr["Modified_CancelledBy"] = 0; // Convert.ToChar(obj.leg2.ModCxlBy1);
            //dr["nnffield"] = (long)LogicClass.DoubleEndianChange((obj.NnfField));
            //dr["Open_Close"] = Convert.ToChar(obj.leg3.OpenClose2);
            //dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.OrderNumber1));

            //dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price1) / 100.00;


            //dr["Pro_ClientIndicator"] = IPAddress.HostToNetworkOrder(obj.ProClient1);
            //dr["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode1);
            //dr["Settlor"] = Encoding.ASCII.GetString(obj.Settlor1);
            ////  dr["TimeStamp1"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss");
            ////dr["TimeStamp2"] = Convert.ToChar(obj.TimeStamp2);
            //dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.Token1);


            //dr["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.leg3.TotalVolRemaining2) / lotSize;
            //dr["TraderId"] = IPAddress.HostToNetworkOrder(obj.TraderId1);
            //// dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
            ////  dr["UserId"] = IPAddress.HostToNetworkOrder(obj);
            //dr["Volume"] = IPAddress.HostToNetworkOrder(obj.leg3.Volume2) / lotSize;

            //dr["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.leg3.VolumeFilledToday2) / lotSize;

            //dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.leg3.token)).Name);
            //dr["Unique_Id"] = ((long)LogicClass.DoubleEndianChange((obj.OrderNumber1))).ToString() + (IPAddress.HostToNetworkOrder(obj.leg3.token)).ToString();
            //Global.Instance.OrdetTable.Rows.Add(dr);
            //======================================================================================================================

            #endregion


            Global.Instance.OrdetTable.Rows.Add(dr);
            //if (this.InvokeRequired)
            //{
            //    MethodInvoker del = delegate
            //    {
            //        DGV.Refresh();
            //       DGV2.Refresh();

            //    };
            //    this.Invoke(del);
            //    return;
            //}
          
               }
           }
            catch(Exception ex)
           {
                
               // MessageBox.Show("2155 transaction  code "+ex.StackTrace);
            }
             
        }



        public void THRL_ORDER_ERROR(byte[] buffer) //-- 2156
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }
       
        public void TWOL_ORDER_CXL_CONFIRMATION(byte[] buffer) //-- 2131
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ORDER_ERROR_OUTDel(TWOL_ORDER_CXL_CONFIRMATION), buffer);
                    return;
                }
                object ob = new object();
                lock (ob)
                {
                  
                    MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
                    string uno = ((long)LogicClass.DoubleEndianChange((obj.OrderNumber1))).ToString() + (IPAddress.HostToNetworkOrder(obj.Token1)).ToString();
                    DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_Id ='" + uno + "'");
                    if (dr.Length > 0)
                    {
                        dr[0]["Status"] =  orderStatus.Cancel.ToString();
                        dr[0]["Price"] = IPAddress.HostToNetworkOrder(obj.Price1) / 100.00;
                        dr[0]["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.header_obj.LogTime)).ToString("HH:mm:ss");
                    //   Global.Instance.OrdetTable.Rows.Remove(dr[0]);
                     //   Global.Instance.OrdetTable.AcceptChanges();
                    }

                    DataRow[] dr1 = Global.Instance.OrdetTable.Select("Unique_Id = '" + ((long)LogicClass.DoubleEndianChange((obj.OrderNumber1))).ToString() + (IPAddress.HostToNetworkOrder(obj.leg2.token)).ToString() + "'");
                    if (dr1.Length > 0)
                    {
                        dr1[0]["Status"] =   orderStatus.Cancel.ToString();
                        dr[0]["Price"] = IPAddress.HostToNetworkOrder(obj.leg2.Price2) / 100.00;
                        dr[0]["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.header_obj.LogTime)).ToString("HH:mm:ss");
                      //  Global.Instance.OrdetTable.Rows.Remove(dr1[0]);
                     //   Global.Instance.OrdetTable.AcceptChanges();
                    }

                  //  Global.Instance.OrdetTable.AcceptChanges();
                    //if (!DGV.InvokeRequired)
                    //{
                    //    DGV.Refresh();
                    //}
                    //if (!DGV2.InvokeRequired)
                    //{
                    //    DGV2.Refresh();
                    //}
                }
            }
            catch
            {

            }
        }

        public void THRL_ORDER_CXL_CONFIRMATION(byte[] buffer) //-- 2132
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }


        #endregion NNF OUT Messages

        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Modifiers == Keys.Shift)
            {
                if (e.KeyCode == Keys.F2)
                {
                    foreach (DataGridViewRow dgvr in DGV.SelectedRows)
                    {
                       
                            ModifyOrder(dgvr);
                      
                    }
                }
            }

            else if (e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewRow dgvr in DGV.SelectedRows)
                {
                    CancelOrder(dgvr);
                }
            }
        }


        private void ModifyOrder(DataGridViewRow Dr)
        {

            using (var frmord = new FrmOrderEntry())
            {
               // int lotSize = CSV_Class.cimlist.Where(q => q.Token == Convert.ToInt32(Dr.Cells["TokenNo"])).Select(a => a.BoardLotQuantity).First();

                frmord.lblOrderMsg.Text = "Modify " + Dr.Cells["Symbol"].Value + " " + Dr.Cells["Buy_SellIndicator"].Value.ToString() + "(" + Dr.Cells["OrderNumber"].Value + ")  ";
                frmord.lblOrderMsg.BackColor = Dr.Cells["Buy_SellIndicator"].Value.ToString() == "BUY" ? Color.Blue : Color.Red;
                frmord.LEG_PRICE = Convert.ToDouble(Dr.Cells["Price"].Value);
                frmord.LEG_SIZE = Convert.ToInt32(Dr.Cells["Volume"].Value);
                int x = (Screen.PrimaryScreen.WorkingArea.Width - frmord.Width) / 2;
                int y = (Screen.PrimaryScreen.WorkingArea.Height - frmord.Height) - 50;
                frmord.Location = new Point(x, y);

                if (frmord.ShowDialog(this) == DialogResult.OK)
                {
                    if (frmord.FormDialogResult == (int)OrderEntryButtonCase.SUBMIT)
                    {

                        NNFInOut.Instance.ORDER_MOD_IN_TR( Convert.ToInt64(Dr.Cells["OrderNumber"].Value),
                             frmord.LEG_SIZE*CSV_Class.cimlist.Where(q => q.Token == Convert.ToInt32(Dr.Cells["TokenNo"].Value)).Select(a => a.BoardLotQuantity).First() ,
                               Convert.ToInt32(frmord.LEG_PRICE*100));
                    }
                    }
                }                        
            }
    

        private void CancelOrder(DataGridViewRow Dr)
        {
            NNFInOut.Instance.ORDER_CANCEL_IN_TR(Convert.ToInt64(Dr.Cells["OrderNumber"].Value));
        }

        public void AutoSave()
        {
            DataTable dt = new DataTable("ExcelTable");
            Excel.ExcelUtlity2 obj = new Excel.ExcelUtlity2();
            DataView dv = new DataView(Global.Instance.OrdetTable);
            dv.RowFilter = "status =  'Traded'";
            dv.ToTable("ExcelTable");
            obj.WriteDataTableToExcel(Global.Instance.OrdetTable, "Excel Report", Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + " Default.xlsx", "Details");
            
        }
        private void frmGenOrderBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            
           
                //DataTable dt = new DataTable("ExcelTable");
                //Excel.ExcelUtlity2 obj = new Excel.ExcelUtlity2();
                //DataView dv = new DataView(Global.Instance.OrdetTable);
                //dv.RowFilter = "status =  'Traded'";
                //dv.ToTable("ExcelTable");
                //obj.WriteDataTableToExcel(Global.Instance.OrdetTable, "Excel Report", Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + " Default.xlsx", "Details");
            

            Settings.Default.Window_LocationOBook = this.Location;

            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.Window_SizeOBook = this.Size;
            }
            else
            {
                Settings.Default.Window_SizeOBook = this.RestoreBounds.Size;
            }

            // Save settings
            Settings.Default.Save();
            e.Cancel = true;

            this.Hide();
        }
       
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try { 
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.Filter = "*.xlsx|*.*";
            if(saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
             //   DataTable dt = new DataTable("ExcelTable");
                Excel.ExcelUtlity obj = new Excel.ExcelUtlity();
                DataView dv = new DataView(Global.Instance.OrdetTable);
                dv.RowFilter = "status =  'Traded'";
                dv.ToTable("ExcelTable");
                DataTable dt = dv.ToTable("ExcelTable");
                obj.WriteDataTableToExcel(dt, "Excel Report", saveFileDialog1.FileName, "Details");
           }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Order Book -  Funtion Name-  btnExportExcel_Click  " + ex.Message);
            }
        }

       

        private void btncancelall_Click(object sender, EventArgs e)
        {
            timerCancelbtn.Start();
            btncancelall.Enabled = false;
            byte[] buffer = DataPacket.RawSerialize(new C_LotIN());
          //  NNFHandler.Instance.Publisher(MsgType.CANCELALL, buffer);

            NNFInOut.Instance.KILL_switch_in();
            //try
            //{
            //    for (int i = 0; i < DGV.Rows.Count; i++)
            //    {
            //        NNFInOut.Instance.KILL_switch_in(Convert.ToInt64(DGV.Rows[i].Cells["OrderNumber"].Value.ToString()));
            //        break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Order Book -  Funtion Name-  btnExportExcel_Click  " + ex.Message);
            //}           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            { 
            Profile_forOrderBook pfltrade_book = new Profile_forOrderBook();
            var config = new Config { GroupName = null };
            if (pfltrade_book.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewColumn dc in DGV.Columns)
                {

                    this.DGV.Columns[dc.HeaderText.Replace(" ", "")].Visible = true;
                    this.DGV2.Columns[dc.HeaderText.Replace(" ", "")].Visible = true;
                }
                String GetProfileName = pfltrade_book.GetProfileName();

                DataSet ds = new DataSet();
                ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Order_Profiles" + Path.DirectorySeparatorChar + GetProfileName + ".xml");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string st = ds.Tables[0].Rows[i]["Input"].ToString();
                    this.DGV.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = false;
                    this.DGV2.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = false;
                }
                config.SetValue("GenOrderBook", Convert.ToString(0), GetProfileName);
            }
            }
            catch (Exception ex)
            {
            //    MessageBox.Show("Order Book -  Funtion Name-  toolStripButton1_Click  " + ex.Message);
            }
            
        }

     
        private void DGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                if (DGV.InvokeRequired)
                {
                    DGV.Invoke(new On_DataPaintdDelegate(DGV_RowPrePaint), sender, e);
                    return;
                }
                DGV.PerformLayout();
                if (DGV.Rows[e.RowIndex].Cells["Buy_SellIndicator"].Value.ToString() == "BUY")
                {                
                    DGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                }
                else
                {                 
                    DGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
              //  MessageBox.Show("Order Book -  Funtion Name-  DGV_RowPrePaint  " + ex.Message);
            }
        }
       
        private void DGV2_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                DGV2.PerformLayout();
                if (DGV2.InvokeRequired)
                {
                    DGV2.Invoke(new On_DataPaintdDelegate(DGV2_RowPrePaint), sender, e);
                    return;
                }
                if (DGV2.Rows[e.RowIndex].Cells["Buy_SellIndicator"].Value.ToString().Trim() == "BUY")
                {
                    //  DGV.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    DGV2.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                }
                else
                {
                 DGV2.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Order Book -  Funtion Name-  DGV2_RowPrePaint  " + ex.Message);
            }
        }

        private void DGV_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
          
        }
        private void DGV2_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
           
        }
        // datatable  events 

        private static void Row_Deleted(object sender, DataRowChangeEventArgs e)
        {
            Console.WriteLine("Row_Deleted Event: name={0}; action={1}",
                e.Row["name", DataRowVersion.Original], e.Action);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(NNFHandler.Instance.cout.ToString());
        }

        private void DGV2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV2.CurrentCell.Selected == true)
                DGV2.ClearSelection();

            else
            {
                DGV2.DefaultCellStyle.SelectionBackColor = DGV2.DefaultCellStyle.BackColor;
                DGV2.DefaultCellStyle.SelectionForeColor = DGV2.DefaultCellStyle.ForeColor;
                DGV2.CurrentCell.Selected = true;
            }
        }

        private void lblcount_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
         
           
           
          
           
        }

        private void timerCancelbtn_Tick(object sender, EventArgs e)
        {
            btncancelall.Enabled = true;
            timerCancelbtn.Stop();
        }
        
       
    }
   
}
