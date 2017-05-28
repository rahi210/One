using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WM_CLASSIFICATIONSCHEME实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("WM_CLASSIFICATIONSCHEME")]
    public class WMCLASSIFICATIONSCHEME
    {
        [DataMember]
        public string SCHEMEID
        { get;set; }

        [DataMember]
        public string COMPUTERNAME
        { get;set; }

        [DataMember]
        public string NAME
        { get;set; }

    }
}
