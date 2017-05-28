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
    public partial class frm_user : FormBase
    {
        public frm_user()
        {
            InitializeComponent();
        }

        private void frm_user_Load(object sender, EventArgs e)
        {
            GetData("");
        }

        private void GetData(string str)
        {
            IsysService service = sysService.GetService();
            var list = service.GetUserList(str);
            list.ForEach((p) => {
                p.RE_REVIEW = (p.RE_REVIEW == "0" ? "No" : "Yes");
            });
            grdData.DataSource = list;
        }

        private void tlTxt_Enter(object sender, EventArgs e)
        {
            if (tlTxt.Text.Trim() == "Please input id or phone or name")
            {
                tlTxt.Text = "";
                tlTxt.ForeColor = SystemColors.WindowText;
            }
        }

        private void tlTxt_Leave(object sender, EventArgs e)
        {
            if (tlTxt.Text.Trim() == "")
            {
                tlTxt.Text = "Please input id or phone or name";
                tlTxt.ForeColor = SystemColors.ActiveBorder;
            }
        }

        private void tlAdd_Click(object sender, EventArgs e)
        {
            frm_useredit frm = new frm_useredit();
            frm.IsAdd = true;
            if (frm.ShowDialog() == DialogResult.OK)
                tlSearch.PerformClick();
        }

        private void tlEdit_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                return;

            string id = grdData.SelectedRows[0].Cells["ColCode"].Value.ToString();
            var data = grdData.DataSource as List<TBUSER>;
            frm_useredit frm = new frm_useredit();
            frm.tbUser = data.FirstOrDefault(p => p.USERID == id);
            if (frm.ShowDialog() == DialogResult.OK)
                tlSearch.PerformClick();
        }

        private void tlDel_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                return;

            if (MsgBoxEx.ConfirmYesNo("Are you sure to delete the record?") == DialogResult.No)
                return;

            string id = grdData.SelectedRows[0].Cells["ColCode"].Value.ToString();

            IsysService service = sysService.GetService();
            service.DelUser(id, DataCache.UserInfo.ID);

            tlSearch.PerformClick();
        }

        private void tlRefresh_Click(object sender, EventArgs e)
        {
            tlSearch.PerformClick();
        }

        private void tlSearch_Click(object sender, EventArgs e)
        {
            GetData(tlTxt.Text.Trim().Replace("Please input id or phone or name", ""));
        }

        private void grdData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (grdData.Columns[e.ColumnIndex].Name == "Column11")
            {
                string id = grdData.Rows[e.RowIndex].Cells["ColCode"].Value.ToString();
                string iid = grdData.SelectedRows[0].Cells["ColID"].Value.ToString();
                frm_userrole frm = new frm_userrole();
                frm.userID = id;
                frm.ID = iid;

                if (frm.ShowDialog() == DialogResult.OK)
                    tlSearch.PerformClick();
            }
        }

        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                return;

            string id = grdData.SelectedRows[0].Cells["ColCode"].Value.ToString();
            string iid = grdData.SelectedRows[0].Cells["ColID"].Value.ToString();
            grdRole.AllowUserToOrderColumns = false;
            IsysService service = sysService.GetService();
            grdRole.DataSource = service.GetRoleByUserId(id);
            grdRule.DataSource = service.GetRuleByUserid(iid);
        }

        private void grdRule_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (grdRule.Columns[e.ColumnIndex].DataPropertyName == "LAYER")
            {
                if (e.Value.ToString() == "*")
                    e.Value = "All";
            }
        }
    }
}
