using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Data;
using System.IO;
using System.ComponentModel;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.Utils;
using WR.WCF.Contract;
using WR.WCF.DataContract;

namespace WR.Client.UI
{
    public partial class frm_report : FormBase
    {
        private string[] _oparams;
        /// <summary>
        /// 参数
        /// </summary>
        public string[] Oparams
        {
            get { return _oparams; }
            set { _oparams = value; }
        }

        private bool dateFlag = false;

        private List<WmidentificationEntity> Idens = null;

        public frm_report()
        {
            InitializeComponent();
        }

        private void SetChtDefect()
        {
            chtDefect.Legends[0].Enabled = false;
            //chtDefect.ChartAreas[0].Area3DStyle.Enable3D = true;
            //chtDefect.ChartAreas[0].Area3DStyle.PointDepth = 50;
            //chtDefect.ChartAreas[0].Area3DStyle.PointGapDepth = 50;
            //chtDefect.ChartAreas[0].Area3DStyle.WallWidth = 0;
            chtDefect.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chtDefect.ChartAreas[0].AxisY.MajorGrid.Enabled = true;//不显示网格线
        }

        private void SetChtPolat()
        {
            //chartPolat.Legends[0].Enabled = false;
            chartPolat.ChartAreas[0].Area3DStyle.Enable3D = false;
            //chartPolat.ChartAreas[0].Area3DStyle.PointDepth = 30;
            //chartPolat.ChartAreas[0].Area3DStyle.PointGapDepth = 30;
            //chartPolat.ChartAreas[0].Area3DStyle.WallWidth = 0;
            chartPolat.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartPolat.ChartAreas[0].AxisY.MajorGrid.Enabled = true;//不显示网格线
            //chartPolat.ChartAreas[0].AxisX.Minimum = 1;
        }

        private void SetChtYield()
        {
            //chartYield.Legends[0].Enabled = false;
            chartYield.ChartAreas[0].Area3DStyle.Enable3D = false;
            //chartYield.ChartAreas[0].Area3DStyle.PointDepth = 50;
            //chartYield.ChartAreas[0].Area3DStyle.PointGapDepth = 50;
            //chartYield.ChartAreas[0].Area3DStyle.WallWidth = 0;
            chartYield.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartYield.ChartAreas[0].AxisY.MajorGrid.Enabled = true;//不显示网格线
        }

