using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WR.WCF.DataContract;
using WR.WCF.Contract;
using WR.Client.WCF;
using WR.Client.Utils;

namespace WR.Client.UI
{
    public partial class frm_exam : FormBase
    {
        public frm_exam()
        {
            InitializeComponent();
        }

        private void frm_exam_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void InitData()
        {
            LoadMarkData();

            LoadLibraryData();

            IwrService service = wrService.GetService();

            var planList = service.GetEmPlan("");
            planList.Insert(0, new EMPLAN() { PID = "", PLANNAME = "-" });

            cbxPlan.DisplayMember = "PLANNAME";
            cbxPlan.ValueMember = "PID";
            cbxPlan.DataSource = planList;
        }

        #region 设置分数
        private void LoadMarkData()
        {
            IwrService service = wrService.GetService();

            var list = service.GetCLASSIFICATIONMARK("");

            //grdMark.DataSource = new BindingCollection<EMCLASSIFICATIONMARK>(list);
            grdMark.DataSource = list;
        }

        private void tlMarkEdit_Click(object sender, EventArgs e)
        {
            if (grdMark.SelectedRows == null || grdMark.SelectedRows.Count < 1)
                return;

            var id = grdMark.SelectedRows[0].Cells["Id"].Value.ToString();
            var data = grdMark.DataSource as List<EMCLASSIFICATIONMARK>;

            frm_markedit frm = new frm_markedit();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.emMark = data.FirstOrDefault(p => p.CID == id);

            if (frm.ShowDialog() == DialogResult.OK)
                LoadMarkData();
        }

        private void tlMarkRefresh_Click(object sender, EventArgs e)
        {
            LoadMarkData();
        }

        private void tlMarkSearch_Click(object sender, EventArgs e)
        {
            LoadMarkData();
        }

        private void grdMark_DoubleClick(object sender, EventArgs e)
        {
            tlMarkEdit_Click(sender, e);
        }


        //private void tlClass_Enter(object sender, EventArgs e)
        //{
        //    if (tlClass.Text.Trim() == "Please input id or name")
        //    {
        //        tlClass.Text = "";
        //        tlClass.ForeColor = SystemColors.WindowText;
        //    }
        //}

        //private void tlClass_Leave(object sender, EventArgs e)
        //{
        //    if (tlClass.Text.Trim() == "")
        //    {
        //        tlClass.Text = "Please input id or name";
        //        tlClass.ForeColor = SystemColors.ActiveBorder;
        //    }
        //}
        #endregion

        #region 题库

        private void LoadLibraryData()
        {
            IwrService service = wrService.GetService();

            var list = service.GetLIBRARY(tlTxt.Text.Replace("Please input name", ""));
            grdLibrary.AutoGenerateColumns = false;
            grdLibrary.DataSource = list;
        }

        private void tlAdd_Click(object sender, EventArgs e)
        {
            frm_examedit frm = new frm_examedit();
            frm.IsAdd = true;
            if (frm.ShowDialog() == DialogResult.OK)
                tlSearch.PerformClick();
        }

        private void tlEdit_Click(object sender, EventArgs e)
        {
            if (grdLibrary.SelectedRows == null || grdLibrary.SelectedRows.Count < 1)
                return;

            string id = grdLibrary.SelectedRows[0].Cells["lid"].Value.ToString();
            var data = grdLibrary.DataSource as List<EMLIBRARY>;
            frm_examedit frm = new frm_examedit();
            frm.Entity = data.FirstOrDefault(p => p.LID == id);
            if (frm.ShowDialog() == DialogResult.OK)
                tlSearch.PerformClick();
        }

        private void tlDel_Click(object sender, EventArgs e)
        {
            if (grdLibrary.SelectedRows == null || grdLibrary.SelectedRows.Count < 1)
                return;

            if (MsgBoxEx.ConfirmYesNo("Are you sure to delete the record?") == DialogResult.No)
                return;

            string id = grdLibrary.SelectedRows[0].Cells["lid"].Value.ToString();

            IwrService service = wrService.GetService();

            service.DeleteLibray(id, DataCache.UserInfo.ID);

            tlSearch.PerformClick();
        }

        private void tlRefresh_Click(object sender, EventArgs e)
        {
            LoadLibraryData();
        }

        private void tlTxt_Enter(object sender, EventArgs e)
        {
            if (tlTxt.Text.Trim() == "Please input name")
            {
                tlTxt.Text = "";
                tlTxt.ForeColor = SystemColors.WindowText;
            }
        }

