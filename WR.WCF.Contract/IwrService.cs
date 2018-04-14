using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using WR.WCF.DataContract;

/*******************************
 * 
 * 程序入口契约
 * 
 * 
 * 
 * ****************************/

namespace WR.WCF.Contract
{
    [ServiceContract]
    public interface IwrService
    {
        [OperationContract]
        List<WMCLASSIFICATIONITEM> GetClassificationItem(string schemeid, string userid);

        [OperationContract]
        List<WMCLASSIFICATIONSCHEME> GetClassificationScheme(string schemeid, string name);

        [OperationContract]
        List<WmClassificationItemEntity> GetClassSummary(string resultid);

        [OperationContract]
        List<WmidentificationEntity> GetIdentification(string operatorid, string fromday, string today, string done);

        [OperationContract]
        List<WmwaferResultEntity> GetWaferResult(string operatorid, string fromday, string today, string done);

        [OperationContract]
        Stream GetPic(string filename, int type = 0);

        [OperationContract]
        Stream GetXml(string filename);

        [OperationContract]
        List<WmdefectlistEntity> GetDefectList(string resultid, string ischecked);

        [OperationContract]
        WmwaferResultEntity UpdateDefect(string resultid, string checkedby, string mclassid, string finish);

        [OperationContract]
        int UpdateClassificationItemUser(string hotkeys, string userid);

        [OperationContract]
        int DeleteWafer(string waferid, string delby);

        [OperationContract]
        int UpdateWaferStatus(string waferid, string status, string reby);

        [OperationContract]
        WMWAFERRESULT GetWaferResultById(string id);

        [OperationContract]
        List<WmdensityReport> GetDensityReport(string lot, string fdate, string tdate, string isFilter);

        [OperationContract]
        WMDIELAYOUT GetDielayoutById(string id);

        [OperationContract]
        List<WmdielayoutlistEntitiy> GetDielayoutListById(string id);

        [OperationContract]
        int[] GetCountInspected(string lot, string stDate, string edDate);

        [OperationContract]
        List<WmCategoryReport> GetCategoryReport(string lot, string stDate, string edDate, string isFilter);

        [OperationContract]
        List<WmDefectiveDieReport> GetDefectiveDieReport(string lot, string stDate, string edDate, string isFilter);

        [OperationContract]
        List<WmInpDieReport> GetDieInspDieReport(string lot, string stDate, string edDate, string isFilter);

        [OperationContract]
        List<WmDefectListReport> GetDefectListReport(string lot, string stDate, string edDate, string isFilter);

        [OperationContract]
        List<WmGoodDieReport> GetGoodDieReport(string lot, string stDate, string edDate, string isFilter);

        [OperationContract]
        List<WMCLASSIFICATIONITEM> GetClassificationItemsByLayer(string lot, string stDate, string edDate);

        [OperationContract]
        List<WMCLASSIFICATIONITEM> GetClassificationItemsByLot(string lot, string stDate, string edDate);

        [OperationContract]
        List<WmItemsSummaryEntity> GetItemsSummaryByLot(string lot, string stDate, string edDate);

        [OperationContract]
        List<WmItemsSummaryEntity> GetDefectSummaryByLot(string lot, string stDate, string edDate);

        [OperationContract]
        List<WmClassificationItemEntity> GetBaseClassificationItem();

        [OperationContract]
        string DataArchive(string sdate, string edate, string type);

        [OperationContract]
        int ImpOrExpDatabase(string type, string date = null);

        [OperationContract]
        List<string> GetDBFilesList();

        [OperationContract]
        List<DiskInfoEntity> GetDiskList();

        [OperationContract]
        bool AddTableSpace(string name);

        [OperationContract]
        List<EMCLASSIFICATIONMARK> GetCLASSIFICATIONMARK(string filter);

        [OperationContract]
        string EditCLASSIFICATIONMARK(string id, string name, int mark);

        [OperationContract]
        //List<WmwaferResultEntity> GetWaferResultHis(string device, string layer, string lot, string wafer);
        List<WmwaferResultEntity> GetWaferResultHis(string stDate, string edDate, string lot);

        [OperationContract]
        List<EMLIBRARY> GetLIBRARY(string filter);

        [OperationContract]
        int AddLibray(string resultId, string papername, string remark, string by, string status);

        [OperationContract]
        int UpdateLibray(string id, string name, string remark, string status, string by);

        [OperationContract]
        int DeleteLibray(string id, string by);

        [OperationContract]
        List<EMPLAN> GetEmPlan(string lid);

        [OperationContract]
        int AddPlan(string lid, string name, string stDate, string edDate, int usernum, int defectnum, string remark, string by);
        [OperationContract]
        int UpdatePlan(string id, string name, string stDate, string edDate, int usernum, int defectnum, string remark, string by);
        [OperationContract]
        int DeletePlan(string id, string by);

        [OperationContract]
        int AddExamResult(string userid, string pid);
        [OperationContract]
        int UpdateExamResult(string resultid, string checkedby, string mclassid, string finish);

        [OperationContract]
        List<EmdefectlistEntity> GetPaperDefectList(string resultid, string ischecked);

        [OperationContract]
        List<EmExamResultEntity> GetExamResultReport(string sdate, string edate, string pname);

        [OperationContract]
        List<EmExamResultEntity> GetExamResult(string by, string eid);
        [OperationContract]
        int GetPaper(string by);
        [OperationContract]
        List<WMCLASSIFICATIONITEM> GetClassificationItemByResultId(string resultid);

        [OperationContract]
        int UpdateWaferResultToReadOnly(string id, string isreview);

        [OperationContract]
        List<WMYIELDSETTING> GetAllYieldSetting();

        [OperationContract]
        int AddYield(string repiceId, string type, decimal layeryield, decimal waferyield, decimal maskayield, decimal maskbyield, decimal maskcyield, decimal maskdyield, decimal maskeyield,string imagename);

        [OperationContract]
        int EditYield(string repiceId, string type, decimal layeryield, decimal waferyield, decimal maskayield, decimal maskbyield, decimal maskcyield, decimal maskdyield, decimal maskeyield, string imagename);

        [OperationContract]
        int DelYield(string repiceId);

        [OperationContract]
        //bool UploadFile(string filename, Stream stream);
        void UploadFile(UpFile stream);
    }

    [MessageContract]
    public class UpFile
    {
        //[MessageHeader]
        //public long FileSize { get; set; }
        [MessageHeader]
        public string FileName { get; set; }
        [MessageBodyMember]
        public Stream FileStream { get; set; }
    }
}
