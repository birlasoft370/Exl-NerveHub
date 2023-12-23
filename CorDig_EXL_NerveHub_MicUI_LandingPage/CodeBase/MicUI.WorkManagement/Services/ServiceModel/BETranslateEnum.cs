using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
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
