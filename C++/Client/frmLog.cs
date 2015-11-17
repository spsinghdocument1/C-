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
    public partial class frmLog : Form
    {
        
        private frmLog()
        {
            InitializeComponent();
        }

        static frmLog()
        {
        }
        private static readonly frmLog _instance = new frmLog();
        public static frmLog Instance
        {
            get
            {
                return _instance;
            }
        }

        private void frmLog_Load(object sender, EventArgs e)
        {

        }

        private void frmLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

    }
}
