using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef
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
