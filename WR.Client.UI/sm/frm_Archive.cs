using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WR.Client.Utils;
using WR.WCF.Contract;
using WR.Client.WCF;
using WR.WCF.DataContract;

namespace WR.Client.UI
{
    public partial class frm_Archive : FormBase
    {
        private List<DiskInfoEntity> diskList;

        public frm_Archive()
        {
            InitializeComponent();
        }

        private void DataOperation(int type)
        {
            string sdate = string.Empty;
            string edate = string.Empty;

            if (type == 0)
            {
                if (rbtASpecified.Checked == true)
                {
                    if (dtpAfrom.Value > dtpAto.Value)
                    {
                        MsgBoxEx.Info("Start time cannot be greater than end time.");
                        return;
                    }

                    sdate = dtpAfrom.Value.ToString("yyyyMMdd");
                    edate = dtpAto.Value.ToString("yyyyMMdd");
                }
                else
                {
                    sdate = DateTime.MinValue.ToString("yyyyMMdd");
                    edate = dtpAless.Value.ToString("yyyyMMdd");
                }
            }
            else if (type == 1)
            {
                if (rbtRSpecified.Checked == true)
                {
                    if (dtpRfrom.Value > dtpRto.Value)
                    {
                        MsgBoxEx.Info("Start time cannot be greater than end time.");
                        return;
                    }

                    sdate = dtpRfrom.Value.ToString("yyyyMMdd");
                    edate = dtpRto.Value.ToString("yyyyMMdd");
                }
                else
                {
                    sdate = DateTime.MinValue.ToString("yyyyMMdd");
                    edate = dtpRless.Value.ToString("yyyyMMdd");
                }
            }
            else if (type == 2)
            {
                if (rbtDSpecified.Checked == true)
                {
                    if (dtpDfrom.Value > dtpDto.Value)
                    {
                        MsgBoxEx.Info("Start time cannot be greater than end time.");
                        return;
                    }

                    sdate = dtpDfrom.Value.ToString("yyyyMMdd");
                    edate = dtpDto.Value.ToString("yyyyMMdd");
                }
                else
                {
                    sdate = DateTime.MinValue.ToString("yyyyMMdd");
                    edate = dtpDless.Value.ToString("yyyyMMdd");
                }
            }

            try
            {
                ShowLoading(ToopEnum.loading);

                IwrService service = wrService.GetService();

                var msg = service.DataArchive(sdate, edate, type.ToString());

                MsgBoxEx.Info(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //3020
                IsysService service = sysService.GetService();

                var list = service.GetCmn("3020");

                if (list[0].CODE == "1")
                {
                    list[0].CODE = "0";

                    service.UpdateDict(list);
                }

                CloseLoading();
            }
        }

        private void frm_Archive_Load(object sender, EventArgs e)
        {
            IwrService service = wrService.GetService();
            //cbxFiles.Items.AddRange(service.GetDBFilesList().ToArray());

            diskList = service.GetDiskList();

            cbxDisk.Items.AddRange(diskList.Select(s => s.Name).ToArray());

            cbxDisk.SelectedIndex = 0;

            txtArchiveDate.Text = DataCache.CmnDict.Where(s => s.DICTID == "3021" && s.CODE == "0").Select(s => s.VALUE).FirstOrDefault();

            //ArchiveDate();

            var dbList = service.GetTableSpaceList();
            chartDb.Series[0].Points.Clear();
            chartDb.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            if (dbList.Count > 0)
            {
                chartDb.Series[0].Points.AddXY("Free Space(GB)", Math.Round(dbList[0].FreeSpace / 1024 / 1024 / 1024, 1));
                chartDb.Series[0].Points.AddXY("Used Space(GB)", Math.Round(dbList[0].UsedSpace / 1024 / 1024 / 1024, 1));
                chartDb.Series[0].IsValueShownAsLabel = true;
            }

            if (!DataCache.IsOracle)
            {
                groupBox4.Visible = false;
                groupBox8.Visible = false;
            }
        }

        private void ArchiveDate()
        {
            var lastDate = DataCache.CmnDict.Where(s => s.DICTID == "3021" && s.CODE == "0").Select(s => s.VALUE).FirstOrDefault();

            if (string.IsNullOrEmpty(lastDate))
            {
                MsgBoxEx.Info("Data not yet archived, please archive now.");
            }
            else
            {
                var intervalDay = (DateTime.Now - DateTime.ParseExact(lastDate,
                              "yyyyMMdd",
                               System.Globalization.CultureInfo.InvariantCulture)).Days;

                if (intervalDay > 7)
                {
                    MsgBoxEx.Info("It's been over seven days since the last archive, please archive now.");
                }
            }
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            if (MsgBoxEx.ConfirmYesNo("Are you sure to archive the data") == DialogResult.No)
                return;

            DataOperation(0);

            DataCache.RefreshCache();

            IsysService service = sysService.GetService();

            var entity = service.GetCmn("3021").FirstOrDefault(s => s.CODE == "0");

            if (entity != null)
                txtArchiveDate.Text = entity.VALUE;
        }

        private void btnRecovery_Click(object sender, EventArgs e)
        {
            if (MsgBoxEx.ConfirmYesNo("Are you sure to recovery the data") == DialogResult.No)
                return;

            DataOperation(1);

            DataCache.RefreshCache();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MsgBoxEx.ConfirmYesNo("Are you sure to delete the data") == DialogResult.No)
                return;

            DataOperation(2);
        }

