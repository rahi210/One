namespace WR.Client.Controls
{
    partial class WrMenuItem
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
            this.lblItem = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblItem
            // 
            this.lblItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblItem.BackColor = System.Drawing.Color.Transparent;
            this.lblItem.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.lblItem.ForeColor = System.Drawing.Color.White;
            this.lblItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblItem.Location = new System.Drawing.Point(8, 8);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(235, 42);
            this.lblItem.TabIndex = 1;
            this.lblItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblItem.Click += new System.EventHandler(this.lblItem_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Silver;
            this.label5.Location = new System.Drawing.Point(9, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 1);
            this.label5.TabIndex = 2;
            // 
            // WrMenuItem
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblItem);
            this.Name = "WrMenuItem";
            this.Size = new System.Drawing.Size(240, 60);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Label label5;
    }
}
