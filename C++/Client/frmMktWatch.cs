using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using csv;
using Structure;
using System.IO;
using System.Xml;
using System.Collections;
using System.Net;
using AMS.Profile;
using System.ComponentModel;
using System.Globalization;



namespace Client
{
    public partial class frmMktWatch : Form
    {
      
        public string GetProfileName { get; set; }
        private readonly Dictionary<int, DataGridViewRow> _mwatchDict = new Dictionary<int, DataGridViewRow>();
        private DataGridViewCellStyle _makeItBlack;
        private DataGridViewCellStyle _makeItBlue;
        private DataGridViewCellStyle _makeItRed;
        DataTable dt_mktwatch = new DataTable("mktwatch");
        List<long> T = new List<long>();
        long date = 0;
        public frmMktWatch()
        {
            InitializeComponent();

           // this.WindowState = FormWindowState.Maximized;
            //=================================================================================================================================================
        
            
            //===================================================================================================================================================

           
            _makeItRed = new DataGridViewCellStyle();
            _makeItBlue = new DataGridViewCellStyle();
            _makeItBlack = new DataGridViewCellStyle();

            _makeItRed.BackColor = Color.IndianRed;

            _makeItBlue.BackColor = Color.Blue;
            _makeItBlack.BackColor = Color.Black;
          
            if (!Directory.Exists(Application.StartupPath +Path.DirectorySeparatorChar +"Mwatch"))
            {
                Directory.CreateDirectory(Application.StartupPath + Path.DirectorySeparatorChar + "Mwatch");
            }
            if (!Directory.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles"))
            {
                Directory.CreateDirectory(Application.StartupPath + "Profiles");
            }
           

        }


        void InstType()
        {
            comboB_Symbol.Items.Clear();
            string[] symm = CSV_Class.cimlist.Where(a => a.InstrumentName == comboBInstType.Text).OrderBy(we => we.Symbol).OrderBy(q1 => q1.Symbol).Select(q => q.Symbol).Distinct().ToArray();
               
            comboB_Symbol.Items.AddRange(symm);


            combo_OptionType.Enabled = true;
            combo_StrikePrice.Enabled = true;
            combo_Exoiry.Items.Clear();
            combo_OptionType.Items.Clear();
            combo_StrikePrice.Items.Clear();


            if (comboBInstType.Text == "FUTIVX" || comboBInstType.Text == "FUTIDX" || comboBInstType.Text == "FUTSTK")
            {

                combo_OptionType.Enabled = false;
                combo_StrikePrice.Enabled = false;
                combo_Exoiry.Items.Clear();
                combo_OptionType.Items.Clear();
                combo_StrikePrice.Items.Clear();


            }
        }

        public static DateTime ConvertFromTimestamp(long timstamp)
        {
            DateTime datetime = new DateTime(1980, 1, 1, 0, 0, 0, 0);
            return datetime.AddSeconds(timstamp);
        }
        void Exoirry()
        {
            combo_Exoiry.Items.Clear();
            combo_Exoiry.Text = "";



            T = CSV_Class.cimlist.Where(a => a.Symbol == comboB_Symbol.Text && a.InstrumentName == comboBInstType.Text).OrderBy(s => s.ExpiryDate).Select(d => d.ExpiryDate).Distinct().ToList();
            // EXPcomboBox5.Items.AddRange(Enumerable.Range(1, T.Count()).Select(x => x.ToString()));
            foreach (long ex in T)
            {
                string on = ConvertFromTimestamp(ex).ToShortDateString();
                combo_Exoiry.Items.Add(on);
            }

            combo_OptionType.Text = "";
            combo_StrikePrice.Text = "";

        }
        ///////////////////////////////////////////////////////////////////////////
        void optionType()
        {
            combo_OptionType.Text = "";

            string[] op = CSV_Class.cimlist.Where(a => a.ExpiryDate == date && a.InstrumentName == comboBInstType.Text && a.Symbol == comboB_Symbol.Text).Select(s => s.OptionType).Distinct().ToArray();

            combo_OptionType.Items.AddRange(op);

            combo_StrikePrice.Text = "";

        }
        /// //////////////////////////////////////////////////////////////
        void strike_prise()
        {
            var p = CSV_Class.cimlist.Where(a => a.ExpiryDate == date && a.InstrumentName == comboBInstType.Text && a.Symbol == comboB_Symbol.Text && a.OptionType == combo_OptionType.Text).OrderBy(w => w.StrikePrice).Select(a => a.StrikePrice).Distinct().ToArray();
            foreach (int x in p)
                combo_StrikePrice.Items.Add(x);

        }
        //////////////////////////////////////////////////////////////////////
        public void lavesho(string token1)
        {

            //label8.Text = token1;

        }
                
        private void btnprofile_Click(object sender, EventArgs e)
        {
            var frmprf = new frmProfile();

            foreach (DataGridViewColumn dc in DGV.Columns)
            {
                //frmprf.lbxPrimary.Items.Add(dc.HeaderText);
                if (!frmprf.lbxSecondary.Items.Contains(dc.HeaderText))
                {
                    frmprf.lbxPrimary.Items.Add(dc.HeaderText);
                   // this.DGV.Columns[dc.HeaderText].Visible = false;
                }
                //else
                //{
                //    this.DGV.Columns[dc.HeaderText].Visible = true;
                //}
            }
            //if (frmprf.ShowDialog() == DialogResult.OK)
            //{
            //    GetProfileName = frmprf.GetProfileName();

            //    LoadDgcOlumns(Application.StartupPath +Path.DirectorySeparatorChar+ "Profiles" + Path.DirectorySeparatorChar + GetProfileName + ".xml");
            //}


           

            if (frmprf.ShowDialog() == DialogResult.OK)
            {

                foreach (DataGridViewColumn dc in DGV.Columns)
                {
                    this.DGV.Columns[dc.HeaderText.Replace(" ", "")].Visible = true;
                }
                String GetProfileName = frmprf.GetProfileName();
                var config = new Config { GroupName = null };
                DataSet ds = new DataSet();
                ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + GetProfileName + ".xml");
                if (ds.Tables.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string st = ds.Tables[0].Rows[i]["Input"].ToString();
                    this.DGV.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = false;
                }
                config.SetValue("MKT_Profile", Convert.ToString(0), GetProfileName);
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

        private void LoadDgcOlumns(String fileName)
        {
            var clmns = new ArrayList();

            if (File.Exists(fileName))
            {
                var settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;

                using (XmlReader reader = XmlReader.Create(fileName, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && "Column" == reader.LocalName)
                        {
                            reader.MoveToFirstAttribute();
                            clmns.Add(reader.Value);
                        }
                    }
                }
            }
        }

        public static DataGridViewColumn GetGridColumn(string ColumnName, string DataType, Boolean KeepNull = true,
           Boolean KeepUnique = false)
        {
            //DataGridViewColumn dc = null;
            //dc = new DataGridViewColumn();
            //dc.Name = ColumnName.Trim();
            //dc.DataPropertyName = ColumnName.Trim();
            //dc.ValueType = System.Type.GetType("System." + DataType);
            //dc.HeaderText = ColumnName.Trim();


            var dataGridViewColumn = new DataGridViewColumn();
            DataGridViewCell dataGridViewCell = new DataGridViewTextBoxCell();
            
            dataGridViewCell.ValueType = Type.GetType("System." + DataType);
            dataGridViewColumn.DataPropertyName = ColumnName.Trim();
            dataGridViewColumn.HeaderText = ColumnName.Trim();
            dataGridViewColumn.CellTemplate = dataGridViewCell;
            dataGridViewColumn.Name = ColumnName.Trim();

            //dc.Caption  = "Abc" + ColumnName;
            return dataGridViewColumn;
        }


        private void ReadyDatatable()
        {
            DGV.Columns.Add(GetGridColumn("InstrumentName", "String"));
            DGV.Columns.Add(GetGridColumn("Description", "String"));
            DGV.Columns.Add(GetGridColumn("UniqueIdentifier", "String"));
            DGV.Columns.Add(GetGridColumn("Symbol", "String"));
            DGV.Columns.Add(GetGridColumn("ExpiryDate", "String"));
            DGV.Columns.Add(GetGridColumn("OptionType", "String"));
            DGV.Columns.Add(GetGridColumn("StrikePrice", "String"));

            DGV.Columns.Add(GetGridColumn("Bid", "Double"));
            DGV.Columns.Add(GetGridColumn("BidQ", "Int32"));
            DGV.Columns.Add(GetGridColumn("TBidOrder", "Int32"));
            DGV.Columns.Add(GetGridColumn("Ask", "Double"));
            DGV.Columns.Add(GetGridColumn("AskQ", "Int32"));
            DGV.Columns.Add(GetGridColumn("TAskOrder", "Int32"));
                     

            DGV.Columns.Add(GetGridColumn("LTP", "Double"));
            DGV.Columns.Add(GetGridColumn("LTQ", "Int32"));
            DGV.Columns.Add(GetGridColumn("LTT", "String"));
            DGV.Columns.Add(GetGridColumn("Open", "Double"));
            DGV.Columns.Add(GetGridColumn("High", "Double"));
            DGV.Columns.Add(GetGridColumn("Low", "Double"));
            DGV.Columns.Add(GetGridColumn("Close", "Double"));
            DGV.Columns.Add(GetGridColumn("Volume", "Double"));
            DGV.Columns.Add(GetGridColumn("OI", "Double"));
            DGV.Columns.Add(GetGridColumn("ATP", "Double"));
            DGV.Columns.Add(GetGridColumn("TBQ", "Int32"));
            DGV.Columns.Add(GetGridColumn("TSQ", "Int32"));
            DGV.Columns.Add(GetGridColumn("TotalTrades", "Int32"));
            DGV.Columns.Add(GetGridColumn("TotalQtyTraded", "Int32"));
            DGV.Columns.Add(GetGridColumn("TotalTradedValue", "Double"));
            DGV.Columns.Add(GetGridColumn("HighestPriceEver", "Double"));
            DGV.Columns.Add(GetGridColumn("LowestPriceever", "Double"));
            DGV.Columns.Add(GetGridColumn("DecimalLocator", "Int16"));
            DGV.Columns.Add(GetGridColumn("Exchange", "String"));
            DGV.Columns.Add(GetGridColumn("Lotsize", "Int32"));

            DataTable dt = (DataTable)DGV.DataSource;

        }

        private void combo_Exchange_SelectedIndexChanged(object sender, EventArgs e)
        {
           // InsertType_fun();

         //   InstType();
        }

        private void comboBInstType_SelectedIndexChanged(object sender, EventArgs e)
        {
           InstType();
        }

        private void comboB_Symbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            Exoirry();

         //   combo_Exoiry.SelectedIndex = 0;

        }

