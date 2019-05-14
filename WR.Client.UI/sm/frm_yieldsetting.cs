using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;
using WR.Client.Utils;

namespace WR.Client.UI
{
    public partial class frm_yieldsetting : FormBase
    {
        public frm_yieldsetting()
        {
            InitializeComponent();
        }

        private void GetData()
        {
            IwrService service = wrService.GetService();
            var data = service.GetAllYieldSetting();

            grdData.AutoGenerateColumns = false;
            grdData.DataSource = data.OrderBy(p => p.RECIPE_ID).ToList();
        }

        private void tlAdd_Click(object sender, EventArgs e)
        {
            frm_yieldedit frm = new frm_yieldedit();

            frm.IsAdd = true;
            if (frm.ShowDialog() == DialogResult.OK)
                tlRefresh.PerformClick();
        }

        private void tlEdit_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                return;

            string id = grdData.SelectedRows[0].Cells["RECIPE_ID"].Value.ToString();
            var data = grdData.DataSource as List<WMYIELDSETTING>;

            frm_yieldedit frm = new frm_yieldedit();
            frm.wmYield = data.Find(p => p.RECIPE_ID == id);

            if (frm.ShowDialog() == DialogResult.OK)
                tlRefresh.PerformClick();
        }

        private void tlDel_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                return;

            if (MsgBoxEx.ConfirmYesNo("Are you sure to delete the record?") == DialogResult.No)
                return;

            string id = grdData.SelectedRows[0].Cells["RECIPE_ID"].Value.ToString();

            IwrService service = wrService.GetService();
            service.DelYield(id);

            tlRefresh.PerformClick();
        }

        private void tlRefresh_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void frm_yieldsetting_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void grdData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (grdData.Columns[e.ColumnIndex].Name == "Type")
                {
                    string val = e.Value.ToString();
                    if (val == "0")
                        e.Value = "Repice";
                    else if (val == "1")
                        e.Value = "Layer";
                    else
                        e.Value = "Device";
                }
            }
        }
    }
}
