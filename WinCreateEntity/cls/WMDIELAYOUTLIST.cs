using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WM_DIELAYOUTLIST实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("WM_DIELAYOUTLIST")]
    public class WMDIELAYOUTLIST
    {
        [DataMember]
        public string ID
        { get;set; }

        [DataMember]
        public string LAYOUTID
        { get;set; }

        [DataMember]
        public int DIEADDRESSX
        { get;set; }

        [DataMember]
        public int DIEADDRESSY
        { get;set; }

        [DataMember]
        public string DISPOSITION
        { get;set; }

        [DataMember]
        public int INSPCLASSIFIID
        { get;set; }

        [DataMember]
        public int REVIEWCLASSIFCATIONID
        { get;set; }

        [DataMember]
        public int ADCCID
        { get;set; }

        [DataMember]
        public int PSDCID
        { get;set; }

        [DataMember]
        public string ISINSPECTABLE
        { get;set; }

        [DataMember]
        public string ISEDGEDIE
        { get;set; }

    }
}
