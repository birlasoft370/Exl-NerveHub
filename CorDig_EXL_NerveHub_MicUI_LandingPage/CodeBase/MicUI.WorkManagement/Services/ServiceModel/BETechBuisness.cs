using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Serializable]
    [DataContract]
    public class BETechBuisness : ObjectBase
    {
        [DataMember]
        public int iTechUserID { get; set; }

        [DataMember]
        public int iBusinessUserID { get; set; }

        [DataMember]
        public string sTechTechName { get; set; }

        [DataMember]
        public string sBusinessUserName { get; set; }
    }
}
