using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * TB_USER实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("TB_USER")]
    public class TBUSER
    {
        [DataMember]
        public string ID
        { get;set; }

        [DataMember]
        public string USERID
        { get;set; }

        [DataMember]
        public string USERNAME
        { get;set; }

        [DataMember]
        public string PWD
        { get;set; }

        [DataMember]
        public string TELEPHONE
        { get;set; }

        [DataMember]
        public string EMAIL
        { get;set; }

        [DataMember]
        public string REMARK
        { get;set; }

        [DataMember]
        public string UPDATEDATE
        { get;set; }

        [DataMember]
        public string UPDATEID
        { get;set; }

        [DataMember]
        public string CREATEDATE
        { get;set; }

        [DataMember]
        public string CREATEID
        { get;set; }

        [DataMember]
        public string DELFLAG
        { get;set; }

    }
}
