namespace Client.Spread
{
    partial class FrmSpdOrderBook
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
            this.dgvSpdOrderBook = new CustomControls.AtsGrid.AtsDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpdOrderBook)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSpdOrderBook
            // 
            this.dgvSpdOrderBook.AllowUserToAddRows = false;
            this.dgvSpdOrderBook.AllowUserToDeleteRows = false;
            this.dgvSpdOrderBook.AllowUserToOrderColumns = true;
            this.dgvSpdOrderBook.AllowUserToResizeRows = false;
            this.dgvSpdOrderBook.BackgroundColor = System.Drawing.Color.White;
            this.dgvSpdOrderBook.BindSource = null;
            this.dgvSpdOrderBook.BindSourceView = null;
            this.dgvSpdOrderBook.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSpdOrderBook.CurGroupColIdx = -1;
            this.dgvSpdOrderBook.CurMouseColIdx = 0;
            this.dgvSpdOrderBook.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSpdOrderBook.EnableHeadersVisualStyles = false;
            this.dgvSpdOrderBook.Location = new System.Drawing.Point(0, 0);
            this.dgvSpdOrderBook.Name = "dgvSpdOrderBook";
            this.dgvSpdOrderBook.ReadOnly = true;
            this.dgvSpdOrderBook.RowHeadersVisible = false;
            this.dgvSpdOrderBook.RowHeadersWidth = 11;
            this.dgvSpdOrderBook.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSpdOrderBook.SettingPath = "";
            this.dgvSpdOrderBook.Size = new System.Drawing.Size(719, 368);
            this.dgvSpdOrderBook.TabIndex = 0;
            this.dgvSpdOrderBook.UniqueName = "";
            this.dgvSpdOrderBook.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvSpdOrderBook_DataError_1);
            // 
            // FrmSpdOrderBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(719, 368);
            this.Controls.Add(this.dgvSpdOrderBook);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "FrmSpdOrderBook";
            this.Text = "Spread Order Book";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSpdOrderBook_FormClosing);
            this.Load += new System.EventHandler(this.FrmOrderBook_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpdOrderBook)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public CustomControls.AtsGrid.AtsDataGridView dgvSpdOrderBook;

    }
}