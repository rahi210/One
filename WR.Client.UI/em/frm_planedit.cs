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
    public partial class frm_planedit : Form
    {
        public bool IsAdd = false;
        public EMPLAN emPlan;
        public string LID { get; set; }

        public frm_planedit()
        {
            InitializeComponent();
        }

        private void frm_planedit_Load(object sender, EventArgs e)
        {
            if (IsAdd)
            {
                this.Text = "Create New Plan";
            }
            else
            {

                this.Text = "Update Plan";

                if (emPlan != null)
                {
                    txtName.Text = emPlan.PLANNAME;
                    dtDate.Value = emPlan.STARTDATE.Date;

                    dtStartTime.Value = emPlan.STARTDATE;
                    dtEndTime.Value = emPlan.ENDDATE;

                    nudUser.Value = emPlan.USERNUM;
                    nudDefect.Value = emPlan.NUMDEFECT;

                    txtRemark.Text = emPlan.REMARK;
                }
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

            int tdays = (int)(dtDate.Value - DateTime.Now.Date).TotalDays;

            if (tdays < 0)
            {
                dtDate.Focus();
                MsgBoxEx.Info("From date selection error.");
                return;
            }

            tdays = (int)(dtEndTime.Value - dtStartTime.Value).TotalMinutes;

            if (tdays <= 0)
            {
                dtEndTime.Focus();
                MsgBoxEx.Info("From date selection error.");
                return;
            }

            if (nudUser.Value <= 0)
            {
                nudUser.Focus();
                MsgBoxEx.Info("Number selection error.");
                return;
            }

            if (nudDefect.Value <= 0)
            {
                nudDefect.Focus();
                MsgBoxEx.Info("Defect Number selection error.");
                return;
            }

            IwrService service = wrService.GetService();
            var res = 0;

            var startTime = string.Format("{0} {1}:00", dtDate.Value.ToString("yyyy-MM-dd"), dtStartTime.Value.ToString("HH:mm"));
            var endTime = string.Format("{0} {1}:00", dtDate.Value.ToString("yyyy-MM-dd"), dtEndTime.Value.ToString("HH:mm"));
            if (IsAdd)
            {
                res = service.AddPlan(LID, txtName.Text.Trim(), startTime, endTime, (int)nudUser.Value, (int)nudDefect.Value, txtRemark.Text, DataCache.UserInfo.ID);
            }
            else
            {
                res = service.UpdatePlan(emPlan.PID, txtName.Text.Trim(), startTime, endTime, (int)nudUser.Value, (int)nudDefect.Value, txtRemark.Text, DataCache.UserInfo.ID);
            }

            if (res != 1)
            {
                MsgBoxEx.Info(res.ToString());
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
