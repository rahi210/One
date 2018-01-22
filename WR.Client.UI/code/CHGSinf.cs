using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using WR.WCF.DataContract;
using WR.Utils;

namespace WR.Client.UI
{
    public class CHGSinf
    {
        protected LoggerEx log = null;

        public CHGSinf()
        {
            log = LogService.Getlog(this.GetType());
        }

        public bool Export(string path, string device, string lot, string wafer, string notchlocation,
            WMDIELAYOUT dielayout, List<WmdielayoutlistEntitiy> dielist, List<WmdefectlistEntity> defectlist)
        {
            StreamWriter sw = null;
            try
            {
                //特殊坐标点
                string[] AnchorDie = dielayout.ANCHORDIE.Substring(0, dielayout.ANCHORDIE.IndexOf("|")).Split(new char[] { ',' });
                string[] pitch = dielayout.PITCH.Split(new char[] { ',' });
                sw = new StreamWriter(path);

                double xd = double.Parse(pitch[0]);
                double yd = double.Parse(pitch[1]);

                double xxd = xd == 0 ? 0 : xd / 1000;
                double yyd = yd == 0 ? 0 : yd / 1000;

                sw.WriteLine("DEVICE:{0}", device);
                sw.WriteLine("LOT:{0}", lot);
                sw.WriteLine("WAFER:{0}", wafer);
                sw.WriteLine("FNLOC:{0}", notchlocation);
                sw.WriteLine("ROWCT:{0}", dielayout.ROWS_);
                sw.WriteLine("COLCT:{0}", dielayout.COLUMNS_);
                sw.WriteLine("BCEQU:{0}", "00");
                sw.WriteLine("REFPX:{0}", AnchorDie[0]);
                sw.WriteLine("REFPY:{0}", AnchorDie[1]);
                sw.WriteLine("DUTMS:{0}", "MM");
                sw.WriteLine("XDIES:{0:N6}", xxd);
                sw.WriteLine("YDIES:{0:N6}", yyd);

                sw.Flush();
                for (int i = 0; i < dielayout.ROWS_; i++)
                {
                    //var cols = dielist.Where(p => p.DIEADDRESSY == i).OrderBy(p => p.DIEADDRESSX);
                    sw.Write("RowData:");
                    for (int j = 0; j < dielayout.COLUMNS_; j++)
                    {
                        string address = string.Format("{0},{1}", j, dielayout.ROWS_ - 1 - i);
                        var defect = defectlist.FirstOrDefault(p => p.DieAddress == address);

                        if (defect != null)
                        {
                            var cclassid = defectlist.Where(p => p.DieAddress == address).Max(s => s.Cclassid);

                            //sw.Write("{0}", defect.Cclassid.Value.ToString("X").ToUpper().PadLeft(2, '0'));
                            sw.Write("{0}", cclassid.Value.ToString("X").ToUpper().PadLeft(2, '0'));
                            if ((j + 1) != dielayout.COLUMNS_)
                                sw.Write(" ");

                            continue;
                        }

                        var die = dielist.FirstOrDefault(p => p.DIEADDRESSY == dielayout.ROWS_ - 1 - i && p.DIEADDRESSX == j);
                        if (die != null)
                        {
                            if (die.DISPOSITION == "NotProcess     ")
                                sw.Write("@@");
                            else
                                sw.Write("00");

                            if ((j + 1) != dielayout.COLUMNS_)
                                sw.Write(" ");

                            continue;
                        }

                        sw.Write("__");
                        if ((j + 1) != dielayout.COLUMNS_)
                            sw.Write(" ");
                    }

                    if ((i + 1) != dielayout.ROWS_)
                        sw.WriteLine();

                    sw.Flush();
                }
                sw.WriteLine();
                sw.Close();

                return true;
            }
            catch (Exception ex)
            {
                if (sw != null)
                    sw.Close();

                log.Error(ex);
                return false;
            }
        }

        [Obsolete("y轴向下方向的坐标展示，已废弃")]
        public bool Export_old(string path, string device, string lot, string wafer, string notchlocation,
           WMDIELAYOUT dielayout, List<WmdielayoutlistEntitiy> dielist, List<WmdefectlistEntity> defectlist)
        {
            StreamWriter sw = null;
            try
            {
                //特殊坐标点
                string[] AnchorDie = dielayout.ANCHORDIE.Substring(0, dielayout.ANCHORDIE.IndexOf("|")).Split(new char[] { ',' });
                sw = new StreamWriter(path);

                sw.WriteLine("DEVICE:{0}", device);
                sw.WriteLine("LOT:{0}", lot);
                sw.WriteLine("WAFER:{0}", wafer);
                sw.WriteLine("FNLOC:{0}", notchlocation);
                sw.WriteLine("ROWCT:{0}", dielayout.ROWS_);
                sw.WriteLine("COLCT:{0}", dielayout.COLUMNS_);
                sw.WriteLine("BCEQU:{0}", "00");
                sw.WriteLine("REFPX:{0}", AnchorDie[0]);
                sw.WriteLine("REFPY:{0}", AnchorDie[1]);
                sw.WriteLine("DUTMS:{0}", "MM");
                sw.WriteLine("XDIES:{0}", "0.000000");
                sw.WriteLine("YDIES:{0}", "0.000000");
                sw.Flush();
                for (int i = 0; i < dielayout.ROWS_; i++)
                {
                    //var cols = dielist.Where(p => p.DIEADDRESSY == i).OrderBy(p => p.DIEADDRESSX);
                    sw.Write("RowData:");
                    for (int j = 0; j < dielayout.COLUMNS_; j++)
                    {
                        string address = string.Format("{0},{1}", j, i);
                        var defect = defectlist.FirstOrDefault(p => p.DieAddress == address);
                        if (defect != null)
                        {
                            sw.Write("{0}", defect.Cclassid.Value.ToString("X").ToUpper().PadLeft(2, '0'));
                            if ((j + 1) != dielayout.COLUMNS_)
                                sw.Write(" ");

                            continue;
                        }

                        var die = dielist.FirstOrDefault(p => p.DIEADDRESSY == i && p.DIEADDRESSX == j);
                        if (die != null)
                        {
                            sw.Write("00");
                            if ((j + 1) != dielayout.COLUMNS_)
                                sw.Write(" ");

                            continue;
                        }

                        sw.Write("__");
                        if ((j + 1) != dielayout.COLUMNS_)
                            sw.Write(" ");
                    }

                    if ((i + 1) != dielayout.ROWS_)
                        sw.WriteLine();

                    sw.Flush();
                }
                sw.WriteLine();
                sw.Close();

                return true;
            }
            catch (Exception ex)
            {
                if (sw != null)
                    sw.Close();

                log.Error(ex);
                return false;
            }
        }
    }
}
