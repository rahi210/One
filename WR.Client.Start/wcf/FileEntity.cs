using System;
using System.Runtime.Serialization;

namespace WR.WCF.Client.DataContract
{
    [DataContract]
    public class FileEntity
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public long FileLength { get; set; }
        [DataMember]
        public string MapPath { get; set; }
        [DataMember]
        public DateTime LastTime { get; set; }
    }
}
