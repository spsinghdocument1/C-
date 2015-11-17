namespace Client
{
    partial class frmErrorLog
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
            this.tbelog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbelog
            // 
            this.tbelog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbelog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbelog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tbelog.Location = new System.Drawing.Point(0, 0);
            this.tbelog.Multiline = true;
            this.tbelog.Name = "tbelog";
            this.tbelog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbelog.Size = new System.Drawing.Size(1022, 285);
            this.tbelog.TabIndex = 0;
            this.tbelog.Text = "Error";
            // 
            // frmErrorLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 285);
            this.Controls.Add(this.tbelog);
            this.Name = "frmErrorLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmErrorLog_FormClosing);
            this.Load += new System.EventHandler(this.frmErrorLog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbelog;
    }
}