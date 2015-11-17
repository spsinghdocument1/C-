namespace Client
{
    partial class fofospreadwatch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fofospreadwatch));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.btnsaveMktwatch = new System.Windows.Forms.ToolStripButton();
            this.btnLoadMktWatch = new System.Windows.Forms.ToolStripButton();
            this.btnprofile = new System.Windows.Forms.ToolStripButton();
            this.btnStartAll = new System.Windows.Forms.ToolStripButton();
            this.btnStopAll = new System.Windows.Forms.ToolStripButton();
            this.DGV1 = new System.Windows.Forms.DataGridView();
            this.Enable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.txt = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.btnsaveMktwatch,
            this.btnLoadMktWatch,
            this.btnprofile,
            this.btnStartAll,
            this.btnStopAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(967, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabel1.Text = "Add Token";
            this.toolStripLabel1.Click += new System.EventHandler(this.toolStripLabel1_Click);
            // 
            // btnsaveMktwatch
            // 
            this.btnsaveMktwatch.Image = ((System.Drawing.Image)(resources.GetObject("btnsaveMktwatch.Image")));
            this.btnsaveMktwatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnsaveMktwatch.Name = "btnsaveMktwatch";
            this.btnsaveMktwatch.Size = new System.Drawing.Size(128, 22);
            this.btnsaveMktwatch.Text = "Save Market Watch";
            this.btnsaveMktwatch.ToolTipText = "Save Market Watch";
            this.btnsaveMktwatch.Click += new System.EventHandler(this.btnsaveMktwatch_Click);
            // 
            // btnLoadMktWatch
            // 
            this.btnLoadMktWatch.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadMktWatch.Image")));
            this.btnLoadMktWatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadMktWatch.Name = "btnLoadMktWatch";
            this.btnLoadMktWatch.Size = new System.Drawing.Size(130, 22);
            this.btnLoadMktWatch.Text = "Load Market Watch";
            this.btnLoadMktWatch.Click += new System.EventHandler(this.btnLoadMktWatch_Click);
            // 
            // btnprofile
            // 
            this.btnprofile.Image = ((System.Drawing.Image)(resources.GetObject("btnprofile.Image")));
            this.btnprofile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnprofile.Name = "btnprofile";
            this.btnprofile.Size = new System.Drawing.Size(129, 22);
            this.btnprofile.Text = "Create/Load Profile";
            this.btnprofile.Click += new System.EventHandler(this.btnprofile_Click);
            // 
            // btnStartAll
            // 
            this.btnStartAll.Image = ((System.Drawing.Image)(resources.GetObject("btnStartAll.Image")));
            this.btnStartAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStartAll.Name = "btnStartAll";
            this.btnStartAll.Size = new System.Drawing.Size(68, 22);
            this.btnStartAll.Text = "Start All";
            this.btnStartAll.Click += new System.EventHandler(this.btnStartAll_Click);
            // 
            // btnStopAll
            // 
            this.btnStopAll.Image = ((System.Drawing.Image)(resources.GetObject("btnStopAll.Image")));
            this.btnStopAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStopAll.Name = "btnStopAll";
            this.btnStopAll.Size = new System.Drawing.Size(68, 22);
            this.btnStopAll.Text = "Stop All";
            this.btnStopAll.Click += new System.EventHandler(this.btnStopAll_Click);
            // 
            // DGV1
            // 
            this.DGV1.AllowUserToAddRows = false;
            this.DGV1.AllowUserToDeleteRows = false;
            this.DGV1.AllowUserToResizeRows = false;
            this.DGV1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV1.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DGV1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Enable});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV1.DefaultCellStyle = dataGridViewCellStyle2;
            this.DGV1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGV1.Location = new System.Drawing.Point(0, 25);
            this.DGV1.MultiSelect = false;
            this.DGV1.Name = "DGV1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DGV1.RowHeadersVisible = false;
            this.DGV1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV1.Size = new System.Drawing.Size(967, 344);
            this.DGV1.TabIndex = 5;
            this.DGV1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV1_CellClick);
            this.DGV1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV1_CellContentClick);
            this.DGV1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV1_CellEnter);
            this.DGV1.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV1_CellLeave);
            this.DGV1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV1_CellValueChanged);
            this.DGV1.CurrentCellDirtyStateChanged += new System.EventHandler(this.DGV1_CurrentCellDirtyStateChanged_1);
            this.DGV1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV1_DataError);
            this.DGV1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DGV1_Scroll);
           this.DGV1.SelectionChanged += new System.EventHandler(this.DGV1_SelectionChanged);
            this.DGV1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DGV1_KeyDown);
            // 
            // Enable
            // 
            this.Enable.HeaderText = "Enable";
            this.Enable.Name = "Enable";
            // 
            // txt
            // 
            this.txt.Location = new System.Drawing.Point(855, 0);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(100, 20);
            this.txt.TabIndex = 6;
            this.txt.Visible = false;
            this.txt.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txt_MouseClick);
            this.txt.TextChanged += new System.EventHandler(this.txt_TextChanged);
            this.txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.txt.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // fofospreadwatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 369);
            this.Controls.Add(this.txt);
            this.Controls.Add(this.DGV1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "fofospreadwatch";
            this.Text = "Spread Watch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Fo_Fo_mktwatch_FormClosing);
            this.Load += new System.EventHandler(this.Fo_Fo_mktwatch_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton btnsaveMktwatch;
        private System.Windows.Forms.ToolStripButton btnLoadMktWatch;
        private System.Windows.Forms.ToolStripButton btnprofile;
        private System.Windows.Forms.DataGridView DGV1;
        private System.Windows.Forms.ToolStripButton btnStartAll;
        private System.Windows.Forms.ToolStripButton btnStopAll;
        private System.Windows.Forms.TextBox txt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Enable;
    }
}