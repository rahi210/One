using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;

namespace WR.Client.UI
{
    public partial class frm_changepwd : FormBase
    {
        public frm_changepwd()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtOldPwd.Text.Trim().Length < 1)
            {
                txtOldPwd.Focus();
                MsgBoxEx.Info("Please input old password");
                return;
            }

            if (txtNewPwd.Text.Trim().Length < 1)
            {
                txtNewPwd.Focus();
                MsgBoxEx.Info("Please input new password");
                return;
            }

            if (txtConPwd.Text.Trim().Length < 1)
            {
                txtConPwd.Focus();
                MsgBoxEx.Info("Please input confirm password");
                return;
            }

            if (txtNewPwd.Text.Trim() != txtConPwd.Text.Trim())
            {
                txtNewPwd.Focus();
                MsgBoxEx.Info("New password and confirm password is not the same");
                return;
            }

            IsysService service = sysService.GetService();
            string res = service.UpdatePwd(DataCache.UserInfo.ID, txtOldPwd.Text.Trim(), txtNewPwd.Text.Trim(), DataCache.UserInfo.ID);

            if (res == "1")
            {
                MsgBoxEx.Info("Your password has been changed successfully,Please log in again.");
                //btnClose_Click(null, null);
                if (frmMain != null)
                {
                    frmMain.ReLogin();
                }
            }
            else if (res == "-1")
            {
                MsgBoxEx.Error("Your old password input errors");
            }
            else
            {
                MsgBoxEx.Error("Your password modification failed");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            txtConPwd.Clear();
            txtNewPwd.Clear();
            txtOldPwd.Clear();
        }
    }
}
