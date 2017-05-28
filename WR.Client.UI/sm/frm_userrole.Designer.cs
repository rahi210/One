namespace WR.Client.UI
{
    partial class frm_userrole
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
            this.pnl = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabRole = new System.Windows.Forms.TabPage();
            this.tabRule = new System.Windows.Forms.TabPage();
            this.lstR = new System.Windows.Forms.ListView();
            this.lstL = new System.Windows.Forms.ListView();
            this.btnR = new System.Windows.Forms.Button();
            this.btnL = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel2 = new System.Windows.Forms.Button();
            this.btnOK2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabRole.SuspendLayout();
            this.tabRule.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl
            // 
            this.pnl.AutoScroll = true;
            this.pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl.Location = new System.Drawing.Point(3, 3);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(544, 243);
            this.pnl.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SeaShell;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 246);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(544, 63);
            this.panel1.TabIndex = 1;
            // 
            // btnReset
            // 
            this.btnReset.BackgroundImage = global::WR.Client.UI.Properties.Resources.button;
            this.btnReset.Location = new System.Drawing.Point(297, 9);
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
            this.btnOK.Location = new System.Drawing.Point(190, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 42);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabRole);
            this.tabControl1.Controls.Add(this.tabRule);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(558, 341);
            this.tabControl1.TabIndex = 6;
            // 
            // tabRole
            // 
            this.tabRole.Controls.Add(this.pnl);
            this.tabRole.Controls.Add(this.panel1);
            this.tabRole.Location = new System.Drawing.Point(4, 25);
            this.tabRole.Name = "tabRole";
            this.tabRole.Padding = new System.Windows.Forms.Padding(3);
            this.tabRole.Size = new System.Drawing.Size(550, 312);
            this.tabRole.TabIndex = 0;
            this.tabRole.Text = "Binding Role";
            this.tabRole.UseVisualStyleBackColor = true;
            // 
            // tabRule
            // 
            this.tabRule.Controls.Add(this.lstR);
            this.tabRule.Controls.Add(this.lstL);
            this.tabRule.Controls.Add(this.btnR);
            this.tabRule.Controls.Add(this.btnL);
            this.tabRule.Controls.Add(this.panel2);
            this.tabRule.Location = new System.Drawing.Point(4, 25);
            this.tabRule.Name = "tabRule";
            this.tabRule.Padding = new System.Windows.Forms.Padding(3);
            this.tabRule.Size = new System.Drawing.Size(550, 312);
            this.tabRule.TabIndex = 1;
            this.tabRule.Text = "Binding Rule";
            this.tabRule.UseVisualStyleBackColor = true;
            // 
            // lstR
            // 
            this.lstR.FullRowSelect = true;
            this.lstR.Location = new System.Drawing.Point(317, 6);
            this.lstR.Name = "lstR";
            this.lstR.Size = new System.Drawing.Size(230, 232);
            this.lstR.TabIndex = 9;
            this.lstR.UseCompatibleStateImageBehavior = false;
            this.lstR.View = System.Windows.Forms.View.List;
            // 
            // lstL
            // 
            this.lstL.FullRowSelect = true;
            this.lstL.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstL.Location = new System.Drawing.Point(3, 7);
            this.lstL.MultiSelect = false;
            this.lstL.Name = "lstL";
            this.lstL.Size = new System.Drawing.Size(223, 232);
            this.lstL.TabIndex = 8;
            this.lstL.UseCompatibleStateImageBehavior = false;
            this.lstL.View = System.Windows.Forms.View.List;
            // 
            // btnR
            // 
            this.btnR.Location = new System.Drawing.Point(247, 86);
            this.btnR.Name = "btnR";
            this.btnR.Size = new System.Drawing.Size(50, 23);
            this.btnR.TabIndex = 7;
            this.btnR.Text = "<";
            this.btnR.UseVisualStyleBackColor = true;
            this.btnR.Click += new System.EventHandler(this.btnR_Click);
            // 
            // btnL
            // 
            this.btnL.Location = new System.Drawing.Point(247, 57);
            this.btnL.Name = "btnL";
            this.btnL.Size = new System.Drawing.Size(50, 23);
            this.btnL.TabIndex = 7;
            this.btnL.Text = ">";
            this.btnL.UseVisualStyleBackColor = true;
            this.btnL.Click += new System.EventHandler(this.btnL_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SeaShell;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnCancel2);
            this.panel2.Controls.Add(this.btnOK2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 246);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(544, 63);
            this.panel2.TabIndex = 0;
            // 
            // btnCancel2
            // 
            this.btnCancel2.BackgroundImage = global::WR.Client.UI.Properties.Resources.button;
            this.btnCancel2.Location = new System.Drawing.Point(297, 9);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(75, 42);
            this.btnCancel2.TabIndex = 2;
            this.btnCancel2.Text = "Cancel";
            this.btnCancel2.UseVisualStyleBackColor = true;
            this.btnCancel2.Click += new System.EventHandler(this.btnCancel2_Click);
            // 
            // btnOK2
            // 
            this.btnOK2.BackgroundImage = global::WR.Client.UI.Properties.Resources.button;
            this.btnOK2.Location = new System.Drawing.Point(190, 9);
            this.btnOK2.Name = "btnOK2";
            this.btnOK2.Size = new System.Drawing.Size(75, 42);
            this.btnOK2.TabIndex = 1;
            this.btnOK2.Text = "OK";
            this.btnOK2.UseVisualStyleBackColor = true;
            this.btnOK2.Click += new System.EventHandler(this.btnOK2_Click);
            // 
            // frm_userrole
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(558, 341);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frm_userrole";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Binding Privilege";
            this.Load += new System.EventHandler(this.frm_userrole_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabRole.ResumeLayout(false);
            this.tabRule.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabRole;
        private System.Windows.Forms.TabPage tabRule;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel2;
        private System.Windows.Forms.Button btnOK2;
        private System.Windows.Forms.Button btnR;
        private System.Windows.Forms.Button btnL;
        private System.Windows.Forms.ListView lstR;
        private System.Windows.Forms.ListView lstL;
    }
}