namespace Client
{
    partial class frmProfile
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbxPrimary = new System.Windows.Forms.ListBox();
            this.lbxSecondary = new System.Windows.Forms.ListBox();
            this.btnadd = new System.Windows.Forms.Button();
            this.btnremove = new System.Windows.Forms.Button();
            this.cmbprofile = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(338, 43);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(338, 43);
            this.label1.TabIndex = 0;
            this.label1.Text = "Create/Edit Market Profile";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(186, 343);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(105, 343);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 2;
            this.btnOkay.Text = "Save";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Profile Name";
            // 
            // lbxPrimary
            // 
            this.lbxPrimary.FormattingEnabled = true;
            this.lbxPrimary.Location = new System.Drawing.Point(19, 84);
            this.lbxPrimary.Name = "lbxPrimary";
            this.lbxPrimary.Size = new System.Drawing.Size(120, 251);
            this.lbxPrimary.TabIndex = 5;
            this.lbxPrimary.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbxPrimary_MouseDoubleClick);
            // 
            // lbxSecondary
            // 
            this.lbxSecondary.AllowDrop = true;
            this.lbxSecondary.FormattingEnabled = true;
            this.lbxSecondary.Location = new System.Drawing.Point(209, 84);
            this.lbxSecondary.Name = "lbxSecondary";
            this.lbxSecondary.Size = new System.Drawing.Size(120, 251);
            this.lbxSecondary.TabIndex = 6;
            this.lbxSecondary.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbxSecondary_MouseDoubleClick);
            // 
            // btnadd
            // 
            this.btnadd.Location = new System.Drawing.Point(142, 124);
            this.btnadd.Name = "btnadd";
            this.btnadd.Size = new System.Drawing.Size(65, 77);
            this.btnadd.TabIndex = 7;
            this.btnadd.Text = ">>";
            this.btnadd.UseVisualStyleBackColor = true;
            this.btnadd.Click += new System.EventHandler(this.btnadd_Click);
            // 
            // btnremove
            // 
            this.btnremove.Location = new System.Drawing.Point(142, 207);
            this.btnremove.Name = "btnremove";
            this.btnremove.Size = new System.Drawing.Size(65, 90);
            this.btnremove.TabIndex = 8;
            this.btnremove.Text = "<<";
            this.btnremove.UseVisualStyleBackColor = true;
            this.btnremove.Click += new System.EventHandler(this.btnremove_Click);
            // 
            // cmbprofile
            // 
            this.cmbprofile.FormattingEnabled = true;
            this.cmbprofile.Location = new System.Drawing.Point(92, 49);
            this.cmbprofile.Name = "cmbprofile";
            this.cmbprofile.Size = new System.Drawing.Size(237, 21);
            this.cmbprofile.TabIndex = 9;
            this.cmbprofile.SelectedIndexChanged += new System.EventHandler(this.cmbprofile_SelectedIndexChanged);
            // 
            // frmProfile
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(338, 365);
            this.ControlBox = false;
            this.Controls.Add(this.cmbprofile);
            this.Controls.Add(this.btnremove);
            this.Controls.Add(this.btnadd);
            this.Controls.Add(this.lbxSecondary);
            this.Controls.Add(this.lbxPrimary);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmProfile";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create/Edit Profile";
            this.Load += new System.EventHandler(this.frmProfile_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ListBox lbxPrimary;
        public System.Windows.Forms.ListBox lbxSecondary;
        private System.Windows.Forms.Button btnadd;
        private System.Windows.Forms.Button btnremove;
        private System.Windows.Forms.ComboBox cmbprofile;
    }
}