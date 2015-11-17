using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using csv;
using Structure;
namespace Client
{
    public struct _SelectionOut
    {
        public String PFName;
        public int Token1;
        public int Token2;
        public String Desc1;
        public String Desc2;
        public int tok1_qty1;
        public int tok2_qty2;
    }
   
    public partial class AddToken : Form
    {
        long date = 0;
     
        string token = "";

        List<long> T = new List<long>();
        public void firsttoken(string firs)
        {

            label14.Text = firs;

        }

      

       public _SelectionOut _objOut;
      


       //private static readonly AddToken instance = new AddToken();
       // public static AddToken Instance
       // {
       //     get
       //     {
       //         return instance;
       //     }
       // }
     
       
        public AddToken()
        {
            InitializeComponent();
          
          
        }

        /////////////////////////////////////////////    Exchange  ////////////////////////////////////////////////////////


        void Exchange()
        {
            string[] strexchange = { "NFO", "SPREAD" };
            EXcomboBox1.Items.AddRange(strexchange);
            EXcomboBox1.SelectedIndex = EXcomboBox1.Items.IndexOf("NFO");

            string[] strordertype = { "Normal", "Spread" };
            ORcomboBox2.Items.AddRange(strordertype);
            ORcomboBox2.SelectedIndex = ORcomboBox2.Items.IndexOf("Normal");
            //List<string> list = new List<string>();
            //list.Add("NFO");
            //list.Add("SPREAD");

            //foreach (string ex in list)
            //{
            //    EXcomboBox1.Items.Add(ex);

            //}


        }
        /////////////////////////////////////////////    ORDER Type  ////////////////////////////////////////////////////////

        void order_type()
        {
            List<string> list = new List<string>();
            list.Add("Normal");
            list.Add("Spread");
            foreach (string ex in list)
            {

                ORcomboBox2.Items.Add(ex);

            }


        }

         void InsertType_fun()
        {
       //    Holder. clliest_contractfile = CSV_Class.cimlist.Where(ab => ab.InstrumentName != null).ToList();
            string[] dis = CSV_Class.cimlist.Where(ab => ab.InstrumentName != null && (ab.InstrumentName == "FUTSTK" || ab.InstrumentName == "FUTIDX" || ab.InstrumentName == "OPTIDX" || ab.InstrumentName == "OPTSTK")).Select(a => a.InstrumentName).Distinct().ToArray();
            INSTcomboBox3.Items.AddRange(dis);
        }

        ////////////////////////////////////////////////////////////////////////////////////////// Exoirry  /////////////


        public static DateTime ConvertFromTimestamp(long timstamp)
        {
            DateTime datetime = new DateTime(1980, 1, 1, 0, 0, 0, 0);
            return datetime.AddSeconds(timstamp);
        }

      

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            InstType();
        }
        //////////////////////////////////////////////////////// SYMBOL ////////////////////////

        void InstType()
        {
            SYMcomboBox4.Text = "";
            SYMcomboBox4.Items.Clear();

        //  IEnum symm = CSV_Class.cimlist.Where(a => a.InstrumentName == INSTcomboBox3.Text).Select(q => q.Symbol).Distinct().ToList();
             
         string[] symm = CSV_Class.cimlist.Where(a => a.InstrumentName == INSTcomboBox3.Text && a.InstrumentName !="" && a.InstrumentName !=null ).Select(q => q.Symbol).Distinct().ToArray();
           // string[] symm =Holder. clliest_contractfile.Where(a => a.InstrumentName == INSTcomboBox3.Text && a.InstrumentName != "" && a.InstrumentName != null).Select(q => q.Symbol).Distinct().ToArray();

         SYMcomboBox4.Items.AddRange(symm);
         
            //foreach (string ex in symm)
            //{
            //    if (ex == null || ex == "")
            //    {
            //        continue;
            //    }
            //    else
            //    SYMcomboBox4.Items.Add(ex);
                OPcomboBox6.Enabled = true;
                STRIKecomboBox7.Enabled = true;
                EXPcomboBox5.Items.Clear();
                OPcomboBox6.Items.Clear();
                STRIKecomboBox7.Text = "";

           // }
            if (INSTcomboBox3.Text == "FUTIVX" || INSTcomboBox3.Text == "FUTIDX" || INSTcomboBox3.Text == "FUTSTK")
            {

                OPcomboBox6.Enabled = false;
                STRIKecomboBox7.Enabled = false;
                EXPcomboBox5.Items.Clear();
                OPcomboBox6.Items.Clear();
                STRIKecomboBox7.Items.Clear();


            }


        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            Expiry();
        }

