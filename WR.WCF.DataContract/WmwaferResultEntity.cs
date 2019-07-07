using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WmwaferResultEntity实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    public class WmwaferResultEntity
    {
        [DataMember]
        public int Id
        { get; set; }

        [DataMember]
        public string RESULTID
        { get; set; }

        [DataMember]
        public string DEVICE
        { get; set; }

        [DataMember]
        public string LAYER
        { get; set; }

        [DataMember]
        public string LOT
        { get; set; }

        [DataMember]
        public string SUBSTRATE_SLOT
        { get; set; }

        [DataMember]
        public string SUBSTRATE_ID
        { get; set; }

        [DataMember]
        public decimal? SFIELD
        { get; set; }

        [DataMember]
        public Int64? NUMDEFECT
        { get; set; }

        [DataMember]
        public string ISCHECKED
        { get; set; }

        [DataMember]
        public string COMPUTERNAME
        { get; set; }

        [DataMember]
        public Int64? COMPLETIONTIME
        { get; set; }

        [DataMember]
        public Int64? CHECKEDDATE
        { get; set; }

        [DataMember]
        public Int64? CREATEDDATE
        { get; set; }

        [DataMember]
        public string FILETYPE
        { get; set; }

        [DataMember]
        public string DISPOSITION
        { get; set; }

        [DataMember]
        public double? DEFECTDENSITY
        { get; set; }

        [DataMember]
        public string IDENTIFICATIONID
        { get; set; }

        [DataMember]
        public Int64? LOTCOMPLETIONTIME
        { get; set; }

        [DataMember]
        public string CLASSIFICATIONINFOID
        { get; set; }

        [DataMember]
        public string DIELAYOUTID
        { get; set; }

        [DataMember]
        public string SUBSTRATE_NOTCHLOCATION
        { get; set; }

        [DataMember]
        public string RECIPE_ID
        { get; set; }

        public decimal? LFIELD
        { get; set; }

        [DataMember]
        public string ISREVIEW
        { get; set; }

        [DataMember]
        public decimal? MASKA_DIE
        { get; set; }
        [DataMember]
        public decimal? MASKB_DIE
        { get; set; }
        [DataMember]
        public decimal? MASKC_DIE
        { get; set; }
        [DataMember]
        public decimal? MASKD_DIE
        { get; set; }
        [DataMember]
        public decimal? MASKE_DIE
        { get; set; }
        [DataMember]
        public decimal? MASKNULL_DIE
        { get; set; }
    }
}
