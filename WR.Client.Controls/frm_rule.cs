using System;
using System.Windows.Forms;

namespace WR.Client.Controls
{
    public partial class frm_rule : Form
    {
        private string ruleid = string.Empty;
        /// <summary>
        /// rule id
        /// </summary>
        public string Ruleid
        {
            get { return ruleid; }
            set { ruleid = value; }
        }

        private string rulename = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Rulename
        {
            get { return rulename; }
            set { rulename = value; }
        }

        private string ruledesr = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Ruledesr
        {
            get { return ruledesr; }
            set { ruledesr = value; }
        }

        private string deviceid = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Deviceid
        {
            get { return deviceid; }
            set { deviceid = value; }
        }

        private string layer = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        private string rtype = string.Empty;
        /// <summary>
        /// 操作类型
        /// </summary>
        public string RType
        {
            get { return rtype; }
            set { rtype = value; }
        }

        public WrRule pCtrl = null;

        public frm_rule()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void frm_rule_Load(object sender, EventArgs e)
        {
            txtRule.Text = Rulename;
            txtDescrp.Text = Ruledesr;
            txtDevice.Text = Deviceid;

            if (RType == "EDIT")
                cbxLayer.Text = (Layer == "*" ? "All" : Layer);
            else
                cbxLayer.Text = "All";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtRule.Text.Trim().Length < 1)
            {
                txtRule.Focus();
                WR.Client.Utils.MsgBoxEx.Info("Please input rule");
                return;
            }

            if (txtDevice.Text.Trim().Length < 1)
            {
                txtDevice.Focus();
                WR.Client.Utils.MsgBoxEx.Info("Please input device id");
                return;
            }

            if (cbxLayer.Text.Trim().Length < 1)
            {
                cbxLayer.Focus();
                WR.Client.Utils.MsgBoxEx.Info("Please input setup id");
                return;
            }

            var rs = false;

            if (pCtrl != null)
                rs = pCtrl.SaveRule(RType, new string[] { Ruleid, txtRule.Text.Trim(), txtDescrp.Text.Trim(), txtDevice.Text.Trim(), cbxLayer.Text.Trim() });

            if (rs)
                DialogResult = DialogResult.OK;
        }
    }
}
