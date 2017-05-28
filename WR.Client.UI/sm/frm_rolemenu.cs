using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WR.Client.WCF;
using WR.WCF.Contract;

namespace WR.Client.UI
{
    public partial class frm_rolemenu : FormBase
    {
        public string roleID = "";

        public frm_rolemenu()
        {
            InitializeComponent();
        }

        private void frm_rolemenu_Load(object sender, EventArgs e)
        {
            IsysService service = sysService.GetService();
            var data = service.GetAllMenuByRoleId(roleID);

            foreach (var m in data.OrderBy(p => p.MENUCODE))
            {
                CheckBox cb = new CheckBox();
                cb.Tag = m.ID;
                cb.Text = string.Format("[{0}]{1}", m.MENUCODE, m.MENUNAME);
                cb.Checked = (m.FLG == "0" ? true : false);
                cb.Width = 160;
                if (m.MENUCODE == "123")
                {
                    cb.Enabled = false;
                    cb.Checked = true;
                }

                pnl.Controls.Add(cb);
            }
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
            service.AddRoleMenu(roleID, sbt.ToString());
            DialogResult = DialogResult.OK;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}
