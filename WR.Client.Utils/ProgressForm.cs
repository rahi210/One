using System;
using System.Windows.Forms;

namespace WR.Client.Utils
{
    public partial class ProgressForm : Form
    {
        private string[] pp = new string[] { ".", "..", "...", "....", ".....", "......" };
        private int p = 0;
        public int type = 0;
        private string textFormat = "loading{0}";

        public ProgressForm()
        {
            InitializeComponent();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            //数据加载
            if (type == 0)
            { }
            else if (type == 1)
            {
                //文件下载
                textFormat = "exporting{0}";
                lbl.Text = "exporting";
            }
            else
            {
                //文件下载
                textFormat = "saving{0}";
                lbl.Text = "saving";
            }

            timer1.Interval = 500;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl.Text = string.Format(textFormat, pp[p]);
            p++;

            if (p > 5)
                p = 0;

            this.Update();
        }

        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
