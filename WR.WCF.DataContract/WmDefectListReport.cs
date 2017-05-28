using System;
using System.Runtime.Serialization;
/********************************************
 * WmDefectListReport实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class WmDefectListReport
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
        public string CodeDESCRIPTION
        { get; set; }

        [DataMember]
        public string SIZE_
        { get; set; }

        [DataMember]
        public string XSIZE_
        { get; set; }

        [DataMember]
        public string YSIZE_
        { get; set; }

        [DataMember]
        public string AREA_
        { get; set; }

        [DataMember]
        public Decimal? COMPLETIONTIME
        { get; set; }

        [DataMember]
        public int? DEFECTNUMBER
        { get; set; }
    }
}
