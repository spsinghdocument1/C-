namespace Client
{
    partial class frmMktWatch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMktWatch));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.combo_Exchange = new System.Windows.Forms.ToolStripComboBox();
            this.comboB_OrderType = new System.Windows.Forms.ToolStripComboBox();
            this.comboBInstType = new System.Windows.Forms.ToolStripComboBox();
            this.comboB_Symbol = new System.Windows.Forms.ToolStripComboBox();
            this.combo_Exoiry = new System.Windows.Forms.ToolStripComboBox();
            this.combo_OptionType = new System.Windows.Forms.ToolStripComboBox();
            this.combo_StrikePrice = new System.Windows.Forms.ToolStripComboBox();
            this.btnsaveMktwatch = new System.Windows.Forms.ToolStripButton();
            this.btnLoadMktWatch = new System.Windows.Forms.ToolStripButton();
            this.btnprofile = new System.Windows.Forms.ToolStripButton();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.combo_Exchange,
            this.comboB_OrderType,
            this.comboBInstType,
            this.comboB_Symbol,
            this.combo_Exoiry,
            this.combo_OptionType,
            this.combo_StrikePrice,
            this.btnsaveMktwatch,
            this.btnLoadMktWatch,
            this.btnprofile});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1102, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // combo_Exchange
            // 
            this.combo_Exchange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Exchange.Name = "combo_Exchange";
            this.combo_Exchange.Size = new System.Drawing.Size(121, 25);
            this.combo_Exchange.Sorted = true;
            this.combo_Exchange.ToolTipText = "Instrument Name";
            this.combo_Exchange.SelectedIndexChanged += new System.EventHandler(this.combo_Exchange_SelectedIndexChanged);
            // 
            // comboB_OrderType
            // 
            this.comboB_OrderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboB_OrderType.Name = "comboB_OrderType";
            this.comboB_OrderType.Size = new System.Drawing.Size(121, 25);
            this.comboB_OrderType.Sorted = true;
            this.comboB_OrderType.ToolTipText = "Symbol";
            this.comboB_OrderType.SelectedIndexChanged += new System.EventHandler(this.comboB_OrderType_SelectedIndexChanged);
            // 
            // comboBInstType
            // 
            this.comboBInstType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBInstType.Name = "comboBInstType";
            this.comboBInstType.Size = new System.Drawing.Size(121, 25);
            this.comboBInstType.Sorted = true;
            this.comboBInstType.ToolTipText = "Option Type";
            this.comboBInstType.SelectedIndexChanged += new System.EventHandler(this.comboBInstType_SelectedIndexChanged);
            // 
            // comboB_Symbol
            // 
            this.comboB_Symbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboB_Symbol.Name = "comboB_Symbol";
            this.comboB_Symbol.Size = new System.Drawing.Size(121, 25);
            this.comboB_Symbol.Sorted = true;
            this.comboB_Symbol.ToolTipText = "Strike Price";
            this.comboB_Symbol.SelectedIndexChanged += new System.EventHandler(this.comboB_Symbol_SelectedIndexChanged);
            this.comboB_Symbol.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboB_Symbol_KeyDown);
            // 
            // combo_Exoiry
            // 
            this.combo_Exoiry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Exoiry.Name = "combo_Exoiry";
            this.combo_Exoiry.Size = new System.Drawing.Size(121, 25);
            this.combo_Exoiry.ToolTipText = "Seriex/Expiry Date";
            this.combo_Exoiry.SelectedIndexChanged += new System.EventHandler(this.combo_Exoiry_SelectedIndexChanged);
            this.combo_Exoiry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.combo_Exoiry_KeyDown);
            this.combo_Exoiry.Click += new System.EventHandler(this.combo_Exoiry_Click);
            // 
            // combo_OptionType
            // 
            this.combo_OptionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_OptionType.Name = "combo_OptionType";
            this.combo_OptionType.Size = new System.Drawing.Size(121, 25);
            this.combo_OptionType.SelectedIndexChanged += new System.EventHandler(this.combo_OptionType_SelectedIndexChanged);
            // 
            // combo_StrikePrice
            // 
            this.combo_StrikePrice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_StrikePrice.Name = "combo_StrikePrice";
            this.combo_StrikePrice.Size = new System.Drawing.Size(121, 25);
            this.combo_StrikePrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.combo_Exoiry_KeyDown);
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
            this.btnprofile.Click += new System.EventHandler(this.btnprofile_Click);
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToOrderColumns = true;
            this.DGV.AllowUserToResizeRows = false;
            this.DGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DGV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV.DefaultCellStyle = dataGridViewCellStyle5;
            this.DGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DGV.Location = new System.Drawing.Point(0, 28);
            this.DGV.Name = "DGV";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.DGV.RowHeadersVisible = false;
            this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV.Size = new System.Drawing.Size(1102, 341);
            this.DGV.TabIndex = 4;
            this.DGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellContentClick);
            this.DGV.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_ColumnHeaderMouseClick);
            this.DGV.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DGV_KeyDown);
            this.DGV.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DGV_KeyPress);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(990, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Visible = false;
            // 
            // frmMktWatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1102, 369);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.DGV);
            this.Name = "frmMktWatch";
            this.Text = "Market Watch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMktWatch_FormClosing);
            this.Load += new System.EventHandler(this.frmMktWatch_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox combo_Exchange;
        private System.Windows.Forms.ToolStripComboBox comboB_OrderType;
        private System.Windows.Forms.ToolStripComboBox comboBInstType;
        private System.Windows.Forms.ToolStripComboBox comboB_Symbol;
        private System.Windows.Forms.ToolStripComboBox combo_Exoiry;
        private System.Windows.Forms.ToolStripButton btnsaveMktwatch;
        private System.Windows.Forms.ToolStripButton btnLoadMktWatch;
        private System.Windows.Forms.ToolStripButton btnprofile;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.ToolStripComboBox combo_OptionType;
        private System.Windows.Forms.ToolStripComboBox combo_StrikePrice;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox textBox1;
    }
}