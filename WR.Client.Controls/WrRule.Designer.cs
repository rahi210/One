namespace WR.Client.Controls
{
    partial class WrRule
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDel = new System.Windows.Forms.LinkLabel();
            this.lblEdit = new System.Windows.Forms.LinkLabel();
            this.label12 = new System.Windows.Forms.Label();
            this.lblStepup = new System.Windows.Forms.Label();
            this.lblDevice = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblDesrp = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblRule = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblInst = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDel
            // 
            this.lblDel.AutoSize = true;
            this.lblDel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblDel.Location = new System.Drawing.Point(255, 146);
            this.lblDel.Name = "lblDel";
            this.lblDel.Size = new System.Drawing.Size(53, 12);
            this.lblDel.TabIndex = 12;
            this.lblDel.TabStop = true;
            this.lblDel.Text = "[Delete]";
            this.lblDel.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblDel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDel_LinkClicked);
            // 
            // lblEdit
            // 
            this.lblEdit.AutoSize = true;
            this.lblEdit.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblEdit.Location = new System.Drawing.Point(208, 146);
            this.lblEdit.Name = "lblEdit";
            this.lblEdit.Size = new System.Drawing.Size(41, 12);
            this.lblEdit.TabIndex = 13;
            this.lblEdit.TabStop = true;
            this.lblEdit.Text = "[Edit]";
            this.lblEdit.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblEdit_LinkClicked);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(43, 107);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 11;
            this.label12.Text = "SetupID:";
            // 
            // lblStepup
            // 
            this.lblStepup.AutoSize = true;
            this.lblStepup.Location = new System.Drawing.Point(104, 107);
            this.lblStepup.Name = "lblStepup";
            this.lblStepup.Size = new System.Drawing.Size(119, 12);
            this.lblStepup.TabIndex = 10;
            this.lblStepup.Text = "TL-SLL-LSN240XA2(L)";
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(104, 81);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(23, 12);
            this.lblDevice.TabIndex = 3;
            this.lblDevice.Text = "UBM";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(37, 81);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 12);
            this.label16.TabIndex = 2;
            this.label16.Text = "DeviceID:";
            // 
            // lblDesrp
            // 
            this.lblDesrp.AutoSize = true;
            this.lblDesrp.Location = new System.Drawing.Point(104, 47);
            this.lblDesrp.Name = "lblDesrp";
            this.lblDesrp.Size = new System.Drawing.Size(65, 12);
            this.lblDesrp.TabIndex = 4;
            this.lblDesrp.Text = "wafer list";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(19, 47);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(77, 12);
            this.label18.TabIndex = 6;
            this.label18.Text = "Description:";
            // 
            // lblRule
            // 
            this.lblRule.AutoSize = true;
            this.lblRule.Location = new System.Drawing.Point(104, 24);
            this.lblRule.Name = "lblRule";
            this.lblRule.Size = new System.Drawing.Size(77, 12);
            this.lblRule.TabIndex = 5;
            this.lblRule.Text = "WaferResults";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(61, 24);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(35, 12);
            this.label20.TabIndex = 7;
            this.label20.Text = "Rule:";
            // 
            // lblInst
            // 
            this.lblInst.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblInst.Image = global::WR.Client.Controls.Properties.Resources.AddUser;
            this.lblInst.Location = new System.Drawing.Point(133, 59);
            this.lblInst.Name = "lblInst";
            this.lblInst.Size = new System.Drawing.Size(68, 46);
            this.lblInst.TabIndex = 15;
            this.lblInst.Text = "       ";
            this.lblInst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblInst.Click += new System.EventHandler(this.label1_Click);
            // 
            // WrRule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblInst);
            this.Controls.Add(this.lblDel);
            this.Controls.Add(this.lblEdit);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblStepup);
            this.Controls.Add(this.lblDevice);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.lblDesrp);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.lblRule);
            this.Controls.Add(this.label20);
            this.Name = "WrRule";
            this.Size = new System.Drawing.Size(326, 178);
            this.Load += new System.EventHandler(this.WrRule_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lblDel;
        private System.Windows.Forms.LinkLabel lblEdit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblStepup;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblDesrp;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblRule;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblInst;
    }
}
