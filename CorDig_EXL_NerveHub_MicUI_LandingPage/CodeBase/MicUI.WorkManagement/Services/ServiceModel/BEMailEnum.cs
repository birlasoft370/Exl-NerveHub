using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Flags]
    public enum BatchFrequencyType
    {
        [EnumMember]
        Daily = 1,
        [EnumMember]
        Weekly = 2,
        [EnumMember]
        Fortnightly = 3,
        [EnumMember]
        Monthly = 4,
        [EnumMember]
        Quarterly = 5,
        [EnumMember]
        HalfYearly = 6,
        [EnumMember]
        Yearly = 7,
    }
}
