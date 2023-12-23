using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Serializable]
    public class BEFormulaData : ObjectBase
    {
        [DataMember]
        public int iObjectId { get; set; }

        [DataMember]
        public string ObjectEevent { get; set; }

        [DataMember]
        public int iCampaignId { get; set; }

    }
}
