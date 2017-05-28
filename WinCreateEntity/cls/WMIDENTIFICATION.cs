using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WM_IDENTIFICATION实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("WM_IDENTIFICATION")]
    public class WMIDENTIFICATION
    {
        [DataMember]
        public string IDENTIFICATIONID
        { get;set; }

        [DataMember]
        public string LOT
        { get;set; }

        [DataMember]
        public string OPERATOR
        { get;set; }

        [DataMember]
        public string CARRIER_ID
        { get;set; }

        [DataMember]
        public string CARRIER_STATION
        { get;set; }

        [DataMember]
        public string DEVICE
        { get;set; }

        [DataMember]
        public string LAYER
        { get;set; }

        [DataMember]
        public string PPNAME
        { get;set; }

        [DataMember]
        public Int64 MODIFICATIONTIME
        { get;set; }

        [DataMember]
        public string LASTAUTHOR
        { get;set; }

        [DataMember]
        public string SUBSTRATE_ID
        { get;set; }

        [DataMember]
        public string SUBSTRATE_NUMBER
        { get;set; }

        [DataMember]
        public string SUBSTRATE_DIAMETERMM
        { get;set; }

        [DataMember]
        public string SUBSTRATE_SLOT
        { get;set; }

        [DataMember]
        public string SUBSTRATE_TYPE
        { get;set; }

        [DataMember]
        public string SUBSTRATE_FIDUCIALTYPE
        { get;set; }

    }
}
