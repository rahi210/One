using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Text;

using WR.Client.Utils;
using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;
using WR.Client.Controls;

namespace WR.Client.UI
{
    public partial class frm_preview : FormBase
    {
        //private Brush _bgColor = new SolidBrush(Color.DarkBlue);
        private Brush _bgColor = new SolidBrush(SystemColors.ControlDarkDark);
        private Brush _egPen = new SolidBrush(SystemColors.Control);
        private Pen _linePen = new Pen(Color.White);

        private Brush _dPen = new SolidBrush(Color.Black);//new SolidBrush(Color.DarkGreen);
        private Brush _lPen = new SolidBrush(Color.DarkGreen);//new SolidBrush(Color.ForestGreen);
        private Brush _rPen = new SolidBrush(Color.Red);

        private string[] _oparams;
        /// <summary>
        /// 参数
        /// </summary>
        public string[] Oparams
        {
            get { return _oparams; }
            set { _oparams = value; }
        }

        private string _schemeid;
        /// <summary>
        /// 缺陷分类ID
        /// </summary>
        public string Schemeid
        {
            get { return _schemeid; }
            set { _schemeid = value; }
        }

        private string _resultid;// = "0a86e51c-3b6d-49f8-9815-df5555cb9a40";
        /// <summary>
        /// 晶片检测ID
        /// </summary>
        public string Resultid
        {
            get { return _resultid; }
            set { _resultid = value; }
        }

        /// <summary>
        /// 缺陷列表
        /// </summary>
        private List<WmdefectlistEntity> _defectlist;
        /// <summary>
        /// Layout信息
        /// </summary>
        private List<WmdielayoutlistEntitiy> _dielayoutlist;

        public bool IsLayoutRole { get; set; }

        public frm_preview()
        {
            InitializeComponent();
            grdData.AutoGenerateColumns = false;
            PicShow.WrImage = null;

            picWafer.DefectList = new List<Controls.DefectCoordinate>();

            GetLayout();
        }

        private void frm_preview_Load(object sender, EventArgs e)
        {
            grdData.Visible = true;
            lstView.Visible = false;
            grdData.Dock = DockStyle.Fill;
            lstView.Dock = DockStyle.Fill;

            if (Oparams != null && Oparams.Length > 2)
            {
                Resultid = Oparams[0];
                lblWaferID.Text = string.Format("Lot:{0}  Wafer:{1} Defect:{2} Yield:{3}", Oparams[1], Oparams[2], Oparams[3], Oparams[4]);
            }

            //InitData();
            tlsStatus.SelectedIndex = 0;

            timer1.Enabled = true;

            tlsClass.Visible = false;

            //判断用户是否有权限变更布局
            IsLayoutRole = DataCache.Tbmenus.Count(s => s.MENUCODE == "40003") > 0;

            if (!IsLayoutRole)
            {
                splitter1.Enabled = false;
                splitter2.Enabled = false;
                splitter3.Enabled = false;
            }

            panel2.Width = Convert.ToInt32(panel4.Height * 1.25);
        }