        private void tlTxt_Leave(object sender, EventArgs e)
        {
            if (tlTxt.Text.Trim() == "")
            {
                tlTxt.Text = "Please input name";
                tlTxt.ForeColor = SystemColors.ActiveBorder;
            }
        }

        private void tlSearch_Click(object sender, EventArgs e)
        {
            LoadLibraryData();
        }

        private void grdLibrary_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (grdLibrary.Columns[e.ColumnIndex].HeaderText == "Enabled")
            {
                if (e.Value.ToString() == "0")
                    e.Value = "False";
                else
                    e.Value = "True";
            }
        }

        private void grdLibrary_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (grdLibrary.Columns[e.ColumnIndex].Name == "Column1")
            {
                var lid = grdLibrary.Rows[e.RowIndex].Cells["LID"].Value.ToString();

                var frm = new frm_paper();
                frm.Oparams = new string[] { lid, "0", "", "", "" };
                frm.ShowDialog();
            }
        }
        #endregion

        #region 试卷
        private void tlPlanAdd_Click(object sender, EventArgs e)
        {
            string lid = grdLibrary.SelectedRows[0].Cells["LID"].Value.ToString();

            frm_planedit frm = new frm_planedit();
            frm.LID = lid;

            frm.IsAdd = true;
            if (frm.ShowDialog() == DialogResult.OK)
                LoadPlanData(lid);
        }

        private void tlPlanEdit_Click(object sender, EventArgs e)
        {
            string id = grdPlan.SelectedRows[0].Cells["pid"].Value.ToString();
            var data = grdPlan.DataSource as List<EMPLAN>;

            frm_planedit frm = new frm_planedit();
            frm.emPlan = data.FirstOrDefault(p => p.PID == id);

            if (frm.ShowDialog() == DialogResult.OK)
                tlPlanRefresh.PerformClick();
        }

        private void tlPlanDelete_Click(object sender, EventArgs e)
        {
            if (grdPlan.SelectedRows == null || grdPlan.SelectedRows.Count < 1)
                return;

            if (MsgBoxEx.ConfirmYesNo("Are you sure to delete the record?") == DialogResult.No)
                return;

            string id = grdPlan.SelectedRows[0].Cells["pid"].Value.ToString();

            IwrService service = wrService.GetService();

            service.DeletePlan(id, DataCache.UserInfo.ID);

            tlPlanRefresh.PerformClick();
        }

        private void tlPlanRefresh_Click(object sender, EventArgs e)
        {
            string lid = grdLibrary.SelectedRows[0].Cells["LID"].Value.ToString();

            LoadPlanData(lid);
        }

        private void grdLibrary_SelectionChanged(object sender, EventArgs e)
        {
            if (grdLibrary.SelectedRows == null || grdLibrary.SelectedRows.Count < 1)
                return;
            string lid = grdLibrary.SelectedRows[0].Cells["LID"].Value.ToString();

            LoadPlanData(lid);
        }

        private void LoadPlanData(string lid)
        {
            IwrService service = wrService.GetService();

            grdPlan.AutoGenerateColumns = false;
            grdPlan.DataSource = service.GetEmPlan(lid);
        }
        #endregion

        #region 报表
        private void btnQuery_Click(object sender, EventArgs e)
        {
            IwrService service = wrService.GetService();

            //service.AddExamResult(DataCache.UserInfo.ID, "41c5c298-7a55-4302-a7a1-0079fc3efc72");

            var list = service.GetExamResultReport(dtDate.Value.ToString("yyyyMMdd"), dateTo.Value.ToString("yyyyMMdd"), cbxPlan.SelectedValue.ToString());

            grdExamResult.AutoGenerateColumns = false;
            grdExamResult.DataSource = list;
        }
        #endregion

        private void grdExamResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (grdExamResult.Columns[e.ColumnIndex].Name == "Column8")
            {
                var eid = grdExamResult.Rows[e.RowIndex].Cells["EID"].Value.ToString();
                var sdate = grdExamResult.Rows[e.RowIndex].Cells["r_startdate"].Value;
                var edate = grdExamResult.Rows[e.RowIndex].Cells["r_enddate"].Value;

                var frm = new frm_paper();
                frm.Oparams = new string[] { eid, "1", "", "", "" };
                frm.ShowDialog();
            }
        }

        private void tabExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabExam.SelectedTab == tabPage2)
            {
                IwrService service = wrService.GetService();

                var planList = service.GetEmPlan("");
                planList.Insert(0, new EMPLAN() { PID = "", PLANNAME = "-" });

                cbxPlan.DisplayMember = "PLANNAME";
                cbxPlan.ValueMember = "PID";
                cbxPlan.DataSource = planList;
            }
        }
    }
}
