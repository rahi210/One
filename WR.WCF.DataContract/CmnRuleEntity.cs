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
    public class CmnRuleEntity
    {
        [DataMember]
        public string USERID
        { get; set; }

        [DataMember]
        public string RULEID
        { get; set; }

        [DataMember]
        public string RULENAME
        { get; set; }

        [DataMember]
        public string DEVICE
        { get; set; }

        [DataMember]
        public string LAYER
        { get; set; }

        [DataMember]
        public string DESCRP
        { get; set; }
    }
}
