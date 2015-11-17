using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using Client.LZO_NanoData;
using AMS.Profile;

namespace Client.Spread
{
    public partial class frmSpreadMktWatch : Form
    {
        DataView dvMktWatch;
        PlaceSpreadOrder pSpreadOrd;
        public frmSpreadMktWatch()
        {
            InitializeComponent();
        }
        public static int[] LoadFormLocationAndSize(Form xForm)
        {
            int[] t = { 0, 0, 900, 300 };
            if (!File.Exists(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "SpreadMKTformclose.xml"))
                return t;
            DataSet dset = new DataSet();
            dset.ReadXml(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "SpreadMKTformclose.xml");
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
               // MessageBox.Show(ex.Message);
            }
            return LocationAndSize;
        }

        public static void SaveFormLocationAndSize(object sender, FormClosingEventArgs e)
        {
            Form xForm = sender as Form;
            var settings = new XmlWriterSettings { Indent = true };

            XmlWriter writer = XmlWriter.Create(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "SpreadMKTformclose.xml", settings);

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
       
        private void frmSpreadMktWatch_Load(object sender, EventArgs e)
         {
            var AbbA = LoadFormLocationAndSize(this);
            this.Location = new Point(AbbA[0], AbbA[1]);
            this.Size = new Size(AbbA[2], AbbA[3]);
            dvMktWatch = new DataView(CommonData.dtSpreadMktWatch);
            dgvMktWatch.BindSourceView = dvMktWatch;
            dgvMktWatch.LoadSaveSettings();
                       
          
            cmbInstType.Items.Clear();
            var distinctInstNames = (from row in CommonData.dtSpreadContract.AsEnumerable() select row.Field<string>("InstrumentName1")).Distinct().ToArray();
            cmbInstType.Items.AddRange(distinctInstNames);
            if (cmbInstType.Items.Count > 0)
                cmbInstType.SelectedIndex = 0;

            LzoNanoData.Instance.OnSpreadDataChange += spradTableMethods.UpdateRecord;
            this.FormClosing += new FormClosingEventHandler(SaveFormLocationAndSize);

            //this.dgvMktWatch.DefaultCellStyle.Font = new Font("Tahoma", 15);
            //this.dgvMktWatch.DefaultCellStyle.ForeColor = Color.White;
            //this.dgvMktWatch.DefaultCellStyle.BackColor = Color.Black;
            //this.dgvMktWatch.DefaultCellStyle.SelectionForeColor = Color.White;
            //this.dgvMktWatch.DefaultCellStyle.SelectionBackColor = Color.Black;

            var config = new Config { GroupName = null };
            string iforms =(string) config.GetValue("Spread_mkt", Convert.ToString(0));
            iforms = iforms + ".xml";
            if (File.Exists(iforms))
            {
                // CommonData.dtSpreadMktWatch.ReadXml(iforms);
                CommonData.dtSpreadMktWatch.ReadXml(iforms);
            }
                foreach (DataRow dr in CommonData.dtSpreadMktWatch.Rows)
                {
                    // Global.Instance.pData.SubscribeSpread = dr[SpreadContract.Token1] + "-" + dr[SpreadContract.Token2];

                    Global.Instance.BoardLotDict.TryAdd(Convert.ToInt32(dr[SpreadContract.Token2]), new int());
                    Global.Instance.BoardLotDict[Convert.ToInt32(dr[SpreadContract.Token2])] = Convert.ToInt32(dr[SpreadContract.BoardLotQuantity2]);

                    LZO_NanoData.LzoNanoData.Instance.Subscribe = Convert.ToInt64(dr[SpreadContract.Token1].ToString() + dr[SpreadContract.Token2].ToString());
                    Global.Instance.Data_With_Nano.AddOrUpdate(Convert.ToInt64(dr[SpreadContract.Token1].ToString() + dr[SpreadContract.Token2].ToString()), ClassType.SPREAD, (k, v) => ClassType.SPREAD);

                }
               
               

            
      
        }

        public void InitDict()
        {
            foreach (DataGridViewRow r in dgvMktWatch.Rows)
            {
                // row = r;
                // spradTableMethods._SprdwatchDict.Add(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value), r);
                //  dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
                //Global.Instance._SprdwatchDict.AddOrUpdate(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token2].Value).ToString(), r, (k, v) => r);

                Global.Instance._SprdwatchDict[Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token2].Value).ToString()] = r;


            }

        }

        private void cmbInstType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSymbol.Items.Clear();
            var distinctSymbol = (from m in CommonData.dtSpreadContract.AsEnumerable() where m.Field<string>("InstrumentName1") == cmbInstType.SelectedItem.ToString() select m.Field<string>("Symbol1")).Distinct().ToArray();

            cmbSymbol.Items.AddRange(distinctSymbol);
            if (cmbSymbol.Items.Count > 0)
                cmbSymbol.SelectedIndex = 0;
        }

        private void cmbSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbOptionType.Items.Clear();
           
            var distinctOpType = (from row in CommonData.dtSpreadContract.AsEnumerable()
                                       where row.Field<string>("InstrumentName1") == cmbInstType.SelectedItem.ToString()
                                       && row.Field<string>("Symbol1") == cmbSymbol.SelectedItem.ToString()
                                  select row.Field<string>("OptionType1")).Distinct().ToArray();

            cmbOptionType.Items.AddRange(distinctOpType);
            if(distinctOpType.Length>0)
            cmbOptionType.SelectedIndex = 0;
        }

        private void cmbOptionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbStrikePrice.Items.Clear();
            var distinctStrikePrice = (from row in CommonData.dtSpreadContract.AsEnumerable()
                                 where row.Field<string>("InstrumentName1") == cmbInstType.SelectedItem.ToString()
                                 && row.Field<string>("Symbol1") == cmbSymbol.SelectedItem.ToString()
                                 && row.Field<string>("OptionType1") == cmbOptionType.SelectedItem.ToString()
                                  select row.Field<Int32>("StrikePrice1").ToString()).Distinct().ToArray();

            cmbStrikePrice.Items.AddRange(distinctStrikePrice);
            if (distinctStrikePrice.Length > 0)
                cmbStrikePrice.SelectedIndex = 0;
        }
        long selectedExp1, selectedExp2;
        private void cmbStrikePrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbExpiry.Items.Clear();
            selectedExp1 = 0;
            selectedExp2 = 0;
            var distinctExpiry = (from row in CommonData.dtSpreadContract.AsEnumerable()
                                       where row.Field<string>("InstrumentName1") == cmbInstType.SelectedItem.ToString()
                                       && row.Field<string>("Symbol1") == cmbSymbol.SelectedItem.ToString()
                                       && row.Field<int>("StrikePrice1") == Convert.ToInt32(cmbStrikePrice.SelectedItem.ToString())
                                       && row.Field<string>("OptionType1") == cmbOptionType.SelectedItem.ToString()
                                       select new 
                                       {
                                           dt1= row.Field<int>("ExpiryDate1"),
                                           dt2=row.Field<int>("ExpiryDate2")
                                       });
            
            foreach(var i in distinctExpiry)
            {
                selectedExp1 = i.dt1;
                selectedExp2 = i.dt2;

                var combinedExpiry = (CommonData.UnixTimeStampToDateTime(i.dt1).ToString("ddMMMyy-").ToUpper() + CommonData.UnixTimeStampToDateTime(i.dt2).ToString("ddMMMyy").ToUpper());
                cmbExpiry.Items.Add(combinedExpiry);
            }
            if(cmbExpiry.Items.Count>0)
            cmbExpiry.SelectedIndex = 0;
                    
        }

       
        int selectedTkn1, selectedTkn2;
        private void cmbExpiry_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime userDate = DateTime.ParseExact(((cmbExpiry.SelectedItem.ToString().Split('-')[0]).ToUpper()), "ddMMMyy", null);
            DateTime dateValue = new DateTime(userDate.Year,userDate.Month,userDate.Day,14,30, 0, 0);
            selectedExp1 = CommonData.UnixTimestampFromDateTime(dateValue);
            userDate = DateTime.ParseExact(((cmbExpiry.SelectedItem.ToString().Split('-')[1]).ToUpper()), "ddMMMyy", null);
            dateValue = new DateTime(userDate.Year, userDate.Month, userDate.Day, 14, 30, 0, 0);
            selectedExp2 = CommonData.UnixTimestampFromDateTime(dateValue);

            cmbSymDiscription.Items.Clear();
            var distiSymbol = (from row in CommonData.dtSpreadContract.AsEnumerable()
                                  where row.Field<string>("InstrumentName1") == cmbInstType.SelectedItem.ToString()
                                  && row.Field<string>("Symbol1") == cmbSymbol.SelectedItem.ToString()
                                  && row.Field<int>("StrikePrice1") == Convert.ToInt32(cmbStrikePrice.SelectedItem.ToString())
                                  && row.Field<string>("OptionType1") == cmbOptionType.SelectedItem.ToString()
                                  && row.Field<int>("ExpiryDate1") ==  selectedExp1
                                  && row.Field<int>("ExpiryDate2") ==  selectedExp2
                                  select new
                                  {
                                      sym1 = row.Field<string>("Symbol1"),
                                      sym2 = row.Field<string >("Symbol2"),
                                      tk1 = row.Field<int>("Token1"),
                                      tk2 = row.Field<int>("Token2")
                                  });

            foreach (var i in distiSymbol)
            {
                var combinedSym = (i.sym1 + cmbExpiry.SelectedItem.ToString().Split('-')[0]).ToUpper() + (i.sym2 + cmbExpiry.SelectedItem.ToString().Split('-')[1]).ToUpper();
                cmbSymDiscription.Items.Add(combinedSym);
                selectedTkn1 = i.tk1;
                selectedTkn2 = i.tk2;
            }
            if (cmbSymDiscription.Items.Count > 0)
                cmbSymDiscription.SelectedIndex = 0;
        }
        DataGridViewRow row;
        private void cmbSymDiscription_Enter(object sender, EventArgs e)
        {

            DataRow[] drMktWatch = CommonData.dtSpreadMktWatch.Select("Token1=" + selectedTkn1 + " and " + "Token2=" + selectedTkn2);
          
            if (drMktWatch.Length > 0)
                return;
            LZO_NanoData.LzoNanoData.Instance.Subscribe = Convert.ToInt64(selectedTkn1.ToString() + selectedTkn2.ToString());
            Global.Instance.Data_With_Nano.AddOrUpdate(Convert.ToInt64(selectedTkn1.ToString() + selectedTkn2.ToString()), ClassType.SPREAD, (k, v) => ClassType.SPREAD);
           
            DateTime userDate = DateTime.ParseExact(((cmbExpiry.SelectedItem.ToString().Split('-')[0]).ToUpper()), "ddMMMyy", null);
            DateTime dateValue = new DateTime(userDate.Year, userDate.Month, userDate.Day, 14, 30, 0, 0);
            selectedExp1 = CommonData.UnixTimestampFromDateTime(dateValue);
            userDate = DateTime.ParseExact(((cmbExpiry.SelectedItem.ToString().Split('-')[1]).ToUpper()), "ddMMMyy", null);
            dateValue = new DateTime(userDate.Year, userDate.Month, userDate.Day, 14, 30, 0, 0);
            selectedExp2 = CommonData.UnixTimestampFromDateTime(dateValue);
            
            DataRow drRec= CommonData.dtSpreadMktWatch.NewRow();
          //  DataRow[] drContract = CommonData.dtSpreadContract.Select("ExpiryDate1=" + selectedExp1 + " and " + "ExpiryDate2=" + selectedExp2 + " and " + "InstrumentName1=" + cmbInstType.SelectedItem.ToString());
            DataRow[] drContract = CommonData.dtSpreadContract.Select("Token1=" + selectedTkn1 + " and " + "Token2=" + selectedTkn2);
                    
             if (drContract.Length > 0)
                {
                   //Global.Instance.pData.SubscribeSpread = selectedTkn1 + "-" + selectedTkn2;
                    drRec[SpreadContract.Symbol1] = drContract[0][SpreadContract.Symbol1];
                    drRec[SpreadContract.Symbol2] = drContract[0][SpreadContract.Symbol2];
                    drRec[SpreadContract.Token1] = drContract[0][SpreadContract.Token1];
                    drRec[SpreadContract.Token2] = drContract[0][SpreadContract.Token2];
                    drRec[SpreadContract.StrikePrice1] = drContract[0][SpreadContract.StrikePrice1];
                    drRec[SpreadContract.StrikePrice2] = drContract[0][SpreadContract.StrikePrice2];
                    drRec[SpreadContract.OptionType1] = drContract[0][SpreadContract.OptionType1];
                    drRec[SpreadContract.OptionType2] = drContract[0][SpreadContract.OptionType2];
                    drRec[SpreadContract.InstrumentName1] = drContract[0][SpreadContract.InstrumentName1];
                    drRec[SpreadContract.InstrumentName2] = drContract[0][SpreadContract.InstrumentName2];
                    drRec[SpreadContract.ExpiryDate1] = CommonData.UnixTimeStampToDateTime(Convert.ToInt32(drContract[0][SpreadContract.ExpiryDate1])).ToString("ddMMM").ToUpper();
                    drRec[SpreadContract.ExpiryDate2] = CommonData.UnixTimeStampToDateTime(Convert.ToInt32(drContract[0][SpreadContract.ExpiryDate2])).ToString("ddMMM").ToUpper();
                    drRec[SpreadContract.CALevel2] = drContract[0][SpreadContract.CALevel2];
                    drRec[SpreadContract.CALevel1] = drContract[0][SpreadContract.CALevel1];
                    drRec[SpreadContract.UnixExpiry1] = drContract[0][SpreadContract.ExpiryDate1];
                    drRec[SpreadContract.UnixExpiry2] = drContract[0][SpreadContract.ExpiryDate2];
                    drRec[SpreadContract.BoardLotQuantity1] = drContract[0][SpreadContract.BoardLotQuantity1];
                    drRec[SpreadContract.BoardLotQuantity2] = drContract[0][SpreadContract.BoardLotQuantity1];
                }
             

             // DGV1.Rows[row.Index].Cells["BNSFTQ"]
            //Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber), new Order((int)_Type.MS_OE_RESPONSE_TR));
           //Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].mS_OE_RESPONSE_TR = obj;
          //userDic.AddOrUpdate(authUser.UserId,sessionId,(key, oldValue) => sessionId);
             Global.Instance.BoardLotDict.TryAdd(Convert.ToInt32(drRec[SpreadContract.Token1]),new int());
            Global.Instance.BoardLotDict[Convert.ToInt32(drRec[SpreadContract.Token1])] =Convert.ToInt32(drRec[SpreadContract.BoardLotQuantity1]);

            Global.Instance.BoardLotDict.TryAdd(Convert.ToInt32(drRec[SpreadContract.Token2]), new int());
            Global.Instance.BoardLotDict[Convert.ToInt32(drRec[SpreadContract.Token2])] = Convert.ToInt32(drRec[SpreadContract.BoardLotQuantity2]);

             CommonData.dtSpreadMktWatch.Rows.Add(drRec);
             foreach (DataGridViewRow r in dgvMktWatch.Rows)
             {
                 // row = r;
                // spradTableMethods._SprdwatchDict.Add(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value), r);
                                                                                                                                                                                                                                                                                                                                                                                                                                                             //  dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
                 //Global.Instance._SprdwatchDict.AddOrUpdate(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token2].Value).ToString(), r,(k,v)=>r);
                 Global.Instance._SprdwatchDict[Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token2].Value).ToString()]= r;
             }
            
          

        }

        private void btnsaveMktwatch_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var config = new Config { GroupName = null };
                CommonData.dtSpreadMktWatch.WriteXml(saveFileDialog1.FileName+".xml", true);
                config.SetValue("Spread_mkt", Convert.ToString(0), saveFileDialog1.FileName);
            }
        }

        private void btnLoadMktWatch_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();

            if (OpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CommonData.dtSpreadMktWatch.ReadXml(OpenFile.FileName);
            }

            foreach(DataRow dr in CommonData.dtSpreadMktWatch.Rows)
            {
               // Global.Instance.pData.SubscribeSpread = dr[SpreadContract.Token1] + "-" + dr[SpreadContract.Token2];

                Global.Instance.BoardLotDict.TryAdd(Convert.ToInt32(dr[SpreadContract.Token2]), new int());
                Global.Instance.BoardLotDict[Convert.ToInt32(dr[SpreadContract.Token2])] = Convert.ToInt32(dr[SpreadContract.BoardLotQuantity2]);

                LZO_NanoData.LzoNanoData.Instance.Subscribe = Convert.ToInt64(dr[SpreadContract.Token1].ToString() + dr[SpreadContract.Token2].ToString());
                Global.Instance.Data_With_Nano.AddOrUpdate(Convert.ToInt64(dr[SpreadContract.Token1].ToString() + dr[SpreadContract.Token2].ToString()), ClassType.SPREAD, (k, v) => ClassType.SPREAD);
            
            }
            foreach (DataGridViewRow r in dgvMktWatch.Rows)
            {
                // row = r;
                // spradTableMethods._SprdwatchDict.Add(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value), r);
                //  dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
                //Global.Instance._SprdwatchDict.AddOrUpdate(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token2].Value).ToString(), r, (k, v) => r);
                Global.Instance._SprdwatchDict[Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token2].Value).ToString()]= r;
            }



           
        }

        private void frmSpreadMktWatch_FormClosing(object sender, FormClosingEventArgs e)
        {
           // Global.Instance.pData.OnSpreadDataChange -= spradTableMethods.UpdateRecord;
          //  LzoNanoData.Instance.OnSpreadDataChange -= spradTableMethods.UpdateRecord;
            e.Cancel = true;
            this.Hide();
        }

        private void dgvMktWatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.F1 || e.KeyCode == Keys.Oemplus)
            {
                foreach (DataGridViewRow DGVR in dgvMktWatch.SelectedRows)
                {
                    BuyOrder(DGVR);
                }
            }
            else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.F2 || e.KeyCode == Keys.OemMinus)
            {
                foreach (DataGridViewRow DGVR in dgvMktWatch.SelectedRows)
                {
                    SellOrder(DGVR);
                }
            }

            else if (e.KeyCode == Keys.Delete)
            {
                if (dgvMktWatch.SelectedRows.Count > 0)
                {
                    //if (MessageBox.Show(this, "Are you sure you Want to delete selected script?",
                    //                      Application.ProductName, MessageBoxButtons.YesNo,
                    //                      MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                        DataGridViewSelectedRowCollection dr = dgvMktWatch.SelectedRows;
                        DataGridViewRow temp = null;
                        for (int i = 0; i < dr.Count; i++)
                        {
                            int idx = dr[i].Index;
                           // Global.Instance._SprdwatchDict.TryRemove(Convert.ToInt32(dr[i].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dr[i].Cells[SpreadContract.Token2].Value).ToString(), out temp);
                            Global.Instance._SprdwatchDict.Remove(Convert.ToInt32(dr[i].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dr[i].Cells[SpreadContract.Token2].Value).ToString());
                            dgvMktWatch.Rows.RemoveAt(idx);
                           // spradTableMethods._SprdwatchDict.AddOrUpdate(Convert.ToInt32(dr[i].Cells[SpreadContract.Token1]).ToString() + Convert.ToInt32(dr[i].Cells[SpreadContract.Token1]).ToString(), r, (k, v) => r);


                           
                        }
                   // }
                }
            }

            else if (e.KeyCode == Keys.F6)
            {
                foreach (DataGridViewRow DGVR in dgvMktWatch.SelectedRows)
                {
                    mktPicture(DGVR);
                }
            }
        }
        private void mktPicture(DataGridViewRow Dr)
        {
            // if (AppGlobal.frmMarketPicture == null)
            // {
            FrmMarketPicture _frmpic = new FrmMarketPicture();
            var distinctInstNames = (from row in CommonData.dtSpreadContract.AsEnumerable() select row.Field<string>("InstrumentName1")).Distinct().ToArray();
            _frmpic.cmbInstrument.Items.AddRange(distinctInstNames);
          
            _frmpic.cmbInstrument.Text = Dr.Cells[SpreadContract.InstrumentName1].Value.ToString();

           var distinctSymbol = (from m in CommonData.dtSpreadContract.AsEnumerable() where m.Field<string>("InstrumentName1") == _frmpic.cmbInstrument.SelectedItem.ToString() select m.Field<string>("Symbol1")).Distinct().ToArray();
           cmbSymbol.Items.AddRange(distinctSymbol);

           _frmpic.cmbSymbol.SelectedIndex = _frmpic.cmbSymbol.Items.IndexOf(Dr.Cells[SpreadContract.Symbol1].Value.ToString());

         //_frmpic.cmbSymbol.Text = Dr.Cells[SpreadContract.Symbol1].Value.ToString();
           _frmpic.cmbExpirty.Text = Dr.Cells[SpreadContract.ExpiryDate1].Value.ToString() + "-" + Dr.Cells[SpreadContract.ExpiryDate2].Value.ToString();
           _frmpic.cmbSeries.Text = Dr.Cells[SpreadContract.OptionType1].Value.ToString();
           _frmpic.cmbStrikePrice.Text = Dr.Cells[SpreadContract.StrikePrice1].Value.ToString();

           _frmpic.token.Text = Dr.Cells[SpreadContract.Token1].Value.ToString() + Dr.Cells[SpreadContract.Token2].Value.ToString();
           _frmpic.Show(this);


            //   FrmMarketPicture _frmpic = new FrmMarketPicture();
            //  _frmpic.Show(this);

            //frmMarketDepth _frmpic = new frmMarketDepth();
            //  _frmpic.cmbInstrument = Dr.Cells[SpreadContract.InstrumentName1].Value;
           // _frmpic.Show(this);


            // frmtest _frmtest = new frmtest();

            //_frmtest.Show();
            // _frmMktWatch.WindowState = FormWindowState.Minimized;

            //  UDP_Reciever.Instance.OnDataArrived += _frmMktWatch.OnDataArrived;
            // }
            // AppGlobal.frmMarketPicture.MdiParent = this;

        }
        private void BuyOrder(DataGridViewRow Dr)
        {
            using (var frmord = new frmSpreadOrdEntry())
            {
                
                frmord.lblSpdOrderMsg.Text = "BUY " + Dr.Cells[SpreadContract.Symbol1].Value + Dr.Cells[SpreadContract.ExpiryDate2].Value;// + Dr.Cells[SpreadContract.ExpiryDate1].Value;
                //frmord.lblSpdOrderMsg.BackColor = Color.Green;
                frmord.BackColor = Color.SeaGreen;
                
                frmord.cmbInstName1.Text = Dr.Cells[SpreadContract.InstrumentName1].Value.ToString();
                frmord.cmbInstName2.Text = Dr.Cells[SpreadContract.InstrumentName2].Value.ToString();
                frmord.cmbSymbol1.Text = Dr.Cells[SpreadContract.Symbol1].Value.ToString();
                frmord.cmbSymbol2.Text = Dr.Cells[SpreadContract.Symbol2].Value.ToString();
                frmord.cmbOpType1.Text = Dr.Cells[SpreadContract.OptionType1].Value.ToString();
                frmord.cmbOpType2.Text = Dr.Cells[SpreadContract.OptionType2].Value.ToString();
                frmord.cmbStrikePrice1.Text = Dr.Cells[SpreadContract.StrikePrice1].Value.ToString();
                frmord.cmbStrikePrice2.Text = Dr.Cells[SpreadContract.StrikePrice2].Value.ToString();
                frmord.cmbExpiry1.Text = Dr.Cells[SpreadContract.ExpiryDate1].Value.ToString();
                frmord.cmbExpiry2.Text = Dr.Cells[SpreadContract.ExpiryDate2].Value.ToString();
                frmord.txtPrice1.Text = Dr.Cells[SpreadContract.Ask].Value.ToString();
                frmord.txtTotalQty1.Text = "1";
                frmord.txtTotalQty2.Text = "1";


                frmord.txtMktLot1.Text = Dr.Cells[SpreadContract.BoardLotQuantity1].Value.ToString();
                frmord.txtMktLot2.Text = Dr.Cells[SpreadContract.BoardLotQuantity2].Value.ToString();
                
                frmord.cmbBuySell1.Text = "SELL";
                frmord.cmbBuySell2.Text = "BUY";

                // frmord.DesktopLocation = new Point(100, 100);
                int x = (Screen.PrimaryScreen.WorkingArea.Width - frmord.Width) / 2;
                int y = (Screen.PrimaryScreen.WorkingArea.Height - frmord.Height) - 50;
                frmord.Location = new Point(x, y);

               // var v = Convert.ToInt32(Dr.Cells[SpreadContract.ExpiryDate1].Value);
                if (frmord.ShowDialog(this) == DialogResult.OK)
                {

                    if (frmord.FormSpdDialogResult == (int)OrderEntryButtonCase.SUBMIT)
                    {
                        //if (MessageBox.Show(this, "Are you sure you Want to Place Buy Order?",
                        //                  Application.ProductName, MessageBoxButtons.YesNo,
                        //                  MessageBoxIcon.Question) == DialogResult.Yes)
                        //{
                            pSpreadOrd = new PlaceSpreadOrder();
                            pSpreadOrd.SecInfo1.Symbol=frmord.cmbSymbol1.Text;
                            pSpreadOrd.SecInfo2.Symbol = frmord.cmbSymbol2.Text;
                            pSpreadOrd.SecInfo1.InstrumentName = frmord.cmbInstName1.Text;
                            pSpreadOrd.SecInfo2.InstrumentName = frmord.cmbInstName2.Text;
                            
                            pSpreadOrd.SecInfo1.ExpiryDate = Convert.ToInt32( Dr.Cells[SpreadContract.UnixExpiry1].Value);
                            pSpreadOrd.SecInfo2.ExpiryDate = Convert.ToInt32(Dr.Cells[SpreadContract.UnixExpiry2].Value);
                            pSpreadOrd.SecInfo1.OptionType = frmord.cmbOpType1.Text;
                            pSpreadOrd.SecInfo2.OptionType = frmord.cmbOpType2.Text;
                            pSpreadOrd.SecInfo1.StrikePrice = Convert.ToInt32(frmord.cmbStrikePrice1.Text);
                            pSpreadOrd.SecInfo2.StrikePrice = Convert.ToInt32(frmord.cmbStrikePrice2.Text);
                            pSpreadOrd.SecInfo1.CALevel = Convert.ToInt16( Dr.Cells[SpreadContract.CALevel1].Value);
                            pSpreadOrd.SecInfo2.CALevel = Convert.ToInt16( Dr.Cells[SpreadContract.CALevel2].Value);

                            pSpreadOrd.Price1 = Convert.ToInt32(Convert.ToSingle(frmord.txtPrice1.Text) * 100);
                            pSpreadOrd.Volume1 = Convert.ToInt32(frmord.txtTotalQty1.Text)* Convert.ToInt32(Dr.Cells[SpreadContract.BoardLotQuantity1].Value);
                            pSpreadOrd.Volume2 = Convert.ToInt32(frmord.txtTotalQty2.Text) * Convert.ToInt32(Dr.Cells[SpreadContract.BoardLotQuantity1].Value);

                            pSpreadOrd.Token1 = Convert.ToInt32(Dr.Cells[SpreadContract.Token1].Value);
                            pSpreadOrd.Token2 = Convert.ToInt32(Dr.Cells[SpreadContract.Token2].Value);

                            pSpreadOrd.BuySell1 = 2;
                            pSpreadOrd.BuySell2 = 1;

                            NNFInOut.Instance.SP_BOARD_LOT_IN(pSpreadOrd);

                            SpdOrderTableMethods._PriceDiff[pSpreadOrd.Token1] = new Dictionary<Structure.BUYSELL, float>();
                            SpdOrderTableMethods._PriceDiff[pSpreadOrd.Token1][Structure.BUYSELL.SELL] = Convert.ToSingle(frmord.txtPrice1.Text);

                      //  }

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
            }
        }

        private void SellOrder(DataGridViewRow Dr)
        {
            using (var frmord = new frmSpreadOrdEntry())
            {

                frmord.lblSpdOrderMsg.Text = "SELL " + Dr.Cells[SpreadContract.Symbol1].Value + Dr.Cells[SpreadContract.ExpiryDate2].Value;//+ Dr.Cells[SpreadContract.ExpiryDate1].Value;
                //frmord.lblSpdOrderMsg.BackColor = Color.Green;
                frmord.BackColor = ControlPaint.Light(Color.IndianRed);//Color.PaleVioletRed;

                frmord.cmbInstName1.Text = Dr.Cells[SpreadContract.InstrumentName1].Value.ToString();
                frmord.cmbInstName2.Text = Dr.Cells[SpreadContract.InstrumentName2].Value.ToString();
                frmord.cmbSymbol1.Text = Dr.Cells[SpreadContract.Symbol1].Value.ToString();
                frmord.cmbSymbol2.Text = Dr.Cells[SpreadContract.Symbol2].Value.ToString();
                frmord.cmbOpType1.Text = Dr.Cells[SpreadContract.OptionType1].Value.ToString();
                frmord.cmbOpType2.Text = Dr.Cells[SpreadContract.OptionType2].Value.ToString();
                frmord.cmbStrikePrice1.Text = Dr.Cells[SpreadContract.StrikePrice1].Value.ToString();
                frmord.cmbStrikePrice2.Text = Dr.Cells[SpreadContract.StrikePrice2].Value.ToString();
                frmord.cmbExpiry1.Text = Dr.Cells[SpreadContract.ExpiryDate1].Value.ToString();
                frmord.cmbExpiry2.Text = Dr.Cells[SpreadContract.ExpiryDate2].Value.ToString();
                frmord.txtPrice1.Text = Dr.Cells[SpreadContract.Bid].Value.ToString();
               frmord.txtTotalQty1.Text = "1";
               frmord.txtTotalQty2.Text = "1";

                frmord.txtMktLot1.Text = Dr.Cells[SpreadContract.BoardLotQuantity1].Value.ToString();
                frmord.txtMktLot2.Text = Dr.Cells[SpreadContract.BoardLotQuantity2].Value.ToString();

                frmord.cmbBuySell1.Text = "BUY";
                frmord.cmbBuySell2.Text = "SELL";
                pSpreadOrd.Token1 = Convert.ToInt32(Dr.Cells[SpreadContract.Token1].Value);
                pSpreadOrd.Token2 = Convert.ToInt32(Dr.Cells[SpreadContract.Token2].Value);

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

                            pSpreadOrd = new PlaceSpreadOrder();
                            pSpreadOrd.SecInfo1.Symbol = frmord.cmbSymbol1.Text;
                            pSpreadOrd.SecInfo2.Symbol = frmord.cmbSymbol2.Text;
                            pSpreadOrd.SecInfo1.InstrumentName = frmord.cmbInstName1.Text;
                            pSpreadOrd.SecInfo2.InstrumentName = frmord.cmbInstName2.Text;

                            pSpreadOrd.SecInfo1.ExpiryDate = Convert.ToInt32(Dr.Cells[SpreadContract.UnixExpiry1].Value);
                            pSpreadOrd.SecInfo2.ExpiryDate = Convert.ToInt32(Dr.Cells[SpreadContract.UnixExpiry2].Value);
                            pSpreadOrd.SecInfo1.OptionType = frmord.cmbOpType1.Text;
                            pSpreadOrd.SecInfo2.OptionType = frmord.cmbOpType2.Text;
                            pSpreadOrd.SecInfo1.StrikePrice = Convert.ToInt32(frmord.cmbStrikePrice1.Text);
                            pSpreadOrd.SecInfo2.StrikePrice = Convert.ToInt32(frmord.cmbStrikePrice2.Text);
                            pSpreadOrd.SecInfo1.CALevel = Convert.ToInt16(Dr.Cells[SpreadContract.CALevel1].Value);
                            pSpreadOrd.SecInfo2.CALevel = Convert.ToInt16(Dr.Cells[SpreadContract.CALevel2].Value);

                            pSpreadOrd.BuySell1 = 1;
                            pSpreadOrd.BuySell2 = 2;
                            pSpreadOrd.Token1 =Convert.ToInt32(Dr.Cells[SpreadContract.Token1].Value);
                            pSpreadOrd.Token2 = Convert.ToInt32(Dr.Cells[SpreadContract.Token2].Value);

                            pSpreadOrd.Price1 = Convert.ToInt32(Convert.ToSingle(frmord.txtPrice1.Text) * 100);
                            pSpreadOrd.Volume1 = Convert.ToInt32(frmord.txtTotalQty1.Text) * Convert.ToInt32(Dr.Cells[SpreadContract.BoardLotQuantity1].Value);
                            pSpreadOrd.Volume2 = Convert.ToInt32(frmord.txtTotalQty2.Text) * Convert.ToInt32(Dr.Cells[SpreadContract.BoardLotQuantity1].Value);
                            
                            NNFInOut.Instance.SP_BOARD_LOT_IN(pSpreadOrd);

                            SpdOrderTableMethods._PriceDiff[pSpreadOrd.Token1] = new Dictionary<Structure.BUYSELL, float>();
                            SpdOrderTableMethods._PriceDiff[pSpreadOrd.Token1][Structure.BUYSELL.BUY] = Convert.ToSingle(frmord.txtPrice1.Text);
                        }
                    }
                }
            }
        }

        private void dgvMktWatch_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dgvMktWatch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvMktWatch_ColumnHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow r in dgvMktWatch.Rows)
            {
                // row = r;
                // spradTableMethods._SprdwatchDict.Add(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value), r);
                //  dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
                //Global.Instance._SprdwatchDict.AddOrUpdate(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token2].Value).ToString(), r, (k, v) => r);
                Global.Instance._SprdwatchDict[Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token2].Value).ToString()]= r;
            }
        }

        private void dgvMktWatch_KeyPress(object sender, KeyPressEventArgs e)
      {
          if (e.KeyChar==13)
          {
              this.textBox1.Text = "";
          }
            if (Char.IsLetter(e.KeyChar))
            {
                int index = 0;
                // This works only if dataGridView1's SelectionMode property is set to FullRowSelect
                this.textBox1.Text = this.textBox1.Text + e.KeyChar.ToString();
                if (dgvMktWatch.SelectedRows.Count > 0)
                {
                    index = dgvMktWatch.SelectedRows[0].Index + 1;
                }
                for (int i = index; i < (dgvMktWatch.Rows.Count + index); i++)
                {
                    if (dgvMktWatch.Rows[i % dgvMktWatch.Rows.Count].Cells[SpreadContract.Symbol1].Value.ToString().StartsWith(this.textBox1.Text, true, CultureInfo.InvariantCulture))
                    {
                        foreach (var row in dgvMktWatch.Rows.Cast<DataGridViewRow>().Where(t => t.Selected))
                        {
                            row.Selected = false;
                        }
                        dgvMktWatch.Rows[i % dgvMktWatch.Rows.Count].Cells[0].Selected = true;
                        dgvMktWatch.FirstDisplayedScrollingRowIndex = dgvMktWatch.SelectedRows[0].Index;
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
