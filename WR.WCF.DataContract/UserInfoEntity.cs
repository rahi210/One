using System.Collections.Generic;
using System.Runtime.Serialization;

/********************************************
 * 实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class UserInfoEntity
    {
        [DataMember]
        public int IsOK
        { get; set; }

        [DataMember]
        public string Msg
        { get; set; }

        [DataMember]
        public string ID
        { get; set; }

        [DataMember]
        public string USERID
        { get; set; }

        [DataMember]
        public string USERNAME
        { get; set; }

        [DataMember]
        public string PASSWORD
        { get; set; }

        [DataMember]
        public string TELEPHONE
        { get; set; }

        [DataMember]
        public string EMAIL
        { get; set; }

        [DataMember]
        public string RE_REVIEW
        { get; set; }

        [DataMember]
        public List<TBMENU> MenuList
        { get; set; }

        /// <summary>
        /// 加载历史未完成记录
        /// </summary>
        [DataMember]
        public bool notdone
        { get; set; }

        /// <summary>
        /// 加载当天数据
        /// </summary>
        [DataMember]
        public bool theday
        { get; set; }

        /// <summary>
        /// 加载最近一周数据
        /// </summary>
        [DataMember]
        public bool lastday
        { get; set; }

        /// <summary>
        /// 加载最近一周数据
        /// </summary>
        [DataMember]
        public bool specifiedday
        { get; set; }

        [DataMember]
        public int fromday
        { get; set; }

        [DataMember]
        public int today
        { get; set; }

        [DataMember]
        public int IntervalDays { get; set; }

        /// <summary>
        /// 过滤重复数据
        /// </summary>
        public bool FilterData { get; set; }
    }
}
