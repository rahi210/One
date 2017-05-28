namespace WR.Client.UI
{
    partial class frm_role
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdData = new CRD.WinUI.Editors.WrDataGridView();
            this.ColCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewImageColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdMenu = new CRD.WinUI.Editors.WrDataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlAddRole = new System.Windows.Forms.ToolStripButton();
            this.tlEdit = new System.Windows.Forms.ToolStripButton();
            this.tlDel = new System.Windows.Forms.ToolStripButton();
            this.tlRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tlName = new System.Windows.Forms.ToolStripTextBox();
            this.tlSearch = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MENUNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMenu)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grdData);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(801, 354);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Role List";
            // 
            // grdData
            // 
            this.grdData.AllowUserToAddRows = false;
            this.grdData.AllowUserToDeleteRows = false;
            this.grdData.AllowUserToOrderColumns = true;
            this.grdData.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(246)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdData.ColumnHeadersHeight = 26;
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColCode,
            this.ColName,
            this.ColComment,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column9});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Wheat;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.DarkSlateBlue;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdData.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.EnableHeadersVisualStyles = false;
            this.grdData.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdData.Location = new System.Drawing.Point(3, 17);
            this.grdData.MultiSelect = false;
            this.grdData.Name = "grdData";
            this.grdData.ReadOnly = true;
            this.grdData.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grdData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdData.RowTemplate.Height = 23;
            this.grdData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdData.Size = new System.Drawing.Size(795, 334);
            this.grdData.TabIndex = 0;
            this.grdData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdData_CellClick);
            this.grdData.SelectionChanged += new System.EventHandler(this.grdData_SelectionChanged);
            // 
            // ColCode
            // 
            this.ColCode.DataPropertyName = "ID";
            this.ColCode.HeaderText = "Code";
            this.ColCode.Name = "ColCode";
            this.ColCode.ReadOnly = true;
            this.ColCode.Width = 200;
            // 
            // ColName
            // 
            this.ColName.DataPropertyName = "ROLENAME";
            this.ColName.HeaderText = "Name";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            this.ColName.Width = 180;
            // 
            // ColComment
            // 
            this.ColComment.DataPropertyName = "REMARK";
            this.ColComment.HeaderText = "Comment";
            this.ColComment.Name = "ColComment";
            this.ColComment.ReadOnly = true;
            this.ColComment.Width = 300;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "UPDATEDATE";
            this.Column1.HeaderText = "UPDATEDATE";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "UPDATEID";
            this.Column2.HeaderText = "UPDATEID";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Visible = false;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "CREATEDATE";
            this.Column3.HeaderText = "CREATEDATE";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Visible = false;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "CREATEID";
            this.Column4.HeaderText = "CREATEID";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Visible = false;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Privilege";
            this.Column9.Image = global::WR.Client.UI.Properties.Resources.bindings;
            this.Column9.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.ToolTipText = "Binding Privilege";
            this.Column9.Width = 60;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdMenu);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(801, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(388, 354);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Menu List";
            // 
            // grdMenu
            // 
            this.grdMenu.AllowUserToAddRows = false;
            this.grdMenu.AllowUserToDeleteRows = false;
            this.grdMenu.AllowUserToOrderColumns = true;
            this.grdMenu.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdMenu.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(246)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdMenu.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grdMenu.ColumnHeadersHeight = 26;
            this.grdMenu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdMenu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.MENUNAME,
            this.Column5,
            this.Column8,
            this.Column7,
            this.Column10,
            this.Column11,
            this.Column12});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Wheat;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.DarkSlateBlue;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdMenu.DefaultCellStyle = dataGridViewCellStyle5;
            this.grdMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMenu.EnableHeadersVisualStyles = false;
            this.grdMenu.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdMenu.Location = new System.Drawing.Point(3, 17);
            this.grdMenu.Name = "grdMenu";
            this.grdMenu.ReadOnly = true;
            this.grdMenu.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdMenu.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.grdMenu.RowHeadersVisible = false;
            this.grdMenu.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdMenu.RowTemplate.Height = 23;
            this.grdMenu.Size = new System.Drawing.Size(382, 334);
            this.grdMenu.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlAddRole,
            this.tlEdit,
            this.tlDel,
            this.tlRefresh,
            this.toolStripSeparator1,
            this.tlName,
            this.tlSearch});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1189, 75);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlAddRole
            // 
            this.tlAddRole.Image = global::WR.Client.UI.Properties.Resources.AddUser;
            this.tlAddRole.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tlAddRole.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlAddRole.Name = "tlAddRole";
            this.tlAddRole.Size = new System.Drawing.Size(135, 72);
            this.tlAddRole.Text = "Create New Role";
            this.tlAddRole.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tlAddRole.Click += new System.EventHandler(this.tlAddRole_Click);
            // 
            // tlEdit
            // 
            this.tlEdit.Image = global::WR.Client.UI.Properties.Resources.EditUser;
            this.tlEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tlEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlEdit.Name = "tlEdit";
            this.tlEdit.Size = new System.Drawing.Size(104, 72);
            this.tlEdit.Text = "Update Role";
            this.tlEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tlEdit.Click += new System.EventHandler(this.tlEdit_Click);
            // 
            // tlDel
            // 
            this.tlDel.Image = global::WR.Client.UI.Properties.Resources.Delete;
            this.tlDel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tlDel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlDel.Name = "tlDel";
            this.tlDel.Size = new System.Drawing.Size(98, 72);
            this.tlDel.Text = "Delete Role";
            this.tlDel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tlDel.Click += new System.EventHandler(this.tlDel_Click);
            // 
            // tlRefresh
            // 
            this.tlRefresh.Image = global::WR.Client.UI.Properties.Resources.Refresh;
            this.tlRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tlRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlRefresh.Name = "tlRefresh";
            this.tlRefresh.Size = new System.Drawing.Size(68, 72);
            this.tlRefresh.Text = "Refresh";
            this.tlRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tlRefresh.Click += new System.EventHandler(this.tlRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 75);
            // 
            // tlName
            // 
            this.tlName.AutoSize = false;
            this.tlName.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.tlName.Margin = new System.Windows.Forms.Padding(4, 0, 1, 0);
            this.tlName.Name = "tlName";
            this.tlName.Size = new System.Drawing.Size(180, 75);
            this.tlName.Text = "Please input name";
            this.tlName.Enter += new System.EventHandler(this.tlName_Enter);
            this.tlName.Leave += new System.EventHandler(this.tlName_Leave);
            // 
            // tlSearch
            // 
            this.tlSearch.Image = global::WR.Client.UI.Properties.Resources.query;
            this.tlSearch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tlSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlSearch.Name = "tlSearch";
            this.tlSearch.Size = new System.Drawing.Size(62, 72);
            this.tlSearch.Text = "Search";
            this.tlSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tlSearch.Click += new System.EventHandler(this.tlSearch_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "Column9";
            this.dataGridViewImageColumn1.Image = global::WR.Client.UI.Properties.Resources.bindings;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "MENUCODE";
            this.Column6.HeaderText = "Code";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 70;
            // 
            // MENUNAME
            // 
            this.MENUNAME.DataPropertyName = "MENUNAME";
            this.MENUNAME.HeaderText = "Name";
            this.MENUNAME.Name = "MENUNAME";
            this.MENUNAME.ReadOnly = true;
            this.MENUNAME.Width = 160;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "ID";
            this.Column5.HeaderText = "ID";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Visible = false;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "REMARK";
            this.Column8.HeaderText = "Remark";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 160;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "UPDATEDATE";
            this.Column7.HeaderText = "UPDATEDATE";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Visible = false;
            // 
            // Column10
            // 
            this.Column10.DataPropertyName = "UPDATEID";
            this.Column10.HeaderText = "UPDATEID";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Visible = false;
            // 
            // Column11
            // 
            this.Column11.DataPropertyName = "CREATEDATE";
            this.Column11.HeaderText = "CREATEDATE";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.Visible = false;
            // 
            // Column12
            // 
            this.Column12.DataPropertyName = "CREATEID";
            this.Column12.HeaderText = "CREATEID";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            this.Column12.Visible = false;
            // 
            // frm_role
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1189, 429);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frm_role";
            this.Text = "frm_role";
            this.Load += new System.EventHandler(this.frm_role_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMenu)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CRD.WinUI.Editors.WrDataGridView grdData;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlAddRole;
        private System.Windows.Forms.ToolStripButton tlEdit;
        private System.Windows.Forms.ToolStripButton tlDel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tlRefresh;
        private System.Windows.Forms.ToolStripButton tlSearch;
        private System.Windows.Forms.ToolStripTextBox tlName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private CRD.WinUI.Editors.WrDataGridView grdMenu;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewImageColumn Column9;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn MENUNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
    }
}