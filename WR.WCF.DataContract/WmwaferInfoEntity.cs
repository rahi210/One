using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WmwaferInfoEntity实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class WmwaferInfoEntity
    {
        [DataMember]
        public string resultid
        { get; set; }

        [DataMember]
        public Int64? checkeddate
        { get; set; }

        [DataMember]
        public int? yieldnum
        { get; set; }

        [DataMember]
        public int? defectnum
        { get; set; }

        [DataMember]
        public string dielayoutid
        { get; set; }
    }
}