        private void frm_report_Load(object sender, EventArgs e)
        {
            //tabPolat.Hide();
            tabPolat.Parent = null;

            SetChtDefect();
            SetChtPolat();
            SetChtYield();

            if (SystemInformation.WorkingArea.Width <= 1025)
            {
                groupBox1.Height = 112;
                label4.Location = new System.Drawing.Point(5, 70);
                cbxLayer.Location = new System.Drawing.Point(52, 70);
                label3.Location = new System.Drawing.Point(220, 70);
                cbxLot.Location = new System.Drawing.Point(260, 70);
            }
            else if (SystemInformation.WorkingArea.Width <= 1370)
            {
                groupBox1.Height = 112;
                label3.Location = new System.Drawing.Point(38, 74);
                cbxLot.Location = new System.Drawing.Point(76, 74);
                cbxLot.Size = new System.Drawing.Size(230, 20);
            }

            dateFlag = false;
            dtDate.Value = DateTime.Now.AddMonths(-1);
            dateTo.Value = DateTime.Now;

            //DataCache.IdentifcationInfo.ForEach((p) =>
            //{
            //    p.DVALUE = string.Format("{0}|{1}|{2}|", p.DEVICE, p.LAYER, p.LOT);
            //});

            //cbxLot.DisplayMember = "LOT";
            //cbxLot.ValueMember = "DVALUE";
            //cbxLot.DataSource = DataCache.IdentifcationInfo;

            var serie = chtDefect.Series[0];
            serie.Points.Clear();

            cbxDevice.DisplayMember = "DEVICE";
            cbxDevice.ValueMember = "DEVICE";

            cbxLayer.DisplayMember = "LAYER";
            cbxLayer.ValueMember = "DVALUE";

            cbxLot.DisplayMember = "LOT";
            cbxLot.ValueMember = "DVALUE";

            LoadInfo();

            if (Oparams != null && Oparams.Length == 4)
            {
                string[] data = Oparams[2].Split(new char[] { '|' });
                cbxDevice.Text = data[0];
                cbxDevice.SelectedValue = data[0];

                //cbxLayer.Text = data[1];
                cbxLayer.SelectedValue = string.Format("{0}|{1}||", data[0], data[1]);

                //cbxLot.Text = Oparams[0];
                cbxLot.SelectedValue = Oparams[2];
                switch (Oparams[1])
                {
                    case "1":
                        tabReport.SelectedTab = tabDensity;
                        break;
                    case "2":
                        tabReport.SelectedTab = tabDefective;
                        break;
                    case "3":
                        tabReport.SelectedTab = tabDefectList;
                        break;
                    case "4":
                        tabReport.SelectedTab = tabDieSum;
                        break;
                    case "5":
                        tabReport.SelectedTab = tabGood;
                        break;
                    case "6":
                        tabReport.SelectedTab = tabDie;
                        break;
                    case "7":
                        tabReport.SelectedTab = tabYield;
                        break;
                    case "8":
                        tabReport.SelectedTab = tabPolat;
                        break;
                    default:
                        break;
                }

                btnQuery_Click(null, null);
            }
            dateFlag = true;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            switch (tabReport.SelectedTab.Name)
            {
                case "tabDensity":
                    var dt = grdDensity.DataSource as BindingCollection<WmdensityReport>;
                    if (dt != null)
                        dt.Clear();
                    break;
                case "tabDefective":
                    var dtCate = grdCategory.DataSource as BindingCollection<WmCategoryReport>;
                    if (dtCate != null)
                        dtCate.Clear();

                    var serie = chtDefect.Series[0];
                    serie.Points.Clear();
                    break;
                case "tabDie":
                    var dtDie = grdDie.DataSource as BindingCollection<WmDefectiveDieReport>;
                    if (dtDie != null)
                        dtDie.Clear();
                    break;
                case "tabDieSum":
                    var dtSum = grdInspDie.DataSource as BindingCollection<WmInpDieReport>;
                    if (dtSum != null)
                        dtSum.Clear();
                    break;
                case "tabDefectList":
                    var dtlist = grdDefectList.DataSource as BindingCollection<WmDefectListReport>;
                    if (dtlist != null)
                        dtlist.Clear();
                    break;
                case "tabGood":
                    var dtGood = grdGoodDie.DataSource as BindingCollection<WmGoodDieReport>;
                    if (dtGood != null)
                        dtGood.Clear();
                    break;
                case "tabYield":
                    grdYields.DataSource = null;
                    chartYield.Series.Clear();
                    break;
                case "tabPolat":
                    grdPolat.DataSource = null;
                    chartPolat.Series.Clear();
                    break;
                default:
                    break;
            }

            double tdays = (dateTo.Value - dtDate.Value).TotalDays;
            //if (tdays > 10)
            //{
            //    dateTo.Focus();
            //    MsgBoxEx.Info("Please select the time period within 10 days.");
            //    return;
            //}
            if (tdays < 0)
            {
                dtDate.Focus();
                MsgBoxEx.Info("From date selection error.");
                return;
            }

            if (cbxLot.Text.Trim().Length < 1 || cbxLot.SelectedValue == null)
            {
                cbxLot.Focus();
                MsgBoxEx.Info("Please input lot");
                return;
            }

            //必须选中lot
            if (tabReport.SelectedTab == tabPolat && cbxLot.SelectedIndex < 1)
            {
                cbxLot.Focus();
                MsgBoxEx.Info("Please input lot");
                return;
            }

            ShowLoading(ToopEnum.loading);

            try
            {


                switch (tabReport.SelectedTab.Name)
                {
                    case "tabDensity":
                        ShowDensity();
                        break;
                    case "tabDefective":
                        ShowCountCategory();
                        break;
                    case "tabDie":
                        ShowDefectiveDie();
                        break;
                    case "tabDieSum":
                        ShowInspDie();
                        break;
                    case "tabDefectList":
                        ShowDefectiveList();
                        break;
                    case "tabGood":
                        //ShowGoodDie();
                        ShowGoodDieNew();
                        break;
                    case "tabYield":
                        ShowYield();
                        break;
                    case "tabPolat":
                        ShowPolat();
                        break;
                    default:
                        break;
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

        private string GetLot()
        {
            string lot = "";
            if (cbxLot.SelectedValue == null || cbxLot.SelectedValue.ToString().Length < 1 || cbxLot.SelectedIndex < 0)
                lot = "||" + cbxLot.Text.Trim();
            else
                lot = cbxLot.SelectedValue.ToString();

            if (tabReport.SelectedTab.Name == "tabYield")
            {
                if (cbxLayer.SelectedValue != null)
                    lot = cbxLayer.SelectedValue.ToString();
                else
                    lot = cbxDevice.SelectedValue.ToString();
            }
            return lot;
        }

        /// <summary>
        /// 查询density
        /// </summary>
        private void ShowDensity()
        {
            IwrService service = wrService.GetService();
            var lst = service.GetDensityReport(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"), DataCache.UserInfo.FilterData ? "1" : "0");
            var blst = new BindingCollection<WmdensityReport>(lst);
            grdDensity.DataSource = blst;

            //总数
            int[] total = service.GetCountInspected(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            lblDensityTotal.Text = total[0].ToString("N0");

            if (lst != null && lst.Count > 0)
            {
                //lblDensityDefective.Text = lst.Count((p) => { return p.ID != 0; }).ToString("N0");
                var dftive = lst.Where(p => p.ID != 0).DistinctBy(t => t.DIEADDRESS + t.SUBSTRATE_ID + t.DEVICE + t.LAYER + t.LOT);
                lblDensityDefective.Text = dftive.Count().ToString("N0");
                int good = total[0] - dftive.Count();
                lblDensityGood.Text = good < 0 ? "0" : good.ToString("N0");

                lblDensityRepNum.Text = lst.DistinctBy((p) => { return p.SUBSTRATE_ID + p.DEVICE + p.LAYER + p.LOT; }).Count().ToString("N0");
                lblDensityListNum.Text = lblDensityRepNum.Text;
            }
            else
            {
                lblDensityDefective.Text = "0";
                lblDensityGood.Text = lblDensityTotal.Text;
                lblDensityRepNum.Text = total[1].ToString("N0");
                lblDensityListNum.Text = total[1].ToString("N0");
            }
        }

        private void grdDensity_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grdDensity.Columns[e.ColumnIndex].DataPropertyName == "DESCRIPTION")
                {
                    var ent = grdDensity.Rows[e.RowIndex].DataBoundItem as WmdensityReport;
                    e.Value = string.Format("({0}){1}", ent.ID, e.Value);
                }
            }
        }

        /// <summary>
        /// 获取分类汇总
        /// </summary>
        private void ShowCountCategory()
        {
            IwrService service = wrService.GetService();
            var lst = service.GetCategoryReport(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"), DataCache.UserInfo.FilterData ? "1" : "0");
            var blst = new BindingCollection<WmCategoryReport>(lst);
            grdCategory.DataSource = blst;

            ChtShow(lst);
        }

        /// <summary>
        /// 显示图表
        /// </summary>
        /// <param name="list"></param>
        private void ChtShow(List<WmCategoryReport> list)
        {
            //list = list.OrderByDescending(s => s.ID).ToList();

            var serie = chtDefect.Series[0];
            serie.Points.Clear();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].DefectNum < 1)
                    continue;

                DataPoint p = new DataPoint();
                p.AxisLabel = list[i].DESCRIPTION;
                //p.SetValueXY(i + 1, list[i].DefectNum);
                p.SetValueXY(list.Count - i, list[i].DefectNum);
                serie.Points.Add(p);
            }
        }

        /// <summary>
        /// 获取缺陷的die
        /// </summary>
        private void ShowDefectiveDie()
        {
            IwrService service = wrService.GetService();
            var lst = service.GetDefectiveDieReport(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"), DataCache.UserInfo.FilterData ? "1" : "0");

            lst.ForEach((p) =>
            {
                decimal area = 0;
                if (decimal.TryParse(p.AREA_, out area))
                    p.AREA_ = string.Format("{0:0.000}", area);

                var sz = p.SIZE_.Split(new char[] { ',' });
                p.SIZE_ = string.Format("{0},{1}", Math.Round(decimal.Parse(sz[0]), 3), Math.Round(decimal.Parse(sz[1]), 3));
            });

            var blst = new BindingCollection<WmDefectiveDieReport>(lst);
            grdDie.DataSource = blst;

            int[] total = service.GetCountInspected(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            lblDieTotal.Text = total[0].ToString("N0");


            if (lst != null && lst.Count > 0)
            {
                //lblDieDefective.Text = lst.Count((p) => { return p.ID != 0; }).ToString("N0");
                var dftive = lst.Where(p => p.ID != 0).DistinctBy(t => t.DIEADDRESS + t.SUBSTRATE_ID + t.DEVICE + t.LAYER + t.LOT);
                lblDieDefective.Text = dftive.Count().ToString("N0");

                //int good = total[0] - lst.Count((p) => { return p.ID != 0; });
                int good = total[0] - dftive.Count();
                lblDieGood.Text = good < 0 ? "0" : good.ToString("N0");

                lblDieRepNum.Text = lst.DistinctBy((p) => { return p.SUBSTRATE_ID + p.DEVICE + p.LAYER + p.LOT; }).Count().ToString("N0");
                lblDieListNum.Text = lblDieRepNum.Text;
            }
            else
            {
                lblDieDefective.Text = "0";
                lblDieGood.Text = lblDieTotal.Text;
                lblDieRepNum.Text = total[1].ToString("N0");
                lblDieListNum.Text = lblDieRepNum.Text;
            }
        }

        private void grdDie_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grdDie.Columns[e.ColumnIndex].DataPropertyName == "DESCRIPTION")
                {
                    var ent = grdDie.Rows[e.RowIndex].DataBoundItem as WmDefectiveDieReport;
                    e.Value = string.Format("({0}){1}", ent.ID, e.Value);
                }
            }
        }

