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
using System.Text;

namespace WR.Client.UI
{
    public partial class frm_config : FormBase
    {
        public frm_config()
        {
            InitializeComponent();
        }

        private void YieldInit()
        {
            //tabControl2.SelectTab(1);
            var frmYield = new frm_yieldsetting();
            frmYield.FormBorderStyle = FormBorderStyle.None;
            frmYield.TopLevel = false;
            frmYield.Show();
            frmYield.Parent = tabControl2.TabPages[1];
            frmYield.Dock = DockStyle.Fill;
            tabControl2.SelectedTab.AutoScroll = true;
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
                dtDate.Value = DateTime.Today.AddDays(-365);
                dateTo.Value = DateTime.Today;
            }

            cbxInterval.Checked = DataCache.UserInfo.IntervalDays > 0 ? true : false;
            if (cbxInterval.Checked)
                nudDay.Value = DataCache.UserInfo.IntervalDays;

            //过滤重复数据
            cbxFilter.Checked = DataCache.UserInfo.FilterData;

            InitClassificationRole(service);

            txtSinfPath.Text = DataCache.SinfPath;

            if (DataCache.BinCodeType == "10")
                rbnDecimal.Checked = true;
            else
                rbnHexadecimal.Checked = true;

            if (DataCache.SinfType == "000")
                rbnThreeByte.Checked = true;
            else
                rbnTwoByte.Checked = true;

            InitWaferOption(service);

            InitSystemOption(service);

            YieldInit();

            InitHotKey();
        }

        /// <summary>
        /// 快捷键
        /// </summary>
        private void InitHotKey()
        {
            if (DataCache.UserInfo.USERID != "Admin")
            {
                tabPage7.Parent = null;
                return;
            }

            IwrService wService = wrService.GetService();

            List<CMNDICT> hotkey = DataCache.CmnDict.Where(p => p.DICTID == "2010").ToList();
            hotkey.Add(new CMNDICT() { DICTID = "2010", CODE = null, NAME = "-" });

            if (this.InvokeRequired)
                this.Invoke(new Action(() =>
                {
                    colHotKey.DisplayMember = "NAME";
                    colHotKey.ValueMember = "CODE";
                    colHotKey.DataSource = hotkey;
                }));
            else
            {
                colHotKey.DisplayMember = "NAME";
                colHotKey.ValueMember = "CODE";
                colHotKey.DataSource = hotkey;
            }

            grdClass.AutoGenerateColumns = false;
            grdClass.DataSource = wService.GetBaseClassificationItem();
        }

        private void SetItem(WmClassificationItemEntity item)
        {
            if (item == null)
                return;

            //已经保存
            if (!string.IsNullOrEmpty(item.InspectionType))
                return;

            item.InspectionType = string.Format("{0}|{1}", item.HOTKEY, item.COLOR);
        }

