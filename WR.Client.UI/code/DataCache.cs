using System.Collections.Generic;
using System.Linq;

using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;

using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace WR.Client.UI
{
    public class DataCache
    {
        /// <summary>
        /// 刷新数据
        /// </summary>
        public static void RefreshCache()
        {
            _identifcationInfo = null;
            _waferResultInfo = null;

            InitHotKeyDict();
            int cnt = IdentifcationInfo.Count;
            cnt = WaferResultInfo.Count;
        }

        public static string SinfPath { get; set; }

        public static string SinfType { get; set; }

        public static string BinCodeType { get; set; }

        public static UserInfoEntity UserInfo;

        public static List<CMNDICT> CmnDict;

        public static List<WMYIELDSETTING> YieldSetting { get; set; }

        public static List<TbMenuEntity> Tbmenus;

        private static List<WmidentificationEntity> _identifcationInfo;
        /// <summary>
        /// 机台信息
        /// </summary>
        public static List<WmidentificationEntity> IdentifcationInfo
        {
            get
            {
                if (_identifcationInfo == null)
                {
                    string[] filters = GetFilter();
                    IwrService service = wrService.GetService();
                    //_identifcationInfo = service.GetIdentification(DataCache.UserInfo.ID, filters[0], filters[1], filters[2]);
                    _identifcationInfo = service.GetIdentification(string.Empty, filters[0], filters[1], filters[2]);

                    _identifcationInfo.ForEach(s => s.OPERATOR = DataCache.UserInfo.USERID);
                }

                return _identifcationInfo;
            }
        }

        private static List<WmwaferResultEntity> _waferResultInfo;
        /// <summary>
        /// Wafer检查信息
        /// </summary>
        public static List<WmwaferResultEntity> WaferResultInfo
        {
            get
            {
                if (_waferResultInfo == null)
                {
                    string[] filters = GetFilter();
                    IwrService service = wrService.GetService();
                    //_waferResultInfo = service.GetWaferResult(DataCache.UserInfo.ID, filters[0], filters[1], filters[2]);
                    _waferResultInfo = service.GetWaferResult(string.Empty, filters[0], filters[1], filters[2]);

                    var lotList = ((from w in _waferResultInfo
                                    group w by new { w.DEVICE, w.LAYER, w.LOT } into l
                                    select new { DEVICE = l.Key.DEVICE, LAYER = l.Key.LAYER, LOT = l.Key.LOT, LFIELD = l.Average(s => s.SFIELD) }))
                                    .ToList();

                    _waferResultInfo.ForEach(s => s.LFIELD = lotList.FirstOrDefault(l => l.DEVICE == s.DEVICE && l.LAYER == s.LAYER && l.LOT == s.LOT).LFIELD);

                    if (UserInfo.FilterData)
                    {
                        var newWaferList = ((from w in _waferResultInfo
                                             group w by new { w.DEVICE, w.LAYER, w.LOT, w.SUBSTRATE_ID } into l
                                             select new { DEVICE = l.Key.DEVICE, LAYER = l.Key.LAYER, LOT = l.Key.LOT, SUBSTRATE_ID = l.Key.SUBSTRATE_ID, CREATEDDATE = l.Max(s => s.CREATEDDATE) }))
                                       .ToList();

                        _waferResultInfo = (from w in _waferResultInfo
                                            join n in newWaferList
                                            on new { w.DEVICE, w.LAYER, w.LOT, w.SUBSTRATE_ID, w.CREATEDDATE } equals new { n.DEVICE, n.LAYER, n.LOT, n.SUBSTRATE_ID, n.CREATEDDATE }
                                            select w).ToList();

                        lotList = ((from w in _waferResultInfo
                                    group w by new { w.DEVICE, w.LAYER, w.LOT } into l
                                    select new { DEVICE = l.Key.DEVICE, LAYER = l.Key.LAYER, LOT = l.Key.LOT, LFIELD = l.Average(s => s.SFIELD) }))
                                   .ToList();

                        _waferResultInfo.ForEach(s => s.LFIELD = lotList.FirstOrDefault(l => l.DEVICE == s.DEVICE && l.LAYER == s.LAYER && l.LOT == s.LOT).LFIELD);
                    }

                    _waferResultInfo.ForEach(s => s.RECIPE_ID = string.IsNullOrEmpty(s.RECIPE_ID) ? "" : s.RECIPE_ID);

                    YieldSetting = service.GetAllYieldSetting();
                }

                return _waferResultInfo;
            }
        }

        private decimal? GetLField(List<WmwaferResultEntity> list, string device, string layer, string lot)
        {
            decimal? lField = 0;

            lField = list.FirstOrDefault(s => s.DEVICE == device && s.LAYER == layer && s.LOT == lot).LFIELD;

            return lField;
        }

        /// <summary>
        /// 获取过滤条件
        /// </summary>
        /// <returns></returns>
        private static string[] GetFilter()
        {
            string done = UserInfo.notdone ? "1" : "0";
            string fromday = System.DateTime.Today.ToString("yyyyMMddHHmmss");
            string today = System.DateTime.Today.AddDays(1).ToString("yyyyMMddHHmmss");

            if (UserInfo.lastday)
                fromday = System.DateTime.Today.AddDays(-7).ToString("yyyyMMddHHmmss");
            else if (UserInfo.specifiedday)
            {
                fromday = UserInfo.fromday.ToString() + "000000";
                today = UserInfo.today.ToString() + "235959";
            }
            else if (UserInfo.IntervalDays > 0)
            {
                today = string.Format("{0}235959", System.DateTime.Today.ToString("yyyyMMdd")); ;
                fromday = string.Format("{0}000000", System.DateTime.Today.AddDays(-UserInfo.IntervalDays).ToString("yyyyMMdd"));
            }

            return new string[] { fromday, today, done };
        }

        private static Dictionary<string, string> _hotKeyDict;
        /// <summary>
        /// 初始化快捷键
        /// </summary>
        private static void InitHotKeyDict()
        {
            if (_hotKeyDict == null)
                _hotKeyDict = new Dictionary<string, string>();

            _hotKeyDict.Clear();

            //_hotKeyDict.Add(
        }

        /// <summary>
        /// 缺陷定义快捷键
        /// </summary>
        public static Dictionary<string, string> HotKeyDict
        {
            get { return _hotKeyDict; }
        }

        public static bool HasExam { get; set; }

        /// <summary>
        /// 根据行、列生成全部的layout数据
        /// 由于性能优化，xml中的layout数据不会全部保存到数据库，只会保存一下类型的数据（不存在、缺陷类型！=0）
        /// </summary>
        /// <param name="oldList"></param>
        /// <returns></returns>
        public static List<WmdielayoutlistEntitiy> GetAllDielayoutListById(List<WmdielayoutlistEntitiy> oldList)
        {
            var rows = oldList[0].ROWS_;
            var cols = oldList[0].COLUMNS_;
            var layoutId = oldList[0].LAYOUTID;

            var newList = new List<WmdielayoutlistEntitiy>();

            if (rows * cols == oldList.Count)
                return oldList.Where(s => !s.DISPOSITION.Trim().Equals("NotExist")).ToList();

            for (int i = 0; i < rows; i++)
            {
                for (int y = 0; y < cols; y++)
                {
                    newList.Add(new WmdielayoutlistEntitiy() { ID = System.Guid.NewGuid().ToString(), DIEADDRESSX = y, DIEADDRESSY = i, LAYOUTID = layoutId, COLUMNS_ = cols, ROWS_ = rows, DISPOSITION = "", INSPCLASSIFIID = 0 });
                }
            }

            var list = (from n in newList
                        join o in oldList
                        on new { n.DIEADDRESSX, n.DIEADDRESSY } equals new { o.DIEADDRESSX, o.DIEADDRESSY }
                        into tmp
                        from t in tmp.DefaultIfEmpty()
                        select new WmdielayoutlistEntitiy()
                        {
                            ID = t == null ? n.ID : t.ID,
                            DIEADDRESSX = n.DIEADDRESSX,
                            DIEADDRESSY = n.DIEADDRESSY,
                            LAYOUTID = layoutId,
                            DISPOSITION = t == null ? n.DISPOSITION : t.DISPOSITION,
                            //INSPCLASSIFIID = t == null ? n.INSPCLASSIFIID : t.INSPCLASSIFIID,
                            INSPCLASSIFIID = 0,
                            ROWS_ = rows,
                            COLUMNS_ = cols
                        }).Where(s => !s.DISPOSITION.Trim().Equals("NotExist")).ToList();

            return list;
        }

        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="layoutId"></param>
        /// <returns></returns>
        public static List<WmdielayoutlistEntitiy> GetDielayoutListById(string layoutId)
        {
            IwrService service = wrService.GetService();

            //return service.GetDielayoutListById(layoutId);

            var layoutList = new List<WmdielayoutlistEntitiy>();
            var serializer = new JsonSerializer();

            var byteArray = service.GetDielayoutListById(layoutId);

            if (byteArray.Count > 0 && byteArray[0].Layoutdetails != null)
            {
                var json = System.Text.Encoding.Default.GetString(byteArray[0].Layoutdetails);
                var sr = new StringReader(json);

                object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<WmdielayoutlistEntitiy>));

                layoutList = o as List<WmdielayoutlistEntitiy>;

                if (layoutList.Count > 0)
                {
                    layoutList[0].ROWS_ = byteArray[0].ROWS_;
                    layoutList[0].COLUMNS_ = byteArray[0].COLUMNS_;
                }
            }
            else
            {
                layoutList = byteArray;
            }

            return layoutList;
        }
    }
}