        private void rbt_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtAless.Checked)
                rbtASpecified.Checked = false;
            else
                rbtASpecified.Checked = true;

            if (rbtRless.Checked)
                rbtRSpecified.Checked = false;
            else
                rbtRSpecified.Checked = true;

            if (rbtDless.Checked)
                rbtDSpecified.Checked = false;
            else
                rbtDSpecified.Checked = true;
        }

        private void btnDbExp_Click(object sender, EventArgs e)
        {
            if (MsgBoxEx.ConfirmYesNo("Are you sure to back up the database") == DialogResult.No)
                return;

            ShowLoading(ToopEnum.loading);

            IwrService service = wrService.GetService();
            service.ImpOrExpDatabase("0");

            CloseLoading();
            //timer1.Enabled = true;
        }

        private void btnDbImp_Click(object sender, EventArgs e)
        {
            if (MsgBoxEx.ConfirmYesNo("Are you sure to import the file") == DialogResult.No)
                return;

            if (string.IsNullOrEmpty(cbxFiles.Text))
            {
                MsgBoxEx.Info("Please select a file to import.");
                return;
            }

            ShowLoading(ToopEnum.loading);

            IwrService service = wrService.GetService();
            var rs = service.ImpOrExpDatabase("1", cbxFiles.Text) >= 0;

            if (rs)
            {
                cbxFiles.Items.AddRange(service.GetDBFilesList().ToArray());
            }

            CloseLoading();
        }

        private void cbxDisk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDisk.SelectedIndex != -1)
            {
                chartDisk.Series[0].Points.Clear();
                chartDisk.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

                chartDisk.Series[0].Points.AddXY("Free Space(GB)", diskList[cbxDisk.SelectedIndex].FreeSpace);
                chartDisk.Series[0].Points.AddXY("Used Space(GB)", diskList[cbxDisk.SelectedIndex].UsedSpace);
                chartDisk.Series[0].IsValueShownAsLabel = true;

                //alter
                var diskAlter = DataCache.CmnDict.Where(s => s.DICTID == "3022" && cbxDisk.Text.Contains(s.REMARK.ToUpper())).Select(s => s.VALUE).FirstOrDefault();

                if (!string.IsNullOrEmpty(diskAlter))
                {
                    var percent = Convert.ToInt32(diskList[cbxDisk.SelectedIndex].FreeSpace * 100 / diskList[cbxDisk.SelectedIndex].TotalSize);

                    if (percent < int.Parse(diskAlter))
                        MsgBoxEx.Warn("Disk remaining space is less than alert value, please clean up");

                }
            }
        }

        /// <summary>
        /// 表空间扩容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTableSpaceAdd_Click(object sender, EventArgs e)
        {
            if (MsgBoxEx.ConfirmYesNo("Are you sure to add the tablespace") == DialogResult.No)
                return;

            ShowLoading(ToopEnum.loading);

            try
            {
                IwrService service = wrService.GetService();
                var rs = service.AddTableSpace("");

                if (rs)
                {
                    MsgBoxEx.Info("Please restart the program.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                CloseLoading();
            }
        }
    }
}
