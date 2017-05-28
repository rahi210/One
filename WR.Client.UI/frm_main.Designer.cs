namespace WR.Client.UI
{
    partial class frm_main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_main));
            this.pnlContext = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cnsHelp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.mnuLogout = new WR.Client.Controls.WrMenuItem();
            this.mnuSelection = new WR.Client.Controls.WrMenuItem();
            this.lblLeftRole = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblLeftPwd = new System.Windows.Forms.Label();
            this.lblLeftOptions = new System.Windows.Forms.Label();
            this.lblLeftUser = new System.Windows.Forms.Label();
            this.mnuReview = new WR.Client.Controls.WrMenuItem();
            this.mnuSetting = new WR.Client.Controls.WrMenuItem();
            this.mnuSelect = new WR.Client.Controls.WrMenuItem();
            this.lblArrow = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblConnction = new System.Windows.Forms.Label();
            this.lblHelp = new System.Windows.Forms.Label();
            this.cbxLang = new System.Windows.Forms.ComboBox();
            this.picMin = new System.Windows.Forms.PictureBox();
            this.picClose = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tlsConn = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.cnsHelp.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContext
            // 
            this.pnlContext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContext.Location = new System.Drawing.Point(235, 60);
            this.pnlContext.Name = "pnlContext";
            this.pnlContext.Size = new System.Drawing.Size(1045, 640);
            this.pnlContext.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cnsHelp
            // 
            this.cnsHelp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlsConn,
            this.tlsAbout});
            this.cnsHelp.Name = "cnsHelp";
            this.cnsHelp.Size = new System.Drawing.Size(191, 48);
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackgroundImage = global::WR.Client.UI.Properties.Resources.left;
            this.pnlLeft.Controls.Add(this.mnuLogout);
            this.pnlLeft.Controls.Add(this.mnuSelection);
            this.pnlLeft.Controls.Add(this.lblLeftRole);
            this.pnlLeft.Controls.Add(this.lblUser);
            this.pnlLeft.Controls.Add(this.lblLeftPwd);
            this.pnlLeft.Controls.Add(this.lblLeftOptions);
            this.pnlLeft.Controls.Add(this.lblLeftUser);
            this.pnlLeft.Controls.Add(this.mnuReview);
            this.pnlLeft.Controls.Add(this.mnuSetting);
            this.pnlLeft.Controls.Add(this.mnuSelect);
            this.pnlLeft.Controls.Add(this.lblArrow);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 60);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(235, 640);
            this.pnlLeft.TabIndex = 1;
            // 
            // mnuLogout
            // 
            this.mnuLogout.BackColor = System.Drawing.Color.Transparent;
            this.mnuLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mnuLogout.ItemBgColor = System.Drawing.Color.Transparent;
            this.mnuLogout.ItemImage = global::WR.Client.UI.Properties.Resources.logout;
            this.mnuLogout.Location = new System.Drawing.Point(4, 441);
            this.mnuLogout.Name = "mnuLogout";
            this.mnuLogout.Size = new System.Drawing.Size(255, 57);
            this.mnuLogout.TabIndex = 7;
            this.mnuLogout.WrText = "Logout";
            this.mnuLogout.ItemClick += new System.EventHandler(this.mnuSelect_ItemClick);
            // 
            // mnuSelection
            // 
            this.mnuSelection.BackColor = System.Drawing.Color.Transparent;
            this.mnuSelection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mnuSelection.ItemBgColor = System.Drawing.Color.Transparent;
            this.mnuSelection.ItemImage = global::WR.Client.UI.Properties.Resources.selection;
            this.mnuSelection.Location = new System.Drawing.Point(4, 58);
            this.mnuSelection.Name = "mnuSelection";
            this.mnuSelection.Size = new System.Drawing.Size(255, 57);
            this.mnuSelection.TabIndex = 6;
            this.mnuSelection.Tag = "30001";
            this.mnuSelection.WrText = "Wafer Selection";
            this.mnuSelection.ItemClick += new System.EventHandler(this.mnuSelect_ItemClick);
            // 
            // lblLeftRole
            // 
            this.lblLeftRole.BackColor = System.Drawing.Color.Transparent;
            this.lblLeftRole.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLeftRole.Font = new System.Drawing.Font("宋体", 10F);
            this.lblLeftRole.ForeColor = System.Drawing.Color.White;
            this.lblLeftRole.Location = new System.Drawing.Point(58, 404);
            this.lblLeftRole.Name = "lblLeftRole";
            this.lblLeftRole.Size = new System.Drawing.Size(208, 30);
            this.lblLeftRole.TabIndex = 5;
            this.lblLeftRole.Tag = "20001";
            this.lblLeftRole.Text = "- Role";
            this.lblLeftRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLeftRole.Click += new System.EventHandler(this.lblLeftRole_Click);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblUser.Image = global::WR.Client.UI.Properties.Resources.user24;
            this.lblUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUser.Location = new System.Drawing.Point(17, 25);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(143, 21);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "      welcome,admin";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLeftPwd
            // 
            this.lblLeftPwd.BackColor = System.Drawing.Color.Transparent;
            this.lblLeftPwd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLeftPwd.Font = new System.Drawing.Font("宋体", 10F);
            this.lblLeftPwd.ForeColor = System.Drawing.Color.White;
            this.lblLeftPwd.Location = new System.Drawing.Point(58, 308);
            this.lblLeftPwd.Name = "lblLeftPwd";
            this.lblLeftPwd.Size = new System.Drawing.Size(208, 30);
            this.lblLeftPwd.TabIndex = 5;
            this.lblLeftPwd.Tag = "20004";
            this.lblLeftPwd.Text = "- Change Password";
            this.lblLeftPwd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLeftPwd.Click += new System.EventHandler(this.lblLeftUser_Click);
            // 
            // lblLeftOptions
            // 
            this.lblLeftOptions.BackColor = System.Drawing.Color.Transparent;
            this.lblLeftOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLeftOptions.Font = new System.Drawing.Font("宋体", 10F);
            this.lblLeftOptions.ForeColor = System.Drawing.Color.White;
            this.lblLeftOptions.Location = new System.Drawing.Point(58, 340);
            this.lblLeftOptions.Name = "lblLeftOptions";
            this.lblLeftOptions.Size = new System.Drawing.Size(208, 30);
            this.lblLeftOptions.TabIndex = 5;
            this.lblLeftOptions.Tag = "20003";
            this.lblLeftOptions.Text = "- Options";
            this.lblLeftOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLeftOptions.Click += new System.EventHandler(this.lblLeftUser_Click);
            // 
            // lblLeftUser
            // 
            this.lblLeftUser.BackColor = System.Drawing.Color.Transparent;
            this.lblLeftUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLeftUser.Font = new System.Drawing.Font("宋体", 10F);
            this.lblLeftUser.ForeColor = System.Drawing.Color.White;
            this.lblLeftUser.Location = new System.Drawing.Point(58, 372);
            this.lblLeftUser.Name = "lblLeftUser";
            this.lblLeftUser.Size = new System.Drawing.Size(208, 30);
            this.lblLeftUser.TabIndex = 5;
            this.lblLeftUser.Tag = "20002";
            this.lblLeftUser.Text = "- User";
            this.lblLeftUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLeftUser.Click += new System.EventHandler(this.lblLeftUser_Click);
            // 
            // mnuReview
            // 
            this.mnuReview.BackColor = System.Drawing.Color.Transparent;
            this.mnuReview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mnuReview.ItemBgColor = System.Drawing.Color.Transparent;
            this.mnuReview.ItemImage = global::WR.Client.UI.Properties.Resources.review;
            this.mnuReview.Location = new System.Drawing.Point(4, 121);
            this.mnuReview.Name = "mnuReview";
            this.mnuReview.Size = new System.Drawing.Size(255, 57);
            this.mnuReview.TabIndex = 4;
            this.mnuReview.Tag = "30002";
            this.mnuReview.WrText = "Wafer Review";
            this.mnuReview.ItemClick += new System.EventHandler(this.mnuSelect_ItemClick);
            // 
            // mnuSetting
            // 
            this.mnuSetting.BackColor = System.Drawing.Color.Transparent;
            this.mnuSetting.Cursor = System.Windows.Forms.Cursors.Default;
            this.mnuSetting.ItemBgColor = System.Drawing.Color.Transparent;
            this.mnuSetting.ItemImage = global::WR.Client.UI.Properties.Resources.settings;
            this.mnuSetting.Location = new System.Drawing.Point(4, 247);
            this.mnuSetting.Name = "mnuSetting";
            this.mnuSetting.Size = new System.Drawing.Size(255, 57);
            this.mnuSetting.TabIndex = 4;
            this.mnuSetting.Tag = "20000";
            this.mnuSetting.WrText = "System Settings";
            // 
            // mnuSelect
            // 
            this.mnuSelect.BackColor = System.Drawing.Color.Transparent;
            this.mnuSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mnuSelect.ItemBgColor = System.Drawing.Color.Transparent;
            this.mnuSelect.ItemImage = global::WR.Client.UI.Properties.Resources.select;
            this.mnuSelect.Location = new System.Drawing.Point(4, 184);
            this.mnuSelect.Name = "mnuSelect";
            this.mnuSelect.Size = new System.Drawing.Size(255, 57);
            this.mnuSelect.TabIndex = 4;
            this.mnuSelect.Tag = "30003";
            this.mnuSelect.WrText = "Defect Report";
            this.mnuSelect.ItemClick += new System.EventHandler(this.mnuSelect_ItemClick);
            // 
            // lblArrow
            // 
            this.lblArrow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblArrow.BackColor = System.Drawing.Color.Transparent;
            this.lblArrow.Image = global::WR.Client.UI.Properties.Resources.pleft;
            this.lblArrow.Location = new System.Drawing.Point(216, 2);
            this.lblArrow.Name = "lblArrow";
            this.lblArrow.Size = new System.Drawing.Size(16, 20);
            this.lblArrow.TabIndex = 3;
            this.lblArrow.Click += new System.EventHandler(this.lblArrow_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::WR.Client.UI.Properties.Resources.top2;
            this.panel1.Controls.Add(this.lblConnction);
            this.panel1.Controls.Add(this.lblHelp);
            this.panel1.Controls.Add(this.cbxLang);
            this.panel1.Controls.Add(this.picMin);
            this.panel1.Controls.Add(this.picClose);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 60);
            this.panel1.TabIndex = 0;
            // 
            // lblConnction
            // 
            this.lblConnction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnction.BackColor = System.Drawing.Color.Transparent;
            this.lblConnction.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblConnction.Image = global::WR.Client.UI.Properties.Resources.connection;
            this.lblConnction.Location = new System.Drawing.Point(1080, 21);
            this.lblConnction.Name = "lblConnction";
            this.lblConnction.Size = new System.Drawing.Size(25, 25);
            this.lblConnction.TabIndex = 4;
            this.lblConnction.Click += new System.EventHandler(this.lblConnction_Click);
            // 
            // lblHelp
            // 
            this.lblHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHelp.BackColor = System.Drawing.Color.Transparent;
            this.lblHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblHelp.Image = global::WR.Client.UI.Properties.Resources.help;
            this.lblHelp.Location = new System.Drawing.Point(1122, 20);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(24, 24);
            this.lblHelp.TabIndex = 4;
            this.lblHelp.Click += new System.EventHandler(this.lblHelp_Click);
            // 
            // cbxLang
            // 
            this.cbxLang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxLang.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbxLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLang.Enabled = false;
            this.cbxLang.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbxLang.FormattingEnabled = true;
            this.cbxLang.Items.AddRange(new object[] {
            "English",
            "Chinese"});
            this.cbxLang.Location = new System.Drawing.Point(938, 23);
            this.cbxLang.Name = "cbxLang";
            this.cbxLang.Size = new System.Drawing.Size(121, 20);
            this.cbxLang.TabIndex = 3;
            // 
            // picMin
            // 
            this.picMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picMin.BackColor = System.Drawing.Color.Transparent;
            this.picMin.Image = global::WR.Client.UI.Properties.Resources.Minimized;
            this.picMin.Location = new System.Drawing.Point(1164, 6);
            this.picMin.Name = "picMin";
            this.picMin.Size = new System.Drawing.Size(48, 48);
            this.picMin.TabIndex = 2;
            this.picMin.TabStop = false;
            this.picMin.Click += new System.EventHandler(this.picMin_Click);
            // 
            // picClose
            // 
            this.picClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picClose.BackColor = System.Drawing.Color.Transparent;
            this.picClose.Image = global::WR.Client.UI.Properties.Resources.close48;
            this.picClose.Location = new System.Drawing.Point(1226, 6);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(48, 48);
            this.picClose.TabIndex = 1;
            this.picClose.TabStop = false;
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::WR.Client.UI.Properties.Resources.top1;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(235, 60);
            this.panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::WR.Client.UI.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(5, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(62, 58);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Italic);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "         Wafer Review System";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(784, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "2015/08/12 09:30:23";
            // 
            // tlsConn
            // 
            this.tlsConn.Image = global::WR.Client.UI.Properties.Resources.connection;
            this.tlsConn.Name = "tlsConn";
            this.tlsConn.Size = new System.Drawing.Size(190, 22);
            this.tlsConn.Text = "Connection settings";
            this.tlsConn.Click += new System.EventHandler(this.tlsConn_Click);
            // 
            // tlsAbout
            // 
            this.tlsAbout.Image = global::WR.Client.UI.Properties.Resources.about;
            this.tlsAbout.Name = "tlsAbout";
            this.tlsAbout.Size = new System.Drawing.Size(190, 22);
            this.tlsAbout.Text = "About me";
            this.tlsAbout.Click += new System.EventHandler(this.tlsAbout_Click);
            // 
            // frm_main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 700);
            this.Controls.Add(this.pnlContext);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wafer Review";
            this.Load += new System.EventHandler(this.frm_main_Load);
            this.cnsHelp.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlContext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picClose;
        private System.Windows.Forms.PictureBox picMin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblArrow;
        private Controls.WrMenuItem mnuSetting;
        private System.Windows.Forms.Label lblLeftRole;
        private System.Windows.Forms.Label lblLeftUser;
        private Controls.WrMenuItem mnuSelection;
        private Controls.WrMenuItem mnuLogout;
        private System.Windows.Forms.Label lblLeftOptions;
        public Controls.WrMenuItem mnuReview;
        private System.Windows.Forms.ComboBox cbxLang;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblHelp;
        public Controls.WrMenuItem mnuSelect;
        private System.Windows.Forms.ContextMenuStrip cnsHelp;
        private System.Windows.Forms.ToolStripMenuItem tlsConn;
        private System.Windows.Forms.ToolStripMenuItem tlsAbout;
        private System.Windows.Forms.Label lblLeftPwd;
        private System.Windows.Forms.Label lblConnction;
    }
}