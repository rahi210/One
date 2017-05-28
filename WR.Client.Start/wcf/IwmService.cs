using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using WR.WCF.Client.DataContract;

/*******************************
 * 
 * 程序入口契约
 * 
 * 
 * 
 * ****************************/

namespace WR.WCF.Client.Contract
{
    [ServiceContract]
    public interface IwmService
    {
        [WebGet()]
        List<FileEntity> GetAppFilesList(string updateDate);

        [WebGet(UriTemplate = "{path}/{appfile}")]
        Stream DownloadFileStream(string appfile, string path);

        [WebGet(UriTemplate = "{path}/{appfile}?offset={offset}&count={count}")]
        [OperationContract(Name = "DownloadFileStreamExt")]
        Stream DownloadFileStream(string appfile, string path, long offset = 0, long count = 0);
    }
}
