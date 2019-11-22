using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WR.Client.Utils;
using System.IO;
using WR.WCF.DataContract;
using WR.WCF.Contract;
using WR.Client.WCF;

namespace WR.Client.UI
{
    public partial class frm_classedit : FormBase
    {
        private string schemeid;
        private List<WMCLASSIFICATIONITEM> items;

        public frm_classedit()
        {
            InitializeComponent();
        }

        public frm_classedit(string schemeid, List<WMCLASSIFICATIONITEM> items)
        {
            InitializeComponent();

            this.schemeid = schemeid;
            this.items = items;
        }

        private void frm_classedit_Load(object sender, EventArgs e)
        {
            InitHotkey();
        }

        private void InitHotkey()
        {
            List<CMNDICT> hotkey = DataCache.CmnDict.Where(p => p.DICTID == "2010").ToList();
            hotkey.Add(new CMNDICT() { DICTID = "2010", CODE = null, NAME = "-" });

            cbxHotkey.DisplayMember = "NAME";
            cbxHotkey.ValueMember = "CODE";
            cbxHotkey.DataSource = hotkey;
        }

        private void txtColor_Click(object sender, EventArgs e)
        {
            if (clrDialog.ShowDialog() == DialogResult.OK)
            {
                txtColor.Text = ColorTranslator.ToHtml(clrDialog.Color);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (items.Any(p => p.ID == nudId.Value))
            {
                nudId.Focus();
                MsgBoxEx.Info(string.Format("Id[{0}] already repeated!", nudId.Value));
                return;
            }

            if (txtClassification.Text.Trim().Length < 1)
            {
                txtClassification.Focus();
                MsgBoxEx.Info("Please input name");
                return;
            }

            if (txtClassification.Text.Trim().Length > 40)
            {
                txtClassification.Focus();
                MsgBoxEx.Info("Please enter no more than 40 characters");
                return;
            }

            //hot key
            var hotkey = "";
            if (cbxHotkey.SelectedValue != null)
            {
                if (items.Any(p => p.HOTKEY == cbxHotkey.SelectedValue.ToString()))
                {
                    cbxHotkey.Focus();
                    MsgBoxEx.Info(string.Format("Acc Keys[{0}] already repeated!", DataCache.CmnDict.FirstOrDefault(p => p.DICTID == "2010" && p.CODE == cbxHotkey.SelectedValue.ToString()).NAME));
                    return;
                }

                hotkey = cbxHotkey.SelectedValue.ToString();
            }

            if (txtColor.Text.Trim().Length < 1)
            {
                txtColor.Focus();
                MsgBoxEx.Info("Please input color");
                return;
            }

            IwrService service = wrService.GetService();

            var rs = service.AddClassificationItem(schemeid, (int)nudId.Value, txtClassification.Text.Trim(), txtColor.Text.Trim(), hotkey, (int)nudPriority.Value, DataCache.UserInfo.ID);

            if (rs == -1)
            {
                MsgBoxEx.Info("");
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
