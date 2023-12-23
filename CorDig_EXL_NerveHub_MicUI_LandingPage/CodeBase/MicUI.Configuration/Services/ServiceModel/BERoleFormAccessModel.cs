using System.Runtime.Serialization;
namespace MicUI.Configuration.Services.ServiceModel
{

    [Serializable]
    [DataContract]
    public class BERoleFormAccessModel : ObjectBase
    {
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public string FormId { get; set; }
        [DataMember]
        public string FormName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string View { get; set; }
        [DataMember]
        public string Modify { get; set; }
        [DataMember]
        public string Delete { get; set; }
        [DataMember]
        public string Approve { get; set; }
        [DataMember]
        public string Add { get; set; }
    }
}