        private void GetLayout()
        {
            var layout = WR.Utils.Config.GetAppSetting("previewLayout");

            if (!string.IsNullOrEmpty(layout))
            {
                var controlsArray = layout.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                foreach (var control in controlsArray)
                {
                    var param = control.Split(':');
                    var width = int.Parse(param[1].Split(',')[0]);
                    var height = int.Parse(param[1].Split(',')[1]);

                    switch (param[0])
                    {
                        case "pnlPic":
                            pnlPic.Width = width;
                            pnlPic.Height = height;
                            break;
                        case "panel1":
                            panel1.Width = width;
                            panel1.Height = height;
                            break;
                        case "panel2":
                            panel2.Width = width;
                            panel2.Height = height;
                            break;
                        case "panel4":
                            panel4.Width = width;
                            panel4.Height = height;
                            break;
                        case "tabControl1":
                            tabControl1.Width = width;
                            tabControl1.Height = height;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private bool freshing = false;
        private bool hasDraw = true;

        /// <summary>
        /// 初始化缺陷类型列表
        /// </summary>
        private void InitClassList()
        {
            var classList = (from c in _defectlist
                             orderby c.Cclassid
                             group c by new { c.Cclassid, c.Description } into g
                             select new ClassDropDownModel { Cclassid = g.Key.Cclassid, Description = g.Key.Description }).ToList();

            classList.Insert(0, new ClassDropDownModel { Cclassid = -1, Description = "All" });

            tlsClass.ComboBox.DisplayMember = "Description";
            tlsClass.ComboBox.ValueMember = "Cclassid";
            tlsClass.ComboBox.DataSource = classList;
        }

        /// <summary>
        /// 数据加载
        /// </summary>
        private void InitData()
        {
            if (freshing)
                return;

            freshing = true;

            ShowLoading();

            //重绘图片
            if (hasDraw)
            {
                if (picWafer.DefectList.Count > 0)
                {
                    picWafer.DefectList.Clear();
                    picWafer.scaleX = 1;
                    picWafer.scaleY = 1;
                }

                picWafer.WrImage = null;
            }

            tlsSaveResult.Enabled = false;
            tlsFinish.Enabled = false;
            tlsReclass.Enabled = false;
            tlsReclass.DropDownItems.Clear();

            Thread thr = new Thread(new ThreadStart(() =>
            {
                try
                {
                    //获取缺陷列表
                    IwrService service = wrService.GetService();

                    string sts = "";
                    if (this.InvokeRequired)
                        this.Invoke(new Action(() => { sts = GetStatus(); }));
                    else
                        sts = GetStatus();

                    var defList = service.GetDefectList(Resultid, sts);
                    _defectlist = defList;

                    if (this.InvokeRequired)
                        this.Invoke(new Action(() => { InitClassList(); }));
                    else
                        InitClassList();

                    var wf = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == Resultid);
                    if (wf == null)
                        return;

                    _dielayoutlist = service.GetDielayoutListById(wf.DIELAYOUTID);

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

                    //查询缺陷分类
                    Schemeid = wf.CLASSIFICATIONINFOID;
                    var clst = service.GetClassificationItem(Schemeid, DataCache.UserInfo.ID).OrderBy(p => p.ID).ToList();

                    //过滤没有权限的缺陷分类
                    var classificationRoleCnt = DataCache.Tbmenus.Count(s => s.MENUCODE == "40001");

                    if (classificationRoleCnt > 0)
                    {
                        var forbidClassificationItem = DataCache.CmnDict.Where(s => s.DICTID == "3000").Select(s => s.CODE).ToList();

                        clst = clst.Where(s => !forbidClassificationItem.Contains(s.ID.ToString())).ToList();
                    }

                    if (this.InvokeRequired)
                        this.Invoke(new Action(() =>
                        {
                            clst.ForEach((p) =>
                            {
                                ToolStripItem itm = tlsReclass.DropDownItems.Add(string.Format("{0} {1}", p.ID, p.NAME));
                                itm.Tag = p.ITEMID;
                                itm.Click += new EventHandler(itm_Click);
                            });

                            grdClass.DataSource = clst;
                            tabControl1_SelectedIndexChanged(null, null);
                        }));
                    else
                    {
                        clst.ForEach((p) =>
                        {
                            ToolStripItem itm = tlsReclass.DropDownItems.Add(string.Format("{0} {1}", p.ID, p.NAME));
                            itm.Tag = p.ITEMID;
                            itm.Click += new EventHandler(itm_Click);
                        });
                        grdClass.DataSource = clst;
                        tabControl1_SelectedIndexChanged(null, null);
                    }

                    if (this.InvokeRequired)
                        this.Invoke(new Action(() =>
                        {
                            //grdData.DataSource = new BindingCollection<WmdefectlistEntity>(defList);
                            grdData.DataSource = defList;
                            lstView.VirtualMode = true;
                            lstView.VirtualListSize = defList.Count;

                            //tabControl1_SelectedIndexChanged(null, null);

                            if (wf.ISCHECKED != "2")
                            {
                                tlsSaveResult.Enabled = true;
                                tlsFinish.Enabled = true;
                                tlsReclass.Enabled = true;
                            }

                            if (!grdData.Visible)
                            {
                                lstView.Focus();
                                lstView.Items[0].Selected = true;
                                lstView.Items[0].Focused = true;
                                lstView.EnsureVisible(0);
                                //DrawDefect(defList[0].DieAddress);
                            }

                            if (defList.Count > 1)
                                DrawDefect(defList[0].DieAddress);
                        }));
                    else
                    {
                        //grdData.DataSource = new BindingCollection<WmdefectlistEntity>(defList);
                        grdData.DataSource = defList;
                        lstView.VirtualMode = true;
                        lstView.VirtualListSize = defList.Count;

                        //tabControl1_SelectedIndexChanged(null, null);

                        if (wf.ISCHECKED != "2")
                        {
                            tlsSaveResult.Enabled = true;
                            tlsFinish.Enabled = true;
                            tlsReclass.Enabled = true;
                        }

                        if (!grdData.Visible && defList.Count > 1)
                        {
                            lstView.Focus();
                            lstView.Items[0].Selected = true;
                            lstView.Items[0].Focused = true;
                            lstView.EnsureVisible(0);
                            DrawDefect(defList[0].DieAddress);
                        }
                    }

                    if (defList.Count == 1)
                    {
                        if (this.InvokeRequired)
                            this.Invoke(new Action(() =>
                            {
                                DrawDefect(defList[0].DieAddress);
                            }));
                        else
                        {
                            DrawDefect(defList[0].DieAddress);
                        }
                    }
                    else if (defList.Count < 1)
                    {
                        if (this.InvokeRequired)
                            this.Invoke(new Action(() =>
                            {
                                DrawDefect("0,0");
                            }));
                        else
                        {
                            DrawDefect("0,0");
                        }
                    }

                    if (this.InvokeRequired)
                        this.Invoke(new Action(() =>
                        {
                            if (grdData.Visible)
                                grdData.Focus();
                            else if (lstView.Visible)
                                lstView.Focus();
                        }));
                    else
                    {
                        if (grdData.Visible)
                            grdData.Focus();
                        else if (lstView.Visible)
                            lstView.Focus();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MsgBoxEx.Error("An error occurred while attempting to load data");
                }
                finally
                {
                    CloseLoading();

                    freshing = false;
                }
            }));

            thr.IsBackground = true;
            thr.Start();
        }

        /// <summary>
        /// 重新定位class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itm_Click(object sender, EventArgs e)
        {
            try
            {
                var items = grdClass.DataSource as List<WMCLASSIFICATIONITEM>;
                if (items == null)
                    return;

                var cl = (ToolStripItem)sender;
                var itm = items.FirstOrDefault(p => p.ITEMID == cl.Tag.ToString());
                if (itm == null)
                    return;

                if (grdData.Visible)
                {
                    if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                        return;

                    var ent = grdData.SelectedRows[0].DataBoundItem as WmdefectlistEntity;
                    if (ent == null)
                        return;

                    ent.Cclassid = itm.ID;
                    ent.InspclassifiId = itm.ITEMID;
                    ent.ModifiedDefect = ent.INSPID;
                    ent.Description = itm.NAME;

                    UpdateDefectClassification(ent);

                    grdData.InvalidateRow(grdData.SelectedRows[0].Index);
                    //DrawDefect(ent.DieAddress);
                }
                else
                {
                    if (lstView.SelectedIndices == null || lstView.SelectedIndices.Count < 1)
                        return;

                    List<WmdefectlistEntity> list = grdData.DataSource as List<WmdefectlistEntity>;
                    var ent = list[lstView.SelectedIndices[0]];
                    ent.Cclassid = itm.ID;
                    ent.InspclassifiId = itm.ITEMID;
                    ent.ModifiedDefect = ent.INSPID;
                    ent.Description = itm.NAME;

                    UpdateDefectClassification(ent);

                    lstView.RedrawItems(lstView.SelectedIndices[0], lstView.SelectedIndices[0], false);
                    DrawDefect(ent.DieAddress);
                }

                tabControl1_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MsgBoxEx.Error("An error occurred while re-classify");
            }
        }

        /// <summary>
        /// 加载缺陷图片
        /// </summary>
        /// <param name="filename"></param>
        private void GetImage(string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                    PicShow.WrImage = null;
                else
                {
                    IwrService service = wrService.GetService();
                    Stream st = service.GetPic(Resultid + "\\" + filename);
                    Image pic = Image.FromStream(st, true);
                    PicShow.WrImage = pic;
                    PicShow.Tag = filename;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MsgBoxEx.Error("An error occurred while attempting to load image");
            }
        }

        /// <summary>
        /// 获取缺陷条目检测状态
        /// </summary>
        /// <returns></returns>
        private string GetStatus()
        {
            string res = "";
            if (tlsStatus.SelectedIndex == 1)
                res = "0";
            else if (tlsStatus.SelectedIndex == 2)
                res = "1";

            return res;
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tlsFilter.Checked)
                tlsFilter.Checked = false;

            InitData();

            //if (lstView.Visible && lstView.SelectedIndices != null && lstView.SelectedIndices.Count > 0)
            //{
            //    lstView.Items[lstView.SelectedIndices[0]].Selected = false;
            //}
        }

        /// <summary>
        /// 选中行变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0 && grdData.Visible)
            {
                ResetTck();
                GetImage(grdData.SelectedRows[0].Cells["ColImageName"].Value as string);
                //picWafer.Invalidate();

                var ent = grdData.SelectedRows[0].DataBoundItem as WmdefectlistEntity;
                DrawDefect(ent.DieAddress);
            }
        }

        /// <summary>
        /// 画图
        /// </summary>
        private void DrawDefect(string loction)
        {
            if (_dielayoutlist == null || _dielayoutlist.Count < 1)
                return;

            //if (!hasDraw)
            //    return;

            int col = _dielayoutlist[0].COLUMNS_;
            int row = _dielayoutlist[0].ROWS_;

            //die宽、高
            int ww = 5;
            int wh = 4;
            int wd = col * ww + 40;
            int hg = row * wh + 40;

            if (col == row)
            {
                ww = 5;
                wh = 5;
                hg = row * wh + 20;
            }
            else if (col < row)
            {
                ww = 4;
                wh = 5;

                wd = col * ww + 40;
                hg = row * wh + 20;
            }
            else if ((col - row) > 30)
            {
                wh = 6;
                hg = row * wh + 20;
            }
            else if ((col - row) < 10)
            {
                wd = col * ww + 60;
                hg = row * wh + 60;
            }

            //计算偏移量
            int offsetX = (wd - col * ww) / 2;
            int offsetY = (hg - row * wh) / 2;

            //背景图
            Bitmap btp = new Bitmap(wd, hg);
            Graphics gc = Graphics.FromImage(btp);
            gc.Clear(Color.White);
            gc.SmoothingMode = SmoothingMode.HighSpeed;

            GraphicsPath ep = new GraphicsPath();
            ep.AddEllipse(0, 0, btp.Width, btp.Height);
            gc.FillPath(_bgColor, ep);

            //背景颜色
            GraphicsPath bp = new GraphicsPath();
            //晶片颜色
            GraphicsPath wp = new GraphicsPath();
            //缺陷晶片颜色
            GraphicsPath rp = new GraphicsPath();

            //画出die
            foreach (WmdielayoutlistEntitiy die in _dielayoutlist)
            {
                bp.AddRectangle(new Rectangle(die.DIEADDRESSX * ww + offsetX, (row - die.DIEADDRESSY) * wh + offsetY, ww, wh));
                wp.AddRectangle(new Rectangle(die.DIEADDRESSX * ww + offsetX, (row - die.DIEADDRESSY) * wh + offsetY, ww - 1, wh - 1));
            }

            gc.FillPath(_dPen, bp);
            gc.FillPath(_lPen, wp);

            var items = grdClass.DataSource as List<WMCLASSIFICATIONITEM>;

            bool lineg = false;
            int lx = 0;
            int ly = 0;

            double scaleX = Math.Round(Convert.ToDouble(picWafer.Width) / wd, 8);
            double scaleY = Math.Round(Convert.ToDouble(picWafer.Height) / hg, 8);

            //画出defect
            foreach (WmdefectlistEntity def in _defectlist)
            {
                if (string.IsNullOrEmpty(def.DieAddress))
                    continue;

                string[] adr = def.DieAddress.Split(new char[] { ',' });
                int ax = int.Parse(adr[0]);
                int ay = int.Parse(adr[1]);

                if (items != null)
                {
                    //显示定义的颜色
                    var clr = items.FirstOrDefault(p => p.ITEMID == def.InspclassifiId);
                    if (clr != null && string.IsNullOrEmpty(clr.USERID))
                    {
                        def.Color = clr.COLOR;
                    }
                }

                ////NotProcess属于没有被检测的die，这种die显示灰色
                //var dieNotProcessCnt = _dielayoutlist.Count(s => s.DIEADDRESSX == ax && s.DIEADDRESSY == ay && s.DISPOSITION.Trim() == "NotProcess");
                //if (dieNotProcessCnt > 0)
                //    def.Color = Color.Gray.Name;

                gc.FillRectangle(new SolidBrush(ConvterColor(def.Color)), ax * ww + offsetX, (row - ay) * wh + offsetY, ww - 1, wh - 1);

                //die坐标信息集合
                WR.Client.Controls.DefectCoordinate defectModel = new Controls.DefectCoordinate();

                defectModel.Location = def.DieAddress;
                defectModel.Points = new List<Point>() { new Point(Convert.ToInt32((ax * ww + offsetX) * scaleX),Convert.ToInt32(((row - ay) * wh + offsetY) * scaleY))
                    ,new Point(Convert.ToInt32((ax * ww + offsetX+ww-1) * scaleX),Convert.ToInt32(((row - ay) * wh + offsetY) * scaleY))
                    ,new Point(Convert.ToInt32((ax * ww + offsetX+ww-1) * scaleX),Convert.ToInt32(((row - ay) * wh + offsetY+wh-1) * scaleY))
                    ,new Point(Convert.ToInt32((ax * ww + offsetX) * scaleX),Convert.ToInt32(((row - ay) * wh + offsetY+wh-1) * scaleY))};

                picWafer.DefectList.Add(defectModel);

                //判断定位画线
                if (def.DieAddress == loction)
                {
                    lineg = true;
                    lx = ax;
                    ly = row - ay;
                    //gc.DrawLine(_linePen, 0, ay * wh + 11, btp.Width, ay * wh + 11);
                    //gc.DrawLine(_linePen, ax * ww + 21, 0, ax * ww + 21, btp.Height);
                }
            }

            //NotProcess属于没有被检测的die，这种die显示灰色
            var notProcessLayoutList = _dielayoutlist.Where(s => s.DISPOSITION.Trim() == "NotProcess").Select(s => new { s.DIEADDRESSX, s.DIEADDRESSY }).ToList();

            foreach (var n in notProcessLayoutList)
            {
                gc.FillRectangle(new SolidBrush(ConvterColor(Color.Gray.Name)), n.DIEADDRESSX * ww + offsetX, (row - n.DIEADDRESSY) * wh + offsetY, ww - 1, wh - 1);
            }
            //定位画线
            if (lineg)
            {
                gc.DrawLine(_linePen, 0, ly * wh + offsetY + 1, btp.Width, ly * wh + offsetY + 1);
                gc.DrawLine(_linePen, lx * ww + offsetX + 1, 0, lx * ww + offsetX + 1, btp.Height);
            }

            //画出定位三角
            Point p1 = new Point(btp.Width / 2, btp.Height - 10);
            Point p2 = new Point(btp.Width / 2 - 6, btp.Height);
            Point p3 = new Point(btp.Width / 2 + 6, btp.Height);
            gc.FillPolygon(_egPen, new Point[] { p1, p2, p3 }, System.Drawing.Drawing2D.FillMode.Alternate);
            gc.Dispose();

            //缩略图片
            Bitmap outBmp = new Bitmap(picWafer.Width, picWafer.Height);

            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.DrawImage(btp, new Rectangle(0, 0, outBmp.Width, outBmp.Height), new Rectangle(0, 0, btp.Width, btp.Height), GraphicsUnit.Pixel);
            g.Dispose();

            //绑定图片
            picWafer.WrImage = outBmp;
            hasDraw = false;
        }

        /// <summary>
        /// 画图
        /// </summary>
        private void DrawDefect1(string loction)
        {
            if (_dielayoutlist == null || _dielayoutlist.Count < 1)
                return;

            int col = _dielayoutlist[0].COLUMNS_;
            int row = _dielayoutlist[0].ROWS_;

            //die宽、高
            int ww = 5;
            int wh = 4;
            int wd = col * ww + 40;
            int hg = row * wh + 40;

            if (col == row)
            {
                ww = 5;
                wh = 5;
                hg = row * wh + 20;
            }
            else if (col < row)
            {
                ww = 4;
                wh = 5;

                wd = col * ww + 40;
                hg = row * wh + 20;
            }
            else if ((col - row) > 30)
            {
                wh = 6;
                hg = row * wh + 20;
            }

            //背景图
            Bitmap btp = new Bitmap(wd, hg);
            Graphics gc = Graphics.FromImage(btp);
            gc.Clear(Color.White);
            gc.SmoothingMode = SmoothingMode.HighSpeed;

            GraphicsPath ep = new GraphicsPath();
            ep.AddEllipse(0, 0, btp.Width, btp.Height);
            gc.FillPath(_bgColor, ep);

            //背景颜色
            GraphicsPath bp = new GraphicsPath();
            //晶片颜色
            GraphicsPath wp = new GraphicsPath();
            //缺陷晶片颜色
            GraphicsPath rp = new GraphicsPath();

            //画出die
            foreach (WmdielayoutlistEntitiy die in _dielayoutlist)
            {
                bp.AddRectangle(new Rectangle(die.DIEADDRESSX * ww + 20, (row - die.DIEADDRESSY + 4) * wh + 10, ww, wh));
                wp.AddRectangle(new Rectangle(die.DIEADDRESSX * ww + 20, (row - die.DIEADDRESSY + 4) * wh + 10, ww - 1, wh - 1));
            }

            gc.FillPath(_dPen, bp);
            gc.FillPath(_lPen, wp);

            var items = grdClass.DataSource as List<WMCLASSIFICATIONITEM>;

            bool lineg = false;
            int lx = 0;
            int ly = 0;

            //画出defect
            foreach (WmdefectlistEntity def in _defectlist)
            {
                if (string.IsNullOrEmpty(def.DieAddress))
                    continue;

                string[] adr = def.DieAddress.Split(new char[] { ',' });
                int ax = int.Parse(adr[0]);
                int ay = int.Parse(adr[1]);

                if (items != null)
                {
                    //显示定义的颜色
                    var clr = items.FirstOrDefault(p => p.ITEMID == def.InspclassifiId);
                    if (clr != null && string.IsNullOrEmpty(clr.USERID))
                    {
                        def.Color = clr.COLOR;
                    }
                }
                gc.FillRectangle(new SolidBrush(ConvterColor(def.Color)), ax * ww + 20, (row - ay + 4) * wh + 10, ww - 1, wh - 1);

                //判断定位画线
                if (def.DieAddress == loction)
                {
                    lineg = true;
                    lx = ax;
                    ly = row - ay + 4;
                    //gc.DrawLine(_linePen, 0, ay * wh + 11, btp.Width, ay * wh + 11);
                    //gc.DrawLine(_linePen, ax * ww + 21, 0, ax * ww + 21, btp.Height);
                }
            }

            //定位画线
            if (lineg)
            {
                gc.DrawLine(_linePen, 0, ly * wh + 11, btp.Width, ly * wh + 11);
                gc.DrawLine(_linePen, lx * ww + 21, 0, lx * ww + 21, btp.Height);
            }

            //画出定位三角
            Point p1 = new Point(btp.Width / 2, btp.Height - 10);
            Point p2 = new Point(btp.Width / 2 - 6, btp.Height);
            Point p3 = new Point(btp.Width / 2 + 6, btp.Height);
            gc.FillPolygon(_egPen, new Point[] { p1, p2, p3 }, System.Drawing.Drawing2D.FillMode.Alternate);
            gc.Dispose();

            //缩略图片
            Bitmap outBmp = new Bitmap(picWafer.Width, picWafer.Width);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.DrawImage(btp, new Rectangle(0, 0, outBmp.Width, outBmp.Height), new Rectangle(0, 0, btp.Width, btp.Height), GraphicsUnit.Pixel);
            g.Dispose();

            //绑定图片
            picWafer.WrImage = outBmp;
        }

        /// <summary>
        /// 画图 old
        /// </summary>
        [Obsolete("y轴向下的画图，已废弃")]
        private void DrawDefect_old(string loction)
        {
            if (_dielayoutlist == null || _dielayoutlist.Count < 1)
                return;

            int col = _dielayoutlist[0].COLUMNS_;
            int row = _dielayoutlist[0].ROWS_;

            //die宽、高
            int ww = 5;
            int wh = 4;
            int wd = col * ww + 40;
            int hg = row * wh + 40;

            if (col == row)
            {
                ww = 5;
                wh = 5;
                hg = row * wh + 20;
            }
            else if (col < row)
            {
                ww = 4;
                wh = 5;

                wd = col * ww + 40;
                hg = row * wh + 20;
            }
            else if ((col - row) > 30)
            {
                wh = 6;
                hg = row * wh + 20;
            }


            //背景图
            Bitmap btp = new Bitmap(wd, hg);
            Graphics gc = Graphics.FromImage(btp);
            gc.Clear(Color.White);
            gc.SmoothingMode = SmoothingMode.HighSpeed;

            GraphicsPath ep = new GraphicsPath();
            ep.AddEllipse(0, 0, btp.Width, btp.Height);
            gc.FillPath(_bgColor, ep);

            //背景颜色
            GraphicsPath bp = new GraphicsPath();
            //晶片颜色
            GraphicsPath wp = new GraphicsPath();
            //缺陷晶片颜色
            GraphicsPath rp = new GraphicsPath();

            //画出die
            foreach (WmdielayoutlistEntitiy die in _dielayoutlist)
            {
                bp.AddRectangle(new Rectangle(die.DIEADDRESSX * ww + 20, die.DIEADDRESSY * wh + 10, ww, wh));
                wp.AddRectangle(new Rectangle(die.DIEADDRESSX * ww + 20, die.DIEADDRESSY * wh + 10, ww - 1, wh - 1));
            }

            gc.FillPath(_dPen, bp);
            gc.FillPath(_lPen, wp);

            var items = grdClass.DataSource as List<WMCLASSIFICATIONITEM>;

            bool lineg = false;
            int lx = 0;
            int ly = 0;

            //画出defect
            foreach (WmdefectlistEntity def in _defectlist)
            {
                if (string.IsNullOrEmpty(def.DieAddress))
                    continue;

                string[] adr = def.DieAddress.Split(new char[] { ',' });
                int ax = int.Parse(adr[0]);
                int ay = int.Parse(adr[1]);

                if (items != null)
                {
                    //显示定义的颜色
                    var clr = items.FirstOrDefault(p => p.ITEMID == def.InspclassifiId);
                    if (clr != null && string.IsNullOrEmpty(clr.USERID))
                    {
                        def.Color = clr.COLOR;
                    }
                }
                gc.FillRectangle(new SolidBrush(ConvterColor(def.Color)), ax * ww + 20, ay * wh + 10, ww - 1, wh - 1);

                //判断定位画线
                if (def.DieAddress == loction)
                {
                    lineg = true;
                    lx = ax;
                    ly = ay;
                    //gc.DrawLine(_linePen, 0, ay * wh + 11, btp.Width, ay * wh + 11);
                    //gc.DrawLine(_linePen, ax * ww + 21, 0, ax * ww + 21, btp.Height);
                }
            }

            //定位画线
            if (lineg)
            {
                gc.DrawLine(_linePen, 0, ly * wh + 11, btp.Width, ly * wh + 11);
                gc.DrawLine(_linePen, lx * ww + 21, 0, lx * ww + 21, btp.Height);
            }

            //画出定位三角
            Point p1 = new Point(btp.Width / 2, btp.Height - 10);
            Point p2 = new Point(btp.Width / 2 - 6, btp.Height);
            Point p3 = new Point(btp.Width / 2 + 6, btp.Height);
            gc.FillPolygon(_egPen, new Point[] { p1, p2, p3 }, System.Drawing.Drawing2D.FillMode.Alternate);
            gc.Dispose();

            //缩略图片
            Bitmap outBmp = new Bitmap(picWafer.Width, picWafer.Width);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.DrawImage(btp, new Rectangle(0, 0, outBmp.Width, outBmp.Height), new Rectangle(0, 0, btp.Width, btp.Height), GraphicsUnit.Pixel);
            g.Dispose();

            //绑定图片
            picWafer.WrImage = outBmp;
        }

        /// <summary>
        /// 缺陷列表显示格式化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string col = grdData.Columns[e.ColumnIndex].Name;
                switch (col)
                {
                    case "ColImage":
                        if (imageList2.Images.Count < 1)
                            return;
                        string img = grdData["ColImageName", e.RowIndex].Value as string;
                        if (string.IsNullOrEmpty(img))
                            e.Value = imageList2.Images[1];
                        else
                            e.Value = imageList2.Images[0];
                        break;
                    case "ColCol":
                        string xy = grdData["ColDieAddress", e.RowIndex].Value.ToString();
                        var cr = xy.Split(new char[] { ',' });
                        e.Value = cr[0];
                        break;
                    case "ColRow":
                        string xy2 = grdData["ColDieAddress", e.RowIndex].Value.ToString();
                        var cr2 = xy2.Split(new char[] { ',' });
                        e.Value = cr2[1];
                        break;
                    //case "ColUpdated":
                    //    string ck = grdData["ColIschecked", e.RowIndex].Value.ToString();
                    //    e.Value = (ck == "1" ? "√" : "");
                    //    break;
                    //case "Colmanually":
                    //    string md = grdData["ColModifiedDefect", e.RowIndex].Value as string;
                    //    e.Value = (!string.IsNullOrEmpty(md) ? "√" : "");
                    //    break;
                    case "ColUpdated":
                        string md = grdData["ColModifiedDefect", e.RowIndex].Value as string;
                        e.Value = (!string.IsNullOrEmpty(md) ? "√" : "");
                        break;
                    default:
                        break;
                }
            }
        }

