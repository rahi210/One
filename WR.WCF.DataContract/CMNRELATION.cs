using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * CMN_RELATION实体类
 * 
 * *****************************************/
namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("CMN_RELATION")]
    public class CMNRELATION
    {
        [Key]
        [Column(Order = 0)]
        [DataMember]
        public string USERID
        { get; set; }

        [Key]
        [Column(Order = 1)]
        [DataMember]
        public string DEVICE
        { get; set; }

        [Key]
        [Column(Order = 2)]
        [DataMember]
        public string LAYER
        { get; set; }

        [DataMember]
        public string RULEID
        { get; set; }
    }
}
