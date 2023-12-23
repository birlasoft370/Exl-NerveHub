using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.Configuration
{
    [Serializable]
    [DataContract]
    public class BECampaignClientProcess : ObjectBase
    {




        [DataMember]
        public int ClientID
        {
            get; set;
        }
        [DataMember]
        public int ProcessID
        {
            get;
            set;
        }
        [DataMember]
        public int CampaignID
        {
            get;
            set;
        }

    }
}
