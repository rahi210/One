namespace WR.Client.Controls
{
    partial class frm_rule
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
            this.label12 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtRule = new System.Windows.Forms.TextBox();
            this.txtDescrp = new System.Windows.Forms.TextBox();
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbxLayer = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(50, 116);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 15;
            this.label12.Text = "SetupID:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(44, 90);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 12);
            this.label16.TabIndex = 12;
            this.label16.Text = "DeviceID:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(26, 37);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(77, 12);
            this.label18.TabIndex = 13;
            this.label18.Text = "Description:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(68, 14);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(35, 12);
            this.label20.TabIndex = 14;
            this.label20.Text = "Rule:";
            // 
            // txtRule
            // 
            this.txtRule.Location = new System.Drawing.Point(109, 11);
            this.txtRule.MaxLength = 25;
            this.txtRule.Name = "txtRule";
            this.txtRule.Size = new System.Drawing.Size(246, 21);
            this.txtRule.TabIndex = 0;
            // 
            // txtDescrp
            // 
            this.txtDescrp.Location = new System.Drawing.Point(109, 37);
            this.txtDescrp.MaxLength = 50;
            this.txtDescrp.Multiline = true;
            this.txtDescrp.Name = "txtDescrp";
            this.txtDescrp.Size = new System.Drawing.Size(246, 46);
            this.txtDescrp.TabIndex = 1;
            // 
            // txtDevice
            // 
            this.txtDevice.Location = new System.Drawing.Point(109, 87);
            this.txtDevice.MaxLength = 50;
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.Size = new System.Drawing.Size(246, 21);
            this.txtDevice.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(109, 154);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 33);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(202, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 33);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbxLayer
            // 
            this.cbxLayer.FormattingEnabled = true;
            this.cbxLayer.Items.AddRange(new object[] {
            "All"});
            this.cbxLayer.Location = new System.Drawing.Point(109, 113);
            this.cbxLayer.MaxLength = 50;
            this.cbxLayer.Name = "cbxLayer";
            this.cbxLayer.Size = new System.Drawing.Size(246, 20);
            this.cbxLayer.TabIndex = 3;
            this.cbxLayer.Text = "All";
            // 
            // frm_rule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 206);
            this.Controls.Add(this.cbxLayer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtDescrp);
            this.Controls.Add(this.txtDevice);
            this.Controls.Add(this.txtRule);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label20);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_rule";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rule Setting";
            this.Load += new System.EventHandler(this.frm_rule_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtRule;
        private System.Windows.Forms.TextBox txtDescrp;
        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbxLayer;
    }
}