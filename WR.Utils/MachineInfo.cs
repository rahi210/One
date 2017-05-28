using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace WR.Utils
{
    public class MachineInfo
    {
        public static ManagementObjectSearcher Sql(string key)
        {
            SelectQuery query = new SelectQuery("Select * From " + key);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            return searcher;
        }

        public static string[] GetDiskVolumeSerialNumber()
        {
            List<string> vols = new List<string>();

            //首先获取物理硬盘序列号
            ManagementObjectSearcher phyMedia = Sql("Win32_PhysicalMedia");
            foreach (ManagementBaseObject disk in phyMedia.Get())
            {
                try
                {
                    if (disk["SerialNumber"] != null)
                    {
                        string v = disk["SerialNumber"].ToString().Trim();
                        if (!string.IsNullOrEmpty(v))
                            vols.Add(v);
                    }
                }
                catch { }
            }

            //如果获取物理硬盘序列号失败，则获取逻辑硬盘编号
            ManagementObjectSearcher Lgcdisk = Sql("win32_logicaldisk ");
            foreach (ManagementBaseObject disk in Lgcdisk.Get())
            {
                if (disk["DeviceID"] != null)
                {
                    try
                    {
                        string v = disk["VolumeSerialNumber"].ToString().Trim();
                        if (!string.IsNullOrEmpty(v))
                            vols.Add(v);
                    }
                    catch { }
                }
            }
            return vols.ToArray();
        }
    }
}
