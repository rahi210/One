using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WR.WCF.Contract;
using WR.Client.WCF;
using WR.WCF.DataContract;
using WR.Client.Utils;

namespace WR.Client.UI
{
    public partial class frm_resultlist : Form
    {
        private List<WmidentificationEntity> Idens = null;

        public List<ComboxModel> ResultList { get; set; }

        public frm_resultlist()
        {
            InitializeComponent();
        }

        private void LoadInfo()
        {
            dtDate.Value = DateTime.Now.AddYears(-3);
            dateTo.Value = DateTime.Now;

            IwrService service = wrService.GetService();
            DateTime dtime = dateTo.Value;
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
                }
                return;
            }

            cbxLot.Enabled = true;
            btnQuery.Enabled = true;

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
            }
        }

        private void frm_resultlist_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            double tdays = (dateTo.Value - dtDate.Value).TotalDays;

            if (tdays < 0)
            {
                dtDate.Focus();
                MsgBoxEx.Info("From date selection error.");
                return;
            }

            IwrService service = wrService.GetService();
            var lst = service.GetWaferResultHis(dtDate.Value.ToString("yyyyMMdd000000"), dateTo.Value.ToString("yyyyMMdd235959"), GetLot());

            if (DataCache.UserInfo.FilterData)
            {
                var newWaferList = ((from w in lst
                                     group w by new { w.DEVICE, w.LAYER, w.LOT, w.SUBSTRATE_ID } into l
                                     select new { DEVICE = l.Key.DEVICE, LAYER = l.Key.LAYER, LOT = l.Key.LOT, SUBSTRATE_ID = l.Key.SUBSTRATE_ID, CREATEDDATE = l.Max(s => s.CREATEDDATE) }))
                                      .ToList();

                lst = (from w in lst
                       join n in newWaferList
                       on new { w.DEVICE, w.LAYER, w.LOT, w.SUBSTRATE_ID, w.CREATEDDATE } equals new { n.DEVICE, n.LAYER, n.LOT, n.SUBSTRATE_ID, n.CREATEDDATE }
                       select w).ToList();
            }

            grdData.AutoGenerateColumns = false;
            grdData.DataSource = lst;
        }

        private string GetLot()
        {
            string lot = "";
            if (cbxLot.SelectedValue == null || cbxLot.SelectedValue.ToString().Length < 1 || cbxLot.SelectedIndex < 0)
                lot = "||" + cbxLot.Text.Trim();
            else
                lot = cbxLot.SelectedValue.ToString();

            return lot;
        }

        private void grdData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (grdData.Columns[e.ColumnIndex].Name == "ck")
            {
                var v = grdData.Rows[e.RowIndex].Cells["ck"].Value;

                if (v == null || v.Equals(0))
                    grdData.Rows[e.RowIndex].Cells["ck"].Value = 1;
                else
                    grdData.Rows[e.RowIndex].Cells["ck"].Value = 0;
            }


        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var list = grdData.DataSource as List<WmwaferResultEntity>;

            var slist = list.Where(s => s.Id == 1)
                .Select(s => new ComboxModel { ID = s.RESULTID, NAME = string.Format("{0}-{1}-{2}-{3}", s.DEVICE, s.LAYER, s.LOT, s.SUBSTRATE_ID) })
                .ToList();

            ResultList.AddRange(slist);

            DialogResult = DialogResult.OK;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}
