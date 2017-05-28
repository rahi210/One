using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * TbMenuEntity实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class TbMenuEntity
    {
        [DataMember]
        public string ID
        { get; set; }

        [DataMember]
        public string MENUNAME
        { get; set; }

        [DataMember]
        public string MENUCODE
        { get; set; }

        [DataMember]
        public string REMARK
        { get; set; }

        [DataMember]
        public string FLG
        { get; set; }
    }
}
