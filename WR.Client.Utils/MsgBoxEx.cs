using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WR.Client.Utils
{
    public partial class MsgBoxEx : Form
    {
        public MsgBoxEx()
        {
            InitializeComponent();
        }

        private void MsgBoxEx_Load(object sender, EventArgs e)
        {

        }

        internal void EInfo(string msg)
        {
            lblType.Image = global::WR.Client.Utils.Properties.Resources.msg_info_128;
            btnNo.Visible = false;
            btnOK.Visible = true;
            btnYes.Visible = false;
            lblMsg.Text = msg;
        }

        internal void EAlert(string msg)
        {
            lblType.Image = global::WR.Client.Utils.Properties.Resources.msg_alert_128;
            btnNo.Visible = false;
            btnOK.Visible = true;
            btnYes.Visible = false;
            lblMsg.Text = msg;
        }

        internal void EError(string msg)
        {
            lblType.Image = global::WR.Client.Utils.Properties.Resources.msg_error_128;
            btnNo.Visible = false;
            btnOK.Visible = true;
            btnYes.Visible = false;
            lblMsg.Text = msg;
        }

        internal void EConfirm(string msg)
        {
            lblType.Image = global::WR.Client.Utils.Properties.Resources.msg_question_128;
            btnNo.Visible = true;
            btnOK.Visible = false;
            btnYes.Visible = true;
            lblMsg.Text = msg;
        }

        #region 消息类型
        public static void Warn(string msg)
        {
            MsgBoxEx box = new MsgBoxEx();
            box.EAlert(msg);
            box.ShowDialog();
        }

        public static void Info(string msg)
        {
            MsgBoxEx box = new MsgBoxEx();
            box.EInfo(msg);
            box.ShowDialog();
        }

        public static void Error(string msg)
        {
            MsgBoxEx box = new MsgBoxEx();
            box.EError(msg);
            box.ShowDialog();
        }

        public static DialogResult ConfirmYesNo(string msg)
        {
            MsgBoxEx box = new MsgBoxEx();
            box.EConfirm(msg);
            return box.ShowDialog();
        }
        #endregion

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
