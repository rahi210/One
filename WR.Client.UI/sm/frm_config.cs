using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;
using WR.Client.Controls;

namespace WR.Client.UI
{
    public partial class frm_config : FormBase
    {
        public frm_config()
        {
            InitializeComponent();
        }

        private void frm_config_Load(object sender, EventArgs e)
        {
            fpnl.Controls.Clear();
            IsysService service = sysService.GetService();
            var lst = service.GetRule().OrderByDescending(p => p.CREATEDDATE);

            if (lst != null && lst.Count() > 0)
            {
                foreach (var c in lst)
                {
                    WrRule erule = new WrRule();
                    erule.Ruleid = c.ID;
                    erule.Rulename = c.RULENAME;
                    erule.Ruledesr = c.DESCRP;
                    erule.Deviceid = c.DEVICE;
                    erule.Layer = c.LAYER;
                    erule.RType = "EDIT";
                    erule.OptRule += new RuleOptEventHandler(arule_OptRule);
                    fpnl.Controls.Add(erule);
                }
            }

            WrRule arule = new WrRule();
            arule.RType = "ADD";
            arule.OptRule += new RuleOptEventHandler(arule_OptRule);
            fpnl.Controls.Add(arule);

            cbxNotdone.Checked = DataCache.UserInfo.notdone;
            cbxDay.Checked = DataCache.UserInfo.theday;
            cbxLast.Checked = DataCache.UserInfo.lastday;
            cbxSpec.Checked = DataCache.UserInfo.specifiedday;
            if (cbxSpec.Checked)
            {
                dtDate.Value = DateTime.ParseExact(DataCache.UserInfo.fromday.ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None);
                dateTo.Value = DateTime.ParseExact(DataCache.UserInfo.today.ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None);
            }
            else
            {
                dtDate.Value = DateTime.Today.AddDays(-10);
                dateTo.Value = DateTime.Today;
            }

            cbxInterval.Checked = DataCache.UserInfo.IntervalDays > 0 ? true : false;
            if (cbxInterval.Checked)
                nudDay.Value = DataCache.UserInfo.IntervalDays;

            //过滤重复数据
            cbxFilter.Checked = DataCache.UserInfo.FilterData;

            InitClassificationRole(service);

            txtSinfPath.Text = DataCache.SinfPath;

            InitWaferOption(service);

            InitSystemOption(service);
        }

