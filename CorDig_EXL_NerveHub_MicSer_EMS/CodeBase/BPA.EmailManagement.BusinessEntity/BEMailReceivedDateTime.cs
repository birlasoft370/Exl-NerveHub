using BPA.EmailManagement.BusinessEntity.ExternalRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessEntity
{
    [Serializable]
    [DataContract]
    public class BEMailReceivedDateTime : ObjectBase
    {
        [DataMember]
        public int iDSOBJChoiceID { get; set; }
        [DataMember]
        public string sChoiceValue { get; set; }
        [DataMember]
        public string sDSOBJName { get; set; }
        [DataMember]
        public int iMailFolderDetailID { get; set; }
        [DataMember]
        public DateTime dReceivedDateTime { get; set; }
    }
}
