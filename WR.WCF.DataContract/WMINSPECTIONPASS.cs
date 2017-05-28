using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WM_INSPECTIONPASS实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("WM_INSPECTIONPASS")]
    public class WMINSPECTIONPASS
    {
        [Key]
        [DataMember]
        public int PASSID
        { get; set; }

        [DataMember]
        public string INSPID
        { get; set; }

        [DataMember]
        public string ORIENTATION
        { get; set; }

        [DataMember]
        public string INSPECTEDSURFACE
        { get; set; }

        [DataMember]
        public string INSPECTIONTYPE
        { get; set; }

        [DataMember]
        public decimal DEFECTDENSITY
        { get; set; }

        [DataMember]
        public decimal DEFECTIVEAREA
        { get; set; }

        [DataMember]
        public decimal DEFECTRATIO
        { get; set; }

        [DataMember]
        public decimal RANDOMDEFECTDENSITY
        { get; set; }

        [DataMember]
        public int DEFECTIVEDIE
        { get; set; }
    }
}
