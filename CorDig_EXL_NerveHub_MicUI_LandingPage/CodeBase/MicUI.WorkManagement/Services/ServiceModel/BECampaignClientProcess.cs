using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
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
