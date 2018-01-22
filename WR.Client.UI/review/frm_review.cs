using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.Utils;
using WR.WCF.Contract;
using WR.WCF.DataContract;
using System.Threading.Tasks;

namespace WR.Client.UI
{
    public partial class frm_review : FormBase
    {
        //re-view快捷菜单
        private ToolStripItem lstRe_view = null;
        private ToolStripItem grdRe_view = null;

        public string selectedResultid = "";

        private double waferYield = 0;
        private double lotYield = 0;

        public frm_review()
        {
            InitializeComponent();
        }

        private void tslList_Click(object sender, EventArgs e)
        {
            if (grdData.Visible)
                return;

            grdData.Dock = DockStyle.Fill;
            grdData.Visible = true;
            lstData.Visible = false;
            grdData.Focus();
        }

        private void tlsView_Click(object sender, EventArgs e)
        {
            if (lstData.Visible)
                return;

            string selectedwaferid = "";
            if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
            {
                var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;
                selectedwaferid = ent.RESULTID;
            }

            Random r = new Random();
            lstData.BeginUpdate();
            lstData.Clear();
            var wfs = grdData.DataSource as BindingCollection<WmwaferResultEntity>;

            foreach (var item in wfs)
            {
                ListViewItem itm = lstData.Items.Add("ld" + item.RESULTID, item.LOT + "|" + item.SUBSTRATE_ID, r.Next(9));
                if (!string.IsNullOrEmpty(selectedwaferid) && item.RESULTID == selectedwaferid)
                {
                    itm.Selected = true;
                    itm.Focused = true;
                    lstData.EnsureVisible(itm.Index);
                }
            }

            lstData.EndUpdate();

            lstData.Dock = DockStyle.Fill;
            lstData.Visible = true;
            grdData.Visible = false;
            lstData.Focus();
        }

        private void frm_review_Load(object sender, EventArgs e)
        {
            grdData.Dock = DockStyle.Fill;
            grdData.Visible = true;
            lstData.Visible = false;

            if (DataCache.UserInfo.RE_REVIEW == "1")
            {
                //添加具有Re-review权限的功能
                cnsGrd.Items.Add(new ToolStripSeparator());
                cnsList.Items.Add(new ToolStripSeparator());

                grdRe_view = cnsGrd.Items.Add("Re-review");
                grdRe_view.Click += new EventHandler(itmRe_Click);
                lstRe_view = cnsList.Items.Add("Re-review");
                lstRe_view.Click += new EventHandler(itmRe2_Click);
            }

            grdData.AutoGenerateColumns = false;

            LoadData();

            lotYield = double.Parse(DataCache.CmnDict.FirstOrDefault(s => s.DICTID == "3010" && s.CODE == "0").VALUE);
            waferYield = double.Parse(DataCache.CmnDict.FirstOrDefault(s => s.DICTID == "3010" && s.CODE == "0").VALUE);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                //加载Lot菜单
                trList.BeginUpdate();
                trList.Nodes.Clear();
                List<TreeNode> lotNodes = new List<TreeNode>();
                var list = DataCache.IdentifcationInfo;

                if (list != null && list.Count > 0)
                {
                    var devices = list.DistinctBy(p => p.DEVICE);

                    foreach (var dv in devices)
                    {
                        TreeNode node = trList.Nodes.Add(dv.DEVICE);
                        node.ImageIndex = 6;
                        node.SelectedImageIndex = 7;
                        node.Name = "dc" + dv.DEVICE;

                        //node.NodeFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                        //添加setup
                        var setups = list.Where(p => p.DEVICE == dv.DEVICE);
                        if (setups != null && setups.Count() > 0)
                        {
                            //添加Lot
                            var stps = setups.DistinctBy(p => p.LAYER);
                            foreach (var setup in stps)
                            {
                                TreeNode stNode = node.Nodes.Add(setup.LAYER);
                                stNode.ImageIndex = 6;
                                stNode.SelectedImageIndex = 7;
                                stNode.Name = dv.DEVICE + setup.LAYER;

                                var lots = list.Where(p => p.LAYER == setup.LAYER && p.DEVICE == dv.DEVICE);
                                foreach (var lot in lots)
                                {
                                    var n = stNode.Nodes.Add(lot.LOT);
                                    //n.Name = lot.IDENTIFICATIONID;
                                    n.Name = dv.DEVICE + setup.LAYER + lot.LOT;
                                    n.ImageIndex = 6;
                                    n.SelectedImageIndex = 7;

                                    lotNodes.Add(n);
                                }
                            }
                        }
                    }
                }

                Font ckFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                //加载lot信息
                var lotlist = DataCache.WaferResultInfo;
                int idx = 1;
                if (lotlist != null && lotlist.Count > 0)
                {
                    lotlist.ForEach((p) =>
                    {
                        p.Id = idx;
                        idx++;

                        //var node = lotNodes.FirstOrDefault(n => n.Name == p.IDENTIFICATIONID);
                        var node = lotNodes.FirstOrDefault(n => n.Name == p.DEVICE + p.LAYER + p.LOT);
                        if (node != null)
                        {
                            //菜单中添加waferid
                            TreeNode snd = node.Nodes.Add(p.SUBSTRATE_ID);
                            snd.ImageIndex = 6;
                            snd.SelectedImageIndex = 7;
                            snd.Name = "sn" + p.RESULTID;
                            snd.Tag = p.RESULTID;
                            if (p.COMPLETIONTIME.HasValue && p.COMPLETIONTIME.Value > 0)
                            {
                                TreeNode dms = snd.Nodes.Add(string.Format("DMS_[{0:dd/MM/yyyy HH:mm:ss}].xml", DateTime.ParseExact(p.COMPLETIONTIME.Value.ToString(), "yyyyMMddHHmmss", null)));
                                dms.ImageIndex = 3;
                                dms.SelectedImageIndex = 3;
                                //保存waferid
                                dms.Tag = p.RESULTID;
                                dms.Name = p.RESULTID;
                            }
                            else
                            {
                                TreeNode dms = snd.Nodes.Add("DMS_[].xml");
                                dms.ImageIndex = 3;
                                dms.SelectedImageIndex = 3;
                                dms.Tag = p.RESULTID;
                                dms.Name = p.RESULTID;
                            }

                            if (p.ISCHECKED != "2")
                            {
                                node.Parent.Parent.NodeFont = ckFont;
                                node.Parent.Parent.ForeColor = Color.SaddleBrown;
                                node.Parent.NodeFont = ckFont;
                                node.Parent.ForeColor = Color.SaddleBrown;
                                node.NodeFont = ckFont;
                                node.ForeColor = Color.SaddleBrown;
                                snd.NodeFont = ckFont;
                                snd.ForeColor = Color.SaddleBrown;
                            }
                        }
                    });
                }

                //trList.ExpandAll();
                trList.EndUpdate();
                trList.CollapseAll();

                grdData.DataSource = new BindingCollection<WmwaferResultEntity>(lotlist);

                //if (lstData.Visible)
                {
                    Random r = new Random();
                    lstData.BeginUpdate();
                    lstData.Clear();
                    foreach (var item in lotlist)
                    {
                        lstData.Items.Add("ld" + item.RESULTID, item.LOT + "|" + item.SUBSTRATE_ID, r.Next(9));
                    }

                    lstData.EndUpdate();

                    if (lstData.Items.Count > 0)
                    {
                        if (lstData.Visible)
                            trList.CollapseAll();

                        lstData.Items[0].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MsgBoxEx.Error("An error occurred while attempting to load data");
            }
        }

        /// <summary>
        /// Re-review
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void itmRe_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
            {
                var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;

                if (ent != null)
                    Re_review(ent);
            }
        }

