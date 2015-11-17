namespace Client.Spread
{
    partial class frmSpreadMktWatch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSpreadMktWatch));
            this.dgvMktWatch = new CustomControls.AtsGrid.AtsDataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cmbInstType = new System.Windows.Forms.ToolStripComboBox();
            this.cmbSymbol = new System.Windows.Forms.ToolStripComboBox();
            this.cmbOptionType = new System.Windows.Forms.ToolStripComboBox();
            this.cmbStrikePrice = new System.Windows.Forms.ToolStripComboBox();
            this.cmbExpiry = new System.Windows.Forms.ToolStripComboBox();
            this.cmbSymDiscription = new System.Windows.Forms.ToolStripComboBox();
            this.btnsaveMktwatch = new System.Windows.Forms.ToolStripButton();
            this.btnLoadMktWatch = new System.Windows.Forms.ToolStripButton();
            this.btnprofile = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMktWatch)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMktWatch
            // 
            this.dgvMktWatch.AllowUserToAddRows = false;
            this.dgvMktWatch.AllowUserToDeleteRows = false;
            this.dgvMktWatch.AllowUserToOrderColumns = true;
            this.dgvMktWatch.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvMktWatch.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMktWatch.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgvMktWatch.BindSource = null;
            this.dgvMktWatch.BindSourceView = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMktWatch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMktWatch.CurGroupColIdx = -1;
            this.dgvMktWatch.CurMouseColIdx = 0;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.InfoText;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Coral;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMktWatch.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMktWatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMktWatch.EnableHeadersVisualStyles = false;
            this.dgvMktWatch.Location = new System.Drawing.Point(0, 25);
            this.dgvMktWatch.Name = "dgvMktWatch";
            this.dgvMktWatch.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMktWatch.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvMktWatch.RowHeadersVisible = false;
            this.dgvMktWatch.RowHeadersWidth = 11;
            this.dgvMktWatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMktWatch.SettingPath = "";
            this.dgvMktWatch.Size = new System.Drawing.Size(1028, 337);
            this.dgvMktWatch.TabIndex = 1;
            this.dgvMktWatch.UniqueName = "";
            this.dgvMktWatch.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMktWatch_CellContentClick);
            this.dgvMktWatch.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvMktWatch_ColumnHeaderMouseClick_1);
            this.dgvMktWatch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvMktWatch_KeyDown);
            this.dgvMktWatch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvMktWatch_KeyPress);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbInstType,
            this.cmbSymbol,
            this.cmbOptionType,
            this.cmbStrikePrice,
            this.cmbExpiry,
            this.cmbSymDiscription,
            this.btnsaveMktwatch,
            this.btnLoadMktWatch,
            this.btnprofile});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1028, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cmbInstType
            // 
            this.cmbInstType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInstType.Name = "cmbInstType";
            this.cmbInstType.Size = new System.Drawing.Size(75, 25);
            this.cmbInstType.Sorted = true;
            this.cmbInstType.ToolTipText = "Instrument Type";
            this.cmbInstType.SelectedIndexChanged += new System.EventHandler(this.cmbInstType_SelectedIndexChanged);
            // 
            // cmbSymbol
            // 
            this.cmbSymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSymbol.Name = "cmbSymbol";
            this.cmbSymbol.Size = new System.Drawing.Size(121, 25);
            this.cmbSymbol.Sorted = true;
            this.cmbSymbol.ToolTipText = "Symbol";
            this.cmbSymbol.SelectedIndexChanged += new System.EventHandler(this.cmbSymbol_SelectedIndexChanged);
            // 
            // cmbOptionType
            // 
            this.cmbOptionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOptionType.Name = "cmbOptionType";
            this.cmbOptionType.Size = new System.Drawing.Size(75, 25);
            this.cmbOptionType.ToolTipText = "CE/PE";
            this.cmbOptionType.SelectedIndexChanged += new System.EventHandler(this.cmbOptionType_SelectedIndexChanged);
            // 
            // cmbStrikePrice
            // 
            this.cmbStrikePrice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStrikePrice.Name = "cmbStrikePrice";
            this.cmbStrikePrice.Size = new System.Drawing.Size(75, 25);
            this.cmbStrikePrice.ToolTipText = "Strike Price";
            this.cmbStrikePrice.SelectedIndexChanged += new System.EventHandler(this.cmbStrikePrice_SelectedIndexChanged);
            // 
            // cmbExpiry
            // 
            this.cmbExpiry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExpiry.Name = "cmbExpiry";
            this.cmbExpiry.Size = new System.Drawing.Size(125, 25);
            this.cmbExpiry.ToolTipText = "Combined Expiry";
            this.cmbExpiry.SelectedIndexChanged += new System.EventHandler(this.cmbExpiry_SelectedIndexChanged);
            // 
            // cmbSymDiscription
            // 
            this.cmbSymDiscription.Name = "cmbSymDiscription";
            this.cmbSymDiscription.Size = new System.Drawing.Size(170, 25);
            this.cmbSymDiscription.ToolTipText = "Combined Symbol";
            this.cmbSymDiscription.Enter += new System.EventHandler(this.cmbSymDiscription_Enter);
            // 
            // btnsaveMktwatch
            // 
            this.btnsaveMktwatch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnsaveMktwatch.Image = ((System.Drawing.Image)(resources.GetObject("btnsaveMktwatch.Image")));
            this.btnsaveMktwatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnsaveMktwatch.Name = "btnsaveMktwatch";
            this.btnsaveMktwatch.Size = new System.Drawing.Size(23, 22);
            this.btnsaveMktwatch.Text = "Save Market Watch";
            this.btnsaveMktwatch.ToolTipText = "Save Market Watch";
            this.btnsaveMktwatch.Click += new System.EventHandler(this.btnsaveMktwatch_Click);
            // 
            // btnLoadMktWatch
            // 
            this.btnLoadMktWatch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLoadMktWatch.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadMktWatch.Image")));
            this.btnLoadMktWatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadMktWatch.Name = "btnLoadMktWatch";
            this.btnLoadMktWatch.Size = new System.Drawing.Size(23, 22);
            this.btnLoadMktWatch.Text = "Load Market Watch";
            this.btnLoadMktWatch.Click += new System.EventHandler(this.btnLoadMktWatch_Click);
            // 
            // btnprofile
            // 
            this.btnprofile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnprofile.Image = ((System.Drawing.Image)(resources.GetObject("btnprofile.Image")));
            this.btnprofile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnprofile.Name = "btnprofile";
            this.btnprofile.Size = new System.Drawing.Size(23, 22);
            this.btnprofile.Text = "Create/Load Profile";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(789, -1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Visible = false;
            // 
            // frmSpreadMktWatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 362);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dgvMktWatch);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmSpreadMktWatch";
            this.Text = "Spread Market Watch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSpreadMktWatch_FormClosing);
            this.Load += new System.EventHandler(this.frmSpreadMktWatch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMktWatch)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public CustomControls.AtsGrid.AtsDataGridView dgvMktWatch;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox cmbInstType;
        private System.Windows.Forms.ToolStripComboBox cmbSymbol;
        private System.Windows.Forms.ToolStripComboBox cmbOptionType;
        private System.Windows.Forms.ToolStripComboBox cmbStrikePrice;
        private System.Windows.Forms.ToolStripComboBox cmbExpiry;
        private System.Windows.Forms.ToolStripButton btnsaveMktwatch;
        private System.Windows.Forms.ToolStripButton btnLoadMktWatch;
        private System.Windows.Forms.ToolStripButton btnprofile;
        private System.Windows.Forms.ToolStripComboBox cmbSymDiscription;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox textBox1;
    }
}