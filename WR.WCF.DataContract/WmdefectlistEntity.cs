using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WmdefectlistEntity实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class WmdefectlistEntity
    {
        [DataMember]
        public int? Id
        { get; set; }

        [DataMember]
        public int? PASSID
        { get; set; }

        [DataMember]
        public string INSPID
        { get; set; }

        [DataMember]
        public string InspectedSurface
        { get; set; }

        [DataMember]
        public string ModifiedDefect
        { get; set; }

        [DataMember]
        public string InspclassifiId
        { get; set; }

        [DataMember]
        public string ADC
        { get; set; }

        [DataMember]
        public string Size_
        { get; set; }

        [DataMember]
        public string Area_
        { get; set; }

        [DataMember]
        public string DieAddress
        { get; set; }

        [DataMember]
        public string ImageName
        { get; set; }

        [DataMember]
        public string Ischecked
        { get; set; }

        [DataMember]
        public Int64? CheckedDate
        { get; set; }

        [DataMember]
        public string Description
        { get; set; }

        [DataMember]
        public string Schemeid
        { get; set; }

        [DataMember]
        public int? Cclassid
        { get; set; }

        [DataMember]
        public string Color
        { get; set; }
    }
}
