using System;
using System.Configuration;
using System.IO;

namespace WR.WCF.Site
{
    public class Utils
    {
        /// <summary>
        /// 更新文件路径
        /// </summary>
        public static string AppUpdatePath
        {
            get
            {
                string path = "/AppUpdate/";

                try
                {
                    path = ConfigurationManager.AppSettings["AppFilePath"].Trim('/');
                }
                catch { }

                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
        }

        /// <summary>
        /// 图片保存路径
        /// </summary>
        public static string ImgUpdatePath
        {
            get
            {
                string path = "/ImgFile/";

                try
                {
                    path = ConfigurationManager.AppSettings["ImgFilePath"].Trim('/');
                }
                catch { }

                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
        }

        /// <summary>
        /// 合并文件路径
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }
    }
}