        void Expiry()
        {
            EXPcomboBox5.Text = "";
            EXPcomboBox5.Items.Clear();
           
           // var exp = CSV_Class.cimlist.Where(a => a.Symbol == SYMcomboBox4.Text && a.InstrumentName == INSTcomboBox3.Text).OrderBy(r => r.ExpiryDate).Select(d=>d.ExpiryDate).Distinct().ToList();
            T = CSV_Class.cimlist.Where(a => a.Symbol == SYMcomboBox4.Text && a.InstrumentName == INSTcomboBox3.Text).OrderBy(s => s.ExpiryDate).Select(d=>d.ExpiryDate).Distinct().ToList();

         //   T = Holder.clliest_contractfile.Where(a => a.Symbol == SYMcomboBox4.Text && a.InstrumentName == INSTcomboBox3.Text).OrderBy(s => s.ExpiryDate).Select(d => d.ExpiryDate).Distinct().ToList();
            
            // IEnumerable<long> exp = CSV_Class.cimlist.Where(a => a.Symbol == SYMcomboBox4.Text && a.InstrumentName == INSTcomboBox3.Text).Select(r => r.ExpiryDate).Distinct().ToList();

            foreach (long ex in T)
            {

                string on = ConvertFromTimestamp(ex).ToShortDateString();

                EXPcomboBox5.Items.Add(on);
              //  date = ex;

            }

            OPcomboBox6.Items.Clear();
            STRIKecomboBox7.Items.Clear();


        }
       
        private void EXPcomboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = EXPcomboBox5.SelectedIndex;
            date = T[i];
           
