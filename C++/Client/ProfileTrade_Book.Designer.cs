namespace Client
{
    partial class ProfileTrade_Book
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbprofile = new System.Windows.Forms.ComboBox();
            this.btnremove = new System.Windows.Forms.Button();
            this.btnadd = new System.Windows.Forms.Button();
            this.lbx_Secondary = new System.Windows.Forms.ListBox();
            this.lbx_Primary = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 43);
            this.label1.TabIndex = 0;
            this.label1.Text = "Create/Edit Trade Profile";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbprofile
            // 
            this.cmbprofile.FormattingEnabled = true;
            this.cmbprofile.Location = new System.Drawing.Point(92, 45);
            this.cmbprofile.Name = "cmbprofile";
            this.cmbprofile.Size = new System.Drawing.Size(237, 21);
            this.cmbprofile.TabIndex = 18;
            this.cmbprofile.SelectedIndexChanged += new System.EventHandler(this.cmbprofile_SelectedIndexChanged);
            // 
            // btnremove
            // 
            this.btnremove.Location = new System.Drawing.Point(142, 195);
            this.btnremove.Name = "btnremove";
            this.btnremove.Size = new System.Drawing.Size(65, 75);
            this.btnremove.TabIndex = 17;
            this.btnremove.Text = "<<";
            this.btnremove.UseVisualStyleBackColor = true;
            this.btnremove.Click += new System.EventHandler(this.btnremove_Click);
            // 
            // btnadd
            // 
            this.btnadd.Location = new System.Drawing.Point(142, 121);
            this.btnadd.Name = "btnadd";
            this.btnadd.Size = new System.Drawing.Size(65, 69);
            this.btnadd.TabIndex = 16;
            this.btnadd.Text = ">>";
            this.btnadd.UseVisualStyleBackColor = true;
            this.btnadd.Click += new System.EventHandler(this.btnadd_Click);
            // 
            // lbx_Secondary
            // 
            this.lbx_Secondary.AllowDrop = true;
            this.lbx_Secondary.FormattingEnabled = true;
            this.lbx_Secondary.Location = new System.Drawing.Point(209, 72);
            this.lbx_Secondary.Name = "lbx_Secondary";
            this.lbx_Secondary.Size = new System.Drawing.Size(120, 251);
            this.lbx_Secondary.TabIndex = 15;
            this.lbx_Secondary.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbx_Secondary_MouseDoubleClick);
            // 
            // lbx_Primary
            // 
            this.lbx_Primary.FormattingEnabled = true;
            this.lbx_Primary.Location = new System.Drawing.Point(19, 72);
            this.lbx_Primary.Name = "lbx_Primary";
            this.lbx_Primary.Size = new System.Drawing.Size(120, 251);
            this.lbx_Primary.TabIndex = 14;
            this.lbx_Primary.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbx_Primary_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Profile Name";
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(155, 328);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 12;
            this.btnOkay.Text = "Save";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(236, 328);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 43);
            this.panel1.TabIndex = 10;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(19, 330);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(121, 17);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "Defaut Profile Name";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ProfileTrade_Book
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 355);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cmbprofile);
            this.Controls.Add(this.btnremove);
            this.Controls.Add(this.btnadd);
            this.Controls.Add(this.lbx_Secondary);
            this.Controls.Add(this.lbx_Primary);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel1);
            this.Name = "ProfileTrade_Book";
            this.Text = "ProfileTrade_Book";
            this.Load += new System.EventHandler(this.ProfileTrade_Book_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbprofile;
        private System.Windows.Forms.Button btnremove;
        private System.Windows.Forms.Button btnadd;
        public System.Windows.Forms.ListBox lbx_Secondary;
        public System.Windows.Forms.ListBox lbx_Primary;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}