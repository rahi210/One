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
        Stream GetPic(string filename);

        [OperationContract]
        Stream GetXml(string filename);

        [OperationContract]
        List<WmdefectlistEntity> GetDefectList(string resultid, string ischecked);

        [OperationContract]
        int UpdateDefect(string resultid, string checkedby, string mclassid, string finish);

        [OperationContract]
        int UpdateClassificationItemUser(string hotkeys, string userid);

        [OperationContract]
        int DeleteWafer(string waferid, string delby);

        [OperationContract]
        int UpdateWaferStatus(string waferid, string status, string reby);

        [OperationContract]
        WMWAFERRESULT GetWaferResultById(string id);

        [OperationContract]
        List<WmdensityReport> GetDensityReport(string lot, string fdate, string tdate);

        [OperationContract]
        WMDIELAYOUT GetDielayoutById(string id);

        [OperationContract]
        List<WmdielayoutlistEntitiy> GetDielayoutListById(string id);

        [OperationContract]
        int[] GetCountInspected(string lot, string stDate, string edDate);

        [OperationContract]
        List<WmCategoryReport> GetCategoryReport(string lot, string stDate, string edDate);

        [OperationContract]
        List<WmDefectiveDieReport> GetDefectiveDieReport(string lot, string stDate, string edDate);

        [OperationContract]
        List<WmInpDieReport> GetDieInspDieReport(string lot, string stDate, string edDate);

        [OperationContract]
        List<WmDefectListReport> GetDefectListReport(string lot, string stDate, string edDate);

        [OperationContract]
        List<WmGoodDieReport> GetGoodDieReport(string lot, string stDate, string edDate);

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
    }
}
