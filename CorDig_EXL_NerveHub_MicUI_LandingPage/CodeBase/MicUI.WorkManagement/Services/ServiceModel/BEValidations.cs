using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Serializable]
    public class BEValidations : ObjectBase
    {
        [DataMember]
        public int ValidationId { get; set; }
        [DataMember]
        public string ValidationType { get; set; }
    }
}
