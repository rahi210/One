using System.Drawing;
using System.Windows.Forms;
using WR.Client.Utils;
using System.Collections.Generic;
using System;
using System.Drawing.Drawing2D;
using System.Collections;

namespace WR.Client.Controls
{
    public class WrPictureBox : PictureBox
    {
        ToolTip tipGray = new ToolTip();

        public List<DefectCoordinate> DefectList { get; set; }
        public List<DieLayout> DieLayoutList { get; set; }
        public int RowCnt { get; set; }
        public int ColCnt { get; set; }
        public string CurrentDefect { get; set; }
        public ArrayList SelectDefect { get; set; }
        public Rectangle SelectRect { get; set; }

        //public List<DefectCoordinate> GoodDieList { get; set; }
        public ArrayList SelectGoodDie { get; set; }
        public string CurrentDie { get; set; }

        //缩放后图片
        private Image destImage;
        //灰度、亮度变化副本
        private Image briImage;

        //鼠标按下坐标
        private Point mousedownpoint = new Point();
        private bool drag = false;

        //拖动时图片原点
        private int locX = 0;
        private int locY = 0;

        private int locStartX = 0;
        private int locStartY = 0;

        //偏移量
        public double scaleX = 1;
        public double scaleY = 1;

        //放大缩小倍数
        //public int ZoomMultiple { get; set; }

        public int ZoomMultiple
        {
            get
            {
                return _zoom;
            }
            set
            {
                this.Status = "";

                if (value > 1)
                {
                    _zoom = value;
                    this.ReDraw();
                }
                else
                {
                    if (HasDraw)
                        _zoom = 1;
                    else
                        _zoom = value;

                    locX = 0;
                    locY = 0;

                    this.ReDraw();
                }
            }
        } private int _zoom = 0;

        //操作状态
        public string Status { get; set; }
        //是否需要重绘
        public bool HasDraw { get; set; }

        //灰度值
        //public double GrayValue { get; set; }
        //是否显示灰度值
        public bool HasShowGrayValue { get; set; }


        public delegate void DelegateDefectChanged(EventDefectArg e);
        /// <summary>
        /// 缺陷变更
        /// </summary>
        public event DelegateDefectChanged DefectChanged;

