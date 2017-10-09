namespace WR.Client.UI
{
    partial class frm_Archive
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.chartDisk = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbxDisk = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDbExp = new System.Windows.Forms.Button();
            this.cbxFiles = new System.Windows.Forms.ComboBox();
            this.btnDbImp = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnTableSpaceAdd = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpDless = new System.Windows.Forms.DateTimePicker();
            this.rbtDSpecified = new System.Windows.Forms.RadioButton();
            this.dtpDfrom = new System.Windows.Forms.DateTimePicker();
            this.rbtDless = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpDto = new System.Windows.Forms.DateTimePicker();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtpRless = new System.Windows.Forms.DateTimePicker();
            this.rbtRSpecified = new System.Windows.Forms.RadioButton();
            this.dtpRfrom = new System.Windows.Forms.DateTimePicker();
            this.rbtRless = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpRto = new System.Windows.Forms.DateTimePicker();
            this.btnRecovery = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtArchiveDate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpAless = new System.Windows.Forms.DateTimePicker();
            this.rbtASpecified = new System.Windows.Forms.RadioButton();
            this.dtpAfrom = new System.Windows.Forms.DateTimePicker();
            this.rbtAless = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.btnArchive = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpAto = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDisk)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 762);
            this.panel1.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 590);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(984, 172);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.chartDisk);
            this.groupBox7.Controls.Add(this.panel2);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox7.Location = new System.Drawing.Point(3, 17);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(488, 152);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Disk";
            // 
            // chartDisk
            // 
            chartArea1.Name = "ChartArea1";
            this.chartDisk.ChartAreas.Add(chartArea1);
            this.chartDisk.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartDisk.Legends.Add(legend1);
            this.chartDisk.Location = new System.Drawing.Point(3, 55);
            this.chartDisk.Name = "chartDisk";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartDisk.Series.Add(series1);
            this.chartDisk.Size = new System.Drawing.Size(482, 94);
            this.chartDisk.TabIndex = 6;
            this.chartDisk.Text = "chart1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbxDisk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(482, 38);
            this.panel2.TabIndex = 5;
            // 
            // cbxDisk
            // 
            this.cbxDisk.FormattingEnabled = true;
            this.cbxDisk.Location = new System.Drawing.Point(23, 12);
            this.cbxDisk.Name = "cbxDisk";
            this.cbxDisk.Size = new System.Drawing.Size(75, 20);
            this.cbxDisk.TabIndex = 4;
            this.cbxDisk.SelectedIndexChanged += new System.EventHandler(this.cbxDisk_SelectedIndexChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.btnDbExp);
            this.groupBox5.Controls.Add(this.cbxFiles);
            this.groupBox5.Controls.Add(this.btnDbImp);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(0, 490);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(984, 100);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Database Recovery";
            this.groupBox5.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(160, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "Backup Files：";
            // 
            // btnDbExp
            // 
            this.btnDbExp.Image = global::WR.Client.UI.Properties.Resources.database_download;
            this.btnDbExp.Location = new System.Drawing.Point(527, 29);
            this.btnDbExp.Name = "btnDbExp";
            this.btnDbExp.Size = new System.Drawing.Size(75, 55);
            this.btnDbExp.TabIndex = 2;
            this.btnDbExp.UseVisualStyleBackColor = true;
            this.btnDbExp.Visible = false;
            this.btnDbExp.Click += new System.EventHandler(this.btnDbExp_Click);
            // 
            // cbxFiles
            // 
            this.cbxFiles.FormattingEnabled = true;
            this.cbxFiles.Location = new System.Drawing.Point(249, 35);
            this.cbxFiles.Name = "cbxFiles";
            this.cbxFiles.Size = new System.Drawing.Size(242, 20);
            this.cbxFiles.TabIndex = 3;
            // 
            // btnDbImp
            // 
            this.btnDbImp.Image = global::WR.Client.UI.Properties.Resources.database_upload;
            this.btnDbImp.Location = new System.Drawing.Point(29, 29);
            this.btnDbImp.Name = "btnDbImp";
            this.btnDbImp.Size = new System.Drawing.Size(75, 55);
            this.btnDbImp.TabIndex = 2;
            this.btnDbImp.UseVisualStyleBackColor = true;
            this.btnDbImp.Click += new System.EventHandler(this.btnDbImp_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.btnTableSpaceAdd);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 390);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(984, 100);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Database Manage";
            // 
            // btnTableSpaceAdd
            // 
            this.btnTableSpaceAdd.Image = global::WR.Client.UI.Properties.Resources.tablespace;
            this.btnTableSpaceAdd.Location = new System.Drawing.Point(29, 30);
            this.btnTableSpaceAdd.Name = "btnTableSpaceAdd";
            this.btnTableSpaceAdd.Size = new System.Drawing.Size(75, 55);
            this.btnTableSpaceAdd.TabIndex = 3;
            this.btnTableSpaceAdd.UseVisualStyleBackColor = true;
            this.btnTableSpaceAdd.Click += new System.EventHandler(this.btnTableSpaceAdd_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.dtpDless);
            this.groupBox3.Controls.Add(this.rbtDSpecified);
            this.groupBox3.Controls.Add(this.dtpDfrom);
            this.groupBox3.Controls.Add(this.rbtDless);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.dtpDto);
            this.groupBox3.Controls.Add(this.btnDelete);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 260);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(984, 130);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Data Delete";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(525, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(278, 12);
            this.label8.TabIndex = 28;
            this.label8.Text = "Notes:This action cannot be rolled back";
            // 
            // dtpDless
            // 
            this.dtpDless.Location = new System.Drawing.Point(216, 42);
            this.dtpDless.Name = "dtpDless";
            this.dtpDless.Size = new System.Drawing.Size(114, 21);
            this.dtpDless.TabIndex = 27;
            // 
            // rbtDSpecified
            // 
            this.rbtDSpecified.AutoSize = true;
            this.rbtDSpecified.Location = new System.Drawing.Point(162, 68);
            this.rbtDSpecified.Name = "rbtDSpecified";
            this.rbtDSpecified.Size = new System.Drawing.Size(107, 16);
            this.rbtDSpecified.TabIndex = 25;
            this.rbtDSpecified.Text = "Specified date";
            this.rbtDSpecified.UseVisualStyleBackColor = true;
            this.rbtDSpecified.CheckedChanged += new System.EventHandler(this.rbt_CheckedChanged);
            // 
            // dtpDfrom
            // 
            this.dtpDfrom.Location = new System.Drawing.Point(216, 90);
            this.dtpDfrom.Name = "dtpDfrom";
            this.dtpDfrom.Size = new System.Drawing.Size(114, 21);
            this.dtpDfrom.TabIndex = 21;
            // 
            // rbtDless
            // 
            this.rbtDless.AutoSize = true;
            this.rbtDless.Checked = true;
            this.rbtDless.Location = new System.Drawing.Point(162, 20);
            this.rbtDless.Name = "rbtDless";
            this.rbtDless.Size = new System.Drawing.Size(107, 16);
            this.rbtDless.TabIndex = 26;
            this.rbtDless.TabStop = true;
            this.rbtDless.Text = "Less than date";
            this.rbtDless.UseVisualStyleBackColor = true;
            this.rbtDless.CheckedChanged += new System.EventHandler(this.rbt_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(350, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "to:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(178, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 22;
            this.label6.Text = "from:";
            // 
            // dtpDto
            // 
            this.dtpDto.Location = new System.Drawing.Point(377, 90);
            this.dtpDto.Name = "dtpDto";
            this.dtpDto.Size = new System.Drawing.Size(114, 21);
            this.dtpDto.TabIndex = 23;
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::WR.Client.UI.Properties.Resources.database_delete;
            this.btnDelete.Location = new System.Drawing.Point(29, 42);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 55);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpRless);
            this.groupBox2.Controls.Add(this.rbtRSpecified);
            this.groupBox2.Controls.Add(this.dtpRfrom);
            this.groupBox2.Controls.Add(this.rbtRless);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dtpRto);
            this.groupBox2.Controls.Add(this.btnRecovery);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(984, 130);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Recovery";
            // 
            // dtpRless
            // 
            this.dtpRless.Location = new System.Drawing.Point(216, 42);
            this.dtpRless.Name = "dtpRless";
            this.dtpRless.Size = new System.Drawing.Size(114, 21);
            this.dtpRless.TabIndex = 27;
            // 
            // rbtRSpecified
            // 
            this.rbtRSpecified.AutoSize = true;
            this.rbtRSpecified.Location = new System.Drawing.Point(162, 68);
            this.rbtRSpecified.Name = "rbtRSpecified";
            this.rbtRSpecified.Size = new System.Drawing.Size(107, 16);
            this.rbtRSpecified.TabIndex = 25;
            this.rbtRSpecified.Text = "Specified date";
            this.rbtRSpecified.UseVisualStyleBackColor = true;
            this.rbtRSpecified.CheckedChanged += new System.EventHandler(this.rbt_CheckedChanged);
            // 
            // dtpRfrom
            // 
            this.dtpRfrom.Location = new System.Drawing.Point(216, 90);
            this.dtpRfrom.Name = "dtpRfrom";
            this.dtpRfrom.Size = new System.Drawing.Size(114, 21);
            this.dtpRfrom.TabIndex = 21;
            // 
            // rbtRless
            // 
            this.rbtRless.AutoSize = true;
            this.rbtRless.Checked = true;
            this.rbtRless.Location = new System.Drawing.Point(162, 20);
            this.rbtRless.Name = "rbtRless";
            this.rbtRless.Size = new System.Drawing.Size(107, 16);
            this.rbtRless.TabIndex = 26;
            this.rbtRless.TabStop = true;
            this.rbtRless.Text = "Less than date";
            this.rbtRless.UseVisualStyleBackColor = true;
            this.rbtRless.CheckedChanged += new System.EventHandler(this.rbt_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(350, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 24;
            this.label3.Text = "to:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(178, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "from:";
            // 
            // dtpRto
            // 
            this.dtpRto.Location = new System.Drawing.Point(377, 90);
            this.dtpRto.Name = "dtpRto";
            this.dtpRto.Size = new System.Drawing.Size(114, 21);
            this.dtpRto.TabIndex = 23;
            // 
            // btnRecovery
            // 
            this.btnRecovery.Image = global::WR.Client.UI.Properties.Resources.export;
            this.btnRecovery.Location = new System.Drawing.Point(29, 42);
            this.btnRecovery.Name = "btnRecovery";
            this.btnRecovery.Size = new System.Drawing.Size(75, 55);
            this.btnRecovery.TabIndex = 1;
            this.btnRecovery.UseVisualStyleBackColor = true;
            this.btnRecovery.Click += new System.EventHandler(this.btnRecovery_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtArchiveDate);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.dtpAless);
            this.groupBox1.Controls.Add(this.rbtASpecified);
            this.groupBox1.Controls.Add(this.dtpAfrom);
            this.groupBox1.Controls.Add(this.rbtAless);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnArchive);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpAto);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 130);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Archive";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(541, 93);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(376, 12);
            this.label10.TabIndex = 29;
            this.label10.Text = "Notes:It\'s best to archive once a day, or once a week";
            // 
            // txtArchiveDate
            // 
            this.txtArchiveDate.Enabled = false;
            this.txtArchiveDate.Location = new System.Drawing.Point(664, 43);
            this.txtArchiveDate.Name = "txtArchiveDate";
            this.txtArchiveDate.Size = new System.Drawing.Size(116, 21);
            this.txtArchiveDate.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(541, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 12);
            this.label9.TabIndex = 21;
            this.label9.Text = "Last Archived date:";
            // 
            // dtpAless
            // 
            this.dtpAless.Location = new System.Drawing.Point(216, 42);
            this.dtpAless.Name = "dtpAless";
            this.dtpAless.Size = new System.Drawing.Size(114, 21);
            this.dtpAless.TabIndex = 20;
            // 
            // rbtASpecified
            // 
            this.rbtASpecified.AutoSize = true;
            this.rbtASpecified.Location = new System.Drawing.Point(162, 68);
            this.rbtASpecified.Name = "rbtASpecified";
            this.rbtASpecified.Size = new System.Drawing.Size(107, 16);
            this.rbtASpecified.TabIndex = 18;
            this.rbtASpecified.Text = "Specified date";
            this.rbtASpecified.UseVisualStyleBackColor = true;
            this.rbtASpecified.CheckedChanged += new System.EventHandler(this.rbt_CheckedChanged);
            // 
            // dtpAfrom
            // 
            this.dtpAfrom.Location = new System.Drawing.Point(216, 90);
            this.dtpAfrom.Name = "dtpAfrom";
            this.dtpAfrom.Size = new System.Drawing.Size(114, 21);
            this.dtpAfrom.TabIndex = 1;
            // 
            // rbtAless
            // 
            this.rbtAless.AutoSize = true;
            this.rbtAless.Checked = true;
            this.rbtAless.Location = new System.Drawing.Point(162, 20);
            this.rbtAless.Name = "rbtAless";
            this.rbtAless.Size = new System.Drawing.Size(107, 16);
            this.rbtAless.TabIndex = 19;
            this.rbtAless.TabStop = true;
            this.rbtAless.Text = "Less than date";
            this.rbtAless.UseVisualStyleBackColor = true;
            this.rbtAless.CheckedChanged += new System.EventHandler(this.rbt_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(350, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "to:";
            // 
            // btnArchive
            // 
            this.btnArchive.Image = global::WR.Client.UI.Properties.Resources.archive_c;
            this.btnArchive.Location = new System.Drawing.Point(29, 42);
            this.btnArchive.Name = "btnArchive";
            this.btnArchive.Size = new System.Drawing.Size(75, 55);
            this.btnArchive.TabIndex = 0;
            this.btnArchive.UseVisualStyleBackColor = true;
            this.btnArchive.Click += new System.EventHandler(this.btnArchive_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(178, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "from:";
            // 
            // dtpAto
            // 
            this.dtpAto.Location = new System.Drawing.Point(377, 90);
            this.dtpAto.Name = "dtpAto";
            this.dtpAto.Size = new System.Drawing.Size(114, 21);
            this.dtpAto.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(160, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(300, 55);
            this.label11.TabIndex = 29;
            this.label11.Text = "Description: Create a tablespace file. Notes:This action cannot be rolled back.";
            // 
            // frm_Archive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 762);
            this.Controls.Add(this.panel1);
            this.Name = "frm_Archive";
            this.Text = "frmArchive";
            this.Load += new System.EventHandler(this.frm_Archive_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDisk)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnArchive;
        private System.Windows.Forms.Button btnRecovery;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpAfrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpAto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbtASpecified;
        private System.Windows.Forms.DateTimePicker dtpRless;
        private System.Windows.Forms.RadioButton rbtRSpecified;
        private System.Windows.Forms.DateTimePicker dtpRfrom;
        private System.Windows.Forms.RadioButton rbtRless;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpRto;
        private System.Windows.Forms.DateTimePicker dtpAless;
        private System.Windows.Forms.RadioButton rbtAless;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DateTimePicker dtpDless;
        private System.Windows.Forms.RadioButton rbtDSpecified;
        private System.Windows.Forms.DateTimePicker dtpDfrom;
        private System.Windows.Forms.RadioButton rbtDless;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpDto;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnDbExp;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnDbImp;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbxFiles;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ComboBox cbxDisk;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDisk;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtArchiveDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnTableSpaceAdd;
        private System.Windows.Forms.Label label11;
    }
}