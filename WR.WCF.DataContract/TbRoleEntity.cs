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
    public class TbRoleEntity
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
        public string FLG
        { get; set; }
    }
}
