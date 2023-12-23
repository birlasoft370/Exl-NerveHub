using BPA.GlobalResources.UI;
using BPA.GlobalResources.UI.EmailManagement;
using MicUI.ConfiEmailManagementguration.Helper.CustomValidationAttributes;
using MicUI.EmailManagement.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MicUI.EmailManagement.Models.ViewModels
{
    [Serializable]
    public class MailConfigurationViewModel : SearchViewModel
    {

        public MailConfigurationViewModel()
            : base(new string[] { Resources_common.display_Client, Resources_common.display_Process, Resources_common.display_Campaign_Dropdown, Resources_common.display_WorkObjName })
        {
        }
        //[ScaffoldColumn(false)]


        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Client")]
        public string ClientName { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_Process")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Process")]
        public string ProcessName { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_Campaign")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Campaign")]
        public string CampaignName { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailConfiguration_Resources), ErrorMessageResourceName = "required_WorkDefinitionName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_StoreWorkDefinitionName")]
        public string WorkDefinitionName { get; set; }

        // public  IList<GridItem> GridItems = new List<GridItem>();
        public IList<BEMailConfiguration> GridItems = new List<BEMailConfiguration>();

        public int MailConfigID { get; set; }  //1
        public int CampaignID { get; set; }   //2
        public int StoreID { get; set; }
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Client")]
        public string MailBoxName { get; set; }   //3       //(ConfigurationName)

        public string LotusDomainName { get; set; }

        public string LotusDomainPrefix { get; set; }

        public string EmailID { get; set; }    //4

        public bool UseServiceCredentialToPull { get; set; } //5

        public bool UseUserCredentialToSend { get; set; } //6

        public string UserID { get; set; }        //7

        public string Password { get; set; }     //8

        public string MailServerTypeID { get; set; }  //9  (ConnectionType)

        public bool ServiceType { get; set; }

        public string ScheduleInterval { get; set; }     //

        public int AutoReplyTemplate { get; set; }

        public bool AutoReplyEnableDisable { get; set; }

        public string AutoDiscoveryPath { get; set; }

        public string LotusServerPath { get; set; }

        public string NFSFilePath { get; set; }

        public bool WebEnabled { get; set; }

        public string WebServerURL { get; set; }

        public bool IsReadMail { get; set; }

        public int FolderType { get; set; }

        public int MailTemplateID { get; set; }

        public bool PoolingValue { get; set; }

        public bool? Disable { get; set; }

        public bool bSendmailquiqueidentified { get; set; }

        public bool bScheduletosameuser { get; set; }

        public bool bInlineEditing { get; set; }
        public bool bNeedeFile { get; set; }

        public bool bNeedPrint { get; set; }

        public bool bOutLookMailEnabled { get; set; }

        public bool bNeedTicket { get; set; }

        public bool bSubmitDisplay { get; set; }

        public string sEfilePath { get; set; }

        public string sSubmitDisplayMsg { get; set; }

        public bool bReadMailBody { get; set; }

        public bool bCFX { get; set; }

        public bool bDuringUpload { get; set; }

        public string AssignType { get; set; }

        public bool bAssignLast { get; set; }

        public int iUploadBy { get; set; }

        public bool bSensitivity { get; set; }

        public string sCEXlauncherPath { get; set; }

        public int iNeedTicketLenth { get; set; }

        public string sTicketName { get; set; }

        public bool bFreshRequired { get; set; }

        [Display(ResourceType = typeof(MailConfiguration_Resources), Name = "display_OutofOffice")]
        public bool bOutofOfficeEnabled { get; set; }

        [Display(ResourceType = typeof(MailConfiguration_Resources), Name = "display_BCCEnabled")]
        public bool bBCCEnabled { get; set; }

        public bool bOutLookEnabled { get; set; }

        public bool bTranslationEnabled { get; set; }
        public string sOutofOffice { get; set; }

        [Display(ResourceType = typeof(MailConfiguration_Resources), Name = "display_Impersonation")]
        public bool bImpersonation { get; set; }

        [Display(ResourceType = typeof(MailConfiguration_Resources), Name = "display_ImpersonationID")]
        public string sImpersonationID { get; set; }

        [Display(ResourceType = typeof(MailConfiguration_Resources), Name = "display_ImpersonationIDType")]
        public string sImpersonationIDType { get; set; }

        public string FolderID { get; set; }

        public string ParentFolderName { get; set; }
        // public bool IsAdvanceDisable { get; set; }

        public DataTable dtMailConfigDetails = new DataTable();

        public IList<GridList> GridFolderList = new List<GridList>();

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailConfiguration_Resources), Name = "display_BatchFrequency")]
        public string BatchFrequencyType { get; set; }


        [RequiredField(ErrorMessageResourceType = typeof(MailConfiguration_Resources), ErrorMessageResourceName = "required_TimeZone")]
        [Display(ResourceType = typeof(MailConfiguration_Resources), Name = "display_TimeZone")]
        public int iTimeZoneID { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailConfiguration_Resources), Name = "display_ClientID")]
        public string GClinetID { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailConfiguration_Resources), Name = "display_TenentID")]
        public string TenentID { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailConfiguration_Resources), Name = "display_Scope")]
        public string Scope { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailConfiguration_Resources), Name = "display_RedirectUrl")]
        public string RedirectUrl { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailConfiguration_Resources), Name = "display_SWMIntegration")]
        public bool IsForSWMIntegration { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailConfiguration_Resources), Name = "display_SWMEMSIntegration")]
        public bool IsSWMEMSIntegration { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailConfiguration_Resources), Name = "display_Instance")]
        public string Instance { get; set; }
    }

    [Serializable]
    public class GridItem
    {
        public GridItem()
        {

        }

        public int MailConfigID { get; set; }
        public string Name { get; set; }
        public int Interval { get; set; }
        public bool? Disable { get; set; }
    }

    [Serializable]
    public class TreeStructure
    {

        public TreeStructure()
        {
            items = new HashSet<TreeStructure>();
        }
        public string parentId { get; set; }
        public string text { get; set; }
        public string id { get; set; }
        public ICollection<TreeStructure> items { get; set; }

    }

    [Serializable]
    public class GridList
    {
        public int MailFolderDetailID { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Search { get; set; }
        public bool MoveFolder { get; set; }
        public bool Ingestion { get; set; }
        public bool Disable { get; set; }
        public string MailFolderPath { get; set; }
    }
}
