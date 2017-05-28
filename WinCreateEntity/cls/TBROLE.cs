using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * TB_ROLE实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("TB_ROLE")]
    public class TBROLE
    {
        [DataMember]
        public string ID
        { get;set; }

        [DataMember]
        public string ROLENAME
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

    }
}
