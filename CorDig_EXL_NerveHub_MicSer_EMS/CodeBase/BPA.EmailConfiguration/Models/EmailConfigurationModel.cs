using System.Text.Json.Serialization;

namespace BPA.EmailConfiguration
{
    public class TestConnectionModel
    {
        public int MailServerTypeID { get; set; }
        public string? Instance { get; set; }
        public string? TenentID { get; set; }
        public string? ClientID { get; set; }
        public string? Scope { get; set; }
        public string? AutoDiscoveryPath { get; set; }
        public string? RedirectUrl { get; set; }
        public string? UserID { get; set; }
        [JsonPropertyName("IsClientService")]
        public bool EMSWebServerHostingAtClient { get; set; }
        public string? ParentFolderId { get; set; }
        public string? ParentFolderName { get; set; }
    }

    public class EmailConfigurationModel : TestConnectionModel
    {
        [JsonPropertyName("WorkObjNameId")]
        public int StoreID { get; set; }
        public int MailConfigID { get; set; } = 0;

        [JsonPropertyName("MailTemplateId")]
        public int AutoReplyTemplate { get; set; }
        public int CampaignID { get; set; }
        public string? MailBoxName { get; set; }
        public string? EmailID { get; set; }
        public string? Password { get; set; }
        public bool UseServiceCredentialToPull { get; set; }
        public bool UseUserCredentialToSend { get; set; }
        public int ScheduleInterval { get; set; }
        public string? LotusServerPath { get; set; }
        public string? NFSFilePath { get; set; }
        public bool WebEnabled { get; set; }
        public string? EMSWebServerURL { get; set; }
        public bool isPasswordExpire { get; set; } = false;
        public bool AutoReply { get; set; }
        public bool PoolingValue { get; set; } = false;
        public bool IsReadMail { get; set; }
        public int FolderType { get; set; }
        //  public int CreatedBy { get; set; }
        public bool Disabled { get; set; }
        //public int User_XML { get; set; }
        public string? LotusDomainName { get; set; }
        public string? LotusDomainPrefix { get; set; }
        public bool OutofOffice { get; set; }
        public string? OutofOfficeText { get; set; }
        public bool Impersonation { get; set; }
        public string? ImpersonationIDType { get; set; }
        public string? ImpersonationID { get; set; }
        public bool OutLook { get; set; }
        public bool TranslationEnabled { get; set; }
        public bool IsForSWMIntegration { get; set; }
        public bool IsSWMEMSIntegration { get; set; }

        // [JsonPropertyName("IsClientService")]
        //public bool ServiceType { get; set; }
        public List<GridList> GridList { get; set; } = new List<GridList>();
       
    }

    [Serializable]
    public class GridList
    {
        public int MailFolderDetailID { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool Search { get; set; }
        public bool MoveFolder { get; set; }
        public bool Ingestion { get; set; }
        public bool Disable { get; set; }
        public string? MailFolderPath { get; set; }
    }

    [Serializable]
    public class TreeStructure
    {
        public string? parentId { get; set; }
        public string? text { get; set; }
        public string? id { get; set; }
        public ICollection<TreeStructure> items { get; set; } = new HashSet<TreeStructure>();

    }
}
