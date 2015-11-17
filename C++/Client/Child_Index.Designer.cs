namespace Client
{
    partial class Child_Index
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
            this.Index_DGV = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showSpotIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.Index_DGV)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Index_DGV
            // 
            this.Index_DGV.AllowUserToAddRows = false;
            this.Index_DGV.AllowUserToDeleteRows = false;
            this.Index_DGV.AllowUserToResizeRows = false;
            this.Index_DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Index_DGV.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Index_DGV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.Index_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Index_DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Index_DGV.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.Index_DGV.Location = new System.Drawing.Point(0, 0);
            this.Index_DGV.MultiSelect = false;
            this.Index_DGV.Name = "Index_DGV";
            this.Index_DGV.RowHeadersVisible = false;
            this.Index_DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Index_DGV.Size = new System.Drawing.Size(463, 103);
            this.Index_DGV.TabIndex = 0;
            this.Index_DGV.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Index_DGV_DataError);
            this.Index_DGV.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Index_DGV_RowsAdded);
            this.Index_DGV.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Index_DGV_KeyDown);
            this.Index_DGV.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Index_DGV_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showSpotIndexToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(166, 26);
            // 
            // showSpotIndexToolStripMenuItem
            // 
            this.showSpotIndexToolStripMenuItem.Name = "showSpotIndexToolStripMenuItem";
            this.showSpotIndexToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.showSpotIndexToolStripMenuItem.Text = "Show_Spot_Index";
            this.showSpotIndexToolStripMenuItem.Click += new System.EventHandler(this.showSpotIndexToolStripMenuItem_Click);
            // 
            // Child_Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 103);
            this.Controls.Add(this.Index_DGV);
            this.Name = "Child_Index";
            this.Text = "Index";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Child_Index_FormClosing);
            this.Load += new System.EventHandler(this.Child_Index_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Index_DGV)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showSpotIndexToolStripMenuItem;
        public System.Windows.Forms.DataGridView Index_DGV;
    }
}