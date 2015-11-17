using Client.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Client.Spot
{
    public partial class frmSpot : Form
    {
        DataView dvSpotWatch;
        public frmSpot()
        {
            InitializeComponent();
        }

        private void frmSpot_Load(object sender, EventArgs e)
        {
            dvSpotWatch = new DataView(Client.Spread.CommonData.dtSpotWatch);
            dgvMktWatch.BindSourceView = dvSpotWatch;
            Global.Instance.cashDataSection.OSpotnIndexChange += SpotTableMethods.UpdateRecord;
        }

        private void frmSpot_FormClosing(object sender, FormClosingEventArgs e)
        {
           Global.Instance.cashDataSection.OSpotnIndexChange -= SpotTableMethods.UpdateRecord;
        }

        private void dgvMktWatch_MouseClick(object sender, MouseEventArgs e)
        {
            dgvMktWatch.ContextMenuStrip = contextMenuStrip1;
        }

        private void childFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
          //Child_Index.Instance.Show();
        }

        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow DGVR in dgvMktWatch.SelectedRows)
            {
            /*
            DataTable dt = new DataTable();
            dt.Columns.Add(SpreadContract.Symbol, typeof(string));
            dt.Columns.Add(SpreadContract.Price, typeof(string));
            dt.Columns.Add(SpreadContract.ChangeIndicator, typeof(string));
            dt.Columns.Add(SpreadContract.PercentChange, typeof(string));
            dt.Columns.Add(SpreadContract.ClosePrice, typeof(string));
            */
            //_frmpic.cmbInstrument.Text = Dr.Cells["InstrumentName"].Value.ToString();

            Global.Instance.Child_Index_Dict.AddOrUpdate(DGVR.Cells[SpreadContract.Symbol].Value.ToString(),DGVR.Cells[SpreadContract.Price].Value.ToString(),(k,v)=>DGVR.Cells[SpreadContract.Price].Value.ToString());
            //Child_Index.Instance.Index_DGV.Rows[0].Cells[SpreadContract.Symbol].Value = DGVR.Cells[SpreadContract.Symbol].Value.ToString();
           //Child_Index.Instance.Index_DGV.Rows[0].Cells[SpreadContract.Price].Value = DGVR.Cells[SpreadContract.Price].Value.ToString();
          //Child_Index.Instance.Index_DGV.Rows[0].Cells[SpreadContract.ChangeIndicator].Value = DGVR.Cells[SpreadContract.ChangeIndicator].Value.ToString();
         //Child_Index.Instance.Index_DGV.Rows[0].Cells[SpreadContract.PercentChange].Value = DGVR.Cells[SpreadContract.PercentChange].Value.ToString();
        //Child_Index.Instance.Index_DGV.Rows[0].Cells[SpreadContract.ClosePrice].Value = DGVR.Cells[SpreadContract.ClosePrice].Value.ToString();
                DataRow[] dr = Global.Instance.Child_Index.Select("Symbol='" + DGVR.Cells[SpreadContract.Symbol].Value.ToString() + "'");
                if (dr.Length > 0)
                {
                    dr[0][SpreadContract.Price] = DGVR.Cells[SpreadContract.Price].Value.ToString();
                    dr[0][SpreadContract.ChangeIndicator] = DGVR.Cells[SpreadContract.ChangeIndicator].Value.ToString();
                    dr[0][SpreadContract.PercentChange] = DGVR.Cells[SpreadContract.PercentChange].Value.ToString();
                    dr[0][SpreadContract.ClosePrice] = DGVR.Cells[SpreadContract.ClosePrice].Value.ToString();
                }
               else
               {
               DataRow drRec =Global.Instance.Child_Index.NewRow();
               drRec[SpreadContract.Symbol] = DGVR.Cells[SpreadContract.Symbol].Value.ToString();
               drRec[SpreadContract.Price] = DGVR.Cells[SpreadContract.Price].Value.ToString();
               drRec[SpreadContract.ChangeIndicator] = DGVR.Cells[SpreadContract.ChangeIndicator].Value.ToString();
               drRec[SpreadContract.PercentChange] = DGVR.Cells[SpreadContract.PercentChange].Value.ToString();
               drRec[SpreadContract.ClosePrice] = DGVR.Cells[SpreadContract.ClosePrice].Value.ToString();
               Global.Instance.Child_Index.Rows.Add(drRec);
                   }

              
                
              }
        }

        private void dgvMktWatch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvMktWatch_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
