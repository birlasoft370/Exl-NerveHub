using System.Runtime.Serialization;

namespace BPA.EmailManagement.BusinessEntity.ExternalRef.Security
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
