using System.Runtime.Serialization;

namespace MicUI.EmailManagement.Services.ServiceModel
{
    [DataContract]
    [Flags]
    public enum LanguageProvider
    {
        [EnumMember]
        Default = 0,
        [EnumMember]
        SysTran = 1,
        [EnumMember]
        MicroSoft = 2,
        [EnumMember]
        Google = 3
    }
}
