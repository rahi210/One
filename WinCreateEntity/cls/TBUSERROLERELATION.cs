using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * TB_USERROLERELATION实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("TB_USERROLERELATION")]
    public class TBUSERROLERELATION
    {
        [DataMember]
        public string USERID
        { get;set; }

        [DataMember]
        public string USERID
        { get;set; }

        [DataMember]
        public string ROLEID
        { get;set; }

        [DataMember]
        public string ROLEID
        { get;set; }

    }
}
