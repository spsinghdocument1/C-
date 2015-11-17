using Client.Spot;
using Client.Spread;
using Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class Child_Index : Form
    {
        private static readonly Child_Index instance = new Child_Index();

        public static Child_Index Instance
        {
            get
            {
                return instance;
            }
        }

        private DataGridViewCellStyle _makeItBlack;
        private DataGridViewCellStyle _makeItBlue;
        private DataGridViewCellStyle _makeItRed;
        public Child_Index()
        {
             InitializeComponent();
            _makeItRed = new DataGridViewCellStyle();
            _makeItBlue = new DataGridViewCellStyle();
            _makeItBlack = new DataGridViewCellStyle();
            _makeItRed.BackColor = Color.LightPink;
            _makeItBlue.BackColor = Color.LightSkyBlue;
            _makeItBlack.BackColor = Color.MidnightBlue;
            this.Index_DGV.DataSource = null;
            this.Index_DGV.DataSource = Global.Instance.Child_Index;

            Index_DGV.Columns[SpreadContract.Symbol].ReadOnly = true;
            Index_DGV.Columns[SpreadContract.Price].ReadOnly = true;
            Index_DGV.Columns[SpreadContract.ChangeIndicator].ReadOnly = true;
            Index_DGV.Columns[SpreadContract.PercentChange].ReadOnly = true;
            Index_DGV.Columns[SpreadContract.ClosePrice].ReadOnly = true;
            Global.Instance.CashSock.ListenMcastData(Convert.ToInt32(Global.Instance.INDEXPORT), Global.Instance.LanIp, Global.Instance.INDEXIP);
        }

        private void Child_Index_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "Child_Index.xml"))
                {
                    DataSet dst = new DataSet();
                    dst.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "Child_Index.xml");
                    DataRow row = null;
                    for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
                    {
                        row = Global.Instance.Child_Index.NewRow();
                        row[SpreadContract.Symbol] = dst.Tables[0].Rows[i][SpreadContract.Symbol].ToString();
                        row[SpreadContract.Price] = dst.Tables[0].Rows[i][SpreadContract.Price].ToString();
                        row[SpreadContract.ChangeIndicator] = dst.Tables[0].Rows[i][SpreadContract.ChangeIndicator].ToString();
                        row[SpreadContract.PercentChange] = dst.Tables[0].Rows[i][SpreadContract.PercentChange].ToString();
                        row[SpreadContract.ClosePrice] = dst.Tables[0].Rows[i][SpreadContract.ClosePrice].ToString();
                        Global.Instance.Child_Index.Rows.Add(row);
                        Global.Instance.Child_Index_Dict.AddOrUpdate(dst.Tables[0].Rows[i][SpreadContract.Symbol].ToString(), dst.Tables[0].Rows[i][SpreadContract.Price].ToString(), (k, v) => dst.Tables[0].Rows[i][SpreadContract.Price].ToString());
                    }
                    Global.Instance.Child_Index.AcceptChanges();
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void Index_DGV_MouseClick(object sender, MouseEventArgs e)
        {
            Index_DGV.ContextMenuStrip = contextMenuStrip1;
        }

        private void showSpotIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppGlobal.frmSpotIndex = new Spot.frmSpot();
           // AppGlobal.frmSpotIndex.MdiParent = this;
            AppGlobal.frmSpotIndex.TopMost = true;
            AppGlobal.frmSpotIndex.Show();
            //Global.Instance.CashSock.ListenMcastData(Convert.ToInt32(Global.Instance.INDEXPORT),Global.Instance.LanIp,Global.Instance.INDEXIP);
        }
        delegate void OnLZOArrivedDelegate( ReadOnlyEventArgs<MS_INDICES_7207> Stat);
        
        public  void UpdateRecord_child_Index( ReadOnlyEventArgs<MS_INDICES_7207> Stat)
        {
            try
            {
                if (AppGlobal.frmSpotIndex != null && AppGlobal.frmSpotIndex.InvokeRequired)
                {
                    AppGlobal.frmSpotIndex.BeginInvoke(new OnLZOArrivedDelegate(UpdateRecord_child_Index), new ReadOnlyEventArgs<MS_INDICES_7207>(Stat.Parameter));
                    
                }
                else
                {
                    if (Global.Instance.Child_Index == null)
                        return;


                    DataRow[] dr = Global.Instance.Child_Index.Select("Symbol='" + Stat.Parameter.IndexName.Trim() + "'");
                        if (dr.Length > 0)
                        {
                            if (Global.Instance._IndexwatchDict.ContainsKey(Stat.Parameter.IndexName.Trim()))
                            SetData(Global.Instance._IndexwatchDict[Stat.Parameter.IndexName.Trim()].Cells[SpreadContract.ChangeIndicator], (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.IndexValue)) / 100 - (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.ClosingIndex)) / 100);
                            SetData(Global.Instance._IndexwatchDict[Stat.Parameter.IndexName.Trim()].Cells[SpreadContract.Price], (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.IndexValue)) / 100 );
                            dr[0][SpreadContract.Price] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.IndexValue)) / 100;
                            dr[0][SpreadContract.ChangeIndicator] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.IndexValue)) / 100 - (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.ClosingIndex)) / 100; //Convert.ToChar(Stat.Parameter.NetChangeIndicator);
                            dr[0][SpreadContract.PercentChange] = (decimal)IPAddress.NetworkToHostOrder(Stat.Parameter.PercentChange) / 100;
                            dr[0][SpreadContract.ClosePrice] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.ClosingIndex)) / 100;

                            // Global.Instance._IndexwatchDict
                           // DataGridViewRow v = Global.Instance._IndexwatchDict[Stat.Parameter.IndexName.Trim()];
                            

                           
                        }
                     /*   else
                        {
                            DataRow drRec =Global.Instance.Child_Index.NewRow();
                            drRec[SpreadContract.Symbol] = Stat.Parameter.IndexName.Trim();
                            drRec[SpreadContract.Price] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.IndexValue)) / 100;
                            drRec[SpreadContract.ChangeIndicator] = Convert.ToChar(Stat.Parameter.NetChangeIndicator);
                            drRec[SpreadContract.PercentChange] = (decimal)IPAddress.NetworkToHostOrder(Stat.Parameter.PercentChange) / 100;
                            drRec[SpreadContract.ClosePrice] = (decimal)(IPAddress.NetworkToHostOrder(Stat.Parameter.ClosingIndex)) / 100;

                            Global.Instance.Child_Index.Rows.Add(drRec);
                        }
                    */

                  
                }
            }
            catch (Exception e)
            {
                //AppGlobal.Logger.WriteinFileWindowAndBox(e, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
                string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpdateRecord_child_Index.txt");
                File.WriteAllText(destPath,e.StackTrace.ToString());
            }
        }
        public void _indexDict()
        {
            try
            {
                foreach (DataGridViewRow r in Index_DGV.Rows)
                {
                    // row = r;
                    // spradTableMethods._SprdwatchDict.Add(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value), r);
                    //  dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
                    //Global.Instance._SprdwatchDict.AddOrUpdate(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token2].Value).ToString(), r, (k, v) => r);

                    Global.Instance._IndexwatchDict[r.Cells[0].Value.ToString()] = r;


                }
            }
            catch(Exception ex)
            {
                string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_indexDict_child_Index.txt");
                File.WriteAllText(destPath, ex.StackTrace.ToString());
            }

        }
        private void SetData(DataGridViewCell DGCell, decimal ValueOne)
        {
            try
            {
                if (DGCell != null)
                {
                    decimal ValueTwo = Convert.ToDecimal(DGCell.Value);
                    if (ValueOne > ValueTwo)
                    {
                        DGCell.Style.ForeColor = Color.Blue;//_makeItBlue;

                    }
                    else if (ValueOne < ValueTwo)
                    {
                        // DGCell.Style = _makeItRed;
                        DGCell.Style.ForeColor = Color.Red;
                    }
                    //else if (ValueOne == ValueTwo)
                    //{
                    //    DGCell.Style = _makeItBlack;
                    //}
                }

                DGCell.Value = ValueOne;
            }
            catch(Exception ex)
            {
                string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SetData_child_Index.txt");
                File.WriteAllText(destPath, ex.StackTrace.ToString());
            }
        }
        private void Child_Index_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {
                DataTable dt = new DataTable("Child_Index");
                foreach (DataGridViewColumn col in Index_DGV.Columns)
                {
                    dt.Columns.Add(col.HeaderText);
                }

                foreach (DataGridViewRow row in Index_DGV.Rows)
                {
                    DataRow dRow = dt.NewRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dRow[cell.ColumnIndex] = cell.Value;
                    }
                    dt.Rows.Add(dRow);
                }


                dt.WriteXml(Application.StartupPath + Path.DirectorySeparatorChar + "Child_Index.xml", true);

                e.Cancel = true;

                this.Hide();
            }
            catch(Exception ex)
            {
                string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Child_Index_FormClosing_child_Index.txt");
                File.WriteAllText(destPath, ex.StackTrace.ToString());
            }

        }

        private void Index_DGV_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (Index_DGV.RowCount <= 0)
                        return;

                    // DataGrid.Rows.RemoveAt(DataGrid.SelectedRows[0].Index);  

                    // Index_DGV.Rows.RemoveAt(Index_DGV.SelectedRows[0].Index);
                    foreach (DataGridViewRow item in this.Index_DGV.SelectedRows)
                    {
                        Index_DGV.Rows.RemoveAt(item.Index);
                    }



                }
            }catch(Exception t)
            {
                string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index_DGV_child_Index.txt");
                File.WriteAllText(destPath, t.StackTrace.ToString());
            }
        }

        private void Index_DGV_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow r in Index_DGV.Rows)
                {
                    // row = r;
                    // spradTableMethods._SprdwatchDict.Add(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value), r);
                    //  dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
                    //Global.Instance._SprdwatchDict.AddOrUpdate(Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token1].Value).ToString() + Convert.ToInt32(dgvMktWatch.Rows[r.Index].Cells[SpreadContract.Token2].Value).ToString(), r, (k, v) => r);

                    Global.Instance._IndexwatchDict[r.Cells[0].Value.ToString()] = r;


                }
            }
            catch(Exception ex)
            {
                string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index_DGV_RowsAdded_child_Index.txt");
                File.WriteAllText(destPath, ex.StackTrace.ToString());
            }
        }

        private void Index_DGV_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
