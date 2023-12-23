using System.Runtime.Serialization;

namespace MicUI.NerveHub.Models
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