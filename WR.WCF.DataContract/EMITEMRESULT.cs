using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * EM_ITEMRESULT实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("EM_ITEMRESULT")]
    public class EMITEMRESULT
    {
        [DataMember]
        public int ID
        { get;set; }

        [DataMember]
        public string EXAMRESULTID
        { get;set; }

        [DataMember]
        public int PASSID
        { get;set; }

        [DataMember]
        public string INSPID
        { get;set; }

        [DataMember]
        public string RESULTID
        { get;set; }

        [DataMember]
        public string MODIFIEDDEFECT
        { get;set; }

        [DataMember]
        public string STATUS
        { get;set; }

    }
}
