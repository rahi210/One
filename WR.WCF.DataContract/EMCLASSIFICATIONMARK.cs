using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * EM_CLASSIFICATIONMARK实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("EM_CLASSIFICATIONMARK")]
    public class EMCLASSIFICATIONMARK
    {
        [Key]
        [DataMember]
        public string CID
        { get; set; }

        //[DataMember]
        //public int ID
        //{ get; set; }

        [DataMember]
        public string NAME
        { get; set; }

        [DataMember]
        public int MARK
        { get; set; }

    }

    public class ComboxModel
    {
        public string ID
        { get; set; }

        public string NAME
        { get; set; }
    }
}
