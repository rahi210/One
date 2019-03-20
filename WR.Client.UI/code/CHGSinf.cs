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
            var sinfList = dielist.OrderBy(s => s.DIEADDRESSX).OrderByDescending(s => s.DIEADDRESSY).ToList();

            var sinfString = new StringBuilder();

            var rowIndex = 0;

            try
            {
                if (DataCache.SinfType == "000")
                {
                    //特殊坐标点
                    string[] AnchorDie = dielayout.ANCHORDIE.Substring(0, dielayout.ANCHORDIE.IndexOf("|")).Split(new char[] { ',' });
                    string[] pitch = dielayout.PITCH.Split(new char[] { ',' });

                    double xd = double.Parse(pitch[0]);
                    double yd = double.Parse(pitch[1]);

                    double xxd = xd == 0 ? 0 : xd / 1000;
                    double yyd = yd == 0 ? 0 : yd / 1000;

                    sinfString.AppendLine(string.Format("DEVICE:{0}", device));
                    sinfString.AppendLine(string.Format("LOT:{0}", lot));
                    sinfString.AppendLine(string.Format("WAFER:{0}", wafer));
                    sinfString.AppendLine(string.Format("FNLOC:{0}", notchlocation));
                    sinfString.AppendLine(string.Format("ROWCT:{0}", dielayout.ROWS_));
                    sinfString.AppendLine(string.Format("COLCT:{0}", dielayout.COLUMNS_));
                    sinfString.AppendLine(string.Format("BCEQU:{0}", "000"));
                    sinfString.AppendLine(string.Format("REFPX:{0}", AnchorDie[0]));
                    sinfString.AppendLine(string.Format("REFPY:{0}", AnchorDie[1]));
                    sinfString.AppendLine(string.Format("DUTMS:{0}", "MM"));
                    sinfString.AppendLine(string.Format("XDIES:{0:N6}", xxd));
                    sinfString.AppendLine(string.Format("YDIES:{0:N6}", yyd));

                    for (int i = 0; i < sinfList.Count; i++)
                    {
                        if (sinfList[i].DIEADDRESSY != rowIndex)
                        {
                            rowIndex = sinfList[i].DIEADDRESSY;
                            sinfString.AppendFormat("RowData:");
                        }

                        var j = sinfList[i].DIEADDRESSX;

                        string address = string.Format("{0},{1}", j, sinfList[i].DIEADDRESSY);
                        var defect = defectlist.FirstOrDefault(p => p.DieAddress == address);

                        if (defect != null)
                        {
                            var cclassid = defectlist.Where(p => p.DieAddress == address).Max(s => s.Cclassid);

                            if (DataCache.BinCodeType == "10")
                                sinfString.AppendFormat("{0}", cclassid.Value.ToString("D3"));
                            else
                                sinfString.AppendFormat("{0}", cclassid.Value.ToString("X").ToUpper().PadLeft(3, '0'));
                        }
                        else
                        {
                            if (sinfList[i].DISPOSITION != "NotExist")
                            {
                                if (sinfList[i].DISPOSITION == "NotProcess     ")
                                    sinfString.AppendFormat("@@@");
                                else
                                    sinfString.AppendFormat("000");
                            }
                            else
                            {
                                sinfString.AppendFormat("___");
                            }
                        }

                        if ((sinfList[i].DIEADDRESSX + 1) != dielayout.COLUMNS_)
                            sinfString.AppendFormat(" ");
                        else
                            sinfString.AppendLine();
                    }
                }
                else
                {
                    //特殊坐标点
                    string[] AnchorDie = dielayout.ANCHORDIE.Substring(0, dielayout.ANCHORDIE.IndexOf("|")).Split(new char[] { ',' });
                    string[] pitch = dielayout.PITCH.Split(new char[] { ',' });

                    double xd = double.Parse(pitch[0]);
                    double yd = double.Parse(pitch[1]);

                    double xxd = xd == 0 ? 0 : xd / 1000;
                    double yyd = yd == 0 ? 0 : yd / 1000;

                    sinfString.AppendLine(string.Format("DEVICE:{0}", device));
                    sinfString.AppendLine(string.Format("LOT:{0}", lot));
                    sinfString.AppendLine(string.Format("WAFER:{0}", wafer));
                    sinfString.AppendLine(string.Format("FNLOC:{0}", notchlocation));
                    sinfString.AppendLine(string.Format("ROWCT:{0}", dielayout.ROWS_));
                    sinfString.AppendLine(string.Format("COLCT:{0}", dielayout.COLUMNS_));
                    sinfString.AppendLine(string.Format("BCEQU:{0}", "00"));
                    sinfString.AppendLine(string.Format("REFPX:{0}", AnchorDie[0]));
                    sinfString.AppendLine(string.Format("REFPY:{0}", AnchorDie[1]));
                    sinfString.AppendLine(string.Format("DUTMS:{0}", "MM"));
                    sinfString.AppendLine(string.Format("XDIES:{0:N6}", xxd));
                    sinfString.AppendLine(string.Format("YDIES:{0:N6}", yyd));

                    for (int i = 0; i < sinfList.Count; i++)
                    {
                        if (sinfList[i].DIEADDRESSY != rowIndex)
                        {
                            rowIndex = sinfList[i].DIEADDRESSY;
                            sinfString.AppendFormat("RowData:");
                        }

                        var j = sinfList[i].DIEADDRESSX;

                        string address = string.Format("{0},{1}", j, sinfList[i].DIEADDRESSY);
                        var defect = defectlist.FirstOrDefault(p => p.DieAddress == address);

                        if (defect != null)
                        {
                            var cclassid = defectlist.Where(p => p.DieAddress == address).Max(s => s.Cclassid);

                            if (DataCache.BinCodeType == "10")
                                sinfString.AppendFormat("{0}", cclassid.Value.ToString("D2"));
                            else
                                sinfString.AppendFormat("{0}", cclassid.Value.ToString("X").ToUpper().PadLeft(2, '0'));
                        }
                        else
                        {
                            if (sinfList[i].DISPOSITION != "NotExist")
                            {
                                if (sinfList[i].DISPOSITION == "NotProcess     ")
                                    sinfString.AppendFormat("@@");
                                else
                                    sinfString.AppendFormat("00");
                            }
                            else
                            {
                                sinfString.AppendFormat("__");
                            }
                        }

                        if ((sinfList[i].DIEADDRESSX + 1) != dielayout.COLUMNS_)
                            sinfString.AppendFormat(" ");
                        else
                            sinfString.AppendLine();
                    }
                }

                using (FileStream fs = File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(sinfString.ToString());

                    fs.Write(info, 0, info.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
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
