namespace WR.Client.UI
{
    partial class frm_yieldedit
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnImg = new System.Windows.Forms.Button();
            this.txtImg = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.rbDevice = new System.Windows.Forms.RadioButton();
            this.rbLayer = new System.Windows.Forms.RadioButton();
            this.rbRepice = new System.Windows.Forms.RadioButton();
            this.nudEYield = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudDYield = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nudCYield = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudBYield = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudAYield = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudWaferYield = new System.Windows.Forms.NumericUpDown();
            this.nudLotYield = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRecipeId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEYield)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDYield)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCYield)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBYield)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAYield)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWaferYield)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLotYield)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.btnImg);
            this.panel2.Controls.Add(this.txtImg);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.rbDevice);
            this.panel2.Controls.Add(this.rbLayer);
            this.panel2.Controls.Add(this.rbRepice);
            this.panel2.Controls.Add(this.nudEYield);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.nudDYield);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.nudCYield);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.nudBYield);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.nudAYield);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.nudWaferYield);
            this.panel2.Controls.Add(this.nudLotYield);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtRecipeId);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(345, 310);
            this.panel2.TabIndex = 2;
            // 
            // btnImg
            // 
            this.btnImg.Location = new System.Drawing.Point(281, 264);
            this.btnImg.Name = "btnImg";
            this.btnImg.Size = new System.Drawing.Size(25, 23);
            this.btnImg.TabIndex = 39;
            this.btnImg.Text = "..";
            this.btnImg.UseVisualStyleBackColor = true;
            this.btnImg.Click += new System.EventHandler(this.btnImg_Click);
            // 
            // txtImg
            // 
            this.txtImg.Location = new System.Drawing.Point(114, 264);
            this.txtImg.Name = "txtImg";
            this.txtImg.ReadOnly = true;
            this.txtImg.Size = new System.Drawing.Size(171, 21);
            this.txtImg.TabIndex = 38;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 267);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 12);
            this.label10.TabIndex = 37;
            this.label10.Text = "Reference Image:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(73, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 12);
            this.label9.TabIndex = 36;
            this.label9.Text = "Type:";
            // 
            // rbDevice
            // 
            this.rbDevice.AutoSize = true;
            this.rbDevice.Location = new System.Drawing.Point(247, 45);
            this.rbDevice.Name = "rbDevice";
            this.rbDevice.Size = new System.Drawing.Size(59, 16);
            this.rbDevice.TabIndex = 35;
            this.rbDevice.TabStop = true;
            this.rbDevice.Text = "Device";
            this.rbDevice.UseVisualStyleBackColor = true;
            // 
            // rbLayer
            // 
            this.rbLayer.AutoSize = true;
            this.rbLayer.Location = new System.Drawing.Point(181, 45);
            this.rbLayer.Name = "rbLayer";
            this.rbLayer.Size = new System.Drawing.Size(53, 16);
            this.rbLayer.TabIndex = 34;
            this.rbLayer.TabStop = true;
            this.rbLayer.Text = "Layer";
            this.rbLayer.UseVisualStyleBackColor = true;
            // 
            // rbRepice
            // 
            this.rbRepice.AutoSize = true;
            this.rbRepice.Checked = true;
            this.rbRepice.Location = new System.Drawing.Point(114, 45);
            this.rbRepice.Name = "rbRepice";
            this.rbRepice.Size = new System.Drawing.Size(59, 16);
            this.rbRepice.TabIndex = 33;
            this.rbRepice.TabStop = true;
            this.rbRepice.Text = "Recipe";
            this.rbRepice.UseVisualStyleBackColor = true;
            // 
            // nudEYield
            // 
            this.nudEYield.DecimalPlaces = 2;
            this.nudEYield.Location = new System.Drawing.Point(114, 237);
            this.nudEYield.Name = "nudEYield";
            this.nudEYield.Size = new System.Drawing.Size(192, 21);
            this.nudEYield.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(55, 239);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 31;
            this.label8.Text = "E Yield:";
            // 
            // nudDYield
            // 
            this.nudDYield.DecimalPlaces = 2;
            this.nudDYield.Location = new System.Drawing.Point(114, 210);
            this.nudDYield.Name = "nudDYield";
            this.nudDYield.Size = new System.Drawing.Size(192, 21);
            this.nudDYield.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(55, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 29;
            this.label7.Text = "D Yield:";
            // 
            // nudCYield
            // 
            this.nudCYield.DecimalPlaces = 2;
            this.nudCYield.Location = new System.Drawing.Point(114, 183);
            this.nudCYield.Name = "nudCYield";
            this.nudCYield.Size = new System.Drawing.Size(192, 21);
            this.nudCYield.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 27;
            this.label4.Text = "C Yield:";
            // 
            // nudBYield
            // 
            this.nudBYield.DecimalPlaces = 2;
            this.nudBYield.Location = new System.Drawing.Point(114, 156);
            this.nudBYield.Name = "nudBYield";
            this.nudBYield.Size = new System.Drawing.Size(192, 21);
            this.nudBYield.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 25;
            this.label2.Text = "B Yield:";
            // 
            // nudAYield
            // 
            this.nudAYield.DecimalPlaces = 2;
            this.nudAYield.Location = new System.Drawing.Point(114, 129);
            this.nudAYield.Name = "nudAYield";
            this.nudAYield.Size = new System.Drawing.Size(192, 21);
            this.nudAYield.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "A Yield:";
            // 
            // nudWaferYield
            // 
            this.nudWaferYield.DecimalPlaces = 2;
            this.nudWaferYield.Location = new System.Drawing.Point(114, 102);
            this.nudWaferYield.Name = "nudWaferYield";
            this.nudWaferYield.Size = new System.Drawing.Size(192, 21);
            this.nudWaferYield.TabIndex = 21;
            // 
            // nudLotYield
            // 
            this.nudLotYield.DecimalPlaces = 2;
            this.nudLotYield.Location = new System.Drawing.Point(114, 75);
            this.nudLotYield.Name = "nudLotYield";
            this.nudLotYield.Size = new System.Drawing.Size(192, 21);
            this.nudLotYield.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "Wafer Yield:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "Lot Yield:";
            // 
            // txtRecipeId
            // 
            this.txtRecipeId.Location = new System.Drawing.Point(114, 17);
            this.txtRecipeId.Name = "txtRecipeId";
            this.txtRecipeId.Size = new System.Drawing.Size(192, 21);
            this.txtRecipeId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SeaShell;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 310);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(345, 63);
            this.panel1.TabIndex = 3;
            // 
            // btnReset
            // 
            this.btnReset.BackgroundImage = global::WR.Client.UI.Properties.Resources.button;
            this.btnReset.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReset.Location = new System.Drawing.Point(180, 8);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 42);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Cancel";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::WR.Client.UI.Properties.Resources.button;
            this.btnOK.Location = new System.Drawing.Point(73, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 42);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frm_yieldedit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 373);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_yieldedit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frm_yieldedit";
            this.Load += new System.EventHandler(this.frm_yieldedit_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEYield)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDYield)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCYield)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBYield)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAYield)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWaferYield)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLotYield)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtRecipeId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.NumericUpDown nudWaferYield;
        private System.Windows.Forms.NumericUpDown nudLotYield;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudEYield;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudDYield;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudCYield;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudBYield;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudAYield;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton rbDevice;
        private System.Windows.Forms.RadioButton rbLayer;
        private System.Windows.Forms.RadioButton rbRepice;
        private System.Windows.Forms.TextBox txtImg;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnImg;
    }
}