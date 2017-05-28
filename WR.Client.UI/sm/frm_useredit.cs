using System;
using System.Windows.Forms;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;

namespace WR.Client.UI
{
    public partial class frm_useredit : Form
    {
        public bool IsAdd = false;
        public TBUSER tbUser;

        public frm_useredit()
        {
            InitializeComponent();
        }

        private void frm_useredit_Load(object sender, EventArgs e)
        {
            if (IsAdd)
            {
                this.Text = "Create New User";
                //txtID.Text = Guid.NewGuid().ToString("N").ToUpper();
            }
            else
            {
                this.Text = "Update User";
                txtID.Enabled = false;
                txtID.Text = tbUser.USERID;
                txtName.Text = tbUser.USERNAME;
                txtRemark.Text = tbUser.REMARK;
                txtPhone.Text = tbUser.TELEPHONE;
                txtEmail.Text = tbUser.EMAIL;
                txtPwd.Text = tbUser.PWD;
                cbxRereview.Checked = (tbUser.RE_REVIEW == "1" || tbUser.RE_REVIEW == "Yes");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Trim().Length < 1)
            {
                txtID.Focus();
                MsgBoxEx.Info("Please input ID");
                return;
            }

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
                res = service.AddUser(Guid.NewGuid().ToString("N").ToUpper(), txtID.Text.Trim(), txtPwd.Text.Trim(),
                    txtName.Text.Trim(), txtPhone.Text.Trim(), txtEmail.Text.Trim(), txtRemark.Text.Trim(), (cbxRereview.Checked ? "1" : "0"), DataCache.UserInfo.ID);
            }
            else
            {
                res = service.UpdateUser(tbUser.ID, txtID.Text.Trim(), txtPwd.Text.Trim(),
                    txtName.Text.Trim(), txtPhone.Text.Trim(), txtEmail.Text.Trim(), txtRemark.Text.Trim(), (cbxRereview.Checked ? "1" : "0"), DataCache.UserInfo.ID);
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
