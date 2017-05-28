using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using WR.Utils;
using WR.WCF.DataContract;
using WR.WCF.Client.DataContract;

namespace WR.WCF.Site
{
    public class DataCache
    {
        private static LoggerEx log = LogService.Getlog(typeof(DataCache));

        private static string serialN = "";

        /// <summary>
        /// 是否已经授权
        /// </summary>
        public static int IsAuth
        {
            get
            {
                return CheckLisnese();
            }
        }

        /// <summary>
        /// 服务器是否授权
        /// </summary>
        /// <returns></returns>
        private static int CheckLisnese()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "license.lck");
            if (!File.Exists(file))
                return -1;

            StreamReader sr = new StreamReader(file);
            string lck = sr.ReadToEnd();
            sr.Close();

            if (string.IsNullOrEmpty(lck))
                return -1;

            //log.Fatal(lck);

            try
            {
                string decLck = Encrypt3DES.DecryptECB(lck);
                //log.Fatal(decLck);

                string[] strs = decLck.Split(new char[] { ';' });
                if (strs.Length < 3)
                    return -2;

                //获取序列号
                if (string.IsNullOrEmpty(serialN))
                {
                    var lst = MachineInfo.GetDiskVolumeSerialNumber().OrderBy(p => p.ToLower());
                    string vs = "";
                    foreach (string item in lst)
                    {
                        vs = vs + "|" + item;
                    }
                    serialN = vs.TrimStart('|');
                }

                //log.Fatal(serialN);

                //验证硬盘序列号
                if (serialN != strs[0])
                    return -3;

                //验证有效日期
                DateTime vildDt = DateTime.ParseExact(strs[1], "yyyyMMdd", null);
                if (vildDt.AddDays(1) < DateTime.Now)
                    return -4;

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        private static List<FileEntity> _appFiles;

        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void RefreshCache()
        {
            _appFiles = null;
        }

        /// <summary>
        /// 获取客户端升级文件列表
        /// </summary>
        /// <returns></returns>
        public static List<FileEntity> AppFiles()
        {
            if (_appFiles == null || _appFiles.Count == 0)
            {
                _appFiles = new List<FileEntity>();
                string[] files = System.IO.Directory.GetFiles(Utils.AppUpdatePath);
                foreach (string filename in files)
                {
                    FileInfo fileinfo = new FileInfo(filename);
                    FileEntity file = new FileEntity();
                    file.FileName = fileinfo.Name;
                    FileStream fstream = fileinfo.OpenRead();
                    file.FileLength = fstream.Length;
                    fstream.Close();
                    file.LastTime = fileinfo.LastWriteTime;
                    file.MapPath = Utils.AppUpdatePath;
                    _appFiles.Add(file);
                }
            }

            return _appFiles;
        }
    }
}