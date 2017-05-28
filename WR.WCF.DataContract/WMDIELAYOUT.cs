using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WM_DIELAYOUT实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("WM_DIELAYOUT")]
    public class WMDIELAYOUT
    {
        [Key]
        [DataMember]
        public string LAYOUTID
        { get; set; }

        [DataMember]
        public string DIEADDRESSRANGE
        { get; set; }

        [DataMember]
        public string ANCHORDIE
        { get; set; }

        [DataMember]
        public string SIZE_
        { get; set; }

        [DataMember]
        public string PITCH
        { get; set; }

        [DataMember]
        public string DIEADDRESS
        { get; set; }

        [DataMember]
        public string SWCSCOORDINATES
        { get; set; }

        [DataMember]
        public string DIEADDRESSINCREMENT
        { get; set; }

        [DataMember]
        public int COLUMNS_
        { get; set; }

        [DataMember]
        public int ROWS_
        { get; set; }
    }
}
