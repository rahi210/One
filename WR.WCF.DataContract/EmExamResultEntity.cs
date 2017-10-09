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
    public class EmExamResultEntity
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
        public DateTime STARTDATE
        { get; set; }

        [DataMember]
        public DateTime? ENDDATE
        { get; set; }

        [DataMember]
        public string RESULTID
        { get; set; }

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

        [DataMember]
        public string PLANNAME
        { get; set; }

        [DataMember]
        public DateTime PLANENDDATE
        { get; set; }
    }
}