        private void grdClass_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            log.Error(e.Exception);
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
                return ColorTranslator.FromHtml(color);
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

        /// <summary>
        /// 获取汇总后的points
        /// </summary>
        /// <returns></returns>
        private List<WmClassificationItemEntity> GetItemSum()
        {
            var lst = _defectlist;//grdData.DataSource as List<WmdefectlistEntity>;
            var cl = grdClass.DataSource as List<WMCLASSIFICATIONITEM>;

            if (cl == null)
                return new List<WmClassificationItemEntity>();

            if (lst == null)
                lst = new List<WmdefectlistEntity>();

            //汇总defect数
            var k = (from s in lst
                     group s by s.InspclassifiId into g
                     select new
                     {
                         g.Key,
                         cnt = g.Count()
                     }).ToList();

            var itmLst = (from c in cl
                          join t in k
                          on c.ITEMID equals t.Key
                          select new WmClassificationItemEntity
                          {
                              DESCRIPTION = c.DESCRIPTION,
                              ID = c.ID,
                              InspectionType = "Front",
                              ITEMID = c.ITEMID,
                              Points = t.cnt,
                              SCHEMEID = c.SCHEMEID
                          }).ToList();
            //List<WmClassificationItemEntity> itmLst = new List<WmClassificationItemEntity>();

            //cl.ForEach((p) =>
            //{
            //    var itm = new WmClassificationItemEntity();
            //    itm.DESCRIPTION = p.NAME;
            //    itm.ID = p.ID;
            //    itm.InspectionType = "Front";
            //    itm.ITEMID = p.ITEMID;

            //    var ct = k.FirstOrDefault(a => a.Key == itm.ITEMID);
            //    itm.Points = ct == null ? 0 : ct.cnt;

            //    itm.SCHEMEID = p.SCHEMEID;
            //    itmLst.Add(itm);
            //});

            return itmLst;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)// && grdReport.DataSource == null)
            {
                //IwrService service = wrService.GetService();
                var list = GetItemSum();//service.GetClassSummary(Resultid);
                grdReport.DataSource = list;

                ChtShow(list);
            }
        }

