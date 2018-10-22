using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("WM_YIELDSETTING")]
    public class WMYIELDSETTING
    {
        [Key]
        [DataMember]
        public string RECIPE_ID
        { get; set; }

        [DataMember]
        public decimal LOT_YIELD
        { get; set; }

        [DataMember]
        public decimal WAFER_YIELD
        { get; set; }

        [DataMember]
        public decimal MASKA_YIELD
        { get; set; }

        [DataMember]
        public decimal MASKB_YIELD
        { get; set; }

        [DataMember]
        public decimal MASKC_YIELD
        { get; set; }

        [DataMember]
        public decimal MASKD_YIELD
        { get; set; }

        [DataMember]
        public decimal MASKE_YIELD
        { get; set; }

        [DataMember]
        public string YIELD_TYPE { get; set; }

        [DataMember]
        public string IMAGE_NAME { get; set; }
    }
}
