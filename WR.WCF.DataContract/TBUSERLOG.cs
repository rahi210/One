using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * TB_USERLOG实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("TB_USERLOG")]
    public class TBUSERLOG
    {
        [Key]
        [DataMember]
        public string ID
        { get; set; }

        [DataMember]
        public string USERID
        { get; set; }

        [DataMember]
        public string TYPE
        { get; set; }

        [DataMember]
        public string IP
        { get; set; }

        [DataMember]
        public string REMARK
        { get; set; }

        [DataMember]
        public DateTime CREATEDATE
        { get; set; }
    }
}
