using Client.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Client
{
    public partial class frmTradeBook : Form
    {
        private static readonly frmTradeBook instance = new frmTradeBook();
        public static frmTradeBook Instance
        {
            get
            {
                return instance;
            }
        }
        private frmTradeBook()
        {
            InitializeComponent();
        }
        public static int[] LoadFormLocationAndSize(Form xForm)
        {
            int[] t = { 0, 0, 300, 300 };
            if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "formornetclose.xml"))
                return t;
            DataSet dset = new DataSet();
            dset.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "formornetclose.xml");
            int[] LocationAndSize = new int[] { xForm.Location.X, xForm.Location.Y, xForm.Size.Width, xForm.Size.Height };
            //---//
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
            try
            { 
            Form xForm = sender as Form;
            var settings = new XmlWriterSettings { Indent = true };

            XmlWriter writer = XmlWriter.Create(Application.StartupPath + Path.DirectorySeparatorChar + "formornetclose.xml", settings);

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
            e.Cancel = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Trade Book -  Funtion Name-  SaveFormLocationAndSize  " + ex.Message);
            }

        }
        private void frmTradeBook_Load(object sender, EventArgs e)
        {
            //if (Settings.Default.Window_LocationTBook != null)
            //{
            //    this.Location = Settings.Default.Window_LocationTBook;
            //}

            //// Set window size
            //if (Settings.Default.WindowS_izeTBook != null)
            //{
            //    this.Size = Settings.Default.WindowS_izeTBook;
            //}

            var AbbA = frmTradeBook.LoadFormLocationAndSize(this);
            this.Location = new Point(AbbA[0], AbbA[1]);
            this.Size = new Size(AbbA[2], AbbA[3]);

            this.FormClosing += new FormClosingEventHandler(SaveFormLocationAndSize);
          
          
            load_data();
            Type controlType = DGV.GetType();
            PropertyInfo pi = controlType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(DGV, true, null);
          //  DGV.Columns["LOGTIME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            profile_load();
            frmTradeBook.Instance.lblnooftrade.Text = "No Of Trade  =" + frmTradeBook.Instance.DGV.Rows.Count;
        }

        public void load_data()
        {
            try
            { 
            DataView dv = Global.Instance.OrdetTable.AsEnumerable().Where(a => a.Field<string>("Status") == orderStatus.Traded.ToString()).AsDataView();
            DGV.DataSource = dv;
            dv.Sort = "LOGTIME DESC";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Trade Book -  Funtion Name-  Load data  " + ex.Message);
            }
            this.DGV.Columns["LogTime"].DefaultCellStyle.Format = "HH:mm:ss";
         
              
        }

        public void profile_load()
        {
            foreach (DataGridViewColumn dc in DGV.Columns)
            {
                this.DGV.Columns[dc.HeaderText.Replace(" ", "")].Visible = true;              
            }
           try
            {
                if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "Trade_Profiles" + Path.DirectorySeparatorChar + "TradeDefault.xml"))
                    return;
             //   DGV.Columns["FullName"].Visible = true;
                DataSet ds1 = new DataSet();
                ds1.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Trade_Profiles" + Path.DirectorySeparatorChar + "TradeDefault.xml");
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    if (ds1.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "") == "FullName")
                    { continue; }
                    DGV.Columns[ds1.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = false;
                }

                DataGridViewColumnSelector cs = new DataGridViewColumnSelector(DGV);
                cs.MaxHeight = 200;
                cs.Width = 150;
            }
            catch (Exception ex)
            {                
                MessageBox.Show("Column Load Error"+ex.Message );
            }
        }

        private void frmTradeBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Window_LocationTBook = this.Location;

            // Copy window size to app settings
            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowS_izeTBook = this.Size;
            }
            else
            {
                Settings.Default.WindowS_izeTBook = this.RestoreBounds.Size;
            }

            // Save settings
            Settings.Default.Save();
            e.Cancel = true;
            this.Hide();
        }

        private void DGV_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try { 
            ProfileTrade_Book pfltrade_book = new ProfileTrade_Book();
           // Profile_forOrderBook pfltrade_book = new Profile_forOrderBook();
            if (pfltrade_book.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewColumn dc in DGV.Columns)
                {
                    this.DGV.Columns[dc.HeaderText.Replace(" ", "")].Visible = true;
                }
                String GetProfileName = pfltrade_book.GetProfileName();

                DataSet ds = new DataSet();
                ds.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Trade_Profiles" + Path.DirectorySeparatorChar + GetProfileName + ".xml");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string st = ds.Tables[0].Rows[i]["Input"].ToString();
                    this.DGV.Columns[ds.Tables[0].Rows[i]["Input"].ToString().Replace(" ", "")].Visible = false;
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Trade Book -  Funtion Name-  toolStripButton1_Click  " + ex.Message);
            }
        }

      

    

        private void DGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try{
            if (DGV.InvokeRequired)
            {
                DGV.Invoke(new On_DataPaintdDelegate(DGV_RowPrePaint), sender, e);
                return;
            }

            if (DGV.Rows[e.RowIndex].Cells["Buy_SellIndicator"].Value.ToString() == "BUY")
            {
                //  DGV.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                DGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
            }
            else
            {
                //  DGV.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Blue;
                DGV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
            }
             }
            catch (Exception ex)
            {

                MessageBox.Show("Trade Book -  Funtion Name-  toolStripButton1_Click  " + ex.Message);
            }
        }
    }
}