        private void InitWaferOption(IsysService service)
        {
            //yield
            var list = service.GetCmn("3010");

            if (list.Count > 0)
            {
                foreach (var l in list)
                {
                    switch (l.CODE)
                    {
                        case "0":
                            nudLotYield.Value = decimal.Parse(l.VALUE);
                            break;
                        case "1":
                            nudWaferYield.Value = decimal.Parse(l.VALUE);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void InitSystemOption(IsysService service)
        {
            //yield
            var list = service.GetCmn("3022");

            if (list.Count > 0)
            {
                foreach (var l in list)
                {
                    switch (l.CODE)
                    {
                        case "0":
                            nudDisk.Value = decimal.Parse(l.VALUE);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void InitClassificationRole(IsysService service)
        {
            //缺陷分类权限
            IwrService wService = wrService.GetService();
            clbClass.DataSource = wService.GetBaseClassificationItem();
            clbClass.ValueMember = "id";
            clbClass.DisplayMember = "name";

            var dictList = service.GetCmn("3000");

            for (int i = 0; i < clbClass.Items.Count; i++)
            {
                var code = ((WmClassificationItemEntity)clbClass.Items[i]).ID.ToString();

                var rs = dictList.Count(s => s.CODE == code) > 0;

                if (rs)
                    clbClass.SetItemChecked(i, true);
            }
        }

        void arule_OptRule(string type, string[] data)
        {
            IsysService service = sysService.GetService();
            switch (type)
            {
                case "ADD":
                    data[0] = Guid.NewGuid().ToString();
                    service.AddRule(data[0], data[1], data[2], data[3], data[4].ToLower() == "all" ? "*" : data[4]);
                    break;
                case "EDIT":
                    service.EditRule(data[0], data[1], data[2], data[3], data[4].ToLower() == "all" ? "*" : data[4]);
                    break;
                case "DEL":
                    service.DelRule(data[0]);
                    break;
                default:
                    break;
            }

            frm_config_Load(null, null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cbxSpec.Checked)
            {
                double totalday = (dateTo.Value - dtDate.Value).TotalDays;
                if (totalday > 10)
                {
                    dateTo.Focus();
                    MsgBoxEx.Info("Please select the time period within 10 days.");
                    return;
                }
                else if (totalday < 0)
                {
                    dtDate.Focus();
                    MsgBoxEx.Info("From date selection error.");
                    return;
                }
            }

            if (cbxInterval.Checked && nudDay.Value <= 0)
            {
                MsgBoxEx.Info("date interval is greater than 0.");
                nudDay.Focus();
                return;
            }

            System.Configuration.Configuration config = WR.Utils.Config.GetConfig();
            config.AppSettings.Settings.Remove("notdone");
            config.AppSettings.Settings.Add("notdone", (cbxNotdone.Checked ? "0" : "1"));
            config.AppSettings.Settings.Remove("theday");
            config.AppSettings.Settings.Add("theday", (cbxDay.Checked ? "0" : "1"));
            config.AppSettings.Settings.Remove("lastday");
            config.AppSettings.Settings.Add("lastday", (cbxLast.Checked ? "0" : "1"));
            config.AppSettings.Settings.Remove("specifiedday");
            config.AppSettings.Settings.Add("specifiedday", (cbxSpec.Checked ? "0" : "1"));
            config.AppSettings.Settings.Remove("framday");
            config.AppSettings.Settings.Add("framday", dtDate.Value.ToString("yyyyMMdd"));
            config.AppSettings.Settings.Remove("today");
            config.AppSettings.Settings.Add("today", dateTo.Value.ToString("yyyyMMdd"));

            //设置间隔天
            config.AppSettings.Settings.Remove("intervalDays");
            config.AppSettings.Settings.Add("intervalDays", cbxInterval.Checked ? nudDay.Value.ToString() : "0");

            config.AppSettings.Settings.Remove("duplicate_data_visible");
            config.AppSettings.Settings.Add("duplicate_data_visible", (cbxFilter.Checked ? "0" : "1"));

            config.Save();

            WR.Utils.Config.Refresh();

            string done = WR.Utils.Config.GetAppSetting("notdone");
            if (string.IsNullOrEmpty(done) || done != "1")
                DataCache.UserInfo.notdone = true;
            else
                DataCache.UserInfo.notdone = false;

            string theday = WR.Utils.Config.GetAppSetting("theday");
            if (string.IsNullOrEmpty(theday) || theday != "1")
                DataCache.UserInfo.theday = true;
            else
                DataCache.UserInfo.theday = false;

            string lastday = WR.Utils.Config.GetAppSetting("lastday");
            if (!string.IsNullOrEmpty(lastday) && lastday == "0")
                DataCache.UserInfo.lastday = true;
            else
                DataCache.UserInfo.lastday = false;

            string specifiedday = WR.Utils.Config.GetAppSetting("specifiedday");
            if (!string.IsNullOrEmpty(specifiedday) && specifiedday == "0")
            {
                DataCache.UserInfo.specifiedday = true;

                string fromday = WR.Utils.Config.GetAppSetting("framday");
                int day = 0;
                if (int.TryParse(fromday, out day))
                    DataCache.UserInfo.fromday = day;
                else
                    DataCache.UserInfo.fromday = int.Parse(DateTime.Today.ToString("yyyyMMdd"));

                string today = WR.Utils.Config.GetAppSetting("today");
                int tday = 0;
                if (int.TryParse(today, out tday))
                    DataCache.UserInfo.today = tday;
                else
                    DataCache.UserInfo.today = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
            }
            else
                DataCache.UserInfo.specifiedday = false;

            string intervalDays = WR.Utils.Config.GetAppSetting("intervalDays");
            if (!string.IsNullOrEmpty(intervalDays))
            {
                int idays = 0;
                if (int.TryParse(intervalDays, out idays))
                    DataCache.UserInfo.IntervalDays = idays;
            }

            string filterdata = WR.Utils.Config.GetAppSetting("duplicate_data_visible");
            if (string.IsNullOrEmpty(filterdata) || filterdata != "1")
                DataCache.UserInfo.theday = true;
            else
                DataCache.UserInfo.theday = false;

            MsgBoxEx.Info("Save success, please refresh the data.");
        }

        private void cbxDay_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxDay.Checked)
            {
                cbxLast.Checked = false;
                cbxSpec.Checked = false;

                cbxInterval.Checked = false;
            }
        }

        private void cbxLast_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxLast.Checked)
            {
                cbxDay.Checked = false;
                cbxSpec.Checked = false;

                cbxInterval.Checked = false;
            }
        }

        private void cbxSpec_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxSpec.Checked)
            {
                cbxLast.Checked = false;
                cbxDay.Checked = false;

                cbxInterval.Checked = false;
            }
        }

        private void cbxInterval_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxInterval.Checked)
            {
                cbxLast.Checked = false;
                cbxDay.Checked = false;
                cbxSpec.Checked = false;
            }
        }

