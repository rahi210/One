namespace WR.Client.UI
{
    partial class frm_config
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fpnl = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.clbClass = new System.Windows.Forms.CheckedListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BtnClassRole = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbxDay = new System.Windows.Forms.CheckBox();
            this.dateTo = new System.Windows.Forms.DateTimePicker();
            this.nudDay = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxSpec = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxInterval = new System.Windows.Forms.CheckBox();
            this.cbxLast = new System.Windows.Forms.CheckBox();
            this.cbxFilter = new System.Windows.Forms.CheckBox();
            this.cbxNotdone = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSinfPath = new System.Windows.Forms.Button();
            this.txtSinfPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.nudWaferYield = new System.Windows.Forms.NumericUpDown();
            this.nudLotYield = new System.Windows.Forms.NumericUpDown();
            this.btnWaferYield = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnSystem = new System.Windows.Forms.Button();
            this.nudDisk = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDay)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWaferYield)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLotYield)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDisk)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fpnl);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 600);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "All Surface Rule";
            // 
            // fpnl
            // 
            this.fpnl.AutoScroll = true;
            this.fpnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpnl.Location = new System.Drawing.Point(3, 17);
            this.fpnl.Name = "fpnl";
            this.fpnl.Size = new System.Drawing.Size(408, 580);
            this.fpnl.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(414, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 600);
            this.panel1.TabIndex = 19;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 290);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(330, 310);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Classification Item Role";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(324, 290);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.clbClass);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(324, 290);
            this.panel4.TabIndex = 1;
            // 
            // clbClass
            // 
            this.clbClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbClass.FormattingEnabled = true;
            this.clbClass.Location = new System.Drawing.Point(0, 0);
            this.clbClass.Name = "clbClass";
            this.clbClass.Size = new System.Drawing.Size(324, 241);
            this.clbClass.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.BtnClassRole);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 241);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(324, 49);
            this.panel3.TabIndex = 2;
            // 
            // BtnClassRole
            // 
            this.BtnClassRole.BackgroundImage = global::WR.Client.UI.Properties.Resources.button;
            this.BtnClassRole.Image = global::WR.Client.UI.Properties.Resources.move24;
            this.BtnClassRole.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnClassRole.Location = new System.Drawing.Point(224, 7);
            this.BtnClassRole.Name = "BtnClassRole";
            this.BtnClassRole.Size = new System.Drawing.Size(80, 33);
            this.BtnClassRole.TabIndex = 14;
            this.BtnClassRole.Text = "    Save";
            this.BtnClassRole.UseVisualStyleBackColor = true;
            this.BtnClassRole.Click += new System.EventHandler(this.BtnClassRole_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(330, 290);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.cbxFilter);
            this.tabPage1.Controls.Add(this.cbxNotdone);
            this.tabPage1.Controls.Add(this.btnOK);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(322, 264);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Load Wafer Options";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxDay);
            this.groupBox2.Controls.Add(this.dateTo);
            this.groupBox2.Controls.Add(this.nudDay);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dtDate);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cbxSpec);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbxInterval);
            this.groupBox2.Controls.Add(this.cbxLast);
            this.groupBox2.Location = new System.Drawing.Point(21, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 155);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Date interval";
            // 
            // cbxDay
            // 
            this.cbxDay.AutoSize = true;
            this.cbxDay.Checked = true;
            this.cbxDay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxDay.Location = new System.Drawing.Point(6, 20);
            this.cbxDay.Name = "cbxDay";
            this.cbxDay.Size = new System.Drawing.Size(114, 16);
            this.cbxDay.TabIndex = 0;
            this.cbxDay.Text = "Data of the day";
            this.cbxDay.UseVisualStyleBackColor = true;
            this.cbxDay.CheckedChanged += new System.EventHandler(this.cbxDay_CheckedChanged);
            // 
            // dateTo
            // 
            this.dateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTo.Location = new System.Drawing.Point(180, 115);
            this.dateTo.Name = "dateTo";
            this.dateTo.Size = new System.Drawing.Size(85, 21);
            this.dateTo.TabIndex = 10;
            // 
            // nudDay
            // 
            this.nudDay.Location = new System.Drawing.Point(66, 66);
            this.nudDay.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.nudDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDay.Name = "nudDay";
            this.nudDay.Size = new System.Drawing.Size(54, 21);
            this.nudDay.TabIndex = 18;
            this.nudDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(155, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "to:";
            // 
            // dtDate
            // 
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDate.Location = new System.Drawing.Point(45, 115);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(100, 21);
            this.dtDate.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(126, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "day";
            // 
            // cbxSpec
            // 
            this.cbxSpec.AutoSize = true;
            this.cbxSpec.Location = new System.Drawing.Point(6, 93);
            this.cbxSpec.Name = "cbxSpec";
            this.cbxSpec.Size = new System.Drawing.Size(108, 16);
            this.cbxSpec.TabIndex = 0;
            this.cbxSpec.Text = "Specified date";
            this.cbxSpec.UseVisualStyleBackColor = true;
            this.cbxSpec.CheckedChanged += new System.EventHandler(this.cbxSpec_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "from:";
            // 
            // cbxInterval
            // 
            this.cbxInterval.AutoSize = true;
            this.cbxInterval.Location = new System.Drawing.Point(6, 67);
            this.cbxInterval.Name = "cbxInterval";
            this.cbxInterval.Size = new System.Drawing.Size(60, 16);
            this.cbxInterval.TabIndex = 15;
            this.cbxInterval.Text = "Latest";
            this.cbxInterval.UseVisualStyleBackColor = true;
            this.cbxInterval.CheckedChanged += new System.EventHandler(this.cbxInterval_CheckedChanged);
            // 
            // cbxLast
            // 
            this.cbxLast.AutoSize = true;
            this.cbxLast.Location = new System.Drawing.Point(6, 43);
            this.cbxLast.Name = "cbxLast";
            this.cbxLast.Size = new System.Drawing.Size(108, 16);
            this.cbxLast.TabIndex = 0;
            this.cbxLast.Text = "Latest weekly ";
            this.cbxLast.UseVisualStyleBackColor = true;
            this.cbxLast.CheckedChanged += new System.EventHandler(this.cbxLast_CheckedChanged);
            // 
            // cbxFilter
            // 
            this.cbxFilter.AutoSize = true;
            this.cbxFilter.Checked = true;
            this.cbxFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxFilter.Location = new System.Drawing.Point(21, 34);
            this.cbxFilter.Name = "cbxFilter";
            this.cbxFilter.Size = new System.Drawing.Size(168, 16);
            this.cbxFilter.TabIndex = 19;
            this.cbxFilter.Text = "Filtering Duplicate data";
            this.cbxFilter.UseVisualStyleBackColor = true;
            // 
            // cbxNotdone
            // 
            this.cbxNotdone.AutoSize = true;
            this.cbxNotdone.Checked = true;
            this.cbxNotdone.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxNotdone.Location = new System.Drawing.Point(21, 12);
            this.cbxNotdone.Name = "cbxNotdone";
            this.cbxNotdone.Size = new System.Drawing.Size(192, 16);
            this.cbxNotdone.TabIndex = 0;
            this.cbxNotdone.Text = "Not been done of the history";
            this.cbxNotdone.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::WR.Client.UI.Properties.Resources.button;
            this.btnOK.Image = global::WR.Client.UI.Properties.Resources.move24;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(223, 223);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 33);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "    Save";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnSinfPath);
            this.tabPage2.Controls.Add(this.txtSinfPath);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(322, 264);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SINF Options";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnSinfPath
            // 
            this.btnSinfPath.Location = new System.Drawing.Point(276, 14);
            this.btnSinfPath.Name = "btnSinfPath";
            this.btnSinfPath.Size = new System.Drawing.Size(27, 23);
            this.btnSinfPath.TabIndex = 2;
            this.btnSinfPath.Text = "..";
            this.btnSinfPath.UseVisualStyleBackColor = true;
            this.btnSinfPath.Click += new System.EventHandler(this.btnSinfPath_Click);
            // 
            // txtSinfPath
            // 
            this.txtSinfPath.Location = new System.Drawing.Point(119, 16);
            this.txtSinfPath.Name = "txtSinfPath";
            this.txtSinfPath.Size = new System.Drawing.Size(161, 21);
            this.txtSinfPath.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "Export Directory:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.nudWaferYield);
            this.tabPage3.Controls.Add(this.nudLotYield);
            this.tabPage3.Controls.Add(this.btnWaferYield);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(322, 264);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Wafer Options";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // nudWaferYield
            // 
            this.nudWaferYield.Location = new System.Drawing.Point(100, 43);
            this.nudWaferYield.Name = "nudWaferYield";
            this.nudWaferYield.Size = new System.Drawing.Size(94, 21);
            this.nudWaferYield.TabIndex = 17;
            // 
            // nudLotYield
            // 
            this.nudLotYield.Location = new System.Drawing.Point(100, 16);
            this.nudLotYield.Name = "nudLotYield";
            this.nudLotYield.Size = new System.Drawing.Size(94, 21);
            this.nudLotYield.TabIndex = 16;
            // 
            // btnWaferYield
            // 
            this.btnWaferYield.BackgroundImage = global::WR.Client.UI.Properties.Resources.button;
            this.btnWaferYield.Image = global::WR.Client.UI.Properties.Resources.move24;
            this.btnWaferYield.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWaferYield.Location = new System.Drawing.Point(223, 225);
            this.btnWaferYield.Name = "btnWaferYield";
            this.btnWaferYield.Size = new System.Drawing.Size(80, 33);
            this.btnWaferYield.TabIndex = 15;
            this.btnWaferYield.Text = "    Save";
            this.btnWaferYield.UseVisualStyleBackColor = true;
            this.btnWaferYield.Click += new System.EventHandler(this.btnWaferYield_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "Wafer Yield:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Lot Yield:";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnSystem);
            this.tabPage4.Controls.Add(this.nudDisk);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(322, 264);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "System Options";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnSystem
            // 
            this.btnSystem.BackgroundImage = global::WR.Client.UI.Properties.Resources.button;
            this.btnSystem.Image = global::WR.Client.UI.Properties.Resources.move24;
            this.btnSystem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSystem.Location = new System.Drawing.Point(200, 205);
            this.btnSystem.Name = "btnSystem";
            this.btnSystem.Size = new System.Drawing.Size(80, 33);
            this.btnSystem.TabIndex = 19;
            this.btnSystem.Text = "    Save";
            this.btnSystem.UseVisualStyleBackColor = true;
            this.btnSystem.Click += new System.EventHandler(this.btnSystem_Click);
            // 
            // nudDisk
            // 
            this.nudDisk.Location = new System.Drawing.Point(154, 27);
            this.nudDisk.Name = "nudDisk";
            this.nudDisk.Size = new System.Drawing.Size(70, 21);
            this.nudDisk.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "Disk Alert Value(%):";
            // 
            // frm_config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 600);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "frm_config";
            this.Text = "frm_config";
            this.Load += new System.EventHandler(this.frm_config_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDay)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWaferYield)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLotYield)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDisk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel fpnl;
        private System.Windows.Forms.CheckBox cbxSpec;
        private System.Windows.Forms.CheckBox cbxLast;
        private System.Windows.Forms.CheckBox cbxDay;
        private System.Windows.Forms.CheckBox cbxNotdone;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTo;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbxInterval;
        private System.Windows.Forms.NumericUpDown nudDay;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button BtnClassRole;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckedListBox clbClass;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSinfPath;
        private System.Windows.Forms.TextBox txtSinfPath;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnWaferYield;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudLotYield;
        private System.Windows.Forms.NumericUpDown nudWaferYield;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnSystem;
        private System.Windows.Forms.NumericUpDown nudDisk;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbxFilter;
        private System.Windows.Forms.GroupBox groupBox2;

    }
}