        /// <summary>
        /// 保存自定义缺陷
        /// </summary>
        /// <returns></returns>
        private bool SaveHotKey()
        {
            grdClass.EndEdit();

            List<WmClassificationItemEntity> items = grdClass.DataSource as List<WmClassificationItemEntity>;
            if (items == null || items.Count < 1)
                return false;

            StringBuilder sbt = new StringBuilder();
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.InspectionType))
                {
                    if (items.Any(p => p.ID != item.ID && p.HOTKEY == item.HOTKEY && !string.IsNullOrEmpty(item.HOTKEY)))
                    {
                        MsgBoxEx.Info(string.Format("Acc Keys[{0}] already repeated!", DataCache.CmnDict.FirstOrDefault(p => p.DICTID == "2010" && p.CODE == item.HOTKEY).NAME));
                        return false;
                    }

                    sbt.AppendFormat(";{0}|{1}|{2}", item.ID, item.HOTKEY, item.COLOR);
                }
            }

            if (sbt.Length < 1)
                return true;

            IwrService service = wrService.GetService();
            int res = service.UpdateClassificationItemUser(sbt.ToString(), "");
            if (res == 1)
            {
                items.ForEach((p) => { p.InspectionType = ""; });
            }

            return true;
        }

        private void SetClsMenu()
        {
            tlsEdit.Enabled = true;
            tlsEdit.Checked = false;

            tlsSave.Enabled = false;
            tlsClassCancel.Enabled = false;

            //colHotKey.ReadOnly = true;
            grdClass.Columns["colHotKey"].ReadOnly = true;
        }

        private void mnFront_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == tlsSave)
            {
                if (SaveHotKey())
                {
                    SetClsMenu();
                }
            }
            else if (e.ClickedItem == tlsClassCancel)
            {
                SetClsMenu();

                List<WmClassificationItemEntity> items = grdClass.DataSource as List<WmClassificationItemEntity>;
                if (items == null || items.Count < 1)
                    return;

                items.ForEach((p) =>
                {
                    if (!string.IsNullOrEmpty(p.InspectionType))
                    {
                        string[] r = p.InspectionType.Split(new char[] { '|' });
                        p.HOTKEY = r[0];
                        p.COLOR = r[1];
                        p.InspectionType = "";
                    }
                });

                grdClass.Invalidate();
            }
            else
            {
                //colHotKey.ReadOnly = false;
                grdClass.Columns["colHotKey"].ReadOnly = false;

                tlsEdit.Enabled = false;
                tlsEdit.Checked = true;

                tlsSave.Enabled = true;
                tlsClassCancel.Enabled = true;
            }
        }

        private void grdClass_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var item = grdClass.Rows[e.RowIndex].DataBoundItem as WmClassificationItemEntity;
            SetItem(item);
        }

        private void grdClass_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grdClass.Columns[e.ColumnIndex].Name == "Column4")
                {
                    if (tlsEdit.Enabled)
                        return;

                    //if (clrDialog.ShowDialog() == DialogResult.OK)
                    //{
                    //    var ent = grdClass.Rows[e.RowIndex].DataBoundItem as WMCLASSIFICATIONITEM;
                    //    SetItem(ent);
                    //    ent.COLOR = ColorTranslator.ToHtml(clrDialog.Color);
                    //    grdClass.Invalidate();
                    //    grdClass.ClearSelection();
                    //}
                }
            }
        }

        /// <summary>
        /// 显示class定义颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdClass_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            if (grdClass.Columns[e.ColumnIndex].DataPropertyName == "COLOR")
            {
                string color = grdClass[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                e.Value = null;
                e.CellStyle.BackColor = ConvterColor(color);
            }
        }

        private Color ConvterColor(string color)
        {
            try
            {
                var newColor = Color.FromName(color);

                if (!newColor.IsKnownColor)
                {
                    if (!color.StartsWith("#"))
                        color = "#" + color;

                    if (color.Length > 7)
                        newColor = ColorTranslator.FromHtml(color.Substring(0, 7));
                    else
                        newColor = ColorTranslator.FromHtml(color);
                }

                return newColor;
            }
            catch
            {
                if (!color.StartsWith("#"))
                    color = "#" + color;

                if (color.Length > 7)
                    return ColorTranslator.FromHtml(color.Substring(0, 7));

                return ColorTranslator.FromHtml(color);
            }
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

        bool arule_OptRule(string type, string[] data)
        {
            IsysService service = sysService.GetService();
            var rs = string.Empty;

            switch (type)
            {
                case "ADD":
                    data[0] = Guid.NewGuid().ToString();
                    rs = service.AddRule(data[0], data[1], data[2], data[3], data[4].ToLower() == "all" ? "*" : data[4]);
                    break;
                case "EDIT":
                    rs = service.EditRule(data[0], data[1], data[2], data[3], data[4].ToLower() == "all" ? "*" : data[4]);
                    break;
                case "DEL":
                    rs = service.DelRule(data[0]);
                    break;
                default:
                    break;
            }

            if (rs == "-1")
            {
                MsgBoxEx.Info("Name already exist");
                return false;
            }

            frm_config_Load(null, null);

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cbxSpec.Checked)
            {
                double totalday = (dateTo.Value - dtDate.Value).TotalDays;
                if (totalday > 365)
                {
                    dateTo.Focus();
                    MsgBoxEx.Info("Please select the time period within 365 days.");
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
                DataCache.UserInfo.FilterData = true;
            else
                DataCache.UserInfo.FilterData = false;

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

           var rs= service.UpdateDict(list);

           if (rs == "0")
           {
               DataCache.CmnDict = service.GetCmn("");
           }

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

        private void rbnDecimal_CheckedChanged(object sender, EventArgs e)
        {
            System.Configuration.Configuration config = WR.Utils.Config.GetConfig();
            config.AppSettings.Settings.Remove("binCodeType");
            config.AppSettings.Settings.Add("binCodeType", "10");

            config.Save();

            WR.Utils.Config.Refresh();

            DataCache.BinCodeType = "10";
        }

        private void rbnHexadecimal_CheckedChanged(object sender, EventArgs e)
        {
            System.Configuration.Configuration config = WR.Utils.Config.GetConfig();
            config.AppSettings.Settings.Remove("binCodeType");
            config.AppSettings.Settings.Add("binCodeType", "16");

            config.Save();

            WR.Utils.Config.Refresh();

            DataCache.BinCodeType = "16";
        }

        private void rbnTwoByte_CheckedChanged(object sender, EventArgs e)
        {
            System.Configuration.Configuration config = WR.Utils.Config.GetConfig();
            config.AppSettings.Settings.Remove("sinfType");
            config.AppSettings.Settings.Add("sinfType", "00");

            config.Save();

            WR.Utils.Config.Refresh();

            DataCache.SinfType = "00";
        }

        private void rbnThreeByte_CheckedChanged(object sender, EventArgs e)
        {
            System.Configuration.Configuration config = WR.Utils.Config.GetConfig();
            config.AppSettings.Settings.Remove("sinfType");
            config.AppSettings.Settings.Add("sinfType", "000");

            config.Save();

            WR.Utils.Config.Refresh();

            DataCache.SinfType = "000";
        }
        //private void btnYieldSettings_Click(object sender, EventArgs e)
        //{
        //    frm_yieldsetting frm = new frm_yieldsetting();
        //    frm.ShowDialog();
        //}
    }
}