        /// <summary>
        /// 显示图表
        /// </summary>
        /// <param name="list"></param>
        private void ChtShow(List<WmClassificationItemEntity> list)
        {
            var serie = chtDefect.Series[0];
            serie.Points.Clear();
            //serie.IsXValueIndexed = false;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Points < 1)
                    continue;

                DataPoint p = new DataPoint();

                //p.SetValueXY(i + 1, list[i].Points);
                p.SetValueXY(list[i].DESCRIPTION, list[i].Points);
                p.Label = list[i].Points.ToString();

                serie.Points.Add(p);
            }
        }

        private void lblLotOut_Click(object sender, EventArgs e)
        {
            picWafer.ZoomOut(10);
        }

        private void lbl_P_In_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BorderStyle = BorderStyle.FixedSingle;
        }

        private void lbl_P_In_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BorderStyle = BorderStyle.None;
            lbl.Update();
        }

        private void lbl_P_In_Click(object sender, EventArgs e)
        {
            PicShow.ZoomOut(100);
            ResetTck();
        }

        private void lbl_P_Out_Click(object sender, EventArgs e)
        {
            PicShow.ZoomIn(100);
            ResetTck();
        }

        private void ResetTck()
        {
            tckContract.Value = 0;
            tckBright.Value = 0;
            if (lbl_P_Bright.BorderStyle == BorderStyle.Fixed3D)
            {
                tckContract.Visible = false;
                lbl_P_Bright.BorderStyle = BorderStyle.None;
            }

            if (lbl_P_BrightK.BorderStyle == BorderStyle.Fixed3D)
            {
                tckBright.Visible = false;
                lbl_P_BrightK.BorderStyle = BorderStyle.None;
            }
        }

        /// <summary>
        /// 灰度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_P_Bright_Click(object sender, EventArgs e)
        {
            if (tckBright.Visible)
            {
                tckBright.Value = 0;
                tckBright.Visible = false;
                lbl_P_BrightK.BorderStyle = BorderStyle.None;
            }

            //PicShow.MakeGray();
            //PicShow.BrightnessP(false);

            if (!tckContract.Visible)
            {
                tckContract.Visible = true;
                lbl_P_Bright.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                tckContract.Visible = false;
                lbl_P_Bright.BorderStyle = BorderStyle.None;
            }
        }

        /// <summary>
        /// 鼠标效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_PB_In_MouseEnter(object sender, EventArgs e)
        {
            if (tckContract.Visible)
                return;
            Label lbl = (Label)sender;
            lbl.BorderStyle = BorderStyle.FixedSingle;
        }

        private void lbl_PB_In_MouseLeave(object sender, EventArgs e)
        {
            if (tckContract.Visible)
                return;

            Label lbl = (Label)sender;
            lbl.BorderStyle = BorderStyle.None;
            lbl.Update();
        }

        /// <summary>
        /// 亮度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_P_BrightK_Click(object sender, EventArgs e)
        {
            if (tckContract.Visible)
            {
                tckContract.Value = 0;
                tckContract.Visible = false;
                lbl_P_Bright.BorderStyle = BorderStyle.None;
            }

            //PicShow.BrightnessP(true);
            if (!tckBright.Visible)
            {
                tckBright.Visible = true;
                lbl_P_BrightK.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                tckBright.Visible = false;
                lbl_P_BrightK.BorderStyle = BorderStyle.None;
            }
        }

        /// <summary>
        /// 鼠标效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_PK_In_MouseEnter(object sender, EventArgs e)
        {
            if (tckBright.Visible)
                return;
            Label lbl = (Label)sender;
            lbl.BorderStyle = BorderStyle.FixedSingle;
        }

        private void lbl_PK_In_MouseLeave(object sender, EventArgs e)
        {
            if (tckBright.Visible)
                return;

            Label lbl = (Label)sender;
            lbl.BorderStyle = BorderStyle.None;
            lbl.Update();
        }

        /// <summary>
        /// 复原图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_P_Restore_Click(object sender, EventArgs e)
        {
            //PicShow.WrImage = PicShow.WrImage;
            if (PicShow.ZoomMultiple == 0)
                return;

            if (PicShow.ZoomMultiple > 0)
                PicShow.ZoomIn(PicShow.ZoomMultiple);
            else
                PicShow.ZoomOut(Math.Abs(PicShow.ZoomMultiple));

            PicShow.ZoomMultiple = 0;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_P_Save_Click(object sender, EventArgs e)
        {
            ResetTck();
            if (PicShow.WrImage == null)
                return;

            SaveFileDialog fd = new SaveFileDialog();
            fd.FileName = PicShow.Tag.ToString();
            if (DialogResult.OK == fd.ShowDialog())
            {
                PicShow.SaveImage(fd.FileName);
            }
        }

        private void lblLotIn_Click(object sender, EventArgs e)
        {
            picWafer.ZoomIn(10);
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

                List<WMCLASSIFICATIONITEM> items = grdClass.DataSource as List<WMCLASSIFICATIONITEM>;
                if (items == null || items.Count < 1)
                    return;

                items.ForEach((p) =>
                {
                    if (!string.IsNullOrEmpty(p.USERID))
                    {
                        string[] r = p.USERID.Split(new char[] { '|' });
                        p.HOTKEY = r[0];
                        p.COLOR = r[1];
                        p.USERID = "";
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

        private void SetClsMenu()
        {
            tlsEdit.Enabled = true;
            tlsEdit.Checked = false;

            tlsSave.Enabled = false;
            tlsClassCancel.Enabled = false;

            //colHotKey.ReadOnly = true;
            grdClass.Columns["colHotKey"].ReadOnly = true;
        }

        /// <summary>
        /// 保存自定义缺陷
        /// </summary>
        /// <returns></returns>
        private bool SaveHotKey()
        {
            grdClass.EndEdit();

            List<WMCLASSIFICATIONITEM> items = grdClass.DataSource as List<WMCLASSIFICATIONITEM>;
            if (items == null || items.Count < 1)
                return false;

            StringBuilder sbt = new StringBuilder();
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.USERID))
                {
                    if (items.Any(p => p.ITEMID != item.ITEMID && p.HOTKEY == item.HOTKEY && !string.IsNullOrEmpty(item.HOTKEY)))
                    {
                        MsgBoxEx.Info(string.Format("Acc Keys[{0}] already repeated!", DataCache.CmnDict.FirstOrDefault(p => p.DICTID == "2010" && p.CODE == item.HOTKEY).NAME));
                        return false;
                    }

                    sbt.AppendFormat(";{0}|{1}|{2}", item.ITEMID, item.HOTKEY, item.COLOR);
                }
            }

            if (sbt.Length < 1)
                return true;

            IwrService service = wrService.GetService();
            int res = service.UpdateClassificationItemUser(sbt.ToString(), DataCache.UserInfo.ID);
            if (res == 1)
            {
                items.ForEach((p) => { p.USERID = ""; });
            }

            if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
            {
                DrawDefect("0,0");
                return true;
            }

            var ent = grdData.SelectedRows[0].DataBoundItem as WmdefectlistEntity;
            DrawDefect(ent.DieAddress);

            return true;
        }

        private void grdClass_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (grdClass.Columns[e.ColumnIndex].Name == "Column4")
                {
                    if (tlsEdit.Enabled)
                        return;

                    if (clrDialog.ShowDialog() == DialogResult.OK)
                    {
                        //grdClass[e.ColumnIndex, e.RowIndex].Style.BackColor = clrDialog.Color;
                        var ent = grdClass.Rows[e.RowIndex].DataBoundItem as WMCLASSIFICATIONITEM;
                        SetItem(ent);
                        ent.COLOR = ColorTranslator.ToHtml(clrDialog.Color);
                        grdClass.Invalidate();
                        grdClass.ClearSelection();
                    }
                }
            }
        }

        private void grdData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        /// <summary>
        /// 列表显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsList_Click(object sender, EventArgs e)
        {
            if (!grdData.Visible)
            {
                var list = grdData.DataSource as List<WmdefectlistEntity>;
                grdData.Visible = true;
                //grdData.DataSource = new BindingCollection<WmdefectlistEntity>(list);
                grdData.DataSource = list;

                lstView.Visible = false;

                if (lstView.SelectedIndices != null && lstView.SelectedIndices.Count > 0)
                {
                    if (list == null)
                        return;

                    int? id = list[lstView.SelectedIndices[0]].Id;
                    foreach (DataGridViewRow row in grdData.Rows)
                    {
                        WmdefectlistEntity ent = row.DataBoundItem as WmdefectlistEntity;
                        if (ent != null && ent.Id == id)
                        {
                            row.Selected = true;
                            grdData.CurrentCell = row.Cells[grdData.CurrentCell.ColumnIndex];
                            if (!row.Displayed)
                                grdData.FirstDisplayedScrollingRowIndex = row.Index;

                            break;
                        }
                    }
                }

                grdData.Focus();
            }
        }

        /// <summary>
        /// 缩略图显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsThum_Click(object sender, EventArgs e)
        {
            if (!lstView.Visible)
            {
                grdData.Visible = false;
                lstView.Visible = true;
                lstView.Focus();

                if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
                {
                    WmdefectlistEntity selectedent = grdData.SelectedRows[0].DataBoundItem as WmdefectlistEntity;
                    List<WmdefectlistEntity> data = grdData.DataSource as List<WmdefectlistEntity>;
                    int idx = data.FindIndex(p => p.Id == selectedent.Id);
                    lstView.Items[idx].EnsureVisible();
                    lstView.Items[idx].Selected = true;
                    lstView.Items[idx].Focused = true;
                }
            }
        }

        /// <summary>
        /// 列表显示defect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            var list = grdData.DataSource as List<WmdefectlistEntity>;
            if (list == null)
                return;

            var ent = list[e.ItemIndex];
            e.Item = new ListViewItem(string.Format("ID:{0} Class:{1}", list[e.ItemIndex].Id, list[e.ItemIndex].Cclassid));
            e.Item.BackColor = SystemColors.InactiveCaption;

            if (string.IsNullOrEmpty(ent.ImageName))
                return;

            try
            {
                if (!imgsView.Images.ContainsKey(ent.ImageName))
                {
                    IwrService service = wrService.GetService();
                    Stream st = service.GetPic(Resultid + "\\" + ent.ImageName);
                    Image pic = Image.FromStream(st, true);
                    imgsView.Images.Add(ent.ImageName, pic);
                }
                e.Item.ImageIndex = imgsView.Images.IndexOfKey(ent.ImageName);
            }
            catch { }
        }

        /// <summary>
        /// 切换图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstView.SelectedIndices != null && lstView.SelectedIndices.Count > 0)
            {
                ResetTck();
                var list = grdData.DataSource as List<WmdefectlistEntity>;
                if (list == null)
                    return;
                var ent = list[lstView.SelectedIndices[0]];
                string name = ent.ImageName;
                GetImage(name);
                DrawDefect(ent.DieAddress);
            }
        }

        /// <summary>
        /// 上一条wafer记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsPsample_Click(object sender, EventArgs e)
        {
            int total = DataCache.WaferResultInfo.Count;
            if (total <= 1)
                return;

            if (tlsFilter.Checked)
                tlsFilter.Checked = false;

            int nextid = 0;
            int currid = DataCache.WaferResultInfo.FindIndex(p => p.RESULTID == Resultid);
            if (currid < 0)
                nextid = 0;
            else if (currid == 0)
                nextid = total - 1;
            else
                nextid = currid - 1;

            var ent = DataCache.WaferResultInfo[nextid];
            Resultid = ent.RESULTID;
            SaveResultid(Resultid, ent.LOT, ent.SUBSTRATE_ID, ent.NUMDEFECT, ent.SFIELD);
            lblWaferID.Text = string.Format("Lot:{0}  Wafer:{1} Defect:{2} Yield:{3}", ent.LOT, ent.SUBSTRATE_ID, ent.NUMDEFECT, ent.SFIELD);

            hasDraw = true;
            InitData();

            SetClsMenu();

            if (grdData.Visible)
                this.ActiveControl = grdData;
            else
                this.ActiveControl = lstView;
        }

        private void SaveResultid(string id, string lot, string substrate, long? defectCnt, decimal? yield)
        {
            frm_main frm = this.Tag as frm_main;
            if (frm != null)
                frm.Oparams = new string[] { id, lot, substrate, defectCnt.ToString(), yield.ToString() };
        }

        /// <summary>
        /// 下一条wafer记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsNsample_Click(object sender, EventArgs e)
        {
            int total = DataCache.WaferResultInfo.Count;
            if (total <= 1)
                return;

            if (tlsFilter.Checked)
                tlsFilter.Checked = false;

            int nextid = 0;
            int currid = DataCache.WaferResultInfo.FindIndex(p => p.RESULTID == Resultid);
            if (currid < 0)
                nextid = 0;
            else if (currid == total - 1)
                nextid = 0;
            else
                nextid = currid + 1;

            var ent = DataCache.WaferResultInfo[nextid];
            Resultid = ent.RESULTID;
            SaveResultid(Resultid, ent.LOT, ent.SUBSTRATE_ID, ent.NUMDEFECT, ent.SFIELD);
            lblWaferID.Text = string.Format("Lot:{0}  Wafer:{1} Defect:{2} Yield:{3}", ent.LOT, ent.SUBSTRATE_ID, ent.NUMDEFECT, ent.SFIELD);

            hasDraw = true;
            InitData();

            SetClsMenu();

            if (grdData.Visible)
                this.ActiveControl = grdData;
            else
                this.ActiveControl = lstView;
        }

        /// <summary>
        /// 过滤有图片的记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsFilter_Click(object sender, EventArgs e)
        {
            if (!tlsFilter.Checked)
            {
                tlsFilter.Checked = true;
                var tlst = _defectlist.Where(p => !string.IsNullOrEmpty(p.ImageName)).ToList();

                //grdData.DataSource = new BindingCollection<WmdefectlistEntity>(tlst);
                grdData.DataSource = tlst;
                lstView.VirtualListSize = tlst.Count;

                if (grdData.Visible)
                {
                    grdData.Refresh();
                    if (tlst.Count > 0)
                    {
                        grdData.Rows[0].Selected = true;
                        grdData.CurrentCell = grdData[0, 0];
                    }
                }
                else if (lstView.Visible)
                {
                    lstView.Refresh();
                    if (tlst.Count > 0)
                    {
                        lstView.Items[0].Selected = true;
                        lstView.Items[0].Focused = true;
                        lstView.EnsureVisible(0);

                        DrawDefect(tlst[0].DieAddress);
                    }
                }
            }
            else
            {
                tlsFilter.Checked = false;
                if (grdData.Visible)
                {
                    grdData.Refresh();
                    if (_defectlist.Count > 0)
                    {
                        // grdData.CurrentCell = grdData[0, 0];
                        // grdData.Rows[0].Selected = true;
                    }
                }
                else if (lstView.Visible)
                {
                    lstView.Refresh();
                    if (_defectlist.Count > 0)
                    {
                        lstView.Items[0].Selected = true;
                        lstView.Items[0].Focused = true;
                        lstView.EnsureVisible(0);

                        DrawDefect(_defectlist[0].DieAddress);
                    }
                }

                //grdData.DataSource = new BindingCollection<WmdefectlistEntity>(_defectlist);
                grdData.DataSource = _defectlist;
                lstView.VirtualListSize = _defectlist.Count;
            }
        }

        /// <summary>
        /// 上一条defect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsPpoint_Click(object sender, EventArgs e)
        {
            if (grdData.Rows.Count < 2)
                return;

            int rowidx;

            if (grdData.Visible)
            {
                if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                    rowidx = 0;
                else
                {
                    rowidx = grdData.SelectedRows[0].Index;
                    if (rowidx == 0)
                        rowidx = grdData.Rows.Count - 1;
                    else
                        rowidx = rowidx - 1;
                }

                grdData.Rows[rowidx].Selected = true;
                grdData.CurrentCell = grdData.Rows[rowidx].Cells[grdData.CurrentCell.ColumnIndex];

                if (!grdData.Rows[rowidx].Displayed)
                    grdData.FirstDisplayedScrollingRowIndex = rowidx;
            }
            else
            {
                if (!lstView.Focused)
                    lstView.Focus();

                if (lstView.SelectedIndices == null || lstView.SelectedIndices.Count < 1)
                    rowidx = 0;
                else
                {
                    rowidx = lstView.SelectedIndices[0];
                    if (rowidx == 0)
                        rowidx = grdData.Rows.Count - 1;
                    else
                        rowidx = rowidx - 1;
                }

                lstView.Items[rowidx].Selected = true;
                lstView.Items[rowidx].Focused = true;
                lstView.EnsureVisible(rowidx);
            }
        }

        /// <summary>
        /// 下一条defect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsNpoint_Click(object sender, EventArgs e)
        {
            if (grdData.Rows.Count < 2)
                return;

            int rowidx;

            if (grdData.Visible)
            {
                if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
                    rowidx = 0;
                else
                {
                    rowidx = grdData.SelectedRows[0].Index;
                    if (rowidx == grdData.Rows.Count - 1)
                        rowidx = 0;
                    else
                        rowidx = rowidx + 1;
                }

                grdData.Rows[rowidx].Selected = true;
                grdData.CurrentCell = grdData.Rows[rowidx].Cells[grdData.CurrentCell.ColumnIndex];

                if (!grdData.Rows[rowidx].Displayed)
                    grdData.FirstDisplayedScrollingRowIndex = rowidx;
            }
            else
            {
                if (!lstView.Focused)
                    lstView.Focus();

                if (lstView.SelectedIndices == null || lstView.SelectedIndices.Count < 1)
                    rowidx = 0;
                else
                {
                    rowidx = lstView.SelectedIndices[0];
                    if (rowidx == grdData.Rows.Count - 1)
                        rowidx = 0;
                    else
                        rowidx = rowidx + 1;
                }
                lstView.Items[rowidx].Selected = true;
                lstView.Items[rowidx].Focused = true;
                lstView.EnsureVisible(rowidx);
            }
        }

        /// <summary>
        /// 获取修改的defect
        /// 格式：id,passid,inspid,inspclassifiid;id,passid,inspid,inspclassifiid
        /// </summary>
        /// <returns></returns>
        private string GetModifyDefect()
        {
            var defs = grdData.DataSource as List<WmdefectlistEntity>;
            if (defs == null)
                return "";

            //修改后的defect
            var ms = defs.Where(p => !string.IsNullOrEmpty(p.ModifiedDefect));
            if (ms == null || ms.Count() < 1)
                return "";

            StringBuilder sbt = new StringBuilder();
            foreach (var item in ms)
            {
                var array = item.DieAddress.Split(',');
                sbt.AppendFormat(";{0},{1},{2},{3},{4},{5},{6}", item.Id, item.PASSID, item.INSPID, item.InspclassifiId, array[0], array[1], item.Cclassid);
            }

            sbt.Remove(0, 1);
            return sbt.ToString();
        }

        /// <summary>
        /// 保存结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsSaveResult_Click(object sender, EventArgs e)
        {
            IwrService service = wrService.GetService();
            int res = service.UpdateDefect(Resultid, DataCache.UserInfo.ID, GetModifyDefect(), "1");
            if (res >= 0)
            {
                var ent = service.GetWaferResultById(Resultid);
                var wf = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == Resultid);
                wf.ISCHECKED = ent.ISCHECKED;
                wf.CHECKEDDATE = ent.CHECKEDDATE;
                wf.NUMDEFECT = ent.NUMDEFECT;
                wf.SFIELD = ent.SFIELD;

                lblWaferID.Text = string.Format("Lot:{0}  Wafer:{1} Defect:{2} Yield:{3}", wf.LOT, wf.SUBSTRATE_ID, wf.NUMDEFECT, wf.SFIELD);
                //if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
                //{
                //    var dent = grdData.SelectedRows[0].DataBoundItem as WmdefectlistEntity;
                //    DrawDefect(dent.DieAddress);
                //}

                //更新坐标图
                if (grdData.Visible && grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
                {
                    var dent = grdData.SelectedRows[0].DataBoundItem as WmdefectlistEntity;
                    DrawDefect(dent.DieAddress);
                }
                else if (lstView.Visible && lstView.SelectedIndices != null && lstView.SelectedIndices.Count > 0)
                {
                    var list = grdData.DataSource as List<WmdefectlistEntity>;
                    DrawDefect(list[lstView.SelectedIndices[0]].DieAddress);
                }
            }
        }

        /// <summary>
        /// 完成检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsFinish_Click(object sender, EventArgs e)
        {
            if (MsgBoxEx.ConfirmYesNo(MessageConst.frm_preview_msg001) == DialogResult.No)
                return;

            IwrService service = wrService.GetService();
            int res = service.UpdateDefect(Resultid, DataCache.UserInfo.ID, GetModifyDefect(), "2");
            if (res >= 0)
            {
                var ent = service.GetWaferResultById(Resultid);
                var wf = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == Resultid);
                wf.ISCHECKED = ent.ISCHECKED;
                wf.CHECKEDDATE = ent.CHECKEDDATE;
                wf.NUMDEFECT = ent.NUMDEFECT;
                wf.SFIELD = ent.SFIELD;

                if (wf.ISCHECKED == "2")
                {
                    tlsSaveResult.Enabled = false;
                    tlsFinish.Enabled = false;
                    tlsReclass.Enabled = false;
                }
                else
                {
                    tlsSaveResult.Enabled = true;
                    tlsFinish.Enabled = true;
                    tlsReclass.Enabled = true;
                }

                if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
                {
                    var dent = grdData.SelectedRows[0].DataBoundItem as WmdefectlistEntity;
                    DrawDefect(dent.DieAddress);
                }
            }
        }

        /// <summary>
        /// 右键class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && e.Button == MouseButtons.Right)
            {
                grdData.CurrentCell = grdData[e.ColumnIndex, e.RowIndex];
                cnmReclass.Show(MousePosition.X, MousePosition.Y);
            }
        }

        /// <summary>
        /// 画定位线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picWafer_Paint(object sender, PaintEventArgs e)
        {
            //if (_dieWidth < 0 || _dieHeight < 0)
            //    return;

            //if (grdData.SelectedRows == null || grdData.SelectedRows.Count < 1)
            //    return;

            //var ent = grdData.SelectedRows[0].DataBoundItem as WmdefectlistEntity;
            //if (string.IsNullOrEmpty(ent.DieAddress))
            //    return;

            //string[] addr = ent.DieAddress.Split(new char[] { ',' });

            ////横线
            //e.Graphics.DrawLine(_linePen, 0, int.Parse(addr[1]) * _dieHeight + 1 + _offsetY, picWafer.Width, int.Parse(addr[1]) * _dieHeight + 1 + _offsetY);
            ////竖线
            //e.Graphics.DrawLine(_linePen, int.Parse(addr[0]) * _dieWidth + 1 + _offsetX, 0, int.Parse(addr[0]) * _dieWidth + 1 + _offsetX, picWafer.Height);
        }

        private void picWafer_DoubleClick(object sender, EventArgs e)
        {
            if (picWafer.WrImage == null)
                return;

            frm_zoom frm = new frm_zoom();
            frm.picBox.BackgroundImage = picWafer.WrImage;
            frm.Text = lblWaferID.Text;
            frm.ShowDialog();
        }

        /// <summary>
        /// 操作快捷键
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (tlsFinish.Enabled)
            {
                string key = keyData.ToString().Replace(" ", "").Replace("D", "").Replace("NumPad", "");
                var ky = DataCache.CmnDict.FirstOrDefault(p => p.DICTID == "2010" && key == p.VALUE.Trim());
                if (ky != null)
                {
                    List<WMCLASSIFICATIONITEM> cls = grdClass.DataSource as List<WMCLASSIFICATIONITEM>;
                    if (cls != null)
                    {
                        var clf = cls.FirstOrDefault(p => p.HOTKEY == ky.CODE && string.IsNullOrEmpty(p.USERID));
                        if (clf != null)
                        {
                            if (grdData.Visible)
                            {
                                if (grdData.SelectedRows != null && grdData.SelectedRows.Count > 0)
                                {
                                    var ent = grdData.SelectedRows[0].DataBoundItem as WmdefectlistEntity;
                                    if (ent != null)
                                    {
                                        ent.Cclassid = clf.ID;
                                        ent.InspclassifiId = clf.ITEMID;
                                        ent.ModifiedDefect = ent.INSPID;
                                        ent.Description = clf.NAME;
                                        grdData.InvalidateRow(grdData.SelectedRows[0].Index);

                                        UpdateDefectClassification(ent);

                                        //DrawDefect(ent.DieAddress);

                                        tabControl1_SelectedIndexChanged(null, null);

                                    }
                                }
                            }
                            else
                            {
                                if (lstView.SelectedIndices != null && lstView.SelectedIndices.Count > 0)
                                {
                                    List<WmdefectlistEntity> list = grdData.DataSource as List<WmdefectlistEntity>;
                                    var ent = list[lstView.SelectedIndices[0]];
                                    ent.Cclassid = clf.ID;
                                    ent.InspclassifiId = clf.ITEMID;
                                    ent.ModifiedDefect = ent.INSPID;
                                    ent.Description = clf.NAME;

                                    UpdateDefectClassification(ent);

                                    lstView.RedrawItems(lstView.SelectedIndices[0], lstView.SelectedIndices[0], false);
                                    DrawDefect(ent.DieAddress);

                                    tabControl1_SelectedIndexChanged(null, null);
                                }
                            }
                        }
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 窗体打开后定位控件，以备使用快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            grdData.Focus();
            timer1.Enabled = false;
            timer2.Enabled = true;
        }

        private void grdClass_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var item = grdClass.Rows[e.RowIndex].DataBoundItem as WMCLASSIFICATIONITEM;
            SetItem(item);
        }

        private void SetItem(WMCLASSIFICATIONITEM item)
        {
            if (item == null)
                return;

            //已经保存
            if (!string.IsNullOrEmpty(item.USERID))
                return;

            item.USERID = string.Format("{0}|{1}", item.HOTKEY, item.COLOR);
        }

        private void tckBright_Scroll(object sender, EventArgs e)
        {
            PicShow.BrightnessP(tckBright.Value);
        }

        private void tckContract_Scroll(object sender, EventArgs e)
        {
            PicShow.MkBrightness(tckContract.Value);
        }

        private void PicShow_Click(object sender, EventArgs e)
        {
            ResetTck();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            ResetTck();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateDefectClassification(WmdefectlistEntity model)
        {
            var count = 1;

            List<WmdefectlistEntity> list = grdData.DataSource as List<WmdefectlistEntity>;

            //获取同一个image下其他的缺陷
            if (!string.IsNullOrEmpty(model.ImageName))
            {
                var defectIdList = list.Where(s => s.ImageName == model.ImageName
                      && s.Id != model.Id)
                      .Select(s => s.Id).ToList();

                foreach (var id in defectIdList)
                {
                    var index = list.FindIndex(s => s.Id == id);

                    list[index].Cclassid = model.Cclassid;
                    list[index].InspclassifiId = model.InspclassifiId;
                    list[index].ModifiedDefect = model.ModifiedDefect;
                    list[index].Description = model.Description;

                    UpdateDieLayout(list[index].DieAddress, (int)model.Cclassid);
                    //if (grdData.Visible)
                    //    grdData.InvalidateRow(index);
                }
            }

            if (model.Cclassid == 1)
            {
                //获取die下其他的缺陷
                var defectIdList = list.Where(s => s.DieAddress == model.DieAddress
                    && s.PASSID == model.PASSID && s.INSPID == model.INSPID && s.Id != model.Id)
                    .Select(s => s.Id).ToList();

                count += defectIdList.Count;

                foreach (var id in defectIdList)
                {
                    var index = list.FindIndex(s => s.Id == id);

                    list[index].Cclassid = model.Cclassid;
                    list[index].InspclassifiId = model.InspclassifiId;
                    list[index].ModifiedDefect = model.ModifiedDefect;
                    list[index].Description = model.Description;

                    //if (grdData.Visible)
                    //    grdData.InvalidateRow(index);
                }
            }

            UpdateDieLayout(model.DieAddress, (int)model.Cclassid);

            if (grdData.Rows.Count > grdData.CurrentCell.RowIndex + count)
                grdData.CurrentCell = grdData[grdData.CurrentCell.ColumnIndex, grdData.CurrentCell.RowIndex + count];
            else
                grdData.CurrentCell = grdData[grdData.CurrentCell.ColumnIndex, grdData.Rows.Count - 1];

            //重新计算良率 
            decimal goodCnt = _dielayoutlist.Count(s => s.INSPCLASSIFIID == 0);
            decimal defectCnt = _dielayoutlist.Count(s => s.INSPCLASSIFIID != 0);

            Oparams[3] = defectCnt.ToString();
            Oparams[4] = (goodCnt / _dielayoutlist.Count * 100).ToString("0.00");

            lblWaferID.Text = string.Format("Lot:{0}  Wafer:{1} Defect:{2} Yield:{3}", Oparams[1], Oparams[2], Oparams[3], Oparams[4]);
        }

        /// <summary>
        /// 更新layout缺陷类型
        /// </summary>
        /// <param name="dieAddress"></param>
        /// <param name="cclassid"></param>
        private void UpdateDieLayout(string dieAddress, int cclassid)
        {
            var array = dieAddress.Split(',');

            int x = int.Parse(array[0]);
            int y = int.Parse(array[1]);
            var index = _dielayoutlist.FindIndex(s => s.DIEADDRESSX == x && s.DIEADDRESSY == y);

            if (index != -1)
                _dielayoutlist[index].INSPCLASSIFIID = cclassid;
        }

        /// <summary>
        /// 缺陷改变
        /// </summary>
        /// <param name="e"></param>
        private void picWafer_DefectChanged(Controls.EventDefectArg e)
        {
            try
            {
                var selectDefectList = _defectlist.Where(s => s.DieAddress == e.Location).ToList();

                //grdData.DataSource = new BindingCollection<WmdefectlistEntity>(selectDefectList);
                grdData.DataSource = selectDefectList;

                DrawDefect(e.Location);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblReset_Click(object sender, EventArgs e)
        {
            if (picWafer.ZoomMultiple == 0)
                return;

            if (picWafer.ZoomMultiple > 0)
                picWafer.ZoomIn(picWafer.ZoomMultiple);
            else
                picWafer.ZoomOut(Math.Abs(picWafer.ZoomMultiple));

            picWafer.ZoomMultiple = 0;
        }

        /// <summary>
        /// 定时保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            IwrService service = wrService.GetService();

            int res = service.UpdateDefect(Resultid, DataCache.UserInfo.ID, GetModifyDefect(), "1");
            if (res >= 0)
            {
                var ent = service.GetWaferResultById(Resultid);
                var wf = DataCache.WaferResultInfo.FirstOrDefault(p => p.RESULTID == Resultid);
                wf.ISCHECKED = ent.ISCHECKED;
                wf.CHECKEDDATE = ent.CHECKEDDATE;
                wf.NUMDEFECT = ent.NUMDEFECT;
                wf.SFIELD = ent.SFIELD;

                lblWaferID.Text = string.Format("Lot:{0}  Wafer:{1} Defect:{2} Yield:{3}", Oparams[1], Oparams[2], wf.NUMDEFECT, wf.SFIELD);
            }
        }

        /// <summary>
        /// screen capture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblCapture_Click(object sender, EventArgs e)
        {
            FrmCapture frmCapture = new FrmCapture();
            frmCapture.IsCaptureCursor = true;
            frmCapture.Show();
        }

        private void tlsClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            var classId = Convert.ToInt32(tlsClass.ComboBox.SelectedValue);
            var list = _defectlist;

            if (classId != -1)
                list = _defectlist.Where(s => s.Cclassid == Convert.ToInt32(classId)).ToList();

            //grdData.DataSource = new BindingCollection<WmdefectlistEntity>(list);
            grdData.DataSource = list;
        }

        /// <summary>
        /// 保存界面布局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_preview_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsLayoutRole)
            {
                var layout = string.Empty;

                foreach (var item in this.Controls)
                {
                    if (item is Panel)
                    {
                        var control = item as Panel;

                        layout += string.Format("{0}:{1},{2};", control.Name, control.Width, control.Height);
                    }
                }

                layout += string.Format("{0}:{1},{2};", panel4.Name, panel4.Width, panel4.Height);
                layout += string.Format("{0}:{1},{2};", tabControl1.Name, tabControl1.Width, tabControl1.Height);

                System.Configuration.Configuration config = WR.Utils.Config.GetConfig();
                config.AppSettings.Settings.Remove("previewLayout");
                config.AppSettings.Settings.Add("previewLayout", layout);

                config.Save();

                WR.Utils.Config.Refresh();
            }
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //panel4.Height = Convert.ToInt32(panel4.Width * 0.8);
            var height = Convert.ToInt32(panel4.Width * 0.8);

            tabControl1.Height = panel2.Height - height;

            DrawDefect("0,0");
        }

        private void splitter2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            panel2.Width = Convert.ToInt32(panel4.Height * 1.25);

            DrawDefect("0,0");
        }
    }

    public class ClassDropDownModel
    {
        public string Description
        { get; set; }

        public int? Cclassid
        { get; set; }
    }
}
