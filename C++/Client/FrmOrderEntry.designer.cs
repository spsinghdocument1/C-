namespace Client
{
    partial class FrmOrderEntry
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
            this.lblOrderMsg = new System.Windows.Forms.Label();
            this.lblMarketProtection = new System.Windows.Forms.Label();
            this.txtMarketProtection = new CustomControls.AtsTextBox();
            this.cmbAccount = new System.Windows.Forms.ComboBox();
            this.lblTriggerPrice = new System.Windows.Forms.Label();
            this.cmbValidity = new System.Windows.Forms.ComboBox();
            this.lblQty = new System.Windows.Forms.Label();
            this.dtpGTD = new System.Windows.Forms.DateTimePicker();
            this.tbpnlOrderPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblOrderPrice = new System.Windows.Forms.Label();
            this.lblQtyDisClosed = new System.Windows.Forms.Label();
            this.txtOrderPrice = new CustomControls.CustomTextBox();
            this.txtQty = new CustomControls.CustomTextBox();
            this.txtTriggerPrice = new CustomControls.CustomTextBox();
            this.txtQtyDisclosed = new CustomControls.CustomTextBox();
            this.txtRemarks = new CustomControls.AtsTextBox();
            this.cmbBookType = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnModify = new System.Windows.Forms.Button();
            this.tbpnlOrderPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOrderMsg
            // 
            this.lblOrderMsg.AutoSize = true;
            this.lblOrderMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOrderMsg.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderMsg.ForeColor = System.Drawing.Color.White;
            this.lblOrderMsg.Location = new System.Drawing.Point(3, 0);
            this.lblOrderMsg.Name = "lblOrderMsg";
            this.lblOrderMsg.Size = new System.Drawing.Size(1183, 18);
            this.lblOrderMsg.TabIndex = 10;
            this.lblOrderMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMarketProtection
            // 
            this.lblMarketProtection.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMarketProtection.AutoSize = true;
            this.lblMarketProtection.Location = new System.Drawing.Point(801, 9);
            this.lblMarketProtection.Margin = new System.Windows.Forms.Padding(0);
            this.lblMarketProtection.Name = "lblMarketProtection";
            this.lblMarketProtection.Size = new System.Drawing.Size(28, 13);
            this.lblMarketProtection.TabIndex = 12;
            this.lblMarketProtection.Text = "MP:";
            // 
            // txtMarketProtection
            // 
            this.txtMarketProtection.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMarketProtection.AutoMinMax = false;
            this.txtMarketProtection.AutoUpDown = false;
            this.txtMarketProtection.BackColor = System.Drawing.Color.White;
            this.txtMarketProtection.CustomFormat = "";
            this.txtMarketProtection.FocusColor = System.Drawing.Color.LightYellow;
            this.txtMarketProtection.FocusForeColor = System.Drawing.Color.Black;
            this.txtMarketProtection.ForeColor = System.Drawing.Color.Black;
            this.txtMarketProtection.LeaveColor = System.Drawing.Color.White;
            this.txtMarketProtection.LeaveForeColor = System.Drawing.Color.Black;
            this.txtMarketProtection.Location = new System.Drawing.Point(829, 5);
            this.txtMarketProtection.Margin = new System.Windows.Forms.Padding(0);
            this.txtMarketProtection.Masked = CustomControls.Mask.None;
            this.txtMarketProtection.MaxValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtMarketProtection.MinValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtMarketProtection.Name = "txtMarketProtection";
            this.txtMarketProtection.PriceTick = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMarketProtection.Size = new System.Drawing.Size(79, 21);
            this.txtMarketProtection.TabIndex = 8;
            this.txtMarketProtection.WarningColor = System.Drawing.Color.IndianRed;
            this.txtMarketProtection.WatermarkText = "MP";
            // 
            // cmbAccount
            // 
            this.cmbAccount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccount.FormattingEnabled = true;
            this.cmbAccount.Location = new System.Drawing.Point(586, 5);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new System.Drawing.Size(97, 21);
            this.cmbAccount.TabIndex = 6;
            // 
            // lblTriggerPrice
            // 
            this.lblTriggerPrice.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTriggerPrice.AutoSize = true;
            this.lblTriggerPrice.Location = new System.Drawing.Point(482, 9);
            this.lblTriggerPrice.Margin = new System.Windows.Forms.Padding(0);
            this.lblTriggerPrice.Name = "lblTriggerPrice";
            this.lblTriggerPrice.Size = new System.Drawing.Size(26, 13);
            this.lblTriggerPrice.TabIndex = 8;
            this.lblTriggerPrice.Text = "TP:";
            // 
            // cmbValidity
            // 
            this.cmbValidity.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbValidity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbValidity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbValidity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValidity.FormattingEnabled = true;
            this.cmbValidity.Location = new System.Drawing.Point(407, 5);
            this.cmbValidity.Name = "cmbValidity";
            this.cmbValidity.Size = new System.Drawing.Size(69, 21);
            this.cmbValidity.TabIndex = 4;
            this.cmbValidity.SelectedIndexChanged += new System.EventHandler(this.cmbValidity_SelectedIndexChanged);
            // 
            // lblQty
            // 
            this.lblQty.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblQty.AutoSize = true;
            this.lblQty.Location = new System.Drawing.Point(90, 9);
            this.lblQty.Margin = new System.Windows.Forms.Padding(0);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(21, 13);
            this.lblQty.TabIndex = 0;
            this.lblQty.Text = "Q:";
            // 
            // dtpGTD
            // 
            this.dtpGTD.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpGTD.CustomFormat = "ddMMMyyyy";
            this.dtpGTD.Enabled = false;
            this.dtpGTD.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGTD.Location = new System.Drawing.Point(686, 5);
            this.dtpGTD.Margin = new System.Windows.Forms.Padding(0);
            this.dtpGTD.Name = "dtpGTD";
            this.dtpGTD.Size = new System.Drawing.Size(107, 21);
            this.dtpGTD.TabIndex = 7;
            // 
            // tbpnlOrderPanel
            // 
            this.tbpnlOrderPanel.ColumnCount = 16;
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 163F));
            this.tbpnlOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbpnlOrderPanel.Controls.Add(this.lblMarketProtection, 12, 0);
            this.tbpnlOrderPanel.Controls.Add(this.txtMarketProtection, 13, 0);
            this.tbpnlOrderPanel.Controls.Add(this.cmbAccount, 10, 0);
            this.tbpnlOrderPanel.Controls.Add(this.lblTriggerPrice, 8, 0);
            this.tbpnlOrderPanel.Controls.Add(this.cmbValidity, 7, 0);
            this.tbpnlOrderPanel.Controls.Add(this.lblQty, 0, 0);
            this.tbpnlOrderPanel.Controls.Add(this.btnSubmit, 15, 0);
            this.tbpnlOrderPanel.Controls.Add(this.lblOrderPrice, 2, 0);
            this.tbpnlOrderPanel.Controls.Add(this.lblQtyDisClosed, 4, 0);
            this.tbpnlOrderPanel.Controls.Add(this.dtpGTD, 11, 0);
            this.tbpnlOrderPanel.Controls.Add(this.txtOrderPrice, 3, 0);
            this.tbpnlOrderPanel.Controls.Add(this.txtQty, 1, 0);
            this.tbpnlOrderPanel.Controls.Add(this.txtTriggerPrice, 9, 0);
            this.tbpnlOrderPanel.Controls.Add(this.txtQtyDisclosed, 5, 0);
            this.tbpnlOrderPanel.Controls.Add(this.txtRemarks, 14, 0);
            this.tbpnlOrderPanel.Controls.Add(this.cmbBookType, 6, 0);
            this.tbpnlOrderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbpnlOrderPanel.Location = new System.Drawing.Point(0, 18);
            this.tbpnlOrderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tbpnlOrderPanel.Name = "tbpnlOrderPanel";
            this.tbpnlOrderPanel.RowCount = 1;
            this.tbpnlOrderPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tbpnlOrderPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tbpnlOrderPanel.Size = new System.Drawing.Size(1189, 32);
            this.tbpnlOrderPanel.TabIndex = 1;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(1077, 3);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(109, 26);
            this.btnSubmit.TabIndex = 10;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblOrderPrice
            // 
            this.lblOrderPrice.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOrderPrice.AutoSize = true;
            this.lblOrderPrice.Location = new System.Drawing.Point(160, 9);
            this.lblOrderPrice.Margin = new System.Windows.Forms.Padding(0);
            this.lblOrderPrice.Name = "lblOrderPrice";
            this.lblOrderPrice.Size = new System.Drawing.Size(28, 13);
            this.lblOrderPrice.TabIndex = 2;
            this.lblOrderPrice.Text = "OP:";
            // 
            // lblQtyDisClosed
            // 
            this.lblQtyDisClosed.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblQtyDisClosed.AutoSize = true;
            this.lblQtyDisClosed.Location = new System.Drawing.Point(261, 9);
            this.lblQtyDisClosed.Margin = new System.Windows.Forms.Padding(0);
            this.lblQtyDisClosed.Name = "lblQtyDisClosed";
            this.lblQtyDisClosed.Size = new System.Drawing.Size(30, 13);
            this.lblQtyDisClosed.TabIndex = 4;
            this.lblQtyDisClosed.Text = "DQ:";
            // 
            // txtOrderPrice
            // 
            this.txtOrderPrice.AutoMinMax = true;
            this.txtOrderPrice.AutoUpDown = true;
            this.txtOrderPrice.BackColor = System.Drawing.Color.White;
            this.txtOrderPrice.CustomFormat = "";
            this.txtOrderPrice.FocusColor = System.Drawing.Color.LightYellow;
            this.txtOrderPrice.FocusForeColor = System.Drawing.Color.Black;
            this.txtOrderPrice.ForeColor = System.Drawing.Color.Black;
            this.txtOrderPrice.LeaveColor = System.Drawing.Color.White;
            this.txtOrderPrice.LeaveForeColor = System.Drawing.Color.Black;
            this.txtOrderPrice.Location = new System.Drawing.Point(191, 3);
            this.txtOrderPrice.MaxValue = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.txtOrderPrice.MinValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtOrderPrice.Name = "txtOrderPrice";
            this.txtOrderPrice.PriceTick = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtOrderPrice.Size = new System.Drawing.Size(66, 21);
            this.txtOrderPrice.TabIndex = 1;
            this.txtOrderPrice.TextType = CustomControls.CustomTextBox.TextTypeEnum.Decimal;
            this.txtOrderPrice.WarningColor = System.Drawing.Color.IndianRed;
            this.txtOrderPrice.WatermarkText = "Price";
            this.txtOrderPrice.TextChanged += new System.EventHandler(this.txtOrderPrice_TextChanged);
            this.txtOrderPrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOrderPrice_KeyDown);
            // 
            // txtQty
            // 
            this.txtQty.AutoMinMax = true;
            this.txtQty.AutoUpDown = true;
            this.txtQty.BackColor = System.Drawing.Color.White;
            this.txtQty.CustomFormat = "";
            this.txtQty.FocusColor = System.Drawing.Color.LightYellow;
            this.txtQty.FocusForeColor = System.Drawing.Color.Black;
            this.txtQty.ForeColor = System.Drawing.Color.Black;
            this.txtQty.LeaveColor = System.Drawing.Color.White;
            this.txtQty.LeaveForeColor = System.Drawing.Color.Black;
            this.txtQty.Location = new System.Drawing.Point(114, 3);
            this.txtQty.MaxValue = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.txtQty.MinValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtQty.Name = "txtQty";
            this.txtQty.PriceTick = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtQty.Size = new System.Drawing.Size(42, 21);
            this.txtQty.TabIndex = 0;
            this.txtQty.TextType = CustomControls.CustomTextBox.TextTypeEnum.Int;
            this.txtQty.WarningColor = System.Drawing.Color.IndianRed;
            this.txtQty.WatermarkText = "Qty";
            this.txtQty.TextChanged += new System.EventHandler(this.txtQty_TextChanged);
            this.txtQty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQty_KeyDown);
            // 
            // txtTriggerPrice
            // 
            this.txtTriggerPrice.AutoMinMax = true;
            this.txtTriggerPrice.AutoUpDown = true;
            this.txtTriggerPrice.BackColor = System.Drawing.Color.White;
            this.txtTriggerPrice.CustomFormat = "";
            this.txtTriggerPrice.FocusColor = System.Drawing.Color.LightYellow;
            this.txtTriggerPrice.FocusForeColor = System.Drawing.Color.Black;
            this.txtTriggerPrice.ForeColor = System.Drawing.Color.Black;
            this.txtTriggerPrice.LeaveColor = System.Drawing.Color.White;
            this.txtTriggerPrice.LeaveForeColor = System.Drawing.Color.Black;
            this.txtTriggerPrice.Location = new System.Drawing.Point(511, 3);
            this.txtTriggerPrice.MaxValue = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.txtTriggerPrice.MinValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtTriggerPrice.Name = "txtTriggerPrice";
            this.txtTriggerPrice.PriceTick = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtTriggerPrice.Size = new System.Drawing.Size(69, 21);
            this.txtTriggerPrice.TabIndex = 5;
            this.txtTriggerPrice.TextType = CustomControls.CustomTextBox.TextTypeEnum.Decimal;
            this.txtTriggerPrice.WarningColor = System.Drawing.Color.IndianRed;
            this.txtTriggerPrice.WatermarkText = "Trigger Price";
            // 
            // txtQtyDisclosed
            // 
            this.txtQtyDisclosed.AutoMinMax = true;
            this.txtQtyDisclosed.AutoUpDown = true;
            this.txtQtyDisclosed.BackColor = System.Drawing.Color.White;
            this.txtQtyDisclosed.CustomFormat = "";
            this.txtQtyDisclosed.FocusColor = System.Drawing.Color.LightYellow;
            this.txtQtyDisclosed.FocusForeColor = System.Drawing.Color.Black;
            this.txtQtyDisclosed.ForeColor = System.Drawing.Color.Black;
            this.txtQtyDisclosed.LeaveColor = System.Drawing.Color.White;
            this.txtQtyDisclosed.LeaveForeColor = System.Drawing.Color.Black;
            this.txtQtyDisclosed.Location = new System.Drawing.Point(294, 3);
            this.txtQtyDisclosed.MaxValue = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.txtQtyDisclosed.MinValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtQtyDisclosed.Name = "txtQtyDisclosed";
            this.txtQtyDisclosed.PriceTick = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtQtyDisclosed.Size = new System.Drawing.Size(37, 21);
            this.txtQtyDisclosed.TabIndex = 2;
            this.txtQtyDisclosed.TextType = CustomControls.CustomTextBox.TextTypeEnum.Int;
            this.txtQtyDisclosed.WarningColor = System.Drawing.Color.IndianRed;
            this.txtQtyDisclosed.WatermarkText = "DQ";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtRemarks.AutoMinMax = false;
            this.txtRemarks.AutoUpDown = false;
            this.txtRemarks.BackColor = System.Drawing.Color.White;
            this.txtRemarks.CustomFormat = "";
            this.txtRemarks.FocusColor = System.Drawing.Color.LightYellow;
            this.txtRemarks.FocusForeColor = System.Drawing.Color.Black;
            this.txtRemarks.ForeColor = System.Drawing.Color.Black;
            this.txtRemarks.LeaveColor = System.Drawing.Color.White;
            this.txtRemarks.LeaveForeColor = System.Drawing.Color.Black;
            this.txtRemarks.Location = new System.Drawing.Point(911, 5);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(0);
            this.txtRemarks.Masked = CustomControls.Mask.None;
            this.txtRemarks.MaxValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtRemarks.MinValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.PriceTick = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtRemarks.Size = new System.Drawing.Size(163, 21);
            this.txtRemarks.TabIndex = 9;
            this.txtRemarks.WarningColor = System.Drawing.Color.IndianRed;
            this.txtRemarks.WatermarkText = "Remarks";
            // 
            // cmbBookType
            // 
            this.cmbBookType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBookType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBookType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBookType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBookType.FormattingEnabled = true;
            this.cmbBookType.Location = new System.Drawing.Point(337, 5);
            this.cmbBookType.Name = "cmbBookType";
            this.cmbBookType.Size = new System.Drawing.Size(63, 21);
            this.cmbBookType.TabIndex = 3;
            this.cmbBookType.SelectedIndexChanged += new System.EventHandler(this.cmbBookType_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tbpnlOrderPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblOrderMsg, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnModify, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1189, 80);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(3, 53);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(69, 16);
            this.btnModify.TabIndex = 18;
            this.btnModify.Text = "Modify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Visible = false;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // FrmOrderEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1189, 80);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOrderEntry";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "OrderEntry";
            this.Load += new System.EventHandler(this.FrmOrderEntry_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmOrderEntry_KeyUp);
            this.tbpnlOrderPanel.ResumeLayout(false);
            this.tbpnlOrderPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbpnlOrderPanel;
        public System.Windows.Forms.Label lblOrderMsg;
        private System.Windows.Forms.Label lblMarketProtection;
        public CustomControls.AtsTextBox txtMarketProtection;
        public System.Windows.Forms.ComboBox cmbAccount;
        private System.Windows.Forms.Label lblTriggerPrice;
        public System.Windows.Forms.ComboBox cmbValidity;
        private System.Windows.Forms.Label lblQty;
        public System.Windows.Forms.ComboBox cmbBookType;
        public CustomControls.AtsTextBox txtRemarks;
        public System.Windows.Forms.DateTimePicker dtpGTD;
        public System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label lblOrderPrice;
        private System.Windows.Forms.Label lblQtyDisClosed;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public CustomControls.CustomTextBox txtQtyDisclosed;
        public CustomControls.CustomTextBox txtOrderPrice;
        public CustomControls.CustomTextBox txtQty;
        public CustomControls.CustomTextBox txtTriggerPrice;
        public System.Windows.Forms.Button btnModify;
    }
}