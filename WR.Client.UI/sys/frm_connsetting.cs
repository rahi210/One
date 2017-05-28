using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

using WR.Utils;

namespace WR.Client.UI
{
    public partial class frm_connsetting : Form
    {
        public frm_connsetting()
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

        private void frm_connsetting_Load(object sender, EventArgs e)
        {
            try
            {
                Configuration config = Config.GetConfig();
                var k = config.AppSettings.Settings["RemoteURL"];
                if (k != null && k.Value != null)
                {
                    string http = k.Value;
                    txtUrl.Text = http.Substring(0, http.LastIndexOf(":")).ToLower().Replace("http://", "");
                    txtPort.Text = http.Substring(http.LastIndexOf(":") + 1).TrimEnd('/');
                }
            }
            catch { }
        }
    }
}