        private void ShowInspDie()
        {
            IwrService service = wrService.GetService();
            var lst = service.GetDieInspDieReport(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"), DataCache.UserInfo.FilterData ? "1" : "0");

            lst.ForEach((p) =>
            {
                if (!p.DIEQTY.HasValue || p.DIEQTY.Value < 1)
                    p.Yield = 0;
                else
                {
                    //p.Yield = Math.Round((p.DIEQTY.Value - p.DEFECTNUM.Value) / (p.DIEQTY.Value * 1.0m), 4) * 100;
                    p.Yield = Math.Truncate((p.DIEQTY.Value - p.DEFECTNUM.Value) / (p.DIEQTY.Value * 1.0m) * 100000) / 1000;
                }
            });

            var blst = new BindingCollection<WmInpDieReport>(lst);
            grdInspDie.DataSource = blst;

            int[] total = service.GetCountInspected(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            lblInspDieTotal.Text = total[0].ToString("N0");

            if (lst != null && lst.Count > 0)
            {
                lblInspDieDefective.Text = lst.Sum(p => p.DEFECTNUM).Value.ToString("N0");

                int good = total[0] - lst.Sum(p => p.DEFECTNUM).Value;
                lblInspDieGood.Text = good < 0 ? "0" : good.ToString("N0");

                lblInspDieRepNum.Text = lst.DistinctBy((p) => { return p.SUBSTRATE_ID + p.DEVICE + p.LAYER + p.LOT; }).Count().ToString("N0");
                lblInspDieListNum.Text = lblInspDieRepNum.Text;
            }
            else
            {
                lblInspDieDefective.Text = "0";
                lblInspDieGood.Text = lblInspDieTotal.Text;
                lblInspDieRepNum.Text = total[1].ToString("N0");
                lblInspDieListNum.Text = lblInspDieRepNum.Text;
            }
        }

        private void ShowDefectiveList()
        {
            IwrService service = wrService.GetService();
            var lst = service.GetDefectListReport(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"), DataCache.UserInfo.FilterData ? "1" : "0");

            if (DataCache.UserInfo.FilterData)

                lst.ForEach((p) =>
                {
                    p.CodeDESCRIPTION = string.Format("({0}){1}", p.ID, p.DESCRIPTION);
                    try
                    {
                        var sz = p.SIZE_.Split(new char[] { ',' });

                        p.XSIZE_ = Math.Round(decimal.Parse(sz[0]), 3).ToString();
                        p.YSIZE_ = Math.Round(decimal.Parse(sz[1]), 3).ToString();

                        decimal area = 0;
                        if (decimal.TryParse(p.AREA_, out area))
                            p.AREA_ = Math.Round(area, 3).ToString();
                    }
                    catch { }
                });

            var blst = new BindingCollection<WmDefectListReport>(lst);
            grdDefectList.DataSource = blst;

            int[] total = service.GetCountInspected(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            lblListDieTotal.Text = total[0].ToString("N0");

            if (lst != null && lst.Count > 0)
            {
                //lblListDieDefective.Text = lst.Count((p) => { return p.ID != 0; }).ToString("N0");
                var dftive = lst.Where(p => p.ID != 0).DistinctBy(t => t.DIEADDRESS + t.SUBSTRATE_ID + t.DEVICE + t.LAYER + t.LOT);
                lblListDieDefective.Text = dftive.Count().ToString("N0");
                lblListDefect.Text = lst.Count((p) => { return p.ID != 0; }).ToString("N0");

                //int good = total[0] - lst.Count((p) => { return p.ID != 0; });
                int good = total[0] - dftive.Count();
                lblListDieGood.Text = good < 0 ? "0" : good.ToString("N0");

                lblListDieRepNum.Text = lst.DistinctBy((p) => { return p.SUBSTRATE_ID + p.DEVICE + p.LAYER + p.LOT; }).Count().ToString("N0");
                lblListDieListNum.Text = lblListDieRepNum.Text;
            }
            else
            {
                lblListDieDefective.Text = "0";
                lblListDieGood.Text = lblListDieTotal.Text;
                lblListDieRepNum.Text = total[1].ToString("N0");
                lblListDieListNum.Text = lblListDieRepNum.Text;
            }
        }

        private void ShowGoodDie()
        {
            IwrService service = wrService.GetService();
            var lst = service.GetGoodDieReport(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"), DataCache.UserInfo.FilterData ? "1" : "0");
            lst.ForEach((p) =>
            {
                p.GOODCNT = p.INSPCNT - p.DEFECTNUM;
                p.PERCENT = double.Parse(string.Format("{0:0.###}", (double.Parse(p.GOODCNT.Value.ToString()) * 100) / p.INSPCNT.Value));

                DateTime indt;
                DateTime.TryParseExact(p.COMPLETIONTIME.ToString(), "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out indt);
                p.INSPECTIONDATE = indt;
            });
            var blst = new BindingCollection<WmGoodDieReport>(lst);
            grdGoodDie.DataSource = blst;

            int[] total = service.GetCountInspected(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            lblGoodDieTotal.Text = total[0].ToString("N0");

            if (lst != null && lst.Count > 0)
            {
                lblGoodDieDefective.Text = lst.Sum((p) => { return p.DEFECTNUM; }).Value.ToString("N0");

                int good = total[0] - lst.Sum((p) => { return p.DEFECTNUM; }).Value;
                lblGoodDieGood.Text = good < 0 ? "0" : good.ToString("N0");

                lblGoodDieRepNum.Text = lst.DistinctBy((p) => { return p.SUBSTRATE_ID + p.DEVICE + p.LAYER + p.LOT; }).Count().ToString("N0");
                lblGoodDieListNum.Text = lblGoodDieRepNum.Text;
            }
            else
            {
                lblGoodDieDefective.Text = "0";
                lblGoodDieGood.Text = lblGoodDieTotal.Text;
                lblGoodDieRepNum.Text = total[1].ToString("N0");
                lblGoodDieListNum.Text = lblGoodDieRepNum.Text;
            }
        }

        private void ShowGoodDieNew()
        {
            IwrService service = wrService.GetService();
            var lst = service.GetGoodDieReport(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"), DataCache.UserInfo.FilterData ? "1" : "0");
            lst.ForEach((p) =>
            {
                p.GOODCNT = p.INSPCNT - p.DEFECTNUM;
                p.PERCENT = double.Parse(string.Format("{0:0.###}", (double.Parse(p.GOODCNT.Value.ToString()) * 100) / p.INSPCNT.Value));

                DateTime indt;
                DateTime.TryParseExact(p.COMPLETIONTIME.ToString(), "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out indt);
                p.INSPECTIONDATE = indt;
            });

            DataTable dtGood = new DataTable();

            DataColumn col = new DataColumn();
            col.ColumnName = "LOT";
            col.Caption = "LotId";
            dtGood.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "SUBSTRATE_ID";
            col.Caption = "WaferId";
            dtGood.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "INSPECTIONDATE";
            col.Caption = "Inspection Date";
            dtGood.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "INSPCNT";
            col.Caption = "Inspection Die";
            dtGood.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "GOODCNT";
            col.Caption = "Good Die";
            dtGood.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "DEFECTNUM";
            col.Caption = "Defect Die";
            dtGood.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "PERCENT";
            col.Caption = "Percent Of Good Die(%)";
            dtGood.Columns.Add(col);

            var items = service.GetClassificationItemsByLayer(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            var itemsBy = items.Where(s => s.ID != 0).OrderBy(p => p.ID);

            foreach (var item in itemsBy)
            {
                col = dtGood.Columns.Add(item.NAME, typeof(Int64));
                col.ColumnName = item.NAME;
                col.Caption = item.ITEMID;
                col.DefaultValue = 0;
            }

            var itemsSum = service.GetItemsSummaryByLot(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));

            foreach (var item in lst)
            {
                var dr = dtGood.NewRow();

                dr["LOT"] = item.LOT;
                dr["SUBSTRATE_ID"] = item.SUBSTRATE_ID;

                dr["INSPECTIONDATE"] = item.INSPECTIONDATE;
                dr["INSPCNT"] = item.INSPCNT;
                dr["GOODCNT"] = item.GOODCNT;
                dr["DEFECTNUM"] = item.DEFECTNUM;
                dr["PERCENT"] = item.PERCENT;

                var tt = itemsSum.Where(p => p.RESULTID == item.RESULTID && p.Substrate_id == item.SUBSTRATE_ID);

                foreach (var t in tt)
                {
                    //if (dt.Columns.Contains(t.Inspclassifiid))
                    //    dr[t.Inspclassifiid] = t.NumCnt;
                    foreach (DataColumn c in dtGood.Columns)
                    {
                        if (c.Caption == t.Inspclassifiid)
                        {
                            Int64 tcnt = Int64.Parse(dr[c.Ordinal].ToString() == "" ? "0" : dr[c.Ordinal].ToString());
                            if (t.NumCnt.HasValue)
                                dr[c.Ordinal] = tcnt + t.NumCnt;
                            else
                                dr[c.Ordinal] = tcnt;

                            continue;
                        }
                    }
                }

                dtGood.Rows.Add(dr);
            }

            List<string> listColumn = new List<string>();
            listColumn.AddRange(new string[] { "LOT", "SUBSTRATE_ID", "INSPECTIONDATE", "INSPCNT", "GOODCNT", "DEFECTNUM", "PERCENT" });

            for (int i = 7; i < dtGood.Columns.Count; i++)
            {
                object data = dtGood.Compute(string.Format("SUM([{0}])", dtGood.Columns[i].ColumnName), "");
                //if (data == DBNull.Value)
                //    drTotal[i] = 0;
                //else
                //    drTotal[i] = data;

                if (data != DBNull.Value && Convert.ToInt32(data) != 0)
                    listColumn.Add(dtGood.Columns[i].ColumnName);
            }

            DataTable dtNew = dtGood.DefaultView.ToTable(false, listColumn.ToArray());

            grdGoodDie.DataSource = dtNew;

            int[] total = service.GetCountInspected(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            lblGoodDieTotal.Text = total[0].ToString("N0");

            if (lst != null && lst.Count > 0)
            {
                lblGoodDieDefective.Text = lst.Sum((p) => { return p.DEFECTNUM; }).Value.ToString("N0");

                int good = total[0] - lst.Sum((p) => { return p.DEFECTNUM; }).Value;
                lblGoodDieGood.Text = good < 0 ? "0" : good.ToString("N0");

                lblGoodDieRepNum.Text = lst.DistinctBy((p) => { return p.SUBSTRATE_ID + p.DEVICE + p.LAYER + p.LOT; }).Count().ToString("N0");
                lblGoodDieListNum.Text = lblGoodDieRepNum.Text;
            }
            else
            {
                lblGoodDieDefective.Text = "0";
                lblGoodDieGood.Text = lblGoodDieTotal.Text;
                lblGoodDieRepNum.Text = total[1].ToString("N0");
                lblGoodDieListNum.Text = lblGoodDieRepNum.Text;
            }
        }

        private void ShowYield()
        {
            IwrService service = wrService.GetService();
            var items = service.GetClassificationItemsByLayer(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            var itemsBy = items.Where(s => s.ID != 0).OrderBy(p => p.ID);

            DataTable dt = new DataTable();
            var col = dt.Columns.Add("Layer", typeof(string));
            col.ColumnName = "ColLayer";
            col.Caption = "Layer";
            foreach (var item in itemsBy)
            {
                //if (item.ID == 0)
                //    continue;

                col = dt.Columns.Add(item.NAME, typeof(Int64));
                //col.ColumnName = item.ITEMID;//string.Format("[{0}]", item.ITEMID);
                //col.Caption = item.NAME;
                col.ColumnName = item.NAME;
                col.Caption = item.ITEMID;
                col.DefaultValue = 0;
            }
            //添加汇总列
            col = dt.Columns.Add("Inspected Die_Y2", typeof(Int64));
            col.ColumnName = "Inspected Die_Y2";

            var itemsSum = service.GetItemsSummaryByLot(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            var defSum = service.GetDefectSummaryByLot(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));

            var dlyr = itemsSum.Distinct(new LayerSumComparint());

            foreach (var dl in dlyr)
            {
                DataRow dr = dt.NewRow();
                var tt = itemsSum.Where(p => p.Device == dl.Device && p.Layer == dl.Layer);
                dr["ColLayer"] = dl.Layer;

                foreach (var t in tt)
                {
                    //if (dt.Columns.Contains(t.Inspclassifiid))
                    //    dr[t.Inspclassifiid] = t.NumCnt;
                    foreach (DataColumn c in dt.Columns)
                    {
                        if (c.Caption == t.Inspclassifiid)
                        {
                            Int64 tcnt = Int64.Parse(dr[c.Ordinal].ToString() == "" ? "0" : dr[c.Ordinal].ToString());
                            if (t.NumCnt.HasValue)
                                dr[c.Ordinal] = tcnt + t.NumCnt;
                            else
                                dr[c.Ordinal] = tcnt;

                            continue;
                        }
                    }
                }

                dr["Inspected Die_Y2"] = defSum.FirstOrDefault(p => p.Device == dl.Device && p.Layer == dl.Layer).NumCnt;
                dt.Rows.Add(dr);
            }

            //汇总列
            DataRow drTotal = dt.NewRow();
            drTotal["ColLayer"] = "Total";
            List<string> listColumn = new List<string>();

            listColumn.Add("ColLayer");

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                object data = dt.Compute(string.Format("SUM([{0}])", dt.Columns[i].ColumnName), "");
                if (data == DBNull.Value)
                    drTotal[i] = 0;
                else
                    drTotal[i] = data;

                if (Convert.ToInt32(drTotal[i]) != 0)
                    listColumn.Add(dt.Columns[i].ColumnName);
            }

            dt.Rows.Add(drTotal);

            DataTable dtNew = dt.DefaultView.ToTable(false, listColumn.ToArray());
            grdYields.DataSource = dtNew;

            ShowYieldChart2(dtNew);
        }

        private void ShowPolat()
        {
            IwrService service = wrService.GetService();
            var items = service.GetClassificationItemsByLot(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            var itemsBy = items.Where(s => s.ID != 0).OrderBy(p => p.ID);

            DataTable dt = new DataTable();
            var col = dt.Columns.Add("Lot_Wafer", typeof(string));
            col.ColumnName = "Lot_Wafer";

            foreach (var item in itemsBy)
            {
                //if (item.ID == 0)
                //    continue;

                col = dt.Columns.Add(item.NAME, typeof(Int64));
                //col.ColumnName = item.ITEMID;//string.Format("[{0}]", item.ITEMID);
                //col.Caption = item.NAME;
                col.ColumnName = item.NAME;
                col.Caption = item.ITEMID;
                col.DefaultValue = 0;
            }

            //添加汇总列
            col = dt.Columns.Add("Inspected Die_Y2", typeof(Int64));
            col.ColumnName = "Inspected Die_Y2";

            var itemsSum = service.GetItemsSummaryByLot(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));
            var defSum = service.GetDefectSummaryByLot(GetLot(), dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"));

            var dlyr = itemsSum.Distinct(new LotSumComparint());
            int cnt = dlyr.Count();

            foreach (var dl in dlyr)
            {
                DataRow dr = dt.NewRow();
                var tt = itemsSum.Where(p => p.Device == dl.Device && p.Layer == dl.Layer && p.Lot == dl.Lot && p.Substrate_id == dl.Substrate_id);
                dr["Lot_Wafer"] = dl.Substrate_id;

                foreach (var t in tt)
                {
                    //if (dt.Columns.Contains(t.Inspclassifiid))
                    //    dr[t.Inspclassifiid] = t.NumCnt;
                    foreach (DataColumn c in dt.Columns)
                    {
                        if (c.Caption == t.Inspclassifiid)
                        {
                            if (t.NumCnt.HasValue)
                                dr[c.Ordinal] = t.NumCnt;
                            else
                                dr[c.Ordinal] = 0;

                            continue;
                        }
                    }
                }

                dr["Inspected Die_Y2"] = defSum.FirstOrDefault(p => p.Device == dl.Device && p.Layer == dl.Layer && p.Lot == dl.Lot && p.Substrate_id == dl.Substrate_id).NumCnt;
                dt.Rows.Add(dr);
            }

            var tmpDt = dt.Copy();
            for (int i = 1; i < tmpDt.Columns.Count; i++)
            {
                object data = tmpDt.Compute(string.Format("SUM([{0}])", tmpDt.Columns[i].ColumnName), "");
                if (data == DBNull.Value || Convert.ToInt32(data) == 0)
                    dt.Columns.Remove(tmpDt.Columns[i].ColumnName);
            }

            //汇总列
            DataRow drTotal = dt.NewRow();
            drTotal["Lot_Wafer"] = "Total";
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                object data = dt.Compute(string.Format("SUM([{0}])", dt.Columns[i].ColumnName), "");
                if (data == DBNull.Value || Convert.ToInt32(data) == 0)
                    drTotal[i] = 0;
                else
                    drTotal[i] = data;
            }
            dt.Rows.Add(drTotal);

            grdPolat.DataSource = dt;

            ShowPolatChart(dt);
        }

        /// <summary>
        /// 导出xls报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (cbxLot.Text.Trim().Length < 1)
            {
                MsgBoxEx.Info("Please input lot id.");
                cbxLot.Focus();
                return;
            }

            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "xls文件(*.xls)|*.xls";

            switch (tabReport.SelectedTab.Name)
            {
                case "tabDensity":
                    sd.FileName = "Defect Density Report.xls";// string.Format("Defect Density For ({0}).xls", cbxLot.Text.Trim());
                    break;
                case "tabDefective":
                    sd.FileName = "Defect Count By Category Report.xls";//string.Format("Defect Count By Category For ({0}).xls", cbxLot.Text.Trim());
                    break;
                case "tabDie":
                    sd.FileName = "Defective Die List Report.xls";//string.Format("Defective Die List For ({0}).xls", cbxLot.Text.Trim());
                    break;
                case "tabDieSum":
                    sd.FileName = "Summary Of Die Inspection Report.xls";//string.Format("Summary Of Die Inspection For ({0}).xls", cbxLot.Text.Trim());
                    break;
                case "tabDefectList":
                    sd.FileName = "General Defect List Report.xls";//string.Format("General Defect List For ({0}).xls", cbxLot.Text.Trim());
                    break;
                case "tabGood":
                    sd.FileName = "Summary Of Good Die Report.xls";// string.Format("Summary Of Good Die By For ({0}).xls", cbxLot.Text.Trim());
                    break;
                case "tabYield":
                    sd.FileName = "Yield and Defect Report.xls";//string.Format("General Defect List For ({0}).xls", cbxLot.Text.Trim());
                    break;
                case "tabPolat":
                    sd.FileName = "Defect Polat Report.xls";//string.Format("Summary Of Good Die By For ({0}).xls", cbxLot.Text.Trim());
                    break;
                default:
                    break;
            }

            if (sd.ShowDialog() != DialogResult.OK)
                return;

            switch (tabReport.SelectedTab.Name)
            {
                case "tabDensity":
                    NpoiHelper.GridToExcelByNPOI("Defect Density", cbxLot.Text.Trim(),
                        string.Format("{0:yyyy/MM/dd}-{1:yyyy/MM/dd}", dtDate.Value, dateTo.Value),
                        new string[] { "Total die Inspected:", lblDensityTotal.Text, "Total number of defective die:", lblDensityDefective.Text },
                        new string[] { "Total number of good die:", lblDensityGood.Text, "Number of wafer in this report:", lblDensityRepNum.Text },
                        new string[] { "List of wafer numbers:", lblDensityListNum.Text },
                        grdDensity, sd.FileName, true);
                    break;
                case "tabDefective":
                    NpoiHelper.GridToExcelByNPOI("Defect Count By Category", cbxLot.Text.Trim(),
                        string.Format("{0:yyyy/MM/dd}-{1:yyyy/MM/dd}", dtDate.Value, dateTo.Value),
                        null, null, null,
                        grdCategory, sd.FileName, false);
                    break;
                case "tabDie":
                    NpoiHelper.GridToExcelByNPOI("Defective Die List", cbxLot.Text.Trim(),
                       string.Format("{0:yyyy/MM/dd}-{1:yyyy/MM/dd}", dtDate.Value, dateTo.Value),
                       new string[] { "Total die Inspected:", lblDieTotal.Text, "Total number of defective die:", lblDieDefective.Text },
                       new string[] { "Total number of good die:", lblDieGood.Text, "Number of wafer in this report:", lblDieRepNum.Text },
                       new string[] { "List of wafer numbers:", lblDieListNum.Text },
                       grdDie, sd.FileName, true);
                    break;
                case "tabDieSum":
                    NpoiHelper.GridToExcelByNPOI("Summary Of Die Inspection", cbxLot.Text.Trim(),
                       string.Format("{0:yyyy/MM/dd}-{1:yyyy/MM/dd}", dtDate.Value, dateTo.Value),
                       new string[] { "Total die Inspected:", lblInspDieTotal.Text, "Total number of defective die:", lblInspDieDefective.Text },
                       new string[] { "Total number of good die:", lblInspDieGood.Text, "Number of wafer in this report:", lblInspDieRepNum.Text },
                       new string[] { "List of wafer numbers:", lblInspDieListNum.Text },
                       grdInspDie, sd.FileName, true);
                    break;
                case "tabDefectList":
                    NpoiHelper.GridToExcelByNPOI("General Defect List", cbxLot.Text.Trim(),
                       string.Format("{0:yyyy/MM/dd}-{1:yyyy/MM/dd}", dtDate.Value, dateTo.Value),
                       new string[] { "Total die Inspected:", lblListDieTotal.Text, "Total number of defective die:", lblListDieDefective.Text },
                       new string[] { "Total number of good die:", lblListDieGood.Text, "Total number of defects:", lblListDefect.Text },
                       new string[] { "Number of wafer in this report:", lblListDieRepNum.Text, "List of wafer numbers:", lblListDieListNum.Text },
                       grdDefectList, sd.FileName, true);
                    break;
                case "tabGood":
                    NpoiHelper.GridToExcelByNPOI("Summary Of Good Die", cbxLot.Text.Trim(),
                       string.Format("{0:yyyy/MM/dd}-{1:yyyy/MM/dd}", dtDate.Value, dateTo.Value),
                       new string[] { "Total die Inspected:", lblGoodDieTotal.Text, "Total number of defective die:", lblGoodDieDefective.Text },
                       new string[] { "Total number of good die:", lblGoodDieGood.Text, "Number of wafer in this report:", lblGoodDieRepNum.Text },
                       new string[] { "List of wafer numbers:", lblGoodDieListNum.Text },
                       grdGoodDie, sd.FileName, true);
                    break;
                //case "tabYield":
                //    string tmpfile = Path.Combine(Application.StartupPath, Guid.NewGuid().ToString() + ".xls");
                //    File.WriteAllBytes(tmpfile, global::WR.Client.UI.Properties.Resources.yieldreport);

                //    DataTable dtSrc = grdYields.DataSource as DataTable;
                //    DataTable dtSum = new DataTable();
                //    dtSum.Columns.Add("classify", typeof(string));
                //    dtSum.Columns.Add("defectnum", typeof(Int64));
                //    dtSum.Columns.Add("diecount", typeof(Int64));
                //    dtSum.Columns.Add("defecttotal", typeof(Int64));

                //    DataRow drSum = dtSrc.Rows[dtSrc.Rows.Count - 1];
                //    Int64 diecnt = Int64.Parse(drSum[dtSrc.Columns.Count - 1].ToString());
                //    Int64 defcnt = 0;
                //    for (int i = 1; i < dtSrc.Columns.Count - 1; i++)
                //    {
                //        defcnt += Int64.Parse(drSum[i].ToString());
                //    }
                //    for (int i = 1; i < dtSrc.Columns.Count; i++)
                //    {
                //        if ((Int64)drSum[i] <= 0)
                //            continue;

                //        DataRow dr = dtSum.NewRow();
                //        dr["classify"] = dtSrc.Columns[i].ColumnName;
                //        dr["defectnum"] = drSum[i];
                //        dr["diecount"] = diecnt;
                //        dr["defecttotal"] = defcnt;
                //        dtSum.Rows.Add(dr);
                //    }
                //    NpoiHelper.GridToExcelYield(tmpfile, sd.FileName, dtSrc, dtSum);
                //    break;
                case "tabYield":
                    NpoiHelper.GridToExcelByNPOI("Yield and Defect Report", cbxLot.Text.Trim(),
                       string.Format("{0:yyyy/MM/dd}-{1:yyyy/MM/dd}", dtDate.Value, dateTo.Value),
                       null, null, null,
                       grdYields, sd.FileName, false);
                    break;
                case "tabPolat":
                    string tmpfile2 = Path.Combine(Application.StartupPath, Guid.NewGuid().ToString() + ".xls");
                    File.WriteAllBytes(tmpfile2, global::WR.Client.UI.Properties.Resources.polatreport);
                    DataTable dtSrc2 = grdPolat.DataSource as DataTable;

                    DataTable dtMc = new DataTable();
                    if (dtSrc2 == null || dtSrc2.Rows.Count < 1)
                        return;

                    dtMc.Columns.Add(dtSrc2.Columns[0].ColumnName);
                    for (int i = 1; i < dtSrc2.Columns.Count; i++)
                    {
                        Int64 num = Int64.Parse(dtSrc2.Rows[dtSrc2.Rows.Count - 1][i].ToString());
                        if (num <= 0)
                            continue;
                        dtMc.Columns.Add(dtSrc2.Columns[i].ColumnName, typeof(Int64));
                    }

                    foreach (DataRow row in dtSrc2.Rows)
                    {
                        if (row[0].ToString() == "Total")
                            continue;

                        DataRow dr = dtMc.NewRow();
                        foreach (DataColumn col in dtMc.Columns)
                        {
                            dr[col] = row[col.ColumnName];
                        }
                        dtMc.Rows.Add(dr);
                    }

                    string img = Path.Combine(Application.StartupPath, Guid.NewGuid().ToString() + ".Bmp");
                    chartPolat.SaveImage(img, ChartImageFormat.Bmp);

                    NpoiHelper.GridToExcelPolat(tmpfile2, sd.FileName, dtMc, img);
                    break;
                default:
                    break;
            }

            MsgBoxEx.Info("Export has been completed.");
        }

        private void UpdateInfo(DateTimePicker dtPk)
        {
            string start = dtDate.Value.ToString("yyyyMMdd");
            string to = dateTo.Value.ToString("yyyyMMdd");
            if (int.Parse(start) > int.Parse(to))
            {
                //dtPk.Focus();
                MsgBoxEx.Info("The start date is greater than the end date.");
                return;
            }

            //double tdays = (dateTo.Value - dtDate.Value).TotalDays;
            //if (tdays > 10)
            //{
            //    //dtPk.Focus();
            //    MsgBoxEx.Info("Please select the time period within 10 days.");
            //    return;
            //}

            btnQuery.Enabled = false;

            cbxDevice.DataSource = null;
            cbxLayer.DataSource = null;
            cbxLot.DataSource = null;

            Thread thr = new Thread(new ThreadStart(LoadInfo));
            thr.IsBackground = true;
            thr.Start();
        }

        private void LoadInfo()
        {
            IwrService service = wrService.GetService();
            DateTime dtime = dateTo.Value;
            //Idens = service.GetIdentification(DataCache.UserInfo.ID, dtDate.Value.ToString("yyyyMMdd000000"), dtime.AddDays(1).ToString("yyyyMMdd000000"), "0");
            Idens = service.GetIdentification(string.Empty, dtDate.Value.ToString("yyyyMMdd000000"), dtime.AddDays(1).ToString("yyyyMMdd000000"), "0");

            Idens.ForEach((p) =>
            {
                p.DVALUE = string.Format("{0}|{1}|{2}|", p.DEVICE, p.LAYER, p.LOT);
            });

            List<WmidentificationEntity> dvs = Idens.Distinct(new DeviceComparint()).ToList();

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    cbxDevice.DisplayMember = "DEVICE";
                    cbxDevice.ValueMember = "DEVICE";

                    cbxLayer.DisplayMember = "LAYER";
                    cbxLayer.ValueMember = "DVALUE";

                    cbxLot.DisplayMember = "LOT";
                    cbxLot.ValueMember = "DVALUE";

                    btnQuery.Enabled = true;
                    btnExport.Enabled = true;
                    cbxDevice.DataSource = dvs;
                    if (dvs.Count > 0)
                    {
                        cbxDevice.Enabled = true;
                        cbxDevice.SelectedIndex = 0;
                    }
                    else
                    {
                        cbxDevice.Text = "-";
                        cbxLayer.Enabled = false;
                        cbxLot.Enabled = false;
                        btnQuery.Enabled = false;
                        btnExport.Enabled = false;
                        cbxDevice.Enabled = false;
                    }
                }));
            }
            else
            {

                cbxDevice.DisplayMember = "DEVICE";
                cbxDevice.ValueMember = "DEVICE";

                cbxLayer.DisplayMember = "LAYER";
                cbxLayer.ValueMember = "DVALUE";

                cbxLot.DisplayMember = "LOT";
                cbxLot.ValueMember = "DVALUE";

                btnQuery.Enabled = true;
                btnExport.Enabled = true;
                cbxDevice.DataSource = dvs;
                if (dvs.Count > 0)
                {
                    cbxDevice.Enabled = true;
                    cbxDevice.SelectedIndex = 0;
                }
                else
                {
                    cbxDevice.Text = "-";
                    cbxLayer.Enabled = false;
                    cbxLot.Enabled = false;
                    btnQuery.Enabled = false;
                    btnExport.Enabled = false;
                    cbxDevice.Enabled = false;
                }
            }
        }

        public class DeviceComparint : IEqualityComparer<WmidentificationEntity>
        {
            public bool Equals(WmidentificationEntity x, WmidentificationEntity y)
            {
                if (x == null && y == null)
                    return false;
                return x.DEVICE == y.DEVICE;
            }

            public int GetHashCode(WmidentificationEntity obj)
            {
                return obj.ToString().GetHashCode();
            }
        }

        public class LayerComparint : IEqualityComparer<WmidentificationEntity>
        {
            public bool Equals(WmidentificationEntity x, WmidentificationEntity y)
            {
                if (x == null && y == null)
                    return false;
                return (x.DEVICE == y.DEVICE && x.LAYER == y.LAYER);
            }

            public int GetHashCode(WmidentificationEntity obj)
            {
                return obj.ToString().GetHashCode();
            }
        }

        private void cbxDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDevice.SelectedValue == null)
            {
                cbxLayer.Text = "-";
                cbxLayer.Enabled = false;
                return;
            }

            cbxLayer.Enabled = true;
            string dv = cbxDevice.SelectedValue.ToString();
            var dly = Idens.Where(p => p.DEVICE == dv).ToList();
            List<WmidentificationEntity> lyrsT = dly.Distinct(new LayerComparint()).ToList();
            List<WmidentificationEntity> lyrs = new List<WmidentificationEntity>();
            lyrsT.ForEach((p) =>
            {
                WmidentificationEntity ent = new WmidentificationEntity();
                ent.DEVICE = p.DEVICE;
                ent.LAYER = p.LAYER;
                ent.DVALUE = string.Format("{0}|{1}||", p.DEVICE, p.LAYER);
                lyrs.Add(ent);
            });
            lyrs.Insert(0, new WmidentificationEntity() { DEVICE = dv, LAYER = "-All-", DVALUE = dv + "|||" });
            cbxLayer.DataSource = lyrs;
            cbxLayer.SelectedIndex = 0;
        }

        private void cbxLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLayer.SelectedValue == null || !cbxtrg)
            {
                if (cbxLayer.SelectedValue == null)
                {
                    cbxLot.Enabled = false;
                    btnQuery.Enabled = false;
                    btnExport.Enabled = false;
                }
                return;
            }

            cbxLot.Enabled = true;
            btnQuery.Enabled = true;
            btnExport.Enabled = true;

            string[] dvly = cbxLayer.SelectedValue.ToString().Split(new char[] { '|' });

            if (string.IsNullOrEmpty(dvly[1]))
            {
                var dly = Idens.Where(p => p.DEVICE == dvly[0]).ToList();
                dly.Insert(0, new WmidentificationEntity() { DEVICE = dvly[0], LOT = "-All-", DVALUE = dvly[0] + "|||" });
                cbxLot.DataSource = dly;
            }
            else
            {
                var dly = Idens.Where(p => p.DEVICE == dvly[0] && p.LAYER == dvly[1]).ToList();
                dly.Insert(0, new WmidentificationEntity() { DEVICE = dvly[0], LAYER = dvly[1], LOT = "-All-", DVALUE = dvly[0] + "|" + dvly[1] + "||" });
                cbxLot.DataSource = dly;
            }
        }

        private bool cbxtrg = true;
        private void cbxLot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLayer.SelectedValue != null && cbxLot.SelectedValue != null && cbxLot.SelectedIndex != 0)
            {

                btnQuery.Enabled = true;
                btnExport.Enabled = true;
                cbxLot.Enabled = true;
                string[] dvl = cbxLot.SelectedValue.ToString().Split(new char[] { '|' });
                string val = string.Format("{0}|{1}||", dvl[0], dvl[1]);
                if (cbxLayer.SelectedValue.ToString() != val)
                {
                    cbxtrg = false;
                    cbxLayer.SelectedValue = val;
                    cbxtrg = true;
                }
            }
            else if (cbxLayer.SelectedValue == null || cbxLot.SelectedValue == null)
            {
                cbxLot.Text = "-";
                cbxLot.Enabled = false;
                btnQuery.Enabled = false;
                btnExport.Enabled = false;
            }
        }

        private void tabReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabReport.SelectedTab == tabYield)
            {
                label3.Visible = false;
                cbxLot.Visible = false;
            }
            else
            {
                cbxLot.Visible = true;
                label3.Visible = true;
            }
        }

        public class LayerSumComparint : IEqualityComparer<WmItemsSummaryEntity>
        {
            public bool Equals(WmItemsSummaryEntity x, WmItemsSummaryEntity y)
            {
                if (x == null && y == null)
                    return false;
                return (x.Device == y.Device && x.Layer == y.Layer);
            }

            public int GetHashCode(WmItemsSummaryEntity obj)
            {
                return obj.ToString().GetHashCode();
            }
        }

        public class LotSumComparint : IEqualityComparer<WmItemsSummaryEntity>
        {
            public bool Equals(WmItemsSummaryEntity x, WmItemsSummaryEntity y)
            {
                if (x == null && y == null)
                    return false;
                return (x.Device == y.Device && x.Layer == y.Layer && x.Lot == y.Lot && x.Substrate_id == y.Substrate_id);
            }

            public int GetHashCode(WmItemsSummaryEntity obj)
            {
                return obj.ToString().GetHashCode();
            }
        }

        private void ShowYieldChart2(DataTable dtSrc)
        {
            chartYield.Series.Clear();

            if (dtSrc.Rows.Count > 0)
            {
                DataTable dtSum = new DataTable();
                dtSum.Columns.Add("classify", typeof(string));
                dtSum.Columns.Add("defectnum", typeof(Int64));
                dtSum.Columns.Add("diecount", typeof(Int64));
                dtSum.Columns.Add("defecttotal", typeof(Int64));
                dtSum.Columns.Add("PPM", typeof(Int64));
                dtSum.Columns.Add("Percentage", typeof(double));
                dtSum.Columns.Add("CUM", typeof(double));

                DataRow drSum = dtSrc.Rows[dtSrc.Rows.Count - 1];

                for (int i = 1; i < dtSrc.Columns.Count; i++)
                {
                    if ((Int64)drSum[i] <= 0)
                        continue;
                    if (dtSrc.Columns[i].ColumnName == "Inspected Die_Y2")
                        continue;

                    DataRow dr = dtSum.NewRow();
                    dr["classify"] = dtSrc.Columns[i].ColumnName;
                    dr["defectnum"] = drSum[i];

                    dtSum.Rows.Add(dr);
                }

                dtSum.DefaultView.Sort = "defectnum desc";

                Series ser = chartYield.Series.Add("seriePPM");
                ser.ChartType = SeriesChartType.Column;
                ser.ChartArea = "ChartArea1";
                ser.LegendText = "Defect";
                ser.Points.DataBindXY(dtSum.DefaultView, "classify", dtSum.DefaultView, "defectnum");
            }
        }

        /// <summary>
        /// 绘制Polat图
        /// </summary>
        /// <param name="dt"></param>
        private void ShowPolatChart(DataTable dt)
        {
            chartPolat.Series.Clear();

            if (dt.Rows.Count > 0)
            {
                DataTable tmpDt = dt.Copy();
                tmpDt.Rows.RemoveAt(tmpDt.Rows.Count - 1);

                List<string> colNames = new List<string>();

                foreach (DataColumn col in dt.Columns)
                {
                    colNames.Add(col.ColumnName);
                }

                DataRow totalRow = dt.Rows[dt.Rows.Count - 1];
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == "Lot_Wafer")
                        continue;
                    if (totalRow[col.ColumnName].ToString() == "0")
                        continue;

                    Series ser = chartPolat.Series.Add("ser" + col.ColumnName);
                    if (col.ColumnName == "Inspected Die_Y2")
                    {
                        ser.YAxisType = AxisType.Secondary;
                        ser.IsValueShownAsLabel = true;
                        ser.ChartType = SeriesChartType.Line;
                    }
                    else
                        ser.ChartType = SeriesChartType.Column;

                    tmpDt.DefaultView.Sort = col.ColumnName;

                    ser.ChartArea = "ChartArea1";
                    ser.LegendText = col.ColumnName;
                    ser.Points.DataBindXY(tmpDt.DefaultView, "Lot_Wafer", tmpDt.DefaultView, col.ColumnName);
                    //ser.BackGradientStyle = GradientStyle.HorizontalCenter;
                    //ser.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
                }
            }
        }

        /// <summary>
        /// 绘制Yield图
        /// </summary>
        /// <param name="dt"></param>
        private void ShowYieldChart(DataTable dtSrc)
        {
            chartYield.Series.Clear();

            if (dtSrc.Rows.Count > 0)
            {
                DataTable dtSum = new DataTable();
                dtSum.Columns.Add("classify", typeof(string));
                dtSum.Columns.Add("defectnum", typeof(Int64));
                dtSum.Columns.Add("diecount", typeof(Int64));
                dtSum.Columns.Add("defecttotal", typeof(Int64));
                dtSum.Columns.Add("PPM", typeof(Int64));
                dtSum.Columns.Add("Percentage", typeof(double));
                dtSum.Columns.Add("CUM", typeof(double));

                DataRow drSum = dtSrc.Rows[dtSrc.Rows.Count - 1];
                Int64 diecnt = Int64.Parse(drSum[dtSrc.Columns.Count - 1].ToString());
                Int64 defcnt = 0;
                for (int i = 1; i < dtSrc.Columns.Count - 1; i++)
                {
                    defcnt += Int64.Parse(drSum[i].ToString());
                }

                double cum = 0;
                double per = 0;
                for (int i = 1; i < dtSrc.Columns.Count; i++)
                {
                    if ((Int64)drSum[i] <= 0)
                        continue;
                    if (dtSrc.Columns[i].ColumnName == "Inspected Die_Y2")
                        continue;

                    DataRow dr = dtSum.NewRow();
                    dr["classify"] = dtSrc.Columns[i].ColumnName;
                    dr["defectnum"] = drSum[i];
                    dr["diecount"] = diecnt;
                    dr["defecttotal"] = defcnt;

                    dr["PPM"] = (Int64)(((Int64)drSum[i] / (diecnt * 1.0)) * 1000000);

                    per = (Int64)drSum[i] / (defcnt * 1.0);
                    dr["Percentage"] = per;

                    cum += per;
                    dr["CUM"] = cum;

                    dtSum.Rows.Add(dr);
                }

                Series ser = chartYield.Series.Add("seriePPM");
                ser.ChartType = SeriesChartType.Column;
                ser.ChartArea = "ChartArea1";
                ser.LegendText = "ppm";
                ser.Points.DataBindXY(dtSum.DefaultView, "classify", dtSum.DefaultView, "PPM");

                Series serp = chartYield.Series.Add("seriePercentage");
                serp.ChartType = SeriesChartType.Column;
                serp.ChartArea = "ChartArea1";
                serp.LegendText = "percentage";
                serp.YAxisType = AxisType.Secondary;
                //serp.IsValueShownAsLabel = true;
                serp.LabelFormat = "{p}";
                serp.YValueType = ChartValueType.Double;
                serp.Points.DataBindXY(dtSum.DefaultView, "classify", dtSum.DefaultView, "Percentage");

                Series serc = chartYield.Series.Add("serieCum");
                serc.ChartType = SeriesChartType.Line;
                serc.ChartArea = "ChartArea1";
                serc.YAxisType = AxisType.Secondary;
                //serc.IsValueShownAsLabel = true;
                serc.LabelFormat = "{p}";
                serc.YValueType = ChartValueType.Double;
                serc.LegendText = "cum";
                serc.Points.DataBindXY(dtSum.DefaultView, "classify", dtSum.DefaultView, "CUM");
            }
        }

        private void dtDate_Leave(object sender, EventArgs e)
        {
            if (dateFlag)
                UpdateInfo(dtDate);
        }

        private void dateTo_Leave(object sender, EventArgs e)
        {
            if (dateFlag)
                UpdateInfo(dateTo);
        }
    }
}
