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

namespace OrderBook.GUI
{
    public partial class FrmOrderBook : DockContent
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
        
        DataView dvOrderBook;
        
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

        /*
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

                MessageBox.Show(ex.Message);
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
                    if (dr[0]["Status"].ToString() != orderStatus.Traded.ToString())
                    {
                        dr[0]["Status"] = orderStatus.Rejected.ToString();
                        dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price)) / 100.00;
                        dr[0]["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
                        dr[0]["RejectReason"] = Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
                        //dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
                    }
                    else
                    {
                        LogWriterClass.logwritercls.logs("20042 trasactioncode", ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString());
                    }
                    //   dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price))/100.00;
                }
                if (!DGV.InvokeRequired)
                {
                    DGV.Refresh();
                }



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

                MessageBox.Show("Order Book -  Funtion Name-  ORDER_MOD_REJ_OUT  " + ex.Message);
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
                    if (dr[0]["Status"].ToString() != "Traded")
                    {
                        dr[0]["Status"] = orderStatus.Rejected.ToString();
                        dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price)) / 100.00;
                        dr[0]["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
                        dr[0]["RejectReason"] = Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
                        //dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
                    }
                    else
                    {
                        LogWriterClass.logwritercls.logs("20072 trasactioncode", ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString());
                    }
                    //   dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price))/100.00;
                }
                if (!DGV.InvokeRequired)
                {
                    DGV.Refresh();
                }



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

                MessageBox.Show("Order Book -  Funtion Name-  ORDER_MOD_REJ_OUT  " + ex.Message);
            }
        }

       
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

                    int lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.TokenNo)].lotsize;  // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.TokenNo)).Select(a => a.BoardLotQuantity).First();
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

                    //   DataRow[] d_r = Global.Instance.OrdetTable.Select("OrderNumber = '" +Convert.ToString( (long)LogicClass.DoubleEndianChange((obj.OrderNumber))) + "'");

                    Global.Instance.OrdetTable.Rows.Add(dr);
                    if (!DGV.InvokeRequired)
                    {
                        DGV.Refresh();

                    }

                    if (!DGV2.InvokeRequired)
                    {
                        DGV2.Refresh();

                    }
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Order Book -  Funtion Name-  ORDER_CONFIRMATION_TR  " + ex.Message);
            }


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

                    int lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.TokenNo)].lotsize; ; // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.TokenNo)).Select(a => a.BoardLotQuantity).First();
                    DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString() + "'");
                    if (dr.Length == 0)
                    {
                        LogWriterClass.logwritercls.logs("20074trasactioncode", ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString());
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
                    if (!DGV.InvokeRequired)
                    {
                        DGV.Refresh();

                    }

                    if (!DGV2.InvokeRequired)
                    {
                        DGV2.Refresh();

                    }
                   
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Order Book -  Funtion Name-  ORDER_MOD_CONFIRMATION_TR  " + ex.Message);
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
                               
                                dr[0]["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.EntryDateTime));
                                dr[0]["LastModified"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LastModified));
                                dr[0]["Modified_CancelledBy"] = Convert.ToChar(obj.Modified_CancelledBy);
                                dr[0]["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
                                dr[0]["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                               
                                dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                                if (!DGV.InvokeRequired)
                                {
                                    DGV.Refresh();
                                }

                                if (!DGV2.InvokeRequired)
                                {
                                    DGV2.Refresh();

                                }
                                
                                break;
                            }
                        case 3:
                            {
                                var ob = new Order((int)_Type.MS_SPD_OE_REQUEST);
                                Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber), out ob);
                                

                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Order Book -  Funtion Name-  ORDER_CXL_CONFIRMATION_TR  " + ex.Message);
            }
        }

        public void PRICE_CONFIRMATION_TR(byte[] buffer) //-- 20012
        {
            MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
        }

        public void TRADE_CONFIRMATION_TR(byte[] buffer) //-- 20222
        {
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
                    //       else
                    //          ch = 3;
                    switch (ch)
                    {
                        case 1:
                            {
                                var ob = new Order((int)_Type.MS_OE_REQUEST);
                               
                                break;
                            }
                        case 2:
                            {

                                var ob = new Order((int)_Type.MS_OE_RESPONSE_TR);

                               
                                Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber), out ob);
                                int lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.Token)].lotsize;    // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.Token)).Select(a => a.BoardLotQuantity).First();
                                DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.Token)).ToString() + "'");
                                dr[0]["Status"] = orderStatus.Traded.ToString();
                                dr[0]["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume) / lotSize;
                                dr[0]["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) / lotSize;
                                dr[0]["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                                dr[0]["FillPrice"] = IPAddress.HostToNetworkOrder(obj.FillPrice) / 100.00;
                                dr[0]["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.RemainingVolume) / lotSize;
                                dr[0]["Volume"] = IPAddress.HostToNetworkOrder(obj.FillQuantity) / lotSize;
                                dr[0]["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
                                dr[0]["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss.fff");
                                dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                                dr[0]["FillNumber"] = IPAddress.HostToNetworkOrder(obj.FillNumber);

                                if (!DGV.InvokeRequired)
                                {
                                    DGV.Refresh();
                                }
                                if (!DGV2.InvokeRequired)
                                {
                                    DGV2.Refresh();
                                }
                               
                                break;
                            }
                        case 3:
                            {
                                var ob = new Order((int)_Type.MS_SPD_OE_REQUEST);
                                try
                                {
                                    //       Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber), out ob);
                                }
                                catch { }

                                var v = Global.Instance.Ratio.Where(a => a.Key == (IPAddress.HostToNetworkOrder(obj.Token).ToString() + IPAddress.HostToNetworkOrder(obj.Contr_dec_tr_Obj.StrikePrice).ToString() + System.Text.ASCIIEncoding.UTF8.GetString(obj.Contr_dec_tr_Obj.OptionType))).Select(b => b.Value).ToList();
                                var val = Convert.ToInt32(v.FirstOrDefault().ToString());
                                int lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.Token)].lotsize;    // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.Token)).Select(a => a.BoardLotQuantity).First();
                                DataRow[] dr = Global.Instance.OrdetTable.Select("Unique_id = '" + ((long)LogicClass.DoubleEndianChange((obj.ResponseOrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.Token)).ToString() + "'");
                                dr[0]["Status"] = orderStatus.Traded.ToString();
                                dr[0]["Buy_SellIndicator"] = ((BUYSELL)IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator)).ToString();
                                dr[0]["DisclosedVolume"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolume) / lotSize;
                                dr[0]["DisclosedVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) / lotSize;
                                dr[0]["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
                                dr[0]["FillPrice"] = IPAddress.HostToNetworkOrder(obj.FillPrice) / 100.00;
                                dr[0]["TotalVolumeRemaining"] = IPAddress.HostToNetworkOrder(obj.RemainingVolume) / lotSize;
                               
                                dr[0]["VolumeFilledToday"] = IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) / lotSize;
                                dr[0]["LogTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LogTime)).ToString("HH:mm:ss.fff");
                                dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.TransactionCode);
                               
                                if (!DGV.InvokeRequired)
                                {
                                    DGV.Refresh();
                                }
                                if (!DGV2.InvokeRequired)
                                {
                                    DGV2.Refresh();
                                }
                               
                                break;
                            }

                    }





                    if (!this.InvokeRequired)
                    {
                        MethodInvoker del = delegate
                        {



                            frmNetBook.Instance.netposion(IPAddress.HostToNetworkOrder(obj.Token));
                            frmTradeBook.Instance.DGV.Refresh();
                            //   AutoSave();
                            frmTradeBook.Instance.lblnooftrade.Text = "No Of Trade  =" + frmTradeBook.Instance.DGV.Rows.Count;

                            if (IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1)
                                frmTradeBook.Instance.lblb_q.Text = (Convert.ToInt32(frmTradeBook.Instance.lblb_q.Text == "0" ? "0" : frmTradeBook.Instance.lblb_q.Text) + IPAddress.HostToNetworkOrder(obj.FillQuantity)).ToString();
                            else
                                frmTradeBook.Instance.lbls_q.Text = (Convert.ToInt32(frmTradeBook.Instance.lbls_q.Text == "0" ? "0" : frmTradeBook.Instance.lbls_q.Text) + IPAddress.HostToNetworkOrder(obj.FillQuantity)).ToString();

                            if (IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1)
                            {
                                frmTradeBook.Instance.lblb_V.Text = Global.Instance.OrdetTable.AsEnumerable().Where(r => r.Field<string>("Status") == "Traded" && r.Field<string>("Buy_SellIndicator") == "BUY").Sum(r => r.Field<Double>("FillPrice") * Convert.ToDouble(r.Field<string>("Volume"))).ToString();
                            }
                            else
                                frmTradeBook.Instance.lbls_v.Text = Global.Instance.OrdetTable.AsEnumerable().Where(r => r.Field<string>("Status") == "Traded" && r.Field<string>("Buy_SellIndicator") == "SELL").Sum(r => r.Field<Double>("FillPrice") * Convert.ToDouble(r.Field<string>("Volume"))).ToString();
                        };
                        this.Invoke(del);
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Order Book -  Funtion Name-  TRADE_CONFIRMATION_TR  " + ex.Message);
            }
        }


        //Order and Trade Management

        delegate void ORDER_ERROR_OUTDel(byte[] buffer);

        public void ORDER_ERROR_OUT(byte[] buffer) //-- 2231
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ORDER_ERROR_OUTDel(ORDER_ERROR_OUT), buffer);
                return;
            }
            try
            {
                MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
                int lotSize = Holder._DictLotSize[IPAddress.HostToNetworkOrder(obj.TokenNo)].lotsize; ; // CSV_Class.cimlist.Where(q => q.Token == IPAddress.HostToNetworkOrder(obj.TokenNo)).Select(a => a.BoardLotQuantity).First();
                DataRow dr = Global.Instance.OrdetTable.NewRow();
               
                dr["Status"] = orderStatus.Rejected.ToString();
                dr["AccountNumber"] = Encoding.ASCII.GetString(obj.AccountNumber);
              
                dr["Buy_SellIndicator"] = IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) == 1 ? "BUY" : "SELL";
                
                dr["ExpiryDate"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.contract_obj.ExpiryDate));
              
                dr["Symbol"] = Encoding.ASCII.GetString(obj.contract_obj.Symbol);
                
                dr["EntryDateTime"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.EntryDateTime));
                
                dr["LastModified"] = LogicClass.ConvertFromTimestamp(IPAddress.HostToNetworkOrder(obj.LastModified));
                dr["LogTime"] = System.DateTime.Now.ToShortTimeString();
                dr["Modified_CancelledBy"] = Convert.ToChar(obj.Modified_CancelledBy);
                dr["nnffield"] = (long)LogicClass.DoubleEndianChange((obj.nnffield));
                
                dr["OrderNumber"] = (long)LogicClass.DoubleEndianChange((obj.OrderNumber));
                dr["Price"] = IPAddress.HostToNetworkOrder(obj.Price) / 100.00;
               
                dr["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
                
                dr["TokenNo"] = IPAddress.HostToNetworkOrder(obj.TokenNo);
                
                dr["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
                dr["Volume"] = IPAddress.HostToNetworkOrder(obj.Volume) / lotSize;
                
                dr["RejectReason"] = Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
                dr["FullName"] = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == IPAddress.NetworkToHostOrder(obj.TokenNo)).Name);
                dr["Unique_id"] = ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString() + (IPAddress.HostToNetworkOrder(obj.TokenNo)).ToString();
                Global.Instance.OrdetTable.Rows.Add(dr);


               
            }
            catch (Exception ex)
            {

                MessageBox.Show("Order Book -  Funtion Name-  TRADE_CONFIRMATION_TR  " + ex.Message);
            }
        }



        public void PRICE_CONFIRMATION(byte[] buffer) //-- 2012
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
        }


        public void ORDER_CONFIRMATION_OUT(byte[] buffer) //-- 2073
        {
            try
            {
                MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
                Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber), new Order((int)_Type.MS_OE_REQUEST));
                Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].mS_OE_REQUEST = obj;

               
            }
            catch (Exception ex)
            {

                MessageBox.Show("Order Book -  Funtion Name-  ORDER_CONFIRMATION_OUT  " + ex.Message);
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
                if (dr.Length > 0)
                {
                    if (dr[0]["Status"].ToString() != orderStatus.Traded.ToString())
                    {
                        dr[0]["Status"] = orderStatus.Rejected.ToString();
                        dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price)) / 100.00;
                        dr[0]["ReasonCode"] = IPAddress.HostToNetworkOrder(obj.ReasonCode);
                        dr[0]["RejectReason"] = Enum.GetName(typeof(Enums.Error_Codes), IPAddress.HostToNetworkOrder(obj.header_obj.ErrorCode));
                        dr[0]["TransactionCode"] = IPAddress.HostToNetworkOrder(obj.header_obj.TransactionCode);
                    }
                    else
                    {
                        LogWriterClass.logwritercls.logs("2042trasactioncode", ((long)LogicClass.DoubleEndianChange((obj.OrderNumber))).ToString());
                    }
                    //   dr[0]["Price"] = (IPAddress.HostToNetworkOrder(obj.Price))/100.00;
                }
                if (!DGV.InvokeRequired)
                {
                    DGV.Refresh();
                }



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

                MessageBox.Show("Order Book -  Funtion Name-  ORDER_MOD_REJ_OUT  " + ex.Message);
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

                MessageBox.Show("Order Book -  Funtion Name-  ORDER_MOD_CONFIRM_OUT  " + ex.Message);
            }
        }
        public void ORDER_CANCEL_CONFIRM_OUT(byte[] buffer) //-- 2075
        {
            try
            {
                MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
                var ob = new Order((int)_Type.MS_OE_REQUEST);
                Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber), out ob);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Order Book -  Funtion Name-  ORDER_CANCEL_CONFIRM_OUT  " + ex.Message);
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

            if (!DGV.InvokeRequired)
            {
                DGV.Refresh();

            }

            if (!DGV2.InvokeRequired)
            {
                DGV2.Refresh();

            }

        }

        public void ORDER_CXL_REJ_OUT(byte[] buffer) //-- 2072
        {
            try
            {
                
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

               

            }
            catch (Exception ex)
            {

                MessageBox.Show("Order Book -  Funtion Name-  ORDER_CXL_REJ_OUT  " + ex.Message);
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
#endregion
        */
        public FrmOrderBook()
        {
            InitializeComponent();
            Load += FrmOrderBook_Load;
            KeyDown += FrmOrderBook_KeyDown;
            FormClosed += FrmOrderBook_FormClosed;

            ToolStripSeparator tlsmod = new ToolStripSeparator();
            dgvOrderBook.cmsColumn.Items.Insert(0, tlsmod);

            tlsmiModifySelected = new ToolStripMenuItem();
            tlsmiModifySelected.Name = "tlsmiModifySelected";
            tlsmiModifySelected.Text = "Modify Order";
            dgvOrderBook.cmsColumn.Items.Insert(0, tlsmiModifySelected);

            ToolStripSeparator tls = new ToolStripSeparator();
            dgvOrderBook.cmsColumn.Items.Add(tls);

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
            dgvOrderBook.cmsColumn.Items.Add(items);

            tlsmiCancelAll.Click += new EventHandler(tlsmiCancelAll_Click);
            tlsmiCancelBuy.Click += new EventHandler(tlsmiCancelBuy_Click);
            tlsmiCancelSell.Click += new EventHandler(tlsmiCancelSell_Click);
            tlsmiCancelSelected.Click += new EventHandler(tlsmiCancelSelected_Click);
            tlsmiModifySelected.Click += new EventHandler(tlsmiModifySelected_Click);

            dgvOrderBook.DataError += dgvOrderBook_DataError;
        }

