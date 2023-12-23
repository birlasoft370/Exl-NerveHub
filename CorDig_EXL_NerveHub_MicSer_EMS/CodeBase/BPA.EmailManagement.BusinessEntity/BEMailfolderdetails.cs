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
    public class BEMailfolderdetails : ObjectBase
    {
        [DataMember]
        public int MailFolderDetailID { get; set; }

        [DataMember]
        public string sMailFolderID { get; set; }

        [DataMember]
        public string sMailFolderName { get; set; }

        [DataMember]
        public string sMailFolderPath { get; set; }

        [DataMember]
        public bool bIngestion { get; set; }

        [DataMember]
        public bool bMoveFolder { get; set; }

        [DataMember]
        public bool bSearchFolder { get; set; }

        [DataMember]
        public bool bDisabled { get; set; }
    }
}
