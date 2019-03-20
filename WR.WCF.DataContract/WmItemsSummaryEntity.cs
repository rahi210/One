using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WmwaferInfoEntity实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class WmItemsSummaryEntity
    {
        [DataMember]
        public string Device
        { get; set; }

        [DataMember]
        public string Layer
        { get; set; }

        [DataMember]
        public string Lot
        { get; set; }

        [DataMember]
        public string Inspclassifiid
        { get; set; }

        [DataMember]
        public string Substrate_id
        { get; set; }

        [DataMember]
        public Int64? NumCnt
        { get; set; }

        [DataMember]
        public string RESULTID
        { get; set; }

        [DataMember]
        public string MASTERTOOLNAME
        { get; set; }

        [DataMember]
        public string RECIPE_ID
        { get; set; }

        [DataMember]
        public decimal SFIELD
        { get; set; }

        [DataMember]
        public decimal LFIELD
        { get; set; }
    }
}
