using System;
using System.Runtime.Serialization;
/********************************************
 * WmDefectiveDieReport实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class WmInpDieReport
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
        public int? DEFECTNUM
        { get; set; }

        [DataMember]
        public int? DIEQTY
        { get; set; }

        [DataMember]
        public decimal? Yield
        { get; set; }

        [DataMember]
        public Decimal? COMPLETIONTIME
        { get; set; }
    }
}
