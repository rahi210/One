using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * EM_LIBRARY实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("EM_LIBRARY")]
    public class EMLIBRARY
    {
        [Key]
        [DataMember]
        public string LID
        { get;set; }

        [DataMember]
        public string PAPERNAME
        { get;set; }

        [DataMember]
        public string PAPERTYPE
        { get;set; }

        [DataMember]
        public string RESULTID
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
        public string STATUS
        { get;set; }

        [DataMember]
        public int NUMDEFECT
        { get;set; }

    }
}
