namespace Parise.RaisersEdge.ConnectionMonitor.Desktop
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Licenses In Use", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Boot Candidate(s)", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("Excluded Users", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("Safe Users", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "User Name",
            "Test"}, 0);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("User Name", 1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("User Name", 2);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("15 of 20", 3);
            this.taskIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startMonitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutTimer = new System.Windows.Forms.Timer(this.components);
            this.connectionListWorker = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.regularContainer = new System.Windows.Forms.TableLayoutPanel();
            this.connectionWorker = new System.ComponentModel.BackgroundWorker();
            this.navigationPanel = new Ascend.Windows.Forms.NavigationPane();
            this.navPaneSettings = new Ascend.Windows.Forms.NavigationPanePage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtDbConnection = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.activeLicenseLimit = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.maxIdleTime = new System.Windows.Forms.NumericUpDown();
            this.navPaneConnection = new Ascend.Windows.Forms.NavigationPanePage();
            this.panelConnecting = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelBadConnection = new System.Windows.Forms.Panel();
            this.lblOutageMsg = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panelGoodConnection = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.navPaneLicenseUsage = new Ascend.Windows.Forms.NavigationPanePage();
            this.lstViewLicenses = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.imgListLicenseUsage = new System.Windows.Forms.ImageList(this.components);
            this.panelLoadingLicense = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            this.navigationPanel.SuspendLayout();
            this.navPaneSettings.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.activeLicenseLimit)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxIdleTime)).BeginInit();
            this.navPaneConnection.SuspendLayout();
            this.panelConnecting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelBadConnection.SuspendLayout();
            this.panelGoodConnection.SuspendLayout();
            this.navPaneLicenseUsage.SuspendLayout();
            this.panelLoadingLicense.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // taskIcon
            // 
            this.taskIcon.ContextMenuStrip = this.contextMenuStrip1;
            this.taskIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("taskIcon.Icon")));
            this.taskIcon.Text = "RE Connection Monitor";
            this.taskIcon.Visible = true;
            this.taskIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.taskIcon_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startMonitoringToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(152, 54);
            // 
            // startMonitoringToolStripMenuItem
            // 
            this.startMonitoringToolStripMenuItem.Name = "startMonitoringToolStripMenuItem";
            this.startMonitoringToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.startMonitoringToolStripMenuItem.Text = "Start Monitoring";
            this.startMonitoringToolStripMenuItem.Click += new System.EventHandler(this.startMonitoringToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(148, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // logoutTimer
            // 
            this.logoutTimer.Interval = 1000;
            // 
            // connectionListWorker
            // 
            this.connectionListWorker.WorkerSupportsCancellation = true;
            this.connectionListWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            // 
            // regularContainer
            // 
            this.regularContainer.AutoSize = true;
            this.regularContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.regularContainer.ColumnCount = 1;
            this.regularContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.regularContainer.Location = new System.Drawing.Point(666, 55);
            this.regularContainer.Name = "regularContainer";
            this.regularContainer.RowCount = 2;
            this.regularContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.40506F));
            this.regularContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.59494F));
            this.regularContainer.Size = new System.Drawing.Size(0, 0);
            this.regularContainer.TabIndex = 8;
            // 
            // connectionWorker
            // 
            this.connectionWorker.WorkerSupportsCancellation = true;
            this.connectionWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.connectionWorker_DoWork);
            // 
            // navigationPanel
            // 
            this.navigationPanel.AntiAlias = true;
            this.navigationPanel.ButtonActiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(225)))), ((int)(((byte)(155)))));
            this.navigationPanel.ButtonActiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navigationPanel.ButtonBorderColor = System.Drawing.SystemColors.MenuHighlight;
            this.navigationPanel.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.navigationPanel.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.navigationPanel.ButtonGradientHighColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navigationPanel.ButtonGradientLowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navigationPanel.ButtonHighlightGradientHighColor = System.Drawing.Color.White;
            this.navigationPanel.ButtonHighlightGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navigationPanel.CaptionBorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.navigationPanel.CaptionFont = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.navigationPanel.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.navigationPanel.CaptionGradientHighColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navigationPanel.CaptionGradientLowColor = System.Drawing.SystemColors.ActiveCaption;
            this.navigationPanel.Controls.Add(this.navPaneSettings);
            this.navigationPanel.Controls.Add(this.navPaneConnection);
            this.navigationPanel.Controls.Add(this.navPaneLicenseUsage);
            this.navigationPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.navigationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navigationPanel.FooterGradientHighColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navigationPanel.FooterGradientLowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navigationPanel.FooterHeight = 30;
            this.navigationPanel.FooterHighlightGradientHighColor = System.Drawing.Color.White;
            this.navigationPanel.FooterHighlightGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navigationPanel.Location = new System.Drawing.Point(0, 0);
            this.navigationPanel.Name = "navigationPanel";
            this.navigationPanel.NavigationPages.AddRange(new Ascend.Windows.Forms.NavigationPanePage[] {
            this.navPaneConnection,
            this.navPaneLicenseUsage,
            this.navPaneSettings});
            this.navigationPanel.RenderMode = Ascend.Windows.Forms.RenderMode.Glass;
            this.navigationPanel.Size = new System.Drawing.Size(341, 495);
            this.navigationPanel.TabIndex = 13;
            this.navigationPanel.VisibleButtonCount = 3;
            // 
            // navPaneSettings
            // 
            this.navPaneSettings.ActiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(225)))), ((int)(((byte)(155)))));
            this.navPaneSettings.ActiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navPaneSettings.AutoScroll = true;
            this.navPaneSettings.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.navPaneSettings.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.navPaneSettings.Controls.Add(this.linkLabel1);
            this.navPaneSettings.Controls.Add(this.groupBox5);
            this.navPaneSettings.Controls.Add(this.groupBox2);
            this.navPaneSettings.Controls.Add(this.groupBox3);
            this.navPaneSettings.GradientHighColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navPaneSettings.GradientLowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navPaneSettings.HighlightGradientHighColor = System.Drawing.Color.White;
            this.navPaneSettings.HighlightGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navPaneSettings.Image = global::Parise.RaisersEdge.ConnectionMonitor.Desktop.Properties.Resources.process2;
            this.navPaneSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.navPaneSettings.ImageFooter = null;
            this.navPaneSettings.ImageIndex = -1;
            this.navPaneSettings.ImageIndexFooter = -1;
            this.navPaneSettings.ImageKey = "";
            this.navPaneSettings.ImageKeyFooter = "";
            this.navPaneSettings.ImageList = null;
            this.navPaneSettings.ImageListFooter = null;
            this.navPaneSettings.Key = "navPaneSettings";
            this.navPaneSettings.Location = new System.Drawing.Point(1, 27);
            this.navPaneSettings.Name = "navPaneSettings";
            this.navPaneSettings.Size = new System.Drawing.Size(339, 334);
            this.navPaneSettings.TabIndex = 2;
            this.navPaneSettings.Text = "Settings";
            this.navPaneSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.navPaneSettings.ToolTipText = null;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(293, 285);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(35, 13);
            this.linkLabel1.TabIndex = 28;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Save";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // groupBox5
            // 
            this.groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox5.Controls.Add(this.txtDbConnection);
            this.groupBox5.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(11, 125);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(317, 157);
            this.groupBox5.TabIndex = 25;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Database Connection String";
            // 
            // txtDbConnection
            // 
            this.txtDbConnection.Location = new System.Drawing.Point(6, 19);
            this.txtDbConnection.Multiline = true;
            this.txtDbConnection.Name = "txtDbConnection";
            this.txtDbConnection.Size = new System.Drawing.Size(306, 132);
            this.txtDbConnection.TabIndex = 14;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.activeLicenseLimit);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(11, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(317, 54);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Active License Limit";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(83, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 30);
            this.label2.TabIndex = 7;
            this.label2.Text = "Users will not be disconnected until license usage reaches this value.";
            // 
            // activeLicenseLimit
            // 
            this.activeLicenseLimit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activeLicenseLimit.Location = new System.Drawing.Point(19, 17);
            this.activeLicenseLimit.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.activeLicenseLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.activeLicenseLimit.Name = "activeLicenseLimit";
            this.activeLicenseLimit.Size = new System.Drawing.Size(48, 21);
            this.activeLicenseLimit.TabIndex = 6;
            this.activeLicenseLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.maxIdleTime);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(11, 67);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(317, 52);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Maximum Idle Time (Minutes)";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(84, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 30);
            this.label3.TabIndex = 9;
            this.label3.Text = "Users will not be disconnected unless idle time is at or above this value.";
            // 
            // maxIdleTime
            // 
            this.maxIdleTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxIdleTime.Location = new System.Drawing.Point(17, 17);
            this.maxIdleTime.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.maxIdleTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxIdleTime.Name = "maxIdleTime";
            this.maxIdleTime.Size = new System.Drawing.Size(50, 21);
            this.maxIdleTime.TabIndex = 8;
            this.maxIdleTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // navPaneConnection
            // 
            this.navPaneConnection.ActiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(225)))), ((int)(((byte)(155)))));
            this.navPaneConnection.ActiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navPaneConnection.AutoScroll = true;
            this.navPaneConnection.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.navPaneConnection.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.navPaneConnection.Controls.Add(this.panelConnecting);
            this.navPaneConnection.Controls.Add(this.panelBadConnection);
            this.navPaneConnection.Controls.Add(this.panelGoodConnection);
            this.navPaneConnection.GradientHighColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navPaneConnection.GradientLowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navPaneConnection.HighlightGradientHighColor = System.Drawing.Color.White;
            this.navPaneConnection.HighlightGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navPaneConnection.Image = ((System.Drawing.Image)(resources.GetObject("navPaneConnection.Image")));
            this.navPaneConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.navPaneConnection.ImageFooter = null;
            this.navPaneConnection.ImageIndex = 0;
            this.navPaneConnection.ImageIndexFooter = -1;
            this.navPaneConnection.ImageKey = "";
            this.navPaneConnection.ImageKeyFooter = "";
            this.navPaneConnection.ImageList = this.imageList1;
            this.navPaneConnection.ImageListFooter = null;
            this.navPaneConnection.Key = "navPaneConnection";
            this.navPaneConnection.Location = new System.Drawing.Point(1, 27);
            this.navPaneConnection.Name = "navPaneConnection";
            this.navPaneConnection.Size = new System.Drawing.Size(339, 334);
            this.navPaneConnection.TabIndex = 0;
            this.navPaneConnection.Text = "Connection Status";
            this.navPaneConnection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.navPaneConnection.ToolTipText = null;
            // 
            // panelConnecting
            // 
            this.panelConnecting.Controls.Add(this.label4);
            this.panelConnecting.Controls.Add(this.pictureBox1);
            this.panelConnecting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConnecting.Location = new System.Drawing.Point(0, 0);
            this.panelConnecting.Name = "panelConnecting";
            this.panelConnecting.Size = new System.Drawing.Size(339, 334);
            this.panelConnecting.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(2, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(336, 23);
            this.label4.TabIndex = 9;
            this.label4.Text = "Please wait - connecting..";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Parise.RaisersEdge.ConnectionMonitor.Desktop.Properties.Resources.ReportServer;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(339, 334);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelBadConnection
            // 
            this.panelBadConnection.Controls.Add(this.lblOutageMsg);
            this.panelBadConnection.Controls.Add(this.label5);
            this.panelBadConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBadConnection.Location = new System.Drawing.Point(0, 0);
            this.panelBadConnection.Name = "panelBadConnection";
            this.panelBadConnection.Size = new System.Drawing.Size(339, 334);
            this.panelBadConnection.TabIndex = 11;
            this.panelBadConnection.Visible = false;
            // 
            // lblOutageMsg
            // 
            this.lblOutageMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOutageMsg.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutageMsg.ForeColor = System.Drawing.Color.DarkRed;
            this.lblOutageMsg.Location = new System.Drawing.Point(0, 47);
            this.lblOutageMsg.Name = "lblOutageMsg";
            this.lblOutageMsg.Size = new System.Drawing.Size(339, 287);
            this.lblOutageMsg.TabIndex = 2;
            this.lblOutageMsg.Text = "Error Message Here";
            this.lblOutageMsg.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Image = global::Parise.RaisersEdge.ConnectionMonitor.Desktop.Properties.Resources.warning;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label5.Size = new System.Drawing.Size(339, 47);
            this.label5.TabIndex = 1;
            this.label5.Text = "Connection Failed!";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelGoodConnection
            // 
            this.panelGoodConnection.Controls.Add(this.label6);
            this.panelGoodConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGoodConnection.Location = new System.Drawing.Point(0, 0);
            this.panelGoodConnection.Name = "panelGoodConnection";
            this.panelGoodConnection.Size = new System.Drawing.Size(339, 334);
            this.panelGoodConnection.TabIndex = 17;
            this.panelGoodConnection.Visible = false;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Image = global::Parise.RaisersEdge.ConnectionMonitor.Desktop.Properties.Resources.accept;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label6.Size = new System.Drawing.Size(339, 334);
            this.label6.TabIndex = 1;
            this.label6.Text = "Connection Successful!";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "database_search.png");
            this.imageList1.Images.SetKeyName(1, "database_warning.png");
            this.imageList1.Images.SetKeyName(2, "database_accept.png");
            this.imageList1.Images.SetKeyName(3, "database_process.png");
            // 
            // navPaneLicenseUsage
            // 
            this.navPaneLicenseUsage.ActiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(225)))), ((int)(((byte)(155)))));
            this.navPaneLicenseUsage.ActiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navPaneLicenseUsage.AutoScroll = true;
            this.navPaneLicenseUsage.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.navPaneLicenseUsage.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.navPaneLicenseUsage.Controls.Add(this.panelLoadingLicense);
            this.navPaneLicenseUsage.Controls.Add(this.lstViewLicenses);
            this.navPaneLicenseUsage.GradientHighColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navPaneLicenseUsage.GradientLowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navPaneLicenseUsage.HighlightGradientHighColor = System.Drawing.Color.White;
            this.navPaneLicenseUsage.HighlightGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navPaneLicenseUsage.Image = global::Parise.RaisersEdge.ConnectionMonitor.Desktop.Properties.Resources.users;
            this.navPaneLicenseUsage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.navPaneLicenseUsage.ImageFooter = null;
            this.navPaneLicenseUsage.ImageIndex = -1;
            this.navPaneLicenseUsage.ImageIndexFooter = -1;
            this.navPaneLicenseUsage.ImageKey = "";
            this.navPaneLicenseUsage.ImageKeyFooter = "";
            this.navPaneLicenseUsage.ImageList = null;
            this.navPaneLicenseUsage.ImageListFooter = null;
            this.navPaneLicenseUsage.Key = "navPaneLicenseUsage";
            this.navPaneLicenseUsage.Location = new System.Drawing.Point(1, 27);
            this.navPaneLicenseUsage.Name = "navPaneLicenseUsage";
            this.navPaneLicenseUsage.Size = new System.Drawing.Size(339, 334);
            this.navPaneLicenseUsage.TabIndex = 1;
            this.navPaneLicenseUsage.Text = "License Usage";
            this.navPaneLicenseUsage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.navPaneLicenseUsage.ToolTipText = null;
            // 
            // lstViewLicenses
            // 
            this.lstViewLicenses.AutoArrange = false;
            this.lstViewLicenses.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstViewLicenses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstViewLicenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstViewLicenses.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            listViewGroup5.Header = "Licenses In Use";
            listViewGroup5.Name = "lstGroupInUse";
            listViewGroup6.Header = "Boot Candidate(s)";
            listViewGroup6.Name = "lstGroupBoot";
            listViewGroup7.Header = "Excluded Users";
            listViewGroup7.Name = "lstGroupExcluded";
            listViewGroup8.Header = "Safe Users";
            listViewGroup8.Name = "lstGroupSafe";
            this.lstViewLicenses.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup5,
            listViewGroup6,
            listViewGroup7,
            listViewGroup8});
            this.lstViewLicenses.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem5.Group = listViewGroup8;
            listViewItem6.Group = listViewGroup7;
            listViewItem7.Group = listViewGroup6;
            listViewItem8.Group = listViewGroup5;
            this.lstViewLicenses.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8});
            this.lstViewLicenses.Location = new System.Drawing.Point(0, 0);
            this.lstViewLicenses.MultiSelect = false;
            this.lstViewLicenses.Name = "lstViewLicenses";
            this.lstViewLicenses.ShowItemToolTips = true;
            this.lstViewLicenses.Size = new System.Drawing.Size(339, 334);
            this.lstViewLicenses.SmallImageList = this.imgListLicenseUsage;
            this.lstViewLicenses.TabIndex = 10;
            this.lstViewLicenses.UseCompatibleStateImageBehavior = false;
            this.lstViewLicenses.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 200;
            // 
            // imgListLicenseUsage
            // 
            this.imgListLicenseUsage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListLicenseUsage.ImageStream")));
            this.imgListLicenseUsage.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListLicenseUsage.Images.SetKeyName(0, "accept.png");
            this.imgListLicenseUsage.Images.SetKeyName(1, "warning.png");
            this.imgListLicenseUsage.Images.SetKeyName(2, "user_delete.png");
            this.imgListLicenseUsage.Images.SetKeyName(3, "user_accept.png");
            this.imgListLicenseUsage.Images.SetKeyName(4, "user.png");
            // 
            // panelLoadingLicense
            // 
            this.panelLoadingLicense.Controls.Add(this.label1);
            this.panelLoadingLicense.Controls.Add(this.pictureBox2);
            this.panelLoadingLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLoadingLicense.Location = new System.Drawing.Point(0, 0);
            this.panelLoadingLicense.Name = "panelLoadingLicense";
            this.panelLoadingLicense.Size = new System.Drawing.Size(339, 334);
            this.panelLoadingLicense.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(336, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "Please wait - loading...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::Parise.RaisersEdge.ConnectionMonitor.Desktop.Properties.Resources.ReportServer;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(339, 334);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(341, 495);
            this.Controls.Add(this.navigationPanel);
            this.Controls.Add(this.regularContainer);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "RE Connection Monitor ::: Inactive";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.navigationPanel.ResumeLayout(false);
            this.navPaneSettings.ResumeLayout(false);
            this.navPaneSettings.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.activeLicenseLimit)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.maxIdleTime)).EndInit();
            this.navPaneConnection.ResumeLayout(false);
            this.panelConnecting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelBadConnection.ResumeLayout(false);
            this.panelGoodConnection.ResumeLayout(false);
            this.navPaneLicenseUsage.ResumeLayout(false);
            this.panelLoadingLicense.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon taskIcon;
        private System.Windows.Forms.Timer logoutTimer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem startMonitoringToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker connectionListWorker;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.TableLayoutPanel regularContainer;
        private System.Windows.Forms.Panel panelBadConnection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblOutageMsg;
        private System.ComponentModel.BackgroundWorker connectionWorker;
        private Ascend.Windows.Forms.NavigationPane navigationPanel;
        private Ascend.Windows.Forms.NavigationPanePage navPaneConnection;
        private System.Windows.Forms.Panel panelConnecting;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelGoodConnection;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ImageList imageList1;
        private Ascend.Windows.Forms.NavigationPanePage navPaneLicenseUsage;
        private System.Windows.Forms.ListView lstViewLicenses;
        private System.Windows.Forms.ImageList imgListLicenseUsage;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private Ascend.Windows.Forms.NavigationPanePage navPaneSettings;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtDbConnection;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown activeLicenseLimit;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown maxIdleTime;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Panel panelLoadingLicense;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

