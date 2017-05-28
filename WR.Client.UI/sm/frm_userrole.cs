using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

using WR.Client.WCF;
using WR.WCF.Contract;

namespace WR.Client.UI
{
    public partial class frm_userrole : Form
    {
        public string userID = "";
        public string ID = "";

        public frm_userrole()
        {
            InitializeComponent();
        }

        private void frm_userrole_Load(object sender, EventArgs e)
        {
            IsysService service = sysService.GetService();
            var data = service.GetAllRoleByUserId(userID);

            foreach (var m in data.OrderBy(p => p.CREATEDATE))
            {
                CheckBox cb = new CheckBox();
                cb.Tag = m.ID;
                cb.Text = string.Format("[{0}]", m.ROLENAME);
                cb.Checked = (m.FLG == "0" ? true : false);
                cb.Width = 160;
                pnl.Controls.Add(cb);
            }

            lstL.Items.Clear();
            lstR.Items.Clear();

            //全部rule
            var lst = service.GetRule();
            lstL.BeginUpdate();
            foreach (var item in lst)
            {
                ListViewItem itm = new ListViewItem(item.RULENAME);
                itm.Name = item.ID + "L";
                itm.Tag = item.ID;
                lstL.Items.Add(itm);
            }
            lstL.EndUpdate();

            //已绑定rule
            var lstU = service.GetRuleByUserid(ID);
            lstR.BeginUpdate();
            foreach (var item in lstU)
            {
                ListViewItem itm = new ListViewItem(item.RULENAME);
                itm.Name = item.RULEID + "R";
                itm.Tag = item.RULEID;
                lstR.Items.Add(itm);
            }
            lstR.EndUpdate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            StringBuilder sbt = new StringBuilder();
            foreach (var m in pnl.Controls)
            {
                if (m is CheckBox)
                {
                    CheckBox cb = (CheckBox)m;
                    if (cb.Checked)
                        sbt.AppendFormat("|{0}", cb.Tag);
                }
            }

            if (sbt.Length > 0)
                sbt.Remove(0, 1);

            IsysService service = sysService.GetService();
            service.AddUserRole(userID, sbt.ToString());

            StringBuilder sbt2 = new StringBuilder();
            foreach (ListViewItem m in lstR.Items)
            {
                sbt2.AppendFormat("|{0}", m.Tag);
            }

            if (sbt2.Length > 0)
                sbt2.Remove(0, 1);

            service.EditUserRule(ID, sbt2.ToString());

            DialogResult = DialogResult.OK;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void btnL_Click(object sender, EventArgs e)
        {
            if (lstL.SelectedItems == null || lstL.SelectedItems.Count < 1)
                return;

            lstR.BeginUpdate();
            foreach (ListViewItem item in lstL.SelectedItems)
            {
                if (!lstR.Items.ContainsKey(item.Tag.ToString() + "R"))
                {
                    ListViewItem itm = new ListViewItem(item.Text);
                    itm.Name = item.Tag.ToString() + "R";
                    itm.Tag = item.Tag;
                    lstR.Items.Add(itm);
                }
            }
            lstR.EndUpdate();
        }

        private void btnR_Click(object sender, EventArgs e)
        {
            if (lstR.SelectedItems == null || lstR.SelectedItems.Count < 1)
                return;

            List<string> kes = new List<string>();
            foreach (ListViewItem item in lstR.SelectedItems)
            {
                kes.Add(item.Name);
            }
            lstR.BeginUpdate();
            foreach (var item in kes)
            {
                lstR.Items.RemoveByKey(item);
            }
            lstR.EndUpdate();
        }

        private void btnOK2_Click(object sender, EventArgs e)
        {
            StringBuilder sbt2 = new StringBuilder();
            foreach (var m in pnl.Controls)
            {
                if (m is CheckBox)
                {
                    CheckBox cb = (CheckBox)m;
                    if (cb.Checked)
                        sbt2.AppendFormat("|{0}", cb.Tag);
                }
            }

            if (sbt2.Length > 0)
                sbt2.Remove(0, 1);

            IsysService service = sysService.GetService();
            service.AddUserRole(userID, sbt2.ToString());

            StringBuilder sbt = new StringBuilder();
            foreach (ListViewItem m in lstR.Items)
            {
                sbt.AppendFormat("|{0}", m.Tag);
            }

            if (sbt.Length > 0)
                sbt.Remove(0, 1);

            service.EditUserRule(ID, sbt.ToString());
            DialogResult = DialogResult.OK;
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}