            optionType();
        }
        void optionType()
        {
            OPcomboBox6.Text = "";
            OPcomboBox6.Items.Clear();
           // STRIKecomboBox7.Items.Clear();
            STRIKecomboBox7.Text = "";
            string df = INSTcomboBox3.Text;
             
            string[] op = CSV_Class.cimlist.Where(a => a.ExpiryDate == date && a.InstrumentName == INSTcomboBox3.Text && a.Symbol == SYMcomboBox4.Text).Select(s=>s.OptionType).Distinct().ToArray();
         // string[] op = Holder.clliest_contractfile.Where(a => a.ExpiryDate == date && a.InstrumentName == INSTcomboBox3.Text && a.Symbol == SYMcomboBox4.Text).Select(s => s.OptionType).Distinct().ToArray();
         
          var tokenw = CSV_Class.cimlist.Where(a => a.ExpiryDate == date && a.Symbol == SYMcomboBox4.Text).First().Token;
            token = tokenw.ToString();
            OPcomboBox6.Items.AddRange(op);

       //combo_Exoiry.Items.Clear();

            



        }

        private void OPcomboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            strike_prise();
        }
        void strike_prise()
        {

            var p = CSV_Class.cimlist.Where(a => a.ExpiryDate == date && a.InstrumentName == INSTcomboBox3.Text && a.Symbol == SYMcomboBox4.Text && a.OptionType == OPcomboBox6.Text).OrderBy(p1=>p1.StrikePrice) .Select(a => a.StrikePrice/100) .Distinct().ToList();
            STRIKecomboBox7.DataSource = p;
            STRIKecomboBox7.DisplayMember = "StrikePrice";
             //STRIKecomboBox7.ValueMember = "StrikePrice";
            //int price=CSV_Class.cimlist[p].StrikePrice;
           //foreach(int x in p)
          //STRIKecomboBox7.Items.Add(x);


        }

        private void STRIKecomboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        int oldtoken = 0;
        DateTime dt = System.DateTime.Now;
        private void button1_Click(object sender, EventArgs e)
        {
           
            if (INSTcomboBox3.Text == "" || SYMcomboBox4.Text == "" || EXPcomboBox5.Text == "")
                return;
            int t = 0;
            if (INSTcomboBox3.Text == "FUTIVX" || INSTcomboBox3.Text == "FUTIDX" || INSTcomboBox3.Text == "FUTSTK")
            {
               
                t = CSV_Class.cimlist.FindIndex(q => q.Symbol == SYMcomboBox4.Text && q.ExpiryDate == this.date && q.InstrumentName == INSTcomboBox3.Text);
            }
            else 
            {
                if (INSTcomboBox3.Text == "" || SYMcomboBox4.Text == "" || EXPcomboBox5.Text == "" || OPcomboBox6.Text == "" || STRIKecomboBox7.Text == "" )
                {
                    MessageBox.Show("Please Select All Fiels ...", "Warning" , MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                t = CSV_Class.cimlist.FindIndex(q => q.Symbol == SYMcomboBox4.Text.Trim() && q.ExpiryDate == date && q.InstrumentName == INSTcomboBox3.Text.Trim() && q.OptionType == OPcomboBox6.Text.Trim() && q.StrikePrice == Convert.ToInt32(STRIKecomboBox7.Text.Trim())*100);
            }
            int Token =CSV_Class.cimlist[t].Token;
           
            if (button1.Text != "Finish")
            {
             

                dt = Convert.ToDateTime(EXPcomboBox5.Text);
                var v = CSV_Class.cimlist.Where(a => a.Token == Token).ToList();
                _objOut.tok1_qty1 = v.FirstOrDefault().BoardLotQuantity;
                lblPfName.Visible = false;
                txtpfName.Visible = false;
                this.Text = "Add Far Month Token";
                button1.Text = "Finish";
               
                _objOut.PFName = txtpfName.Text;
                _objOut.Token1 = Token;
                _objOut.Desc1 = CSV_Class.cimlist[t].EGMAGM;
                INSTcomboBox3.Enabled = false;
                SYMcomboBox4.Enabled = false;
                EXcomboBox1.Enabled = false;
                ORcomboBox2.Enabled = false;
             //   this.Refresh();
                EXcomboBox1.Focus();
            }
            else
            {
                if (_objOut.Token1 == Token)
                {
                    MessageBox.Show("Token Not Be Same", "Information Message");
                    EXcomboBox1.Focus();
                    return;
                }
                DateTime ab = Convert.ToDateTime(EXPcomboBox5.Text);
                if (dt > Convert.ToDateTime(EXPcomboBox5.Text))
                {
                    MessageBox.Show("Expiry Date Should be gretter then Ist Expiry Date");
                    return;
                }
               
                //FinalPrice fp = new FinalPrice();
                //Holder.holderData.TryGetValue(Token, out fp);

                //Global.Instance.dr["Token2Name"] = CSV_Class.cimlist[t].EGMAGM;
                //Global.Instance.dr["Token2"] = Token;
                //Global.Instance.dr["Token2Bid"] = Math.Round(fp.MAXBID/100.00,2);
                //Global.Instance.dr["Token2Ask"] = Math.Round(fp.MINASK/ 100.00,2);
                //Global.Instance.dr["Token2Ltp"] = Math.Round(fp.LTP / 100.00,2);
                //Global.Instance.dr["NearBidDiff"] = Math.Round(Convert.ToDouble(Global.Instance.dr["Token2Ask"]) - Convert.ToDouble(Global.Instance.dr["Token1Ask"]),2);
                //Global.Instance.dr["NearHitDiff"] = Math.Round(Convert.ToDouble(Global.Instance.dr["Token2Bid"]) - Convert.ToDouble(Global.Instance.dr["Token1Ask"]),2);
                //Global.Instance.dr["FarBidDiff"] = Math.Round(Convert.ToDouble(Global.Instance.dr["Token2Bid"]) - Convert.ToDouble(Global.Instance.dr["Token1Bid"]),2);
                //Global.Instance.dr["FarHitDiff"] = Math.Round(Convert.ToDouble(Global.Instance.dr["Token2Ask"]) - Convert.ToDouble(Global.Instance.dr["Token1Bid"]),2);
                //Fo_Fo_mktwatch.Instance.SpreadTable.Rows.Add(Global.Instance.dr);
                //Global.Instance.dr=null;
               
                _objOut.Token2 = Token;
                _objOut.Desc2 = CSV_Class.cimlist[t].EGMAGM;
                var v = CSV_Class.cimlist.Where(a => a.Token == Token).ToList();
                _objOut.tok2_qty2 = v.FirstOrDefault().BoardLotQuantity;
                this.DialogResult = DialogResult.OK;
               
            }
        }

        private void AddToken_Load(object sender, EventArgs e)
        {
            Exchange();
            InsertType_fun();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        
    }
}
