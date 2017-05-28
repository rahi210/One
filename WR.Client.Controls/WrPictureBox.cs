using System.Drawing;
using System.Windows.Forms;
using WR.Client.Utils;
using System.Collections.Generic;
using System;

namespace WR.Client.Controls
{
    public class WrPictureBox : PictureBox
    {
        public List<DefectCoordinate> DefectList { get; set; }
        public string CurrentDefect { get; set; }

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
        public int ZoomMultiple { get; set; }

        public delegate void DelegateDefectChanged(EventDefectArg e);
        /// <summary>
        /// 缺陷变更
        /// </summary>
        public event DelegateDefectChanged DefectChanged;

        public WrPictureBox()
        {
            this.BackgroundImageLayout = ImageLayout.Center;
            this.Cursor = Cursors.Hand;
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
        }

        /// <summary>
        /// 放大图片
        /// </summary>
        /// <param name="scale"></param>
        public void ZoomOut(int scale)
        {
            if (destImage == null)
                return;

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
                if (destImage.Width <= this.Width && destImage.Height <= this.Height)
                    return;

                mousedownpoint = e.Location;
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
            if (e.Button == MouseButtons.Left && drag)
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

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            drag = false;

            if (DefectList != null)
            {
                if (locX > 0 || locY > 0)
                    GetCurrentDefect(new Point(e.X + locX, e.Y + locY));
                else
                    GetCurrentDefect(new Point(e.X + locStartX, e.Y + locStartY));

            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// 鼠标滚轮
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (destImage != null)
            {
                if (e.Delta > 0)
                {
                    ZoomOut(50);

                    ZoomMultiple += 50;
                }
                else if (e.Delta < 0)
                {
                    ZoomIn(50);

                    ZoomMultiple -= 50;
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
            start.X = Convert.ToInt32(start.X * scaleX);
            start.Y = Convert.ToInt32(start.Y * scaleY);

            var index = DefectList.FindIndex(s => s.Points.IsPointInPolygon(start));

            if (index > -1)
            {
                location = DefectList[index].Location;

                if (CurrentDefect != location)
                    DefectChanged(new EventDefectArg() { Location = location });

                CurrentDefect = location;
            }

            return location;
        }
    }

    /// <summary>
    /// 缺陷
    /// </summary>
    public class DefectCoordinate
    {
        public System.Collections.Generic.List<Point> Points { get; set; }

        public string Location { get; set; }
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
