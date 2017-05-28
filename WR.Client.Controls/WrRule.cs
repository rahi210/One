using System;
using System.Windows.Forms;

namespace WR.Client.Controls
{
    public delegate void RuleOptEventHandler(string type, string[] data);

    public partial class WrRule : UserControl
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

        public event RuleOptEventHandler OptRule;

        private string rtype = string.Empty;
        /// <summary>
        /// 操作类型
        /// </summary>
        public string RType
        {
            get { return rtype; }
            set
            {
                rtype = value;
                if (RType == "ADD")
                {
                    label12.Visible = false;
                    label16.Visible = false;
                    label18.Visible = false;
                    label20.Visible = false;
                    lblDel.Visible = false;
                    lblDesrp.Visible = false;
                    lblDevice.Visible = false;
                    lblEdit.Visible = false;
                    lblRule.Visible = false;
                    lblStepup.Visible = false;
                }
                else
                {
                    lblInst.Visible = false;
                }
            }
        }

        public WrRule()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WrRule_Load(object sender, EventArgs e)
        {
            lblDesrp.Text = Ruledesr;
            lblDevice.Text = Deviceid;
            lblRule.Text = Rulename;
            lblStepup.Text = (Layer == "*" ? "All" : Layer);
        }

        private void lblEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frm_rule frm = new frm_rule();
            frm.pCtrl = this;
            frm.Ruleid = Ruleid;
            frm.Rulename = Rulename;
            frm.Ruledesr = Ruledesr;
            frm.Deviceid = Deviceid;
            frm.Layer = Layer;
            frm.RType = RType;
            frm.ShowDialog();
        }

        private void lblDel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (WR.Client.Utils.MsgBoxEx.ConfirmYesNo("Are you sure delete the rule?") == DialogResult.No)
                return;

            if (OptRule != null)
                OptRule("DEL", new string[] { Ruleid });
        }

        internal void SaveRule(string type, string[] data)
        {
            //type:ADD、EDIT
            if (OptRule != null)
                OptRule(type, data);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            frm_rule frm = new frm_rule();
            frm.RType = RType;
            frm.pCtrl = this;
            frm.ShowDialog();
        }
    }
}
