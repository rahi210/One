using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
//using System.Collections.Generic;

namespace WR.Client.Utils
{
   public  class DrawHelper
    {
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="newW"></param>
        /// <param name="newH"></param>
        /// <returns></returns>
        public static Image ResizeImage(Image bmp, int newW, int newH)
        {
            try
            {
                Bitmap outBmp = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(outBmp);
                g.Clear(Color.Transparent);

                // 插值算法的质量   
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return outBmp;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 剪裁图片
        /// </summary>
        /// <param name="b"></param>
        /// <param name="StartX"></param>
        /// <param name="StartY"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public static Image Cut(Image b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }

            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>  
        /// 将源图像灰度化，但是没有转化为8位灰度图像。  
        /// http://www.bobpowell.net/grayscale.htm  
        /// </summary>  
        /// <param name="original"> 源图像。 </param>  
        /// <returns> 灰度RGB图像。 </returns>  
        public static Bitmap MakeGrayScale(Bitmap original)
        {
            //create a blank bitmap the same size as original  
            Bitmap newBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);

            //get a graphics object from the new image  
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix  
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]   
        {  
            new float[] { .3f, .3f, .3f, 0, 0 },  
            new float[] { .59f, .59f, .59f, 0, 0 },  
            new float[] { .11f, .11f, .11f, 0, 0 },  
            new float[] { 0, 0, 0, 1, 0 },  
            new float[] { 0, 0, 0, 0, 1 }  
        });
            /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
            ┌                          ┐ 
            │  0.3   0.3   0.3   0   0 │ 
            │ 0.59  0.59  0.59   0   0 │ 
            │ 0.11  0.11  0.11   0   0 │ 
            │    0     0     0   1   0 │ 
            │    0     0     0   0   1 │ 
            └                          ┘ 
             * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
            //create some image attributes  
            ImageAttributes attributes = new ImageAttributes();
            //set the color matrix attribute  
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image  
            //using the grayscale color matrix  
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object  
            g.Dispose();
            return newBitmap;
        }

        /// <summary>  
        /// 将源图像灰度化，并转化为8位灰度图像。  
        /// </summary>  
        /// <param name="original"> 源图像。 </param>  
        /// <returns> 8位灰度图像。 </returns>  
        public static Bitmap RgbToGrayScale(Bitmap original)
        {
            if (original != null)
            {
                // 将源图像内存区域锁定  
                Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
                BitmapData bmpData = original.LockBits(rect, ImageLockMode.ReadOnly,
                      original.PixelFormat);

                // 获取图像参数  
                int width = bmpData.Width;
                int height = bmpData.Height;
                int stride = bmpData.Stride;  // 扫描线的宽度  
                int offset = stride - width * 3;  // 显示宽度与扫描线宽度的间隙  
                IntPtr ptr = bmpData.Scan0;   // 获取bmpData的内存起始位置  
                int scanBytes = stride * height;  // 用stride宽度，表示这是内存区域的大小  

                // 分别设置两个位置指针，指向源数组和目标数组  
                int posScan = 0, posDst = 0;
                byte[] rgbValues = new byte[scanBytes];  // 为目标数组分配内存  
                Marshal.Copy(ptr, rgbValues, 0, scanBytes);  // 将图像数据拷贝到rgbValues中  
                // 分配灰度数组  
                byte[] grayValues = new byte[width * height]; // 不含未用空间。  
                // 计算灰度数组  
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        double temp = rgbValues[posScan++] * 0.11 +
                           rgbValues[posScan++] * 0.59 +
                             rgbValues[posScan++] * 0.3;
                        grayValues[posDst++] = (byte)temp;
                    }
                    // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel  
                    posScan += offset;
                }

                // 内存解锁  
                Marshal.Copy(rgbValues, 0, ptr, scanBytes);
                original.UnlockBits(bmpData);  // 解锁内存区域  

                // 构建8位灰度位图  
                Bitmap retBitmap = BuiltGrayBitmap(grayValues, width, height);
                return retBitmap;
            }
            else
            {
                return null;
            }
        }

        /// <summary>  
        /// 用灰度数组新建一个8位灰度图像。  
        /// http://www.cnblogs.com/spadeq/archive/2009/03/17/1414428.html  
        /// </summary>  
        /// <param name="rawValues"> 灰度数组(length = width * height)。 </param>  
        /// <param name="width"> 图像宽度。 </param>  
        /// <param name="height"> 图像高度。 </param>  
        /// <returns> 新建的8位灰度位图。 </returns>  
        private static Bitmap BuiltGrayBitmap(byte[] rawValues, int width, int height)
        {
            // 新建一个8位灰度位图，并锁定内存区域操作  
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            // 计算图像参数  
            int offset = bmpData.Stride - bmpData.Width;        // 计算每行未用空间字节数  
            IntPtr ptr = bmpData.Scan0;                         // 获取首地址  
            int scanBytes = bmpData.Stride * bmpData.Height;    // 图像字节数 = 扫描字节数 * 高度  
            byte[] grayValues = new byte[scanBytes];            // 为图像数据分配内存  

            // 为图像数据赋值  
            int posSrc = 0, posScan = 0;                        // rawValues和grayValues的索引  
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    grayValues[posScan++] = rawValues[posSrc++];
                }
                // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel  
                posScan += offset;
            }

            // 内存解锁  
            Marshal.Copy(grayValues, 0, ptr, scanBytes);
            bitmap.UnlockBits(bmpData);  // 解锁内存区域  

            // 修改生成位图的索引表，从伪彩修改为灰度  
            ColorPalette palette;
            // 获取一个Format8bppIndexed格式图像的Palette对象  
            using (Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                palette = bmp.Palette;
            }
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            // 修改生成位图的索引表  
            bitmap.Palette = palette;

            return bitmap;
        }

        ///// <summary>
        ///// 判断是否选中缺陷
        ///// </summary>
        ///// <param name="polygon"></param>
        ///// <param name="start"></param>
        ///// <returns></returns>
        //public static bool IsPointInPolygon(System.Collections.Generic.List<Point> polygon, Point start)
        //{
        //    int n = polygon.Count;
        //    double x = start.X;
        //    double y = start.Y;
        //    bool inside = false;
        //    int i = 0;
        //    int j = 0;

        //    for (i = 0, j = n - 1; i < n; j = i++)
        //    {
        //        if (((polygon[i].Y > y) != (polygon[j].Y > y)) &&
        //         (x < (polygon[j].X - polygon[i].X) * (y - polygon[i].Y)
        //            / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
        //        {
        //            inside = !inside;
        //        }
        //    }

        //    return inside;
        //}
    }
}