        public WrPictureBox()
        {
            this.BackgroundImageLayout = ImageLayout.Center;
            this.Cursor = Cursors.Hand;

            SelectDefect = new ArrayList();
            SelectGoodDie = new ArrayList();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private Image _wrImage;
        /// <summary>
        /// 源图片
        /// </summary>
        public Image WrImage
        {
            get { return _wrImage; }
            set
            {
                if (value == null)
                {
                    _wrImage = null;
                    destImage = null;
                    briImage = null;
                    this.BackgroundImage = null;
                    this.Refresh();
                    return;
                }

                _wrImage = value;
                int sWidth = _wrImage.Width;
                int sHeight = _wrImage.Height;
                //自动缩放
                if (this.Width < sWidth || this.Height < sHeight)
                {
                    int sW = 0;
                    int sH = 0;
                    if ((sWidth * this.Height) > (sHeight * this.Width))
                    {
                        sW = this.Width;
                        sH = (this.Width * this.Height) / sWidth;
                    }
                    else
                    {
                        sH = this.Height;
                        sW = (sWidth * this.Height) / sHeight;
                    }

                    destImage = DrawHelper.ResizeImage(_wrImage, sW, sH);
                    briImage = DrawHelper.ResizeImage(_wrImage, sW, sH);
                }
                else
                {
                    destImage = DrawHelper.ResizeImage(_wrImage, _wrImage.Width, _wrImage.Height);
                    briImage = DrawHelper.ResizeImage(_wrImage, _wrImage.Width, _wrImage.Height);
                }

                this.BackgroundImage = destImage;
                //this.Refresh();

                if (!HasDraw)
                {
                    if (ZoomMultiple > 0)
                    {
                        //ZoomOut(ZoomMultiple);
                        int w = (int)(((double)WrImage.Width / (WrImage.Width + WrImage.Height)) * (double)ZoomMultiple);
                        int h = (int)(((double)WrImage.Height / (WrImage.Width + WrImage.Height)) * (double)ZoomMultiple);

                        int destWidth = destImage.Width + (w < 2 ? 2 : w);
                        int destHeight = destImage.Height + (h < 2 ? 2 : h);

                        if (destWidth > 1280)
                            destWidth = 1280;
                        if (destHeight > 1280)
                            destHeight = 1280;

                        scaleX = (double)WrImage.Width / destWidth;
                        scaleY = (double)WrImage.Height / destHeight;

                        destImage = DrawHelper.ResizeImage(WrImage, destWidth, destHeight);
                        this.BackgroundImage = destImage;
                        briImage = DrawHelper.ResizeImage(WrImage, destWidth, destHeight);

                        if (locStartX > 0 || locStartY > 0)
                        {
                            if (destImage.Width - this.Width < locStartY && destImage.Width > this.Width)
                                locStartX = destImage.Width - this.Width;
                            if (destImage.Height - this.Height < locStartY && destImage.Height > this.Height)
                                locStartX = destImage.Height - this.Height;

                            this.BackgroundImage = DrawHelper.Cut(destImage, locStartX, locStartY, this.Width, this.Height);
                        }

                        this.Refresh();
                    }
                    else if (ZoomMultiple < 0)
                        ZoomIn(ZoomMultiple);
                    else
                        this.Refresh();

                    locX = 0;
                    locY = 0;
                }
                else
                    this.Refresh();
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="filename"></param>
        public void SaveImage(string filename)
        {
            if (WrImage != null)
            {
                WrImage.Save(filename);
            }
        }

        /// <summary>
        /// 缩小图片
        /// </summary>
        /// <param name="scale"></param>
        public void ZoomIn(int scale)
        {
            if (destImage == null)
                return;

            if (!HasDraw)
            {
                int w = (int)(((double)WrImage.Width / (WrImage.Width + WrImage.Height)) * (double)scale);
                int h = (int)(((double)WrImage.Height / (WrImage.Width + WrImage.Height)) * (double)scale);
                int destWidth = destImage.Width - (w < 2 ? 2 : w);
                int destHeight = destImage.Height - (h < 2 ? 2 : h);

                if (destWidth < 50)
                    destWidth = 50;
                if (destHeight < 50)
                    destHeight = 50;

                scaleX = (double)WrImage.Width / destWidth;
                scaleY = (double)WrImage.Height / destHeight;

                destImage = DrawHelper.ResizeImage(WrImage, destWidth, destHeight);
                briImage = DrawHelper.ResizeImage(WrImage, destWidth, destHeight);
                this.BackgroundImage = destImage;
                this.Refresh();

                locX = 0;
                locY = 0;

                ZoomMultiple -= scale;
            }
        }

        /// <summary>
        /// 放大图片
        /// </summary>
        /// <param name="scale"></param>
        public void ZoomOut(int scale)
        {
            if (destImage == null)
                return;

            if (!HasDraw)
            {
                int w = (int)(((double)WrImage.Width / (WrImage.Width + WrImage.Height)) * (double)scale);
                int h = (int)(((double)WrImage.Height / (WrImage.Width + WrImage.Height)) * (double)scale);

                int destWidth = destImage.Width + (w < 2 ? 2 : w);
                int destHeight = destImage.Height + (h < 2 ? 2 : h);

                if (destWidth > 1280)
                    destWidth = 1280;
                if (destHeight > 1280)
                    destHeight = 1280;

                scaleX = (double)WrImage.Width / destWidth;
                scaleY = (double)WrImage.Height / destHeight;

                destImage = DrawHelper.ResizeImage(WrImage, destWidth, destHeight);
                this.BackgroundImage = destImage;
                briImage = DrawHelper.ResizeImage(WrImage, destWidth, destHeight);
                this.Refresh();

                locX = 0;
                locY = 0;

                ZoomMultiple += scale;
            }
        }

        /// <summary>
        /// 灰度处理
        /// </summary>
        public void MakeGray()
        {
            if (destImage == null)
                return;

            this.BackgroundImage = DrawHelper.MakeGrayScale(new Bitmap(this.BackgroundImage));
            //MkBrightness();

            this.Refresh();
        }

        /// <summary>
        /// 亮度处理
        /// </summary>
        public void BrightnessP(int step)
        {
            if (briImage == null)
                return;

            //Bitmap a = this.BackgroundImage as Bitmap;
            Bitmap a = briImage.Clone() as Bitmap;

            System.Drawing.Imaging.BitmapData bmpData = a.LockBits(new Rectangle(0, 0, a.Width, a.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int bytes = a.Width * a.Height * 3;
            System.IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;
            unsafe
            {
                byte* p = (byte*)ptr;
                int temp;
                for (int j = 0; j < a.Height; j++)
                {
                    for (int i = 0; i < a.Width * 3; i++, p++)
                    {
                        temp = (int)(p[0] + step * 8);
                        temp = (temp > 255) ? 255 : temp < 0 ? 0 : temp;
                        p[0] = (byte)temp;
                    }
                    p += stride - a.Width * 3;
                }
            }
            a.UnlockBits(bmpData);

            this.BackgroundImage = a;
            this.Refresh();
        }

        /// <summary>
        /// 获取灰度值
        /// </summary>
        /// <param name="step"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double GetGrayValue(int x, int y, int step = 0)
        {
            var gray = 0;
            int localX = 0;
            int localY = 0;

            if (HasShowGrayValue)
            {
                if (briImage == null)
                    return gray;

                localX = x - (int)(this.Width - briImage.Width) / 2;
                localY = y - (int)(this.Height - briImage.Height) / 2;

                if (localX < 0 || localY < 0)
                    return gray;

                if (localX >= briImage.Width || localY >= briImage.Height)
                    return gray;

                //if (localX >= briImage.Width)
                //    localX = briImage.Width - 1;

                //if (localY >= briImage.Height)
                //    localY = briImage.Height - 1;

                Bitmap bitmap = briImage.Clone() as Bitmap;

                var color = bitmap.GetPixel(localX, localY);

                //if (step != 0)
                //    gray = color.R;
                //else
                //    gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);

                tipGray.SetToolTip(this, string.Format("grey value:{0}", gray));
                //tipGray.SetToolTip(this, string.Format("灰度值:{4}-{0},{1}-{2},{3}", x, y, localX, localY,gray));
            }

            return gray;
        }

        /// <summary>
        /// 全灰度处理
        /// </summary>
        public void MkBrightness(int step)
        {
            //Bitmap bitmap = this.BackgroundImage as Bitmap;
            if (briImage == null)
                return;

            Bitmap bitmap = briImage.Clone() as Bitmap;
            if (step == 0)
            {
                this.BackgroundImage = bitmap;
                this.Refresh();
                return;
            }

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    //取图片当前的像素点
                    var color = bitmap.GetPixel(i, j);

                    //var gray = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
                    var gray = 0;
                    switch (step)
                    {
                        case 1:
                            //gray = (int)(color.R * 0.333 + color.G * 0.333 + color.B * 0.334);
                            gray = (int)(color.R * 0.8 + color.G * 0.1 + color.B * 0.1);
                            break;
                        case 2:
                            gray = (int)(color.R * 0.7 + color.G * 0.2 + color.B * 0.1);
                            break;
                        case 3:
                            gray = (int)(color.R * 0.6 + color.G * 0.2 + color.B * 0.2);
                            break;
                        case 4:
                            gray = (int)(color.R * 0.5 + color.G * 0.3 + color.B * 0.2);
                            break;
                        case 5:
                            gray = (int)(color.R * 0.4 + color.G * 0.3 + color.B * 0.3);
                            break;
                        case 6:
                            gray = (int)(color.R * 0.3 + color.G * 0.4 + color.B * 0.3);
                            break;
                        case 7:
                            gray = (int)(color.R * 0.2 + color.G * 0.4 + color.B * 0.4);
                            break;
                        case 8:
                            gray = (int)(color.R * 0.1 + color.G * 0.5 + color.B * 0.4);
                            break;
                        case 9:
                            gray = (int)(color.R * 0.1 + color.G * 0.6 + color.B * 0.3);
                            break;
                        case 10:
                            gray = (int)(color.R * 0.05 + color.G * 0.75 + color.B * 0.2);
                            break;
                        default:
                            break;
                    }

                    //重新设置当前的像素点
                    bitmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }

            this.BackgroundImage = bitmap;
            this.Refresh();
        }

        /// <summary>
        /// 记录鼠标按下坐标
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            drag = false;
            Focus(); //获取焦点

            if (e.Button == MouseButtons.Left && destImage != null)
            {
                //if (destImage.Width <= this.Width && destImage.Height <= this.Height)
                //    return;

                mousedownpoint = e.Location;
                //MessageBox.Show(e.X.ToString()+","+e.Y.ToString()+"-"+e.Location.X.ToString()+","+e.Location.Y.ToString());
                drag = true;
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// 拖动图片
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            locStartX = 0;
            locStartY = 0;

            GetGrayValue(e.Location.X, e.Location.Y);

            if (e.Button == MouseButtons.Left && drag)
            {
                if (HasDraw)
                {
                    if (this.Status != "Reclass")
                    {
                        //locX = locX - (int)((e.Location.X - mousedownpoint.X) / scaleX);
                        //locY = locY - (int)((e.Location.Y - mousedownpoint.Y) / scaleY);

                        if (this.ZoomMultiple > 1)
                        {
                            //var newlocX = locX + (int)((e.Location.X - mousedownpoint.X) / scaleX);
                            //var newlocY = locY + (int)((e.Location.Y - mousedownpoint.Y) / scaleY);
                            locX = locX + (int)((e.Location.X - mousedownpoint.X) / scaleX);
                            locY = locY + (int)((e.Location.Y - mousedownpoint.Y) / scaleY);

                            //if ((Math.Abs(newlocX)*scaleX + e.Location.X) - this.Width <= 0)
                            //    locX = newlocX;
                            //if ((Math.Abs(newlocY * scaleY) + e.Location.Y) - this.Height <= 0)
                            //    locY = newlocY;
                        }
                    }

                    locStartX = e.Location.X;
                    locStartY = e.Location.Y;
                    //this.Status = "";

                    ReDraw();
                }
                else
                {
                    locX = locX - (e.Location.X - mousedownpoint.X);
                    locY = locY - (e.Location.Y - mousedownpoint.Y);

                    if (locX < 0 || destImage.Width <= this.Width)
                        locX = 0;
                    if (locY < 0 || destImage.Height <= this.Height)
                        locY = 0;

                    if (destImage.Width - this.Width < locX && destImage.Width > this.Width)
                        locX = destImage.Width - this.Width;
                    if (destImage.Height - this.Height < locY && destImage.Height > this.Height)
                        locY = destImage.Height - this.Height;

                    locStartX = locX;
                    locStartY = locY;

                    this.BackgroundImage = DrawHelper.Cut(destImage, locX, locY, this.Width, this.Height);
                    this.Refresh();
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            drag = false;

            if (e.Button == MouseButtons.Left && this.ZoomMultiple > 0)
            {
                SelectDefect.Clear();
                SelectGoodDie.Clear();

                //if (locX > 0 || locY > 0)
                //    GetCurrentDefect(new Point(e.X + locX, e.Y + locY));
                //else
                //    GetCurrentDefect(new Point(e.X + locStartX, e.Y + locStartY));

                if ((((e.X - locX - this.mousedownpoint.X) / this.ZoomMultiple) + ((e.Y - locY - this.mousedownpoint.Y) / this.ZoomMultiple)) > 12)
                {
                    //选中的good die
                    foreach (var die in DieLayoutList)
                    {
                        if (!die.IsDefect & die.Points[0].X <= (e.X) / this.ZoomMultiple & die.Points[2].X >= this.mousedownpoint.X / this.ZoomMultiple
                            & die.Points[0].Y <= (e.Y) / this.ZoomMultiple & die.Points[2].Y >= this.mousedownpoint.Y / this.ZoomMultiple)
                        {
                            SelectGoodDie.Add(die.X + "," + die.Y);
                        }
                    }

                    //if (SelectGoodDie.Count > 0)
                    //    DefectChanged(new EventDefectArg() { Location = SelectGoodDie[0].ToString() });
                    foreach (var defect in DefectList)
                    {
                        if (defect.Points[0].X <= (e.X) / this.ZoomMultiple & defect.Points[2].X >= this.mousedownpoint.X / this.ZoomMultiple
                            & defect.Points[0].Y <= (e.Y) / this.ZoomMultiple & defect.Points[2].Y >= this.mousedownpoint.Y / this.ZoomMultiple)
                        {
                            SelectDefect.Add(defect.Location);
                        }
                    }

                    if (SelectDefect.Count > 0)
                        DefectChanged(new EventDefectArg() { Location = SelectDefect[0].ToString() });
                    else if (SelectGoodDie.Count > 0)
                    {
                        this.CurrentDefect = SelectGoodDie[0].ToString();
                        ReDraw();
                    }
                }
                else
                {
                    //MessageBox.Show(e.X.ToString()+","+e.Y.ToString());

                    GetCurrentDefect(new Point((int)e.X / this.ZoomMultiple, (int)e.Y / this.ZoomMultiple));
                }
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// 鼠标滚轮
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //if (destImage != null)
            //{
            //    if (e.Delta > 0)
            //    {
            //        ZoomOut(50);

            //        ZoomMultiple += 50;
            //    }
            //    else if (e.Delta < 0)
            //    {
            //        ZoomIn(50);

            //        ZoomMultiple -= 50;
            //    }
            //}

            if (destImage != null)
            {
                if (e.Delta > 0)
                {
                    if (HasDraw)
                        ZoomMultiple += 1;
                    else
                    {
                        ZoomOut(50);
                    }
                }
                else if (e.Delta < 0)
                {
                    if (HasDraw)
                        ZoomMultiple -= 1;
                    else
                    {
                        ZoomIn(50);
                    }
                }
            }

            base.OnMouseWheel(e);
        }

        /// <summary>
        /// 获取选中缺陷的位置
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        private string GetCurrentDefect(Point start)
        {
            var location = string.Empty;

            //根据偏移量计算坐标
            //start.X = Convert.ToInt32(start.X * scaleX);
            //start.Y = Convert.ToInt32(start.Y * scaleY);
            if (DefectList == null)
                return location;

            var index = DefectList.FindIndex(s => s.Points.IsPointInPolygon(start));

            if (index > -1)
            {
                location = DefectList[index].Location;

                SelectDefect.Add(location);

                if (CurrentDefect != location)
                    DefectChanged(new EventDefectArg() { Location = location });

                CurrentDefect = location;
            }
            else
            {
                index = DieLayoutList.FindIndex(s => s.Points.IsPointInPolygon(start));

                if (index > -1)
                {
                    location = DieLayoutList[index].X + "," + DieLayoutList[index].Y;

                    SelectGoodDie.Add(location);

                    if (CurrentDefect != location)
                    {
                        CurrentDefect = location;
                        ReDraw();
                    }
                }

            }

            return location;
        }

        private Brush _bgColor = new SolidBrush(SystemColors.ControlDarkDark);
        private Brush _egPen = new SolidBrush(SystemColors.Control);
        private Pen _linePen = new Pen(Color.White);

        private Brush _dPen = new SolidBrush(Color.Black);
        private Brush _lPen = new SolidBrush(Color.DarkGreen);
        private Brush _rPen = new SolidBrush(Color.Red);

        /// <summary>
        /// 画图
        /// </summary>
        public void ReDraw()
        {
            if (!HasDraw)
                return;

            var col = ColCnt;
            var row = RowCnt;
            string loction = CurrentDefect;
            List<DieLayout> _dielayoutlist = DieLayoutList;
            List<DefectCoordinate> _defectlist = DefectList;

            if (_dielayoutlist == null || _dielayoutlist.Count < 1)
                return;

            if (ZoomMultiple <= 0)
                ZoomMultiple = 1;

            ////拖拽坐标
            //int dragX = locX;
            //int dragY = locY;

            //die宽、高
            int ww = 5;
            int wh = 4;
            int wd = col * ww + 40;
            int hg = row * wh + 40;

            if (col == row)
            {
                ww = 4;
                wh = 4;
               
                hg = row * wh + 60;
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
                hg = row * wh + 80;
            }
            else if ((col - row) < 10)
            {
                wd = col * ww + 60;
                hg = row * wh + 60;
            }

            //计算偏移量
            int offsetX = (wd - col * ww) / 2 + locX;
            int offsetY = (hg - row * wh) / 2 + locY;

            //背景图
            Bitmap btp = new Bitmap(wd, hg);
            Graphics gc = Graphics.FromImage(btp);
            gc.Clear(Color.White);
            gc.SmoothingMode = SmoothingMode.HighSpeed;

            GraphicsPath ep = new GraphicsPath();
            //ep.AddEllipse(0, 0, btp.Width * ZoomMultiple, btp.Height * ZoomMultiple);
            ep.AddEllipse(locX * ZoomMultiple, locY * ZoomMultiple, btp.Width * ZoomMultiple, btp.Height * ZoomMultiple);
            gc.FillPath(_bgColor, ep);

            //背景颜色
            GraphicsPath bp = new GraphicsPath();
            //晶片颜色
            GraphicsPath wp = new GraphicsPath();
            //缺陷晶片颜色
            GraphicsPath rp = new GraphicsPath();

            bool lineg = false;
            int lx = 0;
            int ly = 0;

            double scaleX = Math.Round(Convert.ToDouble(this.Width) / wd, 8);
            double scaleY = Math.Round(Convert.ToDouble(this.Height) / hg, 8);

            //画出die
            foreach (DieLayout die in _dielayoutlist)
            {
                bp.AddRectangle(new Rectangle((die.X * ww + offsetX) * this.ZoomMultiple, ((row - die.Y) * wh + offsetY) * this.ZoomMultiple, ww * this.ZoomMultiple, wh * this.ZoomMultiple));
                wp.AddRectangle(new Rectangle((die.X * ww + offsetX) * this.ZoomMultiple, ((row - die.Y) * wh + offsetY) * this.ZoomMultiple, (ww - 1) * this.ZoomMultiple, (wh - 1) * this.ZoomMultiple));

                if (!string.IsNullOrEmpty(die.FillColor))
                    gc.FillRectangle(new SolidBrush(ConvterColor(Color.Gray.Name)), (die.X * ww + offsetX) * this.ZoomMultiple, ((row - die.Y) * wh + offsetY) * this.ZoomMultiple, (ww - 1) * this.ZoomMultiple, (ww - 1) * this.ZoomMultiple);

                if (string.Format("{0},{1}", die.X, die.Y) == loction)
                {
                    lineg = true;
                    lx = die.X;
                    ly = row - die.Y;

                    //System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red, 2);
                    //myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    //gc.DrawRectangle(myPen, (die.X * ww + offsetX) * this.ZoomMultiple, ((row - die.Y) * wh + offsetY) * this.ZoomMultiple, ww * this.ZoomMultiple, wh * this.ZoomMultiple);
                    //myPen.Dispose();
                }

                die.Points = new List<Point>() { new Point(Convert.ToInt32((die.X * ww + offsetX) * scaleX),Convert.ToInt32(((row - die.Y) * wh + offsetY) * scaleY))
                    ,new Point(Convert.ToInt32((die.X * ww + offsetX+ww-1) * scaleX),Convert.ToInt32(((row - die.Y) * wh + offsetY) * scaleY))
                    ,new Point(Convert.ToInt32((die.X * ww + offsetX+ww-1) * scaleX),Convert.ToInt32(((row - die.Y) * wh + offsetY+wh-1) * scaleY))
                    ,new Point(Convert.ToInt32((die.X * ww + offsetX) * scaleX),Convert.ToInt32(((row - die.Y) * wh + offsetY+wh-1) * scaleY))};
            }

            gc.FillPath(_dPen, bp);
            gc.FillPath(_lPen, wp);

            //bool lineg = false;
            //int lx = 0;
            //int ly = 0;

            //double scaleX = Math.Round(Convert.ToDouble(this.Width) / wd, 8);
            //double scaleY = Math.Round(Convert.ToDouble(this.Height) / hg, 8);

            //画出defect
            foreach (DefectCoordinate def in _defectlist)
            {
                if (string.IsNullOrEmpty(def.Location))
                    continue;

                string[] adr = def.Location.Split(new char[] { ',' });
                int ax = int.Parse(adr[0]);
                int ay = int.Parse(adr[1]);

                gc.FillRectangle(new SolidBrush(ConvterColor(def.FillColor)), (ax * ww + offsetX) * this.ZoomMultiple, ((row - ay) * wh + offsetY) * this.ZoomMultiple, (ww - 1) * this.ZoomMultiple, (wh - 1) * this.ZoomMultiple);

                def.Points = new List<Point>() { new Point(Convert.ToInt32((ax * ww + offsetX) * scaleX),Convert.ToInt32(((row - ay) * wh + offsetY) * scaleY))
                    ,new Point(Convert.ToInt32((ax * ww + offsetX+ww-1) * scaleX),Convert.ToInt32(((row - ay) * wh + offsetY) * scaleY))
                    ,new Point(Convert.ToInt32((ax * ww + offsetX+ww-1) * scaleX),Convert.ToInt32(((row - ay) * wh + offsetY+wh-1) * scaleY))
                    ,new Point(Convert.ToInt32((ax * ww + offsetX) * scaleX),Convert.ToInt32(((row - ay) * wh + offsetY+wh-1) * scaleY))};

                //判断定位画线
                if (def.Location == loction)
                {
                    lineg = true;
                    lx = ax;
                    ly = row - ay;

                    //System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
                    //myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    //gc.DrawRectangle(myPen, (ax * ww + offsetX) * this.ZoomMultiple, ((row - ay) * wh + offsetY) * this.ZoomMultiple, (ww - 1) * this.ZoomMultiple, (wh - 1) * this.ZoomMultiple);
                    //myPen.Dispose();
                }
            }

            //定位画线
            if (lineg)
            {
                gc.DrawLine(_linePen, 0, (ly * wh + offsetY + 1) * this.ZoomMultiple, btp.Width * this.ZoomMultiple, (ly * wh + offsetY + 1) * this.ZoomMultiple);
                gc.DrawLine(_linePen, (lx * ww + offsetX + 1) * this.ZoomMultiple, 0, (lx * ww + offsetX + 1) * this.ZoomMultiple, btp.Height * this.ZoomMultiple);
            }

            //画出定位三角
            Point p1 = new Point((btp.Width / 2) * this.ZoomMultiple, (btp.Height - 10) * this.ZoomMultiple);
            Point p2 = new Point((btp.Width / 2 - 6) * this.ZoomMultiple, btp.Height * this.ZoomMultiple);
            Point p3 = new Point((btp.Width / 2 + 6) * this.ZoomMultiple, btp.Height * this.ZoomMultiple);
            //Point p1 = new Point(btp.Width / 2, btp.Height - 10);
            //Point p2 = new Point(btp.Width / 2 - 6, btp.Height);
            //Point p3 = new Point(btp.Width / 2 + 6, btp.Height);
            gc.FillPolygon(_egPen, new Point[] { p1, p2, p3 }, System.Drawing.Drawing2D.FillMode.Alternate);

            if (this.Status == "Reclass" || Status == "ReDie")
            {
                //System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red, 1.5f * this.ZoomMultiple);
                System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red, (float)(1.5 / scaleX));
                myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                gc.DrawRectangle(myPen, (int)((this.mousedownpoint.X) / scaleX), (int)((this.mousedownpoint.Y) / scaleY),
                    (int)((this.locStartX - this.mousedownpoint.X) / scaleX), (int)((this.locStartY - this.mousedownpoint.Y) / scaleY));

                if (locStartX == 0 || locStartY == 0)
                    SelectRect = new Rectangle((int)((this.mousedownpoint.X)), (int)((this.mousedownpoint.Y)),
                    ww * this.ZoomMultiple, wh * this.ZoomMultiple);
                else
                    SelectRect = new Rectangle((int)((this.mousedownpoint.X)), (int)((this.mousedownpoint.Y)),
                        (int)((this.locStartX - this.mousedownpoint.X)), (int)((this.locStartY - this.mousedownpoint.Y)));

                myPen.Dispose();
            }

            gc.Dispose();

            //缩略图片
            Bitmap outBmp = new Bitmap(this.Width, this.Height);

            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.DrawImage(btp, new Rectangle(0, 0, outBmp.Width, outBmp.Height), new Rectangle(0, 0, btp.Width, btp.Height), GraphicsUnit.Pixel);
            g.Dispose();

            //绑定图片
            this.WrImage = outBmp;
            this.scaleX = scaleX;
            this.scaleY = scaleY;

            HasDraw = true;
            //this.Refresh();
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
                //return ColorTranslator.FromHtml(color);
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
    }

    /// <summary>
    /// 缺陷
    /// </summary>
    public class DefectCoordinate
    {
        public System.Collections.Generic.List<Point> Points { get; set; }

        public string Location { get; set; }

        public string FillColor { get; set; }

        public bool IsSelected { get; set; }

        public bool IsAdd { get; set; }
    }

    public class DieLayout
    {
        public int X { get; set; }
        public int Y { get; set; }

        public string FillColor { get; set; }

        public System.Collections.Generic.List<Point> Points { get; set; }

        public bool IsDefect { get; set; }
    }

    public static class PointHelper
    {
        /// <summary>
        /// 判断是否选中缺陷
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static bool IsPointInPolygon(this System.Collections.Generic.List<Point> polygon, Point start)
        {
            int n = polygon.Count;
            double x = start.X;
            double y = start.Y;
            bool inside = false;
            int i = 0;
            int j = 0;

            for (i = 0, j = n - 1; i < n; j = i++)
            {
                if (((polygon[i].Y > y) != (polygon[j].Y > y)) &&
                 (x < (polygon[j].X - polygon[i].X) * (y - polygon[i].Y)
                    / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }

            return inside;
        }
    }

    public class EventDefectArg : EventArgs
    {
        public string Location { get; set; }
    }
}
