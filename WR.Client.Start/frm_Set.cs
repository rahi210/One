using System;
using System.Configuration;
using System.Windows.Forms;

using WR.Utils.Start;

namespace WR.Client.Start
{
    public partial class frm_Set : Form
    {
        public frm_Set()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtUrl.Text.Trim().Length < 1)
            {
                MessageBox.Show("Please input server ip!", "Info", MessageBoxButtons.OK);
                txtUrl.Focus();
                return;
            }

            if (txtPort.Text.Trim().Length < 1)
            {
                MessageBox.Show("Please input server port!", "Info", MessageBoxButtons.OK);
                txtPort.Focus();
                return;
            }

            Configuration config = Config.GetConfig();
            config.AppSettings.Settings.Remove("RemoteURL");
            config.AppSettings.Settings.Add("RemoteURL", string.Format("http://{0}:{1}", txtUrl.Text.Trim(), txtPort.Text.Trim()));

            //保存
            config.Save(ConfigurationSaveMode.Modified, false);

            DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
