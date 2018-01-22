using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using WR.Utils;
using WR.Client.Utils;
using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;
using WR.Client.Controls;

namespace WR.Client.UI
{
    public partial class frm_main : Form
    {
        /// <summary>
        /// 选中的菜单
        /// </summary>
        private Control _focusedWrMenuItem;
        private FormBase frm;
        private FormBase frmReport;

        public frm_main()
        {
            InitializeComponent();

            Rectangle rt = SystemInformation.WorkingArea;
            this.Width = rt.Width;
            this.Height = rt.Height;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if (MsgBoxEx.ConfirmYesNo("Are you sure to exit?") == DialogResult.No)
                return;

            if (frm.GetType().Name == "frm_preview")
            {
                var frmP = frm as frm_preview;

                frm.Close();
            }

            this.Close();
        }

        private void picMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 菜单面板收缩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblArrow_Click(object sender, EventArgs e)
        {
            if (pnlLeft.Width > 12)
            {
                pnlLeft.Width = 12;
                lblArrow.Image = global::WR.Client.UI.Properties.Resources.pright;
            }
            else
            {
                pnlLeft.Width = 235;
                lblArrow.Image = global::WR.Client.UI.Properties.Resources.pleft;
            }
        }

        private void frm_main_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            cbxLang.SelectedIndex = 0;
            label4.Text = string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
            pnlLeft.Enabled = false;
            pnlLeft.Width = 1;
            lblArrow.Image = global::WR.Client.UI.Properties.Resources.pright;

            ShowForm("");
        }

        private void ArchiveDate()
        {
            if (mnuArchive.Visible)
            {
                var lastDate = DataCache.CmnDict.Where(s => s.DICTID == "3021" && s.CODE == "0").Select(s => s.VALUE).FirstOrDefault();

                if (string.IsNullOrEmpty(lastDate))
                {
                    //MsgBoxEx.Info("Data not yet archived, please archive now.");
                }
                else
                {
                    var intervalDay = (DateTime.Now - DateTime.ParseExact(lastDate,
                                  "yyyyMMdd",
                                   System.Globalization.CultureInfo.InvariantCulture)).Days;

                    if (intervalDay > 7)
                    {
                        //MsgBoxEx.Info("It's been over seven days since the last archive, please archive now.");
                    }
                }
            }
        }

