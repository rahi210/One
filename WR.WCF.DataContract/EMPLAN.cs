using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * EM_PLAN实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("EM_PLAN")]
    public class EMPLAN
    {
        [Key]
        [DataMember]
        public string PID
        { get;set; }

        [DataMember]
        public string LID
        { get;set; }

        [DataMember]
        public DateTime STARTDATE
        { get;set; }

        [DataMember]
        public DateTime ENDDATE
        { get;set; }

        [DataMember]
        public int USERNUM
        { get;set; }

        [DataMember]
        public int ABSENCENUM
        { get;set; }

        [DataMember]
        public decimal PASSNUM
        { get;set; }

        [DataMember]
        public string REMARK
        { get;set; }

        [DataMember]
        public DateTime UPDATEDATE
        { get;set; }

        [DataMember]
        public string UPDATEID
        { get;set; }

        [DataMember]
        public DateTime CREATEDATE
        { get;set; }

        [DataMember]
        public string CREATEID
        { get;set; }

        [DataMember]
        public string DELFLAG
        { get;set; }

        [DataMember]
        public string PLANNAME
        { get;set; }

        [DataMember]
        public int NUMDEFECT
        { get; set; }

    }
}
