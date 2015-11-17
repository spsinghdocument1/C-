namespace Client
{
    partial class frmTradeBook
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DGV = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.lblnooftrade = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbln_v = new System.Windows.Forms.Label();
            this.lbls_v = new System.Windows.Forms.Label();
            this.lbls_q = new System.Windows.Forms.Label();
            this.lblb_V = new System.Windows.Forms.Label();
            this.lblb_q = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblsprice = new System.Windows.Forms.Label();
            this.lblsqty = new System.Windows.Forms.Label();
            this.lblbprice = new System.Windows.Forms.Label();
            this.lblBQty = new System.Windows.Forms.Label();
            this.NV = new System.Windows.Forms.Label();
            this.N_V = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AllowUserToOrderColumns = true;
            this.DGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Location = new System.Drawing.Point(0, 31);
            this.DGV.MultiSelect = false;
            this.DGV.Name = "DGV";
            this.DGV.ReadOnly = true;
            this.DGV.RowHeadersVisible = false;
            this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV.Size = new System.Drawing.Size(754, 212);
            this.DGV.TabIndex = 1;
            this.DGV.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_DataError);
            this.DGV.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.DGV_RowPrePaint);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(754, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.toolStripButton1.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(48, 22);
            this.toolStripButton1.Text = "Profile";
            this.toolStripButton1.ToolTipText = "Export Excel";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // lblnooftrade
            // 
            this.lblnooftrade.AutoSize = true;
            this.lblnooftrade.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnooftrade.Location = new System.Drawing.Point(151, 6);
            this.lblnooftrade.Name = "lblnooftrade";
            this.lblnooftrade.Size = new System.Drawing.Size(105, 15);
            this.lblnooftrade.TabIndex = 3;
            this.lblnooftrade.Text = "No. of Trade  : ";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.N_V);
            this.panel1.Controls.Add(this.NV);
            this.panel1.Controls.Add(this.lbln_v);
            this.panel1.Controls.Add(this.lbls_v);
            this.panel1.Controls.Add(this.lbls_q);
            this.panel1.Controls.Add(this.lblb_V);
            this.panel1.Controls.Add(this.lblb_q);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblsprice);
            this.panel1.Controls.Add(this.lblsqty);
            this.panel1.Controls.Add(this.lblbprice);
            this.panel1.Controls.Add(this.lblBQty);
            this.panel1.Location = new System.Drawing.Point(0, 245);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(752, 27);
            this.panel1.TabIndex = 4;
            // 
            // lbln_v
            // 
            this.lbln_v.AutoSize = true;
            this.lbln_v.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbln_v.Location = new System.Drawing.Point(542, 9);
            this.lbln_v.Name = "lbln_v";
            this.lbln_v.Size = new System.Drawing.Size(13, 13);
            this.lbln_v.TabIndex = 9;
            this.lbln_v.Text = "0";
            // 
            // lbls_v
            // 
            this.lbls_v.AutoSize = true;
            this.lbls_v.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbls_v.Location = new System.Drawing.Point(430, 9);
            this.lbls_v.Name = "lbls_v";
            this.lbls_v.Size = new System.Drawing.Size(13, 13);
            this.lbls_v.TabIndex = 8;
            this.lbls_v.Text = "0";
            // 
            // lbls_q
            // 
            this.lbls_q.AutoSize = true;
            this.lbls_q.Location = new System.Drawing.Point(312, 9);
            this.lbls_q.Name = "lbls_q";
            this.lbls_q.Size = new System.Drawing.Size(13, 13);
            this.lbls_q.TabIndex = 7;
            this.lbls_q.Text = "0";
            // 
            // lblb_V
            // 
            this.lblb_V.AutoSize = true;
            this.lblb_V.Location = new System.Drawing.Point(167, 9);
            this.lblb_V.Name = "lblb_V";
            this.lblb_V.Size = new System.Drawing.Size(13, 13);
            this.lblb_V.TabIndex = 6;
            this.lblb_V.Text = "0";
            // 
            // lblb_q
            // 
            this.lblb_q.AutoSize = true;
            this.lblb_q.Location = new System.Drawing.Point(40, 9);
            this.lblb_q.Name = "lblb_q";
            this.lblb_q.Size = new System.Drawing.Size(13, 13);
            this.lblb_q.TabIndex = 5;
            this.lblb_q.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(514, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "NQ";
            // 
            // lblsprice
            // 
            this.lblsprice.AutoSize = true;
            this.lblsprice.Location = new System.Drawing.Point(403, 9);
            this.lblsprice.Name = "lblsprice";
            this.lblsprice.Size = new System.Drawing.Size(21, 13);
            this.lblsprice.TabIndex = 3;
            this.lblsprice.Text = "SV";
            // 
            // lblsqty
            // 
            this.lblsqty.AutoSize = true;
            this.lblsqty.Location = new System.Drawing.Point(284, 9);
            this.lblsqty.Name = "lblsqty";
            this.lblsqty.Size = new System.Drawing.Size(22, 13);
            this.lblsqty.TabIndex = 2;
            this.lblsqty.Text = "SQ";
            // 
            // lblbprice
            // 
            this.lblbprice.AutoSize = true;
            this.lblbprice.Location = new System.Drawing.Point(140, 9);
            this.lblbprice.Name = "lblbprice";
            this.lblbprice.Size = new System.Drawing.Size(21, 13);
            this.lblbprice.TabIndex = 1;
            this.lblbprice.Text = "BV";
            // 
            // lblBQty
            // 
            this.lblBQty.AutoSize = true;
            this.lblBQty.Location = new System.Drawing.Point(12, 9);
            this.lblBQty.Name = "lblBQty";
            this.lblBQty.Size = new System.Drawing.Size(22, 13);
            this.lblBQty.TabIndex = 0;
            this.lblBQty.Text = "BQ";
            // 
            // NV
            // 
            this.NV.AutoSize = true;
            this.NV.Location = new System.Drawing.Point(572, 9);
            this.NV.Name = "NV";
            this.NV.Size = new System.Drawing.Size(22, 13);
            this.NV.TabIndex = 10;
            this.NV.Text = "NV";
            // 
            // N_V
            // 
            this.N_V.AutoSize = true;
            this.N_V.Location = new System.Drawing.Point(625, 9);
            this.N_V.Name = "N_V";
            this.N_V.Size = new System.Drawing.Size(0, 13);
            this.N_V.TabIndex = 11;
            // 
            // frmTradeBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 276);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblnooftrade);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.DGV);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::Client.Properties.Settings.Default, "Window_LocationTBook", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Location = global::Client.Properties.Settings.Default.Window_LocationTBook;
            this.Name = "frmTradeBook";
            this.ShowIcon = false;
            this.Text = "TRADE BOOK";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTradeBook_FormClosing);
            this.Load += new System.EventHandler(this.frmTradeBook_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        public System.Windows.Forms.Label lblnooftrade;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblsprice;
        public System.Windows.Forms.Label lblsqty;
        public System.Windows.Forms.Label lblbprice;
        public System.Windows.Forms.Label lblBQty;
        public System.Windows.Forms.Label lbln_v;
        public System.Windows.Forms.Label lbls_v;
        public System.Windows.Forms.Label lbls_q;
        public System.Windows.Forms.Label lblb_V;
        public System.Windows.Forms.Label lblb_q;
        private System.Windows.Forms.Label NV;
        public System.Windows.Forms.Label N_V;

    }
}