        /// <summary>
        /// Re-review
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void itmRe2_Click(object sender, EventArgs e)
        {
            if (trList.SelectedNode == null)
                return;

            if (trList.SelectedNode.Level == 3 || trList.SelectedNode.Level == 4)
            {
                var ent = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == trList.SelectedNode.Tag.ToString());
                if (ent != null)
                    Re_review(ent);
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool Re_review(WmwaferResultEntity ent)
        {
            IwrService service = wrService.GetService();
            int res = service.UpdateWaferStatus(ent.RESULTID, "1", DataCache.UserInfo.ID);

            if (res > 0)
            {
                ent.ISCHECKED = "1";
                grdData.Invalidate();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 选中的lot节点
        /// </summary>
        private TreeNode lotnode = null;

        /// <summary>
        /// 点击waferid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Node.Tag != null)
                {
                    //var list = grdData.DataSource as BindingList<WmwaferResultEntity>;
                    var list = DataCache.WaferResultInfo;
                    var ent = list.FirstOrDefault(p => p.RESULTID == e.Node.Tag.ToString());
                    if (ent != null)
                    {
                        var isReview = GetWaferResultIsReview(ent.RESULTID);
                        if (isReview)
                            return;

                        frm_main frm = this.Tag as frm_main;
                        if (frm != null)
                        {
                            frm.Oparams = new string[] { ent.RESULTID, ent.LOT, ent.SUBSTRATE_ID, ent.NUMDEFECT.ToString(), ent.SFIELD.ToString() };
                            frm.mnuSelect_ItemClick(frm.mnuReview, null);
                        }
                    }
                }
                else if (e.Node.Level == 2 || e.Node.Level == 1 || e.Node.Level == 0)
                {
                    //if (lotnode == e.Node)
                    //    return;
                    if (lotnode != e.Node)
                    {
                        if (lotnode != null)
                        {
                            lotnode.ImageIndex = 6;
                            lotnode.SelectedImageIndex = 7;
                        }

                        lotnode = e.Node;
                    }

                    if (e.Node.ImageIndex == 9)
                    {
                        e.Node.ImageIndex = 6;
                        e.Node.SelectedImageIndex = 7;

                        grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo);
                    }
                    else
                    {
                        e.Node.ImageIndex = 9;
                        e.Node.SelectedImageIndex = 8;

                        if (e.Node.Level == 2)
                            grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo.Where(p => p.DEVICE + p.LAYER + p.LOT == e.Node.Name).ToList());
                        else if (e.Node.Level == 1)
                            grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo.Where(p => p.DEVICE + p.LAYER == e.Node.Name).ToList());
                        else if (e.Node.Level == 0)
                            grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo.Where(p => p.DEVICE == e.Node.Text.Trim()).ToList());
                    }

