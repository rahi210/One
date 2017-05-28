using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WmdielayoutlistEntitiy实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    public class WmdielayoutlistEntitiy
    {
        [DataMember]
        public string ID
        { get; set; }

        [DataMember]
        public string LAYOUTID
        { get; set; }

        [DataMember]
        public int DIEADDRESSX
        { get; set; }

        [DataMember]
        public int DIEADDRESSY
        { get; set; }

        [DataMember]
        public string DISPOSITION
        { get; set; }

        [DataMember]
        public int INSPCLASSIFIID
        { get; set; }

        [DataMember]
        public string ISINSPECTABLE
        { get; set; }

        [DataMember]
        public int COLUMNS_
        { get; set; }

        [DataMember]
        public int ROWS_
        { get; set; }
    }
}
