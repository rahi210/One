using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * EM_EXAMRESULT实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("EM_EXAMRESULT")]
    public class EMEXAMRESULT
    {
        [Key]
        [DataMember]
        public string EID
        { get; set; }

        [DataMember]
        public string PLANID
        { get; set; }

        [DataMember]
        public int TOTALSCORE
        { get; set; }

        [DataMember]
        public string REMARK
        { get; set; }

        [DataMember]
        public DateTime UPDATEDATE
        { get; set; }

        [DataMember]
        public string UPDATEID
        { get; set; }

        [DataMember]
        public DateTime CREATEDATE
        { get; set; }

        [DataMember]
        public string CREATEID
        { get; set; }

        [DataMember]
        public string DELFLAG
        { get; set; }

        [DataMember]
        public DateTime STARTDATE
        { get; set; }

        [DataMember]
        public DateTime? ENDDATE
        { get; set; }

        //[DataMember]
        //public string RESULTID
        //{ get; set; }

        [DataMember]
        public string USERID
        { get; set; }

        [DataMember]
        public int RIGHTNUM
        { get; set; }

        [DataMember]
        public int ERRORNUM
        { get; set; }

        [DataMember]
        public int NUMDEFECT
        { get; set; }

    }
}
