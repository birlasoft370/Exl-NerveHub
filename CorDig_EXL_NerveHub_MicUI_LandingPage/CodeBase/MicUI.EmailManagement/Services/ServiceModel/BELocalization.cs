using System.Runtime.Serialization;

namespace MicUI.EmailManagement.Services.ServiceModel
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