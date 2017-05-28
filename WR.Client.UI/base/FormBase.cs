using System;
using System.Windows.Forms;

using WR.Utils;
using WR.Client.Utils;

namespace WR.Client.UI
{
    public partial class FormBase : Form
    {
        public enum ToopEnum
        {
            loading = 0,
            downloading = 1
        }

        //加载窗体
        private ProgressForm frm = null;
        //主窗体
        public frm_main frmMain = null;

        protected LoggerEx log = null;

        public FormBase()
        {
            InitializeComponent();

            log = LogService.Getlog(this.GetType());
        }

        protected void ShowLoading()
        {
            if (frm == null || frm.IsDisposed)
                frm = new ProgressForm();

            this.Enabled = false;
            //frm.TopMost = true;
            frm.Owner = this;
            frm.Show();

            frm.Update();
        }

        protected void ShowLoading(ToopEnum enm)
        {
            if (frm == null || frm.IsDisposed)
                frm = new ProgressForm();
            frm.type = (int)enm;

            this.Enabled = false;
            //frm.TopMost = true;
            frm.Owner = this;
            frm.Show();

            frm.Update();
        }

        protected void CloseLoading()
        {
            if (frm != null || !frm.IsDisposed)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        frm.Close();
                        this.Enabled = true;

                    }));
                }
                else
                {
                    frm.Close();
                    this.Enabled = true;
                }
            }
        }
    }
}
