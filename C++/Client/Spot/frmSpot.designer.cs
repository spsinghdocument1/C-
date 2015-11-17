namespace Client.Spot
{
    partial class frmSpot
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMktWatch = new CustomControls.AtsGrid.AtsDataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMktWatch)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMktWatch
            // 
            this.dgvMktWatch.AllowUserToAddRows = false;
            this.dgvMktWatch.AllowUserToDeleteRows = false;
            this.dgvMktWatch.AllowUserToOrderColumns = true;
            this.dgvMktWatch.AllowUserToResizeRows = false;
            this.dgvMktWatch.BackgroundColor = System.Drawing.Color.White;
            this.dgvMktWatch.BindSource = null;
            this.dgvMktWatch.BindSourceView = null;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMktWatch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMktWatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMktWatch.CurGroupColIdx = -1;
            this.dgvMktWatch.CurMouseColIdx = 0;
            this.dgvMktWatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMktWatch.EnableHeadersVisualStyles = false;
            this.dgvMktWatch.Location = new System.Drawing.Point(0, 0);
            this.dgvMktWatch.Name = "dgvMktWatch";
            this.dgvMktWatch.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMktWatch.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMktWatch.RowHeadersVisible = false;
            this.dgvMktWatch.RowHeadersWidth = 11;
            this.dgvMktWatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMktWatch.SettingPath = "";
            this.dgvMktWatch.Size = new System.Drawing.Size(456, 169);
            this.dgvMktWatch.TabIndex = 5;
            this.dgvMktWatch.UniqueName = "";
            this.dgvMktWatch.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMktWatch_CellContentClick);
            this.dgvMktWatch.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvMktWatch_DataError);
            this.dgvMktWatch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvMktWatch_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRowToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // addRowToolStripMenuItem
            // 
            this.addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            this.addRowToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.addRowToolStripMenuItem.Text = "Add_Row";
            this.addRowToolStripMenuItem.Click += new System.EventHandler(this.addRowToolStripMenuItem_Click);
            // 
            // frmSpot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 169);
            this.Controls.Add(this.dgvMktWatch);
            this.Name = "frmSpot";
            this.Text = "frmSpot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSpot_FormClosing);
            this.Load += new System.EventHandler(this.frmSpot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMktWatch)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public CustomControls.AtsGrid.AtsDataGridView dgvMktWatch;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addRowToolStripMenuItem;
    }
}