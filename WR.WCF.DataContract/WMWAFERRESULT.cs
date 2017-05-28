using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WM_WAFERRESULT实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("WM_WAFERRESULT")]
    public class WMWAFERRESULT
    {
        [Key]
        [DataMember]
        public string RESULTID
        { get; set; }

        [DataMember]
        public Int64 STARTTIME
        { get; set; }

        [DataMember]
        public Int64 COMPLETIONTIME
        { get; set; }

        [DataMember]
        public Int64 REVIEWSTARTTIME
        { get; set; }

        [DataMember]
        public Int64 REVIEWCOMPLETIONTIME
        { get; set; }

        [DataMember]
        public Int64 LOTSTARTTIME
        { get; set; }

        [DataMember]
        public Int64 LOTCOMPLETIONTIME
        { get; set; }

        [DataMember]
        public string DISPOSITION
        { get; set; }

        [DataMember]
        public string MASTERTOOLNAME
        { get; set; }

        [DataMember]
        public string MASTERSOFTWAREVERSION
        { get; set; }

        [DataMember]
        public string MASTERTOOLCOMPUTERNAME
        { get; set; }

        [DataMember]
        public string MASTERTOOLTOHOSTLINKSTATE
        { get; set; }

        [DataMember]
        public string MODULENAME
        { get; set; }

        [DataMember]
        public string COMPUTERNAME
        { get; set; }

        [DataMember]
        public string SOFTWAREVERSION
        { get; set; }

        [DataMember]
        public string PRIMARYSURFACE
        { get; set; }

        [DataMember]
        public string IDENTIFICATIONID
        { get; set; }

        [DataMember]
        public string CLASSIFICATIONINFOID
        { get; set; }

        [DataMember]
        public string DIELAYOUTID
        { get; set; }

        [DataMember]
        public string ISCHECKED
        { get; set; }

        [DataMember]
        public Int64 CHECKEDDATE
        { get; set; }

        [DataMember]
        public string CHECKEDBY
        { get; set; }

        [DataMember]
        public Int64 CREATEDDATE
        { get; set; }

        [NotMapped]
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
        public string DELFLAG
        { get; set; }

        [DataMember]
        public string DELBY
        { get; set; }
    }
}
