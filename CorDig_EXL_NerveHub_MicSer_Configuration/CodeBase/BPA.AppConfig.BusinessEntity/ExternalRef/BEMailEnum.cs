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
    public enum SearchOptions
    {
        [EnumMember]
        Subject = 0,
        [EnumMember]
        Body = 1,
        [EnumMember]
        OnDate = 2,
        [EnumMember]
        BeforeDate = 3,
        [EnumMember]
        SinceDate = 4
    }
    [DataContract]
    [Flags]
    public enum ReplyType
    {
        [EnumMember]
        NoAction = 0,
        [EnumMember]
        Reply = 1,
        [EnumMember]
        ReplyAll = 2,
        [EnumMember]
        Forward = 3,
        [EnumMember]
        New = 4,
        [EnumMember]
        MailException = 5,
        [EnumMember]
        PermissionDenied = 6,
        [EnumMember]
        MailNotFound = 7
    }
    [DataContract]
    [Flags]
    public enum emailType
    {
        [EnumMember]
        Primary = 1,
        [EnumMember]
        SharedMailbox = 2,
        [EnumMember]
        PublicFolder = 3
    }

    [DataContract]
    [Flags]
    public enum ImpersonationIDType
    {
        [EnumMember]
        PrincipalName = 1,
        [EnumMember]
        SID = 2,
        [EnumMember]
        SmtpAddress = 3
    }

    [DataContract]
    [Flags]
    public enum EmailServerType
    {
        [EnumMember]
        Exchange2007SP1 = 1,
        [EnumMember]
        Exchange2010SP1 = 2,
        [EnumMember]
        Exchange2010SP2 = 3,
        [EnumMember]
        Office365 = 4,
        [EnumMember]
        MicrosoftGraph = 5,
        [EnumMember]
        Dominos = 0
    }

    [DataContract]
    [Flags]
    public enum AttachementType
    {
        [EnumMember]
        FileAttachment = 1,
        [EnumMember]
        ItemAttachment = 2,

    }

    [DataContract]
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
