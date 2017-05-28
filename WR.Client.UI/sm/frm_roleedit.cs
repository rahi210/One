using System;
using System.Windows.Forms;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;

namespace WR.Client.UI
{
    public partial class frm_roleedit : FormBase
    {
        public bool IsAdd = false;
        public TBROLE tbRole;

        public frm_roleedit()
        {
            InitializeComponent();
        }

        private void frm_roleedit_Load(object sender, EventArgs e)
        {
            if (IsAdd)
            {
                this.Text = "Create New Role";
                txtCode.Text = Guid.NewGuid().ToString("N").ToUpper();
            }
            else
            {
                this.Text = "Update Role";
                txtCode.Text = tbRole.ID;
                txtName.Text = tbRole.ROLENAME;
                txtRemark.Text = tbRole.REMARK;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim().Length < 1)
            {
                txtName.Focus();
                MsgBoxEx.Info("Please input name");
                return;
            }
            IsysService service = sysService.GetService();
            string res = "";

            if (IsAdd)
            {
                res = service.AddRole(txtCode.Text, txtName.Text.Trim(), txtRemark.Text.Trim(), DataCache.UserInfo.ID);
            }
            else
            {
                res = service.UpdateRole(txtCode.Text, txtName.Text.Trim(), txtRemark.Text.Trim(), DataCache.UserInfo.ID);
            }

            if (res != "1")
            {
                MsgBoxEx.Info(res);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}