                    if (lstData.Visible)
                    {
                        string selectedwaferid = "";
                        if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
                        {
                            var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;
                            selectedwaferid = ent.RESULTID;
                        }

                        Random r = new Random();
                        lstData.BeginUpdate();
                        lstData.Clear();
                        var wfs = grdData.DataSource as BindingCollection<WmwaferResultEntity>;

                        foreach (var item in wfs)
                        {
                            ListViewItem itm = lstData.Items.Add("ld" + item.RESULTID, item.LOT + "|" + item.SUBSTRATE_ID, r.Next(9));
                            if (!string.IsNullOrEmpty(selectedwaferid) && item.RESULTID == selectedwaferid)
                            {
                                itm.Selected = true;
                                itm.Focused = true;
                                lstData.EnsureVisible(itm.Index);
                            }
                        }

                        lstData.EndUpdate();
                    }
                }
            }
        }

        private void grdData_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            frm_main frm = this.Tag as frm_main;
            if (frm != null)
            {
                var ent = grdData.Rows[e.RowIndex].DataBoundItem as WmwaferResultEntity;

                var isReview = GetWaferResultIsReview(ent.RESULTID);
                if (isReview)
                    return;

                frm.Oparams = new string[] { ent.RESULTID, ent.LOT, ent.SUBSTRATE_ID, ent.NUMDEFECT.ToString(), ent.SFIELD.ToString() };
                frm.mnuSelect_ItemClick(frm.mnuReview, null);
            }
        }

        private void grdData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (grdData["ColFileStatus", e.RowIndex].Value == null)
                    return;

                if (grdData["ColFileStatus", e.RowIndex].Value.ToString() == "2")
                {
                    e.CellStyle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    e.CellStyle.ForeColor = Color.DarkGreen;
                }

                var _lotYield = lotYield;
                var _waferYield = waferYield;

                if (grdData["RECIPE_ID", e.RowIndex].Value != null)
                {
                    var yieldModel = DataCache.YieldSetting.FirstOrDefault(s => s.RECIPE_ID == grdData["RECIPE_ID", e.RowIndex].Value.ToString());

                    if (yieldModel != null)
                    {
                        _lotYield = Convert.ToDouble(yieldModel.LOT_YIELD);
                        _waferYield = Convert.ToDouble(yieldModel.WAFER_YIELD);
                    }
                }

                string name = grdData.Columns[e.ColumnIndex].Name;
                switch (name)
                {
                    case "ColFileStatus":
                        string val = e.Value.ToString();
                        if (val == "0")
                            e.Value = "ImageTaken";
                        else if (val == "1")
                            e.Value = "Review";
                        else
                            e.Value = "Done";
                        break;

                    //格式化时间
                    case "Column18":
                    case "Column11":
                    case "Column13":
                        if (e.Value != null)
                        {
                            string ival = e.Value.ToString();
                            if (ival != "0")
                            {
                                e.Value = DateTime.ParseExact(ival, "yyyyMMddHHmmss", null).ToString("yyyy/MM/dd HH:mm:ss");
                            }
                        }
                        break;

                    case "Column10":
                        if (e.Value != null)
                        {
                            if (double.Parse(e.Value.ToString()) < _waferYield)
                                e.CellStyle.ForeColor = Color.Red;
                        }
                        break;
                    case "Column19":
                        if (e.Value != null)
                        {
                            if (double.Parse(e.Value.ToString()) < _lotYield)
                                e.CellStyle.ForeColor = Color.Red;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private void tlGrdPreview_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
            {
                frm_main frm = this.Tag as frm_main;
                if (frm != null)
                {
                    var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;

                    var isReview = GetWaferResultIsReview(ent.RESULTID);
                    if (isReview)
                        return;

                    frm.Oparams = new string[] { ent.RESULTID, ent.LOT, ent.SUBSTRATE_ID, ent.NUMDEFECT.ToString(), ent.SFIELD.ToString() };
                    frm.mnuSelect_ItemClick(frm.mnuReview, null);
                }
            }
        }

        private bool refreshing = false;
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlRefresh_Click(object sender, EventArgs e)
        {
            selectedid = string.Empty;

            if (refreshing)
                return;

            refreshing = true;

            ShowLoading();
            Thread thr = new Thread(new ThreadStart(() =>
            {
                DataCache.RefreshCache();

                if (this.InvokeRequired)
                    this.Invoke(new Action(() => { LoadData(); }));
                else
                    LoadData();

                CloseLoading();
                refreshing = false;
            }));

            thr.IsBackground = true;
            thr.Start();

            if (lstData.Visible)
                this.ActiveControl = lstData;
            else
                this.ActiveControl = grdData;
        }

        /// <summary>
        /// preview缺陷
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void preview_Click(object sender, EventArgs e)
        {
            //if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
            //{
            //    frm_main frm = this.Tag as frm_main;
            //    if (frm != null)
            //    {
            //        var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;
            //        frm.Oparams = new string[] { ent.RESULTID, ent.LOT, ent.SUBSTRATE_ID };
            //        frm.mnuSelect_ItemClick(frm.mnuReview, null);

            //    }
            //}

            if (trList.SelectedNode == null)
                return;

            if (trList.SelectedNode.Level == 3 || trList.SelectedNode.Level == 4)
            {
                frm_main frm = this.Tag as frm_main;
                if (frm != null)
                {
                    //var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;
                    var ent = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == trList.SelectedNode.Tag.ToString());

                    var isReview = GetWaferResultIsReview(ent.RESULTID);
                    if (isReview)
                        return;

                    frm.Oparams = new string[] { ent.RESULTID, ent.LOT, ent.SUBSTRATE_ID, ent.NUMDEFECT.ToString(), ent.SFIELD.ToString() };
                    frm.mnuSelect_ItemClick(frm.mnuReview, null);
                }
            }
        }

        /// <summary>
        /// 显示右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = trList.GetNodeAt(e.Location);
                if (node != null)
                {
                    SetCntList(node.Level);
                    trList.SelectedNode = node;

                    if (node.Level == 4)
                        SelectID(node.Tag.ToString(), true);
                    trList.SelectedNode = node;

                    if (node.Level != 4 && lstRe_view != null)
                        lstRe_view.Enabled = false;
                    else if (node.Level == 4)
                        CheckItem(node.Tag.ToString());

                    cnsList.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        /// <summary>
        /// 选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trList_NodeMouseClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.Level == 4)
            {
                SelectID(e.Node.Tag.ToString(), true);
            }
            else if (e.Button == MouseButtons.Left && e.Node.Level < 3)
            {
                if (e.Node.Bounds.Contains(e.Location))
                    trList_NodeMouseClick(sender, e); //单击lot 右边列表显示该lot对应的信息
            }
        }

        /// <summary>
        /// 定位选中行
        /// </summary>
        /// <param name="resultid"></param>
        private void SelectID(string resultid, bool sl)
        {
            if (grdData.Rows.Count < 1)
                return;

            //if (grdData.Visible)
            //{
            foreach (DataGridViewRow row in grdData.Rows)
            {
                if (row.Cells["ColRESULTID"].Value.ToString() == resultid)
                {
                    if (!row.Selected)
                    {
                        row.Selected = true;
                        grdData.CurrentCell = row.Cells[grdData.CurrentCell.ColumnIndex];
                        int dis = grdData.DisplayedRowCount(false) + grdData.FirstDisplayedScrollingRowIndex;
                        if (row.Index > dis || row.Index < grdData.FirstDisplayedScrollingRowIndex)
                            grdData.FirstDisplayedScrollingRowIndex = row.Index;
                    }

                    break;
                }
            }
            //}
            //else 
            if (sl)
            {
                ListViewItem[] lvs = lstData.Items.Find("ld" + resultid, false);
                if (lvs != null && lvs.Length > 0)
                {
                    if (!lvs[0].Selected)
                    {
                        lvs[0].Selected = true;
                        lvs[0].Focused = true;
                        lstData.EnsureVisible(lvs[0].Index);
                    }
                }
            }
        }

        private void SetCntList(int level)
        {
            switch (level)
            {
                case 0:
                case 1:
                    tlsLPreview.Enabled = false;
                    //tlsLLoad.Enabled = false;
                    tlsLLoad.Enabled = true;
                    tlsLDelete.Enabled = false;
                    tlsLReport.Enabled = true;
                    ItmYield.Enabled = true;
                    ItmPolat.Enabled = false;
                    break;
                case 2:
                    tlsLPreview.Enabled = false;
                    //tlsLLoad.Enabled = false;
                    tlsLLoad.Enabled = true;
                    tlsLDelete.Enabled = false;
                    tlsLReport.Enabled = true;
                    ItmYield.Enabled = false;
                    ItmPolat.Enabled = true;
                    break;
                case 3:
                case 4:
                    tlsLPreview.Enabled = true;
                    tlsLLoad.Enabled = true;
                    tlsLDelete.Enabled = true;
                    tlsLReport.Enabled = false;
                    break;
                default:
                    tlsLPreview.Enabled = false;
                    tlsLLoad.Enabled = false;
                    tlsLDelete.Enabled = false;
                    tlsLReport.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// 删除xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsLDelete_Click(object sender, EventArgs e)
        {
            TreeNode node = trList.SelectedNode;
            if (node == null || (node.Level != 4 && node.Level != 3))
            {
                MsgBoxEx.Info("Please select a wafer record.");
                return;
            }

            string id = "";
            if (node.Level == 4)
                id = node.Name;
            else
                id = node.Nodes[0].Name;

            DeleteXml(id);
        }

        /// <summary>
        /// 删除xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsGrdDelete_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
            {
                MsgBoxEx.Info("Please select a wafer record.");
                return;
            }

            var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;
            DeleteXml(ent.RESULTID);
        }

        /// <summary>
        /// 执行删除
        /// </summary>
        /// <param name="id"></param>
        private void DeleteXml(string id)
        {
            try
            {
                if (MsgBoxEx.ConfirmYesNo("are you sure delete from the list") != DialogResult.Yes)
                    return;

                IwrService service = wrService.GetService();
                int res = service.DeleteWafer(id, DataCache.UserInfo.ID);
                if (res < 1)
                {
                    MsgBoxEx.Error("Delete failed, please check the data.");
                    return;
                }
                //本地删除数据
                var ent = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == id);
                if (ent != null)
                {
                    if (DataCache.WaferResultInfo.Count(p => p.DEVICE == ent.DEVICE && p.LAYER == ent.LAYER && p.LOT == ent.LOT) <= 1)
                    {
                        var idn = DataCache.IdentifcationInfo.FirstOrDefault(p => p.DEVICE == ent.DEVICE && p.LAYER == ent.LAYER && p.LOT == ent.LOT);
                        if (idn != null)
                        {
                            DataCache.IdentifcationInfo.Remove(idn);
                            TreeNode[] tns = trList.Nodes.Find(idn.DEVICE + idn.LAYER + idn.LOT, true);
                            if (tns != null && tns.Length > 0)
                            {
                                //layer只一个节点
                                if (tns[0].Parent.Nodes.Count <= 1)
                                {
                                    //device只一个节点
                                    if (tns[0].Parent.Parent.Nodes.Count <= 1)
                                    {
                                        trList.Nodes.Remove(tns[0].Parent.Parent);
                                    }
                                    else
                                    {
                                        trList.Nodes.Remove(tns[0].Parent);
                                    }
                                }
                                else
                                    trList.Nodes.Remove(tns[0]);
                            }
                        }
                    }

                    //删除缩略图
                    if (lstData.Visible)
                    {
                        ListViewItem[] lst = lstData.Items.Find("ld" + ent.RESULTID, false);
                        if (lst != null && lst.Length > 0)
                        {
                            lstData.Items.Remove(lst[0]);
                        }
                    }

                    //删除Tree节点
                    TreeNode[] tnds = trList.Nodes.Find("sn" + ent.RESULTID, true);
                    if (tnds != null && tnds.Length > 0)
                        trList.Nodes.Remove(tnds[0]);

                    var bent = grdData.DataSource as BindingCollection<WmwaferResultEntity>;
                    //bent.Remove(ent);
                    //DataCache.WaferResultInfo.Remove(ent);

                    var tent = bent.FirstOrDefault(p => p.RESULTID == ent.RESULTID);
                    if (tent != null)
                        bent.Remove(tent);

                    var sent = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == ent.RESULTID);
                    if (sent != null)
                        DataCache.WaferResultInfo.Remove(sent);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MsgBoxEx.Error("An error occurred while deleting the record.");
            }
        }

        /// <summary>
        /// 是否可用re-view
        /// </summary>
        private void CheckItem(string id)
        {
            if (lstRe_view == null)
                return;

            lstRe_view.Enabled = false;
            grdRe_view.Enabled = false;

            var ent = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == id);
            if (ent == null)
                return;

            if (ent.ISCHECKED == "2")
            {
                lstRe_view.Enabled = true;
                grdRe_view.Enabled = true;
            }
        }

        #region 报表菜单跳转
        private void ShowReport(string type)
        {
            TreeNode node = trList.SelectedNode;
            if (node == null || node.Level > 2)
                return;

            frm_main frm = this.Tag as frm_main;
            if (frm != null)
            {
                //var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;
                //var ent = DataCache.WaferResultInfo.FirstOrDefault(p => p.DEVICE + p.LAYER + p.LOT == node.Name);
                if (node.Level == 2)
                    frm.Oparams = new string[] { node.Text, type, node.Parent.Parent.Text + "|" + node.Parent.Text + "|" + node.Text + "|", "" };
                else if (node.Level == 1)
                    frm.Oparams = new string[] { node.Text, type, node.Parent.Text + "|" + node.Text + "||", "" };
                else if (node.Level == 0)
                    frm.Oparams = new string[] { node.Text, type, node.Text + "|||", "" };

                frm.mnuSelect_ItemClick(frm.mnuSelect, null);
                frm.Oparams = null;
            }
        }

        /// <summary>
        /// 报表跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItmDensity_Click(object sender, EventArgs e)
        {
            ShowReport("1");
        }

        private void ItmCategory_Click(object sender, EventArgs e)
        {
            ShowReport("2");
        }

        private void ItmGeneral_Click(object sender, EventArgs e)
        {
            ShowReport("3");
        }

        private void ItmDieIns_Click(object sender, EventArgs e)
        {
            ShowReport("4");
        }

        private void ItmGoodDie_Click(object sender, EventArgs e)
        {
            ShowReport("5");
        }

        private void ItmDefective_Click(object sender, EventArgs e)
        {
            ShowReport("6");
        }

        private void ItmYield_Click(object sender, EventArgs e)
        {
            ShowReport("7");
        }

        private void ItmPolat_Click(object sender, EventArgs e)
        {
            ShowReport("8");
        }
        #endregion

        /// <summary>
        /// 下载xml
        /// </summary>
        /// <param name="srcfile"></param>
        /// <param name="filename"></param>
        private void DownloadXml(string srcfile, string filename)
        {
            ShowLoading(ToopEnum.downloading);

            try
            {
                IwrService service = wrService.GetService();
                Stream st = service.GetXml(srcfile);
                FileStream fs = File.Create(filename);
                byte[] buff = new byte[1024];

                while (st.CanRead)
                {
                    int rd = st.Read(buff, 0, buff.Length);
                    if (rd <= 0)
                        break;

                    fs.Write(buff, 0, rd);
                };

                fs.Flush();
                fs.Close();
            }
            finally
            {
                CloseLoading();
            }
        }

        private void lstData_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewItem node = lstData.GetItemAt(e.Location.X, e.Location.Y);
                if (node != null)
                {
                    node.Selected = true;
                    node.Focused = true;
                    CheckItem(node.Name.Substring(2));
                    cnsGrd.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void lstData_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                return;

            frm_main frm = this.Tag as frm_main;
            if (frm != null)
            {
                var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;
                frm.Oparams = new string[] { ent.RESULTID, ent.LOT, ent.SUBSTRATE_ID, ent.NUMDEFECT.ToString(), ent.SFIELD.ToString() };

                var isReview = GetWaferResultIsReview(ent.RESULTID);
                if (isReview)
                    return;

                frm.mnuSelect_ItemClick(frm.mnuReview, null);
            }
        }

        private void lstData_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lstData.Visible)
            {
                SelectID(e.Item.Name.Replace("ld", ""), false);
                SelectTree(e.Item.Name.Replace("ld", ""));
            }
        }

        private void grdData_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                grdData.Rows[e.RowIndex].Selected = true;
                grdData.CurrentCell = grdData.Rows[e.RowIndex].Cells[grdData.CurrentCell.ColumnIndex];

                var ent = grdData.Rows[e.RowIndex].DataBoundItem as WmwaferResultEntity;
                CheckItem(ent.RESULTID);
                cnsGrd.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private TreeNode selectedNode = null;
        /// <summary>
        /// 定位选择节点
        /// </summary>
        /// <param name="id"></param>
        private void SelectTree(string id)
        {
            TreeNode[] tds = trList.Nodes.Find(id, true);
            //if (tds != null && tds.Length > 0 && trList.SelectedNode != tds[0])
            //{
            //    if (trList.SelectedNode != null)
            //        trList.SelectedNode.BackColor = Color.White;

            //    trList.SelectedNode = tds[0];
            //    trList.Focus();
            //    trList.SelectedNode.BackColor = SystemColors.Highlight;

            //    if (!tds[0].IsExpanded)
            //        tds[0].ExpandAll();
            //}

            if (tds != null && tds.Length > 0)
            {
                if (selectedNode != null && selectedNode != tds[0])
                    selectedNode.BackColor = Color.White;

                tds[0].BackColor = SystemColors.Highlight;

                //trList.SelectedNode = null;
                trList.SelectedNode = tds[0];

                selectedNode = tds[0];

                if (!tds[0].IsVisible)
                {
                    tds[0].Parent.EnsureVisible();
                    tds[0].Parent.Expand();
                }
            }
        }

        private string selectedid = "";
        /// <summary>
        /// 选中wafer变动后，更新tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
            {
                WmwaferResultEntity ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;
                if (ent != null && selectedid != ent.RESULTID)
                {
                    selectedid = ent.RESULTID;
                    SelectTree(ent.RESULTID);
                }
            }
        }

        private void trList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (selectedNode != null)// && lstData.Visible)
                selectedNode.BackColor = Color.White;

            if (e.Node.Level == 4)
            {
                SelectID(e.Node.Tag.ToString(), lstData.Visible);
            }
        }

        private void grdData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void frm_review_Shown(object sender, EventArgs e)
        {
            grdData.Focus();
            //定位行
            if (!string.IsNullOrEmpty(selectedResultid))
            {
                foreach (DataGridViewRow row in grdData.Rows)
                {
                    var r = row.DataBoundItem as WmwaferResultEntity;
                    if (r.RESULTID == selectedResultid)
                    {
                        row.Selected = true;
                        grdData.CurrentCell = row.Cells[grdData.CurrentCell.ColumnIndex];
                        break;
                    }
                }
            }
        }

        private void ExportSinf(string filename, WmwaferResultEntity result)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                       {
                           ShowLoading(ToopEnum.downloading);
                       }));
            }
            else
                ShowLoading(ToopEnum.downloading);

            try
            {
                string sinfPath = string.IsNullOrEmpty(DataCache.SinfPath) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SINF") : DataCache.SinfPath;
                var path = Path.Combine(sinfPath, string.Format("{0}-{1}", result.LOT, result.DEVICE));

                filename = Path.Combine(path, string.Format("{0}.sinf", result.SUBSTRATE_ID.Replace(".", "").Replace(" ", "")));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                IwrService service = wrService.GetService();
                var dielayout = service.GetDielayoutById(result.DIELAYOUTID);
                var dielist = DataCache.GetAllDielayoutListById(service.GetDielayoutListById(result.DIELAYOUTID));
                var defectlit = service.GetDefectList(result.RESULTID, "");
                CHGSinf sinf = new CHGSinf();
                bool res = sinf.Export(filename, result.RECIPE_ID, result.LOT, result.SUBSTRATE_ID, result.SUBSTRATE_NOTCHLOCATION, dielayout, dielist, defectlit);
            }
            finally
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        CloseLoading();
                    }));
                }
                else
                    CloseLoading();
            }
        }

        private void BatchExportSinf(List<WmwaferResultEntity> resultList)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    ShowLoading(ToopEnum.downloading);
                }));
            }
            else
                ShowLoading(ToopEnum.downloading);

            try
            {

                Task[] tasks = new Task[resultList.Count];

                IwrService service = wrService.GetService();

                string sinfPath = string.IsNullOrEmpty(DataCache.SinfPath) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SINF") : DataCache.SinfPath;

                //foreach (var result in resultList)
                //{
                //    tasks[i] = Task.Factory.StartNew(() =>
                //    {
                //        var path = Path.Combine(sinfPath, string.Format("{0}_{1}", result.LOT, result.DEVICE));
                //        var filename = Path.Combine(path, string.Format("{0}.sinf", result.SUBSTRATE_ID.Replace(".", "").Replace(" ", "")));

                //        if (!Directory.Exists(path))
                //        {
                //            Directory.CreateDirectory(path);
                //        }

                //        var dielayout = service.GetDielayoutById(result.DIELAYOUTID);
                //        var dielist = service.GetDielayoutListById(result.DIELAYOUTID);
                //        var defectlit = service.GetDefectList(result.RESULTID, "");

                //        CHGSinf sinf = new CHGSinf();
                //        bool res = sinf.Export(filename, result.RECIPE_ID, result.LOT, result.SUBSTRATE_ID, result.SUBSTRATE_NOTCHLOCATION, dielayout, dielist, defectlit);
                //    });

                for (int i = 0; i < resultList.Count; i++)
                {

                    var result = resultList[i];

                    tasks[i] = Task.Factory.StartNew(() =>
                    {
                        var path = Path.Combine(sinfPath, string.Format("{0}-{1}", result.LOT, result.DEVICE));
                        var filename = Path.Combine(path, string.Format("{0}.sinf", result.SUBSTRATE_ID.Replace(".", "").Replace(" ", "")));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var dielayout = service.GetDielayoutById(result.DIELAYOUTID);
                        var dielist = DataCache.GetAllDielayoutListById(service.GetDielayoutListById(result.DIELAYOUTID));
                        var defectlit = service.GetDefectList(result.RESULTID, "");

                        CHGSinf sinf = new CHGSinf();
                        bool res = sinf.Export(filename, result.RECIPE_ID, result.LOT, result.SUBSTRATE_ID, result.SUBSTRATE_NOTCHLOCATION, dielayout, dielist, defectlit);
                    });
                }

                Task.WaitAll(tasks);
            }
            finally
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        CloseLoading();
                    }));
                }
                else
                    CloseLoading();
            }
        }

        private void tsxml_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
            {
                try
                {
                    SaveFileDialog sd = new SaveFileDialog();
                    sd.FileName = "WaferResults.xml";
                    sd.Filter = "XML文件(*.xml)|*.xml";
                    if (sd.ShowDialog() == DialogResult.OK)
                    {
                        var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;
                        //DownloadXml(ent.RESULTID + "\\WaferResults.xml", sd.FileName);
                        DownloadXml(ent.RESULTID, sd.FileName);

                        MsgBoxEx.Info("XML file is complete.");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MsgBoxEx.Error("An error occurred while attempting to load xml");
                }
            }
            else
                MsgBoxEx.Info("Please select a wafer record.");
        }

        private void tsinf_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
            {
                try
                {
                    var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;
                    //SaveFileDialog sd = new SaveFileDialog();
                    //sd.FileName = string.Format("{0}.sinf", ent.SUBSTRATE_ID.Replace(".", "").Replace(" ", ""));
                    //sd.Filter = "SINF文件(*.sinf)|*.sinf";
                    //if (sd.ShowDialog() == DialogResult.OK)
                    //{
                    //    ExportSinf(sd.FileName, ent);
                    //    MsgBoxEx.Info("SINF file is complete.");
                    //}

                    ExportSinf("", ent);

                    MsgBoxEx.Info("SINF file is complete.");
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MsgBoxEx.Error("An error occurred while attempting to export sinf");
                }
            }
            else
                MsgBoxEx.Info("Please select a wafer record.");
        }

        private void tlslxml_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = trList.SelectedNode;
                if (node == null || node.Tag == null || (node.Level != 4 && node.Level != 3))
                {
                    MsgBoxEx.Info("Please select a wafer record.");
                    return;
                }

                SaveFileDialog sd = new SaveFileDialog();
                sd.FileName = "WaferResults.xml";
                sd.Filter = "XML文件(*.xml)|*.xml";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    //DownloadXml(node.Tag.ToString() + "\\WaferResults.xml", sd.FileName);
                    DownloadXml(node.Tag.ToString(), sd.FileName);

                    MsgBoxEx.Info("XML file is complete.");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MsgBoxEx.Error("An error occurred while attempting to load xml");
            }
        }

        private void tlsinf_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = trList.SelectedNode;

                if (node.Level != 4 && node.Level != 3)
                {
                    //batch import
                    var wfs = DataCache.WaferResultInfo.Where(s => s.DEVICE.Equals(node.Text) || s.LAYER.Equals(node.Text) || s.LOT.Equals(node.Text)).ToList();

                    //Thread thr = new Thread(new ThreadStart(() =>
                    //{
                    //    BatchExportSinf(wfs);

                    //    MsgBoxEx.Info("SINF file is complete.");
                    //}));

                    //thr.IsBackground = true;
                    //thr.Start();

                    Task.Factory.StartNew(() =>
                    {
                        BatchExportSinf(wfs);

                        MsgBoxEx.Info("SINF file is complete.");
                    });
                }
                else
                {
                    if (node == null || node.Tag == null || (node.Level != 4 && node.Level != 3))
                    {
                        MsgBoxEx.Info("Please select a wafer record.");
                        return;
                    }

                    //var wfs = grdData.DataSource as BindingList<WmwaferResultEntity>;
                    var wfs = DataCache.WaferResultInfo;
                    var ent = wfs.FirstOrDefault(p => p.RESULTID == node.Tag.ToString());

                    //SaveFileDialog sd = new SaveFileDialog();
                    //sd.FileName = string.Format("{0}.sinf", ent.SUBSTRATE_ID.Replace(".", "").Replace(" ", ""));
                    //sd.Filter = "SINF文件(*.sinf)|*.sinf";
                    //if (sd.ShowDialog() == DialogResult.OK)
                    //{
                    //    ExportSinf(sd.FileName, ent);
                    //    MsgBoxEx.Info("SINF file is complete.");
                    //}

                    Thread thr = new Thread(new ThreadStart(() =>
                    {
                        ExportSinf("", ent);
                        MsgBoxEx.Info("SINF file is complete.");
                    }));

                    thr.IsBackground = true;
                    thr.Start();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MsgBoxEx.Error("An error occurred while attempting to export sinf");
            }
        }

        private void txtId_Enter(object sender, EventArgs e)
        {
            if (txtId.Text.Trim() == "Please input layer or repice id or lot id or wafer id")
            {
                txtId.Text = "";
                txtId.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtId_Leave(object sender, EventArgs e)
        {
            if (txtId.Text.Trim() == "")
            {
                txtId.Text = "Please input layer or repice id or lot id or wafer id";
                txtId.ForeColor = SystemColors.ActiveBorder;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "Please input layer or repice id or lot id or wafer id")
                grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo).ToList();
            else
                grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo.Where(p => p.RECIPE_ID.ToLower().IndexOf(txtId.Text.ToLower()) >= 0
                    || p.LOT.ToLower().IndexOf(txtId.Text.ToLower()) >= 0
                    || p.SUBSTRATE_ID.ToLower().IndexOf(txtId.Text.ToLower()) >= 0
                    || p.LAYER.ToLower().IndexOf(txtId.Text.ToLower()) >= 0).ToList());
        }

        /// <summary>
        /// 合并SINF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMergeSinf_Click(object sender, EventArgs e)
        {
            var frmMerge = new frm_merge();

            frmMerge.ShowDialog();
        }

        private bool GetWaferResultIsReview(string id)
        {
            bool isReview = false;
            IwrService service = wrService.GetService();

            var model = service.GetWaferResultById(id);

            if (model != null)
            {
                isReview = model.ISREVIEW.Equals("1") ? true : false;
            }

            //if (!isReiview)
            //{
            //    service.UpdateWaferResultToReadOnly(id, "1");
            //}

            if (isReview)
            {
                var dialog = MsgBoxEx.ConfirmYesNo("Other users are working on this file,Are you sure to continue?");

                if (dialog == System.Windows.Forms.DialogResult.Yes)
                    isReview = false;
            }

            return isReview;
        }
    }
}
