using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/********************************************
 * EM_DEFECTLIST实体类
 * 
 * *****************************************/

namespace WR.WCF.DataContract
{
    [DataContract]
    [Table("EM_DEFECTLIST")]
    public class EMDEFECTLIST
    {
        
        [DataMember]
        public int ID
        { get;set; }

        [DataMember]
        public int PASSID
        { get;set; }

        [DataMember]
        public string INSPID
        { get;set; }

        [DataMember]
        public string INSPECTIONTYPE
        { get;set; }

        [DataMember]
        public string SWCSCOORDINATES
        { get;set; }

        [DataMember]
        public string INSPCLASSIFIID
        { get;set; }

        [DataMember]
        public string SIZE_
        { get;set; }

        [DataMember]
        public string MAJORAXISSIZE
        { get;set; }

        [DataMember]
        public string MAJORMINORAXISASPECTRATIO
        { get;set; }

        [DataMember]
        public string AREA_
        { get;set; }

        [DataMember]
        public string DIEADDRESS
        { get;set; }

        [DataMember]
        public string IMAGENAME
        { get;set; }

        [DataMember]
        public string STYLE
        { get;set; }

        [DataMember]
        public string PIXELSIZE
        { get;set; }

        [DataMember]
        public string ISCHECKED
        { get;set; }

        [DataMember]
        public Int64 CHECKEDDATE
        { get;set; }

        [DataMember]
        public string CHECKEDBY
        { get;set; }

        [DataMember]
        public string MODIFIEDDEFECT
        { get;set; }

        [DataMember]
        public string RESULTID
        { get;set; }

    }
}
