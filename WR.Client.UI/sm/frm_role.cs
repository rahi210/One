using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;

namespace WR.Client.UI
{
    public partial class frm_role : FormBase
    {
        public frm_role()
        {
            InitializeComponent();
        }
        
        private void tlName_Leave(object sender, EventArgs e)
        {
            if (tlName.Text.Trim() == "")
            {
                tlName.Text = "Please input name";
                tlName.ForeColor = SystemColors.ActiveBorder;
            }
        }

        private void tlName_Enter(object sender, EventArgs e)
        {
            if (tlName.Text.Trim() == "Please input name")
            {
                tlName.Text = "";
                tlName.ForeColor = SystemColors.WindowText;
            }
        }

        private void tlSearch_Click(object sender, EventArgs e)
        {
            GetData(tlName.Text.Trim().Replace("Please input name", ""));
        }

        private void frm_role_Load(object sender, EventArgs e)
        {
            GetData("");
        }

        private void GetData(string rolename)
        {
            IsysService service = sysService.GetService();
            var data=service.GetRoleList(rolename);
            grdData.DataSource = data.OrderBy(p => p.CREATEDATE).ToList();
        }

        private void tlAddRole_Click(object sender, EventArgs e)
        {
            frm_roleedit frm = new frm_roleedit();
            frm.IsAdd = true;
            if(frm.ShowDialog()==DialogResult.OK)
                tlSearch.PerformClick();
        }

        private void tlEdit_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                return;

            string id = grdData.SelectedRows[0].Cells["ColCode"].Value.ToString();
            var data = grdData.DataSource as List<TBROLE>;
            frm_roleedit frm = new frm_roleedit();
            frm.tbRole = data.Find(p => p.ID == id);
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
            service.DelRole(id);

            tlSearch.PerformClick();
        }

        private void tlRefresh_Click(object sender, EventArgs e)
        {
            tlSearch.PerformClick();
        }

        private void grdData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (grdData.Columns[e.ColumnIndex].Name == "Column9")
            {
                string id = grdData.Rows[e.RowIndex].Cells["ColCode"].Value.ToString();
                frm_rolemenu frm = new frm_rolemenu();
                frm.roleID = id;
                if (frm.ShowDialog() == DialogResult.OK)
                    tlSearch.PerformClick();
            }
        }

        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                return;

            string id = grdData.SelectedRows[0].Cells["ColCode"].Value.ToString();

            grdMenu.AllowUserToOrderColumns = false;
            IsysService service = sysService.GetService();
            grdMenu.DataSource = service.GetMenuByRoleId(id);
        }
    }
}
