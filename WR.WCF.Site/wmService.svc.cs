using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

using WR.Utils;
using WR.WCF.Contract;
using WR.WCF.DataContract;
using WR.WCF.Client.DataContract;

namespace WR.WCF.Site
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“wmService”。
    public class wmService : ServiceBase, WR.WCF.Client.Contract.IwmService
    {
        /// <summary>
        /// 获取需要升级的
        /// </summary>
        /// <param name="updateDate">更新时间</param>
        /// <returns></returns>
        public List<FileEntity> GetAppFilesList(string updateDate)
        {
            try
            {
                List<FileEntity> appFiles = DataCache.AppFiles();

                //return appFiles.Where(p => p.LastTime > DateTime.ParseExact(updateDate, "yyyyMMddHHmmss",
                //    System.Globalization.CultureInfo.GetCultureInfo("zh-CN"))).ToList();
                return appFiles.Where(p => p.LastTime > DateTime.ParseExact(updateDate, "yyyyMMddHHmmss",
                   System.Globalization.CultureInfo.InvariantCulture)).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new FaultException("0");
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="appfile"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public Stream DownloadFileStream(string appfile, string path)
        {
            if (!File.Exists(Utils.PathCombine(path, appfile)))
                throw new FaultException("非法请求。");

            FileStream fstream = new FileStream(Utils.PathCombine(path, appfile), FileMode.Open, FileAccess.Read, FileShare.Read);

            var incomingRequest = WebOperationContext.Current.IncomingRequest;
            var outgoingResponse = WebOperationContext.Current.OutgoingResponse;
            long offset = 0, count = fstream.Length;

            if (incomingRequest.Headers.AllKeys.Contains("Range"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(incomingRequest.Headers["Range"], @"(?<=bytes\b*=)(\d*)-(\d*)");
                if (match.Success)
                {
                    outgoingResponse.StatusCode = System.Net.HttpStatusCode.PartialContent;
                    string v1 = match.Groups[1].Value;
                    string v2 = match.Groups[2].Value;
                    if (!match.NextMatch().Success)
                    {
                        if (v1 == "" && v2 != "")
                        {
                            var r2 = long.Parse(v2);
                            offset = count - r2;
                            count = r2;
                        }
                        else if (v1 != "" && v2 == "")
                        {
                            var r1 = long.Parse(v1);
                            offset = r1;
                            count -= r1;
                        }
                        else if (v1 != "" && v2 != "")
                        {
                            var r1 = long.Parse(v1);
                            var r2 = long.Parse(v2);
                            offset = r1;
                            count -= r2 - r1 + 1;
                        }
                        else
                        {
                            outgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                        }
                    }
                }
                outgoingResponse.ContentType = "application/force-download";
                outgoingResponse.ContentLength = count;

                StreamReaderEx fs = new StreamReaderEx(fstream, offset, count);
                fs.Reading += (t) =>
                {
                    //限速代码,实际使用时可以去掉，或者精确控制
                    //Thread.Sleep(300);
                    //Console.WriteLine();
                };
            }

            return fstream;
        }

        /// <summary>
        /// 断点文件下载
        /// </summary>
        /// <param name="appfile"></param>
        /// <param name="path"></param>
        /// <param name="offset">断点开始位置</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Stream DownloadFileStream(string appfile, string path, long offset = 0, long count = 0)
        {
            if (!File.Exists(Utils.PathCombine(appfile, path)))
                throw new FaultException("非法请求。");

            FileStream fstream = new FileStream(Utils.PathCombine(appfile, path), FileMode.Open, FileAccess.Read, FileShare.Read);

            if (offset >= fstream.Length)
            {
                throw new FaultException("请求的断点超过了文件最大长度。");
            }
            if (count == 0)
            {
                count = fstream.Length - offset;
            }

            var outgoingResponse = WebOperationContext.Current.OutgoingResponse;
            outgoingResponse.ContentType = "application/force-download";

            StreamReaderEx fs = new StreamReaderEx(fstream, offset, count);
            fs.Reading += (t) =>
            {
                //限速代码,实际使用时可以去掉，或者精确控制
                //Thread.Sleep(300);
                //Console.WriteLine(t);
            };
            return fstream;
        }

        //public void UploadFile(WR.WCF.Client.Contract.UpFile upFile )
        //{
        //    int count = 0;
        //    int buffersize = 1024;
        //    byte[] buffer = new byte[buffersize];

        //    if (!Directory.Exists(Utils.ImgUploadPath))
        //    {
        //        Directory.CreateDirectory(Utils.ImgUploadPath);
        //    }

        //    FileStream fstream = new FileStream(Path.Combine(Utils.ImgUploadPath, upFile.FileName), FileMode.Create, FileAccess.Write, FileShare.Write);

        //    while ((count = upFile.FileStream.Read(buffer, 0, buffersize)) > 0)
        //    {
        //        fstream.Write(buffer, 0, count);
        //    }

        //    fstream.Flush();
        //    fstream.Close();
        //}
    }
}
