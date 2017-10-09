using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WR.WCF.DataContract
{
    public class DiskInfoEntity
    {
        public string Name { get; set; }

        public double TotalSize { get; set; }

        public double FreeSpace { get; set; }

        public double UsedSpace { get; set; }
    }
}
