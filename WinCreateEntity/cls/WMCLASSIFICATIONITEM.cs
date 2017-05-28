using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WM_CLASSIFICATIONITEM实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("WM_CLASSIFICATIONITEM")]
    public class WMCLASSIFICATIONITEM
    {
        [DataMember]
        public string ITEMID
        { get;set; }

        [DataMember]
        public string ITEMID
        { get;set; }

        [DataMember]
        public string SCHEMEID
        { get;set; }

        [DataMember]
        public string SCHEMEID
        { get;set; }

        [DataMember]
        public int ID
        { get;set; }

        [DataMember]
        public int ID
        { get;set; }

        [DataMember]
        public string NAME
        { get;set; }

        [DataMember]
        public string NAME
        { get;set; }

        [DataMember]
        public string DESCRIPTION
        { get;set; }

        [DataMember]
        public string DESCRIPTION
        { get;set; }

        [DataMember]
        public string COLOR
        { get;set; }

        [DataMember]
        public string COLOR
        { get;set; }

        [DataMember]
        public int PRIORITY
        { get;set; }

        [DataMember]
        public int PRIORITY
        { get;set; }

        [DataMember]
        public string HOTKEY
        { get;set; }

        [DataMember]
        public string HOTKEY
        { get;set; }

        [DataMember]
        public string ISACCEPTABLE
        { get;set; }

        [DataMember]
        public string ISACCEPTABLE
        { get;set; }

        [DataMember]
        public string TYPE
        { get;set; }

        [DataMember]
        public string TYPE
        { get;set; }

        [DataMember]
        public string USERID
        { get;set; }

        [DataMember]
        public string USERID
        { get;set; }

    }
}
