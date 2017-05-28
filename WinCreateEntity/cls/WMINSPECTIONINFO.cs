using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * WM_INSPECTIONINFO实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("WM_INSPECTIONINFO")]
    public class WMINSPECTIONINFO
    {
        [DataMember]
        public string INSPID
        { get;set; }

        [DataMember]
        public string INSPID
        { get;set; }

        [DataMember]
        public string RESULTID
        { get;set; }

        [DataMember]
        public string RESULTID
        { get;set; }

        [DataMember]
        public string MODULENAME
        { get;set; }

        [DataMember]
        public string MODULENAME
        { get;set; }

        [DataMember]
        public Int64 STARTTIME
        { get;set; }

        [DataMember]
        public Int64 STARTTIME
        { get;set; }

        [DataMember]
        public Int64 COMPLETIONTIME
        { get;set; }

        [DataMember]
        public Int64 COMPLETIONTIME
        { get;set; }

        [DataMember]
        public string DEVICE
        { get;set; }

        [DataMember]
        public string DEVICE
        { get;set; }

        [DataMember]
        public string LAYER
        { get;set; }

        [DataMember]
        public string LAYER
        { get;set; }

        [DataMember]
        public string NAME
        { get;set; }

        [DataMember]
        public string NAME
        { get;set; }

        [DataMember]
        public Int64 MODIFICATIONTIME
        { get;set; }

        [DataMember]
        public Int64 MODIFICATIONTIME
        { get;set; }

        [DataMember]
        public string LASTAUTHOR
        { get;set; }

        [DataMember]
        public string LASTAUTHOR
        { get;set; }

        [DataMember]
        public string DISPOSITION
        { get;set; }

        [DataMember]
        public string DISPOSITION
        { get;set; }

        [DataMember]
        public string CLASSIFICATIONSCHEMEID
        { get;set; }

        [DataMember]
        public string CLASSIFICATIONSCHEMEID
        { get;set; }

        [DataMember]
        public decimal DEFECTDENSITY
        { get;set; }

        [DataMember]
        public decimal DEFECTDENSITY
        { get;set; }

        [DataMember]
        public decimal RANDOMDEFECTDENSITY
        { get;set; }

        [DataMember]
        public decimal RANDOMDEFECTDENSITY
        { get;set; }

        [DataMember]
        public decimal DEFECTRATIO
        { get;set; }

        [DataMember]
        public decimal DEFECTRATIO
        { get;set; }

        [DataMember]
        public decimal DEFECTIVEAREA
        { get;set; }

        [DataMember]
        public decimal DEFECTIVEAREA
        { get;set; }

        [DataMember]
        public int DEFECTIVEDIE
        { get;set; }

        [DataMember]
        public int DEFECTIVEDIE
        { get;set; }

        [DataMember]
        public string IMAGESDIRECTORYNAME
        { get;set; }

        [DataMember]
        public string IMAGESDIRECTORYNAME
        { get;set; }

    }
}
