using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * CMN_DICT实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("CMN_DICT")]
    public class CMNDICT
    {
        [DataMember]
        public string DICTID
        { get;set; }

        [DataMember]
        public string DICTID
        { get;set; }

        [DataMember]
        public string CODE
        { get;set; }

        [DataMember]
        public string CODE
        { get;set; }

        [DataMember]
        public string NAME
        { get;set; }

        [DataMember]
        public string NAME
        { get;set; }

        [DataMember]
        public string REMARK
        { get;set; }

        [DataMember]
        public string REMARK
        { get;set; }

    }
}
