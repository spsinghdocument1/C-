using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class frmSpreadOrdEntry : Form
    {
        private Keys _DefaultKey = Keys.F1;

        public frmSpreadOrdEntry()
        {

            InitializeComponent();
        }

        public int FormSpdDialogResult { get; set; }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //  Button btButton = sender as Button;
            if (btnSubmit.Text == "Submit")
            {
                FormSpdDialogResult = (int)OrderEntryButtonCase.SUBMIT;

                DialogResult = DialogResult.OK;
            }
            else
            {
                FormSpdDialogResult = (int)OrderEntryButtonCase.MODIFY;
                DialogResult = DialogResult.OK;
            }
        }

        public Keys DefaultKey
        {
            get { return _DefaultKey; }
            set
            {
                _DefaultKey = value;
                if (_DefaultKey == Keys.F1 || _DefaultKey == Keys.Add)
                {
                    BackColor = Color.Red;
                    ForeColor = Color.White;
                    Text = "Buy Order";
                }
                else if (_DefaultKey == Keys.F2 || _DefaultKey == Keys.Subtract)
                {
                    BackColor = Color.Blue;
                    ForeColor = Color.White;
                    Text = "Sell Order";
                }

            }
        }
       
        private void frmSpreadOrdEntry_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frmSpreadOrdEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
                if (e.KeyCode == Keys.Enter)
                {
                    btnSubmit_Click(sender, e);
                }
            }
            catch (Exception ex)
            {

            }
        }
               
        private void txtTotalQty1_TextChanged(object sender, EventArgs e)
        {
            int dOutput = 1;
            if (Int32.TryParse(txtTotalQty1.Text, out dOutput))
            {
                txtTotalQty2.Text = txtTotalQty1.Text;
            }
            else 
            {
                txtTotalQty1.Text = 0.ToString();
            }
            if (Convert.ToInt32(txtTotalQty1.Text) <= 0)
                txtTotalQty1.Text = 0.ToString();
        }

        private void txtPrice1_TextChanged(object sender, EventArgs e)
        {
            //decimal dOutput = 0.0M;
            
            //if (decimal.TryParse(txtPrice1.Text, out dOutput))
            //{
            //    //Response.Write("Valid double");
            //}
            //else
            //{
            //    txtPrice1.Text = "";
            //}
           
        }

       

        private void txtPrice1_KeyPress(object sender, KeyPressEventArgs e)
        {
             txtTotalQty1.SelectAll();
        }

       

       
    }
}
