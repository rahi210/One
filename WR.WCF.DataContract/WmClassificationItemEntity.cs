using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WmClassificationItemEntity实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class WmClassificationItemEntity
    {
        [DataMember]
        public string ITEMID
        { get; set; }

        [DataMember]
        public string SCHEMEID
        { get; set; }

        [DataMember]
        public int? ID
        { get; set; }

        [DataMember]
        public string NAME
        { get; set; }

        [DataMember]
        public string DESCRIPTION
        { get; set; }

        [DataMember]
        public int Points
        { get; set; }

        [DataMember]
        public string InspectionType
        { get; set; }
    }
}
