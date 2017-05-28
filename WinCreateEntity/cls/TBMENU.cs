using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * TB_MENU实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("TB_MENU")]
    public class TBMENU
    {
        [DataMember]
        public string ID
        { get;set; }

        [DataMember]
        public string MENUNAME
        { get;set; }

        [DataMember]
        public string MENUCODE
        { get;set; }

        [DataMember]
        public string UPDATEDATE
        { get;set; }

        [DataMember]
        public string UPDATEID
        { get;set; }

        [DataMember]
        public string CREATEDATE
        { get;set; }

        [DataMember]
        public string CREATEID
        { get;set; }

        [DataMember]
        public string REMARK
        { get;set; }

    }
}
