using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Serializable]
    [DataContract]
    public class BELocalization
    {
        [DataMember]
        public string CultureInfo { get; set; }
        [DataMember]
        public string TimeZone { get; set; }

    }
}