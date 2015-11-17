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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Client
{
    public struct dstruct
    {
       
      public String InstrumentName;
      public String FullName;
      public String  Symbol;
      public String TokenNo;   // int32
      public String Buy_SellIndicator;
      public String OptionType;
      public Int32  StrikePrice;
      public String      Price;  
      public double FillPrice;
      public long   FillNumber;
      public String      Volume;
      public String     Status;         
      public String      AccountNumber;
      public String      BookType;
      public Int32 BranchId;
      public String      BrokerId;
      public String      CloseoutFlag;
      public String      ExpiryDate;
      public Int32 DisclosedVolume;
      public Int32 DisclosedVolumeRemaining;
      public String      EntryDateTime;
      public Int32 filler;
      public String      GoodTillDate;
      public String      LastModified;
      public String      LogTime;
      public char Modified_CancelledBy;
      public Int64 nnffield;
      public String      Open_Close;
      public String      OrderNumber;
      public String      RejectReason;
      public Int16  Pro_ClientIndicator;
      public Int16  ReasonCode;
      public String      Settlor;
      public String      TimeStamp1;
      public String      TimeStamp2;
      public Int32 TotalVolumeRemaining;
      public Int32 TraderId;
      public Int16 TransactionCode;
      public Int32 UserId;
      public String     VolumeFilledToday;
      public String      Unique_id;
     
    }
    public partial class frmNetBook : Form
    {
        List<dstruct> studentDetails = new List<dstruct>();
        private static readonly frmNetBook instance = new frmNetBook();
        public static frmNetBook Instance
        {
            get
            {
                return instance;
            }
        }
        private frmNetBook()
        {
            InitializeComponent();
        }
        public static int[] LoadFormLocationAndSize(Form xForm)
        {
            int[] t = { 0, 0, 900, 300 };
            if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "formnetorderclose.xml"))
                return t;
            DataSet dset = new DataSet();
            dset.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "formnetorderclose.xml");
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

            //ini.IniWriteValue("FOFOFORM","Location", String.Format("{0};{1};{2};{3}", xForm.Location.X, xForm.Location.Y, xForm.Size.Width, xForm.Size.Height));

            var settings = new XmlWriterSettings { Indent = true };

            XmlWriter writer = XmlWriter.Create(Application.StartupPath + Path.DirectorySeparatorChar + "formnetorderclose.xml", settings);

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
        private void frmNetBook_Load(object sender, EventArgs e)
        {
            //if (Settings.Default.Window_LocationNBook != null)
            //{
            //    this.Location = Settings.Default.Window_LocationNBook;
            //}

            //// Set window size
            //if (Settings.Default.Window_SizeNBook != null)
            //{
            //    this.Size = Settings.Default.Window_SizeNBook;
            //}
            var AbbA = LoadFormLocationAndSize(this);
            this.Location = new Point(AbbA[0], AbbA[1]);
            this.Size = new Size(AbbA[2], AbbA[3]);

            this.FormClosing += new FormClosingEventHandler(SaveFormLocationAndSize);

          netposion(0, 0);
            this.DGV.Columns["SellAvg"].DefaultCellStyle.Format = "0.##";
            this.DGV.Columns["NetQty"].DefaultCellStyle.Format = "0.##";
            //  this.DGV.Columns["TotalSellQty"].DefaultCellStyle.Format = "0.0##";
            this.DGV.Columns["BuyAvg"].DefaultCellStyle.Format = "0.##";
            this.DGV.Columns["NetValue"].DefaultCellStyle.Format = "0.##";
            this.DGV.Columns["BEP"].DefaultCellStyle.Format = "0.##";
          
            DataGridViewColumnSelector cs = new DataGridViewColumnSelector(DGV);
            cs.MaxHeight = 200;
            cs.Width = 150;
        }

        public List<SelectListItem> query1 = new List<SelectListItem>();
     

        public void

            netposion(int tok, int lot)
        {


            try
            {
                int t_1 = 0;
                //double i;

                query1 = Global.Instance.OrdetTable.AsEnumerable().Where(p => p.Field<string>("Status") == orderStatus.Traded.ToString()).GroupBy(r => Convert.ToInt32(r.Field<string>("TokenNo")))
                          .Select(store => new SelectListItem
                          {
                              TredingSymbol = System.Text.ASCIIEncoding.ASCII.GetString(csv.CSV_Class.cimlist.First(tkn => tkn.Token == Convert.ToInt32(store.Key)).Name),
                              TokenNo =Convert.ToInt32(store.Key),
                              //   InstrumentName = store.First().Field<string>("InstrumentName"),
                              BuyQty = store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p => Convert.ToInt32(p.Field<string>("Volume"))),

                              BuyAvg = (store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p => ((Convert.ToInt32(p.Field<string>("Volume"))) * Convert.ToDouble(p.Field<string>("FillPrice")))) /
                              ((store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p => Convert.ToInt32(p.Field<string>("Volume")))) == 0 ? 1 : (store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p8 => Convert.ToInt32(p8.Field<string>("Volume")))))),

                              SellQty = store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p => Convert.ToInt32(p.Field<string>("Volume"))),

                              SellAvg = (store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p => (Convert.ToInt32(p.Field<string>("Volume"))) * Convert.ToDouble(p.Field<string>("FillPrice"))) /
                                 ((store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p => Convert.ToInt32(p.Field<string>("Volume")))) == 0 ? 1 : (store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p8 => Convert.ToInt32(p8.Field<string>("Volume")))))),

                              NetQty = (store.Where(a1 => a1.Field<String>("Buy_SellIndicator") == "BUY").Sum(p1 => Convert.ToInt32(p1.Field<string>("Volume")))) -
                           (store.Where(a2 => a2.Field<String>("Buy_SellIndicator") == "SELL").Sum(p2 => Convert.ToInt32(p2.Field<string>("Volume")))),

                              BEP = (Math.Round(Convert.ToDouble((((((store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p => Convert.ToDouble(p.Field<string>("Price")) * Convert.ToInt32(p.Field<string>("Volume"))))) -
                                   (store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p => Convert.ToDouble(p.Field<string>("FillPrice")) * Convert.ToInt32(p.Field<string>("Volume")))))
                           / (((store.Where(a1 => a1.Field<String>("Buy_SellIndicator") == "SELL").Sum(p1 => Convert.ToInt32(p1.Field<string>("Volume")))) -
                               (store.Where(a2 => a2.Field<String>("Buy_SellIndicator") == "BUY").Sum(p2 => Convert.ToInt32(p2.Field<string>("Volume"))))) == 0 ? 1 :
                                (Convert.ToInt32((store.Where(a1 => a1.Field<String>("Buy_SellIndicator") == "SELL").Sum(p1 => Convert.ToInt32(p1.Field<string>("Volume")))) -
                               (store.Where(a2 => a2.Field<String>("Buy_SellIndicator") == "BUY").Sum(p2 => Convert.ToInt32(p2.Field<string>("Volume")))))))))), 2)),

                              //dev1 =  (store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p => Convert.ToDouble(p.Field<string>("Price")) * Convert.ToInt32(p.Field<string>("Volume")))),

                              //MTOM = ((store.Where(a1 => a1.Field<String>("Buy_SellIndicator") == "BUY").Sum(p1 => Convert.ToInt32(p1.Field<string>("Volume")))) -
                              //(store.Where(a2 => a2.Field<String>("Buy_SellIndicator") == "SELL").Sum(p2 => Convert.ToInt32(p2.Field<string>("Volume"))))),
                              //NetValue = (((store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p => Convert.ToDouble(p.Field<string>("Price"))))) -
                              //((store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p => Convert.ToDouble(p.Field<string>("Price")))))),


                              //Convert.ToInt32(store.Select(a1 => a1.Field<string>("TokenNo"))))
                              //(Math.Round(Convert.ToDouble(Global.Instance.MTMDIct.Where(a => a.Key == store.Key).Select(q => q.Value)))

                              MTOM = ((Convert.ToDouble(Global.Instance.MTMDIct.ContainsKey(tok) == true ? Global.Instance.MTMDIct[tok] : 0) -
                        (Math.Round(Convert.ToDouble((((((store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p => Convert.ToDouble(p.Field<string>("FillPrice")) * Convert.ToInt32(p.Field<string>("Volume"))))) -
                                   (store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p => Convert.ToDouble(p.Field<string>("FillPrice")) * Convert.ToInt32(p.Field<string>("Volume")))))
                           / (((store.Where(a1 => a1.Field<String>("Buy_SellIndicator") == "SELL").Sum(p1 => Convert.ToInt32(p1.Field<string>("Volume")))) -
                               (store.Where(a2 => a2.Field<String>("Buy_SellIndicator") == "BUY").Sum(p2 => Convert.ToInt32(p2.Field<string>("Volume"))))) == 0 ? 1 :
                                (Convert.ToInt32((store.Where(a1 => a1.Field<String>("Buy_SellIndicator") == "SELL").Sum(p1 => Convert.ToInt32(p1.Field<string>("Volume")))) -
                               (store.Where(a2 => a2.Field<String>("Buy_SellIndicator") == "BUY").Sum(p2 => Convert.ToInt32(p2.Field<string>("Volume")))))))))), 2)))) * (store.Where(a1 => a1.Field<String>("Buy_SellIndicator") == "BUY").Sum(p1 => Convert.ToInt32(p1.Field<string>("Volume")))) -
                           (store.Where(a2 => a2.Field<String>("Buy_SellIndicator") == "SELL").Sum(p2 => Convert.ToInt32(p2.Field<string>("Volume")))),

                              NetValue = (store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p => Convert.ToInt32(p.Field<string>("Volume")) * lot) * (store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p => (Convert.ToInt32(p.Field<string>("Volume")) * lot) * Convert.ToDouble(p.Field<string>("FillPrice"))) /
                                 ((store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p => Convert.ToInt32(p.Field<string>("Volume")) * lot)) == 0 ? 1 : (store.Where(a => a.Field<String>("Buy_SellIndicator") == "SELL").Sum(p8 => Convert.ToInt32(p8.Field<string>("Volume")) * lot))))) - (store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p => Convert.ToInt32(p.Field<string>("Volume")) * lot) * (store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p => ((Convert.ToInt32(p.Field<string>("Volume")) * lot) * Convert.ToDouble(p.Field<string>("FillPrice")))) /
                              ((store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p => Convert.ToInt32(p.Field<string>("Volume")) * lot)) == 0 ? 1 : (store.Where(a => a.Field<String>("Buy_SellIndicator") == "BUY").Sum(p8 => Convert.ToInt32(p8.Field<string>("Volume"))) * lot)))),



                          }).ToList();

                //query1.FirstOrDefault().MTOM=
                //   var v = query1.FirstOrDefault().MTOM;
                //studentDetails = ConvertDataTable<dstruct>(Global.Instance.OrdetTable);
                //var v = studentDetails.Where(a => a.TokenNo == "123" && a.Buy_SellIndicator == "buy").Sum(a =>Convert.ToInt32(a.Volume));
                
                //DGV.DataSource = query1;
              //  int v = Global.Instance._Netbook.Where(a => a.Key == 1).FirstOrDefault().Value;
                int v = 0;
                if (Global.Instance._Netbook.ContainsKey(1))
                {
                     v = Global.Instance._Netbook[1] == null ? 0 : Global.Instance._Netbook[1];
                }
                DGV.DataSource = Short_fun(v);
            }
            catch { }


            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    DGV.Refresh();

                };
                this.Invoke(del);
                return;
            }
        }


        private List<SelectListItem> Short_fun(int cl)
        {
            List<SelectListItem> Tmp=new List<SelectListItem>();
            switch (cl)
            {
                case 0:
                         Tmp = query1.OrderBy(a => a.TredingSymbol).ToList();
                        
                 
                    break;
                case 1:

                    Tmp = query1.OrderBy(a => a.TokenNo).ToList();
                       
                       
                    break;
                case 2:

                    Tmp = query1.OrderBy(a => a.BuyQty).ToList();
                        
                       
                    break;
                case 3:
                    Tmp = query1.OrderBy(a => a.BuyAvg).ToList();
                      
                       
                    break;
                case 4:

                    Tmp = query1.OrderBy(a => a.SellQty).ToList();
                        
                    break;
                case 5:
                    Tmp = query1.OrderBy(a => a.SellAvg).ToList();
                     
                     
                    break;
                case 6:

                    Tmp = query1.OrderBy(a => a.NetQty).ToList();
                        
                      
                    break;
                case 7:

                    Tmp = query1.OrderBy(a => a.BEP).ToList();
                        
                       
                    break;
                case 8:

                    Tmp = query1.OrderBy(a => a.MTOM).ToList();
                      
                      
                    break;
                case 9:

                    Tmp = query1.OrderBy(a => a.NetValue).ToList();
                        
                     
                    break;

            }
            return Tmp;


        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        private void frmNetBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Settings.Default.Window_LocationNBook = this.Location;

            //// Copy window size to app settings
            //if (this.WindowState == FormWindowState.Normal)
            //{
            //    Settings.Default.Window_SizeNBook = this.Size;
            //}
            //else
            //{
            //    Settings.Default.Window_SizeNBook = this.RestoreBounds.Size;
            //}
            //// Save settings
            //Settings.Default.Save();
            //e.Cancel = true;
            //this.Hide();
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
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
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.Filter = "*.xlsx|*.*";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Excel.ExcelUtlity obj = new Excel.ExcelUtlity();
                obj.WriteDataTableToExcel(dt, "Excel Report", saveFileDialog1.FileName, "Details");
            }
        }

        private void frmNetBook_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        int i = 1;
        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //  this.DGV.Sort(this.DGV.Columns[0], ListSortDirection.Ascending);
           
        }





        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DGV_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Global.Instance._Netbook.AddOrUpdate(1, e.ColumnIndex, (k, v1) => e.ColumnIndex);
            switch (e.ColumnIndex)
            {
                case 0:
                    if (i == 1)
                    {
                        var Tmp = query1.OrderBy(a => a.TredingSymbol).ToList();
                        DGV.DataSource = Tmp;
                        i = 0;
                    }
                    else
                    {
                        var Tmp = query1.OrderByDescending(a => a.TredingSymbol).ToList();
                        DGV.DataSource = Tmp;
                        i = 1;
                    }
                    break;
                case 1:
                    if (i == 1)
                    {
                        var Tmp1 = query1.OrderBy(a => a.TokenNo).ToList();
                        DGV.DataSource = Tmp1;
                        i = 0;
                    }

                    else
                    {
                        var Tmp1 = query1.OrderByDescending(a => a.TokenNo).ToList();
                        DGV.DataSource = Tmp1;
                        i = 1;
                    }
                    break;
                case 2:
                    if (i == 1)
                    {
                        var Tmp2 = query1.OrderBy(a => a.BuyQty).ToList();
                        DGV.DataSource = Tmp2;
                        i = 0;
                    }
                    else
                    {
                        var Tmp2 = query1.OrderByDescending(a => a.BuyQty).ToList();
                        DGV.DataSource = Tmp2;
                        i = 1;
                    }
                    break;
                case 3:
                    if (i == 1)
                    {
                        var Tmp3 = query1.OrderBy(a => a.BuyAvg).ToList();
                        DGV.DataSource = Tmp3;
                        i = 0;
                    }
                    else
                    {
                        var Tmp3 = query1.OrderByDescending(a => a.BuyAvg).ToList();
                        DGV.DataSource = Tmp3;
                        i = 1;
                    }
                    break;
                case 4:
                    if (i == 1)
                    {
                        var Tmp4 = query1.OrderBy(a => a.SellQty).ToList();
                        DGV.DataSource = Tmp4;
                        i = 0;
                    }
                    else
                    {
                        var Tmp4 = query1.OrderByDescending(a => a.SellQty).ToList();
                        DGV.DataSource = Tmp4;
                        i = 1;
                    }
                    break;
                case 5:
                    if (i == 1)
                    {
                        var Tmp5 = query1.OrderBy(a => a.SellAvg).ToList();
                        DGV.DataSource = Tmp5;
                        i = 0;
                    }
                    else
                    {
                        var Tmp5 = query1.OrderByDescending(a => a.SellAvg).ToList();
                        DGV.DataSource = Tmp5;
                        i = 1;
                    }
                    break;
                case 6:
                    if (i == 1)
                    {
                        var Tmp6 = query1.OrderBy(a => a.NetQty).ToList();
                        DGV.DataSource = Tmp6;
                        i = 0;
                    }
                    else
                    {
                        var Tmp6 = query1.OrderByDescending(a => a.NetQty).ToList();
                        DGV.DataSource = Tmp6;
                        i = 1;
                    }
                    break;
                case 7:
                    if (i == 1)
                    {
                        var Tmp7 = query1.OrderBy(a => a.BEP).ToList();
                        DGV.DataSource = Tmp7;
                        i = 0;
                    }
                    else
                    {
                        var Tmp7 = query1.OrderByDescending(a => a.BEP).ToList();
                        DGV.DataSource = Tmp7;
                        i = 1;
                    }
                    break;
                case 8:
                    if (i == 1)
                    {
                        var Tmp8 = query1.OrderBy(a => a.MTOM).ToList();
                        DGV.DataSource = Tmp8;
                        i = 0;
                    }
                    else
                    {
                        var Tmp8 = query1.OrderByDescending(a => a.MTOM).ToList();
                        DGV.DataSource = Tmp8;
                        i = 1;
                    }
                    break;
                case 9:
                    if (i == 1)
                    {
                        var Tmp9 = query1.OrderBy(a => a.NetValue).ToList();
                        DGV.DataSource = Tmp9;
                        i = 0;
                    }
                    else
                    {
                        var Tmp9 = query1.OrderByDescending(a => a.NetValue).ToList();
                        DGV.DataSource = Tmp9;
                        i = 1;
                    }
                    break;

            }


        }
    }
    public class SelectListItem
    {
        public string TredingSymbol { get; set; }
     //   public string InstrumentName { get; set; }
        public int TokenNo { get; set; }
        public int BuyQty { get; set; }
        public double BuyAvg { get; set; }
        public int SellQty { get; set; }
        public double SellAvg { get; set; }
        public int NetQty { get; set; }       
        public double BEP { get; set; }
        public double MTOM { get; set; }
        public double NetValue { get; set; }

     //   public double dev1 { get; set; }
        // public string SellAvg { get; set; }
    }

}
