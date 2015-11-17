using System;
using System.Windows.Forms;
using System.Xml.Schema;



namespace Client
{
    internal enum OrderEntryButtonCase
    {
        SUBMIT = 0,
        MODIFY = 1,
        CANCEL = 2,
        CANCELALLBUY = 3,
        CANCELALLSELL = 4,
        CANCELFRESH = 5,
        CENCELALL = 6,
        NONE = -1
    }


    public partial class FrmOrderEntry : Form
    {
        private double mLEG_PRICE;
        private int mLEG_SIZE;

        public FrmOrderEntry()
        {
            InitializeComponent();
        }

        public int LEG_SIZE
        {
            get { return mLEG_SIZE; }
            set
            {
                mLEG_SIZE = value;
                txtQty.Text = mLEG_SIZE.ToString();
            }
        }

        public double LEG_PRICE
        {
            get { return mLEG_PRICE; }
            set
            {
                mLEG_PRICE = value;
                txtOrderPrice.Text = value.ToString();
            }
        }

        public int FormDialogResult { get; set; }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
          //  Button btButton = sender as Button;
            if (btnSubmit.Text == "Submit")
            {
                FormDialogResult = (int) OrderEntryButtonCase.SUBMIT;
                DialogResult = DialogResult.OK;
            }
            else
            {
                FormDialogResult = (int)OrderEntryButtonCase.MODIFY;
                DialogResult = DialogResult.OK;
            }
        }

        private void FrmOrderEntry_Load(object sender, EventArgs e)
        {
            cmbBookType.Items.Add("RL");
            cmbBookType.Items.Add("SL");
            cmbValidity.Items.Add("DAY");
            cmbValidity.Items.Add("GTD");
            cmbValidity.Items.Add("GTC");
            cmbAccount.Items.Add("PRO");
            cmbAccount.SelectedIndex = 0;
            cmbBookType.SelectedIndex = 0;
            cmbValidity.SelectedIndex = 0;
            
        }

        private void cmbBookType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbBookType.Text)
            {
                case "RL":
                    txtTriggerPrice.Enabled = false;
                    break;
                default:
                    txtTriggerPrice.Enabled = true;
                    break;
            }
        }

        private void cmbValidity_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbValidity.Text)
            {
                case "GTD":
                    dtpGTD.Enabled = true;
                    break;
                default:
                    dtpGTD.Enabled = false;
                    break;
            }
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text.Length>0)
            {               
            mLEG_SIZE = Convert.ToInt32(txtQty.Text);
            }
        }

        private void txtOrderPrice_TextChanged(object sender, EventArgs e)
        {
            Double n;
            if (txtOrderPrice.Text.Trim().Length >0)
              if( Double.TryParse(txtOrderPrice.Text, out n))
                 mLEG_PRICE = Convert.ToDouble(n);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
           
        }

        private void FrmOrderEntry_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FormDialogResult = (int) OrderEntryButtonCase.NONE;
                DialogResult = DialogResult.Cancel;
            }
        }

        private void txtOrderPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSubmit_Click(sender, e);
            }
        }

        private void txtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                if (Convert.ToInt32(txtQty.Text.Length) ==0)
                {
                    txtQty.Text = 0.ToString();
                }
               else if(Convert.ToInt32(txtQty.Text) <=0)
                {
                    txtQty.Text = 0.ToString();
                }
            }
        }
    }
}