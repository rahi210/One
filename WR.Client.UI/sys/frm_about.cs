using System;
using System.Windows.Forms;

namespace WR.Client.UI
{
    public partial class frm_about : Form
    {
        public frm_about()
        {
            InitializeComponent();
        }

        private void frm_about_Load(object sender, EventArgs e)
        {
            label1.Text = string.Format("Version:{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
