using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WM_INSPECTEDDIELIST实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("WM_INSPECTEDDIELIST")]
    public class WMINSPECTEDDIELIST
    {
        [Key]
        [DataMember]
        public string INSPECTEDDIEID
        { get;set; }

        [DataMember]
        public int PASSID
        { get;set; }

        [DataMember]
        public string INSPID
        { get;set; }

        [DataMember]
        public string DIEADDRESS
        { get;set; }

        [DataMember]
        public string CLASSIFICATIONID
        { get;set; }

        [DataMember]
        public string DISPOSITION
        { get;set; }

    }
}