        private void combo_Exoiry_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = combo_Exoiry.SelectedIndex;
            date = T[i];
            optionType();
        }

        private void combo_OptionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            strike_prise();
        }
        public static int[] LoadFormLocationAndSize(Form xForm)
        {
            int[] t={0,0,900,300};
            if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "formmktclose.xml"))
                return t ;
            DataSet dset = new DataSet();
            dset.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "formmktclose.xml");
            int[] LocationAndSize = new int[] { xForm.Location.X, xForm.Location.Y, xForm.Size.Width, xForm.Size.Height };

            try
            {
                var AbbA = dset.Tables[0].Rows[0]["Input"].ToString().Split(';');              
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
            var settings = new XmlWriterSettings { Indent = true };
            XmlWriter writer = XmlWriter.Create(Application.StartupPath + Path.DirectorySeparatorChar + "formmktclose.xml", settings);
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
       
        private void frmMktWatch_Load(object sender, EventArgs e)
        {
            
                var AbbA = LoadFormLocationAndSize(this);
                this.Location = new Point(AbbA[0], AbbA[1]);
                this.Size = new Size(AbbA[2], AbbA[3]);

                this.FormClosing += new FormClosingEventHandler(SaveFormLocationAndSize);
                //   this.WindowState = FormWindowState.Normal;
                DataGridViewColumnSelector cs = new DataGridViewColumnSelector(DGV);
                cs.MaxHeight = 200;
                cs.Width = 150;
                ReadyDatatable();
                Exchange();
                frmmktdefaultfun();
               // Global.Instance.pData.OnDataChange += PDataOnOnDataChange;
                LZO_NanoData.LzoNanoData.Instance.OnDataChange += PDataOnOnDataChange;
        }

        private void frmmktdefaultfun()
        {
            try
            {
                foreach (DataGridViewColumn dc in DGV.Columns)
                {

                    this.DGV.Columns[dc.HeaderText.Replace(" ", "")].Visible = true;
                }

                DataSet ds = new DataSet();
                //config.SetValue("Fo_FO_Profile", Convert.ToString(0), GetProfileName);
                var config = new Config { GroupName = null };
                var iforms = config.GetValue("MKT_Profile", Convert.ToString(0));
                if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + iforms + ".xml"))
                {
                    ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + iforms + ".xml");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string st = ds.Tables[0].Rows[i]["Input"].ToString();
                        this.DGV.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = false;
                    }

                    DataSet dst = new DataSet();
                    dst.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Defaultfrmwatch.xml");

                    for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
                    {
                        int RowIndex = DGV.Rows.Add();
                        DGV.Rows[RowIndex].Cells["UniqueIdentifier"].Value = dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString();
                        DGV.Rows[RowIndex].Cells["InstrumentName"].Value = dst.Tables[0].Rows[i]["InstrumentName"].ToString();
                        DGV.Rows[RowIndex].Cells["Description"].Value = dst.Tables[0].Rows[i]["Description"].ToString();
                        DGV.Rows[RowIndex].Cells["ExpiryDate"].Value = dst.Tables[0].Rows[i]["ExpiryDate"].ToString();
                        DGV.Rows[RowIndex].Cells["OptionType"].Value = dst.Tables[0].Rows[i]["OptionType"].ToString();
                        DGV.Rows[RowIndex].Cells["StrikePrice"].Value = dst.Tables[0].Rows[i]["StrikePrice"].ToString();
                        DGV.Rows[RowIndex].Cells["Symbol"].Value = dst.Tables[0].Rows[i]["Symbol"].ToString();
                        DGV.Rows[RowIndex].Cells["Bid"].Value = dst.Tables[0].Rows[i]["Bid"].ToString();
                        DGV.Rows[RowIndex].Cells["BidQ"].Value = dst.Tables[0].Rows[i]["BidQ"].ToString();
                        DGV.Rows[RowIndex].Cells["Ask"].Value = dst.Tables[0].Rows[i]["Ask"].ToString();
                        DGV.Rows[RowIndex].Cells["AskQ"].Value = dst.Tables[0].Rows[i]["AskQ"].ToString();
                        DGV.Rows[RowIndex].Cells["LTP"].Value = dst.Tables[0].Rows[i]["LTP"].ToString();
                        DGV.Rows[RowIndex].Cells["LTQ"].Value = dst.Tables[0].Rows[i]["LTQ"].ToString();
                        DGV.Rows[RowIndex].Cells["LTT"].Value = dst.Tables[0].Rows[i]["LTT"].ToString();
                        DGV.Rows[RowIndex].Cells["Open"].Value = dst.Tables[0].Rows[i]["Open"].ToString();
                        DGV.Rows[RowIndex].Cells["High"].Value = dst.Tables[0].Rows[i]["High"].ToString();
                        DGV.Rows[RowIndex].Cells["Low"].Value = dst.Tables[0].Rows[i]["Low"].ToString();
                        DGV.Rows[RowIndex].Cells["Close"].Value = dst.Tables[0].Rows[i]["Close"].ToString();
                        DGV.Rows[RowIndex].Cells["Volume"].Value = dst.Tables[0].Rows[i]["Volume"].ToString();
                        DGV.Rows[RowIndex].Cells["OI"].Value = dst.Tables[0].Rows[i]["OI"].ToString();
                        DGV.Rows[RowIndex].Cells["ATP"].Value = dst.Tables[0].Rows[i]["ATP"].ToString();
                        DGV.Rows[RowIndex].Cells["TBQ"].Value = dst.Tables[0].Rows[i]["TBQ"].ToString();
                        DGV.Rows[RowIndex].Cells["TSQ"].Value = dst.Tables[0].Rows[i]["TSQ"].ToString();
                        DGV.Rows[RowIndex].Cells["TotalTrades"].Value = dst.Tables[0].Rows[i]["TotalTrades"].ToString();
                        DGV.Rows[RowIndex].Cells["TotalQtyTraded"].Value = dst.Tables[0].Rows[i]["TotalQtyTraded"].ToString();
                        DGV.Rows[RowIndex].Cells["TotalTradedValue"].Value = dst.Tables[0].Rows[i]["TotalTradedValue"].ToString();
                        DGV.Rows[RowIndex].Cells["HighestPriceEver"].Value = dst.Tables[0].Rows[i]["HighestPriceEver"].ToString();
                        DGV.Rows[RowIndex].Cells["LowestPriceever"].Value = dst.Tables[0].Rows[i]["LowestPriceever"].ToString();
                        DGV.Rows[RowIndex].Cells["DecimalLocator"].Value = dst.Tables[0].Rows[i]["DecimalLocator"].ToString();
                        DGV.Rows[RowIndex].Cells["Lotsize"].Value = dst.Tables[0].Rows[i]["Lotsize"].ToString();
                        DGV.Rows[RowIndex].Cells["Exchange"].Value = dst.Tables[0].Rows[i]["Exchange"].ToString();
                        _mwatchDict.Add(Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString()), DGV.Rows[RowIndex]);
                        //UDP_Reciever.Instance.Subscribe = Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString());
                        //Global.Instance.pData.Subscribe = Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString());
                        LZO_NanoData.LzoNanoData.Instance.Subscribe = Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString());
                        Global.Instance.Data_With_Nano.AddOrUpdate(Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString()), ClassType.MARKETWTCH, (k, v) => ClassType.MARKETWTCH);
                        Holder._DictLotSize.TryAdd(Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"]), new Csv_Struct()
                        {
                            lotsize = CSV_Class.cimlist.Where(q => q.Token == Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"])).Select(a => a.BoardLotQuantity).First()
                        }
                            );


                    }
                 

                }

            }
            catch
            {
              //  MessageBox.Show("Defauft Profile Not Create", "Error");
            }


            //DGV.Rows.Clear();
            //DataSet dst = new DataSet();
            //if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + "Defaultfrmwatch.xml"))
            //{
            //    dst.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + System.DateTime.Now.Date.ToString("dddd, MMMM d, yyyy") + "Defaultfrmwatch.xml");
              
            //        for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
            //        {
            //            int RowIndex = DGV.Rows.Add();
            //            DGV.Rows[RowIndex].Cells["UniqueIdentifier"].Value = dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString();
            //            DGV.Rows[RowIndex].Cells["InstrumentName"].Value = dst.Tables[0].Rows[i]["InstrumentName"].ToString();
            //            DGV.Rows[RowIndex].Cells["Description"].Value = dst.Tables[0].Rows[i]["Description"].ToString();
            //            DGV.Rows[RowIndex].Cells["ExpiryDate"].Value = dst.Tables[0].Rows[i]["ExpiryDate"].ToString();
            //            DGV.Rows[RowIndex].Cells["OptionType"].Value = dst.Tables[0].Rows[i]["OptionType"].ToString();
            //            DGV.Rows[RowIndex].Cells["StrikePrice"].Value = dst.Tables[0].Rows[i]["StrikePrice"].ToString();
            //            DGV.Rows[RowIndex].Cells["Symbol"].Value = dst.Tables[0].Rows[i]["Symbol"].ToString();
            //            DGV.Rows[RowIndex].Cells["Bid"].Value = dst.Tables[0].Rows[i]["Bid"].ToString();
            //            DGV.Rows[RowIndex].Cells["BidQ"].Value = dst.Tables[0].Rows[i]["BidQ"].ToString();
            //            DGV.Rows[RowIndex].Cells["Ask"].Value = dst.Tables[0].Rows[i]["Ask"].ToString();
            //            DGV.Rows[RowIndex].Cells["AskQ"].Value = dst.Tables[0].Rows[i]["AskQ"].ToString();
            //            DGV.Rows[RowIndex].Cells["LTP"].Value = dst.Tables[0].Rows[i]["LTP"].ToString();
            //            DGV.Rows[RowIndex].Cells["LTQ"].Value = dst.Tables[0].Rows[i]["LTQ"].ToString();
            //            DGV.Rows[RowIndex].Cells["LTT"].Value = dst.Tables[0].Rows[i]["LTT"].ToString();
            //            DGV.Rows[RowIndex].Cells["Open"].Value = dst.Tables[0].Rows[i]["Open"].ToString();
            //            DGV.Rows[RowIndex].Cells["High"].Value = dst.Tables[0].Rows[i]["High"].ToString();
            //            DGV.Rows[RowIndex].Cells["Low"].Value = dst.Tables[0].Rows[i]["Low"].ToString();
            //            DGV.Rows[RowIndex].Cells["Close"].Value = dst.Tables[0].Rows[i]["Close"].ToString();
            //            DGV.Rows[RowIndex].Cells["Volume"].Value = dst.Tables[0].Rows[i]["Volume"].ToString();
            //            DGV.Rows[RowIndex].Cells["OI"].Value = dst.Tables[0].Rows[i]["OI"].ToString();
            //            DGV.Rows[RowIndex].Cells["ATP"].Value = dst.Tables[0].Rows[i]["ATP"].ToString();
            //            DGV.Rows[RowIndex].Cells["TBQ"].Value = dst.Tables[0].Rows[i]["TBQ"].ToString();
            //            DGV.Rows[RowIndex].Cells["TSQ"].Value = dst.Tables[0].Rows[i]["TSQ"].ToString();
            //            DGV.Rows[RowIndex].Cells["TotalTrades"].Value = dst.Tables[0].Rows[i]["TotalTrades"].ToString();
            //            DGV.Rows[RowIndex].Cells["TotalQtyTraded"].Value = dst.Tables[0].Rows[i]["TotalQtyTraded"].ToString();
            //            DGV.Rows[RowIndex].Cells["TotalTradedValue"].Value = dst.Tables[0].Rows[i]["TotalTradedValue"].ToString();
            //            DGV.Rows[RowIndex].Cells["HighestPriceEver"].Value = dst.Tables[0].Rows[i]["HighestPriceEver"].ToString();
            //            DGV.Rows[RowIndex].Cells["LowestPriceever"].Value = dst.Tables[0].Rows[i]["LowestPriceever"].ToString();
            //            DGV.Rows[RowIndex].Cells["DecimalLocator"].Value = dst.Tables[0].Rows[i]["DecimalLocator"].ToString();
            //            DGV.Rows[RowIndex].Cells["Lotsize"].Value = dst.Tables[0].Rows[i]["Lotsize"].ToString();
            //            DGV.Rows[RowIndex].Cells["Exchange"].Value = dst.Tables[0].Rows[i]["Exchange"].ToString();
            //            _mwatchDict.Add(Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString()), DGV.Rows[RowIndex]);
            //            //UDP_Reciever.Instance.Subscribe = Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString());
            //            //Global.Instance.pData.Subscribe = Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString());
            //           LZO_NanoData.LzoNanoData.Instance.Subscribe = Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString());
            //            Global.Instance.Data_With_Nano.AddOrUpdate(Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString()), ClassType.MARKETWTCH, (k, v) => ClassType.MARKETWTCH);
            //            Holder._DictLotSize.TryAdd(Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"]), new Csv_Struct()
            //            {
            //                lotsize = CSV_Class.cimlist.Where(q => q.Token == Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"])).Select(a => a.BoardLotQuantity).First()
            //            }
            //                );
                        
                    
            //    }
            //}

        }
        private void SetData(DataGridViewCell DGCell, decimal ValueOne)
        {
            if (DGCell != null)
            {
                decimal ValueTwo = Convert.ToDecimal(DGCell.Value);
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

      
        private void AddSymbolFirstTime()
        {
            int token=0 ;
            if (comboB_Symbol.Text == "" && comboBInstType.Text == "")
            {
                MessageBox.Show("Selected Token not find ");
                return ;
            }
            if (comboBInstType.Text == "FUTIVX" || comboBInstType.Text == "FUTIDX" || comboBInstType.Text == "FUTSTK")
            {
                int t = CSV_Class.cimlist.FindIndex(q => q.Symbol == comboB_Symbol.Text && q.ExpiryDate == this.date && q.InstrumentName == comboBInstType.Text);

                token = CSV_Class.cimlist[t].Token;
                if (Holder._DictLotSize.ContainsKey(token) == false || token != 0)
                {
                    Holder._DictLotSize.TryAdd(token, new Csv_Struct()
                    {
                        lotsize = CSV_Class.cimlist.Where(q => q.Token == token).Select(a => a.BoardLotQuantity).First()
                    }
                    );
                }
            }
            else
            {
                if (comboB_Symbol.Text == "" || combo_Exoiry.Text == "" || combo_OptionType.Text == "" || comboBInstType.Text == "" || combo_StrikePrice.Text == "")
                {
                    MessageBox.Show("Please Select All Fiels ...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }    
                int t = CSV_Class.cimlist.FindIndex(q => q.Symbol == comboB_Symbol.Text && q.ExpiryDate == this.date && q.InstrumentName == comboBInstType.Text && q.OptionType == combo_OptionType.Text && q.StrikePrice == Convert.ToInt32(combo_StrikePrice.Text));

                token = CSV_Class.cimlist[t].Token;
                if (Holder._DictLotSize.ContainsKey(token) == false || token != 0)
                {
                    Holder._DictLotSize.TryAdd(token, new Csv_Struct()
                    {
                        lotsize = CSV_Class.cimlist.Where(q => q.Token == token).Select(a => a.BoardLotQuantity).First()
                    }
                    );
                }
            }
            
         //   int token = CSV_Class.cimlist.Where(q => q.Symbol == comboB_Symbol.Text && q.ExpiryDate == o && q.InstrumentName ==comboBInstType.Text).Select(a => a.Token).First();
            if (!_mwatchDict.ContainsKey(token))
            {
                int RowIndex = DGV.Rows.Add();
                DGV.Rows[RowIndex].Cells["UniqueIdentifier"].Value = token;
                DGV.Rows[RowIndex].Cells["InstrumentName"].Value =CSV_Class.cimlist.Where(q => q.Token == token).Select(a => a.InstrumentName).First();
                DGV.Rows[RowIndex].Cells["Description"].Value = ASCIIEncoding.ASCII.GetString(CSV_Class.cimlist.Where(q => q.Token == token ).Select(a => a.Name).First());
                DGV.Rows[RowIndex].Cells["ExpiryDate"].Value = date;
                DGV.Rows[RowIndex].Cells["OptionType"].Value = combo_OptionType.Text.Length >0 ? combo_OptionType.Text : "XX";
                DGV.Rows[RowIndex].Cells["StrikePrice"].Value = combo_StrikePrice.Text.Length >0 ? combo_StrikePrice.Text : "-1";
                DGV.Rows[RowIndex].Cells["Symbol"].Value = comboB_Symbol.SelectedItem.ToString();
                DGV.Rows[RowIndex].Cells["Bid"].Value = 0.0;
                DGV.Rows[RowIndex].Cells["BidQ"].Value = 0;
                DGV.Rows[RowIndex].Cells["Ask"].Value = 0;
                DGV.Rows[RowIndex].Cells["AskQ"].Value = 0;
                DGV.Rows[RowIndex].Cells["LTP"].Value = 0;
                DGV.Rows[RowIndex].Cells["LTQ"].Value = 0;
                DGV.Rows[RowIndex].Cells["LTT"].Value = DateTime.Now;
                DGV.Rows[RowIndex].Cells["Open"].Value = 0;
                DGV.Rows[RowIndex].Cells["High"].Value = 0;
                DGV.Rows[RowIndex].Cells["Low"].Value = 0;
                DGV.Rows[RowIndex].Cells["Close"].Value = 0;
                DGV.Rows[RowIndex].Cells["Volume"].Value = 0;
                DGV.Rows[RowIndex].Cells["OI"].Value = 0;
                DGV.Rows[RowIndex].Cells["ATP"].Value = 0;
                DGV.Rows[RowIndex].Cells["TBQ"].Value = 0;
                DGV.Rows[RowIndex].Cells["TSQ"].Value = 0;
                DGV.Rows[RowIndex].Cells["TotalTrades"].Value = 0;
                DGV.Rows[RowIndex].Cells["TotalQtyTraded"].Value = 0;
                DGV.Rows[RowIndex].Cells["TotalTradedValue"].Value = 0;
                DGV.Rows[RowIndex].Cells["HighestPriceEver"].Value = 0;
                DGV.Rows[RowIndex].Cells["LowestPriceever"].Value = 0;
                DGV.Rows[RowIndex].Cells["DecimalLocator"].Value = 100;
                DGV.Rows[RowIndex].Cells["Lotsize"].Value = CSV_Class.cimlist.Where(q => q.Token == token).Select(a => a.BoardLotQuantity).First();
                DGV.Rows[RowIndex].Cells["Exchange"].Value = combo_Exchange.SelectedItem.ToString() ;
                _mwatchDict.Add(token, DGV.Rows[RowIndex]);
                //UDP_Reciever.Instance.Subscribe = token;
               // Global.Instance.pData.Subscribe = token;
               LZO_NanoData.LzoNanoData.Instance.Subscribe = token;
                Global.Instance.Data_With_Nano.AddOrUpdate(token,ClassType.MARKETWTCH  , (k, v) => ClassType.MARKETWTCH);
            }
        }

        private void comboB_Symbol_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void combo_Exoiry_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
                if(!comboBInstType.Text.Substring(1,3).Equals("OPT"))
                    AddSymbolFirstTime();
        }

        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.F1 || e.KeyCode == Keys.Oemplus)
            {
                foreach (DataGridViewRow DGVR in DGV.SelectedRows)
                {
                    BuyOrder(DGVR);
                }
            }
            else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.F2 || e.KeyCode == Keys.OemMinus)
            {
                foreach (DataGridViewRow DGVR in DGV.SelectedRows)
                {
                    SellOrder(DGVR);
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewRow DGVR in DGV.SelectedRows)
                {
                    _mwatchDict.Remove(Convert.ToInt32(DGVR.Cells["UniqueIdentifier"].Value.ToString()));
                }
            }
            else if (e.KeyCode == Keys.F6)
            {

                foreach (DataGridViewRow DGVR in DGV.SelectedRows)
                {
                    mktPicture(DGVR);
                }
            }
        }
        private void mktPicture(DataGridViewRow Dr)
        {
            // if (AppGlobal.frmMarketPicture == null)
            // {
            //FrmMarketPicture frmMarketPicture = new FrmMarketPicture();
            //frmMarketPicture.Show();


            //FrmMarketPicture _frmpic = new FrmMarketPicture();
            //_frmpic.Show(this);

            frmMarketDepth _frmpic = new frmMarketDepth();


            // _frmpic.cmbInstrument =
            _frmpic.cmbInstrument.Text = Dr.Cells["InstrumentName"].Value.ToString();

            string[] symm = CSV_Class.cimlist.Where(a => a.InstrumentName == _frmpic.cmbInstrument.Text && a.InstrumentName != "" && a.InstrumentName != null).Select(q => q.Symbol).Distinct().ToArray();


            _frmpic.cmbSymbol.Items.AddRange(symm);

            
           

           


            _frmpic.cmbSymbol.Text = Dr.Cells["Symbol"].Value.ToString();


            _frmpic.cmbSeries.Text = Dr.Cells["OptionType"].Value.ToString();

            T = CSV_Class.cimlist.Where(a => a.Symbol == _frmpic.cmbSymbol.Text && a.InstrumentName == _frmpic.cmbInstrument.Text).OrderBy(s => s.ExpiryDate).Select(d => d.ExpiryDate).Distinct().ToList();

            //   T = Holder.clliest_contractfile.Where(a => a.Symbol == SYMcomboBox4.Text && a.InstrumentName == INSTcomboBox3.Text).OrderBy(s => s.ExpiryDate).Select(d => d.ExpiryDate).Distinct().ToList();

            // IEnumerable<long> exp = CSV_Class.cimlist.Where(a => a.Symbol == SYMcomboBox4.Text && a.InstrumentName == INSTcomboBox3.Text).Select(r => r.ExpiryDate).Distinct().ToList();
            _frmpic.cmbExpirty.Items.Clear();
            foreach (long ex in T)
            {

                string on = ConvertFromTimestamp(ex).ToShortDateString();

                _frmpic.cmbExpirty.Items.Add(on);
                //  date = ex;

            }
            _frmpic.cmbStrikePrice.Text = Dr.Cells["StrikePrice"].Value.ToString();
            _frmpic.cmbExpirty.Text = ConvertFromTimestamp(Convert.ToInt64(Dr.Cells["ExpiryDate"].Value)).ToString();
            _frmpic.Token.Text = Dr.Cells["UniqueIdentifier"].Value.ToString();


            if (_frmpic.cmbInstrument.Text == "FUTIVX" || _frmpic.cmbInstrument.Text == "FUTIDX" || _frmpic.cmbInstrument.Text == "FUTSTK")
            {

                _frmpic.cmbSeries.Enabled = false;
                _frmpic.cmbStrikePrice.Enabled = false;
                // _frmpic.cmbExpirty.Items.Clear();
                _frmpic.cmbSeries.Items.Clear();
                // cmbStrikePrice.Items.Clear();
                _frmpic.cmbStrikePrice.Text = "";

            }
            //UniqueIdentifier
            _frmpic.Show(this);


            // frmtest _frmtest = new frmtest();

            //_frmtest.Show();
            // _frmMktWatch.WindowState = FormWindowState.Minimized;

            //  UDP_Reciever.Instance.OnDataArrived += _frmMktWatch.OnDataArrived;
            // }
            // AppGlobal.frmMarketPicture.MdiParent = this;


        }

        internal string ok()
        {
            string token = "";
            if (comboB_Symbol.Text == "" && comboBInstType.Text == "")
            {
                return "";
            }
            if (comboBInstType.Text == "FUTIVX" || comboBInstType.Text == "FUTIDX" || comboBInstType.Text == "FUTSTK")
            {
                int t = CSV_Class.cimlist.FindIndex(q => q.Symbol == comboB_Symbol.Text && q.ExpiryDate == this.date && q.InstrumentName == comboBInstType.Text);

                token = CSV_Class.cimlist[t].Token.ToString();
            }
            else
            {
                int t = CSV_Class.cimlist.FindIndex(q => q.Symbol == comboB_Symbol.Text && q.ExpiryDate == this.date && q.InstrumentName == comboBInstType.Text && q.OptionType == combo_OptionType.Text && q.StrikePrice == Convert.ToInt32(combo_StrikePrice.Text));

                token = CSV_Class.cimlist[t].Token.ToString();
            }
            return token;
               
        }
        
        private void BuyOrder(DataGridViewRow Dr)
        {
            using (var frmord = new FrmOrderEntry())
            {
                frmord.lblOrderMsg.Text = "Buy " + Dr.Cells["Symbol"].Value + "(" + Dr.Cells["UniqueIdentifier"].Value +
                                          ")  ";
                frmord.lblOrderMsg.BackColor = Color.Blue;
                frmord.LEG_PRICE = Convert.ToDouble(Dr.Cells["Bid"].Value);
                frmord.LEG_SIZE = 1; //Convert.ToInt32(Dr.Cells["BidQ"].Value);
                // frmord.DesktopLocation = new Point(100, 100);
                int x = (Screen.PrimaryScreen.WorkingArea.Width - frmord.Width) / 2;
                int y = (Screen.PrimaryScreen.WorkingArea.Height - frmord.Height) - 50;
                frmord.Location = new Point(x, y);

                var v = Convert.ToInt32(Dr.Cells["ExpiryDate"].Value);
                if (frmord.ShowDialog(this) == DialogResult.OK)
                {
                    if (frmord.FormDialogResult == (int)OrderEntryButtonCase.SUBMIT)
                    {

                        NNFInOut.Instance.BOARD_LOT_IN_TR(Convert.ToInt32(Dr.Cells["UniqueIdentifier"].Value),
                              Dr.Cells["InstrumentName"].Value.ToString(),
                             Dr.Cells["Symbol"].Value.ToString(),
                             Convert.ToInt32(Dr.Cells["ExpiryDate"].Value),
                              Convert.ToInt32(Dr.Cells["StrikePrice"].Value),
                              Dr.Cells["OptionType"].Value.ToString(),
                              1,
                              frmord.LEG_SIZE * Convert.ToInt32(Dr.Cells["Lotsize"].Value),
                                Convert.ToInt32(frmord.LEG_PRICE * 100)
                                                        );

                    }
                }
            }
        }

        private void SellOrder(DataGridViewRow Dr)
        {
            using (var frmord = new FrmOrderEntry())
            {
                frmord.lblOrderMsg.Text = "Sell " + Dr.Cells["Symbol"].Value + "(" + Dr.Cells["UniqueIdentifier"].Value +   ")  ";
                frmord.lblOrderMsg.BackColor = Color.Red;
                frmord.LEG_PRICE = Convert.ToDouble(Dr.Cells["Ask"].Value);
                frmord.LEG_SIZE = 1; //Convert.ToInt32(Dr.Cells["AskQ"].Value);
                int x = (Screen.PrimaryScreen.WorkingArea.Width - frmord.Width) / 2;
                int y = (Screen.PrimaryScreen.WorkingArea.Height - frmord.Height) - 50;
                frmord.Location = new Point(x, y);

                if (frmord.ShowDialog(this) == DialogResult.OK)
                {
                    if (frmord.FormDialogResult == (int)OrderEntryButtonCase.SUBMIT)
                    {

                        NNFInOut.Instance.BOARD_LOT_IN_TR(Convert.ToInt32(Dr.Cells["UniqueIdentifier"].Value),
                             Dr.Cells["InstrumentName"].Value.ToString(),
                            Dr.Cells["Symbol"].Value.ToString(),
                            Convert.ToInt32(Dr.Cells["ExpiryDate"].Value),
                             Convert.ToInt32(Dr.Cells["StrikePrice"].Value),
                             Dr.Cells["OptionType"].Value.ToString(),
                             2,
                             frmord.LEG_SIZE * Convert.ToInt32(Dr.Cells["Lotsize"].Value),
                               Convert.ToInt32(frmord.LEG_PRICE)*100
                                                       );
                    }
                }
            }
        }
        delegate void OnLZOArrivedDelegate(Object o, ReadOnlyEventArgs<INTERACTIVE_ONLY_MBP> Stat);

        struct INTERACTIVE_LZO_MBP
        {
            public int Token;


            public int VolumeTradedToday;
            public decimal LastTradedPrice;

            public int NetPriceChangeFromClosingPrice;
            public int LastTradeQuantity;
            public int LastTradeTime;
            public decimal AverageTradePrice;

            public Int32 BQuantity;
            public decimal BPrice;
            public short BNumberOfOrders;

            public Int32 AQuantity;
            public decimal APrice;
            public short ANumberOfOrders;

            public short BbTotalBuyFlag;
            public short BbTotalSellFlag;
            public double TotalBuyQuantity;
            public double TotalSellQuantity;
            public short Indicator;
            public decimal ClosingPrice;
            public decimal OpenPrice;
            public decimal HighPrice;
            public decimal LowPrice;
        }


        private INTERACTIVE_LZO_MBP _dataMbp;
        private double DoubleIndianChange(double value)
        {
            return BitConverter.ToDouble(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
        }

        delegate void OnLZOArrivedmktDelegate(Object o, ReadOnlyEventArgs<INTERACTIVE_ONLY_MBP> Stat);

        private void PDataOnOnDataChange(object sender, ReadOnlyEventArgs<INTERACTIVE_ONLY_MBP> Stat)
        {

            try
            {
                if (DGV.Rows.Count == 0)
                {
                    return;
                }
                if (DGV.InvokeRequired)
                {
                    DGV.Invoke(new OnLZOArrivedmktDelegate(PDataOnOnDataChange), sender, new ReadOnlyEventArgs<INTERACTIVE_ONLY_MBP>(Stat.Parameter));
                    return;
                }
            }
            catch (Exception ex)
            {

            }

            int Token = IPAddress.HostToNetworkOrder(Stat.Parameter.Token);
            if (_mwatchDict.ContainsKey(Token))
            {
                _dataMbp.Token = Token;
                _dataMbp.ANumberOfOrders = IPAddress.HostToNetworkOrder(Stat.Parameter.RecordBuffer[5].NumberOfOrders);
                _dataMbp.BNumberOfOrders = IPAddress.HostToNetworkOrder(Stat.Parameter.RecordBuffer[0].NumberOfOrders);

                _dataMbp.APrice =
                    (decimal)IPAddress.HostToNetworkOrder(Stat.Parameter.RecordBuffer[5].Price) / 100;
                _dataMbp.BPrice =
                    (decimal)IPAddress.HostToNetworkOrder(Stat.Parameter.RecordBuffer[0].Price)/ 100;

                _dataMbp.AQuantity = IPAddress.HostToNetworkOrder(Stat.Parameter.RecordBuffer[5].Quantity);
                _dataMbp.BQuantity = IPAddress.HostToNetworkOrder(Stat.Parameter.RecordBuffer[0].Quantity);

                _dataMbp.TotalBuyQuantity = DoubleIndianChange(Stat.Parameter.TotalBuyQuantity);
                _dataMbp.TotalSellQuantity = DoubleIndianChange(Stat.Parameter.TotalBuyQuantity);

                _dataMbp.ClosingPrice = (decimal)IPAddress.HostToNetworkOrder(Stat.Parameter.ClosingPrice) / 100;
                _dataMbp.AverageTradePrice =
                    (decimal)IPAddress.HostToNetworkOrder(Stat.Parameter.RecordBuffer[5].Price) / 100;
                
                _dataMbp.HighPrice = (decimal)IPAddress.HostToNetworkOrder(Stat.Parameter.HighPrice) / 100;
                _dataMbp.LowPrice = (decimal)IPAddress.HostToNetworkOrder(Stat.Parameter.LowPrice) / 100;
                _dataMbp.OpenPrice =(decimal)IPAddress.HostToNetworkOrder(Stat.Parameter.OpenPrice) / 100;
                _dataMbp.LastTradedPrice =
                    (decimal)IPAddress.HostToNetworkOrder(Stat.Parameter.LastTradedPrice)/ 100;
                _dataMbp.LastTradeQuantity = IPAddress.HostToNetworkOrder(Stat.Parameter.LastTradeQuantity);

                _dataMbp.VolumeTradedToday = IPAddress.HostToNetworkOrder(Stat.Parameter.VolumeTradedToday);


                SetData(_mwatchDict[Token].Cells["Bid"], _dataMbp.BPrice);
                SetData(_mwatchDict[Token].Cells["BidQ"], _dataMbp.BQuantity);
                SetData(_mwatchDict[Token].Cells["TBidOrder"], _dataMbp.BNumberOfOrders);
                //(IPAddress.NetworkToHostOrder(Stat.Parameter.mbpSells[0].Price)) / 100
                SetData(_mwatchDict[Token].Cells["Ask"], _dataMbp.APrice);
                SetData(_mwatchDict[Token].Cells["AskQ"], _dataMbp.AQuantity);
                SetData(_mwatchDict[Token].Cells["TAskOrder"], _dataMbp.ANumberOfOrders);

                SetData(_mwatchDict[Token].Cells["LTP"], _dataMbp.LastTradedPrice);
                SetData(_mwatchDict[Token].Cells["LTQ"], _dataMbp.LastTradeQuantity);
                SetData(_mwatchDict[Token].Cells["Open"], _dataMbp.OpenPrice);
                SetData(_mwatchDict[Token].Cells["High"], _dataMbp.HighPrice);
                SetData(_mwatchDict[Token].Cells["Low"], _dataMbp.LowPrice);
                SetData(_mwatchDict[Token].Cells["Close"], _dataMbp.ClosingPrice);
                SetData(_mwatchDict[Token].Cells["Volume"], _dataMbp.VolumeTradedToday);

              //  SetData(_mwatchDict[Token].Cells["TBQ"], _dataMbp.TotalBuyQuantity);
               // SetData(_mwatchDict[Token].Cells["TSQ"], _dataMbp.TotalSellQuantity);
                SetData(_mwatchDict[Token].Cells["ATP"], _dataMbp.AverageTradePrice);

            }
        }
        //delegate void OnDataArrivedmktDelegate(Object o, ReadOnlyEventArgs<FinalPrice> Stat);
        //public void OnDataArrived(Object o, ReadOnlyEventArgs<FinalPrice> Stat)
        //{
        //    try
        //    {
        //        if (DGV.Rows.Count == 0)
        //        {
        //            return;
        //        }
        //        if (DGV.InvokeRequired)
        //        {
        //            DGV.Invoke(new OnDataArrivedmktDelegate(OnDataArrived), o, new ReadOnlyEventArgs<FinalPrice>(Stat.Parameter));
        //            return;
        //        }

        //        if (_mwatchDict.ContainsKey(Stat.Parameter.Token))
        //             {
                         
        //            SetData(_mwatchDict[Stat.Parameter.Token].Cells["Bid"], Convert.ToDouble(Stat.Parameter.MAXBID)/100);
        //            //SetData(_mwatchDict[Stat.Parameter.Token].Cells["BidQ"], Convert.ToDouble(Stat.Parameter.MAXBID));
                    
        //            SetData(_mwatchDict[Stat.Parameter.Token].Cells["Ask"],Convert.ToDouble(Stat.Parameter.MINASK)/100);
        //           // SetData(_mwatchDict[Stat.Parameter.Token].Cells["AskQ"], Convert.ToDouble(Stat.Parameter.MAXBID));
                    
        //            SetData(_mwatchDict[Stat.Parameter.Token].Cells["LTP"],Convert.ToDouble(Stat.Parameter.LTP)/100);
        //            //Holder.holderDownload.AddOrUpdate(IPAddress.HostToNetworkOrder (obj.header_obj.TransactionCode), obj,(k, v) => obj);
        //          // Global.Instance.MTMDIct.AddOrUpdate(Stat.Parameter.Token, Stat.Parameter.LTP, (k, v) => Stat.Parameter.LTP);
        //        }

        //    }
        //    catch (DataException a)
        //    {
        //        MessageBox.Show("From Live Data fill " + Environment.NewLine + a.Message);
        //    }
        //}

        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void frmMktWatch_FormClosing(object sender, FormClosingEventArgs e)
        {
            LZO_NanoData.LzoNanoData.Instance.OnDataChange -= PDataOnOnDataChange;
           // Global.Instance.pData.OnDataChange -= PDataOnOnDataChange;
            if (DGV.Rows.Count == 0)
            {
                e.Cancel = true;
                this.Hide();
                return;
            }
            //Settings.Default.WindowLocation = this.Location;

            //// Copy window size to app settings
            //if (this.WindowState == FormWindowState.Normal)
            //{
            //    Settings.Default.WindowSize = this.Size;
            //}
            //else
            //{
            //    Settings.Default.WindowSize = this.RestoreBounds.Size;
            //}
            DataTable dt = new DataTable("frmwatch");
            foreach (DataGridViewColumn col in DGV.Columns)
            {
                dt.Columns.Add(col.HeaderText);
            }

            foreach (DataGridViewRow row in DGV.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }

           
            dt.WriteXml(Application.StartupPath + Path.DirectorySeparatorChar  + "Defaultfrmwatch.xml", true);
            //}

            e.Cancel = true;
            this.Hide();
        }
       
        private void combo_Exoiry_Click(object sender, EventArgs e)
        {
            
        }


        void Exchange()
        {
            combo_Exchange.Items.Clear();
            comboB_OrderType.Items.Clear();
            combo_Exchange.Items.Add("NFO");
            combo_Exchange.Items.Add("SPREAD");
            comboB_OrderType.Items.Add("Normal");
            comboB_OrderType.Items.Add("Spread");

        }

        private void comboB_OrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] dis = CSV_Class.cimlist.Where(ab => ab.InstrumentName != "" && ab.InstrumentName != null).Select(a => a.InstrumentName).Distinct().ToArray();
            comboBInstType.Items.AddRange(dis);
        }
      
        private void btnsaveMktwatch_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable("frmwatch");
            foreach (DataGridViewColumn col in DGV.Columns)
            {
                dt.Columns.Add(col.HeaderText);
            }

            foreach (DataGridViewRow row in DGV.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }

          //  saveFileDialog1.ShowDialog();
         //   dt.WriteXml(Application.StartupPath+Path.DirectorySeparatorChar+"a.xml" ,true );
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dt.WriteXml(saveFileDialog1.FileName ,true);
            }

       
        }

        private void btnLoadMktWatch_Click(object sender, EventArgs e)
        {
            
            DataSet dst = new DataSet();
            OpenFileDialog OpenFile = new OpenFileDialog();

            if (OpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DGV.Rows.Clear();
                dst.ReadXml(OpenFile.FileName);


                for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
                {
                    int RowIndex = DGV.Rows.Add();
                    DataColumnCollection columns = dst.Tables[0].Columns;

    if (!columns.Contains("UniqueIdentifier"))
    {
        return;
    }
                   
                       
                    DGV.Rows[RowIndex].Cells["UniqueIdentifier"].Value = dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString();
                    DGV.Rows[RowIndex].Cells["InstrumentName"].Value = dst.Tables[0].Rows[i]["InstrumentName"].ToString();
                    DGV.Rows[RowIndex].Cells["Description"].Value = dst.Tables[0].Rows[i]["Description"].ToString();
                    DGV.Rows[RowIndex].Cells["ExpiryDate"].Value = dst.Tables[0].Rows[i]["ExpiryDate"].ToString();
                    DGV.Rows[RowIndex].Cells["OptionType"].Value = dst.Tables[0].Rows[i]["OptionType"].ToString();
                    DGV.Rows[RowIndex].Cells["StrikePrice"].Value = dst.Tables[0].Rows[i]["StrikePrice"].ToString();
                    DGV.Rows[RowIndex].Cells["Symbol"].Value = dst.Tables[0].Rows[i]["Symbol"].ToString();
                    DGV.Rows[RowIndex].Cells["Bid"].Value = dst.Tables[0].Rows[i]["Bid"].ToString();
                    DGV.Rows[RowIndex].Cells["BidQ"].Value = dst.Tables[0].Rows[i]["BidQ"].ToString();
                    DGV.Rows[RowIndex].Cells["Ask"].Value = dst.Tables[0].Rows[i]["Ask"].ToString();
                    DGV.Rows[RowIndex].Cells["AskQ"].Value = dst.Tables[0].Rows[i]["AskQ"].ToString();
                    DGV.Rows[RowIndex].Cells["LTP"].Value = dst.Tables[0].Rows[i]["LTP"].ToString();
                    DGV.Rows[RowIndex].Cells["LTQ"].Value = dst.Tables[0].Rows[i]["LTQ"].ToString();
                    DGV.Rows[RowIndex].Cells["LTT"].Value = dst.Tables[0].Rows[i]["LTT"].ToString();
                    DGV.Rows[RowIndex].Cells["Open"].Value = dst.Tables[0].Rows[i]["Open"].ToString();
                    DGV.Rows[RowIndex].Cells["High"].Value = dst.Tables[0].Rows[i]["High"].ToString();
                    DGV.Rows[RowIndex].Cells["Low"].Value = dst.Tables[0].Rows[i]["Low"].ToString();
                    DGV.Rows[RowIndex].Cells["Close"].Value = dst.Tables[0].Rows[i]["Close"].ToString();
                    DGV.Rows[RowIndex].Cells["Volume"].Value = dst.Tables[0].Rows[i]["Volume"].ToString();
                    DGV.Rows[RowIndex].Cells["OI"].Value = dst.Tables[0].Rows[i]["OI"].ToString();
                    DGV.Rows[RowIndex].Cells["ATP"].Value = dst.Tables[0].Rows[i]["ATP"].ToString();
                    DGV.Rows[RowIndex].Cells["TBQ"].Value = dst.Tables[0].Rows[i]["TBQ"].ToString();
                    DGV.Rows[RowIndex].Cells["TSQ"].Value = dst.Tables[0].Rows[i]["TSQ"].ToString();
                    DGV.Rows[RowIndex].Cells["TotalTrades"].Value = dst.Tables[0].Rows[i]["TotalTrades"].ToString();
                    DGV.Rows[RowIndex].Cells["TotalQtyTraded"].Value = dst.Tables[0].Rows[i]["TotalQtyTraded"].ToString();
                    DGV.Rows[RowIndex].Cells["TotalTradedValue"].Value = dst.Tables[0].Rows[i]["TotalTradedValue"].ToString();
                    DGV.Rows[RowIndex].Cells["HighestPriceEver"].Value = dst.Tables[0].Rows[i]["HighestPriceEver"].ToString();
                    DGV.Rows[RowIndex].Cells["LowestPriceever"].Value = dst.Tables[0].Rows[i]["LowestPriceever"].ToString();
                    DGV.Rows[RowIndex].Cells["DecimalLocator"].Value = dst.Tables[0].Rows[i]["DecimalLocator"].ToString();
                    DGV.Rows[RowIndex].Cells["Lotsize"].Value = dst.Tables[0].Rows[i]["Lotsize"].ToString();
                    DGV.Rows[RowIndex].Cells["Exchange"].Value = dst.Tables[0].Rows[i]["Exchange"].ToString();
                    
                    _mwatchDict.Add(Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString()), DGV.Rows[RowIndex]);
                    //UDP_Reciever.Instance.Subscribe = Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString());
                    LZO_NanoData.LzoNanoData.Instance.Subscribe = Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString());
                    Global.Instance.Data_With_Nano.AddOrUpdate(Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"].ToString()), ClassType.MARKETWTCH, (k, v) => ClassType.MARKETWTCH);
                    Holder._DictLotSize.TryAdd(Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"]), new Csv_Struct()
                   {
                       lotsize = CSV_Class.cimlist.Where(q => q.Token == Convert.ToInt32(dst.Tables[0].Rows[i]["UniqueIdentifier"])).Select(a => a.BoardLotQuantity).First()
                   }
                       );


                }
            }
           
            
       
         
        }

        private void DGV_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //DataGridViewColumn newColumn = DGV.Columns[e.ColumnIndex];
            //DataGridViewColumn oldColumn = DGV.SortedColumn;
            //ListSortDirection direction;

            //// If oldColumn is null, then the DataGridView is not sorted. 
            //if (oldColumn != null)
            //{
            //    // Sort the same column again, reversing the SortOrder. 
            //    if (oldColumn == newColumn &&
            //        DGV.SortOrder == SortOrder.Ascending)
            //    {
            //        direction = ListSortDirection.Descending;
            //    }
            //    else
            //    {
            //        // Sort a new column and remove the old SortGlyph.
            //        direction = ListSortDirection.Ascending;
            //        oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            //    }
            //}
            //else
            //{
            //    direction = ListSortDirection.Ascending;
            //}

            //// Sort the selected column.
            //DGV.Sort(newColumn, direction);
            //newColumn.HeaderCell.SortGlyphDirection =
            //    direction == ListSortDirection.Ascending ?
            //    SortOrder.Ascending : SortOrder.Descending;

        }

        private void DGV_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                this.textBox1.Text = "";
            }
            if (Char.IsLetter(e.KeyChar))
            {
                int index = 0;
                // This works only if dataGridView1's SelectionMode property is set to FullRowSelect
                this.textBox1.Text = this.textBox1.Text + e.KeyChar.ToString();
                if (DGV.SelectedRows.Count > 0)
                {
                    index = DGV.SelectedRows[0].Index + 1;
                }
                for (int i = index; i < (DGV.Rows.Count + index); i++)
                {
                    if (DGV.Rows[i % DGV.Rows.Count].Cells["Symbol"].Value.ToString().StartsWith(this.textBox1.Text, true, CultureInfo.InvariantCulture))
                    {
                        foreach (var row in DGV.Rows.Cast<DataGridViewRow>().Where(t => t.Selected))
                        {
                            row.Selected = false;
                        }
                        DGV.Rows[i % DGV.Rows.Count].Cells[0].Selected = true;
                        DGV.FirstDisplayedScrollingRowIndex = DGV.SelectedRows[0].Index;
                        //return; // stop looping
                    }
                }
                if (this.textBox1.Text.Length >= 3)
                {
                    this.textBox1.Text = "";
                }

            }
        }

      
      


       

    }
}
