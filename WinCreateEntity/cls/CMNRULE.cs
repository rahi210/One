using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * CMN_RULE实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("CMN_RULE")]
    public class CMNRULE
    {
        [DataMember]
        public string ID
        { get;set; }

        [DataMember]
        public string RULENAME
        { get;set; }

        [DataMember]
        public string DESCRP
        { get;set; }

        [DataMember]
        public string DEVICE
        { get;set; }

        [DataMember]
        public string LAYER
        { get;set; }

    }
}
