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
        [Key]
        [Column(Order = 1)]
        [DataMember]
        public string ROLEID
        { get; set; }

        [Key]
        [Column(Order = 2)]
        [DataMember]
        public string MENUID
        { get; set; }
    }
}
