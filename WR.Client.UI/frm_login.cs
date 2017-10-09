using System;
using System.Threading;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;

using System.Linq;

namespace WR.Client.UI
{
    public partial class frm_login : FormBase
    {
        //IsysService service;

        public frm_login()
        {
            InitializeComponent();

            //service = sysService.GetService();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string userid = txtUser.Text.Trim();
            if (string.IsNullOrEmpty(userid))
            {
                MsgBoxEx.Info(MessageConst.frm_login_msg001);
                return;
            }

            string pwd = txtPwd.Text.Trim();
            if (string.IsNullOrEmpty(pwd))
            {
                MsgBoxEx.Info(MessageConst.frm_login_msg002);
                return;
            }

            btnOK.Enabled = false;
            btnReset.Enabled = false;
            txtUser.Enabled = false;
            txtPwd.Enabled = false;

            lblMsg.Visible = true;

            var thr = new Thread(() => { Login(userid, pwd); });
            thr.Start();
        }

        private void Login(string userid, string pwd)
        {
            IsysService service = sysService.GetService();
            UserInfoEntity ent = service.Login(userid, pwd);

            if (ent.IsOK == 0)
            {
                DataCache.UserInfo = ent;
                LoadLocalSettings(DataCache.UserInfo);

                DataCache.CmnDict = service.GetCmn("");
                DataCache.Tbmenus = service.GetMenuByUserId(userid);

                var msg = GetExamInfo();

                if (!string.IsNullOrEmpty(msg))
                {
                    MsgBoxEx.Info(msg);
                    return;
                }

                //加载数据
                DataCache.RefreshCache();

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        System.Configuration.Configuration config = WR.Utils.Config.GetConfig();
                        config.AppSettings.Settings.Remove("userid");
                        config.AppSettings.Settings.Add("userid", userid);
                        config.Save();
                        WR.Utils.Config.Refresh();
                        frmMain.SetForm();
                    }));
                }
                else
                {
                    System.Configuration.Configuration config = WR.Utils.Config.GetConfig();
                    config.AppSettings.Settings.Remove("userid");
                    config.AppSettings.Settings.Add("userid", userid);
                    config.Save();
                    WR.Utils.Config.Refresh();
                    frmMain.SetForm();
                }
            }
            else
            {
                string msg = MessageConst.frm_login_msg003;
                switch (ent.IsOK)
                {
                    case -99:
                        msg = MessageConst.frm_login_msg003;
                        break;
                    case -1:
                        msg = MessageConst.const_msg001;
                        break;
                    case -2:
                        msg = MessageConst.const_msg002;
                        break;
                    case -3:
                        msg = MessageConst.const_msg003;
                        break;
                    case -4:
                        msg = MessageConst.const_msg001;
                        break;
                    default:
                        msg = MessageConst.const_msg001;
                        break;
                }

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        MsgBoxEx.Info(msg);
                        btnOK.Enabled = true;
                        btnReset.Enabled = true;
                        lblMsg.Visible = false;
                        txtUser.Enabled = true;
                        txtPwd.Enabled = true;
                        txtPwd.Focus();
                    }));
                }
                else
                {
                    MsgBoxEx.Info(msg);
                    btnOK.Enabled = true;
                    btnReset.Enabled = true;
                    lblMsg.Visible = false;
                    txtUser.Enabled = true;
                    txtPwd.Enabled = true;
                    txtPwd.Focus();
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtUser.Focus();
            txtUser.Clear();
            txtPwd.Clear();
        }

        /// <summary>
        /// 加载本地配置
        /// </summary>
        /// <param name="ent"></param>
        private void LoadLocalSettings(UserInfoEntity ent)
        {
            string done = WR.Utils.Config.GetAppSetting("notdone");
            if (string.IsNullOrEmpty(done) || done != "1")
                ent.notdone = true;

            string theday = WR.Utils.Config.GetAppSetting("theday");
            if (string.IsNullOrEmpty(theday) || theday != "1")
                ent.theday = true;

            string lastday = WR.Utils.Config.GetAppSetting("lastday");
            if (!string.IsNullOrEmpty(lastday) && lastday == "0")
                ent.lastday = true;

            string specifiedday = WR.Utils.Config.GetAppSetting("specifiedday");
            if (!string.IsNullOrEmpty(specifiedday) && specifiedday == "0")
            {
                ent.specifiedday = true;

                string fromday = WR.Utils.Config.GetAppSetting("framday");
                int day = 0;
                if (int.TryParse(fromday, out day))
                    ent.fromday = day;
                else
                    ent.fromday = int.Parse(DateTime.Today.ToString("yyyyMMdd"));

                string today = WR.Utils.Config.GetAppSetting("today");
                int tday = 0;
                if (int.TryParse(today, out tday))
                    ent.today = tday;
                else
                    ent.today = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
            }

            //时间间隔
            string intervalDays = WR.Utils.Config.GetAppSetting("intervalDays");
            if (!string.IsNullOrEmpty(intervalDays))
            {
                int idays = 0;
                if (int.TryParse(intervalDays, out idays))
                    ent.IntervalDays = idays;
            }

            //sinf输出路径
            string sinfPath = WR.Utils.Config.GetAppSetting("sinfPath");
            if (!string.IsNullOrEmpty(sinfPath))
            {
                DataCache.SinfPath = sinfPath;
            }
            else
            {
                DataCache.SinfPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SINF");
            }
        }

        private void frm_login_Load(object sender, EventArgs e)
        {
            string userid = WR.Utils.Config.GetAppSetting("userid");
            if (!string.IsNullOrEmpty(userid))
            {
                txtUser.Text = userid;
            }
        }

        private void frm_login_Shown(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim().Length > 0)
                txtPwd.Focus();
            else
                txtUser.Focus();
        }

        /// <summary>
        /// 0：正常 -1：没有考试计划 -2:考试已结束
        /// </summary>
        /// <returns></returns>
        private string GetExamInfo()
        {
            var msg = string.Empty;

            var hasExamRole = DataCache.Tbmenus.Count(s => s.MENUCODE == "50003") > 0;

            if (hasExamRole)
            {
                IwrService service = wrService.GetService();

                var rs = service.GetPaper(DataCache.UserInfo.ID);

                switch (rs)
                {
                    case 0:
                        DataCache.HasExam = true;
                        break;
                    case -1:
                        msg = "The exam hasn't started yet.";
                        break;
                    case -2:
                        msg = "The exam is over.";
                        break;
                    default:
                        break;
                }
            }

            return "";
        }
    }
}