        public void SetForm()
        {
            LogService.InitializeService(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Start.exe.config"));

            pnlLeft.Enabled = true;
            pnlLeft.Width = 215;
            lblArrow.Image = global::WR.Client.UI.Properties.Resources.pleft;
            _focusedWrMenuItem = null;

            if (pnlContext.Controls.Count > 0)
            {
                foreach (Control item in pnlContext.Controls)
                {
                    //if (item is Form)
                    //    ((Form)item).Close();
                    if (item is Form && item.Name != "frm_report")
                    {
                        ((Form)item).Close();

                        pnlContext.Controls.Remove(item);
                    }
                    else
                    {
                        if (item is Form)
                            item.Hide();
                    }
                }

                pnlContext.Controls.Clear();
            }

            mnuSelection.Visible = false;
            mnuReview.Visible = false;
            mnuSelect.Visible = false;
            mnuSetting.Visible = false;
            mnuArchive.Visible = false;
            mnuExam.Visible = false;

            lblLeftPwd.Visible = false;
            lblLeftOptions.Visible = false;
            lblLeftRole.Visible = false;
            lblLeftUser.Visible = false;

            lblUser.Text = string.Format("      welcome,{0}", DataCache.UserInfo.USERID);
            //根据权限显示菜单
            if (DataCache.Tbmenus == null && DataCache.Tbmenus.Count < 1)
                return;

            foreach (Control item in pnlLeft.Controls)
            {
                if (item.Tag == null)
                    continue;

                string code = item.Tag.ToString();
                var mn = DataCache.Tbmenus.FirstOrDefault(p => p.MENUCODE == code);
                if (mn != null)
                    item.Visible = true;
            }

            //若有考试权限 其他菜单权限不显示
            if (DataCache.HasExam)
            {
                mnuSelection.Visible = false;
                mnuReview.Visible = true;
            }

            //调整位置
            int y = 58;
            if (mnuSelection.Visible)
            {
                y += 63;
            }
            if (mnuReview.Visible)
            {
                mnuReview.Location = new Point(4, y);
                y += 63;
            }
            if (mnuSelect.Visible)
            {
                mnuSelect.Location = new Point(4, y);
                y += 63;
            }

            if (mnuArchive.Visible)
            {
                mnuArchive.Location = new Point(4, y);
                y += 63;
            }

            if (mnuExam.Visible)
            {
                mnuExam.Location = new Point(4, y);
                y += 63;
            }

            if (lblLeftPwd.Visible || lblLeftOptions.Visible || lblLeftRole.Visible || lblLeftUser.Visible)
            {
                mnuSetting.Visible = true;
                mnuSetting.Location = new Point(4, y);
                y += 61;
                if (lblLeftPwd.Visible)
                {
                    lblLeftPwd.Location = new Point(58, y);
                    y += 32;
                }
                if (lblLeftOptions.Visible)
                {
                    lblLeftOptions.Location = new Point(58, y);
                    y += 32;
                }
                if (lblLeftRole.Visible)
                {
                    lblLeftRole.Location = new Point(58, y);
                    y += 32;
                }
                if (lblLeftUser.Visible)
                {
                    lblLeftUser.Location = new Point(58, y);
                    y += 32;
                }

                y += 5;
            }
            mnuLogout.Location = new Point(4, y);

            ArchiveDate();
        }

        /// <summary>
        /// 一级菜单显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void mnuSelect_ItemClick(object sender, EventArgs e)
        {
            WrMenuItem wri = sender as WrMenuItem;
            if (wri == _focusedWrMenuItem)
                return;

            WrMenuItem item = _focusedWrMenuItem as WrMenuItem;
            if (item != null)
                item.ItemBgColor = Color.Transparent;
            else
            {
                Label lbl = _focusedWrMenuItem as Label;
                if (lbl != null)
                    lbl.BackColor = Color.Transparent;
            }

            _focusedWrMenuItem = wri;
            wri.ItemBgColor = Color.Gray;

            ShowForm(wri.WrText);
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="text"></param>
        private void ShowForm(string text)
        {
            if (pnlContext.Controls.Count > 0)
            {
                foreach (Control item in pnlContext.Controls)
                {
                    if (item is Form && item.Name != "frm_report")
                    {
                        ((Form)item).Close();

                        pnlContext.Controls.Remove(item);
                    }
                    else
                        item.Hide();
                }

                //pnlContext.Controls.Clear();
            }

            switch (text)
            {
                case "Wafer Selection":
                    string tmpresultid = "";
                    if (frm != null && frm.Name == "frm_preview")
                        tmpresultid = ((frm_preview)frm).Resultid;
                    frm = new frm_review();
                    ((frm_review)frm).selectedResultid = tmpresultid;
                    frm.frmMain = this;
                    break;
                case "Wafer Review":
                    if (DataCache.HasExam)
                    {
                        frm = new frm_paper();
                        //((frm_paper)frm).Oparams = Oparams;
                    }
                    else
                    {
                        frm = new frm_preview();
                        ((frm_preview)frm).Oparams = Oparams;
                    }
                    frm.frmMain = this;
                    break;
                case "Defect Report":
                    if (frmReport == null)
                    {
                        frm = new frm_report();
                        frmReport = frm;
                    }
                    else
                    {
                        frm = frmReport;
                        frm.Visible = true;
                    }

                    ((frm_report)frm).Oparams = Oparams;
                    frm.frmMain = this;
                    break;
                case "Data Manage":
                    frm = new frm_Archive();
                    //((frm_Archive)frm).Oparams = Oparams;
                    frm.frmMain = this;
                    break;
                case "Exam Manage":
                    frm = new frm_exam();
                    frm.frmMain = this;
                    break;
                case "- Change Password":
                    frm = new frm_changepwd();
                    frm.frmMain = this;
                    break;
                //case "- Configurations":
                //    frm = new frm_config();
                //    break;
                case "- Options":
                    //frm = new frm_options();
                    frm = new frm_config();
                    break;
                case "- User":
                    frm = new frm_user();
                    break;
                case "- Role":
                    frm = new frm_role();
                    break;
                case "Logout":
                    _focusedWrMenuItem = null;
                    mnuLogout.ItemBgColor = Color.Transparent;

                    if (MsgBoxEx.ConfirmYesNo(MessageConst.frm_main_msg001) != DialogResult.Yes)
                        return;

                    pnlLeft.Enabled = false;
                    pnlLeft.Width = 12;
                    lblArrow.Image = global::WR.Client.UI.Properties.Resources.pright;
                    ShowForm("");

                    return;
                default:
                    frm = new frm_login();
                    frm.frmMain = this;
                    break;
            }

            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.Parent = pnlContext;
            frm.Tag = this;

            frm.Show();
        }

        public void ReLogin()
        {
            lblLeftPwd.BackColor = Color.Transparent;
            pnlLeft.Enabled = false;
            pnlLeft.Width = 12;
            lblArrow.Image = global::WR.Client.UI.Properties.Resources.pright;
            ShowForm("");
        }

        private void lblLeftUser_Click(object sender, EventArgs e)
        {
            SubMenuShow(sender);
        }

        private void SubMenuShow(object sender)
        {
            Label wri = sender as Label;
            if (wri == _focusedWrMenuItem)
                return;

            WrMenuItem item = _focusedWrMenuItem as WrMenuItem;
            if (item != null)
                item.ItemBgColor = Color.Transparent;
            else
            {
                Label lbl = _focusedWrMenuItem as Label;
                if (lbl != null)
                    lbl.BackColor = Color.Transparent;
            }

            _focusedWrMenuItem = wri;
            wri.BackColor = Color.Gray;

            ShowForm(wri.Text);
        }

        private void lblLeftRole_Click(object sender, EventArgs e)
        {
            SubMenuShow(sender);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private string[] _oparams;
        /// <summary>
        /// 参数
        /// </summary>
        public string[] Oparams
        {
            get { return _oparams; }
            set { _oparams = value; }
        }

        private void lblHelp_Click(object sender, EventArgs e)
        {
            //cnsHelp.Show(MousePosition.X, MousePosition.Y);
            frm_about frm = new frm_about();
            frm.ShowDialog();
        }

        private void tlsConn_Click(object sender, EventArgs e)
        {
            frm_connsetting frm = new frm_connsetting();
            frm.ShowDialog();
        }

        private void tlsAbout_Click(object sender, EventArgs e)
        {
            frm_about frm = new frm_about();
            frm.ShowDialog();
        }

        private void lblConnction_Click(object sender, EventArgs e)
        {
            frm_connsetting frm = new frm_connsetting();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MsgBoxEx.Info("Saved successfully, Please run the program again.");
                Application.Exit();
            }
        }
    }
}
