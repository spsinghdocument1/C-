namespace OrderBook.GUI
{
    partial class FrmOrderBook
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
            this.dgvOrderBook = new CustomControls.AtsGrid.AtsDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderBook)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOrderBook
            // 
            this.dgvOrderBook.AllowUserToAddRows = false;
            this.dgvOrderBook.AllowUserToDeleteRows = false;
            this.dgvOrderBook.AllowUserToOrderColumns = true;
            this.dgvOrderBook.AllowUserToResizeRows = false;
            this.dgvOrderBook.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrderBook.BindSource = null;
            this.dgvOrderBook.BindSourceView = null;
            this.dgvOrderBook.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderBook.CurGroupColIdx = -1;
            this.dgvOrderBook.CurMouseColIdx = 0;
            this.dgvOrderBook.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrderBook.EnableHeadersVisualStyles = false;
            this.dgvOrderBook.Location = new System.Drawing.Point(0, 0);
            this.dgvOrderBook.Name = "dgvOrderBook";
            this.dgvOrderBook.ReadOnly = true;
            this.dgvOrderBook.RowHeadersVisible = false;
            this.dgvOrderBook.RowHeadersWidth = 11;
            this.dgvOrderBook.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrderBook.SettingPath = "";
            this.dgvOrderBook.Size = new System.Drawing.Size(719, 368);
            this.dgvOrderBook.TabIndex = 0;
            this.dgvOrderBook.UniqueName = "";
            // 
            // FrmOrderBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(719, 368);
            this.Controls.Add(this.dgvOrderBook);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "FrmOrderBook";
            this.Text = "Order Book";
            this.Load += new System.EventHandler(this.FrmOrderBook_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderBook)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public CustomControls.AtsGrid.AtsDataGridView dgvOrderBook;

    }
}