        void dgvOrderBook_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        void FrmOrderBook_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                    Close();
                bool Ismodifypanelvisible;
                dgvOrderBook.Focus();
                dgvOrderBook.Select();
                
                //    AppGlobal.OrderLogger.ShowToErrorMessageLog("Manual Trading Not Allowed.");
                
                
            }
            catch (Exception ex)
            {
                // AppGlobal.OrderLogger.WriteToLogFile(AtsMethods.GetErrorMessage(ex, "OrderBook_KeyDown"));
               
            }
             
        }

        void FrmOrderBook_Load(object sender, EventArgs e)
        {
            dvOrderBook = new DataView(CommonData.CommonData.dtOrderBook);
            dgvOrderBook.BindSourceView = dvOrderBook;
            dgvOrderBook.UniqueName =  "OB";
            dgvOrderBook.LoadSaveSettings();
                        
            ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.Buy_SellIndicator]).FilteringEnabled = true;
            ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.FillNumber]).FilteringEnabled = true;
            ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.FullName]).FilteringEnabled = true;
            ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.InstrumentName]).FilteringEnabled = true;
            ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.ReasonCode]).FilteringEnabled = true;
            ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.Status]).FilteringEnabled = true;
            ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.Symbol]).FilteringEnabled = true;
            ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.TokenNo]).FilteringEnabled = true;
            ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.TransactionCode]).FilteringEnabled = true;
            ((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.filler]).FilteringEnabled = true;

            //((DataGridViewAutoFilterTextBoxColumn)dgvOrderBook.Columns[WatchConst.ExpiryDate]).FilteringEnabled = true;
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
              //ComData.DtOrderBook.DefaultView.RowFilter = _filter;
              dvOrderBook.RowFilter = _filter;
          }
         */

        }

        void FrmOrderBook_FormClosed(object sender, FormClosedEventArgs e)
        {
            CommonData.CommonData.frmOrderBook = null;
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

        void tlsmiModifySelected_Click(object sender, EventArgs e)
        {
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
            /*  try
              {
                  foreach (DataGridViewRow row in dgvOrderBook.SelectedRows)
                  {
                      if (Convert.ToString(row.Cells[WatchConst.OrderStatus].Value) == AtsEnums.OrderStatus.EPending.ToString())
                      {
                          OrderProcess.CancelOrder(Convert.ToUInt32(row.Cells[WatchConst.IntOrderNo].Value), Convert.ToUInt16(row.Cells[WatchConst.UniqueId].Value));
                      }
                      else
                      {
                          AppGlobal.Logger.ShowToMessageLog("This Order Can not be Cancelled.", color: "Red");
                      }
                  }
              }
              catch (Exception ex)
              {
                  AppGlobal.Logger.WriteinFileWindowAndBox(ex, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
              }*/
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
    }
}