        private void BtnClassRole_Click(object sender, EventArgs e)
        {
            List<CMNDICT> dictList = new List<CMNDICT>();

            if (clbClass.SelectedItems.Count == 0)
            {
                MsgBoxEx.Info("Please select at least one item.");
                return;
            }

            foreach (WmClassificationItemEntity item in clbClass.CheckedItems)
            {
                CMNDICT model = new CMNDICT();

                model.DICTID = "3000";
                model.CODE = item.ID.ToString();
                model.NAME = item.NAME.ToString();

                dictList.Add(model);
            }

            IsysService service = sysService.GetService();
            service.AddDict(dictList);

            MsgBoxEx.Info("Save success, please refresh the data.");
        }

        /// <summary>
        /// Set SINF path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSinfPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSinfPath.Text = dialog.SelectedPath;


                System.Configuration.Configuration config = WR.Utils.Config.GetConfig();
                config.AppSettings.Settings.Remove("sinfPath");
                config.AppSettings.Settings.Add("sinfPath", dialog.SelectedPath);

                config.Save();

                WR.Utils.Config.Refresh();

                DataCache.SinfPath = dialog.SelectedPath;
            }

        }

        private void btnWaferYield_Click(object sender, EventArgs e)
        {
            IsysService service = sysService.GetService();

            //yield
            var list = service.GetCmn("3010");

            if (list.Count > 0)
            {
                foreach (var l in list)
                {
                    switch (l.CODE)
                    {
                        case "0":
                            l.VALUE = nudLotYield.Value.ToString();
                            break;
                        case "1":
                            l.VALUE = nudWaferYield.Value.ToString();
                            break;
                        default:
                            break;
                    }
                }
            }

            service.UpdateDict(list);

            MsgBoxEx.Info("Save success, please refresh the data.");
        }

        private void btnSystem_Click(object sender, EventArgs e)
        {
            IsysService service = sysService.GetService();

            var list = service.GetCmn("3022");

            if (list.Count > 0)
            {
                foreach (var l in list)
                {
                    switch (l.CODE)
                    {
                        case "0":
                            l.VALUE = nudDisk.Value.ToString();
                            break;
                        default:
                            break;
                    }
                }
            }

            service.UpdateDict(list);

            MsgBoxEx.Info("Save success, please refresh the data.");
        }
    }
}
