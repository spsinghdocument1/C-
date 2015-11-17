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

namespace Client
{
    public partial class frmDiff : Form
    {

        
        public FOPAIRDIFF _FOPairDiff;
        

        public frmDiff()
        {
            InitializeComponent();
        }

        private void btnaccept_Click(object sender, EventArgs e)
        {
            //_FOPairDiff.Divisor= 100;
            //_FOPairDiff.BFSNDIFF = Convert.ToDouble( tbBFSNDIFF.Text);
            //_FOPairDiff.BNSFDIFF = Convert.ToDouble(tbBNSFDIFF.Text);
            //_FOPairDiff.MINQTY = Convert.ToInt32(tbMinQty.Text);
            //_FOPairDiff.MAXQTY =Convert.ToInt32( tbMaxQty.Text);


            //SpreadTable.Columns.Add("BNSFMNQ", typeof(int));
            //SpreadTable.Columns.Add("BFSNMNQ", typeof(int));
            //SpreadTable.Columns.Add("BNSFMXQ", typeof(int));
           // SpreadTable.Columns.Add("BFSNMXQ", typeof(int));

            this.DialogResult = DialogResult.OK;
        }

        private void frmDiff_Load(object sender, EventArgs e)
        {
            //tbBFSNDIFF.Text = _FOPairDiff.BFSNDIFF.ToString();
            //tbBNSFDIFF.Text = _FOPairDiff.BNSFDIFF.ToString();
            //tbMinQty.Text = _FOPairDiff.MINQTY.ToString();
            //tbMaxQty.Text = _FOPairDiff.MAXQTY.ToString();
        }





    }
}
