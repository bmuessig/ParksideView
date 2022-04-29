namespace ParksideView
{
    partial class MainWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.valueLabel = new System.Windows.Forms.Label();
            this.readoutLayout = new System.Windows.Forms.TableLayoutPanel();
            this.unitLabel = new System.Windows.Forms.Label();
            this.modeLabel = new System.Windows.Forms.Label();
            this.readoutPanel = new System.Windows.Forms.Panel();
            this.recordToggleButton = new System.Windows.Forms.Button();
            this.acquisitionGroup = new System.Windows.Forms.GroupBox();
            this.acquisitionPauseButton = new System.Windows.Forms.Button();
            this.intervalLabel = new System.Windows.Forms.Label();
            this.csvFormatGroup = new System.Windows.Forms.GroupBox();
            this.csvFormatUSRadio = new System.Windows.Forms.RadioButton();
            this.csvFormatDERadio = new System.Windows.Forms.RadioButton();
            this.acquisitionStatusLabel = new System.Windows.Forms.Label();
            this.intervalNumeric = new System.Windows.Forms.NumericUpDown();
            this.connectionGroup = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.connectionStatusLabel = new System.Windows.Forms.Label();
            this.refreshPortsButton = new System.Windows.Forms.Button();
            this.portLabel = new System.Windows.Forms.Label();
            this.portsListBox = new System.Windows.Forms.ComboBox();
            this.windowGroup = new System.Windows.Forms.GroupBox();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.topMostCheck = new System.Windows.Forms.CheckBox();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.acquisitionTimer = new System.Windows.Forms.Timer(this.components);
            this.bargraphBar = new System.Windows.Forms.ProgressBar();
            this.recordSaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.statisticsGroup = new System.Windows.Forms.GroupBox();
            this.maxValueLabel = new System.Windows.Forms.Label();
            this.minValueLabel = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.maxLabel = new System.Windows.Forms.Label();
            this.minLabel = new System.Windows.Forms.Label();
            this.readoutLayout.SuspendLayout();
            this.readoutPanel.SuspendLayout();
            this.acquisitionGroup.SuspendLayout();
            this.csvFormatGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intervalNumeric)).BeginInit();
            this.connectionGroup.SuspendLayout();
            this.windowGroup.SuspendLayout();
            this.statisticsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // valueLabel
            // 
            this.valueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.valueLabel.AutoSize = true;
            this.valueLabel.Font = new System.Drawing.Font("Segoe UI", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueLabel.Location = new System.Drawing.Point(3, 0);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(230, 76);
            this.valueLabel.TabIndex = 0;
            this.valueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.valueLabel.Click += new System.EventHandler(this.valueLabel_Click);
            // 
            // readoutLayout
            // 
            this.readoutLayout.BackColor = System.Drawing.Color.White;
            this.readoutLayout.ColumnCount = 2;
            this.readoutLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.readoutLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.readoutLayout.Controls.Add(this.unitLabel, 1, 0);
            this.readoutLayout.Controls.Add(this.modeLabel, 0, 1);
            this.readoutLayout.Controls.Add(this.valueLabel, 0, 0);
            this.readoutLayout.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.readoutLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.readoutLayout.Location = new System.Drawing.Point(0, 0);
            this.readoutLayout.Name = "readoutLayout";
            this.readoutLayout.RowCount = 2;
            this.readoutLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.readoutLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.readoutLayout.Size = new System.Drawing.Size(338, 114);
            this.readoutLayout.TabIndex = 1;
            this.readoutLayout.Click += new System.EventHandler(this.readoutLayout_Click);
            // 
            // unitLabel
            // 
            this.unitLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unitLabel.AutoSize = true;
            this.unitLabel.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unitLabel.Location = new System.Drawing.Point(239, 0);
            this.unitLabel.Name = "unitLabel";
            this.unitLabel.Size = new System.Drawing.Size(96, 76);
            this.unitLabel.TabIndex = 2;
            this.unitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.unitLabel.Click += new System.EventHandler(this.unitLabel_Click);
            // 
            // modeLabel
            // 
            this.modeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modeLabel.AutoSize = true;
            this.readoutLayout.SetColumnSpan(this.modeLabel, 2);
            this.modeLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modeLabel.Location = new System.Drawing.Point(3, 76);
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(332, 38);
            this.modeLabel.TabIndex = 3;
            this.modeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.modeLabel.Click += new System.EventHandler(this.modeLabel_Click);
            // 
            // readoutPanel
            // 
            this.readoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.readoutPanel.Controls.Add(this.readoutLayout);
            this.readoutPanel.Location = new System.Drawing.Point(13, 11);
            this.readoutPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.readoutPanel.Name = "readoutPanel";
            this.readoutPanel.Size = new System.Drawing.Size(340, 116);
            this.readoutPanel.TabIndex = 2;
            // 
            // recordToggleButton
            // 
            this.recordToggleButton.Location = new System.Drawing.Point(8, 18);
            this.recordToggleButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.recordToggleButton.Name = "recordToggleButton";
            this.recordToggleButton.Size = new System.Drawing.Size(110, 22);
            this.recordToggleButton.TabIndex = 7;
            this.recordToggleButton.Text = "<Record>";
            this.recordToggleButton.UseVisualStyleBackColor = true;
            this.recordToggleButton.Click += new System.EventHandler(this.recordToggleButton_Click);
            // 
            // acquisitionGroup
            // 
            this.acquisitionGroup.Controls.Add(this.acquisitionPauseButton);
            this.acquisitionGroup.Controls.Add(this.intervalLabel);
            this.acquisitionGroup.Controls.Add(this.csvFormatGroup);
            this.acquisitionGroup.Controls.Add(this.acquisitionStatusLabel);
            this.acquisitionGroup.Controls.Add(this.intervalNumeric);
            this.acquisitionGroup.Controls.Add(this.recordToggleButton);
            this.acquisitionGroup.Location = new System.Drawing.Point(14, 276);
            this.acquisitionGroup.Name = "acquisitionGroup";
            this.acquisitionGroup.Size = new System.Drawing.Size(339, 96);
            this.acquisitionGroup.TabIndex = 6;
            this.acquisitionGroup.TabStop = false;
            this.acquisitionGroup.Text = "<Acquisition>";
            // 
            // acquisitionPauseButton
            // 
            this.acquisitionPauseButton.Location = new System.Drawing.Point(124, 18);
            this.acquisitionPauseButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.acquisitionPauseButton.Name = "acquisitionPauseButton";
            this.acquisitionPauseButton.Size = new System.Drawing.Size(96, 22);
            this.acquisitionPauseButton.TabIndex = 8;
            this.acquisitionPauseButton.Text = "<P/C>";
            this.acquisitionPauseButton.UseVisualStyleBackColor = true;
            this.acquisitionPauseButton.Click += new System.EventHandler(this.acquisitionPauseButton_Click);
            // 
            // intervalLabel
            // 
            this.intervalLabel.Location = new System.Drawing.Point(5, 50);
            this.intervalLabel.Name = "intervalLabel";
            this.intervalLabel.Size = new System.Drawing.Size(94, 13);
            this.intervalLabel.TabIndex = 10;
            this.intervalLabel.Text = "<Interval>";
            // 
            // csvFormatGroup
            // 
            this.csvFormatGroup.Controls.Add(this.csvFormatUSRadio);
            this.csvFormatGroup.Controls.Add(this.csvFormatDERadio);
            this.csvFormatGroup.Location = new System.Drawing.Point(226, 18);
            this.csvFormatGroup.Name = "csvFormatGroup";
            this.csvFormatGroup.Size = new System.Drawing.Size(107, 60);
            this.csvFormatGroup.TabIndex = 10;
            this.csvFormatGroup.TabStop = false;
            this.csvFormatGroup.Text = "<CSV format>";
            // 
            // csvFormatUSRadio
            // 
            this.csvFormatUSRadio.Checked = true;
            this.csvFormatUSRadio.Location = new System.Drawing.Point(9, 16);
            this.csvFormatUSRadio.Name = "csvFormatUSRadio";
            this.csvFormatUSRadio.Size = new System.Drawing.Size(93, 17);
            this.csvFormatUSRadio.TabIndex = 11;
            this.csvFormatUSRadio.TabStop = true;
            this.csvFormatUSRadio.Text = "US: 0.12, 0.23";
            this.csvFormatUSRadio.UseVisualStyleBackColor = true;
            // 
            // csvFormatDERadio
            // 
            this.csvFormatDERadio.Location = new System.Drawing.Point(9, 38);
            this.csvFormatDERadio.Name = "csvFormatDERadio";
            this.csvFormatDERadio.Size = new System.Drawing.Size(93, 17);
            this.csvFormatDERadio.TabIndex = 12;
            this.csvFormatDERadio.Text = "DE: 0,12; 0,23";
            this.csvFormatDERadio.UseVisualStyleBackColor = true;
            // 
            // acquisitionStatusLabel
            // 
            this.acquisitionStatusLabel.Location = new System.Drawing.Point(5, 73);
            this.acquisitionStatusLabel.Name = "acquisitionStatusLabel";
            this.acquisitionStatusLabel.Size = new System.Drawing.Size(292, 13);
            this.acquisitionStatusLabel.TabIndex = 5;
            this.acquisitionStatusLabel.Text = "<Acquisition-Status>";
            // 
            // intervalNumeric
            // 
            this.intervalNumeric.Location = new System.Drawing.Point(145, 45);
            this.intervalNumeric.Maximum = new decimal(new int[] {
            172800,
            0,
            0,
            0});
            this.intervalNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.intervalNumeric.Name = "intervalNumeric";
            this.intervalNumeric.Size = new System.Drawing.Size(75, 22);
            this.intervalNumeric.TabIndex = 9;
            this.intervalNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.intervalNumeric.ValueChanged += new System.EventHandler(this.intervalNumeric_ValueChanged);
            this.intervalNumeric.KeyDown += new System.Windows.Forms.KeyEventHandler(this.intervalNumeric_KeyDown);
            this.intervalNumeric.Leave += new System.EventHandler(this.intervalNumeric_Leave);
            // 
            // connectionGroup
            // 
            this.connectionGroup.Controls.Add(this.connectButton);
            this.connectionGroup.Controls.Add(this.connectionStatusLabel);
            this.connectionGroup.Controls.Add(this.refreshPortsButton);
            this.connectionGroup.Controls.Add(this.portLabel);
            this.connectionGroup.Controls.Add(this.portsListBox);
            this.connectionGroup.Location = new System.Drawing.Point(13, 149);
            this.connectionGroup.Name = "connectionGroup";
            this.connectionGroup.Size = new System.Drawing.Size(199, 69);
            this.connectionGroup.TabIndex = 0;
            this.connectionGroup.TabStop = false;
            this.connectionGroup.Text = "<Connection>";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(125, 40);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(66, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "<S/P>";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // connectionStatusLabel
            // 
            this.connectionStatusLabel.Location = new System.Drawing.Point(6, 45);
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(135, 13);
            this.connectionStatusLabel.TabIndex = 3;
            this.connectionStatusLabel.Text = "<Connection-Status>";
            // 
            // refreshPortsButton
            // 
            this.refreshPortsButton.AutoEllipsis = true;
            this.refreshPortsButton.Location = new System.Drawing.Point(125, 15);
            this.refreshPortsButton.Name = "refreshPortsButton";
            this.refreshPortsButton.Size = new System.Drawing.Size(66, 23);
            this.refreshPortsButton.TabIndex = 2;
            this.refreshPortsButton.Text = "<Ref>";
            this.refreshPortsButton.UseVisualStyleBackColor = true;
            this.refreshPortsButton.Click += new System.EventHandler(this.refreshPortsButton_Click);
            // 
            // portLabel
            // 
            this.portLabel.Location = new System.Drawing.Point(6, 20);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(47, 17);
            this.portLabel.TabIndex = 1;
            this.portLabel.Text = "<Port>";
            // 
            // portsListBox
            // 
            this.portsListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portsListBox.FormattingEnabled = true;
            this.portsListBox.Location = new System.Drawing.Point(59, 16);
            this.portsListBox.Name = "portsListBox";
            this.portsListBox.Size = new System.Drawing.Size(60, 21);
            this.portsListBox.TabIndex = 1;
            // 
            // windowGroup
            // 
            this.windowGroup.Controls.Add(this.minimizeButton);
            this.windowGroup.Controls.Add(this.topMostCheck);
            this.windowGroup.Location = new System.Drawing.Point(218, 149);
            this.windowGroup.Name = "windowGroup";
            this.windowGroup.Size = new System.Drawing.Size(134, 69);
            this.windowGroup.TabIndex = 13;
            this.windowGroup.TabStop = false;
            this.windowGroup.Text = "<Window>";
            // 
            // minimizeButton
            // 
            this.minimizeButton.Location = new System.Drawing.Point(8, 39);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(120, 23);
            this.minimizeButton.TabIndex = 15;
            this.minimizeButton.Text = "<Minimize>";
            this.minimizeButton.UseVisualStyleBackColor = true;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // topMostCheck
            // 
            this.topMostCheck.Location = new System.Drawing.Point(11, 18);
            this.topMostCheck.Name = "topMostCheck";
            this.topMostCheck.Size = new System.Drawing.Size(117, 19);
            this.topMostCheck.TabIndex = 14;
            this.topMostCheck.Text = "<Top most>";
            this.topMostCheck.UseVisualStyleBackColor = true;
            this.topMostCheck.CheckedChanged += new System.EventHandler(this.topMostCheck_CheckedChanged);
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.copyrightLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.copyrightLabel.ForeColor = System.Drawing.Color.SaddleBrown;
            this.copyrightLabel.Location = new System.Drawing.Point(0, 382);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(365, 18);
            this.copyrightLabel.TabIndex = 7;
            this.copyrightLabel.Text = "© 2020, Benedikt Müssig, mikrocontroller.net/topic/491973";
            this.copyrightLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.copyrightLabel.Click += new System.EventHandler(this.copyrightLabel_Click);
            // 
            // acquisitionTimer
            // 
            this.acquisitionTimer.Interval = 1;
            this.acquisitionTimer.Tick += new System.EventHandler(this.acquisitionTimer_Tick);
            // 
            // bargraphBar
            // 
            this.bargraphBar.ForeColor = System.Drawing.Color.Green;
            this.bargraphBar.Location = new System.Drawing.Point(13, 130);
            this.bargraphBar.Name = "bargraphBar";
            this.bargraphBar.Size = new System.Drawing.Size(339, 13);
            this.bargraphBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.bargraphBar.TabIndex = 8;
            // 
            // recordSaveDialog
            // 
            this.recordSaveDialog.DefaultExt = "csv";
            // 
            // statisticsGroup
            // 
            this.statisticsGroup.Controls.Add(this.maxValueLabel);
            this.statisticsGroup.Controls.Add(this.minValueLabel);
            this.statisticsGroup.Controls.Add(this.resetButton);
            this.statisticsGroup.Controls.Add(this.maxLabel);
            this.statisticsGroup.Controls.Add(this.minLabel);
            this.statisticsGroup.Location = new System.Drawing.Point(14, 224);
            this.statisticsGroup.Name = "statisticsGroup";
            this.statisticsGroup.Size = new System.Drawing.Size(338, 46);
            this.statisticsGroup.TabIndex = 4;
            this.statisticsGroup.TabStop = false;
            this.statisticsGroup.Text = "<Statistics>";
            // 
            // maxValueLabel
            // 
            this.maxValueLabel.AutoSize = true;
            this.maxValueLabel.Location = new System.Drawing.Point(169, 21);
            this.maxValueLabel.Name = "maxValueLabel";
            this.maxValueLabel.Size = new System.Drawing.Size(11, 13);
            this.maxValueLabel.TabIndex = 4;
            this.maxValueLabel.Text = "-";
            // 
            // minValueLabel
            // 
            this.minValueLabel.AutoSize = true;
            this.minValueLabel.Location = new System.Drawing.Point(41, 21);
            this.minValueLabel.Name = "minValueLabel";
            this.minValueLabel.Size = new System.Drawing.Size(11, 13);
            this.minValueLabel.TabIndex = 3;
            this.minValueLabel.Text = "-";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(247, 16);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 5;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // maxLabel
            // 
            this.maxLabel.AutoSize = true;
            this.maxLabel.Location = new System.Drawing.Point(132, 21);
            this.maxLabel.Name = "maxLabel";
            this.maxLabel.Size = new System.Drawing.Size(31, 13);
            this.maxLabel.TabIndex = 1;
            this.maxLabel.Text = "Max:";
            // 
            // minLabel
            // 
            this.minLabel.AutoSize = true;
            this.minLabel.Location = new System.Drawing.Point(5, 21);
            this.minLabel.Name = "minLabel";
            this.minLabel.Size = new System.Drawing.Size(30, 13);
            this.minLabel.TabIndex = 0;
            this.minLabel.Text = "Min:";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 400);
            this.Controls.Add(this.statisticsGroup);
            this.Controls.Add(this.bargraphBar);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.windowGroup);
            this.Controls.Add(this.connectionGroup);
            this.Controls.Add(this.acquisitionGroup);
            this.Controls.Add(this.readoutPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ParksideView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.readoutLayout.ResumeLayout(false);
            this.readoutLayout.PerformLayout();
            this.readoutPanel.ResumeLayout(false);
            this.acquisitionGroup.ResumeLayout(false);
            this.csvFormatGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.intervalNumeric)).EndInit();
            this.connectionGroup.ResumeLayout(false);
            this.windowGroup.ResumeLayout(false);
            this.statisticsGroup.ResumeLayout(false);
            this.statisticsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.TableLayoutPanel readoutLayout;
        private System.Windows.Forms.Label unitLabel;
        private System.Windows.Forms.Label modeLabel;
        private System.Windows.Forms.Panel readoutPanel;
        private System.Windows.Forms.Button recordToggleButton;
        private System.Windows.Forms.GroupBox acquisitionGroup;
        private System.Windows.Forms.Label acquisitionStatusLabel;
        private System.Windows.Forms.NumericUpDown intervalNumeric;
        private System.Windows.Forms.GroupBox connectionGroup;
        private System.Windows.Forms.Label intervalLabel;
        private System.Windows.Forms.GroupBox csvFormatGroup;
        private System.Windows.Forms.RadioButton csvFormatUSRadio;
        private System.Windows.Forms.RadioButton csvFormatDERadio;
        private System.Windows.Forms.Label connectionStatusLabel;
        private System.Windows.Forms.Button refreshPortsButton;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.ComboBox portsListBox;
        private System.Windows.Forms.GroupBox windowGroup;
        private System.Windows.Forms.CheckBox topMostCheck;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Button minimizeButton;
        private System.Windows.Forms.Button acquisitionPauseButton;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Timer acquisitionTimer;
        private System.Windows.Forms.ProgressBar bargraphBar;
        private System.Windows.Forms.SaveFileDialog recordSaveDialog;
        private System.Windows.Forms.GroupBox statisticsGroup;
        private System.Windows.Forms.Label maxValueLabel;
        private System.Windows.Forms.Label minValueLabel;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Label maxLabel;
        private System.Windows.Forms.Label minLabel;
    }
}

