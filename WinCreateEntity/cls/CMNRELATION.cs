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
        [DataMember]
        public string USERID
        { get;set; }

        [DataMember]
        public string USERID
        { get;set; }

        [DataMember]
        public string USERID
        { get;set; }

        [DataMember]
        public string DEVICE
        { get;set; }

        [DataMember]
        public string DEVICE
        { get;set; }

        [DataMember]
        public string DEVICE
        { get;set; }

        [DataMember]
        public string LAYER
        { get;set; }

        [DataMember]
        public string LAYER
        { get;set; }

        [DataMember]
        public string LAYER
        { get;set; }

    }
}
