using System;
using System.Runtime.Serialization;
/********************************************
 * WmCategoryReport实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class WmCategoryReport
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
        public int? ID
        { get; set; }

        [DataMember]
        public string DESCRIPTION
        { get; set; }

        [DataMember]
        public int DefectNum
        { get; set; }

        [DataMember]
        public Decimal? COMPLETIONTIME
        { get; set; }
    }
}
