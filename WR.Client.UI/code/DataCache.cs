using System.Collections.Generic;
using System.Linq;

using WR.Client.WCF;
using WR.WCF.Contract;
using WR.WCF.DataContract;

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

        public static UserInfoEntity UserInfo;

        public static List<CMNDICT> CmnDict;

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
    }
}
