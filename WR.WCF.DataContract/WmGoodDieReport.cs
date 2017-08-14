using System;
using System.Runtime.Serialization;
/********************************************
 * WmGoodDieReport实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class WmGoodDieReport
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
        public Decimal? COMPLETIONTIME
        { get; set; }

        [DataMember]
        public DateTime? INSPECTIONDATE
        { get; set; }

        [DataMember]
        public int? DEFECTNUM
        { get; set; }

        [DataMember]
        public int? INSPCNT
        { get; set; }

        [DataMember]
        public int? GOODCNT
        { get; set; }

        [DataMember]
        public double? PERCENT
        { get; set; }

        [DataMember]
        public string RESULTID
        { get; set; }
    }
}
