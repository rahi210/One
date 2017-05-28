using System;
using System.Runtime.Serialization;
/********************************************
 * WmdensityReport实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class WmdensityReport
    {
        [DataMember]
        public string LOT
        { get; set; }

        [DataMember]
        public string DEVICE
        { get; set; }

        [DataMember]
        public string LAYER
        { get; set; }

        [DataMember]
        public string SUBSTRATE_ID
        { get; set; }

        [DataMember]
        public string SUBSTRATE_SLOT
        { get; set; }

        [DataMember]
        public string DIEADDRESS
        { get; set; }

        [DataMember]
        public int? ID
        { get; set; }

        [DataMember]
        public string DESCRIPTION
        { get; set; }

        [DataMember]
        public int CNT
        { get; set; }

        [DataMember]
        public int WAFERCNT
        { get; set; }

        [DataMember]
        public Double? DEFECTDENSITY
        { get; set; }

        [DataMember]
        public Decimal? COMPLETIONTIME
        { get; set; }
    }
}
