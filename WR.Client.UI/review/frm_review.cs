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
using System.Runtime.InteropServices;

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
        private bool isExpand = false;

        Thread threadSound;
        private bool hasPlay = false;
        [DllImport("winmm.dll", SetLastError = true)]
        static extern bool PlaySound(string pszSound, UIntPtr hmod, uint fdwSound);

        private int showMode = 2; //显示模式 0：默认 1：CompletionTime 2：ReviewTime 3：Lot

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
            //log.Error("start frm_review_Load");

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

            //判断用户是否有权限变更布局
            var isLayoutRole = DataCache.Tbmenus.Count(s => s.MENUCODE == "40003") > 0;

            if (isLayoutRole)
            {
                GridViewStyleHelper.LoadDataGridViewStyle(grdData);
            }

            grdData.AutoGenerateColumns = false;

            LoadData();

            lotYield = double.Parse(DataCache.CmnDict.FirstOrDefault(s => s.DICTID == "3010" && s.CODE == "0").VALUE);
            waferYield = double.Parse(DataCache.CmnDict.FirstOrDefault(s => s.DICTID == "3010" && s.CODE == "1").VALUE);

            ckPlay.Checked = true;
            threadSound = new Thread(new ThreadStart(PlayWarning));
            threadSound.IsBackground = true;
            threadSound.Start();

            //log.Error("end frm_review_Load");
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData(string filter = "")
        {
            try
            {
                //加载Lot菜单
                trList.BeginUpdate();
                trList.Nodes.Clear();
                List<TreeNode> lotNodes = new List<TreeNode>();

                //加载lot信息
                var lotlist = DataCache.WaferResultInfo;

                if (showMode == 0) //默认
                {
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
                }
                else
                {
                    var modeList = new List<WmwaferResultEntity>();

                    if (showMode == 1)
                    {
                        lotlist = DataCache.WaferResultInfo.OrderBy(s => s.COMPLETIONTIME).ToList();
                    }
                    else if (showMode == 2)
                    {
                        if (string.IsNullOrEmpty(filter))
                            lotlist = DataCache.WaferResultInfo.OrderBy(s => s.COMPLETIONTIME).ToList();
                        else
                            lotlist = DataCache.WaferResultInfo.Where(s => s.LOT.ToLower().IndexOf(txtId.Text.ToLower()) >= 0).OrderBy(s => s.COMPLETIONTIME).ToList();
                    }
                    else
                    {
                        lotlist = DataCache.WaferResultInfo.OrderBy(s => s.CHECKEDDATE).ToList();
                    }

                    var parentList = lotlist.OrderBy(s => s.LOT).OrderByDescending(s => s.COMPLETIONTIME).Select(s => s.LOT).Distinct().ToList();

                    foreach (var n in parentList)
                    {
                        //lot
                        TreeNode lotNode = trList.Nodes.Add(n);
                        lotNode.ImageIndex = 6;
                        lotNode.SelectedImageIndex = 7;
                        lotNode.Name = n;

                        lotNodes.Add(lotNode);

                        //int index = 1;
                        var waferList = lotlist.Where(s => s.LOT == n).OrderBy(s => int.Parse(s.SUBSTRATE_SLOT)).ToList();

                        //waferList.ForEach(s => s.SUBSTRATE_SLOT = (index++).ToString());
                        modeList.AddRange(waferList);

                        foreach (var p in waferList)
                        {
                            //菜单中添加waferid
                            TreeNode snd = lotNode.Nodes.Add(p.SUBSTRATE_ID);
                            snd.ImageIndex = 6;
                            snd.SelectedImageIndex = 7;
                            snd.Name = "sn" + p.RESULTID;
                            snd.Tag = p.RESULTID;

                            if (p.COMPLETIONTIME.HasValue && p.COMPLETIONTIME.Value > 0)
                            {
                                TreeNode dms = snd.Nodes.Add(string.Format("DMS_[{0:dd/MM/yyyy HH:mm:ss}].xml", DateTime.ParseExact(p.COMPLETIONTIME.Value.ToString(), "yyyyMMddHHmmss", null)));
                                dms.ImageIndex = 3;
                                dms.SelectedImageIndex = 3;

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
                        }
                    }

                    if (showMode == 2)
                    {
                        lotlist = modeList;
                    }

                    //foreach (var p in lotlist)
                    //{
                    //    //菜单中添加waferid
                    //    TreeNode snd = trList.Nodes.Add(p.SUBSTRATE_ID + "-" + p.LOT);
                    //    snd.ImageIndex = 6;
                    //    snd.SelectedImageIndex = 7;
                    //    snd.Name = "sn" + p.RESULTID;
                    //    snd.Tag = p.RESULTID;
                    //    if (p.COMPLETIONTIME.HasValue && p.COMPLETIONTIME.Value > 0)
                    //    {
                    //        TreeNode dms = snd.Nodes.Add(string.Format("DMS_[{0:dd/MM/yyyy HH:mm:ss}].xml", DateTime.ParseExact(p.COMPLETIONTIME.Value.ToString(), "yyyyMMddHHmmss", null)));
                    //        dms.ImageIndex = 3;
                    //        dms.SelectedImageIndex = 3;
                    //        //保存waferid
                    //        dms.Tag = p.RESULTID;
                    //        dms.Name = p.RESULTID;
                    //    }
                    //    else
                    //    {
                    //        TreeNode dms = snd.Nodes.Add("DMS_[].xml");
                    //        dms.ImageIndex = 3;
                    //        dms.SelectedImageIndex = 3;
                    //        dms.Tag = p.RESULTID;
                    //        dms.Name = p.RESULTID;
                    //    }

                    //    //if (p.ISCHECKED != "2")
                    //    //{
                    //    //    node.Parent.Parent.NodeFont = ckFont;
                    //    //    node.Parent.Parent.ForeColor = Color.SaddleBrown;
                    //    //    node.Parent.NodeFont = ckFont;
                    //    //    node.Parent.ForeColor = Color.SaddleBrown;
                    //    //    node.NodeFont = ckFont;
                    //    //    node.ForeColor = Color.SaddleBrown;
                    //    //    snd.NodeFont = ckFont;
                    //    //    snd.ForeColor = Color.SaddleBrown;
                    //    //}
                    //}
                }

                //trList.ExpandAll();
                trList.EndUpdate();
                trList.CollapseAll();

                var selectIndex = 0;

                grdData.DataSource = new BindingCollection<WmwaferResultEntity>(lotlist);

                if (!string.IsNullOrEmpty(old_selectedid))
                {
                    if (old_selectedid != "-1")
                        selectIndex = lotlist.FindIndex(s => s.RESULTID == old_selectedid);
                    else
                        selectIndex = lotlist.FindIndex(s => s.COMPLETIONTIME == lotlist.Max(m => m.COMPLETIONTIME));
                }

                if (selectIndex <= 0)
                    selectIndex = 0;
                else
                    grdData.CurrentCell = grdData[0, selectIndex];

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

                        lstData.Items[selectIndex].Selected = true;
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

            if (showMode == 0)
            {
                if (trList.SelectedNode.Level == 3 || trList.SelectedNode.Level == 4)
                {
                    var ent = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == trList.SelectedNode.Tag.ToString());
                    if (ent != null)
                        Re_review(ent);
                }
                else if (trList.SelectedNode.Level == 2)
                {
                    //批量review
                    foreach (TreeNode node in trList.SelectedNode.Nodes)
                    {
                        if (node.Level == 3 || node.Level == 4)
                        {
                            var ent = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == node.Tag.ToString());
                            if (ent != null)
                                Re_review(ent);
                        }
                    }
                }
            }
            else
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
                var rs = service.GetWaferResultById(ent.RESULTID);

                if (rs != null)
                {
                    ent.NUMDEFECT = rs.NUMDEFECT;
                    ent.SFIELD = rs.SFIELD;
                }

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
                        //是否有复判权限
                        var rs = DataCache.Tbmenus.Count(s => s.MENUCODE == "30002") > 0;
                        if (!rs)
                            return;

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

                        if (showMode == 0)
                        {
                            if (e.Node.Level == 2)
                                grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo.Where(p => p.DEVICE + p.LAYER + p.LOT == e.Node.Name).ToList());
                            else if (e.Node.Level == 1)
                                grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo.Where(p => p.DEVICE + p.LAYER == e.Node.Name).ToList());
                            else if (e.Node.Level == 0)
                                grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo.Where(p => p.DEVICE == e.Node.Text.Trim()).ToList());
                        }
                        else
                        {
                            if (e.Node.Level == 0)
                                grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo.Where(p => p.LOT == e.Node.Text.Trim()).ToList());
                        }
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

        private void grdData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            //是否有复判权限
            var rs = DataCache.Tbmenus.Count(s => s.MENUCODE == "30002") > 0;
            if (!rs)
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

                var ayield = waferYield;
                var byield = waferYield;
                var cyield = waferYield;
                var dyield = waferYield;
                var eyield = waferYield;

                if (grdData["RECIPE_ID", e.RowIndex].Value != null)
                {
                    var yieldModel = DataCache.YieldSetting.FirstOrDefault(s => s.RECIPE_ID == grdData["RECIPE_ID", e.RowIndex].Value.ToString());

                    if (yieldModel != null)
                    {
                        _lotYield = Convert.ToDouble(yieldModel.LOT_YIELD);
                        _waferYield = Convert.ToDouble(yieldModel.WAFER_YIELD);

                        if (yieldModel.WAFER_YIELD > 0)
                        {
                            ayield = _waferYield;
                            byield = _waferYield;
                            cyield = _waferYield;
                            dyield = _waferYield;
                            eyield = _waferYield;
                        }

                        if (yieldModel.MASKA_YIELD > 0)
                            ayield = Convert.ToDouble(yieldModel.MASKA_YIELD);
                        if (yieldModel.MASKB_YIELD > 0)
                            byield = Convert.ToDouble(yieldModel.MASKB_YIELD);
                        if (yieldModel.MASKC_YIELD > 0)
                            cyield = Convert.ToDouble(yieldModel.MASKC_YIELD);
                        if (yieldModel.MASKD_YIELD > 0)
                            dyield = Convert.ToDouble(yieldModel.MASKD_YIELD);
                        if (yieldModel.MASKE_YIELD > 0)
                            eyield = Convert.ToDouble(yieldModel.MASKE_YIELD);
                    }
                    else
                    {
                        yieldModel = DataCache.YieldSetting.FirstOrDefault(s => s.RECIPE_ID == grdData["Column12", e.RowIndex].Value.ToString());

                        if (yieldModel != null)
                        {
                            _lotYield = Convert.ToDouble(yieldModel.LOT_YIELD);
                            _waferYield = Convert.ToDouble(yieldModel.WAFER_YIELD);

                            if (yieldModel.WAFER_YIELD > 0)
                            {
                                ayield = _waferYield;
                                byield = _waferYield;
                                cyield = _waferYield;
                                dyield = _waferYield;
                                eyield = _waferYield;
                            }

                            if (yieldModel.MASKA_YIELD > 0)
                                ayield = Convert.ToDouble(yieldModel.MASKA_YIELD);
                            if (yieldModel.MASKB_YIELD > 0)
                                byield = Convert.ToDouble(yieldModel.MASKB_YIELD);
                            if (yieldModel.MASKC_YIELD > 0)
                                cyield = Convert.ToDouble(yieldModel.MASKC_YIELD);
                            if (yieldModel.MASKD_YIELD > 0)
                                dyield = Convert.ToDouble(yieldModel.MASKD_YIELD);
                            if (yieldModel.MASKE_YIELD > 0)
                                eyield = Convert.ToDouble(yieldModel.MASKE_YIELD);
                        }
                        else
                        {
                            yieldModel = DataCache.YieldSetting.FirstOrDefault(s => s.RECIPE_ID == grdData["Column2", e.RowIndex].Value.ToString());

                            if (yieldModel != null)
                            {
                                _lotYield = Convert.ToDouble(yieldModel.LOT_YIELD);
                                _waferYield = Convert.ToDouble(yieldModel.WAFER_YIELD);

                                if (yieldModel.WAFER_YIELD > 0)
                                {
                                    ayield = _waferYield;
                                    byield = _waferYield;
                                    cyield = _waferYield;
                                    dyield = _waferYield;
                                    eyield = _waferYield;
                                }

                                if (yieldModel.MASKA_YIELD > 0)
                                    ayield = Convert.ToDouble(yieldModel.MASKA_YIELD);
                                if (yieldModel.MASKB_YIELD > 0)
                                    byield = Convert.ToDouble(yieldModel.MASKB_YIELD);
                                if (yieldModel.MASKC_YIELD > 0)
                                    cyield = Convert.ToDouble(yieldModel.MASKC_YIELD);
                                if (yieldModel.MASKD_YIELD > 0)
                                    dyield = Convert.ToDouble(yieldModel.MASKD_YIELD);
                                if (yieldModel.MASKE_YIELD > 0)
                                    eyield = Convert.ToDouble(yieldModel.MASKE_YIELD);
                            }
                        }
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
                            {
                                e.CellStyle.ForeColor = Color.Red;
                                e.CellStyle.SelectionForeColor = Color.Red;
                                //e.CellStyle.BackColor = Color.Red;
                                //e.CellStyle.SelectionBackColor = Color.Red;
                                hasPlay = true;
                            }
                        }
                        break;
                    case "Column19":
                        if (e.Value != null)
                        {
                            if (double.Parse(e.Value.ToString()) < _lotYield)
                            {
                                e.CellStyle.ForeColor = Color.Red;
                                e.CellStyle.SelectionForeColor = Color.Red;
                                //e.CellStyle.BackColor = Color.Red;
                                //e.CellStyle.SelectionBackColor = Color.Red;
                            }
                        }
                        break;
                    case "MaskA_Die":
                        if (e.Value != null)
                        {
                            if (double.Parse(e.Value.ToString()) < ayield)
                            {
                                e.CellStyle.ForeColor = Color.Red;
                                e.CellStyle.SelectionForeColor = Color.Red;
                                //e.CellStyle.BackColor = Color.Red;
                                //e.CellStyle.SelectionBackColor = Color.Red;
                            }
                        }
                        break;
                    case "MaskB_Die":
                        if (e.Value != null)
                        {
                            if (double.Parse(e.Value.ToString()) < byield)
                            {
                                e.CellStyle.ForeColor = Color.Red;
                                e.CellStyle.SelectionForeColor = Color.Red;
                                //e.CellStyle.BackColor = Color.Red;
                                //e.CellStyle.SelectionBackColor = Color.Red;
                            }
                        }
                        break;
                    case "MaskC_Die":
                        if (e.Value != null)
                        {
                            if (double.Parse(e.Value.ToString()) < cyield)
                            {
                                e.CellStyle.ForeColor = Color.Red;
                                e.CellStyle.SelectionForeColor = Color.Red;
                                //e.CellStyle.BackColor = Color.Red;
                                //e.CellStyle.SelectionBackColor = Color.Red;
                            }
                        }
                        break;
                    case "MaskD_Die":
                        if (e.Value != null)
                        {
                            if (double.Parse(e.Value.ToString()) < dyield)
                            {
                                e.CellStyle.ForeColor = Color.Red;
                                e.CellStyle.SelectionForeColor = Color.Red;
                                //e.CellStyle.BackColor = Color.Red;
                                //e.CellStyle.SelectionBackColor = Color.Red;
                            }
                        }
                        break;
                    case "MaskE_Die":
                        if (e.Value != null)
                        {
                            if (double.Parse(e.Value.ToString()) < eyield)
                            {
                                e.CellStyle.ForeColor = Color.Red;
                                e.CellStyle.SelectionForeColor = Color.Red;
                                //e.CellStyle.BackColor = Color.Red;
                                //e.CellStyle.SelectionBackColor = Color.Red;
                            }
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
                //是否有复判权限
                var rs = DataCache.Tbmenus.Count(s => s.MENUCODE == "30002") > 0;
                if (!rs)
                    return;

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
        private string old_selectedid = string.Empty;
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlRefresh_Click(object sender, EventArgs e)
        {
            lotYield = double.Parse(DataCache.CmnDict.FirstOrDefault(s => s.DICTID == "3010" && s.CODE == "0").VALUE);
            waferYield = double.Parse(DataCache.CmnDict.FirstOrDefault(s => s.DICTID == "3010" && s.CODE == "1").VALUE);

            //selectedid = string.Empty;
            //old_selectedid = selectedid;
            old_selectedid = "-1";

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

            if (trList.SelectedNode.Level == 3 || trList.SelectedNode.Level == 4 || showMode != 0)
            {
                //是否有复判权限
                var rs = DataCache.Tbmenus.Count(s => s.MENUCODE == "30002") > 0;
                if (!rs)
                    return;

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
                if (showMode == 0)
                {
                    TreeNode node = trList.GetNodeAt(e.Location);
                    if (node != null)
                    {
                        SetCntList(node.Level);
                        trList.SelectedNode = node;

                        if (node.Level == 4)
                            SelectID(node.Tag.ToString(), true);
                        trList.SelectedNode = node;

                        if ((node.Level != 2 && node.Level != 4) && lstRe_view != null)
                            lstRe_view.Enabled = false;
                        else if (node.Level == 4)
                            CheckItem(node.Tag.ToString());

                        cnsList.Show(MousePosition.X, MousePosition.Y);
                    }
                }
                else
                {
                    TreeNode node = trList.GetNodeAt(e.Location);
                    if (node != null)
                    {

                        trList.SelectedNode = node;

                        if (node.Level != 0)
                        {
                            SetCntList(4);

                            SelectID(node.Tag.ToString(), true);
                            CheckItem(node.Tag.ToString());

                            cnsList.Show(MousePosition.X, MousePosition.Y);
                        }
                        //else
                        //{
                        //    SetCntList(node.Level);
                        //    lstRe_view.Enabled = false;
                        //    tlsLLoad.Enabled = false;
                        //}

                        //cnsList.Show(MousePosition.X, MousePosition.Y);
                    }
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
            if (showMode == 0)
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
            else
            {
                if (e.Button == MouseButtons.Left && e.Node.Level < 1)
                {
                    if (e.Node.Bounds.Contains(e.Location))
                        trList_NodeMouseClick(sender, e); //单击lot 右边列表显示该lot对应的信息
                }
                else if (e.Button == MouseButtons.Left && e.Node.Level == 2)
                {
                    SelectID(e.Node.Tag.ToString(), true);
                }
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

                    //tsxml.Enabled = false;
                    //tsImage.Enabled = false;
                    tlslxml.Enabled = false;
                    tlImage.Enabled = false;
                    break;
                case 2:
                    tlsLPreview.Enabled = false;
                    //tlsLLoad.Enabled = false;
                    tlsLLoad.Enabled = true;
                    tlsLDelete.Enabled = false;
                    tlsLReport.Enabled = true;
                    ItmYield.Enabled = false;
                    ItmPolat.Enabled = true;

                    //tsxml.Enabled = false;
                    //tsImage.Enabled = false;
                    tlslxml.Enabled = false;
                    tlImage.Enabled = false;
                    break;
                case 3:
                case 4:
                    tlsLPreview.Enabled = true;
                    tlsLLoad.Enabled = true;
                    tlsLDelete.Enabled = true;
                    tlsLReport.Enabled = false;

                    tlslxml.Enabled = true;
                    tlImage.Enabled = true;
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
            if (node == null || (node.Level != 4 && node.Level != 3 && showMode == 0))
            {
                MsgBoxEx.Info("Please select a wafer record.");
                return;
            }

            string id = "";

            if (showMode == 0)
            {
                if (node.Level == 4)
                    id = node.Name;
                else
                    id = node.Nodes[0].Name;
            }
            else
            {
                if (node.Level == 1)
                    id = node.Name;
                else
                    id = node.Nodes[0].Name;
            }

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
                if (MsgBoxEx.ConfirmYesNo("Are you sure delete from the list") != DialogResult.Yes)
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

            //var ent = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == id);
            //if (ent == null)
            //    return;

            //if (ent.ISCHECKED == "2")
            //{
            //    lstRe_view.Enabled = true;
            //    grdRe_view.Enabled = true;
            //}
            var cnt = DataCache.WaferResultInfo.Count(p => (p.RESULTID == id || p.LOT == id) && p.ISCHECKED == "2");

            if (cnt > 0)
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

        private void lotYieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowReport("9");
        }

        private void itmWaferYieldList_Click(object sender, EventArgs e)
        {
            ShowReport("10");
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

        /// <summary>
        /// 下载Image
        /// </summary>
        /// <param name="srcfile"></param>
        private void DownloadImage(string srcfile, string filename)
        {
            ShowLoading(ToopEnum.downloading);

            try
            {
                IwrService service = wrService.GetService();

                Stream st = service.GetImages(srcfile);

                using (FileStream fs = File.Create(filename))
                {
                    byte[] buff = new byte[1024];

                    while (st.CanRead)
                    {
                        int rd = st.Read(buff, 0, buff.Length);
                        if (rd <= 0)
                            break;

                        fs.Write(buff, 0, rd);
                    };
                }
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
                //是否有复判权限
                var rs = DataCache.Tbmenus.Count(s => s.MENUCODE == "30002") > 0;

                if (!rs)
                    return;

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

                    if (isExpand)
                        SelectTree(ent.RESULTID);

                    isExpand = true;
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
                //var path = Path.Combine(sinfPath, string.Format("{0}-{1}", result.LOT, result.DEVICE));
                var path = Path.Combine(sinfPath, result.DEVICE, result.LAYER, string.Format("{0}-{1}", result.LOT, result.DEVICE));

                filename = Path.Combine(path, string.Format("{0}.sinf", result.SUBSTRATE_ID.Replace(".", "").Replace(" ", "")));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                IwrService service = wrService.GetService();
                var dielayout = service.GetDielayoutById(result.DIELAYOUTID);
                //var dielist = DataCache.GetAllDielayoutListById(DataCache.GetDielayoutListById(result.DIELAYOUTID));
                var dielist = DataCache.GetAllDielayoutListById(DataCache.GetDielayoutListById(result.DIELAYOUTID), false);
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
                IwrService service = wrService.GetService();

                string sinfPath = string.IsNullOrEmpty(DataCache.SinfPath) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SINF") : DataCache.SinfPath;

                Task[] tasks = new Task[resultList.Count];

                for (int i = 0; i < resultList.Count; i++)
                {
                    tasks[i] = Task.Factory.StartNew(t =>
                    {
                        var result = resultList[(int)t];
                        //var path = Path.Combine(sinfPath, string.Format("{0}-{1}", result.LOT, result.DEVICE));
                        var path = Path.Combine(sinfPath, result.DEVICE, result.LAYER, string.Format("{0}-{1}", result.LOT, result.DEVICE));
                        var filename = Path.Combine(path, string.Format("{0}.sinf", result.SUBSTRATE_ID.Replace(".", "").Replace(" ", "")));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var dielayout = service.GetDielayoutById(result.DIELAYOUTID);
                        var dielist = DataCache.GetAllDielayoutListById(DataCache.GetDielayoutListById(result.DIELAYOUTID), false);
                        var defectlit = service.GetDefectList(result.RESULTID, "");

                        CHGSinf sinf = new CHGSinf();
                        bool res = sinf.Export(filename, result.RECIPE_ID, result.LOT, result.SUBSTRATE_ID, result.SUBSTRATE_NOTCHLOCATION, dielayout, dielist, defectlit);
                    }, i);
                }

                Task.WaitAll(tasks);
                //var task = Task.Factory.StartNew(() =>
                //                       {
                //                           for (int i = 0; i < resultList.Count; i++)
                //                           {
                //                               var result = resultList[i];
                //                               var path = Path.Combine(sinfPath, string.Format("{0}-{1}", result.LOT, result.DEVICE));
                //                               //var path = Path.Combine(sinfPath, result.DEVICE, result.LOT);
                //                               var filename = Path.Combine(path, string.Format("{0}.sinf", result.SUBSTRATE_ID.Replace(".", "").Replace(" ", "")));

                //                               if (!Directory.Exists(path))
                //                               {
                //                                   Directory.CreateDirectory(path);
                //                               }

                //                               var dielayout = service.GetDielayoutById(result.DIELAYOUTID);
                //                               var dielist = DataCache.GetAllDielayoutListById(service.GetDielayoutListById(result.DIELAYOUTID));
                //                               var defectlit = service.GetDefectList(result.RESULTID, "");

                //                               CHGSinf sinf = new CHGSinf();
                //                               bool res = sinf.Export(filename, result.RECIPE_ID, result.LOT, result.SUBSTRATE_ID, result.SUBSTRATE_NOTCHLOCATION, dielayout, dielist, defectlit);
                //                           }
                //                       });

                //task.Wait();
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
                if (node == null || node.Tag == null || (node.Level != 4 && node.Level != 3 && showMode == 0))
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

                if ((node.Level != 4 && node.Level != 3 && showMode == 0))
                {
                    //batch import
                    var wfs = new List<WmwaferResultEntity>();
                    var value = node.FullPath.Split(new char[] { '\\' });

                    if (node.Level == 0)
                        wfs = DataCache.WaferResultInfo.Where(s => s.DEVICE.Equals(node.Text)).ToList();
                    else if (node.Level == 1)
                    {
                        wfs = DataCache.WaferResultInfo.Where(s => s.DEVICE.Equals(value[0]) && s.LAYER.Equals(value[1])).ToList();
                    }
                    else if (node.Level == 2)
                    {
                        wfs = DataCache.WaferResultInfo.Where(s => s.DEVICE.Equals(value[0]) && s.LAYER.Equals(value[1]) && s.LOT.Equals(value[2])).ToList();
                    }

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
                    if (node == null || node.Tag == null || (node.Level != 4 && node.Level != 3 && showMode == 0))
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
            //if (txtId.Text == "Please input layer or repice id or lot id or wafer id")
            //    grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo).ToList();
            //else
            //    grdData.DataSource = new BindingCollection<WmwaferResultEntity>(DataCache.WaferResultInfo.Where(p => p.RECIPE_ID.ToLower().IndexOf(txtId.Text.ToLower()) >= 0
            //        || p.LOT.ToLower().IndexOf(txtId.Text.ToLower()) >= 0
            //        || p.SUBSTRATE_ID.ToLower().IndexOf(txtId.Text.ToLower()) >= 0
            //        || p.LAYER.ToLower().IndexOf(txtId.Text.ToLower()) >= 0).ToList());

            if (showMode == 2)
            {
                if (txtId.Text == "Please input layer or repice id or lot id or wafer id")
                    LoadData();
                else
                    LoadData(txtId.Text);
            }
            else
            {
                var list = new List<WmwaferResultEntity>();

                if (txtId.Text == "Please input layer or repice id or lot id or wafer id")
                    list = DataCache.WaferResultInfo;
                else
                    list = DataCache.WaferResultInfo.Where(p => p.RECIPE_ID.ToLower().IndexOf(txtId.Text.ToLower()) >= 0
                        || p.LOT.ToLower().IndexOf(txtId.Text.ToLower()) >= 0
                        || p.SUBSTRATE_ID.ToLower().IndexOf(txtId.Text.ToLower()) >= 0
                        || p.LAYER.ToLower().IndexOf(txtId.Text.ToLower()) >= 0).ToList();

                //switch (cbxOrderBy.SelectedIndex)
                //{
                //    case 0:
                //        list = list.OrderBy(s => s.COMPLETIONTIME).ToList();
                //        break;
                //    case 1:
                //        list = list.OrderBy(s => s.CHECKEDDATE).ToList();
                //        break;
                //    case 2:
                //        list = list.OrderBy(s => s.LOT).ToList();
                //        break;
                //    default:
                //        break;
                //}

                grdData.DataSource = new BindingCollection<WmwaferResultEntity>(list);
            }
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

        /// <summary>
        /// 播放警报
        /// </summary>
        private void PlayWarning()
        {
            while (true)
            {
                try
                {
                    if (hasPlay && ckPlay.Checked)
                    {
                        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.wav");

                        PlaySound(filePath, UIntPtr.Zero,
                           (uint)(SoundFlags.SND_FILENAME | SoundFlags.SND_SYNC | SoundFlags.SND_NOSTOP));

                        if (this.InvokeRequired)
                            this.Invoke(new Action(() => { ckPlay.Checked = false; }));
                        else
                            ckPlay.Checked = false;
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        private void tlImage_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = trList.SelectedNode;
                if (node == null || node.Tag == null || (node.Level != 4 && node.Level != 3 && showMode == 0))
                {
                    MsgBoxEx.Info("Please select a wafer record.");
                    return;
                }

                SaveFileDialog sd = new SaveFileDialog();
                sd.FileName = node.Tag.ToString();
                sd.Filter = "压缩(zipped)文件夹 (.zip)|*.zip";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    DownloadImage(node.Tag.ToString(), sd.FileName);

                    MsgBoxEx.Info("Image file is complete.");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MsgBoxEx.Error("An error occurred while attempting to load image");
            }
        }

        private void tsImage_Click(object sender, EventArgs e)
        {
            if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
            {
                try
                {
                    var ent = grdData.SelectedRows[0].DataBoundItem as WmwaferResultEntity;

                    SaveFileDialog sd = new SaveFileDialog();
                    sd.FileName = ent.RESULTID;
                    sd.Filter = "压缩(zipped)文件夹 (.zip)|*.zip";
                    if (sd.ShowDialog() == DialogResult.OK)
                    {
                        DownloadImage(ent.RESULTID, sd.FileName);

                        MsgBoxEx.Info("Image file is complete.");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MsgBoxEx.Error("An error occurred while attempting to load image");
                }
            }
            else
                MsgBoxEx.Info("Please select a wafer record.");
        }

        private void rbnDefault_CheckedChanged(object sender, EventArgs e)
        {
            isExpand = false;
            showMode = 0;

            if (rbnDefault.Checked)
                LoadData();
        }

        private void rbnCompletionTime_CheckedChanged(object sender, EventArgs e)
        {
            isExpand = false;
            showMode = 1;

            if (rbnCompletionTime.Checked)
                LoadData();
        }

        private void rbnLot_CheckedChanged(object sender, EventArgs e)
        {
            isExpand = false;
            showMode = 2;

            if (rbnLot.Checked)
                LoadData();
        }

        private void rbnReviewTime_CheckedChanged(object sender, EventArgs e)
        {
            isExpand = false;
            showMode = 3;

            if (rbnReviewTime.Checked)
                LoadData();
        }
    }

    [Flags]
    public enum SoundFlags
    {
        /// <summary>play synchronously (default)</summary>
        SND_SYNC = 0x0000,
        /// <summary>play asynchronously</summary>
        SND_ASYNC = 0x0001,
        /// <summary>silence (!default) if sound not found</summary>
        SND_NODEFAULT = 0x0002,
        /// <summary>pszSound points to a memory file</summary>
        SND_MEMORY = 0x0004,
        /// <summary>loop the sound until next sndPlaySound</summary>
        SND_LOOP = 0x0008,
        /// <summary>don’t stop any currently playing sound</summary>
        SND_NOSTOP = 0x0010,
        /// <summary>Stop Playing Wave</summary>
        SND_PURGE = 0x40,
        /// <summary>don’t wait if the driver is busy</summary>
        SND_NOWAIT = 0x00002000,
        /// <summary>name is a registry alias</summary>
        SND_ALIAS = 0x00010000,
        /// <summary>alias is a predefined id</summary>
        SND_ALIAS_ID = 0x00110000,
        /// <summary>name is file name</summary>
        SND_FILENAME = 0x00020000,
        /// <summary>name is resource name or atom</summary>
        SND_RESOURCE = 0x00040004
    }
}
