using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WR.WCF.DataContract;
using WR.Client.Utils;
using WR.WCF.Contract;
using WR.Client.WCF;

namespace WR.Client.UI
{
    public partial class frm_examedit : Form
    {
        public bool IsAdd = false;
        public EMLIBRARY Entity { get; set; }

        public List<ComboxModel> ResultList { get; set; }
        public frm_examedit()
        {
            InitializeComponent();
        }

        private void frm_examedit_Load(object sender, EventArgs e)
        {
            if (IsAdd)
            {
                this.Text = "Create New Library";
            }
            else
            {
                this.Text = "Update Library";
                txtName.Text = Entity.PAPERNAME;
                txtRemark.Text = Entity.REMARK;
                ckStatus.Checked = Entity.STATUS == "1" ? true : false;

                groupBox1.Visible = false;

                this.Height -= 200;
            }

            ResultList = new List<ComboxModel>();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim().Length < 1)
            {
                txtName.Focus();
                MsgBoxEx.Info("Please input name");
                return;
            }

            if (IsAdd && lbResult.Items.Count == 0)
            {
                MsgBoxEx.Info("Please select wafer result");
                return;
            }

            IwrService service = wrService.GetService();
            var res = 0;

            var resultId = string.Join(",", ResultList.Select(s => s.ID).ToArray());

            if (IsAdd)
            {
                res = service.AddLibray(resultId, txtName.Text.Trim(), txtRemark.Text, DataCache.UserInfo.ID, (ckStatus.Checked ? "1" : "0"));
            }
            else
            {
                res = service.UpdateLibray(Entity.LID, txtName.Text.Trim(), txtRemark.Text, (ckStatus.Checked ? "1" : "0"), DataCache.UserInfo.ID);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frm_resultlist frm = new frm_resultlist();

            frm.ResultList = ResultList;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                ResultList = frm.ResultList.Distinct().ToList();

                var list = ResultList.Select(s => s.NAME).ToArray();

                lbResult.Items.AddRange(list);

                lbResult.SelectedIndex = 0;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbResult.SelectedIndex > -1)
            {
                ResultList = ResultList.Where(s => s.NAME != lbResult.Text).ToList();
                lbResult.Items.RemoveAt(lbResult.SelectedIndex);

                if (lbResult.Items.Count > 0)
                    lbResult.SelectedIndex = 0;
            }
        }
    }
}
