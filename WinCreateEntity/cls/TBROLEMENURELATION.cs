using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * TB_ROLEMENURELATION实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("TB_ROLEMENURELATION")]
    public class TBROLEMENURELATION
    {
        [DataMember]
        public string ROLEID
        { get;set; }

        [DataMember]
        public string ROLEID
        { get;set; }

        [DataMember]
        public string MENUID
        { get;set; }

        [DataMember]
        public string MENUID
        { get;set; }

    }
}
