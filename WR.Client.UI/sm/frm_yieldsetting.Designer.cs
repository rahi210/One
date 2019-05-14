namespace WR.Client.UI
{
    partial class frm_yieldsetting
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlAdd = new System.Windows.Forms.ToolStripButton();
            this.tlEdit = new System.Windows.Forms.ToolStripButton();
            this.tlDel = new System.Windows.Forms.ToolStripButton();
            this.tlRefresh = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdData = new CRD.WinUI.Editors.WrDataGridView();
            this.RECIPE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOT_YIELD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WAFER_YIELD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maskayield = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maskbyield = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maskcyield = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maskdyield = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maskeyield = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlAdd,
            this.tlEdit,
            this.tlDel,
            this.tlRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(759, 75);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlAdd
            // 
            this.tlAdd.Image = global::WR.Client.UI.Properties.Resources.AddUser;
            this.tlAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tlAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlAdd.Name = "tlAdd";
            this.tlAdd.Size = new System.Drawing.Size(61, 72);
            this.tlAdd.Text = "Create";
            this.tlAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tlAdd.Click += new System.EventHandler(this.tlAdd_Click);
            // 
            // tlEdit
            // 
            this.tlEdit.Image = global::WR.Client.UI.Properties.Resources.EditUser;
            this.tlEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tlEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlEdit.Name = "tlEdit";
            this.tlEdit.Size = new System.Drawing.Size(67, 72);
            this.tlEdit.Text = "Update";
            this.tlEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tlEdit.Click += new System.EventHandler(this.tlEdit_Click);
            // 
            // tlDel
            // 
            this.tlDel.Image = global::WR.Client.UI.Properties.Resources.Delete;
            this.tlDel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tlDel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlDel.Name = "tlDel";
            this.tlDel.Size = new System.Drawing.Size(61, 72);
            this.tlDel.Text = "Delete";
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
            // panel1
            // 
            this.panel1.Controls.Add(this.grdData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(759, 449);
            this.panel1.TabIndex = 12;
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
            this.RECIPE_ID,
            this.Type,
            this.LOT_YIELD,
            this.WAFER_YIELD,
            this.maskayield,
            this.maskbyield,
            this.maskcyield,
            this.maskdyield,
            this.maskeyield});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.Wheat;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.DarkSlateBlue;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdData.DefaultCellStyle = dataGridViewCellStyle9;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.EnableHeadersVisualStyles = false;
            this.grdData.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdData.Location = new System.Drawing.Point(0, 0);
            this.grdData.MultiSelect = false;
            this.grdData.Name = "grdData";
            this.grdData.ReadOnly = true;
            this.grdData.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdData.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.grdData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdData.RowTemplate.Height = 23;
            this.grdData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdData.Size = new System.Drawing.Size(759, 449);
            this.grdData.TabIndex = 1;
            this.grdData.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdData_CellFormatting);
            // 
            // RECIPE_ID
            // 
            this.RECIPE_ID.DataPropertyName = "RECIPE_ID";
            this.RECIPE_ID.HeaderText = "ID";
            this.RECIPE_ID.Name = "RECIPE_ID";
            this.RECIPE_ID.ReadOnly = true;
            this.RECIPE_ID.Width = 200;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "YIELD_TYPE";
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // LOT_YIELD
            // 
            this.LOT_YIELD.DataPropertyName = "LOT_YIELD";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "0";
            this.LOT_YIELD.DefaultCellStyle = dataGridViewCellStyle2;
            this.LOT_YIELD.HeaderText = "Lot Yield";
            this.LOT_YIELD.Name = "LOT_YIELD";
            this.LOT_YIELD.ReadOnly = true;
            this.LOT_YIELD.Width = 150;
            // 
            // WAFER_YIELD
            // 
            this.WAFER_YIELD.DataPropertyName = "WAFER_YIELD";
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = "0";
            this.WAFER_YIELD.DefaultCellStyle = dataGridViewCellStyle3;
            this.WAFER_YIELD.HeaderText = "Wafer Yield";
            this.WAFER_YIELD.Name = "WAFER_YIELD";
            this.WAFER_YIELD.ReadOnly = true;
            this.WAFER_YIELD.Width = 150;
            // 
            // maskayield
            // 
            this.maskayield.DataPropertyName = "MASKA_YIELD";
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = "0";
            this.maskayield.DefaultCellStyle = dataGridViewCellStyle4;
            this.maskayield.HeaderText = "A Yield";
            this.maskayield.Name = "maskayield";
            this.maskayield.ReadOnly = true;
            // 
            // maskbyield
            // 
            this.maskbyield.DataPropertyName = "MASKB_YIELD";
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = "0";
            this.maskbyield.DefaultCellStyle = dataGridViewCellStyle5;
            this.maskbyield.HeaderText = "B Yield";
            this.maskbyield.Name = "maskbyield";
            this.maskbyield.ReadOnly = true;
            // 
            // maskcyield
            // 
            this.maskcyield.DataPropertyName = "MASKC_YIELD";
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = "0";
            this.maskcyield.DefaultCellStyle = dataGridViewCellStyle6;
            this.maskcyield.HeaderText = "C Yield";
            this.maskcyield.Name = "maskcyield";
            this.maskcyield.ReadOnly = true;
            // 
            // maskdyield
            // 
            this.maskdyield.DataPropertyName = "MASKD_YIELD";
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = "0";
            this.maskdyield.DefaultCellStyle = dataGridViewCellStyle7;
            this.maskdyield.HeaderText = "D Yield";
            this.maskdyield.Name = "maskdyield";
            this.maskdyield.ReadOnly = true;
            // 
            // maskeyield
            // 
            this.maskeyield.DataPropertyName = "MASKE_YIELD";
            dataGridViewCellStyle8.Format = "N2";
            dataGridViewCellStyle8.NullValue = "0";
            this.maskeyield.DefaultCellStyle = dataGridViewCellStyle8;
            this.maskeyield.HeaderText = "E Yield";
            this.maskeyield.Name = "maskeyield";
            this.maskeyield.ReadOnly = true;
            // 
            // frm_yieldsetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 524);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frm_yieldsetting";
            this.Text = "Yield Setting";
            this.Load += new System.EventHandler(this.frm_yieldsetting_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlAdd;
        private System.Windows.Forms.ToolStripButton tlEdit;
        private System.Windows.Forms.ToolStripButton tlDel;
        private System.Windows.Forms.ToolStripButton tlRefresh;
        private System.Windows.Forms.Panel panel1;
        private CRD.WinUI.Editors.WrDataGridView grdData;
        private System.Windows.Forms.DataGridViewTextBoxColumn RECIPE_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOT_YIELD;
        private System.Windows.Forms.DataGridViewTextBoxColumn WAFER_YIELD;
        private System.Windows.Forms.DataGridViewTextBoxColumn maskayield;
        private System.Windows.Forms.DataGridViewTextBoxColumn maskbyield;
        private System.Windows.Forms.DataGridViewTextBoxColumn maskcyield;
        private System.Windows.Forms.DataGridViewTextBoxColumn maskdyield;
        private System.Windows.Forms.DataGridViewTextBoxColumn maskeyield;
    }
}