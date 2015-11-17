using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class frmErrorLog : Form
    {
        public frmErrorLog()
        {
            InitializeComponent();
        }

        private static readonly frmErrorLog _instance = new frmErrorLog();
        public static frmErrorLog Instance
        {
            get
            {
                return _instance;
            }
        }

        private void frmErrorLog_Load(object sender, EventArgs e)
        {
            tbelog.ForeColor = Color.Red;
        }

        private void frmErrorLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


    }
}
